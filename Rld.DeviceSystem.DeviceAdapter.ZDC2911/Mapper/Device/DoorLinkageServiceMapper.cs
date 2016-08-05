using log4net;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device
{
    public class DoorLinkageServiceMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static DoorLinkageService BuildService(byte[] deviceData)
        {
            var linkageService = new DoorLinkageService();
            linkageService.OpenDoorOption = (DoorLinkageOptions)deviceData[25];
            linkageService.AlarmOption = (DoorLinkageOptions)deviceData[26];
            return linkageService;
        }

        public static void UpdateDeviceData(ref byte[] deviceData, DoorLinkageService service)
        {
            if (service.OpenDoorOption.HasValue)
                deviceData[25] = (byte)(service.OpenDoorOption.Value);

            if (service.AlarmOption.HasValue)
                deviceData[25] = (byte)(service.AlarmOption.Value);

        }
    }
}