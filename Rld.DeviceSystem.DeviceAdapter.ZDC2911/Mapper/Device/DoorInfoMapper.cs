using System.Collections.Generic;
using System.Linq;
using log4net;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device
{
    public class DoorInfoMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<DoorInfo> BuildService(byte[] deviceData)
        {
            var doors = new List<DoorInfo>();

            var masterDoor = new DoorInfo();
            masterDoor.Name = "Master Door";
            masterDoor.DoorType = DoorType.Master;
            //masterDoor.ElectricalAppliances = 0;
            //masterDoor.CheckOutAction = CheckOutOptions.Button;
            //masterDoor.AlertType = 0;
            //masterDoor.Remark = "";

            masterDoor.Services.Add(DoorOpenBehaviorServiceMapper.BuildDelayOpenService(deviceData[16]));
            masterDoor.Services.Add(DoorOpenBehaviorServiceMapper.BuildOverTimeOpenService(deviceData[18]));
            masterDoor.Services.Add(DoorOpenBehaviorServiceMapper.BuildIllegalOpenService(deviceData[19]));
            masterDoor.Services.Add(DoorOpenBehaviorServiceMapper.BuildUnlockOpenService(deviceData[27]));
            masterDoor.Services.Add(DuressServiceMapper.BuildService(deviceData));
            
            doors.Add(masterDoor);

            var slaveDoor = new DoorInfo();
            slaveDoor.Name = "Slave Door";
            masterDoor.DoorType = DoorType.Slave;
            //slaveDoor.ElectricalAppliances = 0;
            //slaveDoor.CheckOutAction = CheckOutOptions.Button;
            //slaveDoor.AlertType = 0;
            //slaveDoor.Remark = "";

            slaveDoor.Services.Add(DoorOpenBehaviorServiceMapper.BuildDelayOpenService(deviceData[20]));
            slaveDoor.Services.Add(DoorOpenBehaviorServiceMapper.BuildOverTimeOpenService(deviceData[22]));
            slaveDoor.Services.Add(DoorOpenBehaviorServiceMapper.BuildIllegalOpenService(deviceData[23]));
            slaveDoor.Services.Add(DoorOpenBehaviorServiceMapper.BuildUnlockOpenService(deviceData[28]));
            slaveDoor.Services.Add(DuressServiceMapper.BuildService(deviceData));
            doors.Add(slaveDoor);

            return doors;
        }


        public static void UpdateDeviceData(ref byte[] deviceData, DoorInfo doorService)
        {
            if (doorService.Services == null)
            {
                return;
            }

            if (doorService.DoorType == DoorType.Master)
            {
                foreach (var service in doorService.Services.OfType<DoorOpenBehaviorService>())
                {
                    switch (service.Type)
                    {
                        case DoorOpenBehavior.DelayOpen:
                            deviceData[16] = (byte)service.Seconds;
                            break;
                        case DoorOpenBehavior.OverTimeOpen:
                            deviceData[18] = (byte)service.Seconds;
                            break;
                        case DoorOpenBehavior.IllegalOpen:
                            deviceData[19] = (byte)service.Seconds;
                            break;
                        case DoorOpenBehavior.UnlockOpen:
                            {
                                var timeZoneService = service as DoorUnlockOpenBehaviorService;
                                if (timeZoneService != null)
                                    deviceData[27] = (byte)timeZoneService.TimezoneId;
                            }
                            break;
                        default:
                            Log.WarnFormat("Cannot update DoorOpenBehaviorService with type={0} to device", service.Type);
                            break;
                    }
                }
            }
            else if (doorService.DoorType == DoorType.Slave)
            {
                foreach (var service in doorService.Services.OfType<DoorOpenBehaviorService>())
                {
                    switch (service.Type)
                    {
                        case DoorOpenBehavior.DelayOpen:
                            deviceData[20] = (byte)service.Seconds;
                            break;
                        case DoorOpenBehavior.OverTimeOpen:
                            deviceData[22] = (byte)service.Seconds;
                            break;
                        case DoorOpenBehavior.IllegalOpen:
                            deviceData[23] = (byte)service.Seconds;
                            break;
                        case DoorOpenBehavior.UnlockOpen:
                            {
                                var timeZoneService = service as DoorUnlockOpenBehaviorService;
                                if (timeZoneService != null)
                                    deviceData[28] = (byte)timeZoneService.TimezoneId;
                            }
                            break;
                        default:
                            Log.WarnFormat("Cannot update DoorOpenBehaviorService with type={0} to device", service.Type);
                            break;
                    }
                }
            }
        }
    }
}