using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rld.Acs.Model;

namespace Rld.Acs.WebApi.Framework
{
    public static class SysOperatorExtension
    {
        public static void MaskPassword(this SysOperator sysOperatorInfo)
        {
            if (sysOperatorInfo != null)
            {
                sysOperatorInfo.Salt = "";
                sysOperatorInfo.Password = "";
            }
        }

        public static string ExcryptPassword(string originalPassowrd, string salt)
        {
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(originalPassowrd + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }
    }
}