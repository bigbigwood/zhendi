using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Rld.Acs.Model;

namespace Rld.Acs.DeviceSystem.Message
{
    [MessageContract(IsWrapped = true)]
    public class SyncDeviceUsersRequest : RequestBase
    {
        [MessageBodyMember]
        public SyncOption Option { get; set; }
        [MessageBodyMember]
        public List<User> Users { get; set; }

        [MessageBodyMember]
        public List<DeviceController> Devices { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class SyncDeviceUsersResponse : ResponseBase
    {
    }
}