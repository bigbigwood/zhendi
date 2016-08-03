using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetAllTimeGroupsOperation;
using Rld.DeviceSystem.Contract.Message.GetAllTimeSegmentsOperation;
using Rld.DeviceSystem.Contract.Message.GetAllTimeZonesOperation;
using Rld.DeviceSystem.Contract.Message.GetTimeSegmentOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class GetAllTimeZonesOp
    {
        public GetAllTimeZonesResponse Process(GetAllTimeZonesRequest request)
        {
            var service = new TimeService(DeviceManager.GetInstance().GetDeviceProxy(1));
            var serviceInfos = service.GetAllTimeZoneServices();

            return new GetAllTimeZonesResponse() { ResultType = ResultType.OK, Services = serviceInfos };
        }
    }
}