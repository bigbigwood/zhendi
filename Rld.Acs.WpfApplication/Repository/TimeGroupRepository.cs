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
    public class TimeGroupRepository : BaseRepository<TimeGroup, int>, ITimeGroupRepository
    {
        public TimeGroupRepository()
        {
            RelevantUri = "/api/TimeGroups";
        }

        public override bool Update(TimeGroup timeGroup)
        {
            return Update(timeGroup, timeGroup.TimeGroupID);
        }
    }
}
