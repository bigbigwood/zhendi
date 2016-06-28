using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysDictionary
    {
        public Int32 DictionaryID { get; set; }
        public String Name { get; set; }
        public Int32? TypeID { get; set; }
        public String TypeName { get; set; }
        public Int32? ParentID { get; set; }
        public Int32? LanguageID { get; set; }
        public Int32? Level { get; set; }
        public Int32? ItemID { get; set; }
        public String ItemProperty { get; set; }
        public String ItemValue { get; set; }
        public String Description { get; set; }
        public String Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public Int32 CreateUserID { get; set; }
        public Int32 Status { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32? UpdateUserID { get; set; }
    }
}
