using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetUserOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation
{
    public class UpdateUserOp
    {
        public ModifyUserResponse Process(ModifyUserRequest request)
        {
            if (request.UserInfo == null)
            {

            }

            var service = new UserService(DeviceManager.GetInstance().GetDeviceProxy(1));
            service.ModifyUserInfo(request.UserInfo);


            return new ModifyUserResponse() { ResultType = ResultType.OK };
        }
    }
}