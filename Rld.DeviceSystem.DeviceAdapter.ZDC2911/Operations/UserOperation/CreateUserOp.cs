using System;
using System.Collections.Generic;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.UserInfo;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation
{
    public class CreateUserOp
    {
        public CreateUserInfoResponse Process(CreateUserInfoRequest request)
        {
            if (request.UserInfo == null)
            {

            }

            var userEnrollInfoDao = new UserEnrollInfoDao();
            var userDao = new UserInfoDao();

            var enroll = new Enroll() { DIN = (UInt64)request.UserInfo.UserId, Fingerprint = new byte[Zd2911Utils.MaxFingerprintLength * 10] };
            var deviceUser = new User() { DIN = (UInt64)request.UserInfo.UserId, Enrolls = new List<Enroll> { enroll } };

            UserInfoMapper.UpdateSystemInfo(ref deviceUser, request.UserInfo);
            bool result = userDao.SaveOrUpdateUser(deviceUser);

            return new CreateUserInfoResponse() { Token = request.Token, ResultType = ResultType.OK };
        }
    }
}