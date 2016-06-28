using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysModuleElement
    {
        public Int32 ElementID { get; set; }
        public Int32 ModuleID { get; set; }
        public Boolean Visible { get; set; }
        public Boolean Enabled { get; set; }
        public String Description { get; set; }
        public String Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public Int32 CreateUserID { get; set; }
        public Int32 Status { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32? UpdateUserID { get; set; }
    }
}
