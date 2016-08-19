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
    public class DeviceRoleViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceRoleRepository _deviceRoleRepo = NinjectBinder.GetRepository<IDeviceRoleRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public List<DeviceController> AuthorizationDevices { get; set; }
        public List<TimeZone> Timezones { get; set; }
        public List<SysDictionary> PermissionActionDict { get; set; }
        public TimeZone SelectedTimezone { get; set; }
        public Int32 SelectedPermissionAction { get; set; }
        public DeviceRole CurrentDeviceRole { get; set; }
        public ObservableCollection<SelectableItem> DeviceDtos { get; set; }
        public String Name { get; set; }
        public String Title { get; set; }
        public String DeviceListString
        {
            get { return CurrentDeviceRole.GetDeviceAssociatedDeviceList(AuthorizationDevices); }
        }
        public String PermissionActionString
        {
            get { return CurrentDeviceRole.GetDeviceAssociatedPermissionActionList(PermissionActionDict); }
        }
        public String TimezoneListString
        {
            get { return CurrentDeviceRole.GetDeviceAssociatedTimezoneList(Timezones); }
        }


        public DeviceRoleViewModel(DeviceRole deviceRole)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            DeviceDtos = new ObservableCollection<SelectableItem>();

            Timezones = ApplicationManager.GetInstance().AuthorizationTimezones;
            AuthorizationDevices = ApplicationManager.GetInstance().AuthorizationDevices;
            AuthorizationDevices.ForEach(d => DeviceDtos.Add(new ListBoxItem
                {
                    ID = d.DeviceID, DisplayName = d.Name, IsSelected =  deviceRole.HasDeviceAuthorization(d.DeviceID)
                }));
            PermissionActionDict = DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.DevicePermission);

            CurrentDeviceRole = deviceRole;
            if (deviceRole.DeviceRoleID != 0)
            {
                Name = deviceRole.RoleName;
                if (deviceRole.DeviceRolePermissions.Any())
                {
                    var firstPermission = deviceRole.DeviceRolePermissions.First();
                    SelectedTimezone = Timezones.FirstOrDefault(x => x.TimeZoneID == firstPermission.AllowedAccessTimeZoneID);
                    SelectedPermissionAction = (int)firstPermission.PermissionAction;
                }
            }
            
            Title = (deviceRole.DeviceRoleID == 0) ? "新增设备角色" : "修改设备角色";
        }

        private void Save()
        {
            string message = "";
            try
            {
                CurrentDeviceRole.RoleName = Name;
                CurrentDeviceRole.Status = GeneralStatus.Enabled;
                CurrentDeviceRole.DeviceRolePermissions = GetDeviceRolePermissionFromUI();

                if (CurrentDeviceRole.DeviceRoleID == 0)
                {
                    CurrentDeviceRole.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentDeviceRole.CreateDate = DateTime.Now;
                    CurrentDeviceRole = _deviceRoleRepo.Insert(CurrentDeviceRole);

                    message = "增加设备权限成功!";
                }
                else
                {
                    CurrentDeviceRole.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentDeviceRole.UpdateDate = DateTime.Now;
                    _deviceRoleRepo.Update(CurrentDeviceRole);

                    message = "修改设备权限成功!";
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

        private List<DeviceRolePermission> GetDeviceRolePermissionFromUI()
        {
            var result = new List<DeviceRolePermission>();
            var selected = DeviceDtos.FindAll(x => x.IsSelected);
            foreach (var checkboxItem in selected)
            {
                var rolePermission = CurrentDeviceRole.DeviceRolePermissions.FirstOrDefault(x => x.DeviceID == checkboxItem.ID);
                if (rolePermission == null)
                {
                    rolePermission = new DeviceRolePermission()
                    {
                        DeviceID = checkboxItem.ID,
                        DeviceRoleID = CurrentDeviceRole.DeviceRoleID,
                        Enable = true,
                        STARTDATE = DateTime.Now,
                    };
                }

                rolePermission.PermissionAction = (DevicePermissionAction)SelectedPermissionAction;
                rolePermission.AllowedAccessTimeZoneID = SelectedTimezone.TimeZoneID;

                result.Add(rolePermission);
            }
            return result;
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
