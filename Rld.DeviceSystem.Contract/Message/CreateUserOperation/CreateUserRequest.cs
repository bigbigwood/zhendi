using Rld.DeviceSystem.Contract.Model.Users;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Rld.DeviceSystem.Contract.Message.CreateUserOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class CreateUserRequest : RequestBase
    {
        [DataMember]
        public User UserInfo { get; set; }
    }
}
