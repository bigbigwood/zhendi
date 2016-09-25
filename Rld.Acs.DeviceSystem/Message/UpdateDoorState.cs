using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.Acs.DeviceSystem.Message
{
    [MessageContract(IsWrapped = true)]
    public class UpdateDoorStateRequest : RequestBase
    {
        [MessageBodyMember]
        public DoorControlOption Option { get; set; }

        [MessageBodyMember]
        public Int32 DoorIndex { get; set; }

        [MessageBodyMember]
        public Int32 DeviceCode { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class UpdateDoorStateResponse : ResponseBase
    {
    }
}