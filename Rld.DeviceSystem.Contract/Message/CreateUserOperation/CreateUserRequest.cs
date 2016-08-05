using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.CreateUserOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class CreateUserRequest : RequestBase
    {
        [DataMember]
        public UserInfo UserInfo { get; set; }
    }
}
