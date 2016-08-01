using System.Xml;
using log4net;
using Microsoft.Web.WebSockets;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetUserOperation;
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

        }

        public override void OnClose()
        {
            clients.Remove(this);
        }
    }
}