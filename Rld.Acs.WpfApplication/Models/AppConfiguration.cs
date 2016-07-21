using log4net;

namespace Rld.Acs.WpfApplication.Models
{
    public class AppConfiguration
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string LocalCachePath { get; private set; }

        static AppConfiguration()
        {
            LocalCachePath = "Rld.Acs.WpfApplication";
        }
    }
}
