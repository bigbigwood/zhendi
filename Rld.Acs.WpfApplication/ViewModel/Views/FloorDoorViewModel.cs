using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.WpfApplication.Models.Messages;

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
        public Double LocationX { get; set; }
        public Double LocationY { get; set; }
        public Int32 Rotation { get; set; }

        public String DoorName { get; set; }

        public Boolean Enabled { get; set; }

        public FloorDoorViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
        }

        private void Save()
        {
            RaisePropertyChanged(null);
            Close("");
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseDeviceRoleView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceRoleView_ShowNotification);
        }

        public static Boolean Compare(FloorDoorViewModel current, FloorDoorViewModel other)
        {
            if (current == null && other == null)
                return true;
            else if (current == null || other == null)
                return false;

            return (current.FloorID == other.FloorID) && (current.DoorID == other.DoorID);
        }
    }
}
