using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceGroupRepositoryMB : MyBatisRepository<DeviceGroup, int>, IDeviceGroupRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceGroup"; }
        }
        #endregion
    }
}