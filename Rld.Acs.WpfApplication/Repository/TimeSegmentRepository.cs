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
    public class TimeSegmentRepository : BaseRepository<TimeSegment, int>, ITimeSegmentRepository
    {
        public TimeSegmentRepository()
        {
            RelevantUri = "/api/TimeSegments";
        }

        public override bool Update(TimeSegment timeSegment)
        {
            return Update(timeSegment, timeSegment.TimeSegmentID);
        }
    }
}
