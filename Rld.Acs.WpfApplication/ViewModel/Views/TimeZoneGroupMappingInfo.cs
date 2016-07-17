using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Rld.Acs.WpfApplication.Models;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class TimeZoneGroupMappingInfo : ViewModelBase
    {
        public Int32 ID { get; set; }
        public string Name { get; set; }
        public Int32 DisplayOrder { get; set; }
        public ComboBoxItem SelectedTimeGroupDto { get; set; }
        public List<ComboBoxItem> AllTimeGroupDtos { get; set; }
    }
}
