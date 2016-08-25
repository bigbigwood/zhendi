using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysRole
    {
        public virtual Int32 RoleID { get; set; }
        public virtual String RoleName { get; set; }
        public virtual String Description { get; set; }
        public virtual String Remark { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual List<SysRolePermission> SysRolePermissions { get; set; }

        public SysRole()
        {
            SysRolePermissions = new List<SysRolePermission>();
        }
    }
}
