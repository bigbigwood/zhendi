﻿using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.UpdateUserInfoOp
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UpdateUserInfoRequest : RequestBase
    {
        [DataMember]
        public UserInfo UserInfo { get; set; }
    }
}
