using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DepartmentDeviceRepositoryMB : MyBatisRepository<DepartmentDevice, int>, IDepartmentDeviceRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DepartmentDevice"; }
        }
        #endregion
    }
}