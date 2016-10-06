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
    class DataSyncJob : JobBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        protected override void ProcessBusiness(IJobExecutionContext context)
        {
            Log.Info("Start data sync processing...");

            if (IsTimeToSyncData())
            {
                SyncToSystem();

                SyncToDevice();
            }
        }

        private void SyncToDevice()
        {
            Log.Info("Start sync data to device...");
        }

        private void SyncToSystem()
        {
            Log.Info("Start sync data to system...");
        }

        private bool IsTimeToSyncData()
        {
            var sysConfigRepo = RepositoryManager.GetRepository<ISysConfigRepository>();

            var config = sysConfigRepo.Query(new Hashtable()).FirstOrDefault(x => x.Name == ConstStrings.DataSyncConfig);
            if (config != null)
            {
                var configs = config.Value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                return configs.Select(c => DateTime.Parse(c)).Any(mydt => mydt.Hour == DateTime.Now.Hour && mydt.Minute == DateTime.Now.Minute);
            }

            return false;
        }
    }
}
