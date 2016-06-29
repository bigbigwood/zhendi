using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class TimeSegmentRepositoryMB : MyBatisRepository<TimeSegment, int>, ITimeSegmentRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "TimeSegment.Insert"; }
        }

        protected override string UpdateStatement
        {
            get { return "TimeSegment.Update"; }
        }

        protected override string DeleteStatement
        {
            get { return "TimeSegment.Delete"; }
        }

        protected override string GetByKeyStatement
        {
            get { return "TimeSegment.GetByKey"; }
        }

        protected override string QueryCountStatement
        {
            get { return null; }
        }

        protected override string QueryStatement
        {
            get { return "TimeSegment.Query"; }
        }
        #endregion
    }
}