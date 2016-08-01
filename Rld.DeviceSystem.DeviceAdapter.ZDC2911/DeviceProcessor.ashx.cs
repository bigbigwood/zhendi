using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Microsoft.Web.WebSockets;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetUserOperation;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911
{
    /// <summary>
    /// Summary description for DeviceProcessor
    /// </summary>
    public class DeviceProcessor : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest || context.IsWebSocketRequestUpgrading)
            {
                context.AcceptWebSocketRequest(new DeviceWebSocketHandler());
            }
            else
            {
                //var message = ReadRequestBody(context);
                //new DeviceWebSocketHandler().OnMessage(message);
                context.Response.ContentType = "text/plain";
                context.Response.Write("DeviceProcessor running");
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public string ReadRequestBody(HttpContext context)
        {
            Log.Debug("Read Http Request..");
            String requestBody = "";
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, System.Text.Encoding.UTF8))
            {
                requestBody = reader.ReadToEnd();
                Log.DebugFormat("Raw request: {0}", requestBody);
            }

            return requestBody;
        }
    }
}