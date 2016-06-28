using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class User
    {
        public Int32 UserID { get; set; }
        public Department Department { get; set; }
        public UserType Type { get; set; }
        public String UserCode { get; set; }
        public String Name { get; set; }
        public GenderType Gender { get; set; }
        public String Phone { get; set; }
        public String Photo { get; set; }
        public GeneralStatus Status { get; set; }
        public String Remark { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public UserAuthentication UserAuthentication { get; set; }
        public UserProperty UserProperty { get; set; }
    }
}
