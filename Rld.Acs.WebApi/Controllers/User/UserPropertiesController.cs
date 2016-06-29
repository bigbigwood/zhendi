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
    public class UserPropertiesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = new UserProperty();
            var allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();
            //conditions.Status = allUrlKeyValues.SingleOrDefault(x => x.Key == "Status").Value;
            //conditions.CreateUserID = allUrlKeyValues.SingleOrDefault(x => x.Key == "CreateUserID").Value;
            //conditions.UpdateUserID = allUrlKeyValues.SingleOrDefault(x => x.Key == "UpdateUserID").Value;

            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                var userProperties = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, userProperties.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                var userProperty = repo.GetByKey(id);

                if (userProperty == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, userProperty);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]UserProperty userProperty)
        {
            return ActionWarpper.Process(userProperty, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                repo.Insert(userProperty);

                return Request.CreateResponse(HttpStatusCode.OK, userProperty);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]UserProperty userProperty)
        {
            return ActionWarpper.Process(userProperty, new Func<HttpResponseMessage>(() =>
            {
                userProperty.UserPropertyID = id;
                var repo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                repo.Update(userProperty);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
