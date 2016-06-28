using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceHeadReading
    {
        public Int32 DeviceHeadReadingID { get; set; }
        public Int32 DeviceID { get; set; }
        public String Mac { get; set; }
        public String HeadReadingSN { get; set; }
        public Int32 HeadReadingType { get; set; }
        public String HeadReadingPerformance { get; set; }
        public Int32 Status { get; set; }
    }
}
