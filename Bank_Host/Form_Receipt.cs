using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Threading;
using MICube.SmartDriver.Base.TCP;
using TCPConfig = MICube.SmartDriver.Base.TCP.Config;
using Microsoft.Office.Interop.Excel;

namespace Bank_Host
{
    public partial class Form_InBill : Form
    {
        System.Windows.Forms.Label title1 = new System.Windows.Forms.Label { Text = "SCRAP MAT'L 입고증 / K4 BANK", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold) };
        System.Windows.Forms.Label lCust = new System.Windows.Forms.Label { Text = "CUSTOMER", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.Label ldate = new System.Windows.Forms.Label { Text = "DATE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.ComboBox tb_BCustName = new System.Windows.Forms.ComboBox { Text = "tb_BCustName", Dock = DockStyle.Fill, Margin = Padding.Empty };
        System.Windows.Forms.ComboBox tb_BCustCode = new System.Windows.Forms.ComboBox { Text = "tb_BCustCode", Dock = DockStyle.Fill, Margin = Padding.Empty };
        System.Windows.Forms.TextBox tb_BLineCode = new System.Windows.Forms.TextBox { Text = "AJ45400", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        DateTimePicker dtB = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd", Dock = DockStyle.Fill, Margin = Padding.Empty };
        System.Windows.Forms.TextBox tb_BTTL = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox tb_BGross = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox tb_BRequest = new System.Windows.Forms.TextBox { Text = "tb_BRequest", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox tb_BQTY = new System.Windows.Forms.TextBox { Text = "tb_BQTY", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox tb_BWeight = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.Label lBRecipient = new System.Windows.Forms.Label { Text = "인  수  자", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.Label lBSender = new System.Windows.Forms.Label { Text = "입  고  자", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.TextBox tb_BRecipient = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox tb_BSender = new System.Windows.Forms.TextBox { Text = string.Format("{0}({1})", BankHost_main.strOperator, BankHost_main.strID), Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox tb_BSpec = new System.Windows.Forms.TextBox { Text = "SPEC NO : 001-2698", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };

        System.Windows.Forms.Label lStore = new System.Windows.Forms.Label { Text = "SCRAP MAT'L 입고증 / K4 STORE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold) };
        System.Windows.Forms.Label lSCust = new System.Windows.Forms.Label { Text = "CUSTOMER", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.Label lSdate = new System.Windows.Forms.Label { Text = "DATE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.TextBox tb_SCustName = new System.Windows.Forms.TextBox { Text = "tb_BCustName", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };
        System.Windows.Forms.TextBox tb_SCustCode = new System.Windows.Forms.TextBox { Text = "tb_BCustCode", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };
        System.Windows.Forms.TextBox tb_SLineCode = new System.Windows.Forms.TextBox { Text = "AJ45400", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };
        DateTimePicker dtS = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd", Dock = DockStyle.Fill, Margin = Padding.Empty,Enabled = false };
        System.Windows.Forms.TextBox tb_STTL = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };
        System.Windows.Forms.TextBox tb_SGross = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };
        System.Windows.Forms.TextBox tb_SRequest = new System.Windows.Forms.TextBox { Text = "tb_BRequest", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };
        System.Windows.Forms.TextBox tb_SQTY = new System.Windows.Forms.TextBox { Text = "tb_BQTY", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };
        System.Windows.Forms.TextBox tb_SWeight = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };
        System.Windows.Forms.Label lSRecipient = new System.Windows.Forms.Label { Text = "인  수  자", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.Label lSSender = new System.Windows.Forms.Label { Text = "입  고  자", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.TextBox tb_SRecipient = new System.Windows.Forms.TextBox { Text = "" , Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };
        System.Windows.Forms.TextBox tb_SSender = new System.Windows.Forms.TextBox { Text = string.Format("{0}({1})", BankHost_main.strOperator, BankHost_main.strID), Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };
        System.Windows.Forms.TextBox tb_SSpec = new System.Windows.Forms.TextBox { Text = "SPEC NO : 001-2698", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty, ReadOnly = true };

        List<string> sCustCode = new List<string>();
        List<string> sCustName = new List<string>();
        string sTTL = "";
        string sWT = "";
        string sRequest = "";
        string sQTY = "";
        string sWeight = "";



        public Form_InBill()
        {
            InitializeComponent();
        }

        public Form_InBill(List<string> CustCode , List<string> CustName, string TTL, string WT, string Request, string QTY, string Weight)
        {
            sCustCode = CustCode;
            sCustName = CustName;
            sTTL = TTL;
            sWT = WT;
            sRequest = Request;
            sQTY = QTY;
            sWeight = Weight;

            InitializeComponent();
        }


        private void Form_InBill_Load(object sender, EventArgs e)
        {
            TableLayout();
            TableEvent();

            FillText();
        }

        private void FillText()
        {
            for(int i = 0; i < sCustCode.Count; i++)
            {
                tb_BCustCode.Items.Add(sCustCode[i]);                
            }

            for(int i = 0; i < sCustName.Count; i++)
            {
                tb_BCustName.Items.Add(sCustName[i]);
            }

            tb_BCustCode.Text = sCustCode[0];
            tb_BCustName.Text = sCustName[0];
            tb_BTTL.Text = "";
            tb_BGross.Text = "";
            tb_BRequest.Text = sRequest;
            tb_BQTY.Text = sQTY;
            tb_BWeight.Text = "";
            //tb_BRecipient.Text = BankHost_main.strOperator;
        }

        private void TableEvent()
        {
            tb_BCustName.TextChanged += Tb_BCustName_TextChanged;
            tb_BCustName.SelectedIndexChanged += Tb_BCustName_SelectedIndexChanged;
            tb_BCustCode.TextChanged += Tb_BCustCode_TextChanged;
            tb_BCustCode.SelectedIndexChanged += Tb_BCustCode_SelectedIndexChanged;
            tb_BLineCode.TextChanged += Tb_BLineCode_TextChanged;
            dtB.TextChanged += DtB_TextChanged;
            tb_BTTL.TextChanged += Tb_BTTL_TextChanged;
            tb_BGross.TextChanged += Tb_BGross_TextChanged;
            tb_BRequest.TextChanged += Tb_BRequest_TextChanged;
            tb_BQTY.TextChanged += Tb_BQTY_TextChanged;
            tb_BWeight.TextChanged += Tb_BWeight_TextChanged;
            tb_BRecipient.TextChanged += Tb_BRecipient_TextChanged;
            tb_BSender.TextChanged += Tb_BSender_TextChanged;
            tb_BSpec.TextChanged += Tb_BSpec_TextChanged;
        }

        private void Tb_BCustCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            tb_BCustName.SelectedIndex = tb_BCustCode.SelectedIndex;
        }

        private void Tb_BCustName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Tb_BSpec_TextChanged(object sender, EventArgs e)
        {
            tb_SSpec.Text = tb_BSpec.Text;
        }

        private void Tb_BSender_TextChanged(object sender, EventArgs e)
        {
            tb_SSender.Text = tb_BSender.Text;
        }

        private void Tb_BRecipient_TextChanged(object sender, EventArgs e)
        {
            tb_SRecipient.Text = tb_BRecipient.Text;
        }

        private void Tb_BWeight_TextChanged(object sender, EventArgs e)
        {
            tb_SWeight.Text = tb_BWeight.Text;
        }

        private void Tb_BQTY_TextChanged(object sender, EventArgs e)
        {
            tb_SQTY.Text = tb_BQTY.Text;
        }

        private void Tb_BRequest_TextChanged(object sender, EventArgs e)
        {
            tb_SRequest.Text = tb_BRequest.Text;
        }

        private void Tb_BGross_TextChanged(object sender, EventArgs e)
        {
            tb_SGross.Text = tb_BGross.Text;
        }

        private void Tb_BTTL_TextChanged(object sender, EventArgs e)
        {
            tb_STTL.Text = tb_BTTL.Text;
        }

        private void DtB_TextChanged(object sender, EventArgs e)
        {
            dtS.Text = dtB.Text;
        }

        private void Tb_BLineCode_TextChanged(object sender, EventArgs e)
        {
            tb_SLineCode.Text = tb_BLineCode.Text;
        }

        private void Tb_BCustCode_TextChanged(object sender, EventArgs e)
        {
            tb_SCustCode.Text = tb_BCustCode.Text;
        }

        private void Tb_BCustName_TextChanged(object sender, EventArgs e)
        {
            tb_SCustName.Text = tb_BCustName.Text;
        }

        private void TableLayout()
        {
            tp.Controls.Add(title1, 0, 0);
            tp.SetColumnSpan(title1, 5);

            tp.Controls.Add(lCust, 0, 1);
            tp.SetColumnSpan(lCust, 2);

            tp.Controls.Add(new System.Windows.Forms.Label { Text = "LINE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, 1);

            tp.Controls.Add(ldate, 3, 1);
            tp.SetColumnSpan(ldate, 2);

            tp.Controls.Add(tb_BCustName, 0, 2);
            tp.Controls.Add(tb_BCustCode, 1, 2);
            tp.Controls.Add(tb_BLineCode, 2, 2);

            tp.Controls.Add(dtB, 3, 2);
            tp.SetColumnSpan(dtB, 2);

            tp.Controls.Add(new System.Windows.Forms.Label { Text = "TTL C/T", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 0, 3);
            tp.Controls.Add(new System.Windows.Forms.Label { Text = "Gross W/T", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 1, 3);
            tp.Controls.Add(new System.Windows.Forms.Label { Text = "Request#", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, 3);
            tp.Controls.Add(new System.Windows.Forms.Label { Text = "Lot Qty", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 3, 3);
            tp.Controls.Add(new System.Windows.Forms.Label { Text = "Net WEIGHT", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 4, 3);

            tp.Controls.Add(tb_BTTL, 0, 4);
            tp.Controls.Add(tb_BGross, 1, 4);
            tp.Controls.Add(tb_BRequest, 2, 4);
            tp.Controls.Add(tb_BQTY, 3, 4);
            tp.Controls.Add(tb_BWeight, 4, 4);

            tp.Controls.Add(lBSender, 0, 5);
            tp.Controls.Add(lBRecipient, 3, 5);

            tp.SetColumnSpan(lBSender, 3);
            tp.SetColumnSpan(lBRecipient, 2);

            tp.Controls.Add(tb_BSender, 0, 6);
            tp.Controls.Add(tb_BRecipient, 3, 6);

            tp.SetColumnSpan(tb_BSender, 3);
            tp.SetColumnSpan(tb_BRecipient, 2);

            tp.Controls.Add(tb_BSpec, 3, 7);
            tp.SetColumnSpan(tb_BSpec, 2);

            tp.Controls.Add(lStore, 0, 8);
            tp.SetColumnSpan(lStore, 5);


            tp.Controls.Add(lSCust, 0, 9);
            tp.SetColumnSpan(lSCust, 2);

            tp.Controls.Add(new System.Windows.Forms.Label { Text = "LINE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, 9);

            tp.Controls.Add(lSdate, 3, 9);
            tp.SetColumnSpan(lSdate, 2);

            tp.Controls.Add(tb_SCustName, 0, 10);
            tp.Controls.Add(tb_SCustCode, 1, 10);
            tp.Controls.Add(tb_SLineCode, 2, 10);

            tp.Controls.Add(dtS, 3, 10);
            tp.SetColumnSpan(dtS, 2);

            tp.Controls.Add(new System.Windows.Forms.Label { Text = "TTL C/T", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 0, 11);
            tp.Controls.Add(new System.Windows.Forms.Label { Text = "Gross W/T", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 1, 11);
            tp.Controls.Add(new System.Windows.Forms.Label { Text = "Request#", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, 11);
            tp.Controls.Add(new System.Windows.Forms.Label { Text = "Lot Qty", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 3, 11);
            tp.Controls.Add(new System.Windows.Forms.Label { Text = "Net WEIGHT", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 4, 11);

            tp.Controls.Add(tb_STTL, 0, 12);
            tp.Controls.Add(tb_SGross, 1, 12);
            tp.Controls.Add(tb_SRequest, 2, 12);
            tp.Controls.Add(tb_SQTY, 3, 12);
            tp.Controls.Add(tb_SWeight, 4, 12);

            tp.Controls.Add(lSSender, 0, 13);
            tp.Controls.Add(lSRecipient, 3, 13);

            tp.SetColumnSpan(lSSender, 3);
            tp.SetColumnSpan(lSRecipient, 2);

            tp.Controls.Add(tb_SSender, 0, 14);
            tp.Controls.Add(tb_SRecipient, 3, 14);

            tp.SetColumnSpan(tb_SSender, 3);
            tp.SetColumnSpan(tb_SRecipient, 2);


            tp.Controls.Add(tb_SSpec, 3, 15);
            tp.SetColumnSpan(tb_SSpec, 2);
        }

        private void btn_ExcelOut_Click(object sender, EventArgs e)
        {
            string DestFilePath = "";

            if (Properties.Settings.Default.SCRAP_DEFAULT_PATH == "")
                DestFilePath = System.Windows.Forms.Application.StartupPath + "\\입고증\\" + String.Format("SCRAP MATL 입고증_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
            else
                DestFilePath = Properties.Settings.Default.SCRAP_DEFAULT_PATH;


            saveFileDialog1.InitialDirectory = DestFilePath;

            saveFileDialog1.FileName = string.Format("SCRAP MATL 입고증 {0}_{1}.xlsx", sRequest, DateTime.Now.ToString("yyyyMMdd"));

            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                DestFilePath = string.Join(@"\", saveFileDialog1.FileName.Split('\\'),0, saveFileDialog1.FileName.Split('\\').Length-1);

                Properties.Settings.Default.SCRAP_DEFAULT_PATH = DestFilePath;
                Properties.Settings.Default.Save();

                if (System.IO.Directory.Exists(DestFilePath) == false)
                    System.IO.Directory.CreateDirectory(DestFilePath);

                //if (File.Exists(DestFilePath) == true)
                //{
                //    File.Delete(DestFilePath);
                //}

                //System.IO.File.Copy(System.Windows.Forms.Application.StartupPath + "\\Excel file\\SCRAP MATL 입고증.xlsx", DestFilePath);

                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                Workbook workbook = application.Workbooks.Open(System.Windows.Forms.Application.StartupPath + "\\Excel file\\SCRAP MATL 입고증.xlsx");
                Worksheet worksheet1 = workbook.Worksheets.get_Item(1);
                application.Visible = false;


                ((Range)worksheet1.Cells[12, 2]).Value2 = tb_BCustName.Text;
                ((Range)worksheet1.Cells[12, 3]).Value2 = tb_BCustCode.Text;
                ((Range)worksheet1.Cells[12, 5]).Value2 = dtB.Text;
                ((Range)worksheet1.Cells[14, 2]).Value2 = tb_BTTL.Text;
                ((Range)worksheet1.Cells[14, 3]).Value2 = tb_BGross.Text;
                ((Range)worksheet1.Cells[14, 4]).Value2 = tb_BRequest.Text;
                ((Range)worksheet1.Cells[14, 5]).Value2 = tb_BQTY.Text;
                ((Range)worksheet1.Cells[14, 6]).Value2 = tb_BWeight.Text;
                ((Range)worksheet1.Cells[16, 2]).Value2 = tb_BSender.Text;
                ((Range)worksheet1.Cells[16, 5]).Value2 = tb_BRecipient.Text;
                ((Range)worksheet1.Cells[18, 5]).Value2 = "SPEC NO : " + tb_BSpec.Text;

                ((Range)worksheet1.Cells[25, 2]).Value2 = tb_SCustName.Text;
                ((Range)worksheet1.Cells[25, 3]).Value2 = tb_SCustCode.Text;
                ((Range)worksheet1.Cells[25, 5]).Value2 = dtS.Text;
                ((Range)worksheet1.Cells[27, 2]).Value2 = tb_STTL.Text;
                ((Range)worksheet1.Cells[27, 3]).Value2 = tb_SGross.Text;
                ((Range)worksheet1.Cells[27, 4]).Value2 = tb_SRequest.Text;
                ((Range)worksheet1.Cells[27, 5]).Value2 = tb_SQTY.Text;
                ((Range)worksheet1.Cells[27, 6]).Value2 = tb_SWeight.Text;
                ((Range)worksheet1.Cells[29, 2]).Value2 = tb_SSender.Text;
                ((Range)worksheet1.Cells[29, 5]).Value2 = tb_SRecipient.Text;
                ((Range)worksheet1.Cells[31, 5]).Value2 = "SPEC NO : " + tb_SSpec.Text;

                worksheet1.SaveAs(DestFilePath + string.Format("\\SCRAP MATL 입고증 {0}_{1}.xlsx", sRequest, DateTime.Now.ToString("yyyyMMdd")));

                workbook.Close();

                Close();
            }
        }
    }

}
