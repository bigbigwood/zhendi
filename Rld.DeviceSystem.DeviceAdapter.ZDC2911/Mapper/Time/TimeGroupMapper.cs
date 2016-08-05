using System;
using System.Collections.Generic;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model.Services.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time
{
    public class TimeGroupMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly List<Int32> ZeroToNineCollection = new List<Int32> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public static List<TimeGroupService> BuildServices(byte[] timeGroupData)
        {
            var timeGroupServices = new List<TimeGroupService>();
            for (int index = 0; index < Zd2911Utils.PassItemCount; index++)
            {
                var ids = new List<int>();
                ZeroToNineCollection.ForEach(num => ids.Add(timeGroupData[index * 10 + num]));
                var timeGroupService = new TimeGroupService() { TimeGroupId = index, TimeSegmentIds = ids };

                timeGroupServices.Add(timeGroupService);
            }

            return timeGroupServices;
        }


        public static void UpdateTimeGroupData(ref byte[] timeSegmentData, TimeGroupService service)
        {
            Int32 index = service.TimeGroupId * 10;
            foreach (var timeSegmentId in service.TimeSegmentIds)
            {
                timeSegmentData[index++] = (byte)timeSegmentId;
            }
        }
    }
}