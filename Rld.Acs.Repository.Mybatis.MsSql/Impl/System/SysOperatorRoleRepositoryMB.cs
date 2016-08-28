using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysOperatorRoleRepositoryMB : MyBatisRepository<SysOperatorRole, int>, ISysOperatorRoleRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "SysOperatorRole"; }
        }
        #endregion
    }
}