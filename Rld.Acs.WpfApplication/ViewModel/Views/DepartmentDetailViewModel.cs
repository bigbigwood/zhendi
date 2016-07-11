using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class DepartmentDetailViewModel : ViewModelBase
    {
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
        public DeviceRole DeviceRole { get; set; }
        public List<DepartmentDevice> OwnedDevices { get; set; }
        public List<DeviceController> AuthorizationDevices { get; set; }
        public List<DeviceRole> AuthorizationDeviceRoles { get; set; }

        public DepartmentDetailViewModel()
        {
            PrepareDataCmd = new RelayCommand(PrepareData);
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(Close);

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
            Title = (ID == 0) ? "新增部门" : "修改部门";
        }

        private void Save()
        {
            Close();
        }

        private void Close()
        {
            Messenger.Default.Send(new NotificationMessage(Tokens.CloseDepartmentView), Tokens.CloseDepartmentView);
        }
    }
}
