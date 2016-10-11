using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;
using Font = iTextSharp.text.Font;

namespace Rld.Acs.WpfApplication.Service.ExportFile
{
    public class PdfService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void ExportDataTable(DataTable datatable, string fileName)
        {
            try
            {
                Document document = new Document();
                using (FileStream file = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write))
                {
                    PdfWriter.GetInstance(document, file);
                    document.Open();

                    //var fontName = System.Drawing.SystemFonts.DefaultFont.Name;
                    //Log.InfoFormat("系统默认字体为:{0}", fontName);
                    //Font myFont = FontFactory.GetFont("微软雅黑", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                    BaseFont bfChinese = BaseFont.CreateFont(Environment.CurrentDirectory + @"/Resources/ARIALUNI.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font myFont = new Font(bfChinese, 12);

                    PdfPTable table = new PdfPTable(datatable.Columns.Count);
                    //table header
                    for (int j = 0; j < datatable.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(datatable.Columns[j].ColumnName, myFont));
                    }
                    //table body
                    for (int i = 0; i < datatable.Rows.Count; i++)
                    {
                        for (int j = 0; j < datatable.Columns.Count; j++)
                        {
                            table.AddCell(new Phrase(datatable.Rows[i][j].ToString(), myFont));
                        }
                    }
                    document.Add(table);
                    document.Close();
                }
            }
            catch (DocumentException de)
            {
            }
        }
    }
}
