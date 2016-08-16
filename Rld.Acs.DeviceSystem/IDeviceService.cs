using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rld.Acs.DeviceSystem.Message;
using Rld.Acs.Model;
using Rld.DeviceSystem.Contract;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.Acs.DeviceSystem
{
    [ServiceContract(Namespace = Declarations.NameSpace)]
    public interface IDeviceService
    {
        [OperationContract]
        SyncDBUsersResponse SyncDBUsers(SyncDBUsersRequest request);
        [OperationContract]
        SyncDepartmentUsersResponse SyncDepartmentUsers(SyncDepartmentUsersRequest request);
        [OperationContract]
        SyncDeviceOperationLogsResponse SyncDeviceOperationLogs(SyncDeviceOperationLogsRequest request);

        [OperationContract]
        SyncDeviceTrafficLogsResponse SyncDeviceTrafficLogs(SyncDeviceTrafficLogsRequest request);

        [OperationContract]
        SyncDeviceUsersResponse SyncDeviceUsers(SyncDeviceUsersRequest request);

        [OperationContract]
        SyncDoorStateLogsResponse SyncDoorStateLogs(SyncDoorStateLogsRequest request);
        [OperationContract]
        SyncTimeGroupsResponse SyncTimeGroups(SyncTimeGroupsRequest request);
        [OperationContract]
        SyncTimeSegmentsResponse SyncTimeSegments(SyncTimeSegmentsRequest request);

        [OperationContract]
        SyncTimeZonesResponse SyncTimeZones(SyncTimeZonesRequest request);
    }
}
