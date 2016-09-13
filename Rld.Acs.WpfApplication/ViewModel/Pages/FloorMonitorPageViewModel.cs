using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.ViewModel.Views;
using GalaSoft.MvvmLight.Threading;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class FloorMonitorPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IFloorRepository _floorRepo = NinjectBinder.GetRepository<IFloorRepository>();
        private UserAvatorService _userAvatorService = new UserAvatorService();

        public Int32 IntervalSeconds { get; set; }

        public ObservableCollection<FloorViewModel> FloorViewModels
        {
            get
            {
                var operators = _floorRepo.Query(new Hashtable());
                var vms = operators.Select(AutoMapper.Mapper.Map<FloorViewModel>);
                var viewmodels = new ObservableCollection<FloorViewModel>(vms);

                viewmodels.ForEach(x =>
                {
                    if (!string.IsNullOrWhiteSpace(x.Photo))
                        x.Photo = _userAvatorService.GetAvator(x.Photo);
                });
                return viewmodels;
            }
        }
        public FloorViewModel SelectedFloorViewModel { get; set; }

        public FloorMonitorPageViewModel()
        {
            IntervalSeconds = 15;
        }
    }
}
