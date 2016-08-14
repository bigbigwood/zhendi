using System;
using System.Web;

namespace Rld.Acs.WebApi.DeviceService
{
    /// <summary>
    /// GetClients 的摘要说明
    /// </summary>
    public class GetClients : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var clients = WebSocketClientManager.GetInstance().GetAllClients();

            context.Response.ContentType = "text/plain";
            context.Response.Write(string.Format("Getting {0} clients:", clients.Count) + Environment.NewLine);

            clients.ForEach(d => context.Response.Write(d.Id + Environment.NewLine));
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