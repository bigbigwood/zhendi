using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Logs;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDoorStateRequest : RequestBase
    {
        [DataMember]
        public Int32 DoorIndex { get; set; }
    }


    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDoorStateResponse : ResponseBase
    {
        [DataMember]
        public DoorStateInfo DoorStateInfo { get; set; }
    }
}
