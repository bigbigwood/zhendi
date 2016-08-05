using log4net;
using Rld.DeviceSystem.Contract.Model.Services.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device
{
    public class MultiPersionLockServiceMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static MultiPersionLockService BuildService(byte[] deviceData)
        {
            var multiPersionLockService = new MultiPersionLockService();
            multiPersionLockService.Enabled = (deviceData[24] == 1);
            return multiPersionLockService;
        }

        public static void UpdateDeviceData(ref byte[] deviceData, MultiPersionLockService service)
        {
            deviceData[24] = (byte)(service.Enabled ? 1 : 0);
        }
    }
}