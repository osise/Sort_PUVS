using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Data.OleDb;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using System.Diagnostics;
using ExcelDataReader;

namespace Sort_PUVS
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        private BackgroundWorker bw = new BackgroundWorker();
        int cou = 0;
        int cat = 0;
        public DataTable dt = new DataTable();
        public DataTable dt_copy = new DataTable();
        public DataTable finddata = new DataTable();
        StringBuilder sb = new StringBuilder();

        Excel.Application myExcelApplication;
        Excel.Workbook myExcelWorkbook;
        Excel.Worksheet myExcelWorkSheet;

        public RadForm1()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

        }

        public string ExcelFilePath { get; set; } = string.Empty;

        public void UniqueEx()
        {
            
            try
            {
                dt_copy = dt.Copy();
                dt_copy = dt_copy.DefaultView.ToTable(true, dt_copy.Columns[0].ColumnName); //distinct values from column 0
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось выделить уникальные значения");
                sb.Append(DateTime.Now + ": Не удалось выделить уникальные значения\r\n" + ex);
                
            }

        }

        public void FindEx(DataTable data, int y)
        {
            try
            {
                finddata.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i][0]) == Convert.ToString(dt_copy.Rows[y][0])) // откуда - куда
                    {
                        finddata.ImportRow(dt.Rows[i]);
                        dt.Rows.RemoveAt(i); //гениально!!!!
                        i--;
                    }
                   
                }
                cou += finddata.Rows.Count;
                ExportToExcel(finddata, Convert.ToString(dt_copy.Rows[y][0]));   
            }
            catch (Exception ex)
            {

                MessageBox.Show("Не удалось записать файлы");
                sb.Append(DateTime.Now + ": Не удалось записать файлы\r\n" + ex);
            }
        }

        public static string InsertStrings(string text, string insertString, params int[] rangeLengths)
        {
            var sb = new StringBuilder(text);
            var indexes = new int[rangeLengths.Length];
            for (int i = 0; i < indexes.Length; i++)
                indexes[i] = rangeLengths[i] + indexes.ElementAtOrDefault(i - 1) + insertString.Length;

            for (int i = 0; i < indexes.Length; i++)
            {
                if (indexes[i] < sb.Length)
                    sb.Insert(indexes[i], insertString);
            }

            return sb.ToString();
        }

        public void ExportToExcel(DataTable tbl, string excelFilePath)
        {
            sb.Append("\r\n");
            sb.Append(DateTime.Now + ": Обработка файла\r\n");
            if (excelFilePath.Length == 11)
            {
                sb.Append(DateTime.Now + ": Преобразовываем номер\r\n");
                sb.Append(DateTime.Now + ": " + excelFilePath + " -> ");
                excelFilePath = "0" + excelFilePath;
                excelFilePath = InsertStrings(excelFilePath, "-", 2, 3);
                sb.Append(excelFilePath + "\r\n");
            }
            else if (excelFilePath.Length == 12)
            {
                sb.Append(DateTime.Now + ": Преобразовываем номер\r\n");
                sb.Append(DateTime.Now + ": " + excelFilePath + "-> ");
                excelFilePath = InsertStrings(excelFilePath, "-", 2, 3);
                sb.Append(excelFilePath + "\r\n");
            }
            else
            {

            }
            string nameFolder = "";
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            sb.Append(DateTime.Now + ": Создаем каталог " + excelFilePath + "\r\n");
            nameFolder = @"C:\Sort-PUVS\" + excelFilePath + "\\";
            cat ++;


            if (Directory.Exists(nameFolder))
            {
                sb.Append(DateTime.Now + ": Каталог " + excelFilePath + " существует!\r\n");
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(nameFolder);
            }
            FileInfo fi1 = new FileInfo(nameFolder + excelFilePath + ".xlsx");
            sb.Append(DateTime.Now + ": Создан файл в " + fi1 + "\r\n");
            sb.Append(DateTime.Now + ": Скопировано строк :" + finddata.Rows.Count + "\r\n");
            
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Сорт-ПУВС");
               
                ws.Cells["A1"].LoadFromDataTable(tbl, true);
                
                pck.SaveAs(fi1);
                GC.Collect();
                ws.Dispose();
                pck.Dispose();   
            }
        }

        public DataTable GetTableDataFromXl(string path, bool hasHeader = true)
        {
            dt.Clear();

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // var result = reader.AsDataSet();
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        // Gets or sets a value indicating whether to set the DataColumn.DataType 
                        // property in a second pass.
                        UseColumnDataType = true,

                        // Gets or sets a callback to determine whether to include the current sheet
                        // in the DataSet. Called once per sheet before ConfigureDataTable.
                        FilterSheet = (tableReader, sheetIndex) => true,

                        // Gets or sets a callback to obtain configuration options for a DataTable. 
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            // Gets or sets a value indicating the prefix of generated column names.
                           // EmptyColumnNamePrefix = "Column",

                            // Gets or sets a value indicating whether to use a row from the 
                            // data as column names.
                            UseHeaderRow = true,

                            // Gets or sets a callback to determine which row is the header row. 
                            // Only called when UseHeaderRow = true.
                           /* ReadHeaderRow = (rowReader) => {
                                // F.ex skip the first row and use the 2nd row as column headers:
                                rowReader.Read();
                            },*/

                            // Gets or sets a callback to determine whether to include the 
                            // current row in the DataTable.
                            FilterRow = (rowReader) => {
                                return true;
                            },

                            // Gets or sets a callback to determine whether to include the specific
                            // column in the DataTable. Called once per column after reading the 
                            // headers.
                            FilterColumn = (rowReader, columnIndex) => {
                                return true;
                            }
                        }
                    });



                    // The result of each spreadsheet is in result.Tables

                    dt = result.Tables[0];
                    
                }
            }
           
            //  ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            /*  using (var pck = new OfficeOpenXml.ExcelPackage())
              {
                  using (var stream = File.OpenRead(path))
                  {
                      pck.Load(stream);
                  }
                  var ws = pck.Workbook.Worksheets.First();
                 // DataTable tbl = new DataTable();
                  foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                  {
                      dt.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                  }
                  var startRow = hasHeader ? 2 : 1;
                  for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                  {
                      var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                      DataRow row = dt.Rows.Add();
                      foreach (var cell in wsRow)
                      {
                          row[cell.Start.Column - 1] = cell.Text;
                      }
                  }

              }*/
            return dt;
        }

        public void radButton2_Click(object sender, EventArgs e)
        {
            string strlen = "";
            if (Directory.Exists(@"C:\Sort-PUVS\"))
            {

            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(@"C:\Sort-PUVS\");
            }
            sb.Append("\r\n");
            sb.Append("\r\n");
            sb.Append("------------------------ " + DateTime.Now + " ------------------------\r\n");
            
            OpenFileDialog fbd = new OpenFileDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ExcelFilePath = fbd.FileName;
                radRichTextEditor1.Text += "Выбран файл: " + fbd.FileName + "\n";
                sb.Append(DateTime.Now + ": Выбран файл: " + fbd.FileName + "\r\n");
                string Ext1 = Path.GetExtension(ExcelFilePath);
                if (Ext1 == ".xls" || Ext1 == ".xlsx")
                {
                    radRichTextEditor1.Text += "Файл успешно открыт\n";
                    sb.Append(DateTime.Now + ": Файл успешно открыт\r\n");
                    radRichTextEditor1.Text += "Обработка файла, подождите...\n";

                    GetTableDataFromXl(fbd.FileName);
                    cou = dt.Rows.Count;
                    strlen = dt.Rows[1].ItemArray[0].ToString();
                    if (strlen.Length == 11 || strlen.Length == 12 || strlen.Length == 14)
                    {
                        radRichTextEditor1.Text += "Обнаружено записей в файле: " + dt.Rows.Count + "\n";
                        sb.Append(DateTime.Now + ": Обнаружено записей в файле: " + dt.Rows.Count + "\r\n");

                        UniqueEx();
                        radRichTextEditor1.Text += "Обнаружено номеров страхователей в файле: " + dt_copy.Rows.Count + "\n";
                        sb.Append(DateTime.Now + ": Обнаружено номеров страхователей в файле: " + dt_copy.Rows.Count + "\r\n");
                        radRichTextEditor1.Text += "Нажмите кнопку Начать\n";
                    }  
                    else
                    {
                        radRichTextEditor1.Text += "Не удалось обработать строку. Данные в первом столбце - не регномер или его формат неверен!" + "\n";
                        sb.Append(DateTime.Now + ": Не удалось обработать строку. Данные в первом столбце - не регномер или его формат неверен!\r\n");
                    }

            }
            else
            {
                radRichTextEditor1.Text += "Не удалось открыть файл. Это не файл MS Excel!" + "\n";
                sb.Append(DateTime.Now + ": Не удалось открыть файл.Это не файл MS Excel!\r\n");
            }
               

            }
            File.AppendAllText(@"C:\Sort-PUVS\log.txt", sb.ToString());
            sb.Clear();
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", @"C:\Sort-PUVS\log.txt");
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            cou = 0;
            cat = 0;
            finddata = dt.Clone();
            for (int y = 0; y < dt_copy.Rows.Count; y++)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    int percentage = (y + 1) * 100 / dt_copy.Rows.Count;
                    FindEx(finddata, y);
                    worker.ReportProgress(percentage);
                }
                File.AppendAllText(@"C:\Sort-PUVS\log.txt", sb.ToString());
                sb.Clear();
            }


            finddata.Dispose();
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                progressBar1.Text = "Отменено!";
                radRichTextEditor1.Text += "Отменено!\n";
                sb.Append("\r\n");
                sb.Append(DateTime.Now + ": Отменено!\r\n");
            }

            else if (!(e.Error == null))
            {
                progressBar1.Text = ("Ошибка: " + e.Error.Message);
                radRichTextEditor1.Text += "Ошибка: " + e.Error.Message + "\n";
                sb.Append("\r\n");
                sb.Append(DateTime.Now + ": Ошибка: " + e.Error.Message + "\r\n");
            }

            else
            {
                progressBar1.Text = "Выполнено!";
                radRichTextEditor1.Text += "Выполнено!\n";
                sb.Append("\r\n");
                sb.Append(DateTime.Now + ": Выполнено!\r\n");
            }

            radRichTextEditor1.Text += "Обработано записей страхователей в файле: " +cou + "\n";
            sb.Append(DateTime.Now + ": Обработано записей страхователей в файле: " +cou + "\r\n");
            radRichTextEditor1.Text += "Создано каталогов :" + cat + "\n";
            sb.Append(DateTime.Now + ": Создано каталогов :" + cat + "\r\n");
            File.AppendAllText(@"C:\Sort-PUVS\log.txt", sb.ToString());
            sb.Clear();
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value1 = e.ProgressPercentage;
            progressBar1.Text = (e.ProgressPercentage.ToString() + "%");
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

