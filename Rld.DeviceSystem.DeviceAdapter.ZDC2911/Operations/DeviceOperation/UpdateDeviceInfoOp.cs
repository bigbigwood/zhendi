using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetDeviceInfoOp;
using Rld.DeviceSystem.Contract.Message.UpdateDeviceInfoOp;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation
{
    public class UpdateDeviceInfoOp
    {
        public UpdateDeviceInfoResponse Process(UpdateDeviceInfoRequest request)
        {
            var dao = new DeviceInfoDao(DeviceManager.GetInstance().GetDeviceProxy(1));

            var data = dao.GetDeviceData();

            DeviceInfoMapper.UpdateDeviceData(ref data, request.DeviceInfo);

            dao.UpdateDevice(data);

            return new UpdateDeviceInfoResponse() { ResultType = ResultType.OK};
        }
    }
}