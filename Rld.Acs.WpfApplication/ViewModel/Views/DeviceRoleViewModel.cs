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
using Rld.Acs.Unility.Exceptions;
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
        private ITimeZoneRepository _timeZoneRepository = NinjectBinder.GetRepository<ITimeZoneRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public List<DeviceController> AuthorizationDevices
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices; }
        }
        public List<TimeZone> Timezones
        {
            get { return _timeZoneRepository.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled); }
        }
        public List<SysDictionary> PermissionActionDict
        {
            get { return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.DevicePermission); }
        }
        public TimeZone SelectedTimezone { get; set; }
        public Int32 SelectedPermissionAction { get; set; }
        public DeviceRole CurrentDeviceRole { get; set; }
        public ObservableCollection<SelectableItem> DeviceDtos { get; set; }
        public ViewModelAttachment<DeviceRole> ViewModelAttachment { get; set; }
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
            ViewModelAttachment = new ViewModelAttachment<DeviceRole>();

            var dtos = AuthorizationDevices.Select(x => new ListBoxItem
            {
                ID = x.DeviceID,
                DisplayName = x.Name,
                IsSelected = deviceRole.HasDeviceAuthorization(x.DeviceID)
            });
            DeviceDtos = new ObservableCollection<SelectableItem>(dtos);

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
            else
            {
                SelectedTimezone = Timezones.FirstOrDefault();
                SelectedPermissionAction = PermissionActionDict.FirstOrDefault().ItemID.Value;
            }
            
            Title = (deviceRole.DeviceRoleID == 0) ? "新增设备角色" : "修改设备角色";
        }

        private void Save()
        {
            string message = "";
            try
            {
                var validator = NinjectBinder.GetValidator<DeviceRoleViewModelValidator>();
                var results = validator.Validate(this);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

                CurrentDeviceRole.RoleName = Name;
                CurrentDeviceRole.DeviceRolePermissions = GetDeviceRolePermissionFromUI();

                if (CurrentDeviceRole.DeviceRoleID == 0)
                {
                    CurrentDeviceRole.Status = GeneralStatus.Enabled;
                    CurrentDeviceRole.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentDeviceRole.CreateDate = DateTime.Now;
                    CurrentDeviceRole = _deviceRoleRepo.Insert(CurrentDeviceRole);

                    message = "增加设备角色成功!";
                }
                else
                {
                    CurrentDeviceRole.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentDeviceRole.UpdateDate = DateTime.Now;
                    _deviceRoleRepo.Update(CurrentDeviceRole);

                    message = "修改设备角色成功!";
                }
            }
            catch (BusinessException ex)
            {
                Log.Error("Update device role fails.", ex);
                SendMessage(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Log.Error("Update device role fails.", ex);
                message = "保存设备角色失败";
                SendMessage(message);
                return;
            }

            ViewModelAttachment.CoreModel = CurrentDeviceRole;
            ViewModelAttachment.LastOperationSuccess = true;
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
