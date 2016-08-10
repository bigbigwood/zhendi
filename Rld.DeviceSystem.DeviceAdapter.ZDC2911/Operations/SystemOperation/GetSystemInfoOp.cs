using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.SystemInfo;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.SystemOperation
{
    public class GetSystemInfoOp
    {
        public GetSystemInfoResponse Process(GetSystemInfoRequest request)
        {
            var dao = new SystemInfoDao(DeviceManager.GetInstance().GetDeviceProxy(1));
            var systemData = dao.GetSystemData();
            var serviceData = SystemInfoMapper.ToModel(systemData);

            return new GetSystemInfoResponse() { ResultType = ResultType.OK, SystemInfo = serviceData };
        }
    }
}