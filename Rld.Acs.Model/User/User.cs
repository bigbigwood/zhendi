using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class User
    {
        public Int32 UserID { get; set; }
        public Int32? DepartmentID { get; set; }
        public Int32 Type { get; set; }
        public String UserCode { get; set; }
        public String Name { get; set; }
        public Int32 Gender { get; set; }
        public String Phone { get; set; }
        public Int32 DeportmentID { get; set; }
        public String Photo { get; set; }
        public Int32 Status { get; set; }
        public String Remark { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
