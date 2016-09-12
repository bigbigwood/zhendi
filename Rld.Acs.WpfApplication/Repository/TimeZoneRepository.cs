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
    public class TimeZoneRepository : CacheableRepository<Rld.Acs.Model.TimeZone, int>, ITimeZoneRepository
    {
        public TimeZoneRepository()
        {
            RelevantUri = "/api/TimeZones";
            CacheKey = "CacheKey_TimeZones";
            CacheExpireMinutes = SystemCacheExpireMinutes;
        }

        public override bool Update(Rld.Acs.Model.TimeZone timeZone)
        {
            return Update(timeZone, timeZone.TimeZoneID);
        }

        public override Rld.Acs.Model.TimeZone GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.TimeZoneID == key);
        }
    }
}
