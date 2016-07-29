using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Configuration;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Application start");

            var deviceConfigurations = GetDeviceConfigurations("DeviceConfigGroup");
            DeviceManager.Initialize(deviceConfigurations);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            Log.Error("Unhandled error in the aplication", exc);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Log.Info("Application end.");
            DeviceManager.GetInstance().Stop();
        }

        private List<DeviceConfigurationBase> GetDeviceConfigurations(String groupName)
        {
            var config = GetConfiguration();
            var deviceConfigGroup = config.SectionGroups["DeviceConfigGroup"];
            if (deviceConfigGroup == null)
                return new List<DeviceConfigurationBase>();
            else
                return deviceConfigGroup.Sections.Cast<DeviceConfigurationBase>().ToList();
        }

        private System.Configuration.Configuration GetConfiguration()
        {
            System.Configuration.Configuration configuration = null;
            if (System.Web.HttpContext.Current != null)
            {
                configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            }
            else
            {
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }

            return configuration;
        }
    }
}