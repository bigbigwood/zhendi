using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model;
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

        public DoorStateInfo GetDoorStates(Int32 doorIndex)
        {
            var doorState = new DoorStateInfo() { DoorIndex = doorIndex };
            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                doorState.Opened = GetDoorStatus(doorIndex == 1);
            }

            return doorState;
        }

        public bool UpdateStatus(Int32 doorIndex, DoorControlOption option)
        {
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();
            byte[] data = new byte[8];
            UInt32 paramIndex = Zd2911Utils.DeviceParamLock1Status;
            if (doorIndex == 2)
                paramIndex = Zd2911Utils.DeviceParamLock2Status;

            Array.Copy(BitConverter.GetBytes(paramIndex), 0, data, 0, 4);
            Array.Copy(BitConverter.GetBytes((UInt32)option.GetHashCode()), 0, data, 4, 4);

            var retryablePolicy = Policies.GetRetryablePolicy();
            bool result = retryablePolicy.Execute(
                () =>
                {
                    extraData = data;
                    return _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.SysParam, extraProperty, device, extraData);
                });

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