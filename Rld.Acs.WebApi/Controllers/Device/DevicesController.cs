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
    public class DevicesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                var devices = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, devices.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IDeviceControllerRepository>();
                var device = repo.GetByKey(id);

                if (device == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, device);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]DeviceController deviceInfo)
        {
            return ActionWarpper.Process(deviceInfo, new Func<HttpResponseMessage>(() =>
            {
                if (deviceInfo.DeviceControllerParameter == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "DeviceControllerParameter property cannot be null.");

                if (deviceInfo.DeviceDoors == null || deviceInfo.DeviceDoors.Count == 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "At least has 1 DeviceDoor instance for Device.");

                if (deviceInfo.DeviceHeadReadings == null || deviceInfo.DeviceHeadReadings.Count == 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "At least has 1 DeviceHeadReadings instance for Device.");

                var deviceControllerParameterRepo = RepositoryManager.GetRepository<IDeviceControllerParameterRepository>();
                var deviceDoorRepo = RepositoryManager.GetRepository<IDeviceDoorRepository>();
                var deviceHeadReadingRepo = RepositoryManager.GetRepository<IDeviceHeadReadingRepository>();
                var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

                deviceControllerParameterRepo.Insert(deviceInfo.DeviceControllerParameter);
                deviceInfo.DeviceDoors.ForEach(d => deviceDoorRepo.Insert(d));
                deviceInfo.DeviceHeadReadings.ForEach(h => deviceHeadReadingRepo.Insert(h));
                deviceRepo.Insert(deviceInfo);

                return Request.CreateResponse(HttpStatusCode.OK, deviceInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]DeviceController deviceInfo)
        {
            return ActionWarpper.Process(deviceInfo, new Func<HttpResponseMessage>(() =>
            {
                deviceInfo.DeviceID = id;

                var deviceControllerParameterRepo = RepositoryManager.GetRepository<IDeviceControllerParameterRepository>();
                var deviceDoorRepo = RepositoryManager.GetRepository<IDeviceDoorRepository>();
                var deviceHeadReadingRepo = RepositoryManager.GetRepository<IDeviceHeadReadingRepository>();
                var deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

                var tryFinddeviceInfoInfo = deviceRepo.GetByKey(id);
                if (tryFinddeviceInfoInfo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Device Id={0} does not exist.", id));

                if (deviceInfo.DeviceControllerParameter != null)
                {
                    deviceControllerParameterRepo.Update(deviceInfo.DeviceControllerParameter);
                }
                if (deviceInfo.DeviceDoors != null && deviceInfo.DeviceDoors.Count > 0)
                {
                    deviceInfo.DeviceDoors.ForEach(r => deviceDoorRepo.Update(r));
                }
                if (deviceInfo.DeviceHeadReadings != null && deviceInfo.DeviceHeadReadings.Count > 0)
                {
                    deviceInfo.DeviceHeadReadings.ForEach(h => deviceHeadReadingRepo.Update(h));
                }
                deviceRepo.Update(deviceInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
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

            }), this);
        }
    }
}
