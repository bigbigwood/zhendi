using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class User
    {
        public virtual Int32 UserID { get; set; }
        public virtual Int32 DepartmentID { get; set; }
        public virtual UserType Type { get; set; }
        public virtual String UserCode { get; set; }
        public virtual String Name { get; set; }
        public virtual GenderType Gender { get; set; }
        public virtual String Phone { get; set; }
        public virtual String Photo { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual String Remark { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        public virtual UserAuthentication UserAuthenticationInfo { get; set; }
        public virtual UserProperty UserPropertyInfo { get; set; }
    }
}
