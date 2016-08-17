using System;
using System.Collections.Generic;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Repository.Interfaces;
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
        public List<DeviceController> Devices { get; set; }
        public List<TimeZone> Timezones { get; set; }

        public DeviceRole CurrentDeviceRole { get; set; }
        public String Name { get; set; }
        public String Title { get; set; }
        public String DeviceListString
        {
            get { return CurrentDeviceRole.GetDeviceAssociatedDeviceList(Devices); }
        }
        public String TimezoneListString
        {
            get { return CurrentDeviceRole.GetDeviceAssociatedTimezoneList(Timezones); }
        }

        public DeviceRoleViewModel(DeviceRole deviceRole)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            Devices = ApplicationManager.GetInstance().AuthorizationDevices;
            CurrentDeviceRole = deviceRole;
            if (deviceRole.DeviceRoleID != 0)
            {
                FromCoreModel(deviceRole);
            }

            Title = (deviceRole.DeviceRoleID == 0) ? "新增设备角色" : "修改设备角色";
        }
        private void FromCoreModel(DeviceRole deviceRole)
        {
            Name = deviceRole.RoleName;
            //Code = device.Code;
            //Mac = device.Mac;
            //SN = device.SN;
        }

        private void ToCoreModel(DeviceRole deviceRole)
        {
            deviceRole.RoleName = Name;
            //device.Code = Code;
            //device.Mac = Mac;
            //device.SN = SN;
        }

        private void Save()
        {
            string message = "";
            try
            {
                ToCoreModel(CurrentDeviceRole);

                //if (StartHour.Length == 1) StartHour = "0" + StartHour;
                //if (EndHour.Length == 1) StartHour = "0" + EndHour;

                //CurrentTimeSegment.BeginTime = string.Format("{0}:{1}", StartHour, StartMinute);
                //CurrentTimeSegment.EndTime = string.Format("{0}:{1}", EndHour, EndMinute);
                //CurrentTimeSegment.TimeSegmentName = Name;
                //CurrentTimeSegment.Status = GeneralStatus.Enabled;

                //var validator = NinjectBinder.GetValidator<TimeSegmentValidator>();
                //var results = validator.Validate(CurrentTimeSegment);
                //if (!results.IsValid)
                //{
                //    message = string.Join("\n", results.Errors);
                //    SendMessage(message);
                //    return;
                //}

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

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.OpenDeviceRoleView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceRoleView_ShowNotification);
        }
    }
}
