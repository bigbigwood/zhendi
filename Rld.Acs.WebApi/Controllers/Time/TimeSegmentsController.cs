using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
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
            return ActionWarpper.Process(conditions, OperationCodes.QTMSGM, () =>
            {
                var repo = RepositoryManager.GetRepository<ITimeSegmentRepository>();
                var timeSegments = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, timeSegments.ToList());

            }, this);
        }

        // GET api/customers/5
        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GTMSGM, () =>
            {
                var repo = RepositoryManager.GetRepository<ITimeSegmentRepository>();
                var timeSegment = repo.GetByKey(id);

                if (timeSegment == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, timeSegment);

            }, this);
        }

        [Authorize]
        // POST api/customers
        public HttpResponseMessage Post([FromBody]TimeSegment timeSegmentDto)
        {
            return ActionWarpper.Process(timeSegmentDto, OperationCodes.ATMSGM, () =>
            {
                var repo = RepositoryManager.GetRepository<ITimeSegmentRepository>();
                if (repo.Query(new Hashtable() { { "TimeSegmentCode", timeSegmentDto.TimeSegmentCode } }).Any())
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        // code will automatically generated
                        Content = new StringContent(string.Format("系统中已经存在编号为{0}的时间段", timeSegmentDto.TimeSegmentCode)),
                        ReasonPhrase = ConstStrings.BusinessLogicError,
                    };
                }

                var timeSegment = repo.Insert(timeSegmentDto);
                return Request.CreateResponse(HttpStatusCode.OK, timeSegment);

            }, this);
        }

        [Authorize]
        // PUT api/customers/5
        public HttpResponseMessage Put(int id, [FromBody]TimeSegment timeSegmentDto)
        {
            return ActionWarpper.Process(timeSegmentDto, OperationCodes.MTMSGM, () =>
            {
                timeSegmentDto.TimeSegmentID = id;
                var repo = RepositoryManager.GetRepository<ITimeSegmentRepository>();
                if (repo.Query(new Hashtable() { { "TimeSegmentCode", timeSegmentDto.TimeSegmentCode } }).Any(x => x.TimeSegmentID != id))
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(string.Format("系统中已经存在编号为{0}的时间段", timeSegmentDto.TimeSegmentCode)),
                        ReasonPhrase = ConstStrings.BusinessLogicError,
                    };
                }

                repo.Update(timeSegmentDto);
                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        // DELETE api/customer/5
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DTMSGM, () =>
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

            }, this);
        }
    }
}
