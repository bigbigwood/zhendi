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
    public class UserAuthenticationsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = new UserAuthentication();
            var allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();
            //conditions.Status = allUrlKeyValues.SingleOrDefault(x => x.Key == "Status").Value;
            //conditions.CreateUserID = allUrlKeyValues.SingleOrDefault(x => x.Key == "CreateUserID").Value;
            //conditions.UpdateUserID = allUrlKeyValues.SingleOrDefault(x => x.Key == "UpdateUserID").Value;

            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                var users = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, users.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                var userInfo = repo.GetByKey(id);

                if (userInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, userInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]UserAuthentication userAuthentication)
        {
            return ActionWarpper.Process(userAuthentication, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                repo.Insert(userAuthentication);

                return Request.CreateResponse(HttpStatusCode.OK, userAuthentication);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]UserAuthentication userAuthentication)
        {
            return ActionWarpper.Process(userAuthentication, new Func<HttpResponseMessage>(() =>
            {
                userAuthentication.UserAuthenticationID = id;
                var repo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                repo.Update(userAuthentication);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
