using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceStateHistoryRepositoryMB : MyBatisRepository<DeviceStateHistory, int>, IDeviceStateHistoryRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceStateHistory"; }
        }
        #endregion
    }
}