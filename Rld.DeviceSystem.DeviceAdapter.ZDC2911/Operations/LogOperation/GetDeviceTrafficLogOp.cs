﻿using System;
using System.Linq;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Log;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.LogOperation
{
    public class GetDeviceTrafficLogOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GetDeviceTrafficLogResponse Process(GetDeviceTrafficLogRequest request)
        {
            try
            {
                var dao = new GLogInfoDao();
                var rawData = dao.GetLogData(new QueryLogCondictions()
                {
                    Options = GetLogOptions.GetNewLogs,
                    BeginTime = request.BeginTime,
                    EndTime = request.EndTime,
                    CleanNewLogPosition = true,
                });

                var serviceData = rawData.Select(DeviceAccessLogMapper.ToModel).ToList();

                return new GetDeviceTrafficLogResponse() { Token = request.Token, ResultType = ResultType.OK, Logs = serviceData };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GetDeviceTrafficLogResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}