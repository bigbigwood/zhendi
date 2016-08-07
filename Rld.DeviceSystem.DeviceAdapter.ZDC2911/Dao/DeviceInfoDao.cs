using System;
using System.Text;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao
{
    public class DeviceInfoDao
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;

        public DeviceInfoDao(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public byte[] GetDeviceData()
        {
            bool result;
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                result = retryablePolicy.Execute(
                    () =>
                    {
                        extraData = Zd2911Utils.DeviceAccessControlSettings;
                        return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.AccessControlSettings, extraProperty,
                            ref device, ref extraData);
                    });

                byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                return data;
            }
        }

        public void UpdateDevice(byte[] data)
        {
            bool result = false;
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = Encoding.Unicode.GetString(data);

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                result = retryablePolicy.Execute(
                    () => result = _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.AccessControlSettings, Zd2911Utils.DeviceAccessControlSettings, device, extraData));
            }
        }
    }
}