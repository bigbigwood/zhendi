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
    public class ActionWarpper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static HttpResponseMessage Process<T>(T t, Func<HttpResponseMessage> fun, ApiController controller)
        {
            Log.Info("a");
            var sw = Stopwatch.StartNew();

            try
            {
                using (var conn = RepositoryManager.GetNewConnection())
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        Log.Info("b");
                        var result = fun();
                        Log.Info("c");
                        Log.Info("Commit transaction!");
                        transaction.Commit();
                        Log.Info("d");
                        return result;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Unhandled error", ex);

                        if (transaction != null)
                        {
                            Log.Warn("Rollback transaction!");
                            transaction.Rollback();
                        }

                        return controller.Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "Internal Error", ex);
                    }
                    finally
                    {
                        Log.InfoFormat("Finish processing request, cost {0} milliseconds.", sw.ElapsedMilliseconds);
                        Log.InfoFormat("Http Method: {0}, Request Uri: {1}.", controller.Request.Method, controller.Request.RequestUri);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }

        }
    }
}