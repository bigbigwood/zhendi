using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Model.Extension
{
    public static class DeviceRoleExtension
    {
        public static String GetDeviceAssociatedDeviceList(this DeviceRole deviceRole, List<DeviceController> devices)
        {
            if (deviceRole == null || deviceRole.DeviceRolePermissions == null || deviceRole.DeviceRolePermissions.Count == 0)
                return string.Empty;

            var deviceNames = deviceRole.DeviceRolePermissions.Select(x => devices.First(d => d.DeviceID== x.DeviceID).Name).ToList();
            return string.Join(", ", deviceNames);
        }

        public static String GetDeviceAssociatedTimezoneList(this DeviceRole deviceRole, List<TimeZone> timeZones)
        {
            if (deviceRole == null || deviceRole.DeviceRolePermissions == null || deviceRole.DeviceRolePermissions.Count == 0)
                return string.Empty;

            var deviceNames = deviceRole.DeviceRolePermissions.Select(x => timeZones.First(d => d.TimeZoneID == x.AllowedAccessTimeZoneID).TimeZoneName).ToList();
            return string.Join(", ", deviceNames);
        }
    }
}
