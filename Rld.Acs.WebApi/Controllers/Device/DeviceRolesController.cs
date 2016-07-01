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
    public class DeviceRolesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceRoleRepository>();
                var deviceRoleInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, deviceRoleInfos.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceRoleRepository>();
                var deviceRoleInfo = repo.GetByKey(id);

                if (deviceRoleInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, deviceRoleInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]DeviceRole deviceRoleInfo)
        {
            return ActionWarpper.Process(deviceRoleInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceRoleRepository>();
                repo.Insert(deviceRoleInfo);

                return Request.CreateResponse(HttpStatusCode.OK, deviceRoleInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]DeviceRole deviceRoleInfo)
        {
            return ActionWarpper.Process(deviceRoleInfo, new Func<HttpResponseMessage>(() =>
            {
                deviceRoleInfo.DeviceRoleID = id;
                var repo = RepositoryManager.GetRepository<IDeviceRoleRepository>();
                repo.Update(deviceRoleInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceRoleRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
