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

    public class DoorStateOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ResultType UpdateDoorState(Int32 deviceId, Int32 doorIndex, DoorControlOption option)
        {
            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceId);
            var updateDoorStateRequest = new UpdateDoorStateRequest() { Token = operation.Token, DoorIndex = doorIndex, Option = option };
            string rawRequest = DataContractSerializationHelper.Serialize(updateDoorStateRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);

            var response = DataContractSerializationHelper.Deserialize<UpdateDoorStateResponse>(rawResponse);
            Log.InfoFormat("Update door index:[{0}] for device id:[{1}], result:[{2}]", doorIndex, deviceId, response.ResultType);

            if (response.ResultType != ResultType.OK && response.ResultType != ResultType.NotSupport)
            {
                throw new Exception(string.Format("Update door index:[{0}] for device id:[{1}], result:[{2}]", doorIndex, deviceId, response.ResultType));
            }
            return response.ResultType;
        }

        public bool GetDoorState(Int32 deviceId, Int32 doorIndex)
        {
            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceId);
            var getDoorStateRequest = new GetDoorStateRequest() { Token = operation.Token, DoorIndex = doorIndex };
            string rawRequest = DataContractSerializationHelper.Serialize(getDoorStateRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);

            var response = DataContractSerializationHelper.Deserialize<GetDoorStateResponse>(rawResponse);
            Log.InfoFormat("Get door state, index:[{0}] for device id:[{1}], result:[{2}]", doorIndex, deviceId, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Get door state, index:[{0}] for device id:[{1}], result:[{2}]", doorIndex, deviceId, response.ResultType));
            }

            return response.DoorStateInfo.Opened;
        }
    }
}