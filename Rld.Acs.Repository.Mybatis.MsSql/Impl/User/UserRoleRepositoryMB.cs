using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class UserRoleRepositoryMB : MyBatisRepository<UserRole, int>, IUserRoleRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "UserRole"; }
        }
        #endregion
    }
}