using System;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class GetAllTimeZonesOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GetAllTimeZonesResponse Process(GetAllTimeZonesRequest request)
        {
            try
            {
                var deviceDao = new TimeZoneInfoDao();
                var data = deviceDao.GetTimeZoneData();
                var services = TimeZoneServiceMapper.BuildServices(data);

                return new GetAllTimeZonesResponse() { Token = request.Token, ResultType = ResultType.OK, Services = services };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GetAllTimeZonesResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}