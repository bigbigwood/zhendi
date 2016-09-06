using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;

namespace Rld.Acs.Backend.Jobs
{
    class DeviceAlarmJob : IJob
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysConfigRepository sysConfigRepo = RepositoryManager.GetRepository<ISysConfigRepository>();

        public virtual void Execute(IJobExecutionContext context)
        {
            try
            {
                Log.Info("Trigger DeviceAlarmJob...");
                Stopwatch timer = Stopwatch.StartNew();

                int mvnoId = (int)context.JobDetail.JobDataMap.Get("MvnoId");
                if (mvnoId == null || mvnoId == 0)
                {
                    Log.Warn("Cannot find mvnoid, break process.");
                    return;
                }



                var alarmEmailAccountsConfig = sysConfigRepo.Query(new Hashtable {{ConstStrings.Name, ConstStrings.AlarmEmailAccounts}}).FirstOrDefault();
                var alarmEmailAccounts = ParseAccounts(alarmEmailAccountsConfig);

                var alarmSmsAccountsConfig = sysConfigRepo.Query(new Hashtable { { ConstStrings.Name, ConstStrings.AlarmSMSAccounts } }).FirstOrDefault();
                var alarmSmsAccounts = ParseAccounts(alarmSmsAccountsConfig);

                timer.Stop();
                Log.InfoFormat("DeviceAlarmJob complete! It cost {0} milliseconds", timer.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private List<String> ParseAccounts(SysConfig config)
        {
            List<string> result = new List<string>();
            if (config == null || string.IsNullOrWhiteSpace(config.Value))
                return result;

            return config.Value.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
