using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using log4net;
using Microsoft.SqlServer.Server;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class DataCleanPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysConfigRepository _sysConfigRepository = NinjectBinder.GetRepository<ISysConfigRepository>();
        private const String AutoCleanConfig = "AutoCleanConfig";
        private const String SysLogExpireMonths = "SysLogExpireMonths";
        private const String DeviceTrafficLogExpiredMonths = "DeviceTrafficLogExpiredMonths";
        private const String DeviceMngtLogExpiredMonths = "DeviceMngtLogExpiredMonths";
        private const String DoorHistoryExpiredMonths = "DoorHistoryExpiredMonths";

        public RelayCommand SaveCmd { get; private set; }
        public SysConfigViewModel SysLogExpiredMonthsConfigViewModel { get; set; }
        public SysConfigViewModel DeviceTrafficLogExpiredMonthsConfigViewModel { get; set; }
        public SysConfigViewModel DeviceMngtLogExpiredMonthsConfigViewModel { get; set; }
        public SysConfigViewModel DoorHistoryExpiredMonthsConfigViewModel { get; set; }

        public DataCleanPageViewModel()
        {
            SaveCmd = new AuthCommand(SaveSysConfigs);
            InitConfigs();
        }

        private void InitConfigs()
        {
            var autoCleanConfig = _sysConfigRepository.Query(new Hashtable()).FirstOrDefault(x => x.Name == AutoCleanConfig);

            var configs = autoCleanConfig.Value.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
            var sysLogExpireMonths = configs.First(x => x.StartsWith(SysLogExpireMonths)).Substring(SysLogExpireMonths.Length + 1).ToInt32();
            var deviceTrafficLogExpiredMonths = configs.First(x => x.StartsWith(DeviceTrafficLogExpiredMonths)).Substring(DeviceTrafficLogExpiredMonths.Length + 1).ToInt32();
            var deviceMngtLogExpiredMonths = configs.First(x => x.StartsWith(DeviceMngtLogExpiredMonths)).Substring(DeviceMngtLogExpiredMonths.Length + 1).ToInt32();
            var doorHistoryExpiredMonths = configs.First(x => x.StartsWith(DoorHistoryExpiredMonths)).Substring(DoorHistoryExpiredMonths.Length + 1).ToInt32();

            SysLogExpiredMonthsConfigViewModel = new SysConfigViewModel() {Value = sysLogExpireMonths.ToString(), IsSelected = sysLogExpireMonths != 0};
            DeviceTrafficLogExpiredMonthsConfigViewModel = new SysConfigViewModel() { Value = deviceTrafficLogExpiredMonths.ToString(), IsSelected = deviceTrafficLogExpiredMonths != 0 };
            DeviceMngtLogExpiredMonthsConfigViewModel = new SysConfigViewModel() { Value = deviceMngtLogExpiredMonths.ToString(), IsSelected = deviceMngtLogExpiredMonths != 0 };
            DoorHistoryExpiredMonthsConfigViewModel = new SysConfigViewModel() { Value = doorHistoryExpiredMonths.ToString(), IsSelected = doorHistoryExpiredMonths != 0 };
        }

        private void SaveSysConfigs()
        {
            try
            {
                var values = "";
                values += string.Format("{0}={1};", SysLogExpireMonths,
                    SysLogExpiredMonthsConfigViewModel.IsSelected ? SysLogExpiredMonthsConfigViewModel.Value : "0");

                values += string.Format("{0}={1};", DeviceMngtLogExpiredMonths,
                    DeviceMngtLogExpiredMonthsConfigViewModel.IsSelected ? DeviceMngtLogExpiredMonthsConfigViewModel.Value : "0");

                values += string.Format("{0}={1};", DeviceTrafficLogExpiredMonths,
                    DeviceTrafficLogExpiredMonthsConfigViewModel.IsSelected ? DeviceTrafficLogExpiredMonthsConfigViewModel.Value : "0");

                values += string.Format("{0}={1};", DoorHistoryExpiredMonths,
                    DoorHistoryExpiredMonthsConfigViewModel.IsSelected ? DoorHistoryExpiredMonthsConfigViewModel.Value : "0");

                var autoCleanConfig = _sysConfigRepository.Query(new Hashtable()).FirstOrDefault(x => x.Name == AutoCleanConfig);

                autoCleanConfig.Value = values;
                _sysConfigRepository.Update(autoCleanConfig);

                Messenger.Default.Send(new NotificationMessage("保存成功!"), Tokens.DataCleanPage_ShowNotification);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
