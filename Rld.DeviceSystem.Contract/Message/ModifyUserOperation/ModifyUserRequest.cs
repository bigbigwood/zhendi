using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.GetUserOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ModifyUserRequest : RequestBase
    {
        [DataMember]
        public UserInfo UserInfo { get; set; }
    }
}
