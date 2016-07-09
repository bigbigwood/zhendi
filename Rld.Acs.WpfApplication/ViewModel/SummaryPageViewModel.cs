using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Messages;
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
            GotoStuffCommand = new RelayCommand(GotoStuff);
            GotoDepartmentCommand = new RelayCommand(GotoDepartment);
            GotoDeviceCommand = new RelayCommand(GotoDevice);
            GotoDoorCommand = new RelayCommand(GotoDoor);
        }

        public void GotoStuff()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(Tokens.Summary_GotoSutff), Tokens.Summary_GotoSutff);
        }
        public void GotoDepartment()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(Tokens.Summary_GotoDepartment), Tokens.Summary_GotoDepartment);
        }
        public void GotoDevice()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(Tokens.Summary_GotoDevice), Tokens.Summary_GotoDevice);
        }
        public void GotoDoor()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(Tokens.Summary_GotoDoor), Tokens.Summary_GotoDoor);
        }
    }
}
