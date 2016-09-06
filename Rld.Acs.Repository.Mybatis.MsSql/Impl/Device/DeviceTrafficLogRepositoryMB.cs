using System;
using System.CodeDom;
using System.Collections;
using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Framework.Pagination;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DeviceTrafficLogRepositoryMB : PaginationRepository<DeviceTrafficLog, int>, IDeviceTrafficLogRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "DeviceTrafficLog"; }
        }
        #endregion
    }
}