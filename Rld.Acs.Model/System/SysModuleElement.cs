using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysModuleElement
    {
        public virtual Int32 ElementID { get; set; }
        public virtual SysModule Module { get; set; }
        public virtual Boolean Visible { get; set; }
        public virtual Boolean Enabled { get; set; }
        public virtual String Description { get; set; }
        public virtual String Remark { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
    }
}
