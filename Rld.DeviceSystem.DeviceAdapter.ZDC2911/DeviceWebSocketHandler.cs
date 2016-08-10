using log4net;
using Microsoft.Web.WebSockets;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.LogOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.SystemOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation;

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

            if (message.Contains("GetUserInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetUserInfoRequest>(message);
                var response = new GetUserOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("UpdateUserInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<UpdateUserInfoRequest>(message);
                var response = new UpdateUserOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("CreateUserInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<CreateUserInfoRequest>(message);
                var response = new CreateUserOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("DeleteUserInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<DeleteUserInfoRequest>(message);
                var response = new DeleteUserOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }

            else if (message.Contains("GetSystemInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetSystemInfoRequest>(message);
                var response = new GetSystemInfoOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("UpdateSystemInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<UpdateSystemInfoRequest>(message);
                var response = new UpdateSystemInfoOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
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

            else if (message.Contains("GetDeviceAccessLogRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetDeviceAccessLogRequest>(message);
                var response = new GetDeviceAccessLogOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("GetDeviceAdminLogRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetDeviceAdminLogRequest>(message);
                var response = new GetDeviceAdminLogOp().Process(request);
                this.Send(DataContractSerializationHelper.Serialize(response));
            }
        }

        public override void OnClose()
        {
            clients.Remove(this);
        }
    }
}