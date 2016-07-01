using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WebApi.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Rld.Acs.WebApi.Controllers
{
    public class TimeGroupsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET api/customers
        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeGroupRepository>();
                var timeGroupInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, timeGroupInfos.ToList());

            }), this);
        }

        // GET api/customers/5
        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeGroupRepository>();
                var timeGroupInfo = repo.GetByKey(id);

                if (timeGroupInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, timeGroupInfo);

            }), this);
        }

        // POST api/customers
        public HttpResponseMessage Post([FromBody]TimeGroup timeGroupInfo)
        {
            return ActionWarpper.Process(timeGroupInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeGroupRepository>();
                repo.Insert(timeGroupInfo);

                return Request.CreateResponse(HttpStatusCode.OK, timeGroupInfo);

            }), this);
        }

        // PUT api/customers/5
        public HttpResponseMessage Put(int id, [FromBody]TimeGroup timeGroupInfo)
        {
            return ActionWarpper.Process(timeGroupInfo, new Func<HttpResponseMessage>(() =>
            {
                timeGroupInfo.TimeGroupID = id;
                var repo = RepositoryManager.GetRepository<ITimeGroupRepository>();
                repo.Update(timeGroupInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        // DELETE api/customer/5
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeGroupRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
