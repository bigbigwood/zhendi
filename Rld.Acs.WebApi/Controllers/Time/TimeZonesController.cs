using System.ComponentModel;
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

                timeZoneRepo.Insert(timeZoneInfo);
                timeZoneInfo.TimeGroupAssociations.ForEach(t => t.TimeZoneID = timeZoneInfo.TimeZoneID);
                timeZoneInfo.TimeGroupAssociations.ForEach(t => timeZoneGroupRepo.Insert(t));

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

                var addedTimeGroups = new List<TimeZoneGroup>();
                var deletedTimeGroupIds = new List<int>();
                if (timeZoneInfo.TimeGroupAssociations != null && timeZoneInfo.TimeGroupAssociations.Any())
                {
                    var originalTimeGroupAssociationIDs = originalTimeZoneInfo.TimeGroupAssociations.Select(d => d.TimeZoneGroupID);
                    var timeGroupAssociationIDs = timeZoneInfo.TimeGroupAssociations.Select(d => d.TimeZoneGroupID);
                    deletedTimeGroupIds = originalTimeGroupAssociationIDs.Except(timeGroupAssociationIDs).ToList();

                    addedTimeGroups = timeZoneInfo.TimeGroupAssociations.FindAll(d => d.TimeZoneGroupID == 0);
                }
                else
                {
                    deletedTimeGroupIds = originalTimeZoneInfo.TimeGroupAssociations.Select(d => d.TimeZoneGroupID).ToList();
                }

                deletedTimeGroupIds.ForEach(d => timeZoneGroupRepo.Delete(d));
                addedTimeGroups.ForEach(d => timeZoneGroupRepo.Insert(d));
                timeZoneInfo.TimeGroupAssociations.FindAll(d => d.TimeZoneGroupID != 0).ForEach(d => timeZoneGroupRepo.Update(d));
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
                    timeZoneInfo.TimeGroupAssociations.ForEach(t => timeZoneGroupRepo.Delete(t.TimeZoneGroupID));
                    timeZoneRepo.Delete(id);
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
