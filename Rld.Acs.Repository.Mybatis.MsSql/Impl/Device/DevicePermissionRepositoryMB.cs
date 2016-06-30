using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DevicePermissionRepositoryMB : MyBatisRepository<DevicePermission, int>, IDevicePermissionRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DevicePermission"; }
        }
        #endregion
    }
}