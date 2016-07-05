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
    public class SysModuleElementsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysModuleElementRepository>();
                var sysModuleElementInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, sysModuleElementInfos.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysModuleElementRepository>();
                var sysModuleElementInfo = repo.GetByKey(id);

                if (sysModuleElementInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysModuleElementInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]SysModuleElement sysModuleElementInfo)
        {
            return ActionWarpper.Process(sysModuleElementInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysModuleElementRepository>();
                repo.Insert(sysModuleElementInfo);

                return Request.CreateResponse(HttpStatusCode.OK, sysModuleElementInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]SysModuleElement sysModuleElementInfo)
        {
            return ActionWarpper.Process(sysModuleElementInfo, new Func<HttpResponseMessage>(() =>
            {
                sysModuleElementInfo.ElementID = id;
                var repo = RepositoryManager.GetRepository<ISysModuleElementRepository>();
                repo.Update(sysModuleElementInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysModuleElementRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
