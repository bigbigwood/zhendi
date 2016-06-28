using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class UserRole
    {
        public Int32 SysUserRoleID { get; set; }
        public User User { get; set; }
        public SysRole SysRole { get; set; }
    }
}
