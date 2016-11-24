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
using Rld.Acs.Model;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Excel;
using Rld.Acs.WpfApplication.Service.ExportFile;
using Rld.Acs.WpfApplication.Service.Language;

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
                    FileName = string.Format("{0}1", LanguageManager.GetLocalizationResource(Resource.WorkSheet)),
                    DefaultExt = ".xls",
                    Filter = string.Format("Excel {0}(*.xls)|*.xls", LanguageManager.GetLocalizationResource(Resource.WorkSheet)),
                };
                if (dlg.ShowDialog() == true)
                {
                    var dt = DataContext as DataTable;
                    ExcelService.ExportDataTableToExcel(dt, dlg.FileName, "Sheet");
                    ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_ExportSuccess)));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_ExportFail)));
            }
        }

        private void BtnExportPDF_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog()
                {
                    FileName = string.Format("{0}1", LanguageManager.GetLocalizationResource(Resource.WorkSheet)),
                    DefaultExt = ".pdf",
                    Filter = "PDF (*.pdf)|*.pdf",
                };
                if (dlg.ShowDialog() == true)
                {
                    var dt = DataContext as DataTable;
                    PdfService.ExportDataTable(dt, dlg.FileName);
                    ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_ExportSuccess)));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_ExportFail)));
            }
        }

        private void BtnExport2007_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog()
                {
                    FileName = string.Format("{0}1", LanguageManager.GetLocalizationResource(Resource.WorkSheet)),
                    DefaultExt = ".xlsx",
                    Filter = string.Format("Excel {0}(*.xlsx)|*.xlsx", LanguageManager.GetLocalizationResource(Resource.WorkSheet)),
                };
                if (dlg.ShowDialog() == true)
                {
                    var dt = DataContext as DataTable;
                    ExcelService.ExportDataTableToExcel2007(dt, dlg.FileName, "Sheet");
                    ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_ExportSuccess)));
                }
                }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_ExportFail)));
            }
        }

        private void BtnExportCSV_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog()
                {
                    FileName = string.Format("{0}1", LanguageManager.GetLocalizationResource(Resource.WorkSheet)),
                    DefaultExt = ".csv",
                    Filter = string.Format("CSV ({0})(*.csv)|*.csv", LanguageManager.GetLocalizationResource(Resource.Comma)),
                };
                if (dlg.ShowDialog() == true)
                {
                    var dt = DataContext as DataTable;
                    TextExportingService.ExportDataTable(dt, dlg.FileName, ",");
                    ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_ExportSuccess)));
                }
                }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_ExportFail)));
            }
        }

        private void BtnExportTxt_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.SaveFileDialog()
                {
                    FileName = string.Format("{0}1", LanguageManager.GetLocalizationResource(Resource.WorkSheet)),
                    DefaultExt = ".txt",
                    Filter = "Txt (*.txt)|*.txt",
                };
                if (dlg.ShowDialog() == true)
                {
                    var dt = DataContext as DataTable;
                    TextExportingService.ExportDataTable(dt, dlg.FileName, " ");
                    ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_ExportSuccess)));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_ExportFail)));
            }
        }
    }
}
