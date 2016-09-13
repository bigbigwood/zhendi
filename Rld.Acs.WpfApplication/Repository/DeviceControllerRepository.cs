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
    public class DeviceControllerRepository : CacheableRepository<DeviceController, int>, IDeviceControllerRepository
    {
        public DeviceControllerRepository()
        {
            RelevantUri = "/api/Devices";
            CacheKey = "CacheKey_Devices";
            CacheExpireMinutes = DepartmentCacheExpireMinutes;
        }

        public override bool Update(DeviceController deviceController)
        {
            return Update(deviceController, deviceController.DeviceID);
        }

        public override DeviceController GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.DeviceID == key);
        }
    }
}
