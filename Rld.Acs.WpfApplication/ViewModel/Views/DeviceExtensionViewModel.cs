using System;
using System.Collections.ObjectModel;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Validator;
using Rld.Acs.WpfApplication.ViewModel.Converter;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class DeviceExtensionViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public virtual Int32 DeviceParameterID { get; set; }
        public virtual Int32 AuthticationType { get; set; }
        public virtual Int32? UnlockOpenTimeZone { get; set; }
        public virtual Boolean AntiPassbackEnabled { get; set; }
        public virtual Boolean MultiPersonLock { get; set; }
        public virtual Boolean DoorLinkageEnabled { get; set; }
        public virtual Boolean DuressEnabled { get; set; }
        public virtual Int32 DuressFingerPrintIndex { get; set; }
        public virtual Boolean DuressOpen { get; set; }
        public virtual Boolean DuressAlarm { get; set; }
        public virtual String DuressPassword { get; set; }
    }
}
