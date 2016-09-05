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
    public class SysRolePermissionsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QSYSRLPMS, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();
                var sysRolePermissionInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, sysRolePermissionInfos.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GSYSRLPMS, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();
                var sysRolePermissionInfo = repo.GetByKey(id);

                if (sysRolePermissionInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysRolePermissionInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]SysRolePermission sysRolePermissionInfo)
        {
            return ActionWarpper.Process(sysRolePermissionInfo, OperationCodes.ASYSRLPMS, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();
                repo.Insert(sysRolePermissionInfo);

                return Request.CreateResponse(HttpStatusCode.OK, sysRolePermissionInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]SysRolePermission sysRolePermissionInfo)
        {
            return ActionWarpper.Process(sysRolePermissionInfo, OperationCodes.MSYSRLPMS, () =>
            {
                sysRolePermissionInfo.SysRolePermissionID = id;
                var repo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();
                repo.Update(sysRolePermissionInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DSYSRLPMS, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
