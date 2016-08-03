using System.Xml;
using log4net;
using Microsoft.Web.WebSockets;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.BatchUpdateTimeSegmentsOperation;
using Rld.DeviceSystem.Contract.Message.GetAllTimeSegmentsOperation;
using Rld.DeviceSystem.Contract.Message.GetTimeSegmentOperation;
using Rld.DeviceSystem.Contract.Message.GetUserOperation;
using Rld.DeviceSystem.Contract.Message.ModifyDeviceOperation;
using Rld.DeviceSystem.Contract.Message.UpdateTimeSegmentOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.UserOperation;
using Rld.DeviceSystem.Contract.Message.CreateUserOperation;
using Rld.DeviceSystem.Contract.Message.DeleteUserOperation;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911
{
    public class DeviceWebSocketHandler : WebSocketHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static WebSocketCollection clients = new WebSocketCollection();

        public override void OnOpen()
        {
            clients.Add(this);
        }

        public override void OnMessage(string message)
        {

            if (message.Contains("GetUserRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetUserRequest>(message);
                var response = new GetUserOperation().Process(request);
                this.Send(DataContractSerializationHelper.Serialize<GetUserResponse>(response));
            }
            else if (message.Contains("ModifyUserRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<ModifyUserRequest>(message);
                var response = new ModifyUserOperation().Process(request);
                this.Send(DataContractSerializationHelper.Serialize<ModifyUserResponse>(response));
            }
            else if (message.Contains("CreateUserRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<CreateUserRequest>(message);
                var response = new CreateUserOperation().Process(request);
                this.Send(DataContractSerializationHelper.Serialize<CreateUserResponse>(response));
            }
            else if (message.Contains("DeleteUserRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<DeleteUserRequest>(message);
                var response = new DeleteUserOperation().Process(request);
                this.Send(DataContractSerializationHelper.Serialize<DeleteUserResponse>(response));
            }

            else if (message.Contains("ModifyDeviceRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<ModifyDeviceRequest>(message);
                var response = new ModifyDeviceOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize<ModifyDeviceResponse>(response));
            }
            else if (message.Contains("GetDeviceRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<DeleteUserRequest>(message);
                var response = new DeleteUserOperation().Process(request);
                this.Send(DataContractSerializationHelper.Serialize<DeleteUserResponse>(response));
            }


            else if (message.Contains("GetTimeSegmentRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetTimeSegmentRequest>(message);
                var response = new GetTimeSegmentOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("GetAllTimeSegmentsRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetAllTimeSegmentsRequest>(message);
                var response = new GetAllTimeSegmentsOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("BatchUpdateTimeSegmentsRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<BatchUpdateTimeSegmentsRequest>(message);
                var response = new BatchUpdateTimeSegmentsOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("UpdateTimeSegmentRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<UpdateTimeSegmentRequest>(message);
                var response = new UpdateTimeSegmentOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
        }

        public override void OnClose()
        {
            clients.Remove(this);
        }
    }
}