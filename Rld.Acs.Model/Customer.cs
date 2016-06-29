using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Model
{
    public class Customer
    {
        public virtual int CustomerId { get; set; }
        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }
        public virtual String MSIDSN { get; set; }
        public virtual decimal Balance { get; set; }
        public virtual DateTime ResigterDateTime { get; set; }
        public virtual String Address { get; set; }
    }
}
