using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysConfig
    {
        public virtual Int32 ID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Value { get; set; }
        public virtual String Description { get; set; }
        public virtual String Version { get; set; }
    }
}
