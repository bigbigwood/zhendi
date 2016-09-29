using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceGroup
    {
        public virtual Int32 DeviceGroupID { get; set; }
        public virtual String DeviceGroupName { get; set; }
        public virtual Int32 CheckInDeviceID { get; set; }
        public virtual Int32 CheckOutDeviceID { get; set; }
    }
}
