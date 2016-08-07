using log4net;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.Device;
using Rld.DeviceSystem.Contract.Model.Services.DeviceConn;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.SystemInfo
{
    public class UdpConnectionServiceMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static UdpConnectionService BuildService(SystemEntity entity)
        {
            var service = new UdpConnectionService();
            service.IpAddress = entity.ServerURL;
            service.Port = int.Parse(entity.ServerPort);
            return service;
        }

        public static void UpdateDeviceData(ref SystemEntity entity, UdpConnectionService service)
        {
            if (!string.IsNullOrWhiteSpace(service.IpAddress))
                entity.ServerURL = service.IpAddress;

            if (service.Port != 0)
                entity.ServerPort = service.Port.ToString();
        }
    }
}