using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysModuleRepositoryMB : MyBatisRepository<SysModule, int>, ISysModuleRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "SysModule"; }
        }
        #endregion
    }
}