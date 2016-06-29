using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceRoleRepositoryMB : MyBatisRepository<DeviceRole, int>, IDeviceRoleRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "DeviceRole.Insert"; }
        }

        protected override string UpdateStatement
        {
            get { return "DeviceRole.Update"; }
        }

        protected override string DeleteStatement
        {
            get { return "DeviceRole.Delete"; }
        }

        protected override string GetByKeyStatement
        {
            get { return "DeviceRole.GetByKey"; }
        }

        protected override string QueryCountStatement
        {
            get { return null; }
        }

        protected override string QueryStatement
        {
            get { return "DeviceRole.Query"; }
        }
        #endregion
    }
}