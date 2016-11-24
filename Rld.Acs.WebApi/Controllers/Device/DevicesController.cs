using System.Collections;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
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
    public class DevicesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QDV, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                var devices = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, devices.ToList());

            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GDV, () =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                var device = repo.GetByKey(id);

                if (device == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, device);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]DeviceController deviceInfo)
        {
            return ActionWarpper.Process(deviceInfo, OperationCodes.ADV, () =>
            {
                var deviceControllerParameterRepo = RepositoryManager.GetRepository<IDeviceControllerParameterRepository>();
                var deviceDoorRepo = RepositoryManager.GetRepository<IDeviceDoorRepository>();
                var deviceHeadReadingRepo = RepositoryManager.GetRepository<IDeviceHeadReadingRepository>();
                var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

                if (deviceInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "deviceInfo is null");
                }

                if (deviceInfo.DeviceControllerParameter == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "DeviceControllerParameter is null");
                }

                if (deviceRepo.Query(new Hashtable() { { "Code", deviceInfo.Code } }).Any())
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(Resource.MSG_ExistDeviceHasSameCode.GetHashCode().ToString()),
                        ReasonPhrase = ConstStrings.BusinessLogicError,
                    };
                }

                deviceControllerParameterRepo.Insert(deviceInfo.DeviceControllerParameter);
                deviceRepo.Insert(deviceInfo);

                deviceInfo.DeviceDoors.ForEach(a => a.DeviceID = deviceInfo.DeviceID);
                deviceInfo.DeviceDoors.ForEach(d => deviceDoorRepo.Insert(d));

                deviceInfo.DeviceHeadReadings.ForEach(a => a.DeviceID = deviceInfo.DeviceID);
                deviceInfo.DeviceHeadReadings.ForEach(h => deviceHeadReadingRepo.Insert(h));

                return Request.CreateResponse(HttpStatusCode.OK, deviceInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]DeviceController deviceInfo)
        {
            return ActionWarpper.Process(deviceInfo, OperationCodes.MDV, () =>
            {
                deviceInfo.DeviceID = id;

                var deviceControllerParameterRepo = RepositoryManager.GetRepository<IDeviceControllerParameterRepository>();
                var deviceDoorRepo = RepositoryManager.GetRepository<IDeviceDoorRepository>();
                var deviceHeadReadingRepo = RepositoryManager.GetRepository<IDeviceHeadReadingRepository>();
                var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

                var originalDeviceInfo = deviceRepo.GetByKey(id);
                if (originalDeviceInfo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Device Id={0} does not exist.", id));

                if (deviceRepo.Query(new Hashtable() { { "Code", deviceInfo.Code } }).Any(x => x.DeviceID != id))
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(Resource.MSG_ExistDeviceHasSameCode.GetHashCode().ToString()),
                        ReasonPhrase = ConstStrings.BusinessLogicError,
                    };
                }

                #region Doors
                var addedDeviceDoors = new List<DeviceDoor>();
                var deletedDeviceDoorIds = new List<int>();
                if (deviceInfo.DeviceDoors != null && deviceInfo.DeviceDoors.Any())
                {
                    var originalDeviceDoorIDs = originalDeviceInfo.DeviceDoors.Select(d => d.DeviceDoorID);
                    var currentDeviceDoorIDs = deviceInfo.DeviceDoors.Select(d => d.DeviceDoorID);
                    deletedDeviceDoorIds = originalDeviceDoorIDs.Except(currentDeviceDoorIDs).ToList();

                    addedDeviceDoors = deviceInfo.DeviceDoors.FindAll(d => d.DeviceDoorID == 0);

                    deletedDeviceDoorIds.ForEach(d => deviceDoorRepo.Delete(d));
                    addedDeviceDoors.ForEach(d => deviceDoorRepo.Insert(d));
                    deviceInfo.DeviceDoors.FindAll(d => d.DeviceDoorID != 0).ForEach(d => deviceDoorRepo.Update(d)); 
                }
                else
                {
                    deletedDeviceDoorIds = originalDeviceInfo.DeviceDoors.Select(d => d.DeviceDoorID).ToList();
                    deletedDeviceDoorIds.ForEach(d => deviceDoorRepo.Delete(d));
                }

                #endregion

                #region HeadReading
                var addedDeviceHeadReadings = new List<DeviceHeadReading>();
                var deletedDeviceHeadReadingIds = new List<int>();
                if (deviceInfo.DeviceHeadReadings != null && deviceInfo.DeviceHeadReadings.Any())
                {
                    var originalDeviceHeadReadingIDs = originalDeviceInfo.DeviceHeadReadings.Select(d => d.DeviceHeadReadingID);
                    var currentDeviceHeadReadingIDs = deviceInfo.DeviceHeadReadings.Select(d => d.DeviceHeadReadingID);
                    deletedDeviceHeadReadingIds = originalDeviceHeadReadingIDs.Except(currentDeviceHeadReadingIDs).ToList();

                    addedDeviceHeadReadings = deviceInfo.DeviceHeadReadings.FindAll(d => d.DeviceHeadReadingID == 0);

                    deletedDeviceHeadReadingIds.ForEach(d => deviceHeadReadingRepo.Delete(d));
                    addedDeviceHeadReadings.ForEach(d => deviceHeadReadingRepo.Insert(d));
                    deviceInfo.DeviceHeadReadings.FindAll(d => d.DeviceHeadReadingID != 0).ForEach(d => deviceHeadReadingRepo.Update(d));
                }
                else
                {
                    deletedDeviceHeadReadingIds = originalDeviceInfo.DeviceHeadReadings.Select(d => d.DeviceHeadReadingID).ToList();
                    deletedDeviceHeadReadingIds.ForEach(d => deviceHeadReadingRepo.Delete(d));
                } 
                #endregion

                #region parameters
                if (deviceInfo.DeviceControllerParameter != null)
                    deviceControllerParameterRepo.Update(deviceInfo.DeviceControllerParameter); 
                #endregion
                
                deviceRepo.Update(deviceInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DDV, () =>
            {
                var deviceControllerParameterRepo = RepositoryManager.GetRepository<IDeviceControllerParameterRepository>();
                var deviceDoorRepo = RepositoryManager.GetRepository<IDeviceDoorRepository>();
                var deviceHeadReadingRepo = RepositoryManager.GetRepository<IDeviceHeadReadingRepository>();
                var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

                var deviceInfo = deviceRepo.GetByKey(id);
                if (deviceInfo != null)
                {
                    deviceControllerParameterRepo.Delete(deviceInfo.DeviceControllerParameter.DeviceParameterID);

                    if (deviceInfo.DeviceDoors.Any())
                        deviceInfo.DeviceDoors.ForEach(r => deviceDoorRepo.Delete(r.DeviceDoorID));

                    if (deviceInfo.DeviceHeadReadings.Any())
                        deviceInfo.DeviceHeadReadings.ForEach(h => deviceHeadReadingRepo.Delete(h.DeviceHeadReadingID));

                    deviceRepo.Delete(id);
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }
    }
}
