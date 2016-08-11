using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911
{
    public class DeviceProxy
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Device Device { get; set; }
        public DeviceConnection DeviceConnection { get; set; }

        public void OpenConnection()
        {
            var retryablePolicy = Policies.GetRetryablePolicy();
            bool result = retryablePolicy.Execute(TryOpenConnection);
            
            if (!result)
            {
                throw new Exception(string.Format("Open connection for device #{0} fails", Device.DN));
            }

            Log.InfoFormat("Open connection for device #{0} successfully", Device.DN);
        }

        private bool TryOpenConnection()
        {
            var myDevice = Device;
            var deviceConnection = DeviceConnection.CreateConnection(ref myDevice);
            if (deviceConnection.Open() > 0)
            {
                DeviceConnection = deviceConnection;
                Device = myDevice;
                return true;
            }
            return false;
        }

        public void CloseConnection()
        {
            DeviceConnection.Close();
            Log.InfoFormat("Close connection for device #{0} successfully", Device.DN);
        }
    }
}