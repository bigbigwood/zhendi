using System.Xml;
using log4net;
using Microsoft.Web.WebSockets;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetUserOperation;

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
            var request = DataContractSerializationHelper.Deserialize<RequestBase>(message);


            //this.Send(message);
        }

        public override void OnClose()
        {
            clients.Remove(this);
        }
    }
}