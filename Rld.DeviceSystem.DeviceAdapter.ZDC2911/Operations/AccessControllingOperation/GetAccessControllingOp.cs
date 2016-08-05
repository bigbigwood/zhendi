using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetDeviceServiceOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.AccessControllingOperation
{
    public class GetAccessControllingOp
    {
        public GetDeviceServiceResponse Process(GetDeviceServiceRequest request)
        {
            var deviceDao = new DeviceInfoDao(DeviceManager.GetInstance().GetDeviceProxy(1));
            var deviceData = deviceDao.GetDeviceData();
            var serviceData = DeviceInfoMapper.ToModel(deviceData);

            return new GetDeviceServiceResponse() { ResultType = ResultType.OK, Service = serviceData };
        }
    }
}