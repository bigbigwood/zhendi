using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysRole
    {
        public Int32 RoleID { get; set; }
        public String RoleName { get; set; }
        public String Description { get; set; }
        public String Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public Int32 CreateUserID { get; set; }
        public GeneralStatus Status { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32? UpdateUserID { get; set; }
    }
}
