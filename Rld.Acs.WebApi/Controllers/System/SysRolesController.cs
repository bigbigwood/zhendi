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
    public class SysRolesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysRoleRepository>();
                var sysRoleInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, sysRoleInfos.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysRoleRepository>();
                var sysRoleInfo = repo.GetByKey(id);

                if (sysRoleInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysRoleInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]SysRole sysRoleInfo)
        {
            return ActionWarpper.Process(sysRoleInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysRoleRepository>();
                repo.Insert(sysRoleInfo);

                return Request.CreateResponse(HttpStatusCode.OK, sysRoleInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]SysRole sysRoleInfo)
        {
            return ActionWarpper.Process(sysRoleInfo, new Func<HttpResponseMessage>(() =>
            {
                sysRoleInfo.RoleID = id;
                var repo = RepositoryManager.GetRepository<ISysRoleRepository>();
                repo.Update(sysRoleInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysRoleRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
