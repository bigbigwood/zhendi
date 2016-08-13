using Rld.Acs.Unility;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class BatchUpdateTimeZonesOp
    {
        public BatchUpdateTimeZonesResponse Process(BatchUpdateTimeZonesRequest request)
        {
            if (request.Services == null)
            {

            }

            var dao = new TimeZoneInfoDao();

            var data = dao.GetTimeZoneData();

            request.Services.ForEach(s => TimeZoneServiceMapper.UpdateData(ref data, s));

            bool result = dao.UpdateTimeZoneData(data);

            return new BatchUpdateTimeZonesResponse() { ResultType = ResultType.OK };
        }
    }
}