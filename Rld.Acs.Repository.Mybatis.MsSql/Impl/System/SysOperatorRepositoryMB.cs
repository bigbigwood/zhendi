using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysOperatorRepositoryMB : MyBatisRepository<SysOperator, int>, ISysOperatorRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "SysOperator.Insert"; }
        }

        protected override string UpdateStatement
        {
            get { return "SysOperator.Update"; }
        }

        protected override string DeleteStatement
        {
            get { return "SysOperator.Delete"; }
        }

        protected override string GetByKeyStatement
        {
            get { return "SysOperator.GetByKey"; }
        }

        protected override string QueryCountStatement
        {
            get { return null; }
        }

        protected override string QueryStatement
        {
            get { return "SysOperator.Query"; }
        }
        #endregion
    }
}