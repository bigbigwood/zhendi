using System;
using System.Web;
using Microsoft.Web.WebSockets;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Websocket;

namespace Rld.Acs.DeviceSystem
{
    /// <summary>
    /// DeviceMessageHandler 的摘要说明
    /// </summary>
    public class DeviceMessageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest || context.IsWebSocketRequestUpgrading)
            {
                context.AcceptWebSocketRequest(new DeviceWebSocketHandler());
            }
            else
            {
                var clients = WebSocketClientManager.GetInstance().GetAllClients();

                context.Response.ContentType = "text/plain";
                context.Response.Write(string.Format("Getting {0} clients:", clients.Count) + Environment.NewLine);

                clients.ForEach(d => context.Response.Write(d.Id + Environment.NewLine));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}