using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation
{
    public class UpdateDoorStateOp
    {
        public UpdateDoorStateResponse Process(UpdateDoorStateRequest request)
        {
            if (request.Option == DoorControlOption.Close || request.Option == DoorControlOption.CancelAlarm)
            {
                return new UpdateDoorStateResponse() { ResultType = ResultType.NotSupport, Token = request.Token };
            }

            var dao = new DoorInfoDao();
            var result = dao.UpdateStatus(request.DoorIndex, request.Option);

            var response = new UpdateDoorStateResponse
            {
                Token = request.Token,
                ResultType = result ? ResultType.OK : ResultType.Error,
            };

            return response;
        }
    }
}
