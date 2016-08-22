using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.Acs.DeviceSystem.Service
{
    public class OperationLogOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IList<DeviceOperationLog> QueryNewOperationLogs(Int32 deviceID)
        {
            var operation = new WebSocketOperation(deviceID);

            var getDeviceOperationLogRequest = new GetDeviceOperationLogRequest()
            {
                Token = operation.Token, 
                BeginTime = new DateTime(2015,1,1),
                EndTime = new DateTime(2099, 12,31),
            };

            string rawRequest = DataContractSerializationHelper.Serialize(getDeviceOperationLogRequest);
            var rawResponse = operation.Execute(rawRequest);

            var response = DataContractSerializationHelper.Deserialize<GetDeviceOperationLogResponse>(rawResponse);
            Log.InfoFormat("GetDeviceOperationLogResponse from device id:{0}, result ={1}", deviceID, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("GetDeviceOperationLogResponse from device id:{0} fails", deviceID));
            }

            var deviceOperationLogs = new List<DeviceOperationLog>();
            foreach (var rawlog in response.Logs)
            {
                deviceOperationLogs.Add(new DeviceOperationLog()
                {
                    DeviceId = deviceID,
                    OperatorId = rawlog.AdminId,
                    DeviceUserId = rawlog.UserId,
                    DeviceType = 1,
                    OperationType = rawlog.OperationType,
                    OperationDescription = rawlog.Message,
                    OperationContent = rawlog.Enroll,
                    OperationTime = rawlog.CreateTime,
                    OperationUploadTime = DateTime.Now,
                });
            }

            return deviceOperationLogs;
        }
    }
}