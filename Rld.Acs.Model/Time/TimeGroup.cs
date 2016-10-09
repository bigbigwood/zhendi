using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class TimeGroup
    {
        public virtual Int32 TimeGroupID { get; set; }
        public virtual String TimeGroupName { get; set; }
        public virtual String TimeGroupCode { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual IList<TimeSegment> TimeSegments { get; set; }

        public TimeGroup()
        {
            TimeSegments = new List<TimeSegment>();
        }
    }
}
