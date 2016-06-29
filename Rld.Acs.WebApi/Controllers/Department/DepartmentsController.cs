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
    public class DepartmentsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var departments = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, departments.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var department = repo.GetByKey(id);

                if (department == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, department);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]Department departmentInfo)
        {
            return ActionWarpper.Process(departmentInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDepartmentRepository>();
                repo.Insert(departmentInfo);

                return Request.CreateResponse(HttpStatusCode.OK, departmentInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]Department departmentInfo)
        {
            return ActionWarpper.Process(departmentInfo, new Func<HttpResponseMessage>(() =>
            {
                departmentInfo.DepartmentID = id;
                var repo = RepositoryManager.GetRepository<IDepartmentRepository>();
                repo.Update(departmentInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDepartmentRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
