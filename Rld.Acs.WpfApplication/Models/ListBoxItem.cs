using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Models
{
    public class ListBoxItem
    {
        public Int32 ID { get; set; }
        public Boolean IsSelected { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
