using System;
using Rld.Acs.Model;
using Rld.Acs.Repository.Framework.Pagination;

namespace Rld.Acs.Repository.Interfaces
{
    public interface IDeviceTrafficLogRepository : IPaginationRepository<DeviceTrafficLog, Int32>
    {
    }
}