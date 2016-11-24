using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Message;
using Rld.Acs.DeviceSystem.Model;
using Rld.Acs.DeviceSystem.Service;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
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


        public SyncDeviceUsersResponse SyncDeviceUsers(SyncDeviceUsersRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                try
                {
                    var userRepo = RepositoryManager.GetRepository<IUserRepository>();
                    var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

                    Log.Info("Check offline devices");
                    var allDevices = deviceRepo.QuerySummaryData(new Hashtable() { { "Status", (int)GeneralStatus.Enabled } });
                    var offlineDevices = allDevices.FindAll(d => WebSocketClientManager.GetInstance().GetClientById(d.Code.ToInt32()) == null);
                    var onlineDevices = allDevices.Except(offlineDevices);

                    var requestOfflineDevice = offlineDevices.FindAll(x => request.Devices.Select(xx => xx.DeviceID).Contains(x.DeviceID));
                    if (requestOfflineDevice.Any())
                    {
                        var msg = string.Join(",", requestOfflineDevice.Select(x => x.Name));
                        return new SyncDeviceUsersResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
                    }

                    //if (offlineDevices.Any())
                    //{
                    //    var msg = string.Format("设备:[{0}]未连接", string.Join(",", offlineDevices.Select(x => x.Name)));
                    //    return new SyncDBUsersResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
                    //}

                    Log.Info("Load data for request devices...");
                    var requestDevices = new List<DeviceController>();
                    request.Devices.ForEach(x =>
                    {
                        var d = onlineDevices.FirstOrDefault(e => e.DeviceID == x.DeviceID);
                        if (d != null)
                            requestDevices.Add(d);
                    });
                    request.Devices = requestDevices;

                    Log.Info("Load data for request users...");
                    var requestUsers = new List<User>();
                    request.Users.ForEach(x =>
                    {
                        if (x.UserID != 0)
                        {
                            x = userRepo.GetByKey(x.UserID);
                        }
                        requestUsers.Add(x);
                    });
                    request.Users = requestUsers;

                    Log.Info("Process business...");
                    request.Users.ForEach(user => request.Devices.ForEach(device =>
                    {
                        new DeviceUserOp().SyncUser(request.Option, user, device);
                    }));

                    return new SyncDeviceUsersResponse() { ResultType = ResultTypes.Ok };
                }
                catch (Exception ex)
                {
                    return new SyncDeviceUsersResponse() { ResultType = ResultTypes.UnknownError, Messages = new[] { ex.Message } };
                }
            });
        }

        public SyncDBUsersResponse SyncSystemUsers(SyncDBUsersRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                try
                {
                    var userRepo = RepositoryManager.GetRepository<IUserRepository>();
                    var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

                    Log.Info("Check offline devices");
                    var allDevices = deviceRepo.QuerySummaryData(new Hashtable() { { "Status", (int)GeneralStatus.Enabled } });
                    var offlineDevices = allDevices.FindAll(d => WebSocketClientManager.GetInstance().GetClientById(d.Code.ToInt32()) == null);
                    var onlineDevices = allDevices.Except(offlineDevices);

                    var requestOfflineDevice = offlineDevices.FindAll(x => request.Devices.Select(xx => xx.DeviceID).Contains(x.DeviceID));
                    if (requestOfflineDevice.Any())
                    {
                        var msg = string.Join(",", requestOfflineDevice.Select(x => x.Name));
                        return new SyncDBUsersResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
                    }

                    //if (offlineDevices.Any())
                    //{
                    //    var msg = string.Format("设备:[{0}]未连接", string.Join(",", offlineDevices.Select(x => x.Name)));
                    //    return new SyncDBUsersResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
                    //}

                    Log.Info("Load data for request devices...");
                    var requestDevices = new List<DeviceController>();
                    request.Devices.ForEach(x =>
                    {
                        var d = onlineDevices.FirstOrDefault(e => e.DeviceID == x.DeviceID);
                        if (d != null)
                            requestDevices.Add(d);
                    });
                    request.Devices = requestDevices;

                    Log.Info("Load data for request users...");
                    var requestUsers = new List<User>();
                    request.Users.ForEach(x =>
                    {
                        if (x.UserID != 0)
                        {
                            x = userRepo.GetByKey(x.UserID);
                        }
                        requestUsers.Add(x);
                    });
                    request.Users = requestUsers;

                    Log.Info("Process business...");
                    request.Users.ForEach(user =>
                    {
                        Log.Info("Try to get user data from all devices...");
                        var deviceUserOp = new DeviceUserOp();
                        var userDeviceInfoDict = new Dictionary<int, UserInfo>();
                        var deviceListInSequence = onlineDevices.OrderBy(x => x.DeviceID);
                        foreach (var device in deviceListInSequence)
                        {
                            var tryGetDeviceUser = deviceUserOp.TryGetUesrInfo(user, device);
                            if (tryGetDeviceUser != null)
                                userDeviceInfoDict.Add(device.DeviceID, tryGetDeviceUser);
                        }

                        Log.Info("Try to update user data...");
                        var systemUserOp = new SystemUserOp(onlineDevices, userDeviceInfoDict);
                        systemUserOp.SyncUser(user, request.Devices);
                    });

                    return new SyncDBUsersResponse() { ResultType = ResultTypes.Ok };
                }
                catch (Exception ex)
                {
                    return new SyncDBUsersResponse() { ResultType = ResultTypes.UnknownError, Messages = new[] { ex.Message } };
                }
            });
        }

        public SyncDepartmentUsersResponse SyncDepartmentUsers(SyncDepartmentUsersRequest request)
        {
            throw new NotImplementedException();
        }

        public SyncDeviceOperationLogsResponse SyncDeviceOperationLogs(SyncDeviceOperationLogsRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                try
                {
                    var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

                    Log.Info("Check offline devices");
                    var allDevices = deviceRepo.QuerySummaryData(new Hashtable() { { "Status", (int)GeneralStatus.Enabled } });
                    var offlineDevices = allDevices.FindAll(d => WebSocketClientManager.GetInstance().GetClientById(d.Code.ToInt32()) == null);
                    var onlineDevices = allDevices.Except(offlineDevices);

                    var requestOfflineDevice = offlineDevices.FindAll(x => request.Devices.Select(xx => xx.DeviceID).Contains(x.DeviceID));
                    if (requestOfflineDevice.Any())
                    {
                        var msg = string.Join(",", requestOfflineDevice.Select(x => x.Name));
                        return new SyncDeviceOperationLogsResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
                    }

                    //if (offlineDevices.Any())
                    //{
                    //    var msg = string.Format("设备:[{0}]未连接", string.Join(",", offlineDevices.Select(x => x.Name)));
                    //    return new SyncDBUsersResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
                    //}

                    Log.Info("Load data for request devices...");
                    var requestDevices = new List<DeviceController>();
                    request.Devices.ForEach(x =>
                    {
                        var d = onlineDevices.FirstOrDefault(e => e.DeviceID == x.DeviceID);
                        if (d != null)
                            requestDevices.Add(d);
                    });
                    request.Devices = requestDevices;

                    var repo = RepositoryManager.GetRepository<IDeviceOperationLogRepository>();
                    var op = new OperationLogOp();
                    request.Devices.ForEach(d =>
                    {
                        var logs = op.QueryNewOperationLogs(d);
                        foreach (var deviceOperationLog in logs)
                        {
                            repo.Insert(deviceOperationLog);
                        }
                    });

                    return new SyncDeviceOperationLogsResponse() { ResultType = ResultTypes.Ok };
                }
                catch (Exception ex)
                {
                    return new SyncDeviceOperationLogsResponse() { ResultType = ResultTypes.UnknownError, Messages = new[] { ex.Message } };
                }
            });
        }

        public SyncDeviceTrafficLogsResponse SyncDeviceTrafficLogs(SyncDeviceTrafficLogsRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                try
                {
                    var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

                    Log.Info("Check offline devices");
                    var allDevices = deviceRepo.QuerySummaryData(new Hashtable() { { "Status", (int)GeneralStatus.Enabled } });
                    var offlineDevices = allDevices.FindAll(d => WebSocketClientManager.GetInstance().GetClientById(d.Code.ToInt32()) == null);
                    var onlineDevices = allDevices.Except(offlineDevices);

                    var requestOfflineDevice = offlineDevices.FindAll(x => request.Devices.Select(xx => xx.DeviceID).Contains(x.DeviceID));
                    if (requestOfflineDevice.Any())
                    {
                        var msg = string.Join(",", requestOfflineDevice.Select(x => x.Name));
                        return new SyncDeviceTrafficLogsResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
                    }

                    //if (offlineDevices.Any())
                    //{
                    //    var msg = string.Format("设备:[{0}]未连接", string.Join(",", offlineDevices.Select(x => x.Name)));
                    //    return new SyncDBUsersResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
                    //}

                    Log.Info("Load data for request devices...");
                    var requestDevices = new List<DeviceController>();
                    request.Devices.ForEach(x =>
                    {
                        var d = onlineDevices.FirstOrDefault(e => e.DeviceID == x.DeviceID);
                        if (d != null)
                            requestDevices.Add(d);
                    });
                    request.Devices = requestDevices;

                    var repo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();
                    var op = new TrafficLogOp();
                    request.Devices.ForEach(d =>
                    {
                        var logs = op.QueryNewTrafficLogs(d);
                        foreach (var deviceOperationLog in logs)
                        {
                            repo.Insert(deviceOperationLog);
                        }
                    });

                    return new SyncDeviceTrafficLogsResponse() { ResultType = ResultTypes.Ok };
                }
                catch (Exception ex)
                {
                    return new SyncDeviceTrafficLogsResponse() { ResultType = ResultTypes.UnknownError, Messages = new[] { ex.Message } };
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
            {
                var msg = string.Join(",", request.DeviceCode);
                return new GetDoorStateResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
            }

            var bOpened = new DoorStateOp().GetDoorState(request.DeviceCode, request.DoorIndex);
            return new GetDoorStateResponse() { ResultType = ResultTypes.Ok, IsOpened = bOpened };
        }
        public UpdateDoorStateResponse UpdateDoorState(UpdateDoorStateRequest request)
        {
            if (WebSocketClientManager.GetInstance().GetClientById(request.DeviceCode) == null)
            {
                var msg = string.Join(",", request.DeviceCode);
                return new UpdateDoorStateResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
            }

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

        public QueryDeviceUsersResponse QueryDeviceUsers(QueryDeviceUsersRequest request)
        {
            return PersistenceOperation.Process(request, () =>
            {
                if (WebSocketClientManager.GetInstance().GetClientById(request.Device.Code.ToInt32()) == null)
                {
                    var msg = string.Join(",", request.Device.Name);
                    return new QueryDeviceUsersResponse() { ResultType = ResultTypes.DeviceNotConnected, Messages = new[] { msg } };
                }

                var userDtos = new List<DeviceUserDto>();
                var deviceUsers = new List<UserInfo>();
                var op = new DeviceUserOp();

                if (!string.IsNullOrWhiteSpace(request.UserCode))
                {
                    var userRepo = RepositoryManager.GetRepository<IUserRepository>();
                    var userInfo = userRepo.QueryUsersForSummaryData(new Hashtable() { { "UserCode", request.UserCode } }).FirstOrDefault();
                    if (userInfo != null)
                    {
                        var deviceUser = op.TryGetUesrInfo(userInfo, request.Device);
                        if (deviceUser != null)
                            deviceUsers.Add(deviceUser);
                    }
                }
                else
                {
                    deviceUsers = op.QueryUsersByDevice(request.Device);
                }

                if (deviceUsers != null)
                {
                    deviceUsers.ForEach(x => userDtos.Add(new DeviceUserDto()
                    {
                        UserCode = x.UserId,
                        UserName = string.IsNullOrWhiteSpace(x.UserName) ? "未命名人员" : x.UserName,
                    }));
                }

                return new QueryDeviceUsersResponse() { ResultType = ResultTypes.Ok, Users = userDtos };
            });
        }
    }
}
