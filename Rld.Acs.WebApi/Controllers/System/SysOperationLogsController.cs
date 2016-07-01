using System.Collections;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WebApi.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Rld.Acs.WebApi.Controllers
{
    public class SysOperationLogsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();
                var sysOperationLogInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, sysOperationLogInfos.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();
                var sysOperationLogInfo = repo.GetByKey(id);

                if (sysOperationLogInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysOperationLogInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]SysOperationLog sysOperationLogInfo)
        {
            return ActionWarpper.Process(sysOperationLogInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();
                repo.Insert(sysOperationLogInfo);

                return Request.CreateResponse(HttpStatusCode.OK, sysOperationLogInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]SysOperationLog sysOperationLogInfo)
        {
            return ActionWarpper.Process(sysOperationLogInfo, new Func<HttpResponseMessage>(() =>
            {
                sysOperationLogInfo.LogID = id;
                var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();
                repo.Update(sysOperationLogInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
