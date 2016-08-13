using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class GetAllTimeGroupsOp
    {
        public GetAllTimeGroupsResponse Process(GetAllTimeGroupsRequest request)
        {
            var deviceDao = new TimeGroupInfoDao();
            var data = deviceDao.GetTimeGroupData();
            var services = TimeGroupMapper.BuildServices(data);

            return new GetAllTimeGroupsResponse() { ResultType = ResultType.OK, Services = services };
        }
    }
}