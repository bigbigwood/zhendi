using System;
using log4net;
using WebSocket4Net;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework
{
    internal class WebSocketClient
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly WebSocket websocket;
        public Action<String> ProcessReceiveEvent;

        public WebSocketClient(string serverUri, Action<String> receiveEventAction)
        {
            websocket = new WebSocket(serverUri);
            websocket.Opened += websocket_Opened;
            websocket.Error += websocket_Error;
            websocket.Closed += websocket_Closed;
            websocket.MessageReceived += websocket_MessageReceived;
            ProcessReceiveEvent = receiveEventAction;
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            Log.Info("WebSocketClient opened");
        }

        private void websocket_Error(object sender, EventArgs e)
        {
            Log.Info("WebSocketClient Errors");
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            Log.Info("WebSocketClient Closed");
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message;
            Log.InfoFormat("WebSocketClient receive message: {0}", message);

            ProcessReceiveEvent(message);
        }


        public void Start()
        {
            websocket.Open();
        }

        public void Stop()
        {
            websocket.Close();
        }

        public void Send(string message)
        {
            Log.InfoFormat("WebSocketClient send message: {0}", message);
            websocket.Send(message);
        }

        public bool CheckConnectionAlive()
        {
            return websocket.State != WebSocketState.Closed;
        }
    }
}
