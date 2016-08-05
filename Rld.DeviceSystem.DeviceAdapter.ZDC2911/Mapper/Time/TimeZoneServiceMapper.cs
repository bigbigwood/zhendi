using System;
using System.Collections.Generic;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model.Services.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time
{
    public class TimeZoneServiceMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<TimeZoneService> BuildServices(byte[] data)
        {
            var timeZoneServices = new List<TimeZoneService>();
            for (int index = 0; index < Zd2911Utils.PassItemCount; index++)
            {
                int groupIndex = index * 7;
                var timeZoneService = new TimeZoneService()
                {
                    TimeZoneId = index,
                    MondayTimeGroupId = data[groupIndex + 0],
                    TuesdayTimeGroupId = data[groupIndex + 1],
                    WednesdayTimeGroupId = data[groupIndex + 2],
                    ThursdayTimeGroupId = data[groupIndex + 3],
                    FridayTimeGroupId = data[groupIndex + 4],
                    SaturdayTimeGroupId = data[groupIndex + 5],
                    SundayTimeGroupId = data[groupIndex + 6],
                };

                timeZoneServices.Add(timeZoneService);
            }
            return timeZoneServices;
        }


        public static void UpdateData(ref byte[] data, TimeZoneService service)
        {
            Int32 index = service.TimeZoneId * 7;
            if (service.MondayTimeGroupId.HasValue) data[index + 0] = (byte)service.MondayTimeGroupId;
            if (service.TuesdayTimeGroupId.HasValue) data[index + 1] = (byte)service.TuesdayTimeGroupId;
            if (service.WednesdayTimeGroupId.HasValue) data[index + 2] = (byte)service.WednesdayTimeGroupId;
            if (service.ThursdayTimeGroupId.HasValue) data[index + 3] = (byte)service.ThursdayTimeGroupId;
            if (service.FridayTimeGroupId.HasValue) data[index + 4] = (byte)service.FridayTimeGroupId;
            if (service.SaturdayTimeGroupId.HasValue) data[index + 5] = (byte)service.SaturdayTimeGroupId;
            if (service.SundayTimeGroupId.HasValue) data[index + 6] = (byte)service.SundayTimeGroupId;
        }
    }
}