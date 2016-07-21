using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Rld.Acs.WpfApplication.View
{
    public class BaseWindow : MetroWindow
    {
        #region permission
        #endregion

        #region Message box
        public async void ShowDialog(string message, string title)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "关闭",
                ColorScheme = MetroDialogColorScheme.Theme
            };
            MessageDialogResult result =
                await this.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, mySettings);
        }

        public async void ShowYesNo(string message, string title, Action action)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "确定",
                NegativeButtonText = "取消",
                ColorScheme = MetroDialogColorScheme.Theme
            };
            MessageDialogResult result =
                await this.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
                await Task.Factory.StartNew(action);
        } 
        #endregion
    }
}
