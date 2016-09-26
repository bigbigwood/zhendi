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
    public class DataSyncPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysConfigRepository _sysConfigRepository = NinjectBinder.GetRepository<ISysConfigRepository>();
        private const String DataSyncConfig = "DataSyncConfig";
        public RelayCommand SaveCmd { get; private set; }
        public ObservableCollection<DataSyncConfigViewModel> DataSyncConfigViewModels { get; set; }
        public DataSyncConfigViewModel SelectedDataSyncConfigViewModel { get; set; }

        public DataSyncPageViewModel()
        {
            SaveCmd = new AuthCommand(SaveSysConfigs);
            DataSyncConfigViewModels = new ObservableCollection<DataSyncConfigViewModel>(InitDataSyncConfigs());
        }

        private List<DataSyncConfigViewModel> InitDataSyncConfigs()
        {
            var configViewModels = new List<DataSyncConfigViewModel>();
            for (int index = 0; index < 10; index++)
            {
                configViewModels.Add(new DataSyncConfigViewModel() { Value = "00:00:00" });
            }

            var config = _sysConfigRepository.Query(new Hashtable()).FirstOrDefault(x => x.Name == DataSyncConfig);
            if (config != null)
            {
                var configs = config.Value.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                for (int index = 0; index < configs.Count(); index++)
                {
                    configViewModels[index].Value = configs[index];
                    configViewModels[index].IsSelected = true;
                }
            }

            return configViewModels;
        }

        private void SaveSysConfigs()
        {
            try
            {
                string configValues = "";

                var enabledItems = DataSyncConfigViewModels.FindAll(x => x.IsSelected);
                if (enabledItems.Any())
                {
                    configValues = string.Join(";", enabledItems.Select(x =>
                    {
                        var mydt = DateTime.Parse(x.Value);
                        return mydt.ToString("HH:mm:ss");
                    }));
                }

                var config = _sysConfigRepository.Query(new Hashtable()).FirstOrDefault(x => x.Name == DataSyncConfig);
                config.Value = configValues;
                _sysConfigRepository.Update(config);
                Messenger.Default.Send(new NotificationMessage("保存成功!"), Tokens.DataSyncPage_ShowNotification);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
