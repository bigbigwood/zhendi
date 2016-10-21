using System;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.DeviceOperation
{
    public class UpdateDeviceInfoOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UpdateDeviceInfoResponse Process(UpdateDeviceInfoRequest request)
        {
            try
            {
                var dao = new DeviceInfoDao();

                var data = dao.GetDeviceData();

                DeviceInfoMapper.UpdateDeviceData(ref data, request.DeviceInfo);

                dao.UpdateDevice(data);

                return new UpdateDeviceInfoResponse() { Token = request.Token, ResultType = ResultType.OK };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new UpdateDeviceInfoResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}