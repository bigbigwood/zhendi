using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Rld.Acs.WpfApplication.Service.ExportFile
{
    public class PdfService
    {
        public static void ExportDataTable(DataTable datatable, string fileName)
        {
            try
            {

                Document document = new Document();
                using (FileStream file = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write))
                {
                    PdfWriter.GetInstance(document, file);
                    document.Open();

                    BaseFont bfChinese = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font fontChinese = new Font(bfChinese, 12);

                    //document.Add(new Paragraph(this.TextBox1.Text.ToString(), fontChinese));

                    //iTextSharp.text.Image jpeg = iTextSharp.text.Image.GetInstance(Server.MapPath("xxx.jpg"));

                    //document.Add(jpeg);

                    PdfPTable table = new PdfPTable(datatable.Columns.Count);
                    for (int i = 0; i < datatable.Rows.Count; i++)
                    {
                        for (int j = 0; j < datatable.Columns.Count; j++)
                        {
                            table.AddCell(new Phrase(datatable.Rows[i][j].ToString(), fontChinese));
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
