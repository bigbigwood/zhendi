using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.Acs.DeviceSystem.Service
{
    public class TrafficLogOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IList<DeviceTrafficLog> QueryNewTrafficLogs(Int32 deviceID)
        {
            var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
            var deviceInfo = repo.GetByKey(deviceID);
            var deviceCode = deviceInfo.Code.ToInt32();
            if (WebSocketClientManager.GetInstance().GetClientById(deviceCode) == null)
                throw new DeviceNotConnectedException();

            var operation = new WebSocketOperation(deviceCode);
            var getDeviceTrafficLogRequest = new GetDeviceTrafficLogRequest()
            {
                Token = operation.Token, 
                BeginTime = new DateTime(2016,1,1),
                EndTime = new DateTime(2099, 12,31),
            };

            string rawRequest = DataContractSerializationHelper.Serialize(getDeviceTrafficLogRequest);
            var rawResponse = operation.Execute(rawRequest);

            var response = DataContractSerializationHelper.Deserialize<GetDeviceTrafficLogResponse>(rawResponse);
            Log.InfoFormat("GetDeviceTrafficLogResponse from device id:{0}, result ={1}", deviceID, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("GetDeviceTrafficLogResponse from device id:{0} fails", deviceID));
            }

            var deviceTrafficLogs = new List<DeviceTrafficLog>();
            foreach (var rawlog in response.Logs)
            {
                var log = new DeviceTrafficLog()
                {
                    DeviceID = deviceID,
                    DeviceUserID = rawlog.UserId,
                    DeviceCode = deviceInfo.Code,
                    DeviceType = deviceInfo.Model,
                    DeviceSN = deviceInfo.SN,
                    RecordType = rawlog.AccessLogType.ToString(),
                    RecordTime = rawlog.CreateTime,
                    RecordUploadTime = DateTime.Now,
                    Remark = rawlog.Message,
                };

                if (rawlog.CheckInOptions.Any())
                {
                    log.AuthenticationType = 0;
                    rawlog.CheckInOptions.ForEach(option => log.AuthenticationType += (int)option);
                }

                deviceTrafficLogs.Add(log);
            }

            return deviceTrafficLogs;
        }
    }
}