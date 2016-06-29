using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class TimeSegment
    {
        public virtual Int32 TimeSegmentID { get; set; }
        public virtual DateTime BeginTime { get; set; }
        public virtual DateTime EndTime { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
    }
}
