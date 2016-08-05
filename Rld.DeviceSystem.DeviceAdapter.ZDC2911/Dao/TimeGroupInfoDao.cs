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
    public class TimeGroupInfoDao
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;

        public TimeGroupInfoDao(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public byte[] GetTimeGroupData()
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
                    extraData = Zd2911Utils.DevicePassGroup;
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.PassGroup, extraProperty, ref device, ref extraData);
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


        public bool UpdateTimeGroupData(byte[] data)
        {
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            try
            {
                object extraData = Encoding.Unicode.GetString(data);

                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceBusy);

                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(
                    () => _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.PassGroup, Zd2911Utils.DevicePassGroup, device, extraData));

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