using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Log;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper
{
    public class UdpListener
    {
        private static UdpListener _instance = null;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Zd2911Monitor listener;
        private UdpListener()
        {

        }

        public static UdpListener GetInstance()
        {
            if (_instance == null)
                _instance = new UdpListener();

            return _instance;
        }

        public void Start()
        {
            Log.Info("Starting Udp listener...");
            Monitor m = new Monitor();
            m.UDPAddress = GetLocalIPAddress();
            m.UDPPort = 5055;
            m.Mode = 0;

            listener = Zd2911Monitor.CreateZd2911Monitor(m);
            listener.ReceiveHandler += new ReceiveHandler(listener_ReceiveHandler);
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
            string verify = ConvertObject.IOMode(record.Verify);
            string action = ConvertObject.GLogType(record.Action);

            var logInfo = DeviceAccessLogMapper.ToModel(record);
            Log.InfoFormat("{0} {1} {2} {3} {4} {5}", logInfo.DeviceId, logInfo.UserId, logInfo.AccessLogType, logInfo.Message, logInfo.CheckInOptions, logInfo.CreateTime);

            //Log.InfoFormat("{0} {1} {2} {3} {4}", record.DN.ToString(), record.DIN.ToString(), verify, action, record.Clock.ToString("yyyy-MM-dd HH:mm:ss"));
            //ListViewItem lvi = new ListViewItem(new string[]{no.ToString(), record.DN.ToString(), record.DIN.ToString(),
            //    string.Empty, verify, action, record.Clock.ToString("yyyy-MM-dd HH:mm:ss")});
            //BeginInvoke(new AddRecord(AddRecordToListView), new object[] { lvi });
            //no++;
        }
    }
}