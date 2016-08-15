using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Rld.Acs.Model;

namespace Rld.Acs.DeviceSystem.Message
{
    [MessageContract(IsWrapped = true)]
    public class SyncDeviceUserRequest : RequestBase
    {
        [MessageBodyMember]
        public List<User> DbUsers { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class SyncDeviceUserResponse : ResponseBase
    {
    }
}