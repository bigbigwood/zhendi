using System.Collections;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
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
    public class DeviceTrafficLogsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();

                var paginationResult = repo.QueryPage(conditions);
                var totalCount = paginationResult.TotalCount;
                var deviceTrafficInfos = paginationResult.Entities;
                System.Web.HttpContext.Current.Response.Headers.Add(ConstStrings.HTTP_HEADER_X_Pagination_TotalCount, totalCount.ToString());

                return Request.CreateResponse(HttpStatusCode.OK, deviceTrafficInfos.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();
                var deviceTrafficInfo = repo.GetByKey(id);

                if (deviceTrafficInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, deviceTrafficInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]DeviceTrafficLog deviceTrafficInfo)
        {
            return ActionWarpper.Process(deviceTrafficInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();
                repo.Insert(deviceTrafficInfo);

                return Request.CreateResponse(HttpStatusCode.OK, deviceTrafficInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]DeviceTrafficLog deviceTrafficInfo)
        {
            return ActionWarpper.Process(deviceTrafficInfo, new Func<HttpResponseMessage>(() =>
            {
                deviceTrafficInfo.TrafficID = id;
                var repo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();
                repo.Update(deviceTrafficInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
