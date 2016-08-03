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

        public bool OpenConnection()
        {
            var myDevice = Device;
            var deviceConnection = DeviceConnection.CreateConnection(ref myDevice);
            if (deviceConnection.Open() > 0)
            {
                DeviceConnection = deviceConnection;
                Device = myDevice;
                return true;
            }
            else
            {
                throw new Exception(string.Format("Open connection for device #{0} fails", Device.DN));
            }
        }

        public bool CloseConnection()
        {
            DeviceConnection.Close();
            return true;
        }
    }
}