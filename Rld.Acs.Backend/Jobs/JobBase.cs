using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Framework;

namespace Rld.Acs.Backend.Jobs
{
    abstract class JobBase : IJob
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            var sw = Stopwatch.StartNew();
            IPersistanceTransaction transaction = null;
            HttpResponseMessage result = null;

            try
            {
                using (var conn = RepositoryManager.GetNewConnection())
                {
                    transaction = conn.BeginTransaction();

                    ProcessBusiness(context);

                    Log.Info("Commit transaction!");
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Unhandled error", ex);

                if (transaction != null)
                {
                    Log.Warn("Rollback transaction!");
                    transaction.Rollback();
                }
            }

            Log.InfoFormat("Finish processing request, cost {0} milliseconds.", sw.ElapsedMilliseconds);
        }

        protected abstract void ProcessBusiness(IJobExecutionContext context);
    }
}
