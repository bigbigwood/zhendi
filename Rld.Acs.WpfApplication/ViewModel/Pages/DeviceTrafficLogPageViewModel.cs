using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using log4net;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
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
        public RelayCommand<Int32> QueryCmd { get; private set; }
        public DeviceTrafficLogPageViewModel()
        {
            QueryCmd = new RelayCommand<Int32>(QueryTrafficLog);
            DeviceTrafficLogViewModels = new ObservableCollection<DeviceTrafficLogViewModel>();

            StartDate = DateTime.Now.AddDays(-7);
            EndDate = DateTime.Now;
        }

        private void QueryTrafficLog(Int32 trafficLogType)
        {
            try
            {
                Int32 pageStart = 1;
                Int32 pageEnd = 2;

                var conditions = new Hashtable()
                {
                    {"RecordType", trafficLogType},
                    {"PageStart", pageStart},
                    {"PageEnd", pageEnd},
                    {"StartDate",StartDate},
                    {"EndDate", EndDate},
                };

                if (!string.IsNullOrWhiteSpace(DeviceId))
                    conditions.Add("DeviceId", DeviceId);

                if (!string.IsNullOrWhiteSpace(DeviceUserId))
                    conditions.Add("DeviceUserId", DeviceUserId);

                var logs = _deviceTrafficLogRepo.Query(conditions);
                var logVM = logs.Select(AutoMapper.Mapper.Map<DeviceTrafficLogViewModel>);
                DeviceTrafficLogViewModels = new ObservableCollection<DeviceTrafficLogViewModel>(logVM);

                RaisePropertyChanged(null);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

    }
}
