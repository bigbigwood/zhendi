using log4net;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.Device;
using Rld.DeviceSystem.Contract.Model.Services.DeviceConn;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.SystemInfo
{
    public class TcpConnectionServiceMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static TcpConnectionService BuildService(SystemEntity entity)
        {
            var service = new TcpConnectionService();
            service.IpAddress = entity.IP;
            service.Port = int.Parse(entity.Port);
            return service;
        }

        public static void UpdateDeviceData(ref SystemEntity entity, TcpConnectionService service)
        {
            if (!string.IsNullOrWhiteSpace(service.IpAddress))
                entity.IP = service.IpAddress;

            if (service.Port != 0)
                entity.Port = service.Port.ToString();
        }
    }
}