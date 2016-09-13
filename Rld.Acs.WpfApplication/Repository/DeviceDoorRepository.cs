using System.Collections;
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
    public class DeviceDoorRepository : CacheableRepository<DeviceDoor, int>, IDeviceDoorRepository
    {
        public DeviceDoorRepository()
        {
            RelevantUri = "/api/DeviceDoors";
            CacheKey = "CacheKey_DeviceDoors";
            CacheExpireMinutes = DepartmentCacheExpireMinutes;
        }

        public override bool Update(DeviceDoor deviceDoor)
        {
            return Update(deviceDoor, deviceDoor.DeviceDoorID);
        }

        public override DeviceDoor GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.DeviceDoorID == key);
        }
    }
}
