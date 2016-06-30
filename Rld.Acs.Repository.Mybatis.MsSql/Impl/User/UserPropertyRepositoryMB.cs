using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class UserPropertyRepositoryMB : MyBatisRepository<UserProperty, int>, IUserPropertyRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "UserProperty"; }
        }
        #endregion

        public void SaveOrUpdate(UserProperty entity)
        {
            if (entity.UserPropertyID == 0)
                Insert(entity);
            else
                Update(entity);
        }
    }
}