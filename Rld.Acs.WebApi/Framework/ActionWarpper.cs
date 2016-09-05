using System.Collections;
using System.Security.Authentication;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Rld.Acs.Repository.Framework;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.WebApi.SecurityService;

namespace Rld.Acs.WebApi.Framework
{
    public class ActionWarpper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static HttpResponseMessage Process<T>(T t, OperationCodes operationCode, Func<HttpResponseMessage> fun, ApiController controller)
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

                    var identify = controller.User.Identity as BasicAuthenticationIdentity;
                    if (identify != null && !Authencate(identify))
                        throw new AuthenticationException("Authorization has been denied for this request.");

                    result = fun();

                    LogOperation(operationCode, controller);

                    Log.Info("Commit transaction!");
                    transaction.Commit();
                }
            }
            catch (AuthenticationException ex)
            {
                Log.Error("AuthenticationException error", ex);
                result = controller.Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, ex.Message, ex);
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

        private static bool Authencate(BasicAuthenticationIdentity identify)
        {
            var repo = RepositoryManager.GetRepository<ISysOperatorRepository>();
            var operatorInfos = repo.Query(new Hashtable { { "LoginName", identify.Name }, { "Status", (int)GeneralStatus.Enabled } });
            if (!operatorInfos.Any())
            {
                return false;
            }

            var operatorInfo = operatorInfos.First();
            var hashPassword = SysOperatorExtension.ExcryptPassword(identify.Password, operatorInfo.Salt);
            identify.AuthorizationOperatorInfo = operatorInfo;
            return (hashPassword == operatorInfo.Password);
        }

        private static void LogOperation(OperationCodes operationCode, ApiController controller)
        {
            if (controller.Request.Method == HttpMethod.Get)
                return;

            var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();
            var operationLog = new SysOperationLog()
            {
                OperationCode = operationCode.ToString(),
                OperationName = operationCode.GetDescription(),
                Detail = string.Format("{0} {1}", controller.Request.Method, controller.Request.RequestUri),
                CreateDate = DateTime.Now,
            };

            var identify = controller.User.Identity as BasicAuthenticationIdentity;
            if (identify != null)
            {
                operationLog.UserID = identify.AuthorizationOperatorInfo.OperatorID;
                operationLog.UserName = identify.Name;
            }

            repo.Insert(operationLog);
        }
    }
}