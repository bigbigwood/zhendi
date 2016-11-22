using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Handlers;
using System.Web.Services;
using log4net;
using Rld.Acs.WebApi.WebService;
using Rld.Acs.Repository;

namespace Rld.Acs.WebApi.WebService
{
    /// <summary>
    /// LisenceService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class LisenceService : System.Web.Services.WebService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public GenericResult Online(string id, string ip)
        {
            try
            {
                using (var conn = RepositoryManager.GetNewConnection())
                {
                    Log.InfoFormat("Id: {0}, ip: {1} is requesting online", id, ip);
                    bool result = new LisenceManager().Register(id);

                    if (result)
                        return new GenericResult() {ResultType = ResultType.OK};
                    else
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GenericResult() { ResultType = ResultType.UnknownError, Messages = new[] { "UnknownError" } };
            }
        }

        [WebMethod]
        public GenericResult Offline(string id, string ip)
        {
            try
            {
                using (var conn = RepositoryManager.GetNewConnection())
                {
                    Log.InfoFormat("Id: {0}, ip: {1} is requesting offline", id, ip);
                    bool result = new LisenceManager().Unregister(id);

                    if (result)
                        return new GenericResult() { ResultType = ResultType.OK };
                    else
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GenericResult() { ResultType = ResultType.UnknownError, Messages = new[] { "UnknownError" } };
            }
        }
    }
}
