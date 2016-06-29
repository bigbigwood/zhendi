using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class UserAuthenticationRepositoryMB : MyBatisRepository<UserAuthentication, int>, IUserAuthenticationRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "UserAuthentication.Insert"; }
        }

        protected override string UpdateStatement
        {
            get { return "UserAuthentication.Update"; }
        }

        protected override string DeleteStatement
        {
            get { return "UserAuthentication.Delete"; }
        }

        protected override string GetByKeyStatement
        {
            get { return "UserAuthentication.GetByKey"; }
        }

        protected override string QueryCountStatement
        {
            get { return null; }
        }

        protected override string QueryStatement
        {
            get { return "UserAuthentication.Query"; }
        }
        #endregion
    }
}