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
    public class TimeZonesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET api/customers
        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeZoneRepository>();
                var timeZoneInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, timeZoneInfos.ToList());

            }), this);
        }

        // GET api/customers/5
        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeZoneRepository>();
                var timeZoneInfo = repo.GetByKey(id);

                if (timeZoneInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, timeZoneInfo);

            }), this);
        }

        // POST api/customers
        public HttpResponseMessage Post([FromBody]Rld.Acs.Model.TimeZone timeZoneInfo)
        {
            return ActionWarpper.Process(timeZoneInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeZoneRepository>();
                repo.Insert(timeZoneInfo);

                return Request.CreateResponse(HttpStatusCode.OK, timeZoneInfo);

            }), this);
        }

        // PUT api/customers/5
        public HttpResponseMessage Put(int id, [FromBody]Rld.Acs.Model.TimeZone timeZoneInfo)
        {
            return ActionWarpper.Process(timeZoneInfo, new Func<HttpResponseMessage>(() =>
            {
                timeZoneInfo.TimeZoneID = id;
                var repo = RepositoryManager.GetRepository<ITimeZoneRepository>();
                repo.Update(timeZoneInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        // DELETE api/customer/5
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeZoneRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
