using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceTrafficLog
    {
        public Int32 TrafficID { get; set; }
        public Int32 DeviceID { get; set; }
        public Int32 DeviceType { get; set; }
        public String DeviceSN { get; set; }
        public String RecordType { get; set; }
        public DateTime? RecordTime { get; set; }
        public DateTime? RecordUploadTime { get; set; }
        public Int32? AuthenticationType { get; set; }
        public String Remark { get; set; }
    }
}
