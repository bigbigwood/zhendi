using System;
using System.Text;
using log4net;
using Riss.Devices;
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

            try
            {
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceBusy);

                extraData = Zd2911Utils.DeviceAccessControlSettings;

                var retryablePolicy = Policies.GetRetryablePolicy();
                result = retryablePolicy.Execute(
                    () => _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.AccessControlSettings, extraProperty, ref device, ref extraData));

                byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
            finally
            {
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceIdle);
            }
        }

        public void UpdateDevice(byte[] data)
        {
            bool result = false;
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();
            try
            {
                extraData = Encoding.Unicode.GetString(data);

                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device,
                    DeviceStatus.DeviceBusy);

                var retryablePolicy = Policies.GetRetryablePolicy();
                result = retryablePolicy.Execute(
                    () =>
                        _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.PassSegment,
                            Zd2911Utils.DevicePassSegment, device, extraData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            finally
            {
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceIdle);
            }
        }
    }
}