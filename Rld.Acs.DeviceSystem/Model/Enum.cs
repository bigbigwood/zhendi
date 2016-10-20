using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract;

namespace Rld.Acs.DeviceSystem.Model
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public enum SyncOption
    {
        [EnumMember]
        Unknown = 0,
        [EnumMember]
        Create = 1,
        [EnumMember]
        Update = 2,
        [EnumMember]
        Delete = 3,
    }

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