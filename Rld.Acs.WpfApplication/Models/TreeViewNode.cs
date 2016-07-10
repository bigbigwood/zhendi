using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Models
{
    public class TreeViewNode
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public IList<TreeViewNode> Children { get; set; }

        public TreeViewNode()
        {
            Children = new List<TreeViewNode>();
        }
    }
}
