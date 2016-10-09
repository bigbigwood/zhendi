using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Unility.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(String message) : base(message) { }
        public BusinessException(String message, Exception ex) : base(message, ex) { }
    }
}
