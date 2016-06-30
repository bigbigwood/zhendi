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
    public class DevicesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                var devices = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, devices.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                var device = repo.GetByKey(id);

                if (device == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, device);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]DeviceController deviceInfo)
        {
            return ActionWarpper.Process(deviceInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                repo.Insert(deviceInfo);

                return Request.CreateResponse(HttpStatusCode.OK, deviceInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]DeviceController deviceInfo)
        {
            return ActionWarpper.Process(deviceInfo, new Func<HttpResponseMessage>(() =>
            {
                deviceInfo.DeviceID = id;
                var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                repo.Update(deviceInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
