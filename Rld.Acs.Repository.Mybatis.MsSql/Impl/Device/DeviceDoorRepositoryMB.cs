using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceDoorRepositoryMB : MyBatisRepository<DeviceDoor, int>, IDeviceDoorRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceDoor"; }
        }
        #endregion
    }
}