using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using log4net;
using Rld.Acs.Model;


namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class DeviceTrafficLogViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Int32 TrafficID { get; set; }
        public Int32 DeviceID { get; set; }
        public Int32 DeviceUserID { get; set; }
        public String DeviceCode { get; set; }
        public String DeviceType { get; set; }
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
            var passwordString = dict.First(d => d.ItemID == 1).ItemValue;
            var card = dict.First(d => d.ItemID == 2).ItemValue;
            var fingerPrint = dict.First(d => d.ItemID == 4).ItemValue;
            var wiegand = dict.First(d => d.ItemID == 8).ItemValue;

            _authenticationTypeDict = new Dictionary<int, string>();
            _authenticationTypeDict.Add((int)CheckInOptions.Password, passwordString);
            _authenticationTypeDict.Add((int)CheckInOptions.Card, card);
            _authenticationTypeDict.Add((int)CheckInOptions.FingerPrint, fingerPrint);
            _authenticationTypeDict.Add((int)CheckInOptions.Wiegand, wiegand);
            _authenticationTypeDict.Add((int)CheckInOptions.Password + (int)CheckInOptions.Card, string.Join(",", new[] { passwordString, card }));
            _authenticationTypeDict.Add((int)CheckInOptions.Password + (int)CheckInOptions.FingerPrint, string.Join(",", new[] { passwordString, fingerPrint }));
            _authenticationTypeDict.Add((int)CheckInOptions.Password + (int)CheckInOptions.Wiegand, string.Join(",", new[] { passwordString, wiegand }));
            _authenticationTypeDict.Add((int)CheckInOptions.Card + (int)CheckInOptions.FingerPrint, string.Join(",", new[] { card, fingerPrint }));
            _authenticationTypeDict.Add((int)CheckInOptions.Card + (int)CheckInOptions.Wiegand, string.Join(",", new[] { card, wiegand }));
            _authenticationTypeDict.Add((int)CheckInOptions.FingerPrint + (int)CheckInOptions.Wiegand, string.Join(",", new[] { fingerPrint, wiegand }));
        }
    }
}
