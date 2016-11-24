using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using log4net;
using MahApps.Metro;
using Rld.Acs.Model;
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.Service.Lisence;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// WelcomeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        private void WelcomeWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ShowMessage(LanguageManager.GetLocalizationResource(Resource.MSG_CheckingLisence));
            try
            {
                if (!LisenceService.RequestLisence())
                {
                    new LisenceWindow().ShowDialog();
                    Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowMessage(LanguageManager.GetLocalizationResource(Resource.MSG_LisenceFail));
                Thread.Sleep(2000);
                Close();
            }

            ShowMessage(LanguageManager.GetLocalizationResource(Resource.MSG_LoadingData));
            new Task(AppLoad).Start();
        }

        private void ShowMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                tbInfo.Text = message;
            });

            Log.Info(message);
        }

        private void AppLoad()
        {
            try
            {
                ApplicationManager.Initialize();

                DictionaryManager.Initialize();

                ShowMessage(LanguageManager.GetLocalizationResource(Resource.MSG_LoadDataComplete));
                Dispatcher.Invoke(() =>
                {
                    var loginForm = new sysLogin();
                    Application.Current.MainWindow = loginForm;
                    loginForm.Show();

                    Close();
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowMessage(LanguageManager.GetLocalizationResource(Resource.MSG_LoadDataFail));
            }

        }
    }
}
