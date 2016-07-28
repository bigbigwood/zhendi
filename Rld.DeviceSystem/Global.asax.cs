using System;
using log4net;

namespace Rld.DeviceSystem
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Application start");
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