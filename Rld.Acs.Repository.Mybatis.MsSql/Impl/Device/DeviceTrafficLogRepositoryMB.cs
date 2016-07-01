using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceTrafficLogRepositoryMB : MyBatisRepository<DeviceTrafficLog, int>, IDeviceTrafficLogRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceTrafficLog"; }
        }
        #endregion
    }
}