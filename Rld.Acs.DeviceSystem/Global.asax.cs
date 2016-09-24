using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using Rld.Acs.DeviceSystem.Framework;

namespace Rld.Acs.DeviceSystem
{
    public class Global : System.Web.HttpApplication
    {
        public const Int32 UserId = 3;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Application start");

            Initializer.Initialize();
            Log.Info("Initialization complete");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            Log.Error("Unhandled error in the aplication", exc);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Log.Info("Application end.");
        }
    }
}