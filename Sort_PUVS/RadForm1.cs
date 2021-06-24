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

namespace Sort_PUVS
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        CancellationTokenSource cts; // Источник токена отмены
        DataSet ds = new DataSet();
        ExcelFile excel = new ExcelFile();

        string XlFile = null;
        string XlFile1 = null;

        public RadForm1()
        {
            InitializeComponent();
          
        }


        private async void startButton_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            startButton.Enabled = false;
            cancelButton.Enabled = true;
            progressBar1.Value1 = 0;
            var progress = new Progress<int>(ProgressHandler);
            try
            {
                await WorkAsync(cts.Token, progress);
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show("Операция прервана.", "Внимание.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            startButton.Enabled = true;
            cancelButton.Enabled = false;
        }

        private void ProgressHandler(int number)
        {
            progressBar1.Value1 = number;
        }

        void texty(int i)
        {
            radRichTextEditor1.Text = "загружено " + i + " процентов";
        }
        private async Task WorkAsync(CancellationToken token, IProgress<int> progress)
        {
            for (int i = 1; i <= 100; i++)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100);
                progress?.Report(i);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }

        public void radButton2_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog fbd = new OpenFileDialog();
           // FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                radRichTextEditor1.Text = fbd.FileName; 
                excel.ExcelFilePath = fbd.FileName;
                excel.OpenExcel();
                excel.GetTableDataFromXl(fbd.FileName);
                excel.CloseExcel();
                excel.UniqueEx();
                int length = excel.dt.Rows.Count;
                int length1 = excel.dt_copy.Rows.Count;
                radRichTextEditor1.Text = Convert.ToString(length);
                radRichTextEditor1.Text = Convert.ToString(length1);
                excel.FindEx();
              //  excel.CloseExcel();
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
           
        }
    }
    public class ExcelFile
    {
        public DataTable dt = new DataTable();
        public DataTable dt_copy = new DataTable();


        Excel.Application myExcelApplication;
        Excel.Workbook myExcelWorkbook;
        Excel.Worksheet myExcelWorkSheet;

        public string ExcelFilePath { get; set; } = string.Empty;

        public int Rownumber { get; set; } = 1;

        public void UniqueEx()
        {
            dt_copy = dt.Copy();
            string colname = dt_copy.Columns[0].ColumnName;
            dt_copy = dt_copy.DefaultView.ToTable(true, dt_copy.Columns[0].ColumnName); //distinct values from column 0
        }

        public void FindEx()
        {
            DataTable finddata = new DataTable();
            finddata = dt.Clone();
            
            for (int y = 0; y < dt_copy.Rows.Count; y++)
            {
                finddata.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // progressBar1.Value = progressBar1.Maximum - 1;

                    if (Convert.ToString(dt.Rows[i][0]) == Convert.ToString(dt_copy.Rows[y][0])) // откуда - куда
                    { //MessageBox.Show("Found");
                        finddata.ImportRow(dt.Rows[i]);
                       // excelFile.AddDataToExcel("есть", 8, y + 3); //куда пишем
                    }
                    //label15.Text = Convert.ToString(i);
                }
                ExportToExcel(finddata, @"C:\"+ Convert.ToString(dt_copy.Rows[y][0]) + " .xls");             
                // CreateEx(finddata, Convert.ToString(dt_copy.Rows[y][0]));
                //   label14.Text = Convert.ToString(y);

            }
           // excelFile.CloseExcel();
            MessageBox.Show("Found");
            finddata.Dispose();
            
        }

        public void CreateEx(DataTable data, string reg)
        {
            //string dataInd = DateTime.Now.ToString("dd.MM.yyyy HH mm ss");//дата время запуска программы
            string pathExe = Application.StartupPath.ToString() + "\\";//путь к файлу exe
            string nameFolder = "";
            string sni = string.Empty;
            //System.Text.StringBuilder sni = new System.Text.StringBuilder();

            Excel.Application excel;
            Excel.Workbook wbInputExcel, wbResultExcel;//книги Excel
            Excel.Worksheet wshInputExcel, wshResultExcel;//листы Excel
            //создаем папку в которой будем сохранять все промежуточные файлы и результат
            nameFolder = "Выгрузка_" + reg + "\\";

            if (Directory.Exists(pathExe + nameFolder))
            {
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(pathExe + nameFolder);
            }
            excel = new Excel.Application();

            //создание сводного файла Excel, в который будут объединяться все файлы
            wbResultExcel = excel.Workbooks.Add(System.Reflection.Missing.Value);
            wshResultExcel = wbResultExcel.Sheets[1];
            wbResultExcel.Worksheets.Add(data);
           // excel.Worksheets.Add(data, "1");
            wshResultExcel.Name = "Лист1";
            string nameFile = reg + " .xlsx";
            wbResultExcel.SaveAs(pathExe + nameFolder + nameFile);

            excel.DisplayAlerts = false;
            
        }

        public void ExportToExcel(DataTable tbl, string excelFilePath = null)
        {
            try
            {
                if (tbl == null || tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook
                var excelApp = new Excel.Application();
                excelApp.Workbooks.Add();

                // single worksheet
                Excel._Worksheet workSheet = excelApp.ActiveSheet;

                // column headings
                for (var i = 0; i < tbl.Columns.Count; i++)
                {
                    workSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
                }

                // rows
                for (var i = 0; i < tbl.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (var j = 0; j < tbl.Columns.Count; j++)
                    {
                        workSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
                    }
                }

                // check file path
                if (!string.IsNullOrEmpty(excelFilePath))
                {
                    try
                    {
                        workSheet.SaveAs(excelFilePath);
                        excelApp.Quit();
                        MessageBox.Show("Excel file saved!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                                            + ex.Message);
                    }
                }
                else
                { // no file path is given
                    excelApp.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

        public void OpenExcel()
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

            MessageBox.Show("Success");
        }

        public void AddDataToExcel(string index, int stolb, int row)
        {
            myExcelWorkSheet.Cells[row, stolb] = index;
        }

        public void CloseExcel()
        {
            try
            {
                myExcelWorkbook.SaveAs(ExcelFilePath, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                               System.Reflection.Missing.Value, System.Reflection.Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange,
                                               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                               System.Reflection.Missing.Value, System.Reflection.Missing.Value); // Save data in excel


                myExcelWorkbook.Close(true, ExcelFilePath, System.Reflection.Missing.Value); // close the worksheet
            }
            finally
            {
                //  if (myExcelApplication != null)
                // {

                myExcelApplication.Quit(); // close the excel application
                GC.Collect();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcelWorkSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcelWorkbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcelApplication);
                // }
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
            { MessageBox.Show(Convert.ToString(ex)); }

            return dt;
        }

        private void LoadData()
        {
            ExcelFile excelFile = new ExcelFile();
            try
            {
                //string XlFile1 = null;
                //string XlFile = null;
                DataTable dtInd = GetTableDataFromXl(ExcelFilePath); // Converting Excel Data into DataTable 

                DataTable dtPens = GetTableDataFromXl(ExcelFilePath);
               // excelFile.ExcelFilePath = XlFile1;
                excelFile.OpenExcel();
                MessageBox.Show("1");

                // progressBar1.Maximum = dtPens.Rows.Count * dtInd.Rows.Count;


                for (int y = 0; y < dtPens.Rows.Count; y++)
                {
                    for (int i = 0; i < dtInd.Rows.Count; i++)
                    {
                        // progressBar1.Value = progressBar1.Maximum - 1;

                        if (Convert.ToString(dtInd.Rows[i][1]) == Convert.ToString(dtPens.Rows[y][6])) // откуда - куда
                        { //MessageBox.Show("Found");
                            excelFile.AddDataToExcel("есть", 8, y + 3); //куда пишем
                        }
                        //label15.Text = Convert.ToString(i);
                    }
                    //   label14.Text = Convert.ToString(y);

                }
                excelFile.CloseExcel();
                MessageBox.Show("Found");
            }
            //(dtPens.Rows[6] in dtInd.Rows[1])


            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }
    }
}

