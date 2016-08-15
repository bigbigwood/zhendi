using System;
using System.Diagnostics;
using log4net;
using Rld.Acs.DeviceSystem.Message;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Framework;

namespace Rld.Acs.DeviceSystem.Framework
{
    public class PersistenceOperation
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static TResult Process<T, TResult>(T tParameters, Func<TResult> fun)
            where TResult : ResponseBase, new()
        {
            var sw = Stopwatch.StartNew();
            IPersistanceTransaction transaction = null;
            try
            {
                using (var conn = RepositoryManager.GetNewConnection())
                {
                    transaction = conn.BeginTransaction();

                    var result = fun();

                    Log.Info("Commit transaction!");
                    transaction.Commit();

                    return result;
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

                return new TResult() { ResultType = ResultTypes.UnknownError, Messages = new[] { "Internal error" } };
            }
            finally
            {
                Log.InfoFormat("Finish processing request, cost {0} milliseconds.", sw.ElapsedMilliseconds);
            }
        }
    }
}