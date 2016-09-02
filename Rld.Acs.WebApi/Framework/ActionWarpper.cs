using log4net;
using Rld.Acs.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Rld.Acs.Repository.Framework;

namespace Rld.Acs.WebApi.Framework
{
    public class ActionWarpper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static HttpResponseMessage Process<T>(T t, Func<HttpResponseMessage> fun, ApiController controller)
        {
            Log.InfoFormat("Http Method: {0}, Request Uri: {1}.", controller.Request.Method, controller.Request.RequestUri);
            var sw = Stopwatch.StartNew();
            IPersistanceTransaction transaction = null;
            HttpResponseMessage result = null;

            try
            {
                using (var conn = RepositoryManager.GetNewConnection())
                {
                    transaction = conn.BeginTransaction();

                    result = fun();

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

                result = controller.Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "Internal Error", ex);
            }

            Log.InfoFormat("Response: {0}", result);
            Log.InfoFormat("Finish processing request, cost {0} milliseconds.", sw.ElapsedMilliseconds);
            return result;
        }
    }
}