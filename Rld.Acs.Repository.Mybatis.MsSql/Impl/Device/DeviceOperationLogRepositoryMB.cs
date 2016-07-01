using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceOperationLogRepositoryMB : MyBatisRepository<DeviceOperationLog, int>, IDeviceOperationLogRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceOperationLog"; }
        }
        #endregion
    }
}