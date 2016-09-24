using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using log4net;
using Microsoft.Web.WebSockets;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Websocket;

namespace Rld.Acs.DeviceSystem
{
    /// <summary>
    /// DeviceMessageHandler 的摘要说明
    /// </summary>
    public class TestHandler : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex TokenRegex = new Regex("<Token>.*</Token>");
        public void ProcessRequest(HttpContext context)
        {
            var clients = WebSocketClientManager.GetInstance().GetAllClients();

            Log.Debug("Read Http Request..");
            String requestBody = "";
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, System.Text.Encoding.UTF8))
            {
                requestBody = reader.ReadToEnd();
                Log.DebugFormat("Raw request: {0}", requestBody);
            }

            var response = "response";
            var token = ParseToken(requestBody);
            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }
            if (clients.Any())
            {
                var deviceID = clients.First().Id;
                var operation = new WebSocketOperation(deviceID);
                operation.Token = token;
                response = operation.Execute(requestBody);
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(response);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}