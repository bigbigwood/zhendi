using Rld.DeviceSystem.Contract.Model.Users;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Rld.DeviceSystem.Contract.Message.DeleteUserOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DeleteUserRequest : RequestBase
    {
        [DataMember]
        public Int32 UserId { get; set; }
    }
}
