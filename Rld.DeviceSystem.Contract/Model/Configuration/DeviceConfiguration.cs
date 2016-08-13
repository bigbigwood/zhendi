using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Contract.Model.Configuration
{
    public class DeviceConfiguration
    {
        public Int32 DeviceId { get; set; }
        public String Password { get; set; }
        public String DeviceModel { get; set; }
        public Int32 ConnectionModel { get; set; }
        public WebSocketClientConfig WebSocketClientConfig { get; set; }
    }
}
