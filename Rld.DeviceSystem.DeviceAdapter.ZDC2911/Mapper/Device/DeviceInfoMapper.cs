using log4net;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device
{
    public class DeviceInfoMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static DeviceInfo ToModel(byte[] deviceData)
        {
            var deviceService = new DeviceInfo();
            deviceService.AntiPassbackEnabled = (deviceData[3] == 1);

            deviceService.Services.Add(DoorOpenBehaviorServiceMapper.BuildUnlockOpenService(deviceData[27]));
            deviceService.Services.Add(DuressServiceMapper.BuildService(deviceData));
            deviceService.Services.Add(MultiPersionLockServiceMapper.BuildService(deviceData));
            deviceService.Services.Add(DoorLinkageServiceMapper.BuildService(deviceData));

            var doors = DoorInfoMapper.BuildService(deviceData);
            doors.ForEach(d => deviceService.Services.Add(d));

            return deviceService;
        }

        public static void UpdateDeviceData(ref byte[] deviceData, DeviceInfo deviceService)
        {
            if (deviceService.AntiPassbackEnabled.HasValue)
                deviceData[3] = (byte)(deviceService.AntiPassbackEnabled.Value ? 1 : 0);

            if (deviceService.Services != null)
            {
                foreach (var s in deviceService.Services)
                {
                    if (s is DuressService)
                        DuressServiceMapper.UpdateDeviceData(ref deviceData, s as DuressService);
                    else if (s is MultiPersionLockService)
                        MultiPersionLockServiceMapper.UpdateDeviceData(ref deviceData, s as MultiPersionLockService);
                    else if (s is DoorLinkageService)
                        DoorLinkageServiceMapper.UpdateDeviceData(ref deviceData, s as DoorLinkageService);
                    else if (s is DoorUnlockOpenBehaviorService)
                    {
                        var timeZoneService = s as DoorUnlockOpenBehaviorService;
                        if (timeZoneService != null)
                            deviceData[27] = (byte)timeZoneService.TimezoneId;
                    }
                    else if (s is DoorInfo)
                        DoorInfoMapper.UpdateDeviceData(ref deviceData, s as DoorInfo);
                }
            }
        }
    }
}