using System;
using Rld.Acs.Model;
using Rld.Acs.Repository.Framework.Pagination;

namespace Rld.Acs.Repository.Interfaces
{
    public interface IDeviceOperationLogRepository : IPaginationRepository<DeviceOperationLog, Int32>
    {
    }
}