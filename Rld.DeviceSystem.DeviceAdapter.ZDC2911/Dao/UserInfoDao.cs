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
    public class UserInfoDao
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;

        public UserInfoDao(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public User GetUser(int userId)
        {
            var deviceUser = new User() { DIN = (UInt64)userId };
            object extraData = new object();

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(()
                    =>
                {
                    return _deviceProxy.DeviceConnection.GetProperty(UserProperty.Enroll, null, ref deviceUser, ref extraData);
                });

                if (!result)
                {
                    throw new Exception("Get user fails");
                }

                return deviceUser;
            }
        }

        public bool SaveOrUpdateUser(User deviceUser)
        {
            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(
                    () => _deviceProxy.DeviceConnection.SetProperty(UserProperty.Enroll, new object(), deviceUser, false));

                return result;
            }
        }

        public bool DeleteUser(int userId)
        {
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(
                    () =>
                    {
                        extraData = (UInt64)userId;
                       return _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enrolls, extraProperty, device, extraData);
                    });

                return result;
            }
        }
    }
}