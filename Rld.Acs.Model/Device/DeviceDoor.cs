using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceDoor
    {
        public virtual Int32 DeviceDoorID { get; set; }
        public virtual Int32 DeviceID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Code { get; set; }
        public virtual Int32? ElectricalAppliances { get; set; }
        public virtual Int32? CheckOutOptions { get; set; }
        public virtual Int32? Status { get; set; }
        public virtual String Remark { get; set; }
        public virtual Int32? RingType { get; set; }
        public virtual Int32 DelayOpenSeconds { get; set; }
        public virtual Int32 OverTimeOpenSeconds { get; set; }
        public virtual Int32 IllegalOpenSeconds { get; set; }
        public virtual Boolean LinkageAlarm { get; set; }
        public virtual Boolean DuressEnabled { get; set; }
        public virtual Int32 DuressFingerPrintIndex { get; set; }
        public virtual Boolean DuressOpen { get; set; }
        public virtual Boolean DuressAlarm { get; set; }
        public virtual String DuressPassword { get; set; }
    }
}
