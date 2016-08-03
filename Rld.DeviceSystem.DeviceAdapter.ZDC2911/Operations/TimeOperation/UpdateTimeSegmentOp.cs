using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.UpdateTimeSegmentOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class UpdateTimeSegmentOp
    {
        public UpdateTimeSegmentResponse Process(UpdateTimeSegmentRequest request)
        {
            if (request.Service == null)
            {

            }

            var service = new TimeService(DeviceManager.GetInstance().GetDeviceProxy(1));
            var result = service.BatchUpdateTimeSegmentServices(new[] { request.Service });

            return new UpdateTimeSegmentResponse() { ResultType = ResultType.OK };
        }
    }
}