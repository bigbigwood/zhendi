using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class TimeSegmentRepositoryMB : MyBatisRepository<TimeSegment, int>, ITimeSegmentRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "TimeSegment"; }
        }
        #endregion
    }
}