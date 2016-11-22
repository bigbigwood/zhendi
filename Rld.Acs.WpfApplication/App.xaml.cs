using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using FluentValidation;
using GalaSoft.MvvmLight.Threading;
using log4net;
using Rld.Acs.WpfApplication.Service.Lisence;
using Rld.Acs.WpfApplication.ViewModel;

namespace Rld.Acs.WpfApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public App()
        {
            Process[] processes = Process.GetProcessesByName(GetApplicationName());
            if (processes.Length > 1)
            {
                MessageBox.Show("There's already an application running.");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }

            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Application start.");

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            DispatcherHelper.Initialize();

            ModelMapper.BindModelMap();

            ApplicationEnvironment.Initialize();

            Log.Info("Application start completely.");
        }

        private String GetApplicationName()
        {
            return System.IO.Path.GetFileName(Assembly.GetEntryAssembly().GetName().Name);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            LisenceService.ReleaseLisence();
        }
    }
}
