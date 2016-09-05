using System.Collections;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
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
            return ActionWarpper.Process(conditions, OperationCodes.QSYSOPLOG, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();

                IEnumerable<SysOperationLog> sysOperationLogInfos;
                if (conditions.ContainsKey(ConstStrings.PageStart) && conditions.ContainsKey(ConstStrings.PageEnd))
                {
                    var paginationResult = repo.QueryPage(conditions);
                    var totalCount = paginationResult.TotalCount;
                    sysOperationLogInfos = paginationResult.Entities;
                    System.Web.HttpContext.Current.Response.Headers.Add(ConstStrings.HTTP_HEADER_X_Pagination_TotalCount, totalCount.ToString());
                }
                else
                {
                    sysOperationLogInfos = repo.Query(conditions);
                }

                return Request.CreateResponse(HttpStatusCode.OK, sysOperationLogInfos.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GSYSOPLOG, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();
                var sysOperationLogInfo = repo.GetByKey(id);

                if (sysOperationLogInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysOperationLogInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]SysOperationLog sysOperationLogInfo)
        {
            return ActionWarpper.Process(sysOperationLogInfo, OperationCodes.ASYSOPLOG, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();
                repo.Insert(sysOperationLogInfo);

                return Request.CreateResponse(HttpStatusCode.OK, sysOperationLogInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]SysOperationLog sysOperationLogInfo)
        {
            return ActionWarpper.Process(sysOperationLogInfo, OperationCodes.MSYSOPLOG, () =>
            {
                sysOperationLogInfo.LogID = id;
                var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();
                repo.Update(sysOperationLogInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DSYSOPLOG, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperationLogRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
