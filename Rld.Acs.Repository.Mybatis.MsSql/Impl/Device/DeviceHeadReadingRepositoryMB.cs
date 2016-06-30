using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceHeadReadingRepositoryMB : MyBatisRepository<DeviceHeadReading, int>, IDeviceHeadReadingRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceHeadReading"; }
        }
        #endregion
    }
}