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
    public class DeviceOperationLogsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceOperationLogRepository>();

                var paginationResult = repo.QueryPage(conditions);
                var totalCount = paginationResult.TotalCount;
                var deviceOperationLogInfos = paginationResult.Entities;
                System.Web.HttpContext.Current.Response.Headers.Add(ConstStrings.HTTP_HEADER_X_Pagination_TotalCount, totalCount.ToString());

                return Request.CreateResponse(HttpStatusCode.OK, deviceOperationLogInfos.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceOperationLogRepository>();
                var deviceOperationLogInfo = repo.GetByKey(id);

                if (deviceOperationLogInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, deviceOperationLogInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]DeviceOperationLog deviceOperationLogInfo)
        {
            return ActionWarpper.Process(deviceOperationLogInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceOperationLogRepository>();
                repo.Insert(deviceOperationLogInfo);

                return Request.CreateResponse(HttpStatusCode.OK, deviceOperationLogInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]DeviceOperationLog deviceOperationLogInfo)
        {
            return ActionWarpper.Process(deviceOperationLogInfo, new Func<HttpResponseMessage>(() =>
            {
                deviceOperationLogInfo.LogID = id;
                var repo = RepositoryManager.GetRepository<IDeviceOperationLogRepository>();
                repo.Update(deviceOperationLogInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceOperationLogRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
