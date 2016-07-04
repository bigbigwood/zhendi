using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysModuleElementRepositoryMB : MyBatisRepository<SysModuleElement, int>, ISysModuleElementRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "SysModuleElement"; }
        }
        #endregion
    }
}