using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class UserRole
    {
        public virtual Int32 SysUserRoleID { get; set; }
        public virtual User User { get; set; }
        public virtual SysRole SysRole { get; set; }
    }
}
