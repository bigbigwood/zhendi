using System.Web;
using Microsoft.Web.WebSockets;

namespace Rld.DeviceSystem
{
    /// <summary>
    /// Summary description for Process
    /// </summary>
    public class DeviceProcessor : IHttpHandler
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
                context.Response.Write("DeviceProcessor running");
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