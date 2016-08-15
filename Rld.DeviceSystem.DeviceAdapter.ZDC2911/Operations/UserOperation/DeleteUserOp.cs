using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation
{
    public class DeleteUserOp
    {
        public DeleteUserInfoResponse Process(DeleteUserInfoRequest request)
        {
            if (request.UserId == 0)
            {

            }

            var userDao = new UserInfoDao();
            bool result = userDao.DeleteUser(request.UserId);

            return new DeleteUserInfoResponse() { Token = request.Token, ResultType = ResultType.OK };
        }
    }
}