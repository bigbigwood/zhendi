using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class TimeGroupSegment
    {
        public Int32 TimeGroupSegmentID { get; set; }
        public TimeGroup TimeGroup { get; set; }
        public TimeSegment TimeSegment { get; set; }
    }
}
