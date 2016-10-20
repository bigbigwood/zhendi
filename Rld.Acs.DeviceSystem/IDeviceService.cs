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
        SyncDBUsersResponse SyncSystemUsers(SyncDBUsersRequest request);
        [OperationContract]
        SyncDepartmentUsersResponse SyncDepartmentUsers(SyncDepartmentUsersRequest request);
        [OperationContract]
        SyncDeviceUsersResponse SyncDeviceUsers(SyncDeviceUsersRequest request);
        [OperationContract]
        SyncDevicesResponse SyncDevices(SyncDevicesRequest request);
        [OperationContract]
        SyncDeviceTrafficLogsResponse SyncDeviceTrafficLogs(SyncDeviceTrafficLogsRequest request);
        [OperationContract]
        SyncDeviceOperationLogsResponse SyncDeviceOperationLogs(SyncDeviceOperationLogsRequest request);
        [OperationContract]
        SyncTimeGroupsResponse SyncTimeGroups(SyncTimeGroupsRequest request);
        [OperationContract]
        SyncTimeSegmentsResponse SyncTimeSegments(SyncTimeSegmentsRequest request);
        [OperationContract]
        SyncTimeZonesResponse SyncTimeZones(SyncTimeZonesRequest request);
        [OperationContract]
        GetDoorStateResponse GetDoorState(GetDoorStateRequest request);
        [OperationContract]
        UpdateDoorStateResponse UpdateDoorState(UpdateDoorStateRequest request);
        [OperationContract]
        SearchNewDevicesResponse SearchNewDevices(SearchNewDevicesRequest request);
        [OperationContract]
        QueryDeviceUsersResponse QueryDeviceUsers(QueryDeviceUsersRequest request);
    }
}
