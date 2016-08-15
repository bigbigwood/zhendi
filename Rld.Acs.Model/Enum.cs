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
        Pass = 1,
        ReadLog = 2,
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

    public enum DictionaryType
    {
        Nationality = 10001, //民族
        Gender = 10002, //性别
    }
}
