using System;
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
using Rld.Acs.WpfApplication.Service.Language;
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
                            var resultTypes = new DeviceServiceClient().SyncDeviceOperationLogs(devices.ToArray(), out messages);
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
                        Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_QueryResultIsEmpty)), Tokens.DeviceOperationLogPage_ShowNotification);
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
                    errors.Add(LanguageManager.GetLocalizationResource(Resource.MSG_DeviceCodeMustBeNumber));
                }
                if (ApplicationManager.GetInstance().AuthorizationDevices.All(x => x.Code != DeviceCode))
                {
                    errors.Add(LanguageManager.GetLocalizationResource(Resource.MSG_InputDeviceCodeDoesNotExist));
                }

                conditions.Add("DeviceCode", DeviceCode);
            }

            if (!string.IsNullOrWhiteSpace(UserCode))
            {
                if (UserCode.ToInt32() == ConvertorExtension.ConvertionFailureValue)
                {
                    errors.Add(LanguageManager.GetLocalizationResource(Resource.MSG_UserNoMustBeNumber));
                }

                conditions.Add("DeviceUserId", UserCode);
            }

            if (!string.IsNullOrWhiteSpace(OperatorId))
            {
                if (OperatorId.ToInt32() == ConvertorExtension.ConvertionFailureValue)
                {
                    errors.Add(LanguageManager.GetLocalizationResource(Resource.MSG_InputOperatorIDMustBeNumber));
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
                Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_NoDataToExport)), Tokens.DeviceOperationLogPage_ShowNotification);
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

            dt.Columns["DeviceCode"].ColumnName = LanguageManager.GetLocalizationResource(Resource.DeviceCode);
            dt.Columns["DeviceType"].ColumnName = LanguageManager.GetLocalizationResource(Resource.DeviceType);
            dt.Columns["OperatorId"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperatorId);
            dt.Columns["DeviceUserId"].ColumnName = LanguageManager.GetLocalizationResource(Resource.DeviceUserId);
            dt.Columns["OperationType"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperationType);
            dt.Columns["OperationDescription"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperationDescription);
            dt.Columns["OperationContent"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperationContent);
            dt.Columns["OperationTime"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperationTime);

            Messenger.Default.Send(new OpenWindowMessage() { DataContext = dt }, Tokens.DeviceOperationLogPage_OpenExportView);
        }
    }
}
