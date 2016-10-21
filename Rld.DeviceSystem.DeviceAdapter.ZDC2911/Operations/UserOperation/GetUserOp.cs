using System;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.UserInfo;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation
{
    public class GetUserOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GetUserInfoResponse Process(GetUserInfoRequest request)
        {
            try
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

                return new GetUserInfoResponse() { Token = request.Token, ResultType = ResultType.OK, UserInfo = userInfo };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GetUserInfoResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}