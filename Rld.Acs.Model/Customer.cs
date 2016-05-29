using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Model
{
    public class Customer
    {
        public Int64 CustomerId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String MSIDSN { get; set; }
        public decimal Balance { get; set; }
        public DateTime ResigterDateTime { get; set; }
        public String Address { get; set; }
    }
}
