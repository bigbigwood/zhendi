using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class TimeGroup
    {
        public Int32 TimeGroupID { get; set; }
        public String TimeGroupName { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
