﻿using System;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.SystemInfo;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.SystemOperation
{
    public class UpdateSystemInfoOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UpdateSystemInfoResponse Process(UpdateSystemInfoRequest request)
        {
            try
            {
                var dao = new SystemInfoDao();

                var data = new SystemEntity();

                SystemInfoMapper.UpdateSystemInfo(ref data, request.SystemInfo);

                dao.UpdateSystemData(data);

                return new UpdateSystemInfoResponse() { Token = request.Token, ResultType = ResultType.OK };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new UpdateSystemInfoResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}