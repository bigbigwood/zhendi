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
    public class SyncDevicesRequest : RequestBase
    {
        [MessageBodyMember]
        public List<int> DeviceIds { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class SyncDevicesResponse : ResponseBase
    {
    }
}