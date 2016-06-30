using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class UserAuthenticationRepositoryMB : MyBatisRepository<UserAuthentication, int>, IUserAuthenticationRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "UserAuthentication"; }
        }
        #endregion

        public void SaveOrUpdate(UserAuthentication entity)
        {
            if (entity.UserAuthenticationID == 0)
                Insert(entity);
            else
                Update(entity);
        }
    }
}