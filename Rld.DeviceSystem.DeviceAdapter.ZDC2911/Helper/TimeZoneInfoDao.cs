using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao
{
    public class TimeZoneInfoDao
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;

        public TimeZoneInfoDao(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public byte[] GetData()
        {
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();
            try
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(()
                    =>
                {
                    extraData = Zd2911Utils.DevicePassZone;
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.PassZone, extraProperty, ref device, ref extraData);
                });

                if (result)
                {
                    byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }


        public bool UpdateData(byte[] data)
        {
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            try
            {
                object extraData = Encoding.Unicode.GetString(data);

                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceBusy);

                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(
                    () => _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.PassZone, Zd2911Utils.DevicePassZone, device, extraData));

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
            finally
            {
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceIdle);
            }
        }

    }
}