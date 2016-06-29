using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysOperationLog
    {
        public virtual Int32 LogID { get; set; }
        public virtual Int32? DepartmentID { get; set; }
        public virtual Int32? UserID { get; set; }
        public virtual String UserName { get; set; }
        public virtual String OperationCode { get; set; }
        public virtual String OperationName { get; set; }
        public virtual String Detail { get; set; }
        public virtual String Remark { get; set; }
        public virtual DateTime? CreateDate { get; set; }
    }
}
