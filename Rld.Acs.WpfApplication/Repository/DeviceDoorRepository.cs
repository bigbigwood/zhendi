using System.Collections;
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
    public class DeviceDoorRepository : BaseRepository<DeviceDoor, int>, IDeviceDoorRepository
    {
        public DeviceDoorRepository()
        {
            RelevantUri = "/api/DeviceDoors";
        }

        public override bool Update(DeviceDoor deviceDoor)
        {
            return Update(deviceDoor, deviceDoor.DeviceDoorID);
        }

        public Int32 QueryCount(Hashtable conditions)
        {
            throw new NotImplementedException();
        }
    }
}
