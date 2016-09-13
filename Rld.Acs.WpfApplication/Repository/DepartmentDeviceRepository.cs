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
    /// <summary>
    /// TODO, 不知道有没有用到
    /// </summary>
    public class DepartmentDeviceRepository : BaseRepository<DepartmentDevice, int>, IDepartmentDeviceRepository
    {
        public DepartmentDeviceRepository()
        {
            RelevantUri = "/api/DepartmentDevices";
        }

        public override bool Update(DepartmentDevice departmentDevice)
        {
            return Update(departmentDevice, departmentDevice.DepartmentDeviceID);
        }
    }
}
