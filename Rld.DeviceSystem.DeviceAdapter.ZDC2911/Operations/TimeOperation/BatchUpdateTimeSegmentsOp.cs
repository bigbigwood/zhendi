using System;
using log4net;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class BatchUpdateTimeSegmentsOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BatchUpdateTimeSegmentsResponse Process(BatchUpdateTimeSegmentsRequest request)
        {
            try
            {
                if (request.Services == null)
                {

                }

                var dao = new TimeSegmentInfoDao();

                var data = dao.GetTimeSegmentData();

                request.Services.ForEach(s => TimeSegmentMapper.UpdateTimeSegmentData(ref data, s));

                bool result = dao.UpdateTimeSegmentData(data);

                return new BatchUpdateTimeSegmentsResponse() { Token = request.Token, ResultType = ResultType.OK };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new BatchUpdateTimeSegmentsResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}