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
    public class DeviceStateHistorysController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QDVDRS, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceStateHistoryRepository>();
                var deviceStateHistoryInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, deviceStateHistoryInfos.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GDVDRS, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceStateHistoryRepository>();
                var deviceStateHistoryInfo = repo.GetByKey(id);

                if (deviceStateHistoryInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, deviceStateHistoryInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]DeviceStateHistory deviceStateHistoryInfo)
        {
            return ActionWarpper.Process(deviceStateHistoryInfo, OperationCodes.ADVDRS, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceStateHistoryRepository>();
                repo.Insert(deviceStateHistoryInfo);

                return Request.CreateResponse(HttpStatusCode.OK, deviceStateHistoryInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]DeviceStateHistory deviceStateHistoryInfo)
        {
            return ActionWarpper.Process(deviceStateHistoryInfo, OperationCodes.MDVDRS, () =>
            {
                deviceStateHistoryInfo.DeviceStateHistoryID = id;
                var repo = RepositoryManager.GetRepository<IDeviceStateHistoryRepository>();
                repo.Update(deviceStateHistoryInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DDVDRS, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceStateHistoryRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
