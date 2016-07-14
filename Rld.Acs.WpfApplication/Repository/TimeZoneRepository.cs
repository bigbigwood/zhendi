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
    public class TimeZoneRepository : BaseRepository<Rld.Acs.Model.TimeZone, int>, ITimeZoneRepository
    {
        public TimeZoneRepository()
        {
            RelevantUri = "/api/TimeZones";
        }

        public override bool Update(Rld.Acs.Model.TimeZone timeZone)
        {
            return Update(timeZone, timeZone.TimeZoneID);
        }
    }
}
