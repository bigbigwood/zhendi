using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceControllerParameterRepositoryMB : MyBatisRepository<DeviceControllerParameter, int>, IDeviceControllerParameterRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceControllerParameter"; }
        }
        #endregion
    }
}