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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Rld.Acs.WpfApplication.Extension;
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Views;

namespace Rld.Acs.WpfApplication.Pages
{
    /// <summary>
    /// Interaction logic for DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : Page
    {
        public DepartmentPage()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.DepartmentPage_NoDepartmentIsSelected, 
                new Action<NotificationMessage>((msg) => ProcessNoDepartmentIsSelectedMessage()));
            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenDepartmentView, OpenDepartmentView);
        }

        private void DeleteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (lstDepartment.SelectedItem == null)
            {
                MessageBoxSingleton.Instance.ShowDialog("请先选择部门!", "");
                return;
            }

            string questionMessage = string.Format("确定删除部门: \"{0}\" 吗？", this.departmentDetailView.TextBlock_DepartmentName.Text);
            var message = new NotificationMessageAction(Tokens.DepartmentPage_DeleteDepartmentAction,
                ShowDepartmentDeletedDoneMessage);
            
            MessageBoxSingleton.Instance.ShowYesNo(questionMessage, "删除部门",
                () => Messenger.Default.Send(message, Tokens.DepartmentPage_DeleteDepartmentAction));
        }

        private void ShowDepartmentDeletedDoneMessage()
        {
            MessageBoxSingleton.Instance.ShowDialog("删除部门成功！", "删除部门");
        }

        private void ProcessNoDepartmentIsSelectedMessage()
        {
            MessageBoxSingleton.Instance.ShowDialog("请先选择部门!", "");
        }

        private void OpenDepartmentView(OpenWindowMessage msg)
        {
            var view = new DepartmentView {DataContext = msg.DataContext};
            view.ShowDialog();
        }
    }
}
