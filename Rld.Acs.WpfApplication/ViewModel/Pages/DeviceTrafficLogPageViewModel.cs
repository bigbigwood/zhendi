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
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class DeviceTrafficLogPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceTrafficLogRepository _deviceTrafficLogRepo = NinjectBinder.GetRepository<IDeviceTrafficLogRepository>();


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String DeviceCode { get; set; }
        public String DeviceUserId { get; set; }
        public ObservableCollection<DeviceTrafficLogViewModel> DeviceTrafficLogViewModels { get; set; }
        public DeviceTrafficLogViewModel SelectedTrafficLogViewModel { get; set; }
        public Int32 SelectedLogType { get; set; }

        public List<SysDictionary> DeviceTrafficLogTypeDict
        {
            get { return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.DeviceTrafficLogType); }
        }

        public RelayCommand QueryCommand { get; set; }
        public RelayCommand ExportCommand { get; set; }

        #region 分页相关属性

        private async void QueryCommandFunc()
        {
            var pageIndex = 1;
            ProcessQueryPage(pageIndex);
        }

        /// <summary>
        /// 分页查询命令
        /// </summary>
        private async void NextPageSearchCommandFunc()
        {
            var pageIndex = Convert.ToInt32(CurrentPage);
            ProcessQueryPage(pageIndex);
        }

        private string _totalPage = "1";
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


        public DeviceTrafficLogPageViewModel()
        {
            QueryCommand = new AuthCommand(QueryCommandFunc);
            ExportCommand = new AuthCommand(ExportCommandFunc);
            NextPageSearchCommand = new AuthCommand(NextPageSearchCommandFunc);

            DeviceTrafficLogViewModels = new ObservableCollection<DeviceTrafficLogViewModel>();

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

                    var controller = await DialogCoordinator.Instance.ShowProgressAsync(this,
                        LanguageManager.GetLocalizationResource(Resource.MSG_QueryData),
                        LanguageManager.GetLocalizationResource(Resource.MSG_QueryingData));
                    controller.SetIndeterminate();

                    await Task.Run(() =>
                    {
                        try
                        {
                            Log.Info("Data Synchorizing...");
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
                            var resultTypes = new DeviceServiceClient().SyncDeviceTrafficLogs(devices.ToArray(), out messages);
                            message = MessageHandler.GenerateDeviceMessage(resultTypes, messages,
                                LanguageManager.GetLocalizationResource(Resource.MSG_SyncDataSuccess),
                                LanguageManager.GetLocalizationResource(Resource.MSG_SyncDataFail));
                            Log.Info(message);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }

                        try
                        {
                            Log.Info("Querying data...");
                            int totalCount = 0;
                            DeviceTrafficLogViewModels = QueryData(conditions, out totalCount);
                            TotalPage = ((totalCount / PageSize) + 1).ToString();
                            if (totalCount != 0 && totalCount % PageSize == 0)
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

                    if (!DeviceTrafficLogViewModels.Any())
                    {
                        Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_QueryResultIsEmpty)), Tokens.DeviceTrafficLogPage_ShowNotification);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }


        private ObservableCollection<DeviceTrafficLogViewModel> QueryData(Hashtable conditions, out int totalCount)
        {
            var paninationResult = _deviceTrafficLogRepo.QueryPage(conditions);
            totalCount = paninationResult.TotalCount;
            var logVM = paninationResult.Entities.Select(AutoMapper.Mapper.Map<DeviceTrafficLogViewModel>);

            DeviceTrafficLogViewModels = new ObservableCollection<DeviceTrafficLogViewModel>(logVM);
            return DeviceTrafficLogViewModels;
        }

        private bool TryGetConditions(int pageIndex, int pageSize, out Hashtable conditions)
        {
            var errors = new List<string>();

            Int32 pageStart = pageSize * (pageIndex - 1) + 1;
            Int32 pageEnd = pageSize * pageIndex;
            EndDate = EndDate.AddDays(1).AddSeconds(-1);
            var logType = GetLogType();

            conditions = new Hashtable()
                {
                    {"RecordType", logType},
                    {"StartDate",StartDate},
                    {"EndDate", EndDate},
                    {"PageStart", pageStart},
                    {"PageEnd", pageEnd},
                };

            if (!string.IsNullOrWhiteSpace(DeviceCode))
            {
                if (DeviceCode.ToInt32() == ConvertorExtension.ConvertionFailureValue)
                {
                    errors.Add(LanguageManager.GetLocalizationResource(Resource.MSG_DeviceCodeMustBeNumber));
                }
                if (ApplicationManager.GetInstance().AuthorizationDevices.All(x => x.Code != DeviceCode))
                {
                    errors.Add(LanguageManager.GetLocalizationResource(Resource.MSG_InputDeviceCodeDoesNotExist));
                }

                conditions.Add("DeviceCode", DeviceCode);
            }


            if (!string.IsNullOrWhiteSpace(DeviceUserId))
            {
                if (DeviceUserId.ToInt32() == ConvertorExtension.ConvertionFailureValue)
                {
                    errors.Add(LanguageManager.GetLocalizationResource(Resource.MSG_UserNoMustBeNumber));
                }

                conditions.Add("DeviceUserId", DeviceUserId);
            }

            if (errors.Any())
            {
                var message = string.Join("\n", errors);
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceTrafficLogPage_ShowNotification);
                });
                return false;
            }

            return true;
        }

        private string GetLogType()
        {
            switch (SelectedLogType)
            {
                case 0:
                    return "General";
                case 1:
                    return "Alarm";
                case 2:
                    return "Exception";
                default:
                    return "General";
            }
        }

        private void ExportCommandFunc()
        {
            if (!DeviceTrafficLogViewModels.Any())
            {
                Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_NoDataToExport)), Tokens.DeviceTrafficLogPage_ShowNotification);
                return;
            }

            var dt = DataHelper<DeviceTrafficLogViewModel>.ListToDataTable(DeviceTrafficLogViewModels.ToList());
            dt.Columns.Remove("TrafficID");
            dt.Columns.Remove("RecordType");
            dt.Columns.Remove("RecordUploadTime");
            dt.Columns.Remove("AuthenticationType");
            dt.Columns.Remove("IsInDesignMode");
            dt.Columns.Remove("DeviceId");

            dt.Columns["DeviceUserID"].SetOrdinal(3);
            dt.Columns["AuthenticationString"].SetOrdinal(4);
            dt.Columns["RecordTime"].SetOrdinal(6);

            dt.Columns["DeviceCode"].ColumnName = LanguageManager.GetLocalizationResource(Resource.DeviceCode);
            dt.Columns["DeviceType"].ColumnName = LanguageManager.GetLocalizationResource(Resource.DeviceType);
            dt.Columns["DeviceSN"].ColumnName = LanguageManager.GetLocalizationResource(Resource.DeviceSN);
            dt.Columns["DeviceUserID"].ColumnName = LanguageManager.GetLocalizationResource(Resource.DeviceUserID);
            dt.Columns["AuthenticationString"].ColumnName = LanguageManager.GetLocalizationResource(Resource.AuthenticationString);
            dt.Columns["Remark"].ColumnName = LanguageManager.GetLocalizationResource(Resource.AuthenticationResult);
            dt.Columns["RecordTime"].ColumnName = LanguageManager.GetLocalizationResource(Resource.RecordTime);

            Messenger.Default.Send(new OpenWindowMessage() { DataContext = dt }, Tokens.DeviceTrafficLogPage_OpenExportView);
        }
    }
}
