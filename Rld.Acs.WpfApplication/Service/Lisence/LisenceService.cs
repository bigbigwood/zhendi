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
            return worker.GetLisence();
        }

        public static bool UpdateLisence(string key)
        {
            try
            {
                var sn = SnProvider.CalculateSN();
                var password = Encryption.MakePassword(sn, AppConfiguration.ApplicationIdentifier);
                if (LisenceTool.ToProductCodeFormat(password) != key)
                {
                    throw new Exception("invalid lisence key!");
                }

                long datetimeTick = AddYearsImpl(DateTime.Now, 10).Ticks;
                string HideInfo = string.Format("{0};{1};{2}", datetimeTick, password, LisenceType.Full.GetHashCode());
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
                var password = sn; // trial version..

                long datetimeTick = AddDaysImpl(DateTime.Now, 14).Ticks;
                string HideInfo = string.Format("{0};{1};{2}", datetimeTick, password, LisenceType.Trial.GetHashCode());
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

        private static DateTime AddDaysImpl(DateTime dt, int day)
        {
            var debugmode = (ConfigurationSettings.AppSettings.Get("DebugMode") == "true");
            return debugmode ? dt.AddMinutes(day) : dt.AddDays(day);
        }

        private static DateTime AddYearsImpl(DateTime dt, int day)
        {
            var debugmode = (ConfigurationSettings.AppSettings.Get("DebugMode") == "true");
            return debugmode ? dt.AddMinutes(day) : dt.AddYears(day);
        }
    }
}
