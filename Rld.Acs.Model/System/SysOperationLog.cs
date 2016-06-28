using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysOperationLog
    {
        public Int32 LogID { get; set; }
        public Int32? DepartmentID { get; set; }
        public Int32? UserID { get; set; }
        public String UserName { get; set; }
        public String OperationCode { get; set; }
        public String OperationName { get; set; }
        public String Detail { get; set; }
        public String Remark { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
