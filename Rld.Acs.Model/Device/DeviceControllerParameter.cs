using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceControllerParameter
    {
        public virtual Int32 DeviceParameterID { get; set; }
        public virtual Int32 AuthticationType { get; set; }
        public virtual Int32? UnlockOpenTimeZone { get; set; }
        public virtual Boolean AntiPassbackEnabled { get; set; }
        public virtual Boolean MultiPersonLock { get; set; }
        public virtual Boolean DoorLinkageEnabled { get; set; }
        public virtual Boolean DuressEnabled { get; set; }
        public virtual Int32 DuressFingerPrintIndex { get; set; }
        public virtual Boolean DuressOpen { get; set; }
        public virtual Boolean DuressAlarm { get; set; }
        public virtual String DuressPassword { get; set; }
    }
}
