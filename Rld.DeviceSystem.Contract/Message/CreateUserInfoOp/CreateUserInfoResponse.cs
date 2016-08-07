using System.Runtime.Serialization;
using System.ServiceModel;

namespace Rld.DeviceSystem.Contract.Message.CreateUserInfoOp
{
    [MessageContract(IsWrapped = true)]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class CreateUserInfoResponse : ResponseBase
    {
    }
}
