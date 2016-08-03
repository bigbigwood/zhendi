﻿using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Message.GetTimeSegmentOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetTimeSegmentResponse : ResponseBase
    {
        [DataMember]
        public TimeSegmentService Service { get; set; }

        public GetTimeSegmentResponse()
        {
            Service = new TimeSegmentService();
        }
    }
}