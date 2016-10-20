using System.Collections.Generic;
using Rld.Acs.Unility.Extension;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.UserInfo;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation
{
    public class GetAllUsersOp
    {
        public GetAllUsersResponse Process(GetAllUsersRequest request)
        {
            var users = new List<UserInfo>();
            var userData = new UserInfoDao().GetAllUsers(true);
            if (userData != null)
            {
                userData.ForEach(x => users.Add(UserInfoMapper.ToUserSummaryDto(x)));
            }

            return new GetAllUsersResponse() { Token = request.Token, ResultType = ResultType.OK, Users = users };
        }
    }
}