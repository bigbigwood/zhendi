using System;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Web.WebSockets;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Service;
using Rld.Acs.Unility.Extension;

namespace Rld.Acs.DeviceSystem.Websocket
{
    public class DeviceWebSocketHandler : WebSocketHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Int32 Id;
        private static readonly Regex TokenRegex = new Regex("<Token>.*</Token>");

        public override void OnOpen()
        {
            int deviceId = WebSocketContext.QueryString["Id"].ToInt32();
            if (deviceId != ConvertorExtension.ConvertionFailureValue)
            {
                this.Id = deviceId;
                WebSocketClientManager.GetInstance().AddClient(this);
            }

            Log.InfoFormat("Web socket id: {0} opened.", Id);
        }

        public override void OnMessage(string message)
        {
            Log.InfoFormat("Web socket id: {0} receive message: {1}", Id, message);

            var token = ParseToken(message);
            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            if (message.Contains("DeviceTrafficEvent"))
                DeviceMessageProcessor.ProcessDeviceTrafficEvent(message);
            else if (message.Contains("Response"))
            {
                var op = OperationManager.GetInstance().GetOperationByToken(token);
                if (op != null)
                {
                    op.FillResponse(message);
                }
                else
                {
                    Log.Warn("No operation matches the response message");
                }
            }
        }

        public override void OnClose()
        {
            WebSocketClientManager.GetInstance().RemoveClient(this);
            Log.InfoFormat("Web socket id: {0} close.", Id);
        }

        public string ParseToken(string message)
        {
            string token = string.Empty;
            try
            {
                var match = TokenRegex.Match(message);
                string result = match.ToString();
                token = result.Substring(7, result.Length - 15);
            }
            catch (Exception ex)
            {
                Log.Warn(ex);
            }
            return token;
        }
    }
}