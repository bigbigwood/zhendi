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
        public BatchUpdateTimeSegmentsResponse Process(BatchUpdateTimeSegmentsRequest request)
        {
            if (request.Services == null)
            {

            }

            var dao = new TimeSegmentInfoDao();

            var data = dao.GetTimeSegmentData();

            request.Services.ForEach(s =>  TimeSegmentMapper.UpdateTimeSegmentData(ref data, s));

            bool result = dao.UpdateTimeSegmentData(data);

            return new BatchUpdateTimeSegmentsResponse() { Token = request.Token, ResultType = ResultType.OK };
        }
    }
}