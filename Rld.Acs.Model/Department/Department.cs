using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class Department
    {
        public Int32 DepartmentID { get; set; }
        public String Name { get; set; }
        public String DepartmentCode { get; set; }
        public Department Parent { get; set; }
        public DeviceRole DeviceRole { get; set; }
        public String Remark { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }

        List<DepartmentDevice> DepartmentDevices { get; set; }

        public Department()
        {
            DepartmentDevices = new List<DepartmentDevice>();
        }
    }
}
