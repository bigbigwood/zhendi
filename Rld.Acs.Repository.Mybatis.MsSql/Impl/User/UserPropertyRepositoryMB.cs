using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class UserPropertyRepositoryMB : MyBatisRepository<UserProperty, int>, IUserPropertyRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "UserProperty.Insert"; }
        }

        protected override string UpdateStatement
        {
            get { return "UserProperty.Update"; }
        }

        protected override string DeleteStatement
        {
            get { return "UserProperty.Delete"; }
        }

        protected override string GetByKeyStatement
        {
            get { return "UserProperty.GetByKey"; }
        }

        protected override string QueryCountStatement
        {
            get { return null; }
        }

        protected override string QueryStatement
        {
            get { return "UserProperty.Query"; }
        }
        #endregion
    }
}