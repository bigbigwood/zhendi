using Rld.Acs.Unility;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Time;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Operations.TimeOperation
{
    public class BatchUpdateTimeGroupsOp
    {
        public BatchUpdateTimeGroupsResponse Process(BatchUpdateTimeGroupsRequest request)
        {
            if (request.Services == null)
            {

            }

            var dao = new TimeGroupInfoDao(DeviceManager.GetInstance().GetDeviceProxy(1));

            var data = dao.GetTimeGroupData();

            request.Services.ForEach(s => TimeGroupMapper.UpdateTimeGroupData(ref data, s));

            bool result = dao.UpdateTimeGroupData(data);

            return new BatchUpdateTimeGroupsResponse() { ResultType = ResultType.OK };
        }
    }
}