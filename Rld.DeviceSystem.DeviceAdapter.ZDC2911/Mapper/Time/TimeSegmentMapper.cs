using System;
using System.Collections.Generic;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model.Services.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time
{
    public class TimeSegmentMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<TimeSegmentService> BuildServices(byte[] timeSegmentData)
        {
            var timesegments = new List<TimeSegmentService>();
            for (int index = 0; index < Zd2911Utils.PassItemCount; index++)
            {
                var timeSegmentService = new TimeSegmentService()
                {
                    TimeSegmentId = index,
                    StartHour = Convert.ToInt32(timeSegmentData[index * 4]),
                    StartMinute = Convert.ToInt32(timeSegmentData[index * 4 + 1]),
                    EndHour = Convert.ToInt32(timeSegmentData[index * 4 + 2]),
                    EndMinute = Convert.ToInt32(timeSegmentData[index * 4 + 3]),
                };

                timesegments.Add(timeSegmentService);
            }

            return timesegments;
        }


        public static void UpdateTimeSegmentData(ref byte[] timeSegmentData, TimeSegmentService service)
        {
            Int32 index = service.TimeSegmentId * 4;
            timeSegmentData[index] = (Byte)service.StartHour;
            timeSegmentData[index + 1] = (Byte)service.StartMinute;
            timeSegmentData[index + 2] = (Byte)service.EndHour;
            timeSegmentData[index + 3] = (Byte)service.EndMinute;
        }
    }
}