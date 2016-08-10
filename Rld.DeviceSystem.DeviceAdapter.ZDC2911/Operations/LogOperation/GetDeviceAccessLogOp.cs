using System.Linq;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Log;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.LogOperation
{
    public class GetDeviceAccessLogOp
    {
        public GetDeviceAccessLogResponse Process(GetDeviceAccessLogRequest request)
        {
            var dao = new GLogInfoDao(DeviceManager.GetInstance().GetDeviceProxy(1));
            var rawData = dao.GetLogData(new QueryLogCondictions()
            {
                Options = GetLogOptions.GetAllLogs,
                BeginTime = request.BeginTime,
                EndTime = request.EndTime,
                CleanNewLogPosition = false,
            });

            var serviceData = rawData.Select(DeviceAccessLogMapper.ToModel).ToList();

            return new GetDeviceAccessLogResponse() { ResultType = ResultType.OK, Logs = serviceData};
        }
    }
}