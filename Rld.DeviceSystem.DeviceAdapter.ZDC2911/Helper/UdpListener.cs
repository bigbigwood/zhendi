using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using log4net;
using Riss.Devices;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper
{
    public class UdpListener
    {
        public static UdpListener _instance = null;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Zd2911Monitor listener;
        private UdpListener()
        {

        }

        public UdpListener GetInstance()
        {
            if (_instance == null)
                _instance = new UdpListener();

            return _instance;
        }

        public void Start()
        {
            Monitor m = new Monitor();
            m.UDPAddress = GetLocalIPAddress();
            m.UDPPort = 5055;
            m.Mode = 0;

            listener = Zd2911Monitor.CreateZd2911Monitor(m);
            listener.ReceiveHandler += new ReceiveHandler(listener_ReceiveHandler);
            listener.OpenListen();
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
        }

        private void listener_ReceiveHandler(object sender, ReceiveEventArg e)
        {
            Record record = e.record;
            string verify = ConvertObject.IOMode(record.Verify);
            string action = ConvertObject.GLogType(record.Action);

            Log.InfoFormat("{0} {1} {2} {3} {4}", record.DN.ToString(), record.DIN.ToString(), verify, action, record.Clock.ToString("yyyy-MM-dd HH:mm:ss"));
            //ListViewItem lvi = new ListViewItem(new string[]{no.ToString(), record.DN.ToString(), record.DIN.ToString(),
            //    string.Empty, verify, action, record.Clock.ToString("yyyy-MM-dd HH:mm:ss")});
            //BeginInvoke(new AddRecord(AddRecordToListView), new object[] { lvi });
            //no++;
        }
    }
}