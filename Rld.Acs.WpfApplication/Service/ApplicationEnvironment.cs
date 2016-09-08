using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Authorization;
using Rld.Acs.WpfApplication.ViewModel;

namespace Rld.Acs.WpfApplication
{
    public static class ApplicationEnvironment
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string LocalCachePath { get; private set; }
        public static string LocalImageCachePath { get; private set; }

        public static void Initialize()
        {
            Log.Info("Init local cache...");
            LocalCachePath = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppConfiguration.LocalCachePath);
            if (Directory.Exists(LocalCachePath) == false)
            {
                Directory.CreateDirectory(LocalCachePath);
            }

            LocalImageCachePath = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppConfiguration.LocalCachePath + @"\images");
            if (Directory.Exists(LocalImageCachePath) == false)
            {
                Directory.CreateDirectory(LocalImageCachePath);
            }
        }
    }
}
