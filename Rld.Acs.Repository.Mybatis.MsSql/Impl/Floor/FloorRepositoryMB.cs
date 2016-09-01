using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class FloorRepositoryMB : MyBatisRepository<Floor, int>, IFloorRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "Floor"; }
        }
        #endregion
    }
}