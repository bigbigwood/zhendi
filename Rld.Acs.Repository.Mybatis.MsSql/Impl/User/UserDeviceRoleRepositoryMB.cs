using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class UserDeviceRoleRepositoryMB : MyBatisRepository<UserDeviceRole, int>, IUserDeviceRoleRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "UserDeviceRole"; }
        }
        #endregion
    }
}