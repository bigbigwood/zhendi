using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rld.Acs.Model;
using Rld.Acs.WpfApplication.Service.Language;

namespace Rld.Acs.WpfApplication.View.Windows
{
    public class BaseWindow : MetroWindow
    {
        #region permission
        #endregion

        #region Message box

        public virtual void ShowSubViewNotification(NotificationMessage msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Notification))
                ShowDialog(msg.Notification, "");
        }



        public virtual void ProcessCloseViewMessage(NotificationMessage msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Notification))
            {
                MessageBoxSingleton.Instance.ShowDialog(msg.Notification, "");
            }

            Close();
        }

        public virtual void ShowQuestionAndAction(NotificationMessageAction msg, string title)
        {
            ShowYesNo(msg.Notification, title, msg.Execute);
        }

        public virtual void ShowMainWindowNotification(NotificationMessage msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Notification))
                MessageBoxSingleton.Instance.ShowDialog(msg.Notification, "");
        }

        public virtual void MetroWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }

        public async void ShowDialog(string message, string title)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = LanguageManager.GetLocalizationResource(Resource.Close),
                ColorScheme = MetroDialogColorScheme.Theme
            };
            MessageDialogResult result =
                await this.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, mySettings);
        }

        public async void ShowYesNo(string message, string title, Action action)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = LanguageManager.GetLocalizationResource(Resource.Ok),
                NegativeButtonText = LanguageManager.GetLocalizationResource(Resource.Cancel),
                ColorScheme = MetroDialogColorScheme.Theme
            };
            MessageDialogResult result =
                await this.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
                await Task.Factory.StartNew(action);
        }
        #endregion

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
                        var row = datagrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
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
