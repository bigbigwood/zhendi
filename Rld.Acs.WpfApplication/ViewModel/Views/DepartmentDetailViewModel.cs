using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class DepartmentDetailViewModel : ViewModelBase
    {
        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public Boolean IsEdit { get; set; }
        public Int32 ID { get; set; }
        public String DepartmentName { get; set; }
        public String DepartmentCode { get; set; }
        public DeviceRole DeviceRole { get; set; }
        public Int32 StuffCount { get; set; }
        public List<DepartmentDevice> OwnedDevices { get; set; }

        public List<ListBoxItem> DeviceListBoxSource { get; set; }
        public List<DeviceRole> DeviceRoleList { get; set; }
        public List<DeviceController> DeviceList { get; set; }


        public DepartmentDetailViewModel()
        {

        }

        public void BuildDeviceListBox()
        {
            List<ListBoxItem> items = new List<ListBoxItem>();
            foreach (var device in DeviceList)
            {
                items.Add(new ListBoxItem() { ID = device.DeviceID, DisplayName = device.DeviceCode, Description = device.Remark, IsSelected = OwnedDevices.Select(d => d.DeviceID).Contains(device.DeviceID) });
            }

            DeviceListBoxSource = items;
        }
    }
}
