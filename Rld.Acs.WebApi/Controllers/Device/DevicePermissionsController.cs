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
    public class DevicePermissionsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDevicePermissionRepository>();
                var devicePermissionInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, devicePermissionInfos.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDevicePermissionRepository>();
                var devicePermissionInfo = repo.GetByKey(id);

                if (devicePermissionInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, devicePermissionInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]DevicePermission devicePermissionInfo)
        {
            return ActionWarpper.Process(devicePermissionInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDevicePermissionRepository>();
                repo.Insert(devicePermissionInfo);

                return Request.CreateResponse(HttpStatusCode.OK, devicePermissionInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]DevicePermission devicePermissionInfo)
        {
            return ActionWarpper.Process(devicePermissionInfo, new Func<HttpResponseMessage>(() =>
            {
                devicePermissionInfo.DevicePermissionID = id;
                var repo = RepositoryManager.GetRepository<IDevicePermissionRepository>();
                repo.Update(devicePermissionInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDevicePermissionRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
