using System.Collections;
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
    public class FloorsController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QFL, () =>
            {
                var repo = RepositoryManager.GetRepository<IFloorRepository>();
                var floors = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, floors.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GFL, () =>
            {
                var repo = RepositoryManager.GetRepository<IFloorRepository>();
                var floor = repo.GetByKey(id);

                if (floor == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, floor);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]Floor floorInfo)
        {
            return ActionWarpper.Process(floorInfo, OperationCodes.AFL, () =>
            {
                var floorRepo = RepositoryManager.GetRepository<IFloorRepository>();
                var floorDoorRepo = RepositoryManager.GetRepository<IFloorDoorRepository>();

                if (floorInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "floorInfo is null");
                }

                floorRepo.Insert(floorInfo);

                floorInfo.Doors.ForEach(x => x.FloorID = floorInfo.FloorID);
                floorInfo.Doors.ForEach(x => floorDoorRepo.Insert(x));

                return Request.CreateResponse(HttpStatusCode.OK, floorInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]Floor floorInfo)
        {
            return ActionWarpper.Process(floorInfo, OperationCodes.MFL, () =>
            {
                floorInfo.FloorID = id;

                var floorRepo = RepositoryManager.GetRepository<IFloorRepository>();
                var floorDoorRepo = RepositoryManager.GetRepository<IFloorDoorRepository>();

                var originalFloorInfo = floorRepo.GetByKey(id);
                if (originalFloorInfo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Floor Id={0} does not exist.", id));

                #region Doors
                var addedDoors = new List<FloorDoor>();
                var deletedDoorIds = new List<int>();
                if (floorInfo.Doors != null && floorInfo.Doors.Any())
                {
                    var originalDoorIDs = originalFloorInfo.Doors.Select(d => d.FloorDoorID);
                    var currentDeviceDoorIDs = floorInfo.Doors.Select(d => d.FloorDoorID);
                    deletedDoorIds = originalDoorIDs.Except(currentDeviceDoorIDs).ToList();

                    addedDoors = floorInfo.Doors.FindAll(d => d.FloorDoorID == 0);

                    deletedDoorIds.ForEach(d => floorDoorRepo.Delete(d));
                    addedDoors.ForEach(d => floorDoorRepo.Insert(d));
                    floorInfo.Doors.FindAll(d => d.FloorDoorID != 0).ForEach(d => floorDoorRepo.Update(d)); 
                }
                else
                {
                    deletedDoorIds = originalFloorInfo.Doors.Select(d => d.FloorDoorID).ToList();
                    deletedDoorIds.ForEach(d => floorDoorRepo.Delete(d));
                }

                #endregion

                floorRepo.Update(floorInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DFL, () =>
            {
                var floorRepo = RepositoryManager.GetRepository<IFloorRepository>();
                var floorDoorRepo = RepositoryManager.GetRepository<IFloorDoorRepository>();


                var floorInfo = floorRepo.GetByKey(id);
                if (floorInfo != null)
                {
                    if (floorInfo.Doors.Any())
                        floorInfo.Doors.ForEach(r => floorDoorRepo.Delete(r.FloorDoorID));

                    floorRepo.Delete(id);
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
