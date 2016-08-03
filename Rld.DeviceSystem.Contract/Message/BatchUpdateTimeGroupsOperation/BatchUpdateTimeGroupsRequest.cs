﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Message.BatchUpdateTimeGroupsOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class BatchUpdateTimeGroupsRequest : RequestBase
    {
        [DataMember]
        public IEnumerable<TimeGroupService> Services { get; set; }
    }
}
