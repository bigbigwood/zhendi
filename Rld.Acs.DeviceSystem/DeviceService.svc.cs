using System;
using System.Linq;
using System.Threading;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Message;
using Rld.Acs.DeviceSystem.Service;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;


namespace Rld.Acs.DeviceSystem
{
    public class DeviceService : IDeviceService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            throw new NotImplementedException();
        }
        public UpdateDoorStateResponse UpdateDoorState(UpdateDoorStateRequest request)
        {
            Thread.Sleep(3000);
            return new UpdateDoorStateResponse() { ResultType = ResultTypes.Ok};
        }
    }
}
