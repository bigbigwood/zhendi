using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class SummaryPageViewModel : ViewModelBase
    {
        public RelayCommand GotoStuffCommand { get; private set; }
        public RelayCommand GotoDepartmentCommand { get; private set; }
        public RelayCommand GotoDeviceCommand { get; private set; }
        public RelayCommand GotoDoorCommand { get; private set; }

        public SummaryPageViewModel()
        {
            //GotoStuffCommand = new RelayCommand(GotoStuff);
            //GotoDepartmentCommand = new RelayCommand(GotoDepartment);
            //GotoDeviceCommand = new RelayCommand(GotoDevice);
            //GotoDoorCommand = new RelayCommand(GotoDoor);
        }
    }
}
