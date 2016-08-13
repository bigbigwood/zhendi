using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riss.Devices;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Configuration;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework
{
    internal static class DeviceProxyManager
    {
        [ThreadStatic]
        private static DeviceProxy _deviceProxy;

        public static DeviceProxy GetDeviceProxy()
        {
            if (_deviceProxy == null)
            {
                throw new Exception("DeviceProxy is null, probably device proxy is not Bind before.");
            }

            return _deviceProxy;
        }

        public static void Bind(DeviceProxy deviceProxy)
        {
            _deviceProxy = deviceProxy;
        }
    }
}
