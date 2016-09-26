using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Rld.Acs.Backend.Service.Email;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Framework;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
using StmpMailTest;
using Zhuoqin.HXSH.Web.Utility;

namespace Rld.Acs.Backend.Jobs
{
    class DataCleanJob : JobBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string StartDate = "2016-01-01";

        protected override void ProcessBusiness(IJobExecutionContext context)
        {
            Log.Info("Start data clean processing...");
            var sysConfigRepo = RepositoryManager.GetRepository<ISysConfigRepository>();

            var autoCleanConfig = sysConfigRepo.Query(new Hashtable()).FirstOrDefault(x => x.Name == ConstStrings.AutoCleanConfig);

            var configs = autoCleanConfig.Value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var sysLogExpireMonths = configs.First(x => x.StartsWith(ConstStrings.SysLogExpireMonths))
                .Substring(ConstStrings.SysLogExpireMonths.Length + 1).ToInt32();

            var deviceTrafficLogExpiredMonths = configs.First(x => x.StartsWith(ConstStrings.DeviceTrafficLogExpiredMonths))
                .Substring(ConstStrings.DeviceTrafficLogExpiredMonths.Length + 1).ToInt32();

            var deviceMngtLogExpiredMonths = configs.First(x => x.StartsWith(ConstStrings.DeviceMngtLogExpiredMonths))
                .Substring(ConstStrings.DeviceMngtLogExpiredMonths.Length + 1).ToInt32();

            var doorHistoryExpiredMonths = configs.First(x => x.StartsWith(ConstStrings.DoorHistoryExpiredMonths))
                .Substring(ConstStrings.DoorHistoryExpiredMonths.Length + 1).ToInt32();

            if (sysLogExpireMonths != 0)
                CleanExpiredSysLog(DateTime.Now.AddMonths(0 - sysLogExpireMonths));

            if (deviceTrafficLogExpiredMonths != 0)
                CleanDeviceTrafficLog(DateTime.Now.AddMonths(0 - deviceTrafficLogExpiredMonths));

            if (deviceMngtLogExpiredMonths != 0)
                CleanDeviceOperationLog(DateTime.Now.AddMonths(0 - deviceMngtLogExpiredMonths));

            if (doorHistoryExpiredMonths != 0)
                CleanDoorHistoryLog(DateTime.Now.AddMonths(0 - doorHistoryExpiredMonths));
        }

        private void CleanExpiredSysLog(DateTime expiredTime)
        {
            Log.Info("Start clean expired system log...");
            var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();

            var expiredLogs = repo.Query(new Hashtable { { "StartDate", StartDate }, { "EndDate", expiredTime.ToString() } });
            if (expiredLogs.Any())
            {
                var expiredIds = string.Join(",", expiredLogs.Select(x => x.LogID));
                Log.InfoFormat("Clean expired sys log id={0}", expiredIds);
                expiredLogs.ForEach(x => repo.Delete(x.LogID));
            }
            Log.Info("Finish clean expired system log...");
        }

        private void CleanDeviceTrafficLog(DateTime expiredTime)
        {
            Log.Info("Start clean expired device traffic log...");
            var repo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();

            var expiredLogs = repo.Query(new Hashtable { { "StartDate", StartDate }, { "EndDate", expiredTime.ToString() } });
            if (expiredLogs.Any())
            {
                var expiredIds = string.Join(",", expiredLogs.Select(x => x.TrafficID));
                Log.InfoFormat("Clean expired device traffic id={0}", expiredIds);
                expiredLogs.ForEach(x => repo.Delete(x.TrafficID));
            }
            Log.Info("Finish clean expired device traffic log...");
        }

        private void CleanDeviceOperationLog(DateTime expiredTime)
        {
            Log.Info("Start clean expired device operation log...");
            var repo = RepositoryManager.GetRepository<IDeviceOperationLogRepository>();

            var expiredLogs = repo.Query(new Hashtable { { "StartDate", StartDate }, { "EndDate", expiredTime.ToString() } });
            if (expiredLogs.Any())
            {
                var expiredIds = string.Join(",", expiredLogs.Select(x => x.LogID));
                Log.InfoFormat("Clean expired device operation id={0}", expiredIds);
                expiredLogs.ForEach(x => repo.Delete(x.LogID));
            }
            Log.Info("Finish clean expired device operation log...");
        }

        private void CleanDoorHistoryLog(DateTime expiredTime)
        {
            Log.Info("Start clean expired door history log...");
            var repo = RepositoryManager.GetRepository<IDeviceStateHistoryRepository>();

            var expiredLogs = repo.Query(new Hashtable { { "StartDate", StartDate }, { "EndDate", expiredTime.ToString() } });
            if (expiredLogs.Any())
            {
                var expiredIds = string.Join(",", expiredLogs.Select(x => x.DeviceStateHistoryID));
                Log.InfoFormat("Clean expired door history id={0}", expiredIds);
                expiredLogs.ForEach(x => repo.Delete(x.DeviceStateHistoryID));
            }
            Log.Info("Finish clean expired door history log...");
        }

    }
}
