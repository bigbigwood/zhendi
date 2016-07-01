using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class TimeGroupRepositoryMB : MyBatisRepository<TimeGroup, int>, ITimeGroupRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "TimeGroup"; }
        }
        #endregion
    }
}