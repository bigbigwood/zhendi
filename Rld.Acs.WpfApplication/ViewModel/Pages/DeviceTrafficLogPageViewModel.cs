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
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class DeviceTrafficLogPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceTrafficLogRepository _deviceTrafficLogRepo = NinjectBinder.GetRepository<IDeviceTrafficLogRepository>();


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String DeviceId { get; set; }
        public String DeviceUserId { get; set; }
        public ObservableCollection<DeviceTrafficLogViewModel> DeviceTrafficLogViewModels { get; set; }
        public DeviceTrafficLogViewModel SelectedTrafficLogViewModel { get; set; }
        public Int32 SelectedLogType { get; set; }

        public List<SysDictionary> DeviceTrafficLogTypeDict
        {
            get { return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.DeviceTrafficLogType); }
        }

        public RelayCommand QueryCommand { get; set; }

        #region 分页相关属性

        private async void QueryCommandFunc()
        {
            await Task.Run(() =>
            {
                try
                {
                    int totalCount = 0;
                    var pageIndex = 1;
                    var conditions = GetConditions();
                    DeviceTrafficLogViewModels = QueryData(pageIndex, PageSize, out totalCount);
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
        }

        /// <summary>
        /// 分页查询命令
        /// </summary>
        private async void NextPageSearchCommandFunc()
        {
            await Task.Run(() =>
            {
                try
                {
                    int totalCount = 0;
                    var pageIndex = Convert.ToInt32(CurrentPage);
                    var conditions = GetConditions();
                    DeviceTrafficLogViewModels = QueryData(pageIndex, PageSize, out totalCount);
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


        public DeviceTrafficLogPageViewModel()
        {
            QueryCommand = new AuthCommand(QueryCommandFunc);
            NextPageSearchCommand = new AuthCommand(NextPageSearchCommandFunc);

            DeviceTrafficLogViewModels = new ObservableCollection<DeviceTrafficLogViewModel>();

            StartDate = DateTime.Now.AddDays(-7);
            EndDate = DateTime.Now;
        }


        private ObservableCollection<DeviceTrafficLogViewModel> QueryData(int pageIndex, int pageSize, out int totalCount)
        {
            Int32 pageStart = pageSize * (pageIndex - 1) + 1;
            Int32 pageEnd = pageSize * pageIndex;

            var conditions = GetConditions();
            conditions.Add("PageStart", pageStart);
            conditions.Add("PageEnd", pageEnd);

            var paninationResult = _deviceTrafficLogRepo.QueryPage(conditions);
            totalCount = paninationResult.TotalCount;
            var logVM = paninationResult.Entities.Select(AutoMapper.Mapper.Map<DeviceTrafficLogViewModel>);

            DeviceTrafficLogViewModels = new ObservableCollection<DeviceTrafficLogViewModel>(logVM);
            return DeviceTrafficLogViewModels;
        }

        private Hashtable GetConditions()
        {
            var errors = new List<string>();

            var conditions = new Hashtable()
                {
                    {"RecordType", SelectedLogType},
                    {"StartDate",StartDate},
                    {"EndDate", EndDate},
                };


            if (!string.IsNullOrWhiteSpace(DeviceId))
            {
                if (DeviceId.ToInt32() == ConvertorExtension.ConvertionFailureValue)
                {
                    errors.Add("设备ID的输入值必须是数字");
                }

                conditions.Add("DeviceId", DeviceId);
            }


            if (!string.IsNullOrWhiteSpace(DeviceUserId))
            {
                if (DeviceUserId.ToInt32() == ConvertorExtension.ConvertionFailureValue)
                {
                    errors.Add("用户设备ID的输入值必须是数字");
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
                throw new Exception("非法输入");
            }

            return conditions;
        }
    }
}
