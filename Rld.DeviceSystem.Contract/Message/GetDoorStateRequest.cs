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

    }


    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDoorStateResponse : ResponseBase
    {
        [DataMember]
        public IList<DoorStateInfo> DoorStateLogs { get; set; }
    }
}
