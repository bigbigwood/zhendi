using System.Text.RegularExpressions;
using log4net;
using Rld.DeviceSystem.Contract.Interface;
using Rld.DeviceSystem.Contract.Model.Configuration;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Configuration;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911
{
    public class DeviceAdapter : IDevice
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy;
        private UdpListener _udpListener;
        private WebSocketClient _webSocketClient;
        private HealthThread _healthThread;

        private void Init(DeviceConfigurationAdapter deviceConfig)
        {
            _deviceProxy = new DeviceProxy(deviceConfig);
            _udpListener = new UdpListener(deviceConfig.UdpPort, SendMessage);
            _webSocketClient = new WebSocketClient(deviceConfig.WebSocketClientConfig.ServerUrl, ReceiveMessage);
            _healthThread = new HealthThread(CheckServerConnection, ReopenServerConnection);
        }


        public Boolean IsRunning { get; set; }
        public Boolean Start(DeviceConfiguration deviceConfig)
        {
            try
            {
                Log.Info("DeviceAdapter Starting...");
                var config = deviceConfig as DeviceConfigurationAdapter;
                if (config == null)
                    throw new Exception("Init Device Adapter fails, please use DeviceConfigurationAdapter.");

                Init(config);
                _deviceProxy.OpenConnection();
                _udpListener.Start();
                _webSocketClient.Start();
                _healthThread.Start();
                IsRunning = true;
                Log.Info("DeviceAdapter starts finished...");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public Boolean Stop()
        {
            try
            {
                Log.Info("DeviceAdapter Stoping...");
                _healthThread.Stop();
                _udpListener.Stop();
                _webSocketClient.Stop();
                _deviceProxy.CloseConnection();
                IsRunning = false;
                Log.Info("DeviceAdapter stops finished...");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// Receive message from web socket server
        /// </summary>
        /// <param name="message"></param>
        public void ReceiveMessage(String message)
        {
            try
            {
                DeviceProxyManager.Bind(_deviceProxy);
                var response = _deviceProxy.ProcessReceiveEvent(message);
                SendMessage(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                SendMessage(ex.Message);
            }
        }

        /// <summary>
        /// Send message from web socket server
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(String message)
        {
            _webSocketClient.Send(message);
        }

        public bool CheckServerConnection()
        {
            return _webSocketClient.CheckConnectionAlive();
        }

        public bool ReopenServerConnection()
        {
            _webSocketClient.Stop();
            _webSocketClient.Start();
            return true;
        }
    }
}
