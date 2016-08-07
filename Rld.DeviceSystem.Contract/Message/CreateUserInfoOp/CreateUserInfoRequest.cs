﻿using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.CreateUserInfoOp
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class CreateUserInfoRequest : RequestBase
    {
        [DataMember]
        public UserInfo UserInfo { get; set; }
    }
}
