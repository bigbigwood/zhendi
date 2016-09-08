using System.Configuration;
using log4net;

namespace Rld.Acs.WpfApplication.Models
{
    public class AppConfiguration
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string BaseServer { get; private set; }
        public static string DeviceSystemUrl { get; private set; }
        public static string LocalCachePath { get; private set; }
        public static string ApplicationName { get; private set; }
        public static string ApplicationIdentifier { get; private set; }

        static AppConfiguration()
        {
            LocalCachePath = "Rld.Acs.WpfApplication";
            ApplicationName = "RAW";
            ApplicationIdentifier = "136";
            BaseServer = ConfigurationSettings.AppSettings.Get("BaseUri");
            DeviceSystemUrl = ConfigurationSettings.AppSettings.Get("DeviceSystemUrl");
        }
    }
}
