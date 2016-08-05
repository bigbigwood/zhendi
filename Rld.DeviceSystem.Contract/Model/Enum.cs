using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model
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

    [DataContract(Namespace = Declarations.NameSpace)]
    public enum DoorLinkageOptions
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Master = 1,
        [EnumMember]
        Slave = 2,
        [EnumMember]
        All = 3,
    }

    [DataContract(Namespace = Declarations.NameSpace)]
    public enum DoorOpenBehavior
    {
        [EnumMember]
        DelayOpen = 0,
        [EnumMember]
        IllegalOpen = 1,
        [EnumMember]
        OverTimeOpen = 2,
        [EnumMember]
        UnlockOpen = 3,
    }

    [DataContract(Namespace = Declarations.NameSpace)]
    public enum CheckOutOptions
    {
        [EnumMember]
        Button = 0,
        [EnumMember]
        Password = 1,
        [EnumMember]
        Card = 2,
        [EnumMember]
        FingerPrint = 3,
    }

    [DataContract(Namespace = Declarations.NameSpace)]
    public enum DoorType
    {
        [EnumMember]
        Undefined = 0,
        [EnumMember]
        Master = 1,
        [EnumMember]
        Slave = 2,
    }
}
