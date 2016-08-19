using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Model.Extension
{
    public static class DeviceDoorExtension
    {
        public static Boolean Compare(this DeviceDoor current, DeviceDoor other)
        {
            if (other == null)
                return false;

            return current.DeviceID == other.DeviceID;
        }
    }
}
