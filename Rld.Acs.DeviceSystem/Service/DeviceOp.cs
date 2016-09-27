using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Repository.Mybatis.MsSql;
using Rld.Acs.Unility.Extension;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.Device;
using Rld.DeviceSystem.Contract.Model.Services.DeviceConn;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.Acs.DeviceSystem.Service
{

    public class DeviceOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<int> SearchNewDevices()
        {
            var newDeviceIds = new List<int>();

            var registerClients = WebSocketClientManager.GetInstance().GetAllClients();
            if (registerClients.Any())
            {
                var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                var existDevices = repo.Query(new Hashtable() { { "Status", "1" } });

                var registerDeviceIds = registerClients.Select(x => x.Id);
                var existDeviceIds = existDevices.Select(x => x.Code.ToInt32());

                newDeviceIds = registerDeviceIds.Except(existDeviceIds).ToList();

            }
            return newDeviceIds;
        }

        public DeviceController SyncDeviceToSystem(int deviceId)
        {
            var systemInfo = GetSystemInfo(deviceId);
            var deviceInfo = GetDeviceInfo(deviceId);

            if (systemInfo == null || deviceInfo == null)
            {
                throw new Exception("system info or device info is emtpy. break process.");
            }

            var deviceControllerParameterRepo = RepositoryManager.GetRepository<IDeviceControllerParameterRepository>();
            var deviceDoorRepo = RepositoryManager.GetRepository<IDeviceDoorRepository>();
            var deviceHeadReadingRepo = RepositoryManager.GetRepository<IDeviceHeadReadingRepository>();
            var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

            //device controller
            var deviceController = ToDeviceController(deviceId, systemInfo);

            //device parameter
            deviceController.DeviceControllerParameter = ToDeviceParameter(deviceInfo);

            //device door
            var doorInfos = deviceInfo.Services.OfType<DoorInfo>();
            if (doorInfos.Any())
            {
                doorInfos.ForEach(x => deviceController.DeviceDoors.Add(ToDeviceDoor(x, deviceInfo)));
            }

            deviceControllerParameterRepo.Insert(deviceController.DeviceControllerParameter);
            deviceRepo.Insert(deviceController);

            deviceController.DeviceDoors.ForEach(a => a.DeviceID = deviceController.DeviceID);
            deviceController.DeviceDoors.ForEach(d => deviceDoorRepo.Insert(d));

            return deviceController;
        }

        private static DeviceController ToDeviceController(int deviceId, SystemInfo systemInfo)
        {
            var deviceController = new DeviceController();
            deviceController.Mac = systemInfo.Mac;
            deviceController.Name = "未命名新设备";
            deviceController.Code = deviceId.ToString();
            deviceController.SN = systemInfo.SerialNumber;
            deviceController.Model = systemInfo.DeviceModel;
            deviceController.CommunicationType = (int)Rld.Acs.Model.CommunicationType.TCP_IP;
            deviceController.Password = systemInfo.Password;
            deviceController.Label = "";
            deviceController.ServerURL = "";
            deviceController.Remark = "";
            deviceController.Protocol = 0; // set to websocket by default
            deviceController.CreateUserID = Global.DeviceSystemOperatorId;
            deviceController.CreateDate = DateTime.Now;
            deviceController.Status = GeneralStatus.Enabled;

            var serialConnection = systemInfo.Services.OfType<SerialConnectionService>();
            if (serialConnection.Any())
            {
                deviceController.BaudRate = serialConnection.First().Baudrate.ToString();
                deviceController.SerialPort = serialConnection.First().Port.ToString();
            }

            var tcpConnection = systemInfo.Services.OfType<TcpConnectionService>();
            if (tcpConnection.Any())
            {
                deviceController.IP = tcpConnection.First().IpAddress;
                deviceController.Port = tcpConnection.First().Port.ToString();
            }
            return deviceController;
        }

        private DeviceControllerParameter ToDeviceParameter(DeviceInfo deviceInfo)
        {
            var deviceExtraInfo = new DeviceControllerParameter();
            deviceExtraInfo.AuthticationType = deviceInfo.AuthticationType ?? 2;
            // if device return null, set to figure print as default.
            deviceExtraInfo.AntiPassbackEnabled = deviceInfo.AntiPassbackEnabled ?? false;

            var unlockOpenBehavior = deviceInfo.Services.OfType<DoorUnlockOpenBehaviorService>();
            if (unlockOpenBehavior.Any())
            {
                if (unlockOpenBehavior.First().Enabled)
                    deviceExtraInfo.UnlockOpenTimeZone = unlockOpenBehavior.First().TimezoneId;
            }

            var multiPersionLock = deviceInfo.Services.OfType<MultiPersionLockService>();
            if (multiPersionLock.Any())
            {
                deviceExtraInfo.MultiPersonLock = multiPersionLock.First().Enabled;
            }

            var doorLinkageService = deviceInfo.Services.OfType<DoorLinkageService>();
            if (doorLinkageService.Any())
            {
                deviceExtraInfo.DoorLinkageEnabled = doorLinkageService.First().Enabled;
            }

            var duressService = deviceInfo.Services.OfType<DuressService>();
            if (duressService.Any())
            {
                deviceExtraInfo.DuressEnabled = duressService.First().Enabled;
                deviceExtraInfo.DuressFingerPrintIndex = duressService.First().FingerPrintIndex ?? 0;
                deviceExtraInfo.DuressOpen = duressService.First().IsOpenDoor ?? false;
                deviceExtraInfo.DuressAlarm = duressService.First().IsTriggerAlarm ?? false;
                deviceExtraInfo.DuressPassword = duressService.First().Password;
            }
            return deviceExtraInfo;
        }

        private DeviceDoor ToDeviceDoor(DoorInfo doorInfo, DeviceInfo deviceInfo)
        {
            var deviceDoor = new DeviceDoor();
            deviceDoor.Name = doorInfo.Name;
            deviceDoor.Code = doorInfo.DoorIndex.ToString();
            deviceDoor.DoorIndex = doorInfo.DoorIndex;
            deviceDoor.ElectricalAppliances = doorInfo.ElectricalAppliances;
            deviceDoor.CheckOutOptions = doorInfo.CheckOutAction.HasValue ? (int)doorInfo.CheckOutAction : 0;
            deviceDoor.Status = 1;
            deviceDoor.Remark = doorInfo.Remark;
            deviceDoor.RingType = doorInfo.AlertType;

            var doorLinkageAlarms = deviceInfo.Services.OfType<DoorLinkageService>();
            if (doorLinkageAlarms.Any())
            {
                var doorLinkageAlarm = doorLinkageAlarms.First();
                if (doorLinkageAlarm.Enabled && doorLinkageAlarm.AlarmOption.HasValue)
                {
                    if (doorInfo.DoorType == DoorType.Master)
                        deviceDoor.LinkageAlarm = (doorLinkageAlarm.AlarmOption == DoorLinkageOptions.All ||
                                                   doorLinkageAlarm.AlarmOption == DoorLinkageOptions.Master);
                    else if (doorInfo.DoorType == DoorType.Slave)
                        deviceDoor.LinkageAlarm = (doorLinkageAlarm.AlarmOption == DoorLinkageOptions.All ||
                                                   doorLinkageAlarm.AlarmOption == DoorLinkageOptions.Slave);
                }
            }

            var doorOpenBehaviorServices = deviceInfo.Services.OfType<DoorOpenBehaviorService>();
            if (doorOpenBehaviorServices.Any())
            {
                var delayOpen = doorOpenBehaviorServices.FirstOrDefault(x => x.Type == DoorOpenBehavior.DelayOpen);
                if (delayOpen != null && delayOpen.Enabled)
                    deviceDoor.DelayOpenSeconds = delayOpen.Seconds;

                var overTimeOpen = doorOpenBehaviorServices.FirstOrDefault(x => x.Type == DoorOpenBehavior.OverTimeOpen);
                if (overTimeOpen != null && overTimeOpen.Enabled)
                    deviceDoor.OverTimeOpenSeconds = overTimeOpen.Seconds;

                var illegalOpen = doorOpenBehaviorServices.FirstOrDefault(x => x.Type == DoorOpenBehavior.IllegalOpen);
                if (illegalOpen != null && illegalOpen.Enabled)
                    deviceDoor.IllegalOpenSeconds = illegalOpen.Seconds;
            }

            var duressDoorService = deviceInfo.Services.OfType<DuressService>();
            if (duressDoorService.Any())
            {
                deviceDoor.DuressEnabled = duressDoorService.First().Enabled;
                deviceDoor.DuressFingerPrintIndex = duressDoorService.First().FingerPrintIndex ?? 0;
                deviceDoor.DuressOpen = duressDoorService.First().IsOpenDoor ?? false;
                deviceDoor.DuressAlarm = duressDoorService.First().IsTriggerAlarm ?? false;
                deviceDoor.DuressPassword = duressDoorService.First().Password;
            }

            return deviceDoor;
        }

        public SystemInfo GetSystemInfo(Int32 deviceId)
        {
            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceId);
            var getSystemInfoRequest = new GetSystemInfoRequest() { Token = operation.Token };
            string rawRequest = DataContractSerializationHelper.Serialize(getSystemInfoRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);

            var response = DataContractSerializationHelper.Deserialize<GetSystemInfoResponse>(rawResponse);
            Log.InfoFormat("Get system info for device id:[{0}], result:[{1}]", deviceId, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Get system info for device id:[{0}], result:[{1}]", deviceId, response.ResultType));
            }

            return response.SystemInfo;
        }

        public DeviceInfo GetDeviceInfo(Int32 deviceId)
        {
            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceId);
            var getDeviceInfoRequest = new GetDeviceInfoRequest() { Token = operation.Token };
            string rawRequest = DataContractSerializationHelper.Serialize(getDeviceInfoRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);

            var response = DataContractSerializationHelper.Deserialize<GetDeviceInfoResponse>(rawResponse);
            Log.InfoFormat("Get device info for device id:[{0}], result:[{1}]", deviceId, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Get device info for device id:[{0}], result:[{1}]", deviceId, response.ResultType));
            }

            return response.Service;
        }
    }
}