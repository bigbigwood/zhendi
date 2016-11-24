using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
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
            return ActionWarpper.Process(conditions, OperationCodes.QTMGP, () =>
            {
                var repo = RepositoryManager.GetRepository<ITimeGroupRepository>();
                var timeGroupInfos = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, timeGroupInfos.ToList());

            }, this);
        }

        // GET api/customers/5
        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GTMGP, () =>
            {
                var repo = RepositoryManager.GetRepository<ITimeGroupRepository>();
                var timeGroupInfo = repo.GetByKey(id);
                var s = timeGroupInfo.TimeSegments.ToList();
                if (timeGroupInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, timeGroupInfo);

            }, this);
        }

        [Authorize]
        // POST api/customers
        public HttpResponseMessage Post([FromBody]TimeGroup timeGroupInfo)
        {
            return ActionWarpper.Process(timeGroupInfo, OperationCodes.ATMGP, () =>
            {
                var timeGroupRepo = RepositoryManager.GetRepository<ITimeGroupRepository>();
                var timeGroupSegmentRepo = RepositoryManager.GetRepository<ITimeGroupSegmentRepository>();
                var timeSegmentRepo = RepositoryManager.GetRepository<ITimeSegmentRepository>();

                if (timeGroupInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "TimeGroupInfo is null");
                }

                if (timeGroupInfo.TimeSegments != null && timeGroupInfo.TimeSegments.Any())
                {
                    foreach (var timeSegment in timeGroupInfo.TimeSegments.Where(s => timeSegmentRepo.GetByKey(s.TimeSegmentID) == null))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("TimeSegment Id = {0} does not exist yet.", timeSegment.TimeSegmentID));
                    }
                }

                if (timeGroupRepo.Query(new Hashtable() { { "TimeGroupCode", timeGroupInfo.TimeGroupCode } }).Any())
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        // code will automatically generated
                        Content = new StringContent(string.Format("系统中已经存在编号为{0}的时间组", timeGroupInfo.TimeGroupCode)),
                        ReasonPhrase = ConstStrings.BusinessLogicError,
                    };
                }

                timeGroupRepo.Insert(timeGroupInfo);
                timeGroupInfo.TimeSegments.ForEach(g => timeGroupSegmentRepo.Insert(new TimeGroupSegment { TimeGroupID = timeGroupInfo.TimeGroupID, TimeSegmentID = g.TimeSegmentID }));


                return Request.CreateResponse(HttpStatusCode.OK, timeGroupInfo);

            }, this);
        }

        [Authorize]
        // PUT api/customers/5
        public HttpResponseMessage Put(int id, [FromBody]TimeGroup timeGroupInfo)
        {
            return ActionWarpper.Process(timeGroupInfo, OperationCodes.MTMGP, () =>
            {
                if (timeGroupInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "timeGroupInfo is null");
                }
                timeGroupInfo.TimeGroupID = id;

                var timeGroupRepo = RepositoryManager.GetRepository<ITimeGroupRepository>();
                var timeGroupSegmentRepo = RepositoryManager.GetRepository<ITimeGroupSegmentRepository>();
                var timeSegmentRepo = RepositoryManager.GetRepository<ITimeSegmentRepository>();

                var originaltimeGroupInfo = timeGroupRepo.GetByKey(id);
                if (originaltimeGroupInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("TimeGroup Id={0} does not exist.", id));
                }

                if (timeGroupRepo.Query(new Hashtable() { { "TimeGroupCode", timeGroupInfo.TimeGroupCode } }).Any(x => x.TimeGroupID != id))
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(string.Format("系统中已经存在编号为{0}的时间组", timeGroupInfo.TimeGroupCode)),
                        ReasonPhrase = ConstStrings.BusinessLogicError,
                    };
                }

                IList<TimeSegment> addedTimeSegments = new List<TimeSegment>();
                IList<TimeSegment> deletedTimeSegments = new List<TimeSegment>();

                if (timeGroupInfo.TimeSegments != null && timeGroupInfo.TimeSegments.Any())
                {
                    var originalTimeSegmentIds = originaltimeGroupInfo.TimeSegments.Select(s => s.TimeSegmentID);
                    var targetTimeSegmentds = timeGroupInfo.TimeSegments.Select(s => s.TimeSegmentID);

                    addedTimeSegments =
                        timeGroupInfo.TimeSegments.FindAll(s => originalTimeSegmentIds.Contains(s.TimeSegmentID) == false);

                    deletedTimeSegments =
                        originaltimeGroupInfo.TimeSegments.FindAll(s => targetTimeSegmentds.Contains(s.TimeSegmentID) == false);

                    foreach (var addedtimeSegment in addedTimeSegments.Where(s => timeSegmentRepo.GetByKey(s.TimeSegmentID) == null))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("TimeSegment Id = {0} does not exist yet.", addedtimeSegment.TimeSegmentID));
                    }
                }
                else
                {
                    deletedTimeSegments = originaltimeGroupInfo.TimeSegments;
                }

                foreach (var deletedTimeSegment in deletedTimeSegments)
                {
                    var binding = timeGroupSegmentRepo.Query(new Hashtable { { "TimeGroupID", id }, { "TimeSegmentID", deletedTimeSegment.TimeSegmentID } }).ToList();
                    binding.ForEach(b => timeGroupSegmentRepo.Delete(b.TimeGroupSegmentID));
                }
                foreach (var addedTimeSegment in addedTimeSegments)
                {
                    timeGroupSegmentRepo.Insert(new TimeGroupSegment() { TimeGroupID = id, TimeSegmentID = addedTimeSegment.TimeSegmentID });
                }
                timeGroupRepo.Update(timeGroupInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        // DELETE api/customer/5
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DTMGP, () =>
            {
                var timeGroupRepo = RepositoryManager.GetRepository<ITimeGroupRepository>();
                var timeGroupSegmentRepo = RepositoryManager.GetRepository<ITimeGroupSegmentRepository>();
                var timeZoneGroupRepo = RepositoryManager.GetRepository<ITimeZoneGroupRepository>();

                var timeZoneGroupBindings = timeZoneGroupRepo.Query(new Hashtable { { "TimeGroupID", id } });
                if (timeZoneGroupBindings.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.BadGateway, "TimeGroup has binding to TimeZone, cannot be deleted utill the bindings are clean.");
                }

                var timeGroupInfo = timeGroupRepo.GetByKey(id);
                if (timeGroupInfo != null)
                {
                    var bindings = timeGroupSegmentRepo.Query(new Hashtable { { "TimeGroupID", id } }).ToList();
                    bindings.ForEach(b => timeZoneGroupRepo.Delete(b.TimeGroupSegmentID));
                    timeGroupRepo.Delete(id);
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
