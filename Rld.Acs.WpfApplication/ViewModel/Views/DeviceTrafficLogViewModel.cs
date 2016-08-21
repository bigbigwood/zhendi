using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class DeviceTrafficLogViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Int32 TrafficID { get; set; }
        public Int32 DeviceID { get; set; }
        public Int32 DeviceUserID { get; set; }
        public Int32 DeviceType { get; set; }
        public String DeviceSN { get; set; }
        public String RecordType { get; set; }
        public DateTime? RecordTime { get; set; }
        public DateTime? RecordUploadTime { get; set; }
        public Int32? AuthenticationType { get; set; }
        public String Remark { get; set; }


        public DeviceTrafficLogViewModel()
        {

        }
    }
}
