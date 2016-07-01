using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class TimeGroupSegmentRepositoryMB : MyBatisRepository<TimeGroupSegment, int>, ITimeGroupSegmentRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "TimeGroupSegment"; }
        }
        #endregion
    }
}