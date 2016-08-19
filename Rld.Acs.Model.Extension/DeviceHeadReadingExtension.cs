using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Model.Extension
{
    public static class DeviceHeadReadingExtension
    {
        public static Boolean Compare(this DeviceHeadReading current, DeviceHeadReading other)
        {
            if (other == null)
                return false;

            return current.DeviceHeadReadingID == other.DeviceHeadReadingID;
        }
    }
}
