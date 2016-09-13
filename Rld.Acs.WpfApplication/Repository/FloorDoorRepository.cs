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
    public class FloorDoorRepository : CacheableRepository<FloorDoor, int>, IFloorDoorRepository
    {
        public FloorDoorRepository()
        {
            RelevantUri = "/api/FloorDoors";
            CacheKey = "CacheKey_FloorDoors";
            CacheExpireMinutes = DepartmentCacheExpireMinutes;
        }

        public override bool Update(FloorDoor floorDoor)
        {
            return Update(floorDoor, floorDoor.FloorDoorID);
        }

        public override FloorDoor GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.FloorDoorID == key);
        }
    }
}
