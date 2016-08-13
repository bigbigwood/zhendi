using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.DeviceSystem
{
    /// <summary>
    /// GetClients 的摘要说明
    /// </summary>
    public class GetClients : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var devices = DeviceWebSocketHandler.clients.ToList();

            foreach (var d in devices)
            {
                var client  = d as DeviceWebSocketHandler;
                if (client != null)
                    context.Response.Write(client.Id);

                if (client.Id == "12")
                {
                    client.Send("Hello 12");
                }
            }


            context.Response.Write("Hello World");
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