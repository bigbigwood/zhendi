using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.WebSockets;

namespace Rld.DeviceSystem
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
                context.Response.ContentType = "text/plain";
                context.Response.Write("DeviceMessageHandler running");
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