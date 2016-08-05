using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model.Services.DeviceControlling;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service
{
    public class AccessControllingService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;
        public AccessControllingService(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public Contract.Model.Services.DeviceControlling.DeviceService GetDeviceService()
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

                if (result)
                {
                    byte[] data = Encoding.Unicode.GetBytes((string)extraData);

                    var deviceService = new Contract.Model.Services.DeviceControlling.DeviceService();
                    deviceService.AntiPassbackEnabled = (data[3] == 1);
                    deviceService.AutoOpenTimeZoneId = data[27];

                    var duressService = new DuressService();
                    duressService.Enabled = (data[6] == 1);
                    duressService.FingerPrintIndex = data[5];
                    duressService.OpenDoor = (data[1] == 1);
                    duressService.Alarm = (data[2] == 1);
                    StringBuilder pwd = new StringBuilder();
                    for (int i = 0; i < Zd2911Utils.PasswordLength; i++)
                    {
                        pwd.Append((char)data[8 + i]);
                    }
                    duressService.Password = pwd.ToString();
                    deviceService.Services.Add(duressService);

                    var multiPersionLockService = new MultiPersionLockService();
                    multiPersionLockService.Enabled = (data[24] == 1);
                    deviceService.Services.Add(multiPersionLockService);

                    var linkageService = new DoorLinkageService();
                    linkageService.OpenDoorAction = data[25];
                    linkageService.AlarmAction = data[26];
                    deviceService.Services.Add(linkageService);

                    var masterDoor = new DoorService();
                    masterDoor.OpenDelaySeconds = data[16];
                    masterDoor.AlarmService.OverTimeOpenAlarmSeconds = data[18];
                    masterDoor.AlarmService.IllegalOpenAlarmSeconds = data[19];
                    deviceService.Services.Add(masterDoor);

                    var slaveDoor = new DoorService();
                    slaveDoor.OpenDelaySeconds = data[20];
                    slaveDoor.AlarmService.OverTimeOpenAlarmSeconds = data[22];
                    slaveDoor.AlarmService.IllegalOpenAlarmSeconds = data[23];
                    deviceService.Services.Add(slaveDoor);

                    return deviceService;
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
            finally
            {
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceIdle);
            }
        }

        public bool ModifyAccessControllingService(Contract.Model.Services.DeviceControlling.DeviceService service)
        {
            return true;
        }
    }
}