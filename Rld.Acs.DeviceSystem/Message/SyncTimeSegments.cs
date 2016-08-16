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
    public class SyncTimeSegmentsRequest : RequestBase
    {
        [MessageBodyMember]
        public List<TimeSegment> TimeSegments { get; set; }

        [MessageBodyMember]
        public List<DeviceController> Devices { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class SyncTimeSegmentsResponse : ResponseBase
    {
    }
}