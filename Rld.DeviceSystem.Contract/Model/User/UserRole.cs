using System.Runtime.Serialization;
namespace Rld.DeviceSystem.Contract.Model.User
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public enum UserRole
    {
        [EnumMember]
        User = 1,
        [EnumMember]
        Registrar = 2,
        [EnumMember]
        LogQuery = 4,
        [EnumMember]
        Manager = 8,
        [EnumMember]
        Custom = 16,
    }
}
