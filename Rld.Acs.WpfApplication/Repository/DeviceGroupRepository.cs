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
    public class DeviceGroupRepository : CacheableRepository<DeviceGroup, int>, IDeviceGroupRepository
    {
        public DeviceGroupRepository()
        {
            RelevantUri = "/api/DeviceGroups";
            CacheKey = "CacheKey_DeviceGroups";
            CacheExpireMinutes = DepartmentCacheExpireMinutes;
        }

        public override bool Update(DeviceGroup deviceGroup)
        {
            return Update(deviceGroup, deviceGroup.DeviceGroupID);
        }

        public override DeviceGroup GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.DeviceGroupID == key);
        }
    }
}
