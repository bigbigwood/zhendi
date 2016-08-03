using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.BatchUpdateTimeGroupsOperation;
using Rld.DeviceSystem.Contract.Message.BatchUpdateTimeSegmentsOperation;
using Rld.DeviceSystem.Contract.Message.BatchUpdateTimeZonesOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class BatchUpdateTimeZonesOp
    {
        public BatchUpdateTimeZonesResponse Process(BatchUpdateTimeZonesRequest request)
        {
            if (request.Services == null)
            {

            }

            var service = new TimeService(DeviceManager.GetInstance().GetDeviceProxy(1));
            var result = service.BatchUpdateTimeZoneServices(request.Services );

            return new BatchUpdateTimeZonesResponse() { ResultType = ResultType.OK };
        }
    }
}