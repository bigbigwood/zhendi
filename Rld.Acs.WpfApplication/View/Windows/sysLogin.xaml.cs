using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.Service.Security;
using RldModel = Rld.Acs.Model;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for sysLogin.xaml
    /// </summary>
    public partial class sysLogin : MetroWindow
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public sysLogin()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
        }

        public void userLogin(object sender, RoutedEventArgs e)
        {
            string username = userName.Text;
            string password = passWord.Password;

            new Task(() => LoginInBackgroud(username, password)).Start();
        }

        public void loginRevoke(object sender, RoutedEventArgs e)
        {
        }


        private void LoginInBackgroud(string username, string password)
        {
            try
            {
                byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
                string hashpassword = Convert.ToBase64String(hashBytes);

                var service = new SecurityService();
                var authenticationResult = service.Authenticate(username, hashpassword);
                if (authenticationResult.ResultType != ResultType.OK)
                {
                    ShowMessage("登陆信息错误...");
                    return;
                }

                ApplicationManager.GetInstance().UpdateCurrentOperatorAndPermission(ToModel(authenticationResult.OperatorInfo));

                ShowMessage("登陆成功...");
                Dispatcher.Invoke(() =>
                {
                    Window mainWin = new MainWindow();
                    Application.Current.MainWindow = mainWin;
                    mainWin.Show();
                    Close();
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowMessage("登录异常...");
            }
        }

        private RldModel.SysOperator ToModel(SysOperator operatorInfo)
        {
            var sysOperator = new RldModel.SysOperator();
            sysOperator.OperatorID = operatorInfo.OperatorID;
            sysOperator.LoginName = operatorInfo.LoginName;
            sysOperator.Password = operatorInfo.Password;
            sysOperator.LanguageID = operatorInfo.LanguageID;
            sysOperator.Photo = operatorInfo.Photo;
            sysOperator.Status = (operatorInfo.Status == GeneralStatus.Enabled) ? RldModel.GeneralStatus.Enabled : RldModel.GeneralStatus.Disabled;
            sysOperator.UpdateUserID = operatorInfo.UpdateUserID;
            sysOperator.UpdateDate = operatorInfo.UpdateDate;
            sysOperator.CreateUserID = operatorInfo.CreateUserID;
            sysOperator.CreateDate = operatorInfo.CreateDate;

            sysOperator.SysOperatorRoles = operatorInfo.SysOperatorRoles.Select(x => new RldModel.SysOperatorRole()
            {
                SysOperatorRoleID = x.SysOperatorRoleID,
                OperatorID = x.OperatorID,
                RoleID = x.RoleID,
            }).ToList();

            return sysOperator;
        }


        private void ShowMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                loginInfo.Content = message;
            });

            Log.Info(message);
        }
    }
}
