using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class TimeZoneGroup
    {
        public Int32 TimeZoneGroupID { get; set; }
        public TimeZone TimeZone { get; set; }
        public TimeGroup TimeGroup { get; set; }
    }
}
