using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysOperator
    {
        public virtual Int32 OperatorID { get; set; }
        public virtual Int32? UserID { get; set; }
        public virtual String LoginName { get; set; }
        public virtual String Password { get; set; }
        public virtual String Salt { get; set; }
        public virtual Int32 LanguageID { get; set; }
        public virtual String Photo { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
    }
}
