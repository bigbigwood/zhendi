using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetTimeSegmentOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class GetTimeSegmentOp
    {
        public GetTimeSegmentResponse Process(GetTimeSegmentRequest request)
        {
            if (request.Id == 0)
            {

            }

            var service = new TimeService(DeviceManager.GetInstance().GetDeviceProxy(1));
            var serviceInfo = service.GetAllTimeSegmentServices().FirstOrDefault(s => s.TimeSegmentId == request.Id);

            return new GetTimeSegmentResponse() { ResultType = ResultType.OK, Service = serviceInfo };
        }
    }
}