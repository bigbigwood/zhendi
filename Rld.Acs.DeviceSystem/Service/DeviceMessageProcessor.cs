using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Message;
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
           
            PersistenceOperation.Process("", () =>
            {
                var deviceControllerRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                var deviceTrafficLogRepo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();

                var logInfo = new Rld.Acs.Model.DeviceTrafficLog();
                logInfo.DeviceID = -1;
                logInfo.DeviceCode = ev.DeviceTrafficLog.DeviceId.ToString();
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

                var deviceInfo = deviceControllerRepo.Query(new Hashtable() { { "Code", ev.DeviceTrafficLog.DeviceId } }).FirstOrDefault();
                if (deviceInfo != null)
                {
                    logInfo.DeviceID = deviceInfo.DeviceID;
                    logInfo.DeviceType = deviceInfo.Model;
                    logInfo.DeviceSN = deviceInfo.SN;
                }

                deviceTrafficLogRepo.Insert(logInfo);

                return new Message.ResponseBase();
            });
        }

        public static void ProcessGetDeviceOperationLogRequest(string message)
        {

        }
    }
}