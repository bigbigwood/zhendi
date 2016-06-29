using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysRoleModuleElement
    {
        public virtual Int32 SysRoleElementID { get; set; }
        public virtual SysModuleElement Element { get; set; }
        public virtual SysRole Role { get; set; }
    }
}
