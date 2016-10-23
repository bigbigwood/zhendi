using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            ShowMessage("检查用户许可证...");
            var lisence = LisenceService.GetLicense();
            if (lisence == null || lisence.IsExpired)
            {
                var lisenceWindow = new LisenceWindow();
                lisenceWindow.DataContext = lisence;
                lisenceWindow.ShowDialog();
                if (lisenceWindow.Lisenced != true)
                {
                    Close();
                }
            }

            ShowMessage("加载数据中...");
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

                ShowMessage("加载数据完成！");
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
                ShowMessage("加载数据出错！");
            }

        }
    }
}
