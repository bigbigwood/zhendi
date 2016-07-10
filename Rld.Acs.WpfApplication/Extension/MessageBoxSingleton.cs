using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace Rld.Acs.WpfApplication.Extension
{
    public sealed class MessageBoxSingleton
    {
        private static readonly MessageBoxSingleton instance = new MessageBoxSingleton();
        public static MessageBoxSingleton Instance { get { return instance; } } 

        /// <summary>
        /// 调用消息窗口的代理事件
        /// </summary>
        public Action<string, string> ShowDialog { get; set; }
        /// <summary>
        /// 调用消息确认窗口的代理事件
        /// </summary>
        public Action<string, string, Action> ShowYesNo { get; set; }
    }
}
