using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.BatchUpdateTimeGroupsOperation;
using Rld.DeviceSystem.Contract.Message.BatchUpdateTimeSegmentsOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class BatchUpdateTimeGroupsOp
    {
        public BatchUpdateTimeGroupsResponse Process(BatchUpdateTimeGroupsRequest request)
        {
            if (request.Services == null)
            {

            }

            var service = new TimeService(DeviceManager.GetInstance().GetDeviceProxy(1));
            var result = service.BatchUpdateTimeGroupServices(request.Services );

            return new BatchUpdateTimeGroupsResponse() { ResultType = ResultType.OK };
        }
    }
}