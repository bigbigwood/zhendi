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

        private void Init(DeviceConfigurationAdapter deviceConfig)
        {
            _deviceProxy = new DeviceProxy(deviceConfig);
            _udpListener = new UdpListener(deviceConfig.UdpPort, SendMessage);
            _webSocketClient = new WebSocketClient(deviceConfig.WebSocketClientConfig.ServerUrl, ReceiveMessage);
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
                IsRunning = true;
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public Boolean ReStart(DeviceConfiguration deviceConfig)
        {
            bool bStop = Stop();

            return bStop && Start(deviceConfig);
        }

        public Boolean Stop()
        {
            try
            {
                Log.Info("DeviceAdapter Stoping...");
                _deviceProxy.CloseConnection();
                _udpListener.Stop();
                _webSocketClient.Stop();
                IsRunning = false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public void ReceiveMessage(String message)
        {
            DeviceProxyManager.Bind(_deviceProxy);
            var response = _deviceProxy.ProcessReceiveEvent(message);
            SendMessage(response);
        }

        public void SendMessage(String message)
        {
            _webSocketClient.Send(message);
        }
    }
}
