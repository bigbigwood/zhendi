using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DepartmentDevice
    {
        public Int32 DepartmentDeviceID { get; set; }
        public Department Department { get; set; }
        public DeviceController Device { get; set; }
        public String Remark { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
