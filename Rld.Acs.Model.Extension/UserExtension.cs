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
    }
}
