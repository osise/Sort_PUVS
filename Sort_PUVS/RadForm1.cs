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
            nameFolder = @"C:\SPU\" + excelFilePath + "\\";
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

        public void OpenExcel()
        {
            try
            {
                myExcelApplication = null;

                myExcelApplication = new Excel.Application
                {
                    DisplayAlerts = false // turn off alerts
                }; // create Excell App

                myExcelWorkbook = myExcelApplication.Workbooks._Open(ExcelFilePath, System.Reflection.Missing.Value,
                   System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                   System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                   System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                   System.Reflection.Missing.Value, System.Reflection.Missing.Value); // open the existing excel file

                int numberOfWorkbooks = myExcelApplication.Workbooks.Count; // get number of workbooks (optional)

                myExcelWorkSheet = (Excel.Worksheet)myExcelWorkbook.Worksheets[1]; // define in which worksheet, do you want to add data
                myExcelWorkSheet.Name = "Лист 1"; // define a name for the worksheet (optinal)

                int numberOfSheets = myExcelWorkbook.Worksheets.Count; // get number of worksheets (optional)
 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть файл. Проверьте, возможно он уже открыт или поврежден");
                sb.Append(DateTime.Now + ": Не удалось открыть файл. Проверьте, возможно он уже открыт или поврежден\r\n" + ex);
            }

        }

        public void CloseExcel() //Остаются открытыми файлы после работы
        {
            try
            {
                try
                {
                    myExcelWorkbook.SaveAs(ExcelFilePath, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                               System.Reflection.Missing.Value, System.Reflection.Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange,
                                               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                               System.Reflection.Missing.Value, System.Reflection.Missing.Value); // Save data in excel


                    myExcelWorkbook.Close(true, ExcelFilePath, System.Reflection.Missing.Value); // close the worksheet
                }
                catch (Exception ex )
                {
                    MessageBox.Show("Не удалось закрыть исходный файл.\n Проверьте, возможно он уже закрыт или поврежден");
                    sb.Append(DateTime.Now + ": Не удалось закрыть исходный файл. Проверьте, возможно он уже открыт или поврежден\r\n" + ex);
                    //throw;
                }
                
            }
            finally
            {
                myExcelApplication.Quit(); // close the excel application
                GC.Collect();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcelWorkSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcelWorkbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcelApplication);
            }

        }

        public DataTable GetTableDataFromXl(string XlFile)
        {
            dt.Clear(); //Очищаем dt чтобы не суммировалось от нескольких файлов 
            try
            {
                string Ext = Path.GetExtension(XlFile);
                string connectionString = "";
                if (Ext == ".xls")
                {   //For Excel 97-03
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + XlFile + "; Extended Properties = 'Excel 8.0;HDR=YES'";
                }
                else if (Ext == ".xlsx")
                {    //For Excel 07 and greater
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source =" + XlFile + "; Extended Properties = 'Excel 8.0;HDR=YES'";
                }
                OleDbConnection conn = new OleDbConnection(connectionString);
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter();

                cmd.Connection = conn;
                //Fetch 1st Sheet Name
                conn.Open();
                DataTable dtSchema;
                dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string ExcelSheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();
                conn.Close();
                //Read all data of fetched Sheet to a Data Table
                conn.Open();
                cmd.CommandText = "SELECT * From [" + ExcelSheetName + "]";
                dataAdapter.SelectCommand = cmd;
                dataAdapter.Fill(dt);
                conn.Close();
               
            }
            catch (Exception ex)
            { 
                MessageBox.Show("Не удалось прочесть файл\n" + ex);
                sb.Append(DateTime.Now + ": Не удалось прочесть файл\r\n Проверьте, возможно он уже открыт или поврежден\r\n" + ex);
            }

            return dt;
        }
       
        public void radButton2_Click(object sender, EventArgs e)
        {
            sb.Append("\r\n");
            sb.Append("\r\n");
            sb.Append("------------------------ " + DateTime.Now + " ------------------------\r\n");
            
            OpenFileDialog fbd = new OpenFileDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                radRichTextEditor1.Text = fbd.FileName;
                ExcelFilePath = fbd.FileName;
                radRichTextEditor1.Text += "Выбран файл: " + fbd.FileName + "\n";
                sb.Append(DateTime.Now + ": Выбран файл: " + fbd.FileName + "\r\n");

                OpenExcel();
                radRichTextEditor1.Text += "Файл успешно открыт\n";
                sb.Append(DateTime.Now + ": Файл успешно открыт\r\n");
                radRichTextEditor1.Text += "Обработка файла, подождите...\n";

                GetTableDataFromXl(fbd.FileName);
                cou = dt.Rows.Count;
                
                CloseExcel();
                radRichTextEditor1.Text += "Обнаружено записей в файле: " + dt.Rows.Count + "\n";
                sb.Append(DateTime.Now + ": Обнаружено записей в файле: " + dt.Rows.Count + "\r\n");

                UniqueEx();
                radRichTextEditor1.Text += "Обнаружено номеров страхователей в файле: " + dt_copy.Rows.Count + "\n";
                sb.Append(DateTime.Now + ": Обнаружено номеров страхователей в файле: " + dt_copy.Rows.Count + "\r\n");
                radRichTextEditor1.Text += "Нажмите кнопку Начать\n";

            }
            File.AppendAllText(@"C:\log.txt", sb.ToString());
            sb.Clear();
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", @"C:\log.txt");
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
                File.AppendAllText(@"C:\log.txt", sb.ToString());
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
            File.AppendAllText(@"C:\log.txt", sb.ToString());
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

