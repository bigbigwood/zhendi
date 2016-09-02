using System;
using System.Collections.Generic;
using System.Linq;

namespace Rld.Acs.Model.Extension
{
    public static class FloorExtension
    {

        public static String GetDoorList(this Floor floor, List<DeviceDoor> deviceDoors)
        {
            if (floor == null || floor.Doors == null || floor.Doors.Count == 0)
                return string.Empty;

            if (deviceDoors == null || deviceDoors.Count == 0)
                return string.Empty;

            var floorDoors = floor.Doors.Where(x => deviceDoors.Select(d => d.DeviceDoorID).Contains(x.DoorID));
            var doorNames = floorDoors.Select(x => deviceDoors.First(d => d.DeviceDoorID == x.DoorID).Name).Distinct();
            return string.Join(", ", doorNames);
        }
    }
}
