﻿using System;
using System.Linq;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;

namespace Rld.Acs.DeviceSystem.Service
{
    public class DeviceMessageProcessor
    {
        public static void ProcessDeviceTrafficEvent(string message)
        {
            var ev = DataContractSerializationHelper.Deserialize<DeviceTrafficEvent>(message);
            var logInfo = new Rld.Acs.Model.DeviceTrafficLog();
            logInfo.DeviceID = ev.DeviceTrafficLog.DeviceId;
            logInfo.DeviceUserID = ev.DeviceTrafficLog.UserId;
            logInfo.RecordType = ev.DeviceTrafficLog.AccessLogType.ToString();
            logInfo.RecordTime = ev.DeviceTrafficLog.CreateTime;
            logInfo.RecordUploadTime = DateTime.Now;
            logInfo.Remark = ev.DeviceTrafficLog.Message;

            if (ev.DeviceTrafficLog.CheckInOptions.Any())
            {
                logInfo.AuthenticationType = 0;
                ev.DeviceTrafficLog.CheckInOptions.ForEach(option => logInfo.AuthenticationType += (int)option);
            }

            logInfo.DeviceType = 1;
            logInfo.DeviceSN = "hardcode";

            PersistenceOperation.Process(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();
                repo.Insert(logInfo);
            });
        }

        public static void ProcessGetDeviceOperationLogRequest(string message)
        {
            
        }
    }
}