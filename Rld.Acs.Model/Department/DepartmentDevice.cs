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
    }
}
