using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class TimeZoneGroup
    {
        public virtual Int32 TimeZoneGroupID { get; set; }
        public virtual TimeZone TimeZone { get; set; }
        public virtual TimeGroup TimeGroup { get; set; }
    }
}
