using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Model.Extension
{
    public static class DeviceControllerExtension
    {
        public static String GetDeviceAssociatedDoorList(this DeviceController deviceController)
        {
            if (deviceController == null || deviceController.DeviceDoors == null || deviceController.DeviceDoors.Count == 0)
                return string.Empty;

            var doorNames = deviceController.DeviceDoors.Select(x => x.Name).ToList();
            return string.Join(", ", doorNames);
        }

        public static String GetDeviceAssociatedHeadReadingList(this DeviceController deviceController)
        {
            if (deviceController == null || deviceController.DeviceHeadReadings == null || deviceController.DeviceHeadReadings.Count == 0)
                return string.Empty;

            var headReadingNames = deviceController.DeviceHeadReadings.Select(x => x.Name).ToList();
            return string.Join(", ", headReadingNames);
        }
    }
}
