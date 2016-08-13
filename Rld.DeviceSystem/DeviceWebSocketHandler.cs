using log4net;
using Microsoft.Web.WebSockets;

namespace Rld.DeviceSystem
{
    public class DeviceWebSocketHandler : WebSocketHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static WebSocketCollection clients = new WebSocketCollection();
        public string Id;

        public override void OnOpen()
        {
            this.Id = this.WebSocketContext.QueryString["Id"];
            clients.Add(this);
        }

        public override void OnMessage(string message)
        {
            if (message.Contains("Request"))
            {
                this.Send(message);
            }
            else
            {
                Log.Info(message);
            }
        }

        public override void OnClose()
        {
            clients.Remove(this);
        }
    }
}