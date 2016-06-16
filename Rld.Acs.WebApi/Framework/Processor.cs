using log4net;
using Rld.Acs.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Rld.Acs.WebApi.Framework
{
    public class Processor
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static HttpResponseMessage webHandle<T>(T t, Func<HttpResponseMessage> fun, ApiController controller)
        {
            var sw = Stopwatch.StartNew();

            using (var conn = RepositoryManager.GetNewConnection())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var result = fun();

                    Log.Info("Commit transaction!");
                    transaction.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    Log.Error("Unhandled error", ex);

                    // transaction rollback
                    if (transaction != null)
                    {
                        Log.Warn("Rollback transaction!");
                        transaction.Rollback();
                    }

                    return controller.Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "", ex);
                }
                finally
                {
                    Log.InfoFormat("Finish processing {0}, cost {1} milliseconds", controller.ToString(), sw.ElapsedMilliseconds);

                    // release session
                }
            }
        }
    }
}