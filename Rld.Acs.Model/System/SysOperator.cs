using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysOperator
    {
        public Int32 OperatorID { get; set; }
        public User User { get; set; }
        public String LoginName { get; set; }
        public String Password { get; set; }
        public Int32 LanguageID { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
