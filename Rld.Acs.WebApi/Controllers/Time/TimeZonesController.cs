using System.ComponentModel;
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
                var timeZoneRepo = RepositoryManager.GetRepository<ITimeZoneRepository>();
                var timeZoneGroupRepo = RepositoryManager.GetRepository<ITimeZoneGroupRepository>();
                var timeGroupRepo = RepositoryManager.GetRepository<ITimeGroupRepository>();

                if (timeZoneInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "timeZoneInfo is null");
                }

                if (timeZoneInfo.TimeGroups != null && timeZoneInfo.TimeGroups.Any())
                {
                    foreach (var timeGroup in timeZoneInfo.TimeGroups.Where(timeGroup => timeGroupRepo.GetByKey(timeGroup.TimeGroupID) == null))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("TimeGroup Id = {0} does not exist yet.", timeGroup.TimeGroupID));
                    }
                }

                timeZoneRepo.Insert(timeZoneInfo);
                timeZoneInfo.TimeGroups.ForEach(g => timeZoneGroupRepo.Insert(new TimeZoneGroup { TimeZoneID = timeZoneInfo.TimeZoneID, TimeGroupID = g.TimeGroupID }));

                return Request.CreateResponse(HttpStatusCode.OK, timeZoneInfo);

            }), this);
        }

        // PUT api/customers/5
        public HttpResponseMessage Put(int id, [FromBody]Rld.Acs.Model.TimeZone timeZoneInfo)
        {
            return ActionWarpper.Process(timeZoneInfo, new Func<HttpResponseMessage>(() =>
            {
                if (timeZoneInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "timeZoneInfo is null");
                }
                timeZoneInfo.TimeZoneID = id;

                var timeZoneRepo = RepositoryManager.GetRepository<ITimeZoneRepository>();
                var timeZoneGroupRepo = RepositoryManager.GetRepository<ITimeZoneGroupRepository>();
                var timeGroupRepo = RepositoryManager.GetRepository<ITimeGroupRepository>();

                var originalTimeZoneInfo = timeZoneRepo.GetByKey(id);
                if (originalTimeZoneInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("TimeZone Id={0} does not exist.", id));
                }

                var addedTimeGroups = new List<TimeGroup>();
                var deletedTimeGroups = new List<TimeGroup>();

                if (timeZoneInfo.TimeGroups != null && timeZoneInfo.TimeGroups.Any())
                {
                    var originalTimeGroupsIds = originalTimeZoneInfo.TimeGroups.Select(g => g.TimeGroupID);
                    var targetTimeGroupsIds = timeZoneInfo.TimeGroups.Select(g => g.TimeGroupID);

                    addedTimeGroups =
                        timeZoneInfo.TimeGroups.FindAll(g => originalTimeGroupsIds.Contains(g.TimeGroupID) == false);

                    deletedTimeGroups =
                        originalTimeZoneInfo.TimeGroups.FindAll(g => targetTimeGroupsIds.Contains(g.TimeGroupID) == false);

                    foreach (var addedtimeGroup in addedTimeGroups.Where(tg => timeGroupRepo.GetByKey(tg.TimeGroupID) == null))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("TimeGroup Id = {0} does not exist yet.", addedtimeGroup.TimeGroupID));
                    }
                }
                else
                {
                    deletedTimeGroups = originalTimeZoneInfo.TimeGroups;
                }

                foreach (var deletedTimeGroup in deletedTimeGroups)
                {
                    var binding = timeZoneGroupRepo.Query(new Hashtable { { "TimeZoneID", id }, { "TimeGroupID", deletedTimeGroup.TimeGroupID } }).ToList();
                    binding.ForEach(b => timeZoneGroupRepo.Delete(b.TimeZoneGroupID));
                }
                foreach (var addedTimeGroup in addedTimeGroups)
                {
                    timeZoneGroupRepo.Insert(new TimeZoneGroup { TimeZoneID = id, TimeGroupID = addedTimeGroup.TimeGroupID });
                }
                timeZoneRepo.Update(timeZoneInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        // DELETE api/customer/5
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var timeZoneRepo = RepositoryManager.GetRepository<ITimeZoneRepository>();
                var timeZoneGroupRepo = RepositoryManager.GetRepository<ITimeZoneGroupRepository>();

                var timeZoneInfo = timeZoneRepo.GetByKey(id);
                if (timeZoneInfo != null)
                {
                    var bindings = timeZoneGroupRepo.Query(new Hashtable { {"TimeZoneID", id} }).ToList();
                    bindings.ForEach(b => timeZoneGroupRepo.Delete(b.TimeZoneGroupID));
                    timeZoneRepo.Delete(id);
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
