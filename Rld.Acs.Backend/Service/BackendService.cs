using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Quartz.Impl;
using Rld.Acs.Backend.Jobs;

namespace Rld.Acs.Backend.Service
{
    public class BackendService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IScheduler scheduler;
        public BackendService()
        {
            InitScheduleTasks();
        }

        public void OnStart()
        {
            scheduler.Start();
            Log.Info("BackendService started.");
        }

        public void OnStop()
        {
            scheduler.Shutdown();
            Log.Info("BackendService stopped.");
        }

        private void InitScheduleTasks()
        {
            var scheduleFactory = new StdSchedulerFactory();
            scheduler = scheduleFactory.GetScheduler();

            var deviceAlarmJobCronExp = ConfigurationManager.AppSettings["DeviceAlarmJobCronExp"];
            InitJob(scheduler, typeof(DeviceAlarmJob), deviceAlarmJobCronExp);

            var dataCleanJobCronExp = ConfigurationManager.AppSettings["DataCleanJobCronExp"];
            InitJob(scheduler, typeof(DataCleanJob), dataCleanJobCronExp);

            var dataSyncJobCronExp = ConfigurationManager.AppSettings["DataSyncJobCronExp"];
            InitJob(scheduler, typeof(DataSyncJob), dataSyncJobCronExp);

            Log.Info("Init schedule tasks completely.");
        }


        private void InitJob(IScheduler scheduler, Type jobType, string scheduleTime)
        {
            string jobName = string.Empty;

            try
            {
                jobName = string.Format("JobName_{0}", jobType.ToString());
                string triggerName = string.Format("TriggerName_{0}", jobType.ToString());
                string jobGroup = string.Format("JobGroup_{0}", jobType.ToString());
                string triggerGroup = string.Format("TriggerGroup_{0}", jobType.ToString());

                if (string.IsNullOrEmpty(scheduleTime))
                {
                    Log.InfoFormat("There's no schedule time configured for job: {0}, so the job will be skipped.", jobName);
                    return;
                }

                IJobDetail scheduleJobDetail = new JobDetailImpl(jobName, jobGroup, jobType);
                var scheduleTrigger = (ICronTrigger)TriggerBuilder.Create()
                                                                .WithIdentity(triggerName, triggerGroup)
                                                                .WithCronSchedule(scheduleTime)
                                                                .Build();

                scheduler.ScheduleJob(scheduleJobDetail, scheduleTrigger);

                Log.InfoFormat("Init the job successfully, jobName: {0}, schedule time: {1}", jobName, scheduleTime);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Init the job: {0} fails.", jobName);
                Log.Error(ex);
            }
        }
    }
}
