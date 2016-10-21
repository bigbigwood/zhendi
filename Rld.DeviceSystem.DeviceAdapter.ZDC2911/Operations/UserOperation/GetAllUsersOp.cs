using System;
using System.Collections.Generic;
using log4net;
using Rld.Acs.Unility.Extension;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.UserInfo;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation
{
    public class GetAllUsersOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GetAllUsersResponse Process(GetAllUsersRequest request)
        {
            try
            {
                var users = new List<UserInfo>();
                var userData = new UserInfoDao().GetAllUsers(true);
                if (userData != null)
                {
                    userData.ForEach(x => users.Add(UserInfoMapper.ToUserSummaryDto(x)));
                }

                return new GetAllUsersResponse() { Token = request.Token, ResultType = ResultType.OK, Users = users };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GetAllUsersResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}