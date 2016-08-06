using System.Xml;
using log4net;
using Microsoft.Web.WebSockets;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.BatchUpdateTimeGroupsOperation;
using Rld.DeviceSystem.Contract.Message.BatchUpdateTimeSegmentsOperation;
using Rld.DeviceSystem.Contract.Message.BatchUpdateTimeZonesOperation;
using Rld.DeviceSystem.Contract.Message.GetAllTimeGroupsOperation;
using Rld.DeviceSystem.Contract.Message.GetAllTimeSegmentsOperation;
using Rld.DeviceSystem.Contract.Message.GetAllTimeZonesOperation;
using Rld.DeviceSystem.Contract.Message.GetDeviceInfoOp;
using Rld.DeviceSystem.Contract.Message.GetTimeSegmentOperation;
using Rld.DeviceSystem.Contract.Message.GetUserOperation;
using Rld.DeviceSystem.Contract.Message.UpdateDeviceInfoOp;
using Rld.DeviceSystem.Contract.Message.UpdateTimeSegmentOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation;
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
                var response = new UpdateUserOp().Process(request);
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

            else if (message.Contains("GetDeviceRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<DeleteUserRequest>(message);
                var response = new DeleteUserOperation().Process(request);
                this.Send(DataContractSerializationHelper.Serialize<DeleteUserResponse>(response));
            }

            else if (message.Contains("GetDeviceInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetDeviceInfoRequest>(message);
                var response = new GetDeviceInfoOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("UpdateDeviceInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<UpdateDeviceInfoRequest>(message);
                var response = new UpdateDeviceInfoOp().Process(request);
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
            else if (message.Contains("GetAllTimeGroupsRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetAllTimeGroupsRequest>(message);
                var response = new GetAllTimeGroupsOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("BatchUpdateTimeGroupsRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<BatchUpdateTimeGroupsRequest>(message);
                var response = new BatchUpdateTimeGroupsOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("GetAllTimeZonesRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetAllTimeZonesRequest>(message);
                var response = new GetAllTimeZonesOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("BatchUpdateTimeZonesRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<BatchUpdateTimeZonesRequest>(message);
                var response = new BatchUpdateTimeZonesOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }

        }

        public override void OnClose()
        {
            clients.Remove(this);
        }
    }
}