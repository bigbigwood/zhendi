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
using Rld.Acs.WpfApplication.Service;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class DeviceDoorViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public Boolean IsSelected { get; set; }
        public virtual Int32 DeviceDoorID { get; set; }
        public virtual Int32 DeviceID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Code { get; set; }
        public virtual Int32? ElectricalAppliances { get; set; }
        public virtual Int32? CheckOutOptions { get; set; }
        public virtual Int32? Status { get; set; }
        public virtual String Remark { get; set; }
        public virtual Int32? RingType { get; set; }
        public virtual Int32 DelayOpenSeconds { get; set; }
        public virtual Int32 OverTimeOpenSeconds { get; set; }
        public virtual Int32 IllegalOpenSeconds { get; set; }
        public virtual Boolean LinkageAlarm { get; set; }
        public virtual Boolean DuressEnabled { get; set; }
        public virtual Int32 DuressFingerPrintIndex { get; set; }
        public virtual Boolean DuressOpen { get; set; }
        public virtual Boolean DuressAlarm { get; set; }
        public virtual String DuressPassword { get; set; }

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public RelayCommand<DeviceDoorViewModel> ModifyDoorCmd { get; private set; }
        public RelayCommand SelectDoorCmd { get; private set; }

        public List<SysDictionary> CheckOutOptionsDict
        {
            get { return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.CheckOutOptions); }
        }

        public List<SysDictionary> RingTypeDict
        {
            get { return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.RingType); }
        }


        public DeviceDoorViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            ModifyDoorCmd = new RelayCommand<DeviceDoorViewModel>(ModifyDeviceDoor);
            SelectDoorCmd = new RelayCommand(SelectDeviceDoor);
        }

        private void Save()
        {
            if (DeviceDoorID == 0)
            {
                Status = 1;
            }

            RaisePropertyChanged(null);
            Close("");
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseDeviceDoorView);
        }

        private void ModifyDeviceDoor(DeviceDoorViewModel doorViewModel)
        {
            try
            {
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = doorViewModel,
                    WindowType = "DeviceDoorView"
                }, Tokens.OpenDeviceDoorView);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void SelectDeviceDoor()
        {
            try
            {
                if (IsSelected == false)
                {
                    if (FloorDoorManager.GetInstance().AuthorizationFloorDoor.Any(x => x.DoorID == DeviceDoorID))
                    {
                        string message = "门已经和楼层绑定，不能禁用！";
                        Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceView_ShowNotification);
                        IsSelected = true;
                        RaisePropertyChanged("IsSelected");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
