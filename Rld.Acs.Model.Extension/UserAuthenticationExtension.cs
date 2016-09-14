using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public static class UserAuthenticationExtension
    {
        public static UserAuthentication WiseClone(this UserAuthentication userAuthentication)
        {
            return new UserAuthentication()
            {
                //UserAuthenticationID = userAuthentication.UserAuthenticationID,
                UserID = userAuthentication.UserID,
                DeviceID = userAuthentication.DeviceID,
                DeviceUserID = userAuthentication.DeviceUserID,
                AuthenticationType = userAuthentication.AuthenticationType,
                AuthenticationData = userAuthentication.AuthenticationData,
                Version = userAuthentication.Version,
                IsDuress = userAuthentication.IsDuress,
                Remark = userAuthentication.Remark,
                //CreateUserID = userAuthentication.CreateUserID,
                //CreateDate = userAuthentication.CreateDate,
                Status = userAuthentication.Status,
                //UpdateUserID = userAuthentication.UpdateUserID,
                //UpdateDate = userAuthentication.UpdateDate,
            };
        }
    }
}
