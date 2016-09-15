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
            SaveCmd = new RelayCommand(SaveSysConfigs);
            DataSyncConfigViewModels = new ObservableCollection<DataSyncConfigViewModel>(InitDataSyncConfigs());
        }

        private List<DataSyncConfigViewModel> InitDataSyncConfigs()
        {
            var configViewModels = new List<DataSyncConfigViewModel>();
            for (int index = 0; index < 10; index++)
            {
                configViewModels.Add(new DataSyncConfigViewModel() { Value = "00:00:00" });
            }

            var configs = _sysConfigRepository.Query(new Hashtable()).FindAll(x => x.Name == "DataSyncConfig");
            if (configs.Any())
            {
                for (int index = 0; index < configs.Count; index++)
                {
                    configViewModels[index] = Mapper.Map<DataSyncConfigViewModel>(configs[index]);
                    configViewModels[index].IsSelected = true;
                }
            }

            return configViewModels;
        }

        private void SaveSysConfigs()
        {
            //try
            //{
            //    var viewModel = Mapper.Map<DataSyncConfigViewModel>(new SysConfig());
            //    Messenger.Default.Send(new OpenWindowMessage()
            //    {
            //        DataContext = viewModel

            //    }, Tokens.DataSyncView_Open);

            //    if (viewModel.ViewModelAttachment.LastOperationSuccess)
            //    {
            //        DataSyncConfigViewModels.Add(viewModel);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex);
            //}
        }
    }
}
