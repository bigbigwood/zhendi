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
    public class DepartmentDevicesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDepartmentDeviceRepository>();
                var departmentDeviceInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, departmentDeviceInfos.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDepartmentDeviceRepository>();
                var departmentDeviceInfo = repo.GetByKey(id);

                if (departmentDeviceInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, departmentDeviceInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]DepartmentDevice departmentDeviceInfo)
        {
            return ActionWarpper.Process(departmentDeviceInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDepartmentDeviceRepository>();
                repo.Insert(departmentDeviceInfo);

                return Request.CreateResponse(HttpStatusCode.OK, departmentDeviceInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]DepartmentDevice departmentDeviceInfo)
        {
            return ActionWarpper.Process(departmentDeviceInfo, new Func<HttpResponseMessage>(() =>
            {
                departmentDeviceInfo.DepartmentDeviceID = id;
                var repo = RepositoryManager.GetRepository<IDepartmentDeviceRepository>();
                repo.Update(departmentDeviceInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDepartmentDeviceRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
