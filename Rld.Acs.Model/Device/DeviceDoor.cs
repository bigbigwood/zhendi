using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceDoor
    {
        public Int32 DeviceDoorID { get; set; }
        public Int32 DeviceID { get; set; }
        public String Name { get; set; }
        public Int32? ElectricalAppliances { get; set; }
        public Int32? OpenType { get; set; }
        public Int32? Status { get; set; }
        public String Remark { get; set; }
        public Int32? DelayTime { get; set; }
        public Int32? AlertType { get; set; }
        public Int32? OverTimeOpen { get; set; }
        public Boolean? IsOverTime { get; set; }
        public Boolean? ForceOpen { get; set; }
        public Boolean? ConnectionAlerm { get; set; }
        public Boolean? LaunchDuress { get; set; }
        public Int32? DuressFingerPrint { get; set; }
        public Boolean? DuressOpen { get; set; }
        public Boolean? DuressAlarm { get; set; }
        public String DuressPassword { get; set; }
    }
}
