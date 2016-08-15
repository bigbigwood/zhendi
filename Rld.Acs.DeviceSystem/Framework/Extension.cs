using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rld.Acs.Model;

namespace Rld.Acs.DeviceSystem.Framework
{
    public static class Extension
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