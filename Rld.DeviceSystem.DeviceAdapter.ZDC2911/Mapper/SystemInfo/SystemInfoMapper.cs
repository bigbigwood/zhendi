using System;
using log4net;
using Rld.DeviceSystem.Contract.Model.Services.DeviceConn;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.SystemInfo
{
    public class SystemInfoMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(global::System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Contract.Model.SystemInfo ToModel(SystemEntity systemEntity)
        {
            var systemInfo = new Contract.Model.SystemInfo();

            try
            {
                systemInfo.DeviceId = int.Parse(systemEntity.DeviceId);
                systemInfo.Mac = systemEntity.Mac;
                systemInfo.SerialNumber = ConvertObject.ToPrettyString(systemEntity.SerialNumber);
                systemInfo.DeviceModel = ConvertObject.ToPrettyString(systemEntity.Model);
                systemInfo.Password = ConvertObject.ToPrettyString(systemEntity.Password);

                systemInfo.Services.Add(TcpConnectionServiceMapper.BuildService(systemEntity));
                systemInfo.Services.Add(UdpConnectionServiceMapper.BuildService(systemEntity));

                return systemInfo;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public static void UpdateSystemInfo(ref SystemEntity systemEntity, Contract.Model.SystemInfo systemInfo)
        {
            if (systemInfo.DeviceId != 0)
                systemEntity.DeviceId = systemInfo.DeviceId.ToString();

            if (!string.IsNullOrWhiteSpace(systemInfo.Mac))
                systemEntity.Mac = systemInfo.Mac;

            if (!string.IsNullOrWhiteSpace(systemInfo.SerialNumber))
                systemEntity.SerialNumber = systemInfo.SerialNumber;

            if (!string.IsNullOrWhiteSpace(systemInfo.DeviceModel))
                systemEntity.Model = systemInfo.DeviceModel;

            if (!string.IsNullOrWhiteSpace(systemInfo.Password))
                systemEntity.Password = systemInfo.Password;

            if (systemInfo.Services != null)
            {
                foreach (var s in systemInfo.Services)
                {
                    if (s is TcpConnectionService)
                        TcpConnectionServiceMapper.UpdateDeviceData(ref systemEntity, s as TcpConnectionService);
                    else if (s is UdpConnectionService)
                        UdpConnectionServiceMapper.UpdateDeviceData(ref systemEntity, s as UdpConnectionService);

                }
            }
        }
    }
}