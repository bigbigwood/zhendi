using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Model.Extension
{
    public static class UserExtension
    {
        public static List<Int32> GetUserAccessableDeviceIds(this User user)
        {
            if (user == null || user.UserAuthentications == null || user.UserAuthentications.Count == 0)
                return new List<int>();

            var deviceIds = user.UserAuthentications.Select(a => a.DeviceID).Distinct().ToList();
            return deviceIds;
        }

        public static Int32 GetDeviceUserId(this User user, DeviceController device)
        {
            const int falseDeviceUserID = 0;

            if (user == null || user.UserAuthentications == null || user.UserAuthentications.Count == 0)
                return falseDeviceUserID;

            if (device == null || device.DeviceID == 0)
                return falseDeviceUserID;

            var userAuthenticationsOfDevice = user.UserAuthentications.Where(a => a.DeviceID == device.DeviceID);
            if (userAuthenticationsOfDevice.Any())
            {
                return userAuthenticationsOfDevice.First().DeviceUserID;
            }

            return falseDeviceUserID;
        }

        public static IEnumerable<DeviceRolePermission> GetUserDeviceRoleAuthorizedPermissions(this User user, List<DeviceRole> deviceRole)
        {
            if (user == null || user.UserDeviceRoles == null || user.UserDeviceRoles.Count == 0)
                return null;

            var userDeviceRoles = deviceRole.FindAll(d => user.UserDeviceRoles.Select(r => r.DeviceRoleID).Contains(d.DeviceRoleID));
            return userDeviceRoles.SelectMany(r => r.DeviceRolePermissions);
        }

        public static DeviceRolePermission GetUserDeviceRoleAuthorizedPermissionByDeviceId(this User user, Int32 deviceId, List<DeviceRole> deviceRole)
        {
            var userDevicePermissions = GetUserDeviceRoleAuthorizedPermissions(user, deviceRole);
            return userDevicePermissions.Where(p => p.DeviceID == deviceId).OrderByDescending(p => p.PermissionAction).FirstOrDefault();
        }


        public static List<Int32> GetUserRoleAuthorizedDeviceIds(this User user, List<DeviceRole> deviceRole)
        {
            var userDevicePermissions = GetUserDeviceRoleAuthorizedPermissions(user, deviceRole);
            return userDevicePermissions.Select(p => p.DeviceID).Distinct().ToList();
        }
    }
}
