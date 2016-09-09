using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Service
{
    static class PasswordService
    {
        public static string ExcryptPassword(string originalPassowrd)
        {
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(originalPassowrd);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }
    }
}
