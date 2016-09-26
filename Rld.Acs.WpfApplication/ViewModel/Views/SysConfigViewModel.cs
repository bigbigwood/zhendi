using System;
using System.Linq;
using AutoMapper;
using GalaSoft.MvvmLight;
using log4net;
using Rld.Acs.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models.Messages;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Validator;


namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class SysConfigViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
        public Int32 ID { get; set; }
        public String Name { get; set; }
        private String _value;
        public String Value 
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }
        public String Description { get; set; }
        public String Version { get; set; }
    }
}
