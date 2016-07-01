using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class TimeZoneRepositoryMB : MyBatisRepository<TimeZone, int>, ITimeZoneRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "TimeZone"; }
        }
        #endregion
    }
}