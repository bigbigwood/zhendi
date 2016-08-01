using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Riss.Devices;
using Rld.Acs.Unility;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Configuration;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911
{
    public class DeviceManager
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static DeviceManager _instance = null;
        private readonly Dictionary<Int32, DeviceProxy> _deviceDictionary = null;

        public static void Initialize(List<DeviceConfigurationBase> deviceConfigurations)
        {
            Log.Info("Initializing DeviceManager...");
            _instance = new DeviceManager(deviceConfigurations);
            Log.Info("Initializing DeviceManager Finish...");
        }

        private DeviceManager(IEnumerable<DeviceConfigurationBase> deviceConfigurations)
        {
            _deviceDictionary = new Dictionary<int, DeviceProxy>();
            foreach (var deviceConfig in deviceConfigurations)
            {
                _deviceDictionary.Add(deviceConfig.DeviceId, CreateDeviceProxy(deviceConfig));
            }
        }

        public static DeviceManager GetInstance()
        {
            return _instance;
        }

        private static DeviceProxy CreateDeviceProxy(DeviceConfigurationBase deviceConfig)
        {
            var device = new Device()
            {
                DN = deviceConfig.DeviceId,
                Password = deviceConfig.Password,
                Model = deviceConfig.DeviceModel,
                ConnectionModel = deviceConfig.ConnectionModel,
            };

            if (deviceConfig is DeviceTcpConfiguration)
            {
                var deviceTcpConfig = deviceConfig as DeviceTcpConfiguration;
                device.CommunicationType = CommunicationType.Tcp;
                device.IpAddress = deviceTcpConfig.IpAddress;
                device.IpPort = deviceTcpConfig.Port;
            }

            return new DeviceProxy { Device = device };
        }

        public void Run()
        {
            try
            {
                _deviceDictionary.Values.ForEach(p => p.OpenConnection());
            }
            catch (Exception ex)
            {
                Log.Error("Encounting error while trying to Run DeviceManager", ex);
                throw;
            }
        }

        public void Stop()
        {
            try
            {
                _deviceDictionary.Values.ForEach(p => p.CloseConnection());
            }
            catch (Exception ex)
            {
                Log.Error("Encounting error while trying to Stop DeviceManager", ex);
                throw;
            }
        }

        public DeviceProxy GetDeviceProxy(Int32 key)
        {
            DeviceProxy deviceProxy;
            _deviceDictionary.TryGetValue(key, out deviceProxy);
            return deviceProxy;
        }
    }
}