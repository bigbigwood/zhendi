using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class TimeZone
    {
        public virtual Int32 TimeZoneID { get; set; }
        public virtual String TimeZoneName { get; set; }
        public virtual String TimeZoneCode { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual IList<TimeZoneGroup> TimeGroupAssociations { get; set; }

        public TimeZone()
        {
            TimeGroupAssociations = new List<TimeZoneGroup>();
        }
    }
}
