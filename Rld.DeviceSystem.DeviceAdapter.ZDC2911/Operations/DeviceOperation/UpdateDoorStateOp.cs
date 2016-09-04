using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation
{
    public class UpdateDoorStateOp
    {
        public UpdateDoorStateResponse Process(UpdateDoorStateRequest request)
        {
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
