using System;
using System.Configuration;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Configuration
{
    public class DeviceConfigurationBase : ConfigurationSection
    {
        /// <summary>
        /// DeviceId
        /// </summary>
        [ConfigurationProperty("DeviceId", IsRequired = true)]
        public Int32 DeviceId
        {
            get { return (Int32)this["DeviceId"]; }
            set { this["DeviceId"] = value; }
        }

        /// <summary>
        /// DeviceName
        /// </summary>
        [ConfigurationProperty("DeviceName", IsRequired = true)]
        public String DeviceName
        {
            get { return (String)this["DeviceName"]; }
            set { this["DeviceName"] = value; }
        }

        /// <summary>
        /// Password
        /// </summary>
        [ConfigurationProperty("Password", IsRequired = true)]
        public String Password
        {
            get { return (String)this["Password"]; }
            set { this["Password"] = value; }
        }

        /// <summary>
        /// DeviceModel
        /// </summary>
        [ConfigurationProperty("DeviceModel", IsRequired = true)]
        public String DeviceModel
        {
            get { return (String)this["DeviceModel"]; }
            set { this["DeviceModel"] = value; }
        }

        /// <summary>
        /// ConnectionModel
        /// </summary>
        [ConfigurationProperty("ConnectionModel", IsRequired = true)]
        public Int32 ConnectionModel
        {
            get { return (Int32)this["ConnectionModel"]; }
            set { this["ConnectionModel"] = value; }
        }
    }
}