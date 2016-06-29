using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class TimeGroupSegment
    {
        public virtual Int32 TimeGroupSegmentID { get; set; }
        public virtual TimeGroup TimeGroup { get; set; }
        public virtual TimeSegment TimeSegment { get; set; }
    }
}
