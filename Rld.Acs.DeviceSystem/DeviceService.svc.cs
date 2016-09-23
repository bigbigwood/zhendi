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
                var repo = RepositoryManager.GetRepository<IUserRepository>();
                request.Users.ForEach(user => request.Devices.ForEach(device =>
                {
                    var userInfo = repo.GetByKey(user.UserID);
                    new UserOp().UpdateDeviceUser(userInfo, device);
                }));

                return new SyncDeviceUsersResponse() { ResultType = ResultTypes.Ok };
            });
        }

        public SyncDBUsersResponse SyncDBUsers(SyncDBUsersRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                var repo = RepositoryManager.GetRepository<IUserRepository>();
                request.Users.ForEach(user => request.Devices.ForEach(device =>
                {
                    var userInfo = repo.GetByKey(user.UserID);
                    new UserOp().UpdateDBUser(userInfo, device);
                }));

                return new SyncDBUsersResponse() { ResultType = ResultTypes.Ok };
            });
        }

        public SyncDepartmentUsersResponse SyncDepartmentUsers(SyncDepartmentUsersRequest request)
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

        public SyncDeviceOperationLogsResponse SyncDeviceOperationLogs(SyncDeviceOperationLogsRequest request)
        {
            return PersistenceOperation.Process(request, () =>
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
            });
        }

        public SyncDeviceTrafficLogsResponse SyncDeviceTrafficLogs(SyncDeviceTrafficLogsRequest request)
        {
            throw new NotImplementedException();
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
            var bOpened = new DoorStateOp().GetDoorState(request.DeviceId, request.DoorIndex);
            return new GetDoorStateResponse() { ResultType = ResultTypes.Ok, IsOpened = bOpened };
        }
        public UpdateDoorStateResponse UpdateDoorState(UpdateDoorStateRequest request)
        {
            var op = new DoorStateOp();
            var resultTypes = op.UpdateDoorState(request.DeviceId, request.DoorIndex, request.Option);

            return new UpdateDoorStateResponse() { ResultType = (resultTypes == ResultType.NotSupport) ? ResultTypes.NotSupportError : ResultTypes.Ok};
        }

        public SearchNewDevicesResponse SearchNewDevices(SearchNewDevicesRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                var newDeviceIds = new List<String>();

                var registerClients = WebSocketClientManager.GetInstance().GetAllClients();
                if (registerClients.Any())
                {
                    var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                    var existDevices = repo.Query(new Hashtable() { { "Status", "1" } });

                    var registerDeviceIds = registerClients.Select(x => x.Id.ToString());
                    var existDeviceIds = existDevices.Select(x => x.Code);

                    newDeviceIds = registerDeviceIds.Except(existDeviceIds).ToList();
                }

                return new SearchNewDevicesResponse() { ResultType = ResultTypes.Ok, NewDeviceIds = newDeviceIds };
            });
        }

        public SyncDevicesResponse SyncDevices(SyncDevicesRequest request)
        {
             return new SyncDevicesResponse();
        }
    }
}
