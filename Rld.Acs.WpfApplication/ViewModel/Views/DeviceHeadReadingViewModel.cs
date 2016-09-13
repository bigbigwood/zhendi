using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using FluentValidation.Resources;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class DeviceHeadReadingViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Boolean IsSelected { get; set; }
        public Int32 DeviceHeadReadingID { get; set; }
        public Int32 DeviceID { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
        public String Mac { get; set; }
        public String HeadReadingSN { get; set; }
        public Int32 HeadReadingType { get; set; }
        public String HeadReadingPerformance { get; set; }
        public Int32 Status { get; set; }

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public RelayCommand<DeviceHeadReadingViewModel> ModifyHeadReadingCmd { get; private set; }

        public List<SysDictionary> HeadReaderTypeDict
        {
            get { return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.HeadReaderType); }
        }


        public DeviceHeadReadingViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            ModifyHeadReadingCmd = new RelayCommand<DeviceHeadReadingViewModel>(ModifyDeviceHeadReading);
        }

        private void Save()
        {
            if (DeviceHeadReadingID == 0)
            {
                Status = 1;
            }

            RaisePropertyChanged(null);
            Close("");
        }

        private void Cancel()
        {
            RaisePropertyChanged(null);
            Close("");
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseDeviceHeadReadingView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceHeadReadingView_ShowNotification);
        }

        private void ModifyDeviceHeadReading(DeviceHeadReadingViewModel headReadingViewModel)
        {
            try
            {
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = headReadingViewModel,
                    WindowType = "DeviceHeadReadingView"
                }, Tokens.OpenDeviceHeadReadingView);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
