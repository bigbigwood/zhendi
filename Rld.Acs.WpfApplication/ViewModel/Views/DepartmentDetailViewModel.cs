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
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;

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
                RaisePropertyChanged("Title");
            }
        }
        private List<ListBoxItem> _deviceListBoxSource;
        public List<ListBoxItem> DeviceListBoxSource
        {
            get { return _deviceListBoxSource; }
            set
            {
                _deviceListBoxSource = value;
                RaisePropertyChanged("DeviceListBoxSource");
            }
        }
        public Department ParentDepartment { get; set; }
        public Department CurrentDepartment { get; set; }
        public DeviceRole DeviceRole { get; set; }
        public List<DepartmentDevice> OwnedDevices { get; set; }
        public List<Department> AuthorizationDepartments { get; set; }
        public List<DeviceController> AuthorizationDevices { get; set; }
        public List<DeviceRole> AuthorizationDeviceRoles { get; set; }

        public DepartmentDetailViewModel()
        {
            PrepareDataCmd = new RelayCommand(PrepareData);
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            ParentDepartment = new Department();
            CurrentDepartment = new Department();
            AuthorizationDepartments = new List<Department>();
            AuthorizationDevices = new List<DeviceController>();
            AuthorizationDeviceRoles = new List<DeviceRole>();
            OwnedDevices = new List<DepartmentDevice>();
            DeviceListBoxSource = new List<ListBoxItem>();
        }

        private void PrepareData()
        {
            var items = new List<ListBoxItem>();
            foreach (var device in AuthorizationDevices)
            {
                items.Add(new ListBoxItem()
                {
                    ID = device.DeviceID,
                    DisplayName = device.DeviceCode,
                    Description = device.Remark,
                    IsSelected = OwnedDevices.Select(d => d.DeviceID).Contains(device.DeviceID)
                });
            }

            DeviceListBoxSource = items;

            if (ID == 0)
            {
                Title = "新增部门";
            }
            else
            {
                Title = "修改部门";
            }
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

                if (ID == 0)
                {
                    CurrentDepartment.Name = DepartmentName;
                    CurrentDepartment.DepartmentCode = DepartmentCode;
                    CurrentDepartment.Status = GeneralStatus.Enabled;
                    CurrentDepartment.Parent = ParentDepartment;
                    CurrentDepartment.DeviceRoleID = DeviceRole.DeviceRoleID;
                    CurrentDepartment.DeviceAssociations = OwnedDevices;
                    CurrentDepartment.CreateUserID = 1;
                    CurrentDepartment.CreateDate = DateTime.Now;

                    _departmentRepository.Insert(CurrentDepartment);
                    message = "增加部门成功!";
                }
                else
                {
                    CurrentDepartment.Name = DepartmentName;
                    CurrentDepartment.DepartmentCode = DepartmentCode;
                    CurrentDepartment.Status = GeneralStatus.Enabled;
                    CurrentDepartment.Parent = ParentDepartment;
                    CurrentDepartment.DeviceRoleID = DeviceRole.DeviceRoleID;
                    CurrentDepartment.DeviceAssociations = OwnedDevices;
                    CurrentDepartment.UpdateUserID = 1;
                    CurrentDepartment.UpdateDate = DateTime.Now;

                    _departmentRepository.Update(CurrentDepartment);
                    message = "修改部门成功!";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update department fails.", ex);
                message = "保存部门失败";
                return;
            }

            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseDepartmentView);
        }
    }
}
