using System;
using System.Collections;
using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Framework.Pagination;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceOperationLogRepositoryMB : MyBatisRepository<DeviceOperationLog, int>, IDeviceOperationLogRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceOperationLog"; }
        }
        #endregion

        public PaginationResult<DeviceOperationLog> QueryPage(Hashtable conditions)
        {
            Int32 totalCount = _sqlMapper.QueryForObject<Int32>("DeviceOperationLog.QueryCount", conditions);
            var entities = Query(conditions);
            return new PaginationResult<DeviceOperationLog>()
            {
                TotalCount = totalCount,
                Entities = entities,
            };
        }
    }
}