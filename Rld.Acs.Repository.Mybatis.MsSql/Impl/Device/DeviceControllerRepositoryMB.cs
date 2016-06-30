using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceControllerRepositoryMB : MyBatisRepository<DeviceController, int>, IDeviceControllerRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceController"; }
        }
        #endregion
    }
}