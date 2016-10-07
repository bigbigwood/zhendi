using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Message;
using Rld.Acs.DeviceSystem.Service;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.DeviceSystem.Contract.Message;
using GetDoorStateRequest = Rld.Acs.DeviceSystem.Message.GetDoorStateRequest;
using GetDoorStateResponse = Rld.Acs.DeviceSystem.Message.GetDoorStateResponse;
using UpdateDoorStateRequest = Rld.Acs.DeviceSystem.Message.UpdateDoorStateRequest;
using UpdateDoorStateResponse = Rld.Acs.DeviceSystem.Message.UpdateDoorStateResponse;


namespace Rld.Acs.DeviceSystem
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class DeviceService : IDeviceService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Int32 index = 0;
        public SyncDeviceUsersResponse SyncDeviceUsers(SyncDeviceUsersRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                try
                {
                    request.Users.ForEach(user => request.Devices.ForEach(device =>
                    {
                        new DeviceUserOp().SyncUser(request.Option, user, device);
                    }));

                    return new SyncDeviceUsersResponse() { ResultType = ResultTypes.Ok };
                }
                catch (DeviceNotConnectedException dex)
                {
                    return new SyncDeviceUsersResponse() { ResultType = ResultTypes.DeviceNotConnected };
                }
            });
        }

        public SyncDBUsersResponse SyncSystemUsers(SyncDBUsersRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                try
                {
                    request.Users.ForEach(user => request.Devices.ForEach(device =>
                    {
                        new SystemUserOp().SyncUser(user, device);
                    }));

                    return new SyncDBUsersResponse() { ResultType = ResultTypes.Ok };
                }
                catch (DeviceNotConnectedException dex)
                {
                    return new SyncDBUsersResponse() { ResultType = ResultTypes.DeviceNotConnected };
                }
            });
        }

        public SyncDepartmentUsersResponse SyncDepartmentUsers(SyncDepartmentUsersRequest request)
        {
            try
            {
                if ((request.Departments == null || !request.Departments.Any()) ||
                    request.Devices == null || !request.Devices.Any())
                {
                    return new SyncDepartmentUsersResponse() { ResultType = ResultTypes.Ok };
                }

                return PersistenceOperation.Process(request, () =>
                {
                    request.Departments.ForEach(department => new DepartmentOp().SyncDepartment(department, request.Devices));

                    return new SyncDepartmentUsersResponse() { ResultType = ResultTypes.Ok };
                });
            }
            catch (DeviceNotConnectedException dex)
            {
                return new SyncDepartmentUsersResponse() { ResultType = ResultTypes.DeviceNotConnected };
            }
        }

        public SyncDeviceOperationLogsResponse SyncDeviceOperationLogs(SyncDeviceOperationLogsRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                try
                {
                    request.Devices.ForEach(d =>
                    {
                        var repo = RepositoryManager.GetRepository<IDeviceOperationLogRepository>();
                        var logs = new OperationLogOp().QueryNewOperationLogs(d.DeviceID);
                        foreach (var deviceOperationLog in logs)
                        {
                            repo.Insert(deviceOperationLog);
                        }
                    });

                    return new SyncDeviceOperationLogsResponse() { ResultType = ResultTypes.Ok };
                }
                catch (DeviceNotConnectedException dex)
                {
                    return new SyncDeviceOperationLogsResponse() { ResultType = ResultTypes.DeviceNotConnected };
                }
            });

        }

        public SyncDeviceTrafficLogsResponse SyncDeviceTrafficLogs(SyncDeviceTrafficLogsRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                try
                {
                    request.Devices.ForEach(d =>
                    {
                        var repo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();
                        var logs = new TrafficLogOp().QueryNewTrafficLogs(d.DeviceID);
                        foreach (var deviceOperationLog in logs)
                        {
                            repo.Insert(deviceOperationLog);
                        }
                    });

                    return new SyncDeviceTrafficLogsResponse() { ResultType = ResultTypes.Ok };
                }
                catch (DeviceNotConnectedException dex)
                {
                    return new SyncDeviceTrafficLogsResponse() { ResultType = ResultTypes.DeviceNotConnected };
                }
            });
        }



        public SyncTimeGroupsResponse SyncTimeGroups(SyncTimeGroupsRequest request)
        {
            throw new NotImplementedException();
        }

        public SyncTimeSegmentsResponse SyncTimeSegments(SyncTimeSegmentsRequest request)
        {
            throw new NotImplementedException();
        }

        public SyncTimeZonesResponse SyncTimeZones(SyncTimeZonesRequest request)
        {
            throw new NotImplementedException();
        }
        public GetDoorStateResponse GetDoorState(GetDoorStateRequest request)
        {
            if (WebSocketClientManager.GetInstance().GetClientById(request.DeviceCode) == null)
                return new GetDoorStateResponse() { ResultType = ResultTypes.DeviceNotConnected };

            var bOpened = new DoorStateOp().GetDoorState(request.DeviceCode, request.DoorIndex);
            return new GetDoorStateResponse() { ResultType = ResultTypes.Ok, IsOpened = bOpened };
        }
        public UpdateDoorStateResponse UpdateDoorState(UpdateDoorStateRequest request)
        {
            if (WebSocketClientManager.GetInstance().GetClientById(request.DeviceCode) == null)
                return new UpdateDoorStateResponse() { ResultType = ResultTypes.DeviceNotConnected };

            var op = new DoorStateOp();
            var resultTypes = op.UpdateDoorState(request.DeviceCode, request.DoorIndex, request.Option);

            return new UpdateDoorStateResponse() { ResultType = (resultTypes == ResultType.NotSupport) ? ResultTypes.NotSupportError : ResultTypes.Ok };
        }

        public SearchNewDevicesResponse SearchNewDevices(SearchNewDevicesRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                var op = new DeviceOp();
                var newDeviceIds = op.SearchNewDevices();
                return new SearchNewDevicesResponse() { ResultType = ResultTypes.Ok, NewDeviceCodes = newDeviceIds };
            });
        }

        public SyncDevicesResponse SyncDevices(SyncDevicesRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                var newDeviceControllers = new List<DeviceController>();
                var op = new DeviceOp();
                var newDeviceIds = op.SearchNewDevices();
                newDeviceIds.ForEach(x =>
                {
                    Log.InfoFormat("sync device id={0} to system", x);
                    newDeviceControllers.Add(op.SyncDeviceToSystem(x));
                    Log.Info("sync device to system successfully");
                });

                return new SyncDevicesResponse() { ResultType = ResultTypes.Ok, NewDeviceControllers = newDeviceControllers };
            });
        }
    }
}
