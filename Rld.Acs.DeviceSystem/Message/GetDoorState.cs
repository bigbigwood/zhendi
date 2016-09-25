using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Rld.Acs.Model;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.Acs.DeviceSystem.Message
{
    [MessageContract(IsWrapped = true)]
    public class GetDoorStateRequest : RequestBase
    {
        [MessageBodyMember]
        public Int32 DoorIndex { get; set; }

        [MessageBodyMember]
        public Int32 DeviceCode { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class GetDoorStateResponse : ResponseBase
    {
        [MessageBodyMember]
        public Boolean IsOpened { get; set; }
    }
}