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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals(obj as DepartmentDevice);
        }

        public bool Equals(DepartmentDevice other)
        {
            if (other == null) return false;
            return DepartmentID.Equals(other.DepartmentID) && DeviceID.Equals(other.DeviceID);
            //return DepartmentDeviceID.Equals(other.DepartmentDeviceID);
        }
    }
}
