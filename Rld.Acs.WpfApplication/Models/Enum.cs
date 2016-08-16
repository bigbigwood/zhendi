using System.ComponentModel;
using Rld.Acs.WpfApplication.View.Converter;

namespace Rld.Acs.WpfApplication.Models
{

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PoliticalStatus
    {
        [Description("未知")]
        Unknown = 0,
        [Description("群众")]
        PublicPeople = 1,
        [Description("团员")]
        LeagueMember = 2,
        [Description("党员")]
        PartyMember = 3,
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DegreeStatus
    {
        [Description("未知")]
        Unknown = 0,
        [Description("高中")]
        HighSchool = 1,
        [Description("大专")]
        College = 2,
        [Description("本科")]
        Bachelor = 3,
        [Description("硕士")]
        Master = 4,
        [Description("博士")]
        Doctor = 5,
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SyncUserType
    {
        [Description("同步设备数据到人员")]
        SyncDeviceToUser = 0,
        [Description("同步人员数据到设备")]
        SyncUserToDevice = 1,
    }
}
