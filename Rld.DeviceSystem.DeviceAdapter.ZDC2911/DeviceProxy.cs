using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Riss.Devices;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911
{
    public class DeviceProxy
    {
        public Device Device { get; set; }
        public DeviceConnection DeviceConnection { get; set; }

        public DeviceProxy(Device device, DeviceConnection deviceConnection)
        {
            Device = device;
            DeviceConnection = deviceConnection;
        }
    }
}