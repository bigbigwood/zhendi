using System;
using GalaSoft.MvvmLight;
using log4net;
using Rld.Acs.Model;


namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class SysDictionaryViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
