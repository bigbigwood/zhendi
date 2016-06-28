using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class Operator
    {
        public Int32 OperatorID { get; set; }
        public Int32 UserID { get; set; }
        public String LoginName { get; set; }
        public String Password { get; set; }
        public Int32 LanguageID { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public Int32 Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
