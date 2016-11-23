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
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class SysOperationLogPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysOperationLogRepository _sysOperationLogRepo = NinjectBinder.GetRepository<ISysOperationLogRepository>();


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String OperatorId { get; set; }
        public String Keyword { get; set; }
        public ObservableCollection<SysOperationLogViewModel> SysOperationLogViewModels { get; set; }

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

        public SysOperationLogPageViewModel()
        {
            QueryCommand = new AuthCommand(QueryCommandFunc);
            ExportCommand = new AuthCommand(ExportCommandFunc);
            NextPageSearchCommand = new AuthCommand(NextPageSearchCommandFunc);

            SysOperationLogViewModels = new ObservableCollection<SysOperationLogViewModel>();

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
                            Log.Info("Querying data...");
                            int totalCount = 0;
                            SysOperationLogViewModels = QueryData(conditions, out totalCount);
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

                    if (!SysOperationLogViewModels.Any())
                    {
                        Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_QueryResultIsEmpty)), Tokens.SysOperationLogPage_ShowNotification);
                    }
                });

            }
            catch (Exception ex)
            {
                
            }
        }


        private ObservableCollection<SysOperationLogViewModel> QueryData(Hashtable conditions, out int totalCount)
        {
            var paninationResult = _sysOperationLogRepo.QueryPage(conditions);
            totalCount = paninationResult.TotalCount;
            var logVM = paninationResult.Entities.Select(AutoMapper.Mapper.Map<SysOperationLogViewModel>);

            SysOperationLogViewModels = new ObservableCollection<SysOperationLogViewModel>(logVM);
            return SysOperationLogViewModels;
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

            if (!string.IsNullOrWhiteSpace(Keyword))
                conditions.Add("Keyword", Keyword);

            if (!string.IsNullOrWhiteSpace(OperatorId))
            {
                if (OperatorId.ToInt32() == ConvertorExtension.ConvertionFailureValue)
                {
                    errors.Add(LanguageManager.GetLocalizationResource(Resource.MSG_OperatorIDMustBeNumber));
                }

                conditions.Add("UserID", OperatorId);
            }

            if (errors.Any())
            {
                var message = string.Join("\n", errors);
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Messenger.Default.Send(new NotificationMessage(message), Tokens.SysOperationLogPage_ShowNotification);
                });
                return false;
            }

            return true;
        }

        private void ExportCommandFunc()
        {
            if (!SysOperationLogViewModels.Any())
            {
                Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_NoDataToExport)), Tokens.SysOperationLogPage_ShowNotification);
                return;
            }

            var dt = DataHelper<DeviceTrafficLogViewModel>.ListToDataTable(SysOperationLogViewModels.ToList());
            dt.Columns.Remove("LogID");
            dt.Columns.Remove("DepartmentID");
            dt.Columns.Remove("Remark");
            dt.Columns.Remove("IsInDesignMode");

            dt.Columns["UserID"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperatorID);
            dt.Columns["UserName"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperatorName);
            dt.Columns["OperationCode"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperationCode);
            dt.Columns["OperationName"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperationName);
            dt.Columns["Detail"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperationDetail);
            dt.Columns["CreateDate"].ColumnName = LanguageManager.GetLocalizationResource(Resource.OperationCreateDate);

            Messenger.Default.Send(new OpenWindowMessage() { DataContext = dt }, Tokens.SysOperationLogPage_OpenExportView);
        }
    }
}
