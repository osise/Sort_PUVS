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

namespace Sort_PUVS
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        private BackgroundWorker bw = new BackgroundWorker();
        int cou = 0;
        public DataTable dt = new DataTable();
        public DataTable dt_copy = new DataTable();
        public DataTable finddata = new DataTable();

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
            catch (Exception)
            {
                MessageBox.Show("Не удалось выделить уникальные значения");
                throw;
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
            catch (Exception)
            {
                MessageBox.Show("Не удалось записать файлы");
              //  throw;
            }
        }

        public void ExportToExcel(DataTable tbl, string excelFilePath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            FileInfo fi1 = new FileInfo(@"C:\SPU\"+ excelFilePath + ".xls");
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("SPU");
               
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
            catch (Exception)
            {
                MessageBox.Show("Не удалось открыть файл. Проверьте, возможно он уже открыт или поврежден");
                //throw;
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
                catch (Exception)
                {
                    MessageBox.Show("Не удалось файл.");
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
            }

            return dt;
        }

        public void radButton2_Click(object sender, EventArgs e)
        {

            OpenFileDialog fbd = new OpenFileDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                radRichTextEditor1.Text = fbd.FileName;
                ExcelFilePath = fbd.FileName;
                radRichTextEditor1.Text += "Выбран файл: " + fbd.FileName + "\n";

                OpenExcel();
                radRichTextEditor1.Text += "Файл успешно открыт\n";
                GetTableDataFromXl(fbd.FileName);
                cou = dt.Rows.Count;
                radRichTextEditor1.Text += "Обнаружено " + dt.Rows.Count + " записей в файле" + "\n";
                CloseExcel();
                UniqueEx();
                radRichTextEditor1.Text += "Обнаружено " + dt_copy.Rows.Count + " записей страхователей в файле" + "\n";
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            cou = 0;
            finddata = dt.Clone();
            for (int y = 0; y < dt_copy.Rows.Count; y++)
            {
                FindEx(finddata, y);
                int percentage = (y + 1) * 100 / dt_copy.Rows.Count;
            }
            finddata.Dispose();
           
        }

        private void radButton3_Click(object sender, EventArgs e)
        {

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
            }

            finddata.Dispose();
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                progressBar1.Text = "Отменено!";
            }

            else if (!(e.Error == null))
            {
                progressBar1.Text = ("Ошибка: " + e.Error.Message);
            }

            else
            {
                progressBar1.Text = "Выполнено!";
            }

            radRichTextEditor1.Text += "Обработано " + cou + " записей страхователей в файле" + "\n";
            radRichTextEditor1.Text += "Создано " + dt_copy.Rows.Count + " каталогов" + "\n";
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value1 = e.ProgressPercentage;
            progressBar1.Text = (e.ProgressPercentage.ToString() + "%");
        }
    }
}

