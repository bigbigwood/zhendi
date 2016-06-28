using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysModule
    {
        public Int32 ModuleID { get; set; }
        public String ModuleName { get; set; }
        public String Description { get; set; }
        public SysModule Parent { get; set; }
        public String LinkURL { get; set; }
        public String FullClassName { get; set; }
        public Int32 ModuleLevel { get; set; }
        public String Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public Int32 CreateUserID { get; set; }
        public GeneralStatus Status { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32? UpdateUserID { get; set; }
    }
}
