using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Message.GetUserOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetUserRequest : BaseRequest
    {
    }
}
