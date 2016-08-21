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

        public String AuthenticationString
        {
            get
            {
                if (AuthenticationType == null || AuthenticationType == 0)
                    return string.Empty;
                else
                {
                    return _authenticationTypeDict[AuthenticationType.Value];
                }
            }
        }

        private static Dictionary<int, string> _authenticationTypeDict = null;

        static DeviceTrafficLogViewModel ()
        {
            var dict = DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.CheckInOptions);
            _authenticationTypeDict = new Dictionary<int, string>();
            _authenticationTypeDict.Add(1, dict.First(d => d.ItemID == 1).ItemValue);
            _authenticationTypeDict.Add(2, dict.First(d => d.ItemID == 2).ItemValue);
            _authenticationTypeDict.Add(4, dict.First(d => d.ItemID == 4).ItemValue);
            _authenticationTypeDict.Add(8, dict.First(d => d.ItemID == 8).ItemValue);
        }
    }
}
