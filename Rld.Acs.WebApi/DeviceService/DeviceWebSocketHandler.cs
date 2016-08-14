using System;
using System.Linq;
using System.Web.UI.WebControls;
using log4net;
using Microsoft.Web.WebSockets;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.Unility.Serialization;
using Rld.Acs.WebApi.Framework;
using Rld.DeviceSystem.Contract.Message;

namespace Rld.Acs.WebApi.DeviceService
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
            if (message.Contains("DeviceTrafficEvent"))
                DeviceMessageProcessor.ProcessDeviceTrafficEvent(message);
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