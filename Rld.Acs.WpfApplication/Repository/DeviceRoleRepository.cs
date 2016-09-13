using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Repository
{
    public class DeviceRoleRepository : CacheableRepository<DeviceRole, int>, IDeviceRoleRepository
    {
        public DeviceRoleRepository()
        {
            RelevantUri = "/api/DeviceRoles";
            CacheKey = "CacheKey_DeviceRoles";
            CacheExpireMinutes = SystemCacheExpireMinutes;
        }

        public override bool Update(DeviceRole deviceRole)
        {
            return Update(deviceRole, deviceRole.DeviceRoleID);
        }

        public override DeviceRole GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.DeviceRoleID == key);
        }
    }
}
