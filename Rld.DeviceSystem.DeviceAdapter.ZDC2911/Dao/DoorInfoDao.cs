using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model.Logs;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao
{
    public class DoorInfoDao
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = DeviceProxyManager.GetDeviceProxy();

        public List<DoorStateInfo> GetDoorStates()
        {
            var masterDoorState = new DoorStateInfo() { DoorName = "Master Door" };
            var slaveDoorState = new DoorStateInfo() { DoorName = "Slave Door" };
            var result = new List<DoorStateInfo>() { masterDoorState, slaveDoorState };

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                masterDoorState.Opened = GetDoorStatus(true);
                slaveDoorState.Opened = GetDoorStatus(false);
            }

            return result;
        }

        private bool GetDoorStatus(bool isMasterDoor)
        {
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();
            UInt32 paramIndex = isMasterDoor ? Zd2911Utils.DeviceParamLock1Status : Zd2911Utils.DeviceParamLock2Status; ;

            var retryablePolicy = Policies.GetRetryablePolicy();
            bool result = retryablePolicy.Execute(
                () =>
                {
                    extraData = BitConverter.GetBytes(paramIndex);
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.SysParam, extraProperty, ref device, ref extraData);
                });

            if (result)
            {
                UInt32 paramValue = BitConverter.ToUInt32((byte[])extraData, 0);
                return paramValue == 1;
            }

            return false;
        }
    }
}