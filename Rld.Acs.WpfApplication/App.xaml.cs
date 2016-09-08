using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using FluentValidation;
using GalaSoft.MvvmLight.Threading;
using log4net;
using Rld.Acs.WpfApplication.Service.Lisence;
using Rld.Acs.WpfApplication.View.Windows;
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
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Application start.");

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            DispatcherHelper.Initialize();

            ProvisioningModelMapper.BindModelMap();

            ApplicationEnvironment.Initialize();

            Log.Info("Application start completely.");
        }
    }
}
