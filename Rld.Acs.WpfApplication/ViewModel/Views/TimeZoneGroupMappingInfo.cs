using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.Model;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class TimeZoneGroupMappingInfo : ViewModelBase
    {
        private ITimeGroupRepository _timeGroupRepo = NinjectBinder.GetRepository<ITimeGroupRepository>();
        public Int32 ID { get; set; }
        public string Name { get; set; }
        public Int32 DisplayOrder { get; set; }
        public string SelectedTimeGroupName { get; set; }
        public TimeGroupViewModel TimeGroupViewModel { get; set; }

        public List<TimeGroup> AllTiemGroups
        {
            get { return _timeGroupRepo.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled); }
        }

        public List<string> AllTimeGroupNames
        {
            get { return AllTiemGroups.Select(x => x.TimeGroupName).ToList(); }
        }

        public RelayCommand ComboSelectionChangedCmd { get; private set; }

        public TimeZoneGroupMappingInfo()
        {
            ComboSelectionChangedCmd = new RelayCommand(ComboSelectionChanged);
        }

        private void ComboSelectionChanged()
        {
            var selectedTimeGroup = AllTiemGroups.FirstOrDefault(x => x.TimeGroupName == SelectedTimeGroupName);
            TimeGroupViewModel = new TimeGroupViewModel(selectedTimeGroup);
            RaisePropertyChanged(null);
        }
    }
}
