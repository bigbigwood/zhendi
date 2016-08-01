using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.CreateUserOperation;
using Rld.DeviceSystem.Contract.Message.GetUserOperation;
using Rld.DeviceSystem.Contract.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.UserOperation
{
    public class CreateUserOperation
    {
        public CreateUserResponse Process(CreateUserRequest request)
        {
            if (request.UserInfo == null)
            {

            }

            var service = new UserService(DeviceManager.GetInstance().GetDeviceProxy(1));
            service.CreateUserInfo(request.UserInfo);


            return new CreateUserResponse() { ResultType = ResultType.OK };
        }
    }
}