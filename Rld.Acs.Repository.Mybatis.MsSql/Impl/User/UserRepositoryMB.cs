using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class UserRepositoryMB : MyBatisRepository<User, int>, IUserRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "User.Insert"; }
        }

        protected override string UpdateStatement
        {
            get { return "User.Update"; }
        }

        protected override string DeleteStatement
        {
            get { return "User.Delete"; }
        }

        protected override string GetByKeyStatement
        {
            get { return "User.GetByKey"; }
        }

        protected override string QueryCountStatement
        {
            get { return null; }
        }

        protected override string QueryStatement
        {
            get { return "User.Query"; }
        }
        #endregion
    }
}