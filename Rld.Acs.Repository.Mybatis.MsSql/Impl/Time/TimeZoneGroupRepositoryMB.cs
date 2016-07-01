using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class TimeZoneGroupRepositoryMB : MyBatisRepository<TimeZoneGroup, int>, ITimeZoneGroupRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "TimeZoneGroup"; }
        }
        #endregion
    }
}