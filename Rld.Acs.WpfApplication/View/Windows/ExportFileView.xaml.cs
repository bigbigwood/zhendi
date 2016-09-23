using System;
using System.Collections.Generic;
using System.Data;
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
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Excel;
using Rld.Acs.WpfApplication.Service.ExportFile;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for MoveUserView.xaml
    /// </summary>
    public partial class ExportFileView : BaseWindow
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ExportFileView()
        {
            InitializeComponent();
        }

        private void BtnExport2003_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog()
                {
                    FileName = "工作簿1",
                    DefaultExt = ".xls",
                    Filter = "Excel 工作簿(*.xls)|*.xls",
                };
                if (dlg.ShowDialog() == true)
                {
                    var dt = DataContext as DataTable;
                    ExcelService.ExportDataTableToExcel(dt, dlg.FileName, "Sheet1");
                    ShowSubViewNotification(new NotificationMessage("导出成功"));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSubViewNotification(new NotificationMessage("导出失败"));
            }
        }

        private void BtnExportPDF_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog()
                {
                    FileName = "工作簿1",
                    DefaultExt = ".pdf",
                    Filter = "PDF (*.pdf)|*.pdf",
                };
                if (dlg.ShowDialog() == true)
                {
                    var dt = DataContext as DataTable;
                    PdfService.ExportDataTable(dt, dlg.FileName);
                    ShowSubViewNotification(new NotificationMessage("导出成功"));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSubViewNotification(new NotificationMessage("导出失败"));
            }
        }

        private void BtnExport2007_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog()
                {
                    FileName = "工作簿1",
                    DefaultExt = ".xlsx",
                    Filter = "Excel 工作簿(*.xlsx)|*.xlsx",
                };
                if (dlg.ShowDialog() == true)
                {
                    var dt = DataContext as DataTable;
                    ExcelService.ExportDataTableToExcel2007(dt, dlg.FileName, "Sheet1");
                    ShowSubViewNotification(new NotificationMessage("导出成功"));
                }
                }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSubViewNotification(new NotificationMessage("导出失败"));
            }
        }

        private void BtnExportCSV_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog()
                {
                    FileName = "工作簿1",
                    DefaultExt = ".csv",
                    Filter = "CSV (逗号分隔)(*.csv)|*.csv",
                };
                if (dlg.ShowDialog() == true)
                {
                    var dt = DataContext as DataTable;
                    TextExportingService.ExportDataTable(dt, dlg.FileName);
                    ShowSubViewNotification(new NotificationMessage("导出成功"));
                }
                }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSubViewNotification(new NotificationMessage("导出失败"));
            }
        }
    }
}
