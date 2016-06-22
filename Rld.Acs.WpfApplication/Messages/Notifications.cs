using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Messages
{
    public class Notifications
    {
        public static readonly string OpenNewCustomerView = Guid.NewGuid().ToString();
        public static readonly string OpenModifyCustomerView = Guid.NewGuid().ToString();
    }
}
