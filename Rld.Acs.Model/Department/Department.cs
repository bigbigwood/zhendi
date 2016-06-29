using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class Department
    {
        public virtual Int32 DepartmentID { get; set; }
        public virtual String Name { get; set; }
        public virtual String DepartmentCode { get; set; }
        public virtual Department Parent { get; set; }
        public virtual DeviceRole DeviceRole { get; set; }
        public virtual String Remark { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
    }
}
