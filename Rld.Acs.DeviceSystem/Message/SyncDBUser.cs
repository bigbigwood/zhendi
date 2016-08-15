using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.Acs.DeviceSystem.Message
{
    [MessageContract(IsWrapped = true)]
    public class SyncDBUserRequest : RequestBase
    {
        [MessageBodyMember]
        public List<UserInfo> DbUsers { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class SyncDBUserResponse : ResponseBase
    {
    }
}