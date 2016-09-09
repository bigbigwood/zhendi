using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using log4net;
using Rld.Acs.WpfApplication.LisenceTool;

namespace Rld.Acs.WpfApplication.Service.Lisence
{
    static class LisenceService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string GenerateLisence(string sn, int amount, LisenceUnit unit)
        {
            sn = LisenceTool.ToRawCodeFormat(sn);

            var ApplicationIdentifier = ConfigurationSettings.AppSettings.Get("ApplicationIdentifier");
            var password = Encryption.MakePassword(sn, ApplicationIdentifier);

            string eAmount = amount.ToString();
            if (eAmount.Count() == 1) {eAmount = "A" + eAmount;}

            string eUnit = unit.GetHashCode().ToString();

            var passwordWithEnddate = eAmount + eUnit + password.Substring(3, 22);
            
            return LisenceTool.ToProductCodeFormat(passwordWithEnddate);
        }
    }
}
