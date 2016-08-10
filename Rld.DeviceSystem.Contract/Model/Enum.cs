using System;
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
    public enum DoorOperations
    {
        [EnumMember]
        Open = 1,
        [EnumMember]
        Auto = 2,
        [EnumMember]
        KeepOpen = 3,
        [EnumMember]
        KeepClose = 4,
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
    public enum CheckInOptions
    {
        [EnumMember]
        Password = 1,
        [EnumMember]
        Card = 2,
        [EnumMember]
        FingerPrint = 3,
        [EnumMember]
        Wiegand = 4,
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

    [DataContract(Namespace = Declarations.NameSpace)]
    public enum CommunicationType
    {
        [EnumMember]
        Serial = 0,
        [EnumMember]
        Tcp = 1,
        [EnumMember]
        Usb = 2,
        [EnumMember]
        P2P = 3,
        [EnumMember]
        WebSocket = 4,
    }

    [DataContract(Namespace = Declarations.NameSpace)]
    public enum SystemParameters
    {
        [EnumMember]
        AdminCount = 0,
        [EnumMember]
        LanguageFormat = 1,
        [EnumMember]
        IDLength = 2,
        [EnumMember]
        VolumeSize = 3,
        [EnumMember]
        AutoOffTime = 4,
        [EnumMember]
        AutoPowerOn = 5,
        [EnumMember]
        VerifyMode = 6,
        [EnumMember]
        AutoLearning = 7,
        [EnumMember]
        AutoReturnTime = 8,
        [EnumMember]
        StandbyTime = 9,
        [EnumMember]
        EnableAlarmInStandby = 10,
        [EnumMember]
        CardIDType = 11,
        [EnumMember]
        AutoRestart = 12,
        [EnumMember]
        EnableShutdown = 13,
        [EnumMember]
        EnableRelayAlarm = 14,
        [EnumMember]
        FireAlarm = 15,
        [EnumMember]
        OneToOneSecurityLevel = 16,
        [EnumMember]
        OneToNSecurityLevel = 17,
        [EnumMember]
        SLogWarningCount = 18,
        [EnumMember]
        GLogWarningCount = 19,
        [EnumMember]
        ReverifyTime = 20,
        [EnumMember]
        DeviceID = 21,
        [EnumMember]
        Baudrate = 22,
        [EnumMember]
        UserRealTimeLog = 23,
        [EnumMember]
        UDPPort = 24,
        [EnumMember]
        DevicePassword = 25,
        [EnumMember]
        IPAddress = 26,
        [EnumMember]
        SubNetAddress = 27,
        [EnumMember]
        DefaultGate = 28,
        [EnumMember]
        ServerIPAddress = 29,
        [EnumMember]
        ServerUDPPort = 30,
        [EnumMember]
        RS485Use = 31,
        [EnumMember]
        LockDelayTime = 32,
        [EnumMember]
        WiegandMode = 33,
        [EnumMember]
        CheckDoorState = 34,
        [EnumMember]
        MenaceOpenDoor = 35,
        [EnumMember]
        MenaceAlarm = 36,
    }

    [DataContract(Namespace = Declarations.NameSpace)]
    public enum AccessLogType
    {
        [EnumMember]
        General = 0,
        [EnumMember]
        Alarm = 1,
        [EnumMember]
        Exception = 2,
    }
}
