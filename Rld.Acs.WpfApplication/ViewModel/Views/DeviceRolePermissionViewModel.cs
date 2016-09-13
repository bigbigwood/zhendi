using System;
using System.Collections;
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
    public class DeviceRolePermissionViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITimeZoneRepository _timeZoneRepository = NinjectBinder.GetRepository<ITimeZoneRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public List<DeviceController> AuthorizationDevices
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices; }
        }
        public List<TimeZone> AuthorizationTimezones
        {
            get { return _timeZoneRepository.Query(new Hashtable ()).FindAll(x => x.Status == GeneralStatus.Enabled); }
        }

        public Boolean IsSelected { get; set; }
        public Int32 Id { get; set; }
        public Int32 DeviceRoleID { get; set; }
        public String Name { get; set; }
        public DeviceController SelectedDevice { get; set; }
        public DevicePermissionAction PermissionAction { get; set; }
        public String UserGroupVM { get; set; }
        public TimeZone SelectedTimezone { get; set; }
        public DateTime STARTDATE { get; set; }
        public DateTime? Enddate { get; set; }

        public DeviceRolePermissionViewModel(DeviceRolePermission deviceRolePermission)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            FromCoreModel(deviceRolePermission);
        }

        private void FromCoreModel(DeviceRolePermission permission)
        {
            SelectedDevice = AuthorizationDevices.FirstOrDefault(x => x.DeviceID == permission.DeviceID);
            SelectedTimezone = AuthorizationTimezones.FirstOrDefault(x => x.TimeZoneID == permission.AllowedAccessTimeZoneID);

            Id = permission.DeviceRolePermissionID;
            DeviceRoleID = permission.DeviceRoleID;
            Name = SelectedDevice.Name;
            PermissionAction = permission.PermissionAction;
            UserGroupVM = permission.UserGroupVM;
            STARTDATE = permission.STARTDATE;
            Enddate = permission.Enddate;
        }

        public DeviceRolePermission ToCoreModel()
        {
            DeviceRolePermission permission = new DeviceRolePermission();
            permission.DeviceRolePermissionID = Id;
            permission.DeviceID = SelectedDevice.DeviceID;
            permission.DeviceRoleID = DeviceRoleID;
            permission.Enable = true;
            permission.PermissionAction = PermissionAction;
            permission.UserGroupVM = UserGroupVM;
            permission.AllowedAccessTimeZoneID = SelectedTimezone.TimeZoneID;
            permission.STARTDATE = STARTDATE;
            permission.Enddate = Enddate;
            return permission;
        }

        private void Save()
        {
            //string message = "";
            //try
            //{
            //    ToCoreModel()
            //}
            //catch (Exception ex)
            //{
            //    Log.Error("Update device fails.", ex);
            //    message = "保存设备权限失败";
            //    SendMessage(message);
            //    return;
            //}

            RaisePropertyChanged(null);
            Close("");
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseDeviceRolePermissionView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceRolePermissionView_ShowNotification);
        }
    }
}
