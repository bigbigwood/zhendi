using System;
using System.Collections.Generic;
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
        Pass,
        ReadLog,
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

    public enum GenderType
    {
        Male,
        Female,
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
        IcCard = 1,
        Password = 2,
        FingerPrint = 3,
        FacePrint = 4,
    }


}
