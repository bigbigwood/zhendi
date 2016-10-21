using System;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation
{
    public class DeleteUserOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DeleteUserInfoResponse Process(DeleteUserInfoRequest request)
        {
            try
            {
                if (request.UserId == 0)
                {

                }

                var userDao = new UserInfoDao();
                bool result = userDao.DeleteUser(request.UserId);

                return new DeleteUserInfoResponse() { Token = request.Token, ResultType = ResultType.OK };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new DeleteUserInfoResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}