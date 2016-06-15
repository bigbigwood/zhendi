using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Rld.Acs.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Application start");

            GlobalConfiguration.Configure(WebApiConfig.Register);

            Repository.RepositoryManager.AddAssemby(typeof(Repository.Mybatis.MsSql.NinjectBinder).Assembly);
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
