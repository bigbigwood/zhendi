using Microsoft.Web.WebSockets;

namespace Rld.DeviceSystem
{
    public class DeviceWebSocketHandler : WebSocketHandler
    {
        private static WebSocketCollection clients = new WebSocketCollection();
        private string name;

        public override void OnOpen()
        {
            this.name = this.WebSocketContext.QueryString["username"];
            clients.Add(this);
        }

        public override void OnMessage(string message)
        {
            this.Send(message);
        }

        public override void OnClose()
        {
            clients.Remove(this);
        }
    }
}