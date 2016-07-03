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
    public class TimeSegmentsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET api/customers
        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeSegmentRepository>();
                var timeSegments = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, timeSegments.ToList());

            }), this);
        }

        // GET api/customers/5
        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeSegmentRepository>();
                var timeSegment = repo.GetByKey(id);

                if (timeSegment == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, timeSegment);

            }), this);
        }

        // POST api/customers
        public HttpResponseMessage Post([FromBody]TimeSegment timeSegmentDto)
        {
            return ActionWarpper.Process(timeSegmentDto, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ITimeSegmentRepository>();
                var timeSegment = repo.Insert(timeSegmentDto);

                return Request.CreateResponse(HttpStatusCode.OK, timeSegment);

            }), this);
        }

        // PUT api/customers/5
        public HttpResponseMessage Put(int id, [FromBody]TimeSegment timeSegmentDto)
        {
            return ActionWarpper.Process(timeSegmentDto, new Func<HttpResponseMessage>(() =>
            {
                timeSegmentDto.TimeSegmentID = id;
                var repo = RepositoryManager.GetRepository<ITimeSegmentRepository>();
                repo.Update(timeSegmentDto);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        // DELETE api/customer/5
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var timeSegmentRepo = RepositoryManager.GetRepository<ITimeSegmentRepository>();
                var timeGroupSegmentRepo = RepositoryManager.GetRepository<ITimeGroupSegmentRepository>();

                var timeGroupSegmentBindings = timeGroupSegmentRepo.Query(new Hashtable { { "TimeSegmentID", id } });
                if (timeGroupSegmentBindings.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.BadGateway, "TimeSegment has binding to TimeGroup, cannot be deleted utill the bindings are clean.");
                }

                timeSegmentRepo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
