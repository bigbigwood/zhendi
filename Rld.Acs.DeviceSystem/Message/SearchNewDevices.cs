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
    public class SearchNewDevicesRequest : RequestBase
    {
    }

    [MessageContract(IsWrapped = true)]
    public class SearchNewDevicesResponse : ResponseBase
    {
        [MessageBodyMember]
        public List<int> NewDeviceCodes { get; set; }

        public SearchNewDevicesResponse()
        {
            NewDeviceCodes = new List<int>();
        }
    }
}