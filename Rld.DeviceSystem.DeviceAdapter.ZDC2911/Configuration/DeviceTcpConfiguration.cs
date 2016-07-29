using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Configuration
{
    public class DeviceTcpConfiguration : DeviceConfigurationBase
    {
        /// <summary>
        /// Port
        /// </summary>
        [ConfigurationProperty("Port", IsRequired = true)]
        public Int32 Port
        {
            get { return (Int32)this["Port"]; }
            set { this["Port"] = value; }
        }

        /// <summary>
        /// IpAddress
        /// </summary>
        [ConfigurationProperty("IpAddress", IsRequired = true)]
        public String IpAddress
        {
            get { return (String)this["IpAddress"]; }
            set { this["IpAddress"] = value; }
        }
    }
}