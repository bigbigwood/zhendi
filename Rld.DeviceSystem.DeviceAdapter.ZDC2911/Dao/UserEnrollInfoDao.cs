using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao
{
    public class UserEnrollInfoDao
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = DeviceProxyManager.GetDeviceProxy();

        public Enroll GetEnroll(int userId)
        {
            var device = _deviceProxy.Device;
            var extraProperty = (UInt64)userId;
            var extraData = new object();

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(()
                    =>
                {
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.Enrolls, extraProperty, ref device, ref extraData);
                });

                if (!result)
                {
                    Log.WarnFormat("Cannot get enroll for useid: {0}", userId);
                    return null;
                }

                var rawUser = (User)extraData;
                var rawEnroll = rawUser.Enrolls.First();
                return rawEnroll;
            }
        }
    }
}