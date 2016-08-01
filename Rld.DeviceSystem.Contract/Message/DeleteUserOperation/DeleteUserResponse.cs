using System.Runtime.Serialization;
using System.ServiceModel;
using Rld.DeviceSystem.Contract.Model.Users;

namespace Rld.DeviceSystem.Contract.Message.DeleteUserOperation
{
    [MessageContract(IsWrapped = true)]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DeleteUserResponse : ResponseBase
    {
    }
}
