﻿using System.Collections;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
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
    public class DeviceRolesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QDVRL, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceRoleRepository>();
                var deviceRoleInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, deviceRoleInfos.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GDVRL, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceRoleRepository>();
                var deviceRoleInfo = repo.GetByKey(id);

                if (deviceRoleInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, deviceRoleInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]DeviceRole deviceRoleInfo)
        {
            return ActionWarpper.Process(deviceRoleInfo, OperationCodes.ADVRL, () =>
            {
                var deviceRoleRepo = RepositoryManager.GetRepository<IDeviceRoleRepository>();
                var deviceRolePermissionRepo = RepositoryManager.GetRepository<IDeviceRolePermissionRepository>();

                if (deviceRoleInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "deviceRoleInfo is null");
                }

                deviceRoleRepo.Insert(deviceRoleInfo);

                deviceRoleInfo.DeviceRolePermissions.ForEach(a => a.DeviceRoleID = deviceRoleInfo.DeviceRoleID);
                deviceRoleInfo.DeviceRolePermissions.ForEach(a => deviceRolePermissionRepo.Insert(a));

                return Request.CreateResponse(HttpStatusCode.OK, deviceRoleInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]DeviceRole deviceRoleInfo)
        {
            return ActionWarpper.Process(deviceRoleInfo, OperationCodes.MDVRL, () =>
            {
                deviceRoleInfo.DeviceRoleID = id;

                var deviceRoleRepo = RepositoryManager.GetRepository<IDeviceRoleRepository>();
                var deviceRolePermissionRepo = RepositoryManager.GetRepository<IDeviceRolePermissionRepository>();
                var userDeviceRoleRepo = RepositoryManager.GetRepository<IUserDeviceRoleRepository>();

                var originalDeviceRoleInfo = deviceRoleRepo.GetByKey(id);
                if (originalDeviceRoleInfo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Device Role Id={0} does not exist.", id));

                var addedPermissions = new List<DeviceRolePermission>();
                var deletedPermissionIds = new List<int>();
                if (deviceRoleInfo.DeviceRolePermissions != null && deviceRoleInfo.DeviceRolePermissions.Any())
                {
                    var originalPermissionIDs = originalDeviceRoleInfo.DeviceRolePermissions.Select(d => d.DeviceRolePermissionID);
                    var currentPermissionIDs = deviceRoleInfo.DeviceRolePermissions.Select(d => d.DeviceRolePermissionID);
                    deletedPermissionIds = originalPermissionIDs.Except(currentPermissionIDs).ToList();

                    addedPermissions = deviceRoleInfo.DeviceRolePermissions.FindAll(d => d.DeviceRolePermissionID == 0);
                }
                else
                {
                    deletedPermissionIds = originalDeviceRoleInfo.DeviceRolePermissions.Select(d => d.DeviceRolePermissionID).ToList();
                }

                if (userDeviceRoleRepo.Query(new Hashtable() { { "DeviceRoleID", id } }).Any())
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("设备角色已经绑定人员使用，不能修改。"),
                        ReasonPhrase = ConstStrings.BusinessLogicError,
                    };
                }

                deletedPermissionIds.ForEach(d => deviceRolePermissionRepo.Delete(d));
                addedPermissions.ForEach(d => deviceRolePermissionRepo.Insert(d));
                deviceRoleInfo.DeviceRolePermissions.FindAll(d => d.DeviceRolePermissionID != 0).ForEach(d => deviceRolePermissionRepo.Update(d));

                deviceRoleRepo.Update(deviceRoleInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }


        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DDVRL, () =>
            {
                var deviceRoleRepo = RepositoryManager.GetRepository<IDeviceRoleRepository>();
                var deviceRolePermissionRepo = RepositoryManager.GetRepository<IDeviceRolePermissionRepository>();

                var deviceRoleInfo = deviceRoleRepo.GetByKey(id);
                if (deviceRoleInfo != null)
                {
                    deviceRoleInfo.DeviceRolePermissions.ForEach(a => deviceRolePermissionRepo.Delete(a.DeviceRolePermissionID));
                    deviceRoleRepo.Delete(id);
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
