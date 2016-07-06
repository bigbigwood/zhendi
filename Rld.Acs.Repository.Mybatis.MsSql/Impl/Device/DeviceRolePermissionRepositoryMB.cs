using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceRolePermissionRepositoryMB : MyBatisRepository<DeviceRolePermission, int>, IDeviceRolePermissionRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceRolePermission"; }
        }
        #endregion
    }
}