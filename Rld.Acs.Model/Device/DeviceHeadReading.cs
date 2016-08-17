using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceHeadReading
    {
        public virtual Int32 DeviceHeadReadingID { get; set; }
        public virtual Int32 DeviceID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Code { get; set; }
        public virtual String Mac { get; set; }
        public virtual String HeadReadingSN { get; set; }
        public virtual Int32 HeadReadingType { get; set; }
        public virtual String HeadReadingPerformance { get; set; }
        public virtual Int32 Status { get; set; }
    }
}
