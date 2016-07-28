﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Riss.Devices;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911
{
    public class DeviceAdapter
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static DeviceAdapter _instance = null;

        private Device _device;
        private DeviceConnection _deviceConnection;


        public static void Initialize()
        {
            Log.Info("Initializing ApplicationManager...");
            _instance = new DeviceAdapter();

            Log.Info("Initializing ApplicationManager Finish...");
        }

        public static DeviceAdapter GetInstance()
        {
            return _instance;
        }

        public bool OpenConnection()
        {
            Int32 deviceId = 1;
            string password = "0";
            string ip = "192.168.31.73";
            Int32 port = 5500;

            var device = new Device()
            {
                DN = deviceId,
                Password = password,
                Model = "ZDC2911",
                ConnectionModel = 5,
                CommunicationType = CommunicationType.Tcp,
                IpAddress = ip,
                IpPort = port,
            };

            var deviceConnection = DeviceConnection.CreateConnection(ref device);
            if (deviceConnection.Open() > 0)
            {
                _device = device;
                _deviceConnection = deviceConnection;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CloseConnection()
        {
            _deviceConnection.Close();
            return true;
        }
    }
}