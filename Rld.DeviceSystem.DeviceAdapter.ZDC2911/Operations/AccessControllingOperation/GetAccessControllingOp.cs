using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetDeviceServiceOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.AccessControllingOperation
{
    public class GetAccessControllingOp
    {
        public GetDeviceServiceResponse Process(GetDeviceServiceRequest request)
        {
            var service = new AccessControllingService(DeviceManager.GetInstance().GetDeviceProxy(1));
            var serviceInfos = service.GetDeviceService();

            return new GetDeviceServiceResponse() { ResultType = ResultType.OK, Service = serviceInfos };
        }
    }
}