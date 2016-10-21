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
    public class BatchUpdateTimeZonesOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BatchUpdateTimeZonesResponse Process(BatchUpdateTimeZonesRequest request)
        {
            try
            {
                if (request.Services == null)
                {

                }

                var dao = new TimeZoneInfoDao();

                var data = dao.GetTimeZoneData();

                request.Services.ForEach(s => TimeZoneServiceMapper.UpdateData(ref data, s));

                bool result = dao.UpdateTimeZoneData(data);

                return new BatchUpdateTimeZonesResponse() { Token = request.Token, ResultType = ResultType.OK };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new BatchUpdateTimeZonesResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}