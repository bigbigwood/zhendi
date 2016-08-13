using System;
using log4net;
using Microsoft.Web.WebSockets;
using Rld.Acs.Unility.Extension;

namespace Rld.DeviceSystem
{
    public class DeviceWebSocketHandler : WebSocketHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Int32 Id;

        public override void OnOpen()
        {
            int deviceId = WebSocketContext.QueryString["Id"].ToInt32();
            if (deviceId != ConvertorExtension.ConvertionFailureValue)
            {
                this.Id = deviceId;
                WebSocketClientManager.GetInstance().AddClient(this);
            }
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
            WebSocketClientManager.GetInstance().RemoveClient(this);
        }
    }
}