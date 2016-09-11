using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public enum ViewModelOperation
    {
        Create,
        Update,
        Delete,
        Query
    }

    public class ViewModelAttachment<T> where T : class 
    {
        public Boolean LastOperationSuccess { get; set; }
        public ViewModelOperation OperationType { get; set; }
        public T CoreModel { get; set; }
    }
}
