﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;

namespace Rld.Acs.WpfApplication.Service.ExportFile
{
    public class TextExportingService
    {
        public static void ExportDataTable(DataTable datatable, string fileName, string splitChar)
        {
            try
            {
                using (var file = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write))
                {
                    var streamWriter = new StreamWriter(file, new System.Text.UnicodeEncoding());

                    //table header
                    for (int j = 0; j < datatable.Columns.Count; j++)
                    {
                        streamWriter.Write(datatable.Columns[j].ColumnName);
                        streamWriter.Write(splitChar);
                    }
                    streamWriter.WriteLine("");

                    //table body
                    for (int i = 0; i < datatable.Rows.Count; i++)
                    {
                        for (int j = 0; j < datatable.Columns.Count; j++)
                        {
                            streamWriter.Write(datatable.Rows[i][j].ToString());
                            streamWriter.Write(splitChar);
                        }
                        streamWriter.WriteLine("");
                    }
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception de)
            {
            }
        }
    }
}
