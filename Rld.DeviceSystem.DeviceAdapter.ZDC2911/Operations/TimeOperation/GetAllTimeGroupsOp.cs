using System;
using log4net;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class GetAllTimeGroupsOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GetAllTimeGroupsResponse Process(GetAllTimeGroupsRequest request)
        {
            try
            {
                var deviceDao = new TimeGroupInfoDao();
                var data = deviceDao.GetTimeGroupData();
                var services = TimeGroupMapper.BuildServices(data);

                return new GetAllTimeGroupsResponse() { Token = request.Token, ResultType = ResultType.OK, Services = services };
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new GetAllTimeGroupsResponse() { Token = request.Token, ResultType = ResultType.Error };
            }
        }
    }
}