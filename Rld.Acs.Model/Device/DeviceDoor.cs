using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceDoor
    {
        public virtual Int32 DeviceDoorID { get; set; }
        public virtual DeviceController Device { get; set; }
        public virtual String Name { get; set; }
        public virtual Int32? ElectricalAppliances { get; set; }
        public virtual Int32? OpenType { get; set; }
        public virtual Int32? Status { get; set; }
        public virtual String Remark { get; set; }
        public virtual Int32? DelayTime { get; set; }
        public virtual Int32? AlertType { get; set; }
        public virtual Int32? OverTimeOpen { get; set; }
        public virtual Boolean? IsOverTime { get; set; }
        public virtual Boolean? ForceOpen { get; set; }
        public virtual Boolean? ConnectionAlerm { get; set; }
        public virtual Boolean? LaunchDuress { get; set; }
        public virtual Int32? DuressFingerPrint { get; set; }
        public virtual Boolean? DuressOpen { get; set; }
        public virtual Boolean? DuressAlarm { get; set; }
        public virtual String DuressPassword { get; set; }
    }
}
