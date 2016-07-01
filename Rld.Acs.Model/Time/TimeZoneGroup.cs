using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class TimeZoneGroup
    {
        public virtual Int32 TimeZoneGroupID { get; set; }
        public virtual Int32 TimeZoneID { get; set; }
        public virtual Int32 TimeGroupID { get; set; }
    }
}
