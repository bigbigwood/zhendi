using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.UserInfo;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation
{
    public class GetUserOp
    {
        public GetUserInfoResponse Process(GetUserInfoRequest request)
        {
            if (request.UserId == 0)
            {

            }
            UserInfo userInfo = null;
            var dao = new UserInfoDao();
            var userData = dao.GetUser(request.UserId);
            if (userData != null)
            {
                userInfo = UserInfoMapper.ToModel(userData);
            }

            return new GetUserInfoResponse() {Token = request.Token, ResultType = ResultType.OK, UserInfo = userInfo};
        }
    }
}