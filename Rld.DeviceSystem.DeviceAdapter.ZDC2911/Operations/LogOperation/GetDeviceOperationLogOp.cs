using System.Linq;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Log;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.LogOperation
{
    public class GetDeviceOperationLogOp
    {
        public GetDeviceOperationLogResponse Process(GetDeviceOperationLogRequest request)
        {
            var dao = new SLogInfoDao();
            var rawData = dao.GetLogData(new QueryLogCondictions()
            {
                Options = GetLogOptions.GetNewLogs,
                BeginTime = request.BeginTime,
                EndTime = request.EndTime,
                CleanNewLogPosition = true,
            });

            var serviceData = rawData.Select(DeviceAdminLogMapper.ToModel).ToList();

            return new GetDeviceOperationLogResponse() { Token = request.Token, ResultType = ResultType.OK, Logs = serviceData };
        }
    }
}