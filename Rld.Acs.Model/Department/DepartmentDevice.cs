using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DepartmentDevice
    {
        public virtual Int32 DepartmentDeviceID { get; set; }
        public virtual Int32 DepartmentID { get; set; }
        public virtual Int32 DeviceID { get; set; }
        public virtual String Remark { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
    }
}
