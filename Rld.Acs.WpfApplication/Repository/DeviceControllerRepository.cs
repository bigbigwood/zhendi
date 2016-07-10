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
    public class DeviceControllerRepository : BaseRepository<DeviceController, int>, IDeviceControllerRepository
    {
        public DeviceControllerRepository()
        {
            RelevantUri = "/api/Devices";
        }

        public override bool Update(DeviceController deviceController)
        {
            return Update(deviceController, deviceController.DeviceID);
        }
    }
}
