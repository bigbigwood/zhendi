﻿using System.Collections;
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
        private IUserRepository _userRepo = NinjectBinder.GetRepository<IUserRepository>();
        public RelayCommand GotoStuffCommand { get; private set; }
        public RelayCommand GotoDepartmentCommand { get; private set; }
        public RelayCommand GotoDeviceCommand { get; private set; }
        public RelayCommand GotoDoorCommand { get; private set; }

        public int StuffCount
        {
            get { return _userRepo.QueryUsersCount(new Hashtable()); }
        }

        public int DepartmentCount
        {
            get { return ApplicationManager.GetInstance().AuthorizationDepartments.Count - 1; }
        }

        public int DeviceCount
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices.Count; }
        }

        public int DoorCount
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices.SelectMany(x => x.DeviceDoors).Count(); }
        }

        public SummaryPageViewModel()
        {
            //GotoStuffCommand = new AuthCommand(GotoStuff);
            //GotoDepartmentCommand = new AuthCommand(GotoDepartment);
            //GotoDeviceCommand = new AuthCommand(GotoDevice);
            //GotoDoorCommand = new AuthCommand(GotoDoor);
        }
    }
}
