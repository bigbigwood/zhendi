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
    public class SysConfigsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QSYSCONF, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysConfigRepository>();
                var sysConfigs = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, sysConfigs.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GSYSCONF, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysConfigRepository>();
                var sysConfigInfo = repo.GetByKey(id);

                if (sysConfigInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysConfigInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]SysConfig sysConfigInfo)
        {
            return ActionWarpper.Process(sysConfigInfo, OperationCodes.ASYSCONF, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysConfigRepository>();
                repo.Insert(sysConfigInfo);

                return Request.CreateResponse(HttpStatusCode.OK, sysConfigInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]SysConfig sysConfigInfo)
        {
            return ActionWarpper.Process(sysConfigInfo, OperationCodes.MSYSCONF, () =>
            {
                sysConfigInfo.ID = id;
                var repo = RepositoryManager.GetRepository<ISysConfigRepository>();
                repo.Update(sysConfigInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DSYSCONF, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysConfigRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
