using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.Service.Validator;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class DepartmentDetailViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDepartmentRepository _departmentRepository = NinjectBinder.GetRepository<IDepartmentRepository>();

        public RelayCommand PrepareDataCmd { get; private set; }
        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public Int32 ID { get; set; }
        public String DepartmentName { get; set; }
        public String DepartmentCode { get; set; }
        public Int32 StuffCount { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }
        private List<ListBoxItem> _deviceListBoxSource;
        public List<ListBoxItem> DeviceListBoxSource
        {
            get { return _deviceListBoxSource; }
            set
            {
                _deviceListBoxSource = value;
                RaisePropertyChanged();
            }
        }
        public Department ParentDepartment { get; set; }
        public Department CurrentDepartment { get; set; }
        public DeviceRole DeviceRole { get; set; }
        public List<DepartmentDevice> OwnedDevices { get; set; }
        public ViewModelAttachment<Department> ViewModelAttachment { get; set; }

        public List<Department> AuthorizationDepartments
        {
            get { return ApplicationManager.GetInstance().AuthorizationDepartments; }
        }

        public List<DeviceController> AuthorizationDevices
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices; }
        }

        public List<DeviceRole> AuthorizationDeviceRoles
        {
            get { return ApplicationManager.GetInstance().AuthorizationDeviceRoles; }
        }

        public DepartmentDetailViewModel()
        {
            PrepareDataCmd = new RelayCommand(PrepareData);
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            ParentDepartment = new Department();
            CurrentDepartment = new Department();
            OwnedDevices = new List<DepartmentDevice>();
            DeviceListBoxSource = new List<ListBoxItem>();
            DeviceRole = AuthorizationDeviceRoles.First();
            ViewModelAttachment = new ViewModelAttachment<Department>();
        }

        private void PrepareData()
        {
            var items = new List<ListBoxItem>();
            foreach (var device in AuthorizationDevices)
            {
                items.Add(new ListBoxItem()
                {
                    ID = device.DeviceID,
                    DisplayName = device.Name,
                    Description = device.Remark,
                    IsSelected = OwnedDevices.Select(d => d.DeviceID).Contains(device.DeviceID)
                });
            }

            DeviceListBoxSource = items;

            Title = (ID == 0)
                ? LanguageManager.GetLocalizationResourceFormat(Resource.MSG_AddObject, Resource.Department)
                : LanguageManager.GetLocalizationResourceFormat(Resource.MSG_ModifyObject, Resource.Department);
        }

        private void UpdateSelectedDevices()
        {
            var selectedDepartmentDevices = new List<DepartmentDevice>();

            var selectedDeviceItem = DeviceListBoxSource.FindAll(d => d.IsSelected);
            foreach (var item in selectedDeviceItem)
            {
                var existDevice = OwnedDevices.FirstOrDefault(d => d.DeviceID == item.ID);
                if (existDevice != null)
                {
                    selectedDepartmentDevices.Add(existDevice);
                }
                else
                {
                    selectedDepartmentDevices.Add(new DepartmentDevice() {DepartmentID = ID, DeviceID = item.ID});
                }
            }

            OwnedDevices = selectedDepartmentDevices;
        }

        private void Save()
        {
            string message = "";
            try
            {
                UpdateSelectedDevices();

                ToDepartment();

                var validator = NinjectBinder.GetValidator<DepartmentValidator>();
                var results = validator.Validate(CurrentDepartment);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

                if (ID == 0)
                {
                    CurrentDepartment.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentDepartment.CreateDate = DateTime.Now;
                    CurrentDepartment = _departmentRepository.Insert(CurrentDepartment);

                    message = LanguageManager.GetLocalizationResourceFormat(Resource.MSG_AddObjectSuccess, Resource.Department);
                    
                }
                else
                {
                    CurrentDepartment.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentDepartment.UpdateDate = DateTime.Now;
                    _departmentRepository.Update(CurrentDepartment);

                    message = LanguageManager.GetLocalizationResourceFormat(Resource.MSG_ModifyObjectSuccess, Resource.Department);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update department fails.", ex);
                message = LanguageManager.GetLocalizationResource(Resource.MSG_SaveFail);
                SendMessage(message);
                return;
            }

            ViewModelAttachment.CoreModel = CurrentDepartment;
            ViewModelAttachment.LastOperationSuccess = true;
            RaisePropertyChanged(null);
            Close(message);
        }

        private void ToDepartment()
        {
            CurrentDepartment.Name = DepartmentName;
            CurrentDepartment.DepartmentCode = DepartmentCode;
            CurrentDepartment.Status = GeneralStatus.Enabled;
            CurrentDepartment.Parent = ParentDepartment;
            CurrentDepartment.DeviceRoleID = DeviceRole.DeviceRoleID;
            CurrentDepartment.DeviceAssociations = OwnedDevices;
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseDepartmentView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.DepartmentView_ShowNotification);
        }
    }
}
