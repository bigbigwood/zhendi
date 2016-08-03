using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetAllTimeSegmentsOperation;
using Rld.DeviceSystem.Contract.Message.GetTimeSegmentOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class GetAllTimeSegmentsOp
    {
        public GetAllTimeSegmentsResponse Process(GetAllTimeSegmentsRequest request)
        {
            var service = new TimeService(DeviceManager.GetInstance().GetDeviceProxy(1));
            var serviceInfos = service.GetAllTimeSegmentServices();

            return new GetAllTimeSegmentsResponse() { ResultType = ResultType.OK, Services = serviceInfos };
        }
    }
}