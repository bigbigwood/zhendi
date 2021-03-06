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
    public class DeviceRolePermissionsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QDVRLPMS, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceRolePermissionRepository>();
                var deviceRolePermissionInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, deviceRolePermissionInfos.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GDVRLPMS, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceRolePermissionRepository>();
                var deviceRolePermissionInfo = repo.GetByKey(id);

                if (deviceRolePermissionInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, deviceRolePermissionInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]DeviceRolePermission deviceRolePermissionInfo)
        {
            return ActionWarpper.Process(deviceRolePermissionInfo, OperationCodes.ADVRLPMS, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceRolePermissionRepository>();
                repo.Insert(deviceRolePermissionInfo);

                return Request.CreateResponse(HttpStatusCode.OK, deviceRolePermissionInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]DeviceRolePermission deviceRolePermissionInfo)
        {
            return ActionWarpper.Process(deviceRolePermissionInfo, OperationCodes.MDVRLPMS, () =>
            {
                deviceRolePermissionInfo.DeviceRolePermissionID = id;
                var repo = RepositoryManager.GetRepository<IDeviceRolePermissionRepository>();
                repo.Update(deviceRolePermissionInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DDVRLPMS, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceRolePermissionRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
