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
    public class SysModulesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QSYSMDL, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysModuleRepository>();
                var sysModuleInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, sysModuleInfos.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GSYSMDL, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysModuleRepository>();
                var sysModuleInfo = repo.GetByKey(id);

                if (sysModuleInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysModuleInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]SysModule sysModuleInfo)
        {
            return ActionWarpper.Process(sysModuleInfo, OperationCodes.ASYSMDL, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysModuleRepository>();
                repo.Insert(sysModuleInfo);

                return Request.CreateResponse(HttpStatusCode.OK, sysModuleInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]SysModule sysModuleInfo)
        {
            return ActionWarpper.Process(sysModuleInfo, OperationCodes.MSYSMDL, () =>
            {
                sysModuleInfo.ModuleID = id;
                var repo = RepositoryManager.GetRepository<ISysModuleRepository>();
                repo.Update(sysModuleInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DSYSMDL, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysModuleRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
