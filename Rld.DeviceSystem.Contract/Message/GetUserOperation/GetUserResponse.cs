using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.User;

namespace Rld.DeviceSystem.Contract.Message.GetUserOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetUserResponse : BaseResponse
    {
        [DataMember]
        public User UserInfo { get; set; }
    }
}
