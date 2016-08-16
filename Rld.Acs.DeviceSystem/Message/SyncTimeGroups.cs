﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Rld.Acs.Model;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.Acs.DeviceSystem.Message
{
    [MessageContract(IsWrapped = true)]
    public class SyncTimeGroupsRequest : RequestBase
    {
        [MessageBodyMember]
        public List<TimeGroup> TimeGroups { get; set; }

        [MessageBodyMember]
        public List<DeviceController> Devices { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class SyncTimeGroupsResponse : ResponseBase
    {
    }
}