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
        public virtual Int32 DeviceRoleID { get; set; }
        public virtual String Remark { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public IList<DepartmentDevice> DeviceAssociations { get; set; }

        public Department()
        {
            DeviceAssociations = new List<DepartmentDevice>();
        }

        public object TimeGroups { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals(obj as Department);
        }

        public bool Equals(Department other)
        {
            if (other == null) return false;
            return DepartmentID == other.DepartmentID;
        }

        public static bool operator !=(Department department1, Department department2)
        {
            if (department1 == null) return false;

            return !department1.Equals(department2);
        }

        public static bool operator ==(Department department1, Department department2)
        {
            if (department1 == null) return false;

            return department1.Equals(department2);
        }
    }
}
