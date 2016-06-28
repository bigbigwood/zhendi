using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceControllerParameter
    {
        public Int32 DeviceParameterID { get; set; }
        public Int32 AuthticationType { get; set; }
        public Int32? AutoOpenTimeZone { get; set; }
        public Boolean? IsSneak { get; set; }
        public Boolean? MultiPersonLock { get; set; }
        public Boolean? Linkage { get; set; }
        public Boolean? LaunchDuress { get; set; }
        public Int32? DuressFingerPrint { get; set; }
        public Boolean? DuressOpen { get; set; }
        public Boolean? DuressAlarm { get; set; }
        public String DuressPassword { get; set; }
    }
}
