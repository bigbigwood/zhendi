using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceStateHistory
    {
        public Int32 DeviceStateHistoryID { get; set; }
        public Int32 DeviceID { get; set; }
        public Int32 DeviceType { get; set; }
        public String DeviceSN { get; set; }
        public Int32? RecordType { get; set; }
        public DateTime? RecordTime { get; set; }
        public Int32 DoorStatus { get; set; }
        public String Remark { get; set; }
    }
}
