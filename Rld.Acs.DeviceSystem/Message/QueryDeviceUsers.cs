using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Rld.Acs.DeviceSystem.Model;
using Rld.Acs.Model;

namespace Rld.Acs.DeviceSystem.Message
{
    [MessageContract(IsWrapped = true)]
    public class QueryDeviceUsersRequest : RequestBase
    {
        [MessageBodyMember]
        public String UserCode { get; set; }

        [MessageBodyMember]
        public DeviceController Device { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class QueryDeviceUsersResponse : ResponseBase
    {
        [MessageBodyMember]
        public List<DeviceUserDto> Users { get; set; }
    }
}