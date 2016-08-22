using System;
using System.CodeDom;
using System.Collections;
using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Framework.Pagination;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceTrafficLogRepositoryMB : MyBatisRepository<DeviceTrafficLog, int>, IDeviceTrafficLogRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceTrafficLog"; }
        }
        #endregion

        public PaginationResult<DeviceTrafficLog> QueryPage(Hashtable conditions)
        {
            Int32 totalCount = _sqlMapper.QueryForObject<Int32>("DeviceTrafficLog.QueryCount", conditions);
            var entities = Query(conditions);
            return new PaginationResult<DeviceTrafficLog>()
            {
                TotalCount = totalCount,
                Entities = entities,
            };
        }
    }
}