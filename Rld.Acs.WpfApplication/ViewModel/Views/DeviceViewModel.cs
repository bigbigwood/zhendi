using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
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
using Rld.Acs.WpfApplication.ViewModel.Converter;
using TimeZone = Rld.Acs.Model.TimeZone;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class DeviceViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceControllerRepository _deviceControllerRepo = NinjectBinder.GetRepository<IDeviceControllerRepository>();
        private ITimeZoneRepository _timeZoneRepository = NinjectBinder.GetRepository<ITimeZoneRepository>();

        public Int32 Id { get; set; }
        public String Mac { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
        public String SN { get; set; }
        public String Model { get; set; }
        public Int32 CommunicationType { get; set; }
        public String BaudRate { get; set; }
        public String SerialPort { get; set; }
        public String Password { get; set; }
        public String IP { get; set; }
        public String Port { get; set; }
        public Int32 Protocol { get; set; }
        public String Label { get; set; }
        public String ServerURL { get; set; }
        public String Remark { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public String Title { get; set; }

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public ObservableCollection<DeviceDoorViewModel> DoorViewModels { get; set; }
        public ObservableCollection<DeviceHeadReadingViewModel> HeadReadingViewModels { get; set; }
        public DeviceExtensionViewModel DeviceExtensionViewModel { get; set; }

        public String DoorListString
        {
            get { return string.Join(", ", DoorViewModels.FindAll(x => x.IsSelected).Select(x => x.Name)); }
        }
        public String HeadReadingListString
        {
            get { return string.Join(", ", HeadReadingViewModels.FindAll(x => x.IsSelected).Select(x => x.Name)); }
        }
        public ViewModelAttachment<DeviceController> ViewModelAttachment { get; set; }

        public List<SysDictionary> CommunicationTypeDict
        {
            get { return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.CommunicationType); }
        }
        public List<SysDictionary> ProtocolDict
        {
            get { return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.Protocol); }
        }

        public List<SysDictionary> AuthticationTypeDict
        {
            get { return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.AuthticationType); }
        }

        public List<ComboBoxItem> Timezones
        {
            get
            {
                var allTimezones = _timeZoneRepository.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled);
                var items = new List<ComboBoxItem>() {new ComboBoxItem(){ID = 0, DisplayName = ""}};
                items.AddRange(allTimezones.Select(x => new ComboBoxItem() { ID = x.TimeZoneID, DisplayName = x.TimeZoneName }));
                return items;
            }
        }

        public DeviceViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            ViewModelAttachment = new ViewModelAttachment<DeviceController>();
            DoorViewModels = new ObservableCollection<DeviceDoorViewModel>();
            HeadReadingViewModels = new ObservableCollection<DeviceHeadReadingViewModel>();
        }

        private void Save()
        {
            string message = "";
            try
            {
                var deviceController = this.ToCoreModel();

                var validator = NinjectBinder.GetValidator<DeviceValidator>();
                var results = validator.Validate(deviceController);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

                var doorCodes = deviceController.DeviceDoors.Select(x => x.Code);
                var duplicatedDoorCodes = doorCodes.GroupBy(x => x).Where(grp => grp.Count() > 1).Select(grp => grp.Key);
                if (duplicatedDoorCodes.Any())
                {
                    message += string.Format("设备中存在重复的门编号:{0}\n", string.Join(",", duplicatedDoorCodes));
                }

                var readingCodes = deviceController.DeviceHeadReadings.Select(x => x.Code);
                var duplicatedReadingCodes = readingCodes.GroupBy(x => x).Where(grp => grp.Count() > 1).Select(grp => grp.Key);
                if (duplicatedReadingCodes.Any())
                {
                    message += string.Format("设备中存在重复的读头编号:{0}\n", string.Join(",", duplicatedReadingCodes));
                }

                if (!string.IsNullOrWhiteSpace(message))
                {
                    SendMessage(message);
                    return;
                }

                if (Id == 0)
                {
                    deviceController.Status = GeneralStatus.Enabled;
                    deviceController.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    deviceController.CreateDate = DateTime.Now;

                    deviceController = _deviceControllerRepo.Insert(deviceController);
                    Id = deviceController.DeviceID;
                    var newViewModel = deviceController.ToViewModel();
                    DeviceExtensionViewModel = newViewModel.DeviceExtensionViewModel;
                    DoorViewModels = newViewModel.DoorViewModels;
                    HeadReadingViewModels = newViewModel.HeadReadingViewModels;
                    Status = newViewModel.Status;
                    CreateUserID = newViewModel.CreateUserID;
                    CreateDate = newViewModel.CreateDate;

                    message = "增加设备成功!";
                }
                else
                {
                    deviceController.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    deviceController.UpdateDate = DateTime.Now;

                    _deviceControllerRepo.Update(deviceController);

                    message = "修改设备成功!";
                }

                ViewModelAttachment.CoreModel = deviceController;
                ViewModelAttachment.LastOperationSuccess = true;
            }
            catch (BusinessException ex)
            {
                Log.Error("Update device fails.", ex);
                SendMessage(ex.Message);
                return;
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
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseDeviceView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceView_ShowNotification);
        }




    }
}
