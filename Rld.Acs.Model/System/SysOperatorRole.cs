using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysOperatorRole
    {
        public virtual Int32 SysOperatorRoleID { get; set; }
        public virtual Int32 OperatorID { get; set; }
        public virtual Int32 RoleID { get; set; }
    }
}
