using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation
{
    public class GetDoorStateOp
    {
        public GetDoorStateResponse Process(GetDoorStateRequest request)
        {
            var dao = new DoorInfoDao();
            var doorStates = dao.GetDoorStates();

            return new GetDoorStateResponse() { ResultType = ResultType.OK, DoorStateInfos = doorStates };
        }
    }
}
