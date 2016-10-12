using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceStateHistory
    {
        public virtual Int32 DeviceStateHistoryID { get; set; }
        public virtual Int32 DeviceID { get; set; }
        public virtual String DeviceCode { get; set; }
        public virtual String DeviceType { get; set; }
        public virtual String DeviceSN { get; set; }
        public virtual Int32? RecordType { get; set; }
        public virtual DateTime? RecordTime { get; set; }
        public virtual Int32 DoorStatus { get; set; }
        public virtual String Remark { get; set; }
    }
}
