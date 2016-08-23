using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysOperationLogRepositoryMB : PaginationRepository<SysOperationLog, int>, ISysOperationLogRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "SysOperationLog"; }
        }
        #endregion
    }
}