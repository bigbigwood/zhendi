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
    public class SysOperatorsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = new SysOperator();
            var allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();
            //conditions.Status = allUrlKeyValues.SingleOrDefault(x => x.Key == "Status").Value;
            //conditions.CreateUserID = allUrlKeyValues.SingleOrDefault(x => x.Key == "CreateUserID").Value;
            //conditions.UpdateUserID = allUrlKeyValues.SingleOrDefault(x => x.Key == "UpdateUserID").Value;

            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperatorRepository>();
                var users = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, users.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperatorRepository>();
                var sysOperatorInfo = repo.GetByKey(id);

                if (sysOperatorInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysOperatorInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]SysOperator sysOperatorInfo)
        {
            return ActionWarpper.Process(sysOperatorInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperatorRepository>();
                repo.Insert(sysOperatorInfo);

                return Request.CreateResponse(HttpStatusCode.OK, sysOperatorInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]SysOperator sysOperatorInfo)
        {
            return ActionWarpper.Process(sysOperatorInfo, new Func<HttpResponseMessage>(() =>
            {
                sysOperatorInfo.OperatorID = id;
                var repo = RepositoryManager.GetRepository<ISysOperatorRepository>();
                repo.Update(sysOperatorInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysOperatorRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
