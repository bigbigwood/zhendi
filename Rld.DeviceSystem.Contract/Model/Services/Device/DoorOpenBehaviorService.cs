using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.Device
{
    [DataContract(Namespace = Declarations.NameSpace)]
    [KnownType("KnownTypes")]
    public class DoorOpenBehaviorService : ServiceBase
    {
        [DataMember]
        public DoorOpenBehavior Type { get; set; }
        [DataMember]
        public virtual Int32 Seconds { get; set; }

        static IEnumerable<Type> KnownTypes()
        {
            IList<Type> typesOfResource =
                TypesResolver.GetKnownTypes(null).Where(x => typeof(DoorOpenBehaviorService).IsAssignableFrom(x)).ToList();
            return typesOfResource;
        }
    }

    [DataContract(Namespace = Declarations.NameSpace)]
    public class DoorUnlockOpenBehaviorService : DoorOpenBehaviorService
    {
        [DataMember]
        public Int32 TimezoneId { get; set; }

        /// <summary>
        /// hide this second field
        /// </summary>
        public override Int32 Seconds { get; set; }
    }
}
