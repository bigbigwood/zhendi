using System;
using System.Net;
using System.Net.Sockets;
using log4net;
using Riss.Devices;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Log;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework
{
    internal class UdpListener
    {
        private static UdpListener _instance = null;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Zd2911Monitor listener;
        private Int32 port;
        public Action<String> ReportMessage;
        public UdpListener(Int32 udpPort, Action<String> reportMessageAction)
        {
            port = udpPort;
            ReportMessage = reportMessageAction;
        }

        public void Start()
        {
            Log.Info("Starting Udp listener...");
            Monitor m = new Monitor();
            m.UDPAddress = GetLocalIPAddress();
            m.UDPPort = port;
            m.Mode = 0;

            listener = Zd2911Monitor.CreateZd2911Monitor(m);
            listener.ReceiveHandler += listener_ReceiveHandler;
            listener.OpenListen();

            Log.Info("Udp listener started...");
        }

        private string GetLocalIPAddress()
        {
            IPAddress[] ipAddressList = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in ipAddressList)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        public void Stop()
        {
            listener.CloseListen();
            listener = null;
            Log.Info("Udp listener stopped...");
        }

        private void listener_ReceiveHandler(object sender, ReceiveEventArg e)
        {
            Record record = e.record;
            var logInfo = DeviceAccessLogMapper.ToModel(record);
            var deviceAccessEvent = new DeviceTrafficEvent() { DeviceAccessLog = logInfo };
            var message = DataContractSerializationHelper.Serialize(deviceAccessEvent);
            Log.Info(message);

            var logInfo2 = DeviceAdminLogMapper.ToModel(record);
            var message2 = DataContractSerializationHelper.Serialize(logInfo);
            Log.Info(message2);

            ReportMessage(message);
        }
    }
}