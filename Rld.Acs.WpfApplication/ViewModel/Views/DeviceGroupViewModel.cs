using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using AutoMapper;
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
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Validator;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class DeviceGroupViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceGroupRepository _deviceGroupRepo = NinjectBinder.GetRepository<IDeviceGroupRepository>();

        public Int32 DeviceGroupID { get; set; }
        public String DeviceGroupName { get; set; }
        public Int32 CheckInDeviceID { get; set; }
        public Int32 CheckOutDeviceID { get; set; }

        public String Title
        {
            get { return DeviceGroupID == 0 ? "新增设备组" : "修改设备组"; }
        }
        public String CheckInDeviceName 
        { 
            get { return ApplicationManager.GetInstance().AuthorizationDevices.First(x => x.DeviceID == CheckInDeviceID).Name;  }
        }
        public String CheckOutDeviceName
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices.First(x => x.DeviceID == CheckOutDeviceID).Name; }
        }
        public List<DeviceController> AuthorizationDevices
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices; }
        }
        public ViewModelAttachment<DeviceGroup> ViewModelAttachment { get; set; }
        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public DeviceGroupViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            ViewModelAttachment = new ViewModelAttachment<DeviceGroup>();
        }

        private void Save()
        {
            string message = "";
            try
            {
                var validator = NinjectBinder.GetValidator<DeviceGroupViewModelValidator>();
                var results = validator.Validate(this);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

                var coreModel = Mapper.Map<DeviceGroup>(this);

                if (DeviceGroupID == 0)
                {
                    coreModel = _deviceGroupRepo.Insert(coreModel);
                    message = "增加设备组成功!";
                }
                else
                {
                    _deviceGroupRepo.Update(coreModel);
                    message = "增加设备组成功!";
                }

                ViewModelAttachment.CoreModel = coreModel;
                ViewModelAttachment.LastOperationSuccess = true;
            }
            catch (Exception ex)
            {
                Log.Error("Update device role fails.", ex);
                message = "保存设备组失败";
                SendMessage(message);
                return;
            }

            RaisePropertyChanged(null);
            Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceGroupPage_ShowNotification);
            Close("");
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.DeviceGroupView_Close);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceGroupView_ShowNotification);
        }
    }
}
