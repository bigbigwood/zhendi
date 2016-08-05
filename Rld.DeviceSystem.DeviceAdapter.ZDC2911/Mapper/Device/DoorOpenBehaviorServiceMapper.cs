using System;
using log4net;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device
{
    public class DoorOpenBehaviorServiceMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static DoorOpenBehaviorService BuildDelayOpenService(Int32 seconds)
        {
            var service = new DoorOpenBehaviorService()
            {
                Type = DoorOpenBehavior.DelayOpen,
                Seconds = seconds,
                Enabled = (seconds != 0),
            };
            return service;
        }

        public static DoorOpenBehaviorService BuildIllegalOpenService(Int32 seconds)
        {
            var service = new DoorOpenBehaviorService()
            {
                Type = DoorOpenBehavior.IllegalOpen,
                Seconds = seconds,
                Enabled = (seconds != 0),
            };
            return service;
        }

        public static DoorOpenBehaviorService BuildOverTimeOpenService(Int32 seconds)
        {
            var service = new DoorOpenBehaviorService()
            {
                Type = DoorOpenBehavior.OverTimeOpen,
                Seconds = seconds,
                Enabled = (seconds != 0),
            };
            return service;
        }

        public static DoorOpenBehaviorService BuildUnlockOpenService(Int32 timezoneId)
        {
            var service = new DoorUnlockOpenBehaviorService()
            {
                Type = DoorOpenBehavior.UnlockOpen,
                Enabled = (timezoneId != 0),
                TimezoneId = timezoneId,
            };
            return service;
        }
    }
}