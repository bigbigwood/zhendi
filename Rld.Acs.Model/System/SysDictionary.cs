using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysDictionary
    {
        public virtual Int32 DictionaryID { get; set; }
        public virtual String Name { get; set; }
        public virtual Int32? TypeID { get; set; }
        public virtual String TypeName { get; set; }
        public virtual Int32? ParentID { get; set; }
        public virtual Int32? LanguageID { get; set; }
        public virtual Int32? Level { get; set; }
        public virtual Int32? ItemID { get; set; }
        public virtual String ItemProperty { get; set; }
        public virtual String ItemValue { get; set; }
        public virtual String Description { get; set; }
        public virtual String Remark { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
    }
}
