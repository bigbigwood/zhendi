using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service
{
    public class DeviceService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;
        public DeviceService(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public Contract.Model.Devices.Device GetDeviceInfo(int deviceId)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public bool ModifyDeviceInfo(Contract.Model.Devices.Device deviceInfo)
        {
            return true;
        }
    }
}