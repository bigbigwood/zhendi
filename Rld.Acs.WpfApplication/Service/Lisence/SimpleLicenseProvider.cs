using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Rld.Acs.WpfApplication.Models;

namespace Rld.Acs.WpfApplication.Service.Lisence
{
    class SimpleLicenseProvider : LicenseProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            Log.Info("Provider GetLicense...");
            try
            {
                var _HideFilePath = LisenceService.GetLisencePath();
                var readfile = FileReadWrite.ReadFile(_HideFilePath);
                if (string.IsNullOrWhiteSpace(readfile))
                {
                    Log.Warn("No license is found.");
                    return null;
                }

                var info = readfile.Split(';');
                return new SimpleLicense(info[1], long.Parse(info[0]), int.Parse(info[2]));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw new LicenseException(type, instance, "License is invalid");
            }
        }
    }
}
