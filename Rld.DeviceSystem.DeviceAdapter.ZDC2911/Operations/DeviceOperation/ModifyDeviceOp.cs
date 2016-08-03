using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.ModifyDeviceOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation
{
    public class ModifyDeviceOp
    {
        public ModifyDeviceResponse Process(ModifyDeviceRequest request)
        {
            if (request.DeviceInfo == null)
            {

            }

            var service = new DeviceService(DeviceManager.GetInstance().GetDeviceProxy(1));
            service.ModifyDeviceInfo(request.DeviceInfo);

            return new ModifyDeviceResponse() { ResultType = ResultType.OK };
        }
    }
}