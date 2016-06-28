using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysRoleModuleElement
    {
        public Int32 SysRoleElementID { get; set; }
        public SysModuleElement Element { get; set; }
        public SysRole Role { get; set; }
    }
}
