using System;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation
{
    public class GetDoorStateOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GetDoorStateResponse Process(GetDoorStateRequest request)
        {
            try
            {
                var dao = new DoorInfoDao();
                var doorStates = dao.GetDoorStates(request.DoorIndex);

                return new GetDoorStateResponse() { Token = request.Token, ResultType = ResultType.OK, DoorStateInfo = doorStates };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GetDoorStateResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}
