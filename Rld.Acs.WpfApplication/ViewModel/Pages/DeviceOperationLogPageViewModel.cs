﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using log4net;
using MahApps.Metro.Controls.Dialogs;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.DeviceProxy;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class DeviceOperationLogPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceOperationLogRepository _deviceOperationLogRepo = NinjectBinder.GetRepository<IDeviceOperationLogRepository>();


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String DeviceCode { get; set; }
        public String UserCode { get; set; }
        public String OperatorId { get; set; }
        public ObservableCollection<DeviceOperationLogViewModel> DeviceOperationLogViewModels { get; set; }

        public RelayCommand QueryCommand { get; set; }
        public RelayCommand ExportCommand { get; set; }

        private async void QueryCommandFunc()
        {
            var pageIndex = 1;
            ProcessQueryPage(pageIndex);
        }

        #region 分页相关属性

        /// <summary>
        /// 分页查询命令
        /// </summary>
        private async void NextPageSearchCommandFunc()
        {
            var pageIndex = Convert.ToInt32(CurrentPage);
            ProcessQueryPage(pageIndex);
        }

        private string _totalPage = string.Empty;


        /// <summary>
        /// 总页数
        /// </summary>
        public string TotalPage
        {
            get { return _totalPage; }
            set
            {
                _totalPage = value;
                RaisePropertyChanged();
            }
        }
        private string _navigationPage = string.Empty;

        public string NavigationPage
        {
            get { return _navigationPage; }
            set
            {
                _navigationPage = value;
                RaisePropertyChanged();
            }
        }

        private string _currentPage = "1";
        /// <summary>
        /// 当前页
        /// </summary>
        public string CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                RaisePropertyChanged();
            }
        }

        private int _pageSize = 30;
        /// <summary>
        /// 每页显示的记录数
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                RaisePropertyChanged();
            }
        }
        private int _pageIndex;
        private int _totalCount;
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                _pageIndex = value;

                RaisePropertyChanged();
            }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;

                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 分页管理
        /// </summary>
        public RelayCommand NextPageSearchCommand { get; set; }

        #endregion

        public DeviceOperationLogPageViewModel()
        {
            QueryCommand = new AuthCommand(QueryCommandFunc);
            ExportCommand = new AuthCommand(ExportCommandFunc);
            NextPageSearchCommand = new AuthCommand(NextPageSearchCommandFunc);

            DeviceOperationLogViewModels = new ObservableCollection<DeviceOperationLogViewModel>();

            StartDate = DateTime.Now.AddDays(-7);
            EndDate = DateTime.Now;
        }

        private void ProcessQueryPage(int pageIndex)
        {
            try
            {
                var conditions = new Hashtable();
                if (!TryGetConditions(pageIndex, PageSize, out conditions))
                {
                    return;
                }

                DispatcherHelper.CheckBeginInvokeOnUI(async () =>
                {
                    string message = "";

                    var controller = await DialogCoordinator.Instance.ShowProgressAsync(this, "查询数据", "查询数据中，请稍等");
                    controller.SetIndeterminate();

                    await Task.Run(() =>
                    {
                        try
                        {
                            Log.Info("同步数据中..");
                            var devices = new List<DeviceController>();
                            if (!string.IsNullOrWhiteSpace(DeviceCode))
                            {
                                var queriedDevice = ApplicationManager.GetInstance().AuthorizationDevices.First(x => x.Code == DeviceCode);
                                devices.Add(queriedDevice);
                            }
                            else
                            {
                                devices = ApplicationManager.GetInstance().AuthorizationDevices;
                            }

                            string[] messages;
                            var resultTypes = new DeviceServiceClient().SyncDeviceOperationLogs(devices.ToArray(), out messages);
                            message = MessageHandler.GenerateDeviceMessage(resultTypes, messages, "同步数据成功！", "同步数据失败！");
                            Log.Info(message);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }

                        try
                        {
                            Log.Info("查询数据中..");
                            int totalCount = 0;
                            DeviceOperationLogViewModels = QueryData(conditions, out totalCount);
                            if (totalCount % PageSize == 0)
                            {
                                TotalPage = (totalCount / PageSize).ToString();
                            }
                            else
                            {
                                TotalPage = ((totalCount / PageSize) + 1).ToString();
                            }
                            RaisePropertyChanged(null);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }
                    });

                    await controller.CloseAsync();

                    if (!DeviceOperationLogViewModels.Any())
                    {
                        Messenger.Default.Send(new NotificationMessage("查询数据结果为空"), Tokens.DeviceOperationLogPage_ShowNotification);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private ObservableCollection<DeviceOperationLogViewModel> QueryData(Hashtable conditions, out int totalCount)
        {
            var paninationResult = _deviceOperationLogRepo.QueryPage(conditions);
            totalCount = paninationResult.TotalCount;
            var logVM = paninationResult.Entities.Select(AutoMapper.Mapper.Map<DeviceOperationLogViewModel>);

            DeviceOperationLogViewModels = new ObservableCollection<DeviceOperationLogViewModel>(logVM);
            return DeviceOperationLogViewModels;
        }

        private bool TryGetConditions(int pageIndex, int pageSize, out Hashtable conditions)
        {
            var errors = new List<string>();

            Int32 pageStart = pageSize * (pageIndex - 1) + 1;
            Int32 pageEnd = pageSize * pageIndex;
            EndDate = EndDate.AddDays(1).AddSeconds(-1);

            conditions = new Hashtable()
                {
                    {"StartDate",StartDate},
                    {"EndDate", EndDate},
                    {"PageStart", pageStart},
                    {"PageEnd", pageEnd},
                };

            if (!string.IsNullOrWhiteSpace(DeviceCode))
            {
                if (DeviceCode.ToInt32() == ConvertorExtension.ConvertionFailureValue)
                {
                    errors.Add("设备编号的输入值必须是数字");
                }
                if (ApplicationManager.GetInstance().AuthorizationDevices.All(x => x.Code != DeviceCode))
                {
                    errors.Add("输入的设备编号不存在系统中");
                }

                conditions.Add("DeviceCode", DeviceCode);
            }

            if (!string.IsNullOrWhiteSpace(UserCode))
            {
                if (UserCode.ToInt32() == ConvertorExtension.ConvertionFailureValue)
                {
                    errors.Add("人员工号的输入值必须是数字");
                }

                conditions.Add("DeviceUserId", UserCode);
            }

            if (!string.IsNullOrWhiteSpace(OperatorId))
            {
                if (OperatorId.ToInt32() == ConvertorExtension.ConvertionFailureValue)
                {
                    errors.Add("操作人员工号的输入值必须是数字");
                }

                conditions.Add("OperatorId", OperatorId);
            }

            if (errors.Any())
            {
                var message = string.Join("\n", errors);
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceOperationLogPage_ShowNotification);
                });
                return false;
            }

            return true;
        }

        private void ExportCommandFunc()
        {
            if (!DeviceOperationLogViewModels.Any())
            {
                Messenger.Default.Send(new NotificationMessage("没有数据可以导出！"), Tokens.DeviceOperationLogPage_ShowNotification);
                return;
            }

            var dt = DataHelper<DeviceTrafficLogViewModel>.ListToDataTable(DeviceOperationLogViewModels.ToList());
            dt.Columns.Remove("LogID");
            dt.Columns.Remove("OperationUploadTime");
            dt.Columns.Remove("IsInDesignMode");
            dt.Columns.Remove("DeviceId");

            dt.Columns["DeviceCode"].SetOrdinal(0);
            dt.Columns["DeviceType"].SetOrdinal(1);
            dt.Columns["OperatorId"].SetOrdinal(2);
            dt.Columns["DeviceUserId"].SetOrdinal(3);
            dt.Columns["OperationType"].SetOrdinal(4);
            dt.Columns["OperationDescription"].SetOrdinal(5);
            dt.Columns["OperationContent"].SetOrdinal(6);
            dt.Columns["OperationTime"].SetOrdinal(7);

            dt.Columns["DeviceCode"].ColumnName = "设备编号";
            dt.Columns["DeviceType"].ColumnName = "设备类型";
            dt.Columns["OperatorId"].ColumnName = "操作人员工号";
            dt.Columns["DeviceUserId"].ColumnName = "人员工号";
            dt.Columns["OperationType"].ColumnName = "操作类型";
            dt.Columns["OperationDescription"].ColumnName = "操作描述";
            dt.Columns["OperationContent"].ColumnName = "操作内容";
            dt.Columns["OperationTime"].ColumnName = "操作时间";

            Messenger.Default.Send(new OpenWindowMessage() { DataContext = dt }, Tokens.DeviceOperationLogPage_OpenExportView);
        }
    }
}
