using System;
using System.Linq;
using System.Windows;
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
        public sysLogin()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            
        }

        public void userLogin(object sender, RoutedEventArgs e)
        {
            string username = userName.Text;
            string pass = passWord.Password;

            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(pass);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashpassword = Convert.ToBase64String(hashBytes);

            var service = new SecurityService();
            var authenticationResult = service.Authenticate(username, hashpassword);
            if (authenticationResult.ResultType != ResultType.OK)
            {
                loginInfo.Content = "登陆信息错误";
                loginInfo.Visibility = Visibility.Visible;
                return;
            }

            ApplicationManager.GetInstance().UpdateCurrentOperatorAndPermission(ToModel(authenticationResult.OperatorInfo));

            Window mainWin = new MainWindow();
            Application.Current.MainWindow = mainWin;
            mainWin.Show();
            Close();
        }

        private RldModel.SysOperator ToModel(SysOperator operatorInfo)
        {
            var sysOperator = new RldModel.SysOperator();
            sysOperator.OperatorID = operatorInfo.OperatorID;
            sysOperator.LoginName = operatorInfo.LoginName;
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

        public void loginRevoke(object sender, RoutedEventArgs e)
        {
            userName.Text = " ";
            passWord.Password = " ";
        }
        
    }
}
