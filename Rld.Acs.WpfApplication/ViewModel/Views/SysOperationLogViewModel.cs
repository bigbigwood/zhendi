using System;
using GalaSoft.MvvmLight;
using log4net;


namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class SysOperationLogViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
