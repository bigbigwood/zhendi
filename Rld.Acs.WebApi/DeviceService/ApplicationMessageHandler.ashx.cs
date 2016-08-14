using System;
using System.Linq;
using System.Web;
using log4net;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;

namespace Rld.Acs.WebApi.DeviceService
{
    /// <summary>
    /// Summary description for ApplicationProcessor
    /// </summary>
    public class ApplicationMessageHandler : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void ProcessRequest(HttpContext context)
        {
            Int32 deviceId = context.Request.Params["DeviceId"].ToInt32();
            if (deviceId != ConvertorExtension.ConvertionFailureValue)
            {
                var client = WebSocketClientManager.GetInstance().GetAllClients().FirstOrDefault(d => d.Id == deviceId);
                if (client != null)
                {
                    client.Send(GetRequestBody(context));
                    context.Response.Write(ConstStrings.Ok);
                }
                else
                {
                    context.Response.Write("unable to find web socket client.");
                }
            }
            else
            {
                context.Response.Write("invalid request format.");
            }
        }

        public string GetRequestBody(HttpContext context)
        {
            String requestBody = "";
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, System.Text.Encoding.UTF8))
            {
                requestBody = reader.ReadToEnd();
            }
            return requestBody;
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