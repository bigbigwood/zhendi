using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysConfigRepositoryMB : MyBatisRepository<SysConfig, int>, ISysConfigRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "SysConfig"; }
        }
        #endregion
    }
}