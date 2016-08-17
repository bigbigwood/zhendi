using System;
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

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class DeviceViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceControllerRepository _deviceControllerRepo = NinjectBinder.GetRepository<IDeviceControllerRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public DeviceViewModel(DeviceController device)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            CurrentDeviceController = device;
            if (device.DeviceID != 0)
            {
                FromCoreModel(device);
            }

            Title = (device.DeviceID == 0) ? "新增设备" : "修改设备";
        }
        public DeviceController CurrentDeviceController { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
        public String Mac { get; set; }
        public String SN { get; set; }
        public String Title { get; set; }

        public String DoorListString
        {
            get { return CurrentDeviceController.GetDeviceAssociatedDoorList(); }
        }
        public String HeadReadingListString
        {
            get { return CurrentDeviceController.GetDeviceAssociatedHeadReadingList(); }
        }

        private void FromCoreModel(DeviceController device)
        {
            Name = device.Name;
            Code = device.Code;
            Mac = device.Mac;
            SN = device.SN;
        }

        private void ToCoreModel(DeviceController device)
        {
            device.Name = Name;
            device.Code = Code;
            device.Mac = Mac;
            device.SN = SN;
        }

        private void Save()
        {
            string message = "";
            try
            {
                ToCoreModel(CurrentDeviceController);

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

                if (CurrentDeviceController.DeviceID == 0)
                {
                    CurrentDeviceController.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentDeviceController.CreateDate = DateTime.Now;
                    CurrentDeviceController = _deviceControllerRepo.Insert(CurrentDeviceController);

                    message = "增加设备成功!";
                }
                else
                {
                    CurrentDeviceController.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentDeviceController.UpdateDate = DateTime.Now;
                    _deviceControllerRepo.Update(CurrentDeviceController);

                    message = "修改设备成功!";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update device fails.", ex);
                message = "保存设备失败";
                SendMessage(message);
                return;
            }

            RaisePropertyChanged(null);
            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.OpenDeviceView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceView_ShowNotification);
        }
    }
}
