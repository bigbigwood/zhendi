using Rld.DeviceSystem.Contract.Model.Users;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Rld.DeviceSystem.Contract.Message.GetUserOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ModifyUserRequest : RequestBase
    {
        [DataMember]
        public User UserInfo { get; set; }
    }
}
