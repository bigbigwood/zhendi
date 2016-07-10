using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Repository
{
    public class DeviceRoleRepository : BaseRepository<DeviceRole, int>, IDeviceRoleRepository
    {
        public DeviceRoleRepository()
        {
            RelevantUri = "/api/DeviceRoles";
        }

        public override bool Update(DeviceRole deviceRole)
        {
            return Update(deviceRole, deviceRole.DeviceRoleID);
        }
    }
}
