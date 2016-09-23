using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
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

    public class DeviceOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SystemInfo GetSystemInfo(Int32 deviceId)
        {
            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceId);
            var getSystemInfoRequest = new GetSystemInfoRequest() { Token = operation.Token };
            string rawRequest = DataContractSerializationHelper.Serialize(getSystemInfoRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);

            var response = DataContractSerializationHelper.Deserialize<GetSystemInfoResponse>(rawResponse);
            Log.InfoFormat("Get system info for device id:[{0}], result:[{1}]", deviceId, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Get system info for device id:[{0}], result:[{1}]", deviceId, response.ResultType));
            }

            return response.SystemInfo;
        }

        public DeviceInfo GetDeviceInfo(Int32 deviceId)
        {
            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceId);
            var getDeviceInfoRequest = new GetDeviceInfoRequest() { Token = operation.Token };
            string rawRequest = DataContractSerializationHelper.Serialize(getDeviceInfoRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);

            var response = DataContractSerializationHelper.Deserialize<GetDeviceInfoResponse>(rawResponse);
            Log.InfoFormat("Get device info for device id:[{0}], result:[{1}]", deviceId, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Get device info for device id:[{0}], result:[{1}]", deviceId, response.ResultType));
            }

            return response.Service;
        }
    }
}