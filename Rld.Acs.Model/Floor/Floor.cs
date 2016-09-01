using System;
using System.Collections.Generic;

namespace Rld.Acs.Model
{
    public class Floor
    {
        public virtual Int32 FloorID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Photo { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual List<FloorDoor> Doors { get; set; }

        public Floor()
        {
            Doors = new List<FloorDoor>();
        }
    }
}
