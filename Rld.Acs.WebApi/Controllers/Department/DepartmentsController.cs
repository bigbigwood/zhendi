using System.Collections;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
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
            return ActionWarpper.Process(conditions, OperationCodes.QDEPT, () =>
            {
                var repo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var departments = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, departments.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GDEPT, () =>
            {
                var repo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var department = repo.GetByKey(id);

                if (department == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, department);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]Department departmentInfo)
        {
            return ActionWarpper.Process(departmentInfo, OperationCodes.ADEPT, () =>
            {
                var departmentRepo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var departmentDeviceRepo = RepositoryManager.GetRepository<IDepartmentDeviceRepository>();

                if (departmentInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "departmentInfo is null");
                }

                if (departmentInfo.Parent == null)
                {
                    departmentInfo.Parent = new Department() { DepartmentID = -1, Name = "Virtual Top Department" };
                }

                departmentRepo.Insert(departmentInfo);
                departmentInfo.DeviceAssociations.ForEach(d => d.DepartmentID = departmentInfo.DepartmentID);
                departmentInfo.DeviceAssociations.ForEach(d => departmentDeviceRepo.Insert(d));

                return Request.CreateResponse(HttpStatusCode.OK, departmentInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]Department departmentInfo)
        {
            return ActionWarpper.Process(departmentInfo, OperationCodes.MDEPT, () =>
            {
                if (departmentInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "departmentInfo is null");
                }
                departmentInfo.DepartmentID = id;

                var departmentRepo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var departmentDeviceRepo = RepositoryManager.GetRepository<IDepartmentDeviceRepository>();
                var userRepo = RepositoryManager.GetRepository<IUserRepository>();

                var originalDepartmentInfo = departmentRepo.GetByKey(id);
                if (originalDepartmentInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Department Id={0} does not exist.", id));
                }

                if (originalDepartmentInfo.DeviceRoleID != departmentInfo.DeviceRoleID) // DeviceRole changed
                {
                    var deptUsers = userRepo.Query(new Hashtable() { { "DepartmentID", id } });
                    if (deptUsers.Any())
                    {
                        deptUsers.ForEach(x => UpdateUserDeviceRole(x, originalDepartmentInfo.DeviceRoleID, departmentInfo.DeviceRoleID));
                    }
                }

                var addedDevices = new List<DepartmentDevice>();
                var deletedDevicesIds = new List<int>();
                if (departmentInfo.DeviceAssociations != null && departmentInfo.DeviceAssociations.Any())
                {
                    var originalDeviceAssociationIDs = originalDepartmentInfo.DeviceAssociations.Select(d => d.DepartmentDeviceID);
                    var deviceAssociationIDs = departmentInfo.DeviceAssociations.Select(d => d.DepartmentDeviceID);
                    deletedDevicesIds = originalDeviceAssociationIDs.Except(deviceAssociationIDs).ToList();

                    addedDevices = departmentInfo.DeviceAssociations.FindAll(d => d.DepartmentDeviceID == 0);
                }
                else
                {
                    deletedDevicesIds = originalDepartmentInfo.DeviceAssociations.Select(d => d.DepartmentDeviceID).ToList();
                }

                deletedDevicesIds.ForEach(d => departmentDeviceRepo.Delete(d));
                addedDevices.ForEach(d => departmentDeviceRepo.Insert(d));
                departmentInfo.DeviceAssociations.FindAll(d => d.DepartmentDeviceID != 0).ForEach(d => departmentDeviceRepo.Update(d));
                departmentRepo.Update(departmentInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        private void UpdateUserDeviceRole(User user, Int32 currentRoleId, Int32 newRoleId)
        {
            var userDeviceRoleRepo = RepositoryManager.GetRepository<IUserDeviceRoleRepository>();

            var currentDepartmentRoles = user.UserDeviceRoles.FindAll(x => x.DeviceRoleID == currentRoleId);
            if (currentDepartmentRoles.Any())
            {
                currentDepartmentRoles.ForEach(x =>userDeviceRoleRepo.Delete(x.UserDeviceRoleID));
            }

            userDeviceRoleRepo.Insert(new UserDeviceRole() {UserID = user.UserID, DeviceRoleID = newRoleId});
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DDEPT, () =>
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

            }, this);
        }
    }
}
