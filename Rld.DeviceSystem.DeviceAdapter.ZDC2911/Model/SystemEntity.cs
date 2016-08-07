using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model
{
    public class SystemEntity
    {
        public String DeviceId { get; set; }
        public String Mac { get; set; }
        //public String DeviceCode { get; set; }
        public String SerialNumber { get; set; }
        public String Model { get; set; }
        /// <summary>
        /// does not found ,only can found it is using in setup the connection
        /// </summary>
        //public String CommunitionType { get; set; }
        
        public String BaudRate { get; set; }
        /// <summary>
        /// com 1-8, but it does not exist
        /// </summary>
        //public String SerialPort { get; set; }
        public String Password { get; set; }
        public String IP { get; set; }
        /// <summary>
        /// TCP port, but it use UDP port, pfff
        /// </summary>
        public String Port { get; set; }
        //public String Protocol { get; set; }
        //public String Label { get; set; }
        public String ServerURL { get; set; }
        public String ServerPort { get; set; }
    }
}