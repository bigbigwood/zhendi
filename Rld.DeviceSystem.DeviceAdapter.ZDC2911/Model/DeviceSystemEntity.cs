using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model
{
    public class DeviceSystemEntity
    {
        public String Model { get; set; }
        public String SerialNumber { get; set; }
        public Int32 DeviceId { get; set; }
        public String Mac { get; set; }
        public String Password { get; set; }
        public String CommunitionType { get; set; }
        public String Protocol { get; set; }
        public Boolean Status { get; set; }
        public String Remark { get; set; }
    }
}