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
    public class BatchUpdateTimeGroupsOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BatchUpdateTimeGroupsResponse Process(BatchUpdateTimeGroupsRequest request)
        {
            try
            {
                if (request.Services == null)
                {

                }

                var dao = new TimeGroupInfoDao();

                var data = dao.GetTimeGroupData();

                request.Services.ForEach(s => TimeGroupMapper.UpdateTimeGroupData(ref data, s));

                bool result = dao.UpdateTimeGroupData(data);

                return new BatchUpdateTimeGroupsResponse() { Token = request.Token, ResultType = ResultType.OK };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new BatchUpdateTimeGroupsResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}