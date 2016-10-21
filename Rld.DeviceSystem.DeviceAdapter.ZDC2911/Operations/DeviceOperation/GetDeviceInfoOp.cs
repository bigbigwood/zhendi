using System;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation
{
    public class GetDeviceInfoOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GetDeviceInfoResponse Process(GetDeviceInfoRequest request)
        {
            try
            {
                var deviceDao = new DeviceInfoDao();
                var deviceData = deviceDao.GetDeviceData();
                var serviceData = DeviceInfoMapper.ToModel(deviceData);

                return new GetDeviceInfoResponse() { Token = request.Token, ResultType = ResultType.OK, Service = serviceData };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GetDeviceInfoResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}