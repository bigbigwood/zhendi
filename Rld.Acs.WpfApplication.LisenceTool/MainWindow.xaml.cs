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
using Rld.Acs.WpfApplication.Service.Lisence;

namespace Rld.Acs.WpfApplication.LisenceTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnActivate_OnClick(object sender, RoutedEventArgs e)
        {
            var amount = int.Parse(tbAmount.Text);
            var unit = (LisenceUnit) cbLisenceUnit.SelectedValue;

            if (unit == LisenceUnit.Year)
            {
                if (amount < 1 || amount > 99)
                {
                    MessageBox.Show("当单位为年时，有效期年份值必须在1-99之间");
                    return;
                }
            }
            else if (unit == LisenceUnit.Month)
            {
                if (amount < 1 || amount > 12)
                {
                    MessageBox.Show("当单位为月时，有效期年份值必须在1-12之间");
                    return;
                }
            }
            else if (unit == LisenceUnit.Day)
            {
                if (amount < 1 || amount > 30)
                {
                    MessageBox.Show("当单位为天时，有效期年份值必须在1-30之间");
                    return;
                }
            }

            var key = LisenceService.GenerateLisence(tbSN.Text, amount, unit);
            tbKey.Text = key;
        }
    }
}
