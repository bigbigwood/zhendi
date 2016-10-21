using System;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class GetAllTimeSegmentsOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GetAllTimeSegmentsResponse Process(GetAllTimeSegmentsRequest request)
        {
            try
            {
                var deviceDao = new TimeSegmentInfoDao();
                var data = deviceDao.GetTimeSegmentData();
                var services = TimeSegmentMapper.BuildServices(data);

                return new GetAllTimeSegmentsResponse() { Token = request.Token, ResultType = ResultType.OK, Services = services };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GetAllTimeSegmentsResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}