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
            return ActionWarpper.Process(conditions, OperationCodes.QSYSRL, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysRoleRepository>();
                var sysRoleInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, sysRoleInfos.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GSYSRL, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysRoleRepository>();
                var sysRoleInfo = repo.GetByKey(id);

                if (sysRoleInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysRoleInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]SysRole sysRoleInfo)
        {
            return ActionWarpper.Process(sysRoleInfo, OperationCodes.ASYSRL, () =>
            {
                var sysRoleRepo = RepositoryManager.GetRepository<ISysRoleRepository>();
                var sysRolePermissionRepo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();

                if (sysRoleInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "sysRoleInfo is null");
                }

                sysRoleRepo.Insert(sysRoleInfo);
                sysRoleInfo.SysRolePermissions.ForEach(x => sysRolePermissionRepo.Insert(x));

                return Request.CreateResponse(HttpStatusCode.OK, sysRoleInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]SysRole sysRoleInfo)
        {
            return ActionWarpper.Process(sysRoleInfo, OperationCodes.MSYSRL, () =>
            {
                sysRoleInfo.RoleID = id;

                var sysRoleRepo = RepositoryManager.GetRepository<ISysRoleRepository>();
                var sysRolePermissionRepo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();

                var originalsysRoleInfo = sysRoleRepo.GetByKey(id);
                if (originalsysRoleInfo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("role Id={0} does not exist.", id));

                #region role permissions
                var addedPermissions = new List<SysRolePermission>();
                var deletedPermissionIds = new List<int>();
                if (sysRoleInfo.SysRolePermissions != null && sysRoleInfo.SysRolePermissions.Any())
                {
                    var originalPermissionIDs = originalsysRoleInfo.SysRolePermissions.Select(d => d.SysRolePermissionID);
                    var currentPermissionIDs = sysRoleInfo.SysRolePermissions.Select(d => d.SysRolePermissionID);
                    deletedPermissionIds = originalPermissionIDs.Except(currentPermissionIDs).ToList();

                    addedPermissions = sysRoleInfo.SysRolePermissions.FindAll(d => d.SysRolePermissionID == 0);

                    deletedPermissionIds.ForEach(d => sysRolePermissionRepo.Delete(d));
                    addedPermissions.ForEach(d => sysRolePermissionRepo.Insert(d));
                    sysRoleInfo.SysRolePermissions.FindAll(d => d.SysRolePermissionID != 0).ForEach(d => sysRolePermissionRepo.Update(d));
                }
                else
                {
                    deletedPermissionIds = originalsysRoleInfo.SysRolePermissions.Select(d => d.SysRolePermissionID).ToList();
                    deletedPermissionIds.ForEach(d => sysRolePermissionRepo.Delete(d));
                }

                #endregion

                sysRoleRepo.Update(sysRoleInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DSYSRL, () =>
            {
                var sysRoleRepo = RepositoryManager.GetRepository<ISysRoleRepository>();
                var sysRolePermissionRepo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();

                var sysRoleInfo = sysRoleRepo.GetByKey(id);
                if (sysRoleInfo != null)
                {
                    sysRoleInfo.SysRolePermissions.ForEach(x => sysRolePermissionRepo.Delete(x.SysRolePermissionID));
                    sysRoleRepo.Delete(id);
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
