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
    public class DeviceSystemInfoDao
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;

        public DeviceSystemInfoDao(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public byte[] GetDeviceSystemData()
        {
            var entity = new DeviceSystemEntity();
            var device = _deviceProxy.Device;
            bool result;
            object extraProperty = new object();
            object extraData = new object();

            extraData = Zd2911Utils.DeviceModel;
            result = _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.Model, extraProperty, ref device, ref extraData);
            entity.Model = (string)extraData;

            extraData = Zd2911Utils.DeviceSerialNo;
            result = _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.SerialNo, extraProperty, ref device, ref extraData);
            entity.SerialNumber = (string)extraData;


            //bool result;
            //var device = _deviceProxy.Device;
            //var extraProperty = new object();
            //var extraData = new object();

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
    }
}