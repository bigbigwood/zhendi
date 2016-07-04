using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysRolePermissionRepositoryMB : MyBatisRepository<SysRolePermission, int>, ISysRolePermissionRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "SysRolePermission"; }
        }
        #endregion
    }
}