using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
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
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Validator;
using TimeZone = Rld.Acs.Model.TimeZone;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class FloorDoorViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public Int32 FloorDoorID { get; set; }
        public Int32 FloorID { get; set; }
        public Int32 DoorID { get; set; }
        public Int32 DoorType { get; set; }
        public Int32 LocationX { get; set; }
        public Int32 LocationY { get; set; }
        public Int32 Rotation { get; set; }

        public String DoorName { get; set; }

        public Boolean Enabled
        {
            get { return FloorDoorID == 0; }
        }

        public FloorDoorViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
        }

        private void Save()
        {
            string message = "";
            try
            {
                if (FloorDoorID == 0)
                {

                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update device fails.", ex);
                message = "保存设备权限失败";
                SendMessage(message);
                return;
            }

            RaisePropertyChanged(null);
            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseDeviceRoleView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceRoleView_ShowNotification);
        }
    }
}
