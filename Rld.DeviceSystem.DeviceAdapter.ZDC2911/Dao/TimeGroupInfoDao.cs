using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao
{
    public class TimeGroupInfoDao
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = DeviceProxyManager.GetDeviceProxy();

        public byte[] GetTimeGroupData()
        {
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(()
                    =>
                {
                    extraData = Zd2911Utils.DevicePassGroup;
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.PassGroup, extraProperty, ref device, ref extraData);
                });

                if (!result)
                {
                    throw new Exception("Get time group fails");
                }

                return Encoding.Unicode.GetBytes((string)extraData);
            }
        }


        public bool UpdateTimeGroupData(byte[] data)
        {
            var device = _deviceProxy.Device;
            object extraData = Encoding.Unicode.GetString(data);

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(
                    () => _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.PassGroup, Zd2911Utils.DevicePassGroup, device, extraData));

                return result;
            }
        }
    }
}