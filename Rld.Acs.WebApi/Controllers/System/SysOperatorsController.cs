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
    public class SysOperatorsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QSYSOPT, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperatorRepository>();
                var operatorInfos = repo.Query(conditions);

                operatorInfos.ForEach(d => d.MaskPassword());
                return Request.CreateResponse(HttpStatusCode.OK, operatorInfos.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GSYSOPT, () =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperatorRepository>();
                var sysOperatorInfo = repo.GetByKey(id);

                if (sysOperatorInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                sysOperatorInfo.MaskPassword();
                return Request.CreateResponse(HttpStatusCode.OK, sysOperatorInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]SysOperator sysOperatorInfo)
        {
            return ActionWarpper.Process(sysOperatorInfo, OperationCodes.ASYSOPT, () =>
            {
                var sysOperatorRepository = RepositoryManager.GetRepository<ISysOperatorRepository>();
                var sysOperatorRoleRepo = RepositoryManager.GetRepository<ISysOperatorRoleRepository>();

                string salt = Guid.NewGuid().ToString();
                var hashPassword = SysOperatorExtension.ExcryptPassword(sysOperatorInfo.Password, salt);
                sysOperatorInfo.Password = hashPassword;
                sysOperatorInfo.Salt = salt;
                sysOperatorRepository.Insert(sysOperatorInfo);

                sysOperatorInfo.SysOperatorRoles.ForEach(a => a.OperatorID = sysOperatorInfo.OperatorID);
                sysOperatorInfo.SysOperatorRoles.ForEach(a => sysOperatorRoleRepo.Insert(a));

                return Request.CreateResponse(HttpStatusCode.OK, sysOperatorInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]SysOperator sysOperatorInfo)
        {
            return ActionWarpper.Process(sysOperatorInfo, OperationCodes.MSYSOPT, () =>
            {
                sysOperatorInfo.OperatorID = id;
                var sysOperatorRepository = RepositoryManager.GetRepository<ISysOperatorRepository>();
                var sysOperatorRoleRepo = RepositoryManager.GetRepository<ISysOperatorRoleRepository>();

                var originalsysOperatorInfo = sysOperatorRepository.GetByKey(id);
                if (originalsysOperatorInfo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("operator Id={0} does not exist.", id));

                #region SysOperatorRole
                var addedRoles = new List<SysOperatorRole>();
                var deletedRoleIds = new List<int>();
                if (sysOperatorInfo.SysOperatorRoles != null && sysOperatorInfo.SysOperatorRoles.Any())
                {
                    var originalPermissionIDs = originalsysOperatorInfo.SysOperatorRoles.Select(d => d.SysOperatorRoleID);
                    var currentPermissionIDs = sysOperatorInfo.SysOperatorRoles.Select(d => d.SysOperatorRoleID);
                    deletedRoleIds = originalPermissionIDs.Except(currentPermissionIDs).ToList();

                    addedRoles = sysOperatorInfo.SysOperatorRoles.FindAll(d => d.SysOperatorRoleID == 0);

                    deletedRoleIds.ForEach(d => sysOperatorRoleRepo.Delete(d));
                    addedRoles.ForEach(d => sysOperatorRoleRepo.Insert(d));
                    sysOperatorInfo.SysOperatorRoles.FindAll(d => d.SysOperatorRoleID != 0).ForEach(d => sysOperatorRoleRepo.Update(d));
                }
                else
                {
                    deletedRoleIds = originalsysOperatorInfo.SysOperatorRoles.Select(d => d.SysOperatorRoleID).ToList();
                    deletedRoleIds.ForEach(d => sysOperatorRoleRepo.Delete(d));
                }

                #endregion

                string salt = originalsysOperatorInfo.Salt;
                var hashPassword = SysOperatorExtension.ExcryptPassword(sysOperatorInfo.Password, salt);
                sysOperatorInfo.Password = hashPassword;
                sysOperatorInfo.Salt = salt;
                sysOperatorRepository.Update(sysOperatorInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DSYSOPT, () =>
            {
                var sysOperatorRepository = RepositoryManager.GetRepository<ISysOperatorRepository>();
                var sysOperatorRoleRepo = RepositoryManager.GetRepository<ISysOperatorRoleRepository>();
                var sysOperatorInfo = sysOperatorRepository.GetByKey(id);
                if (sysOperatorInfo != null)
                {
                    sysOperatorInfo.SysOperatorRoles.ForEach(x => sysOperatorRoleRepo.Delete(x.SysOperatorRoleID));
                    sysOperatorRepository.Delete(id);
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
