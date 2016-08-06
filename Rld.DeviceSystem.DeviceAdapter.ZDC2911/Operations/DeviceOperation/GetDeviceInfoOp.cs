﻿using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetDeviceInfoOp;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation
{
    public class GetDeviceInfoOp
    {
        public GetDeviceInfoResponse Process(GetDeviceInfoRequest request)
        {
            var deviceDao = new DeviceInfoDao(DeviceManager.GetInstance().GetDeviceProxy(1));
            var deviceData = deviceDao.GetDeviceData();
            var serviceData = DeviceInfoMapper.ToModel(deviceData);

            return new GetDeviceInfoResponse() { ResultType = ResultType.OK, Service = serviceData };
        }
    }
}