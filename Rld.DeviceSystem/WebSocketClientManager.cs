using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.DeviceSystem
{
    public class WebSocketClientManager
    {
        private static readonly Object _lockObject;
        private static readonly WebSocketClientManager _instance;

        private List<DeviceWebSocketHandler> clients;

        private WebSocketClientManager()
        {
            clients = new List<DeviceWebSocketHandler>();
        }

        static WebSocketClientManager()
        {
            _lockObject = new object();
            _instance = new WebSocketClientManager();
        }

        public static WebSocketClientManager GetInstance()
        {
            return _instance;
        }

        public void AddClient(DeviceWebSocketHandler websocketClient)
        {
            lock (_lockObject)
            {
                clients.Add(websocketClient);
            }
        }

        public void RemoveClient(DeviceWebSocketHandler websocketClient)
        {
            lock (_lockObject)
            {
                clients.Remove(websocketClient);
            }
        }

        public List<DeviceWebSocketHandler> GetAllClients()
        {
            return clients;
        }
    }

}