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
using Rld.Acs.Unility;

namespace Rld.Acs.WebApi.Controllers
{
    public class DeviceGroupsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QDVG, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceGroupRepository>();

                var deviceGroups = repo.Query(conditions);
                return Request.CreateResponse(HttpStatusCode.OK, deviceGroups.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GDVG, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceGroupRepository>();
                var deviceGroupInfo = repo.GetByKey(id);

                if (deviceGroupInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, deviceGroupInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]DeviceGroup deviceGroupInfo)
        {
            return ActionWarpper.Process(deviceGroupInfo, OperationCodes.ADVG, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceGroupRepository>();
                repo.Insert(deviceGroupInfo);

                return Request.CreateResponse(HttpStatusCode.OK, deviceGroupInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]DeviceGroup deviceGroupInfo)
        {
            return ActionWarpper.Process(deviceGroupInfo, OperationCodes.MDVG, () =>
            {
                deviceGroupInfo.DeviceGroupID = id;
                var repo = RepositoryManager.GetRepository<IDeviceGroupRepository>();
                repo.Update(deviceGroupInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DDVG, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceGroupRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
