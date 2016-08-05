using System;
using log4net;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper
{
    public class DeviceSystemInfoMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;
        public DeviceSystemInfoMapper(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public DeviceSystemInfo GetDeviceInfo(int deviceId)
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

        public bool ModifyDeviceInfo(DeviceSystemInfo deviceInfo)
        {
            return true;
        }
    }
}