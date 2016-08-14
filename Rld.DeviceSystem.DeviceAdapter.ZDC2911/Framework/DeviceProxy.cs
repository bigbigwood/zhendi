using System;
using log4net;
using Riss.Devices;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Configuration;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.LogOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.SystemOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.UserOperation;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework
{
    internal class DeviceProxy
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Device Device { get; set; }
        public DeviceConnection DeviceConnection { get; set; }

        public DeviceProxy(DeviceConfigurationAdapter deviceConfig)
        {
            Device = new Device()
            {
                DN = deviceConfig.DeviceId,
                Password = deviceConfig.Password,
                Model = deviceConfig.DeviceModel,
                ConnectionModel = deviceConfig.ConnectionModel,
                CommunicationType = CommunicationType.Tcp,
                IpAddress = deviceConfig.TcpAddress,
                IpPort = deviceConfig.TcpPort,
            };
        }

        public string ProcessReceiveEvent(string message)
        {
            if (message.Contains("GetUserInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetUserInfoRequest>(message);
                var response = new GetUserOp().Process(request);
                return DataContractSerializationHelper.Serialize(response);
            }
            else if (message.Contains("UpdateUserInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<UpdateUserInfoRequest>(message);
                var response = new UpdateUserOp().Process(request);
                return DataContractSerializationHelper.Serialize(response);
            }
            else if (message.Contains("CreateUserInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<CreateUserInfoRequest>(message);
                var response = new CreateUserOp().Process(request);
                return DataContractSerializationHelper.Serialize(response);
            }
            else if (message.Contains("DeleteUserInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<DeleteUserInfoRequest>(message);
                var response = new DeleteUserOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }

            else if (message.Contains("GetSystemInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetSystemInfoRequest>(message);
                var response = new GetSystemInfoOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("UpdateSystemInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<UpdateSystemInfoRequest>(message);
                var response = new UpdateSystemInfoOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }

            else if (message.Contains("GetDeviceInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetDeviceInfoRequest>(message);
                var response = new GetDeviceInfoOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("UpdateDeviceInfoRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<UpdateDeviceInfoRequest>(message);
                var response = new UpdateDeviceInfoOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }

            else if (message.Contains("GetAllTimeSegmentsRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetAllTimeSegmentsRequest>(message);
                var response = new GetAllTimeSegmentsOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("BatchUpdateTimeSegmentsRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<BatchUpdateTimeSegmentsRequest>(message);
                var response = new BatchUpdateTimeSegmentsOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("GetAllTimeGroupsRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetAllTimeGroupsRequest>(message);
                var response = new GetAllTimeGroupsOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("BatchUpdateTimeGroupsRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<BatchUpdateTimeGroupsRequest>(message);
                var response = new BatchUpdateTimeGroupsOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("GetAllTimeZonesRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetAllTimeZonesRequest>(message);
                var response = new GetAllTimeZonesOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("BatchUpdateTimeZonesRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<BatchUpdateTimeZonesRequest>(message);
                var response = new BatchUpdateTimeZonesOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }

            else if (message.Contains("GetDeviceTrafficLogRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetDeviceTrafficLogRequest>(message);
                var response = new GetDeviceTrafficLogOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }
            else if (message.Contains("GetDeviceOperationLogRequest"))
            {
                var request = DataContractSerializationHelper.Deserialize<GetDeviceOperationLogRequest>(message);
                var response = new GetDeviceOperationLogOp().Process(request);
                return (DataContractSerializationHelper.Serialize(response));
            }

            return string.Empty;
        }


        public void OpenConnection()
        {
            var retryablePolicy = Policies.GetRetryablePolicy();
            bool result = retryablePolicy.Execute(TryOpenConnection);

            if (!result)
            {
                throw new Exception(string.Format("Open connection for device #{0} fails", Device.DN));
            }

            Log.InfoFormat("Open connection for device #{0} successfully", Device.DN);
        }

        private bool TryOpenConnection()
        {
            var myDevice = Device;
            var deviceConnection = DeviceConnection.CreateConnection(ref myDevice);
            if (deviceConnection.Open() > 0)
            {
                DeviceConnection = deviceConnection;
                Device = myDevice;
                return true;
            }
            return false;
        }

        public void CloseConnection()
        {
            DeviceConnection.Close();
            Log.InfoFormat("Close connection for device #{0} successfully", Device.DN);
        }
    }
}