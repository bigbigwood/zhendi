using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rld.Acs.Unility.Extension;
using Rld.DeviceSystem.Contract.Model.Configuration;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Configuration;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Host
{
    public class ConfigurationHelper
    {
        public static DeviceConfigurationAdapter CreateConfiguration()
        {
            var config = new DeviceConfigurationAdapter()
            {
                DeviceId = ConfigurationManager.AppSettings["DeviceId"].ToInt32(),
                Password = ConfigurationManager.AppSettings["Password"],
                DeviceModel = "ZDC2911",
                ConnectionModel = 5,
                DeviceName = "",
                TcpPort = ConfigurationManager.AppSettings["TcpPort"].ToInt32(),
                TcpAddress = ConfigurationManager.AppSettings["TcpAddress"],
                UdpPort = ConfigurationManager.AppSettings["UdpPort"].ToInt32(),
                WebSocketClientConfig = new WebSocketClientConfig() { ServerUrl = ConfigurationManager.AppSettings["WebSocketServerUrl"] },
            };
            return config;
        }
    }
}
