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
    public class FloorRepository : CacheableRepository<Floor, int>, IFloorRepository
    {
        public FloorRepository()
        {
            RelevantUri = "/api/Floors";
            CacheKey = "CacheKey_Floors";
            CacheExpireMinutes = DepartmentCacheExpireMinutes;
        }

        public override bool Update(Floor floor)
        {
            return Update(floor, floor.FloorID);
        }


        public override Floor GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.FloorID == key);
        }
    }
}
