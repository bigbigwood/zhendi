using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public enum UserRequestType
    {
        [EnumMember]
        All = 0,
        [EnumMember]
        UserName = 1,
        [EnumMember]
        UserStatus = 2,
        [EnumMember]
        UserRole = 3,
        [EnumMember]
        UserComment = 4,
        [EnumMember]
        ExternalUserCode = 5,
        [EnumMember]
        DepartmentId = 6,
        [EnumMember]
        CredentialCard = 7,
        [EnumMember]
        Password = 8,
        [EnumMember]
        FingerPrint0 = 9,
        [EnumMember]
        FingerPrint1 = 10,
        [EnumMember]
        FingerPrint2 = 11,
        [EnumMember]
        FingerPrint3 = 12,
        [EnumMember]
        FingerPrint4 = 13,
        [EnumMember]
        FingerPrint5 = 14,
        [EnumMember]
        FingerPrint6 = 15,
        [EnumMember]
        FingerPrint7 = 16,
        [EnumMember]
        FingerPrint8 = 17,
        [EnumMember]
        FingerPrint9 = 18,
    }
}
