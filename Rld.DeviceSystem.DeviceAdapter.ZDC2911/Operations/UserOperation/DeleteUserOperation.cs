using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.DeleteUserOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.UserOperation
{
    public class DeleteUserOperation
    {
        public DeleteUserResponse Process(DeleteUserRequest request)
        {
            if (request.UserId == 0)
            {

            }

            var service = new UserService(DeviceManager.GetInstance().GetDeviceProxy(1));
            service.DeleteUserInfo(request.UserId);


            return new DeleteUserResponse() { ResultType = ResultType.OK };
        }
    }
}