using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceTrafficLog
    {
        public virtual Int32 TrafficID { get; set; }
        public virtual Int32 DeviceID { get; set; }
        public virtual Int32 DeviceType { get; set; }
        public virtual String DeviceSN { get; set; }
        public virtual String RecordType { get; set; }
        public virtual DateTime? RecordTime { get; set; }
        public virtual DateTime? RecordUploadTime { get; set; }
        public virtual Int32? AuthenticationType { get; set; }
        public virtual String Remark { get; set; }
    }
}
