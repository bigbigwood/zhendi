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
                var departmentRepo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var departmentDeviceRepo= RepositoryManager.GetRepository<IDepartmentDeviceRepository>();

                if (departmentInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "departmentInfo is null");
                }

                if (departmentInfo.Parent == null)
                {
                    departmentInfo.Parent = new Department() {DepartmentID = -1, Name = "Virtual Top Department"};
                }

                departmentRepo.Insert(departmentInfo);
                departmentInfo.DeviceAssociations.ForEach(d => departmentDeviceRepo.Insert(new DepartmentDevice() {DepartmentID = departmentInfo.DepartmentID, DeviceID = d.DeviceID}) );

                return Request.CreateResponse(HttpStatusCode.OK, departmentInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]Department departmentInfo)
        {
            return ActionWarpper.Process(departmentInfo, new Func<HttpResponseMessage>(() =>
            {
                if (departmentInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "departmentInfo is null");
                }
                departmentInfo.DepartmentID = id;

                var departmentRepo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var departmentDeviceRepo = RepositoryManager.GetRepository<IDepartmentDeviceRepository>();

                var originalDepartmentInfo = departmentRepo.GetByKey(id);
                if (originalDepartmentInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Department Id={0} does not exist.", id));
                }

                var addedDevices = new List<DepartmentDevice>();
                var deletedDevices = new List<DepartmentDevice>();
                if (departmentInfo.DeviceAssociations != null && departmentInfo.DeviceAssociations.Any())
                {
                    deletedDevices = originalDepartmentInfo.DeviceAssociations.FindAll(d => departmentInfo.DeviceAssociations.Contains(d) == false);
                    addedDevices = departmentInfo.DeviceAssociations.FindAll(d => d.DepartmentDeviceID == 0);
                }
                else
                {
                    deletedDevices = originalDepartmentInfo.DeviceAssociations.ToList();
                }

                deletedDevices.ForEach(d => departmentDeviceRepo.Delete(d.DepartmentDeviceID));
                addedDevices.ForEach(d => departmentDeviceRepo.Insert(d));
                departmentRepo.Update(departmentInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var departmentRepo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var departmentDeviceRepo = RepositoryManager.GetRepository<IDepartmentDeviceRepository>();

                var originalDepartmentInfo = departmentRepo.GetByKey(id);
                if (originalDepartmentInfo != null)
                {
                    originalDepartmentInfo.DeviceAssociations.ForEach(d => departmentDeviceRepo.Delete(d.DepartmentDeviceID));
                    departmentRepo.Delete(id);
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
