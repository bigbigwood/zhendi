﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service
{
    public class FloorDoorManager
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceControllerRepository _deviceControllerRepo = NinjectBinder.GetRepository<IDeviceControllerRepository>();
        private IFloorRepository _floorRepo = NinjectBinder.GetRepository<IFloorRepository>();

        public List<DeviceDoor> AuthorizationDoors
        {
            get
            {
                var devices = _deviceControllerRepo.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled);
                return devices.SelectMany(x => x.DeviceDoors).ToList();
            }
        }
        public List<FloorDoor> AuthorizationFloorDoor
        {
            get
            {
                var floors = _floorRepo.Query(new Hashtable());
                return floors.SelectMany(x => x.Doors).ToList();
            }
        }
        private static FloorDoorManager _instance = null;

        public static FloorDoorManager GetInstance()
        {
            if (_instance == null)
                _instance = new FloorDoorManager();

            return _instance;
        }
        private FloorDoorManager()
        {
        }
    }
}
