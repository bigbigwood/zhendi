using System;

namespace Rld.Acs.Model
{
    public class FloorDoor
    {
        public virtual Int32 FloorDoorID { get; set; }
        public virtual Int32 FloorID { get; set; }
        public virtual Int32 DoorID { get; set; }
        public virtual Int32 DoorType { get; set; }
        public virtual Double LocationX { get; set; }
        public virtual Double LocationY { get; set; }
        public virtual Int32 Rotation { get; set; }
    }
}
