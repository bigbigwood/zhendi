using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Rld.Acs.WpfApplication
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
