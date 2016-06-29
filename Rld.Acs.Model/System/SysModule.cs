using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysModule
    {
        public virtual Int32 ModuleID { get; set; }
        public virtual String ModuleName { get; set; }
        public virtual String Description { get; set; }
        public virtual SysModule Parent { get; set; }
        public virtual String LinkURL { get; set; }
        public virtual String FullClassName { get; set; }
        public virtual Int32 ModuleLevel { get; set; }
        public virtual String Remark { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
    }
}
