using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Riss.Devices;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework
{
    public class DeviceLockableOperation : IDisposable
    {
        public Device _device { get; set; }
        public DeviceConnection _deviceConnection { get; set; }

        public DeviceLockableOperation(DeviceProxy deviceProxy)
        {
            _device = deviceProxy.Device;
            _deviceConnection = deviceProxy.DeviceConnection;
            LockDevice();
        }

        public void Dispose()
        {
            UnlockDevice();
        }

        private void LockDevice()
        {
            var extraProperty = new object();
            _deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, _device, DeviceStatus.DeviceBusy);
        }

        private void UnlockDevice()
        {
            var extraProperty = new object();
            _deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, _device, DeviceStatus.DeviceIdle);
            
        }
    }
}