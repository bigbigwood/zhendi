using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using log4net;
using Rld.Acs.Repository;

namespace Rld.Acs.WebApi.Framework
{
    public class PersistenceOperationHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(Action fun)
        {
            var sw = Stopwatch.StartNew();

            using (var conn = RepositoryManager.GetNewConnection())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    fun();

                    Log.Info("Commit transaction!");
                    transaction.Commit();
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
                finally
                {
                    Log.InfoFormat("Finish processing request, cost {0} milliseconds.", sw.ElapsedMilliseconds);
                }
            }
        }
    }
}