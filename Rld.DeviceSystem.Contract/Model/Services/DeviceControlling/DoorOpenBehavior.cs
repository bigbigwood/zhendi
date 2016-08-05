using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceControlling
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DoorOpenBehavior : ServiceBase
    {
        [DataMember]
        public String Type { get; set; }
        [DataMember]
        public String AlarmSeconds { get; set; }
    }
}
