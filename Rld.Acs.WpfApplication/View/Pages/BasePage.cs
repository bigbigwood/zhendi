using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace Rld.Acs.WpfApplication.View.Pages
{
    public class BasePage : Page
    {
        public virtual void ShowQuestionAndAction(NotificationMessageAction msg, string title)
        {
            MessageBoxSingleton.Instance.ShowYesNo(msg.Notification, title, msg.Execute);
        }

        public virtual void ShowMessage(NotificationMessage msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Notification))
                MessageBoxSingleton.Instance.ShowDialog(msg.Notification, "");
        }

        public virtual void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }

        protected void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        protected void DataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGrid_LoadingRow(sender, e);
            var datagrid = sender as DataGrid;
            if (datagrid.Items != null)
            {
                for (int i = 0; i < datagrid.Items.Count; i++)
                {
                    try
                    {
                        DataGridRow row = datagrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                        if (row != null)
                        {
                            row.Header = (i + 1).ToString();
                        }
                    }
                    catch { }
                }
            }

        }
    }
}
