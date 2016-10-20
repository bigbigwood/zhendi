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
        private DeviceProxy _deviceProxy = DeviceProxyManager.GetDeviceProxy();

        public IEnumerable<User> GetAllUsers(bool isAlsoGetName)
        {
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();
            var users = new List<User>();

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(
                    () =>
                    {
                        extraProperty= (UInt64)0;
                        return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.Enrolls, extraProperty, ref device, ref extraData);
                    });

                if (result)
                {
                    users = (List<User>) extraData;
                }

                if (isAlsoGetName)
                {
                    var oneRetryPolicy = Policies.GetOneRetryPolicy();
                    users.ForEach(x =>
                    {
                        var user = new User() {DIN = x.DIN};
                        oneRetryPolicy.Execute(
                            () => _deviceProxy.DeviceConnection.GetProperty(UserProperty.UserName, extraProperty, ref user, ref extraData));
                        x.UserName = user.UserName;
                    });
                }

                return users;
            }
        }

        public User GetUser(int userId)
        {
            var deviceUser = new User() { DIN = (UInt64)userId };
            object extraData = new object();

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                var retryablePolicy = Policies.GetOneRetryPolicy();
                bool result = retryablePolicy.Execute(
                    () => _deviceProxy.DeviceConnection.GetProperty(UserProperty.Enroll, null, ref deviceUser, ref extraData));
                return result ? deviceUser : null;
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