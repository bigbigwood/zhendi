using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Web.Services;
using FluentValidation.Resources;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WebApi.Framework;

namespace Rld.Acs.WebApi.SecurityService
{
    /// <summary>
    /// Summary description for SecurityService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SecurityService : System.Web.Services.WebService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public AuthenticateResult Authenticate(string username, string password)
        {
            try
            {
                using (var conn = RepositoryManager.GetNewConnection())
                {
                    var repo = RepositoryManager.GetRepository<ISysOperatorRepository>();
                    var operatorInfos = repo.Query(new Hashtable { { "LoginName", username }, { "Status", (int)GeneralStatus.Enabled } });
                    if (!operatorInfos.Any())
                    {
                        return new AuthenticateResult() { ResultType = ResultType.UserNotFound, Messages = new[] { "User does not exist." } };
                    }

                    var operatorInfo = operatorInfos.First();
                    var hashPassword = SysOperatorExtension.ExcryptPassword(password, operatorInfo.Salt);
                    if (hashPassword == operatorInfo.Password)
                    {
                        operatorInfo.MaskPassword();
                        operatorInfo.Password = password; // return password
                        return new AuthenticateResult() { ResultType = ResultType.OK, Messages = new[] { "OK" }, OperatorInfo = operatorInfo };
                    }
                    else
                    {
                        return new AuthenticateResult()  { ResultType = ResultType.AuthenticationError, Messages = new[] { "invalid user name and password" } };
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new AuthenticateResult() { ResultType = ResultType.UnknownError, Messages = new[] { "UnknownError" } };
            }
        }

        //[WebMethod]
        //public string Authorizate()
        //{
        //    return "Hello World";
        //}

        [WebMethod]
        public GenericResult ChangePassowrd(string username, string oldPassword, string newPassword)
        {
            var sw = Stopwatch.StartNew();

            using (var conn = RepositoryManager.GetNewConnection())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var repo = RepositoryManager.GetRepository<ISysOperatorRepository>();
                    var operatorInfos = repo.Query(new Hashtable { { "LoginName", username }, { "Status", (int)GeneralStatus.Enabled } });
                    if (!operatorInfos.Any())
                    {
                        return new AuthenticateResult() { ResultType = ResultType.UserNotFound, Messages = new[] { "User does not exist." } };
                    }

                    var operatorInfo = operatorInfos.First();
                    var hashPassword = SysOperatorExtension.ExcryptPassword(oldPassword, operatorInfo.Salt);

                    if (hashPassword != operatorInfo.Password)
                    {
                        return new GenericResult() { ResultType = ResultType.AuthenticationError, Messages = new [] { "AuthenticationError" } };
                    }

                    operatorInfo.Salt = Guid.NewGuid().ToString();
                    operatorInfo.Password = SysOperatorExtension.ExcryptPassword(newPassword, operatorInfo.Salt);
                    repo.UpdatePassword(operatorInfo);

                    Log.Info("Commit transaction!");
                    transaction.Commit();

                    return new GenericResult() { ResultType = ResultType.OK, Messages = new[] { "OK" } };
                }
                catch (Exception ex)
                {
                    Log.Error("Unhandled error", ex);

                    if (transaction != null)
                    {
                        Log.Warn("Rollback transaction!");
                        transaction.Rollback();
                    }

                    return new GenericResult() { ResultType = ResultType.UnknownError, Messages = new[] { "UnknownError" } };
                }
                finally
                {
                    Log.InfoFormat("Finish processing request, cost {0} milliseconds.", sw.ElapsedMilliseconds);
                }
            }
        }
    }
}
