using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Rld.Acs.Backend.DeviceSystem;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;


namespace Rld.Acs.Backend.Jobs
{
    class DataSyncJob : JobBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string LastDataSyncJobScanTimeString = "DataSyncJobScanTime";

        protected override void ProcessBusiness(IJobExecutionContext context)
        {
            Log.Info("Start data sync processing...");

            var syncConfigTime = TryGetSyncDataConfigTime();
            if (syncConfigTime.HasValue)
            {
                Log.Info("Get last data sync job scan time.");
                var sysConfigRepo = RepositoryManager.GetRepository<ISysConfigRepository>();
                var lastDataSyncJobScanTimeConfig = sysConfigRepo.Query(new Hashtable { { ConstStrings.Name, LastDataSyncJobScanTimeString } }).FirstOrDefault();
                DateTime lastDataSyncJobScanTime = DateTime.Parse(lastDataSyncJobScanTimeConfig.Value);

                var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                var devices = deviceRepo.Query(new Hashtable() { { "Status", (int)GeneralStatus.Enabled } }).ToArray();

                var eventRepo = RepositoryManager.GetRepository<IUserEventRepository>();
                var systemUserEvents = eventRepo.Query(new Hashtable() { { "IsFinished", false } });

                var deviceUserEvents = GetDeviceOperationEvents(devices, lastDataSyncJobScanTime, syncConfigTime.Value);

                SyncToDevice(devices, systemUserEvents);

                SyncToSystem(devices, deviceUserEvents);

                Log.Info("Update last data sync job scan time.");
                lastDataSyncJobScanTimeConfig.Value = syncConfigTime.ToString();
                sysConfigRepo.Update(lastDataSyncJobScanTimeConfig);
            }
        }

        private void SyncToDevice(IEnumerable<DeviceController> devices, IEnumerable<UserEvent> systemUserEvents)
        {
            Log.Info("Start sync data to device...");
            var proxy = new DeviceServiceClient();
            var userRepo = RepositoryManager.GetRepository<IUserRepository>();

            foreach (var systemUserEvent in systemUserEvents)
            {
                try
                {
                    Log.InfoFormat("Start sync user id={0} to device.", systemUserEvent.UserID);
                    string[] message;
                    var option = GetSyncOption(systemUserEvent);
                    var userInfo = userRepo.GetByKey(systemUserEvent.UserID);
                    var result = proxy.SyncDeviceUsers(devices.ToArray(), option, new[] { userInfo }, out message);
                    if (result != ResultTypes.Ok)
                    {
                        string errorMessage = string.Join(",", message);
                        throw new Exception("sync device user fails." + errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    Log.InfoFormat("Sync user id={0} to device fails.", systemUserEvent.UserID);
                    Log.Error(ex);
                }

                Log.InfoFormat("Finish sync user id={0} to device.", systemUserEvent.UserID);
            }
        }

        private void SyncToSystem(IEnumerable<DeviceController> devices, IEnumerable<DeviceOperationLog> deviceUserEvents)
        {
            Log.Info("Start sync data to system...");
            var proxy = new DeviceServiceClient();
            var userRepo = RepositoryManager.GetRepository<IUserRepository>();

            //filter out duplicated and non user operations.
            var userEvents = deviceUserEvents.GroupBy(x => x.DeviceUserId).Select(e => e.FirstOrDefault(x => x.DeviceUserId != 0));

            foreach (var deviceUserEvent in userEvents)
            {
                int userId = 0;
                Log.InfoFormat("user code={0}.", deviceUserEvent.DeviceUserId);
                try
                {
                    var conditions = new Hashtable() { { "UserCode", deviceUserEvent.DeviceUserId} };
                    var userInfo = userRepo.QueryUsersForSummaryData(conditions).FirstOrDefault();
                    if (userInfo != null)
                    {
                        userId = userInfo.UserID;
                    }
                    else
                    {
                        userInfo = new User() { UserID = 0, UserCode = deviceUserEvent.DeviceUserId.ToString() };
                    }

                    Log.InfoFormat("Start sync user id={0} to system.", userId);
                    string[] message;
                    var result = proxy.SyncSystemUsers(devices.ToArray(), new[] { userInfo }, out message);
                    if (result != ResultTypes.Ok)
                    {
                        string errorMessage = string.Join(",", message);
                        throw new Exception("sync system user fails." + errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    Log.InfoFormat("Sync user id={0} to system fails.", userId);
                    Log.Error(ex);
                }

                Log.InfoFormat("Finish sync user id={0} to system.", userId);
            }
        }

        private SyncOption GetSyncOption(UserEvent userEvent)
        {
            switch (userEvent.EventType)
            {
                case UserEventType.Add:
                    return SyncOption.Create;
                    break;
                case UserEventType.Modify:
                    return SyncOption.Update;
                    break;
                case UserEventType.Delete:
                    return SyncOption.Delete;
                    break;
                default:
                    return SyncOption.Unknown;
                    break;
            }
        }

        private DateTime? TryGetSyncDataConfigTime()
        {
            var sysConfigRepo = RepositoryManager.GetRepository<ISysConfigRepository>();

            var config = sysConfigRepo.Query(new Hashtable()).FirstOrDefault(x => x.Name == ConstStrings.DataSyncConfig);
            if (config != null)
            {
                var configs = config.Value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                return configs.Select(c => DateTime.Parse(c))
                    .FirstOrDefault(mydt => mydt.Hour == DateTime.Now.Hour && mydt.Minute == DateTime.Now.Minute);
            }

            return null;
        }

        private IEnumerable<DeviceOperationLog> GetDeviceOperationEvents(IEnumerable<DeviceController> devices, DateTime startTime, DateTime endTime)
        {
            var events = new List<DeviceOperationLog>();

            string[] messages;
            var resultTypes =  new DeviceServiceClient().SyncDeviceOperationLogs(devices.ToArray(), out messages);
            Log.InfoFormat("SyncDeviceOperationLogs result = {0}", resultTypes.ToString());

            if (resultTypes == ResultTypes.Ok)
            {
                var conditions = new Hashtable()
                {
                    {"StartDate",startTime},
                    {"EndDate", endTime},
                };

                var deviceOperationLogRepo = RepositoryManager.GetRepository<IDeviceOperationLogRepository>();
                events = deviceOperationLogRepo.Query(conditions).ToList();
            }

            return events;
        }
    }
}
