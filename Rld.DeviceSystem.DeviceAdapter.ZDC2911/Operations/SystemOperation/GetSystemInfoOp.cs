using System;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.SystemInfo;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.SystemOperation
{
    public class GetSystemInfoOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GetSystemInfoResponse Process(GetSystemInfoRequest request)
        {
            try
            {
                var dao = new SystemInfoDao();
                var systemData = dao.GetSystemData();
                var serviceData = SystemInfoMapper.ToModel(systemData);

                return new GetSystemInfoResponse() { Token = request.Token, ResultType = ResultType.OK, SystemInfo = serviceData };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GetSystemInfoResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}