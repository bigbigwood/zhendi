using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class FloorDoorRepositoryMB : MyBatisRepository<FloorDoor, int>, IFloorDoorRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "FloorDoor"; }
        }
        #endregion
    }
}