using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceControllerParameter
    {
        public virtual Int32 DeviceParameterID { get; set; }
        public virtual Int32 AuthticationType { get; set; }
        public virtual Int32? AutoOpenTimeZone { get; set; }
        public virtual Boolean? IsSneak { get; set; }
        public virtual Boolean? MultiPersonLock { get; set; }
        public virtual Boolean? Linkage { get; set; }
        public virtual Boolean? LaunchDuress { get; set; }
        public virtual Int32? DuressFingerPrint { get; set; }
        public virtual Boolean? DuressOpen { get; set; }
        public virtual Boolean? DuressAlarm { get; set; }
        public virtual String DuressPassword { get; set; }
    }
}
