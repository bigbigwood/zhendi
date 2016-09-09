using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using log4net;
using Rld.Acs.WpfApplication.Models;

namespace Rld.Acs.WpfApplication.Service.Lisence
{
    static class LisenceService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string identifier;
        private static string softwareName;

        public static SimpleLicense GetLicense()
        {
            var worker = new LisenceCheckWorker();
            var lisence = worker.GetLisence();
            if (lisence != null)
            {
                Log.DebugFormat("{0} lisence expired dateime is: {1}", (LisenceType)lisence.LicenseType, lisence.ExpireDateTime);
            }

            return lisence;
        }

        public static bool UpdateLisence(string key)
        {
            try
            {
                var sn = SnProvider.CalculateSN();
                var password = Encryption.MakePassword(sn, AppConfiguration.ApplicationIdentifier);
                var coreKey = key.Replace("-", string.Empty);
                if (password.Substring(3, 22) != coreKey.Substring(3, 22))
                {
                    throw new Exception("invalid lisence key!");
                }

                var expiredDatetime = GetExpiredDateTime(coreKey);
                string HideInfo = string.Format("{0};{1};{2}", expiredDatetime.ToString(), coreKey, LisenceType.Full.GetHashCode());
                if (File.Exists(GetLisencePath()))
                {
                    File.Delete(GetLisencePath());
                }
                FileReadWrite.WriteFile(GetLisencePath(), HideInfo);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public static bool ApplyTrialLisence()
        {
            try
            {
                if (GetLicense() != null)
                {
                    throw new Exception("Already has lisence key!");
                }

                var sn = SnProvider.CalculateSN(softwareName);
                var password = "142" + sn.Substring(3, 22); // trial version..
                var expiredDatetime = GetExpiredDateTime(password);
                string HideInfo = string.Format("{0};{1};{2}", expiredDatetime.ToString(), password, LisenceType.Trial.GetHashCode());
                FileReadWrite.WriteFile(GetLisencePath(), HideInfo);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public static string GetLisencePath()
        {
            return string.Format(@"{0}\lsf.reg", ApplicationEnvironment.LocalCachePath); ;
        }

        private static DateTime GetExpiredDateTime(string coreKey)
        {
            var debugmode = (ConfigurationSettings.AppSettings.Get("DebugMode") == "true");

            var amount = coreKey.Substring(0, 2);
            if (amount.StartsWith("A")) { amount = amount.Substring(1, 1); }
            var intAmount = int.Parse(amount);

            var unit = coreKey.Substring(2, 1);

            DateTime expireDateTime = DateTime.Now;
            if (unit == "0")
            {
                expireDateTime = debugmode ? DateTime.Now.AddMinutes(intAmount) : DateTime.Now.AddYears(intAmount);
            }
            else if (unit == "1")
            {
                expireDateTime = debugmode ? DateTime.Now.AddMinutes(intAmount) : DateTime.Now.AddMonths(intAmount);
            }
            else if (unit == "2")
            {
                expireDateTime = debugmode ? DateTime.Now.AddMinutes(intAmount) : DateTime.Now.AddDays(intAmount);
            }
            else
            {
                throw new Exception("Invalid key due to the expired datetime info is broken.");
            }

            return expireDateTime;
        }
    }
}
