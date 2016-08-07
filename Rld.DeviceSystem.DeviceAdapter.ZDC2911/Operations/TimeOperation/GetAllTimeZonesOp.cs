﻿using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetAllTimeZonesOp;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class GetAllTimeZonesOp
    {
        public GetAllTimeZonesResponse Process(GetAllTimeZonesRequest request)
        {
            var deviceDao = new TimeZoneInfoDao(DeviceManager.GetInstance().GetDeviceProxy(1));
            var data = deviceDao.GetTimeZoneData();
            var services = TimeZoneServiceMapper.BuildServices(data);

            return new GetAllTimeZonesResponse() { ResultType = ResultType.OK, Services = services };
        }
    }
}