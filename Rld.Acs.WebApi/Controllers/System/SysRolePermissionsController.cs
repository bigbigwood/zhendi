﻿using System.Collections;
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
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();
                var sysRolePermissionInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, sysRolePermissionInfos.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();
                var sysRolePermissionInfo = repo.GetByKey(id);

                if (sysRolePermissionInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysRolePermissionInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]SysRolePermission sysRolePermissionInfo)
        {
            return ActionWarpper.Process(sysRolePermissionInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();
                repo.Insert(sysRolePermissionInfo);

                return Request.CreateResponse(HttpStatusCode.OK, sysRolePermissionInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]SysRolePermission sysRolePermissionInfo)
        {
            return ActionWarpper.Process(sysRolePermissionInfo, new Func<HttpResponseMessage>(() =>
            {
                sysRolePermissionInfo.SysRolePermissionID = id;
                var repo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();
                repo.Update(sysRolePermissionInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysRolePermissionRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}