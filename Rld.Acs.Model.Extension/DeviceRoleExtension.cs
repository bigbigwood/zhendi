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

            if (devices == null || devices.Count == 0) 
                return string.Empty;

            var permissions = deviceRole.DeviceRolePermissions.Where(x => devices.Select(d => d.DeviceID).Contains(x.DeviceID));
            var deviceNames = permissions.Select(x => devices.First(d => d.DeviceID == x.DeviceID).Name).Distinct();
            return string.Join(", ", deviceNames);
        }

        public static String GetDeviceAssociatedTimezoneList(this DeviceRole deviceRole, List<TimeZone> timeZones)
        {
            if (deviceRole == null || deviceRole.DeviceRolePermissions == null || deviceRole.DeviceRolePermissions.Count == 0)
                return string.Empty;

            if (timeZones == null || timeZones.Count == 0) 
                return string.Empty;

            var permissions = deviceRole.DeviceRolePermissions.Where(x => timeZones.Select(d => d.TimeZoneID).Contains(x.AllowedAccessTimeZoneID));
            var timezoneNames = permissions.Select(x => timeZones.First(d => d.TimeZoneID == x.AllowedAccessTimeZoneID).TimeZoneName).Distinct();
            return string.Join(", ", timezoneNames);
        }

        public static String GetDeviceAssociatedPermissionActionList(this DeviceRole deviceRole, List<SysDictionary> permissionActionDict)
        {
            if (deviceRole == null || deviceRole.DeviceRolePermissions == null || deviceRole.DeviceRolePermissions.Count == 0)
                return string.Empty;

            if (permissionActionDict == null || permissionActionDict.Count == 0)
                return string.Empty;

            var permissions = deviceRole.DeviceRolePermissions.Where(x => permissionActionDict.Select(d => d.ItemID).Contains((int)x.PermissionAction));
            var permissionActionNames = permissions.Select(x => permissionActionDict.First(d => d.ItemID == (int)x.PermissionAction).ItemValue).Distinct();
            return string.Join(", ", permissionActionNames);
        }

        public static bool HasDeviceAuthorization(this DeviceRole deviceRole, Int32 deviceId)
        {
            if (deviceRole == null || deviceRole.DeviceRolePermissions == null || deviceRole.DeviceRolePermissions.Count == 0)
                return false;

            return deviceRole.DeviceRolePermissions.Any(r => r.DeviceID == deviceId);
        }
    }
}
