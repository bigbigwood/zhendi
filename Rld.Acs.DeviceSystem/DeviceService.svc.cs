using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Message;
using Rld.Acs.DeviceSystem.Service;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.Acs.DeviceSystem
{
    public class DeviceService : IDeviceService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SyncDeviceUsersResponse SyncDeviceUsers(SyncDeviceUsersRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                //var repo = RepositoryManager.GetRepository<IUserRepository>();
                //request.DbUsers.ForEach(user => request.DeviceControllers.ForEach(device =>
                //{
                //    var userInfo = repo.GetByKey(user.UserID);
                //    new UserOperation().UpdateDeviceUser(userInfo, device);
                //}));

                Thread.Sleep(10 * 1000);

                return new SyncDeviceUsersResponse() { ResultType = ResultTypes.Ok };
            });
        }

        public SyncDBUsersResponse SyncDBUsers(SyncDBUsersRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                //var repo = RepositoryManager.GetRepository<IUserRepository>();
                //request.DbUsers.ForEach(user => request.DeviceControllers.ForEach(device =>
                //{
                //    var userInfo = repo.GetByKey(user.UserID);
                //    new UserOperation().UpdateDeviceUser(userInfo, device);
                //}));

                Thread.Sleep(10*1000);

                return new SyncDBUsersResponse() { ResultType = ResultTypes.Ok };
            });
        }

        public SyncDepartmentUsersResponse SyncDepartmentUsers(SyncDepartmentUsersRequest request)
        {
            Thread.Sleep(3 * 1000);
            return new SyncDepartmentUsersResponse() { ResultType = ResultTypes.Ok };
        }

        public SyncDeviceOperationLogsResponse SyncDeviceOperationLogs(SyncDeviceOperationLogsRequest request)
        {
            throw new NotImplementedException();
        }

        public SyncDeviceTrafficLogsResponse SyncDeviceTrafficLogs(SyncDeviceTrafficLogsRequest request)
        {
            throw new NotImplementedException();
        }

        public SyncDoorStateLogsResponse SyncDoorStateLogs(SyncDoorStateLogsRequest request)
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
    }
}
