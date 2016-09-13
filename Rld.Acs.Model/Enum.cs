using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Model
{
    public enum GeneralStatus
    {
        Disabled = 0,
        Enabled = 1,
    }

    public enum DevicePermissionAction
    {
        User = 1,
        Registrar = 2,
        LogQuery = 4,
        Manager = 8,
        Custom = 16,
    }

    public enum CommunicationType
    {
        USB,
        P2P,
        TCP_IP,
    }

    public enum UserType
    {
        Employee = 1,
        Visitor = 2,
    }

    public enum IDType
    {
        ID = 0,
        Passport = 1,
    }

    public enum GenderType
    {
        Male = 0,
        Female = 1,
    }

    public enum Marriage
    {
        Single = 0,
        Married = 1,
    }

    public enum DeviceType
    {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
        Type4 = 4,
    }

    public enum AuthenticationType
    {
        FingerPrint1 = 0,
        FingerPrint2 = 1,
        FingerPrint3 = 2,
        FingerPrint4 = 3,
        FingerPrint5 = 4,
        FingerPrint6 = 5,
        FingerPrint7 = 6,
        FingerPrint8 = 7,
        FingerPrint9 = 8,
        FingerPrint10 = 9,
        Password = 10,
        IcCard = 11,
        //FacePrint = 12,
    }

    [Flags]
    public enum CheckInOptions
    {
        Password = 1,
        Card = 2,
        FingerPrint = 4,
        Wiegand = 8,
    }

    public enum DictionaryLevel
    {
        TypeHeaderLevel = 1,
        TypeItemsLevel = 2,
    }

    public enum DoorControlOption
    {
        Open = 0,
        KeepOpen = 1,
        KeepClose = 2,
        Auto = 3,
    }

    public enum DictionaryType
    {
        Nationality = 10001, //民族
        Gender = 10002, //性别
        DevicePermission = 10003, //设备权限
        SystemPermission = 10004, //系统权限
        HeadReaderType = 10005, //读头类型
        CheckOutOptions = 10006, //出门选项
        RingType = 10007, //铃声类型
        CommunicationType = 10008, //通讯类型
        Protocol = 10009, //通讯协议
        AuthticationType = 10010, //验证模式
        DeviceTrafficLogType = 10011, //日志记录类型
        CheckInOptions = 10012, //访问记录验证选择
        GeneralStatus = 10013, //人员状态
    }
}
