using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rld.DeviceSystem.Contract.Model.Configuration;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Configuration
{
    public class DeviceConfigurationAdapter : DeviceConfiguration
    {
        public String DeviceName { get; set; }
        public Int32 TcpPort { get; set; }
        public String TcpAddress { get; set; }
        public Int32 UdpPort { get; set; }
    }
}
