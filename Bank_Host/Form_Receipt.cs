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
using System.Data.SqlClient;

namespace Bank_Host
{
    public partial class Form_InBill : Form
    {
        System.Windows.Forms.Label      title1          = new System.Windows.Forms.Label { Text = "SCRAP MAT'L 입고증 / K4 BANK", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold) };
        System.Windows.Forms.Label      lCust           = new System.Windows.Forms.Label { Text = "CUSTOMER", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.Label      ldate           = new System.Windows.Forms.Label { Text = "DATE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.ComboBox   tb_BCustName     = new System.Windows.Forms.ComboBox { Text = "tb_BCustName", Dock = DockStyle.Fill, Margin = Padding.Empty };
        System.Windows.Forms.ComboBox   tb_BCustCode     = new System.Windows.Forms.ComboBox { Text = "tb_BCustCode", Dock = DockStyle.Fill, Margin = Padding.Empty };
        System.Windows.Forms.TextBox    tb_BLineCode     = new System.Windows.Forms.TextBox { Text = "AJ45400", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        DateTimePicker                  dtB             = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd", Dock = DockStyle.Fill, Margin = Padding.Empty };
        System.Windows.Forms.TextBox    tb_BTTL         = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox    tb_BGross       = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox    tb_BRequest     = new System.Windows.Forms.TextBox { Text = "tb_BRequest", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox    tb_BQTY         = new System.Windows.Forms.TextBox { Text = "tb_BQTY", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox    tb_BWeight      = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.Label      lBRecipient     = new System.Windows.Forms.Label { Text = "인  수  자", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.Label      lBSender        = new System.Windows.Forms.Label { Text = "입  고  자", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        System.Windows.Forms.TextBox    tb_BRecipient   = new System.Windows.Forms.TextBox { Text = "", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox    tb_BSender      = new System.Windows.Forms.TextBox { Text = string.Format("{0}({1})", BankHost_main.strOperator, BankHost_main.strID), Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        System.Windows.Forms.TextBox    tb_BSpec        = new System.Windows.Forms.TextBox { Text = "SPEC NO : 001-2698", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };

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
        string sLineCode = "";
        string sDate = "";
        string sTTL = "";
        string sWT = "";
        string sRequest = "";
        string sQTY = "";
        string sWeight = "";
        string sReceipt = "";
        string sConsignee = "";
        bool en = true;


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

        public Form_InBill(List<string> CustCode, List<string> CustName, string LineCode, string Date, string TTL, string WT, 
            string Request, string QTY, string Weight, string Receipt, string Consignee)
        {
            sCustCode = CustCode;
            sCustName = CustName;
            sLineCode = LineCode;
            sDate = Date;
            sTTL = TTL;
            sWT = WT;
            sRequest = Request;
            sQTY = QTY;
            sWeight = Weight;
            sReceipt = Receipt;
            sConsignee = Consignee;

            en = false;

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
            tb_BLineCode.Text = sLineCode == "" ? tb_BLineCode.Text : sLineCode;
            dtB.Text = sDate == "" ? dtB.Text : sDate;
            tb_BTTL.Text = sTTL == "" ? tb_BTTL.Text : sTTL;
            tb_BGross.Text = sWT == "" ? tb_BGross.Text : sWT;
            tb_BRequest.Text = sRequest;
            tb_BQTY.Text = sQTY;
            tb_BWeight.Text = sWeight == "" ? tb_BWeight.Text : sWeight;
            tb_BRecipient.Text = sReceipt == "" ? tb_BRecipient.Text : sReceipt;
            tb_BSender.Text = sConsignee;
            //tb_BRecipient.Text = BankHost_main.strOperator;

            tb_BCustName.Enabled = en;
            tb_BCustCode.Enabled = en;
            tb_BLineCode.Enabled = en;
            dtB.Enabled = en;
            tb_BTTL.Enabled = en;
            tb_BGross.Enabled = en;
            tb_BRequest.Enabled = en;
            tb_BQTY.Enabled = en;
            tb_BWeight.Enabled = en;
            lBRecipient.Enabled = en;
            lBSender.Enabled = en;
            tb_BRecipient.Enabled = en;
            tb_BSender.Enabled = en;
            tb_BSpec.Enabled = en;

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
                string query1 = string.Format("Select count(*) from TB_SCRAP_RECEIPT with(NOLOCK) where [REQUEST_NUM]={0} and [DATE]='{1}'", tb_BRequest.Text, dtB.Text);
                int cnt = run_count(query1);

                if (cnt == 0)
                {
                    string query = string.Format("Insert INTO TB_SCRAP_RECEIPT values('{0}', {1}, '{2}', '{3}', '{4}', '{5}', {6}, {7}, '{8}', '{9}', '{10}', '{11}')",
                        tb_BCustName.Text, tb_BCustCode.Text, tb_BLineCode.Text, dtB.Text, tb_BTTL.Text, tb_BGross.Text, tb_BRequest.Text, tb_BQTY.Text,
                        tb_BWeight.Text, tb_BRecipient.Text, tb_BSender.Text, string.Format("{0}({1})",BankHost_main.strOperator, BankHost_main.strID));

                    run_sql_command(query);

                    DestFilePath = string.Join(@"\", saveFileDialog1.FileName.Split('\\'), 0, saveFileDialog1.FileName.Split('\\').Length - 1);

                    Properties.Settings.Default.SCRAP_DEFAULT_PATH = DestFilePath;
                    Properties.Settings.Default.Save();

                    if (System.IO.Directory.Exists(DestFilePath) == false)
                        System.IO.Directory.CreateDirectory(DestFilePath);

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
                    ((Range)worksheet1.Cells[18, 5]).Value2 = tb_BSpec.Text;

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
                    ((Range)worksheet1.Cells[31, 5]).Value2 = tb_SSpec.Text;

                    worksheet1.SaveAs(DestFilePath + string.Format("\\SCRAP MATL 입고증 {0}_{1}.xlsx", sRequest, DateTime.Now.ToString("yyyyMMdd")));

                    workbook.Close();

                    //MessageBox.Show(string.Format("{2}\\SCRAP MATL 입고증 {0}_{1}.xlsx \n에 저장 되었습니다.", sRequest, DateTime.Now.ToString("yyyyMMdd"), DestFilePath));

                    Close();
                }
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("금일 동일한 Request로 발행된 입고증이 있습니다.\n새로 발행 하시겠습니까?", "재발행", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        string query = string.Format("update TB_SCRAP_RECEIPT set CUSTOMER_NAME='{0}', CUSTOMER_CODE={1}, LINE_CODE='{2}', [DATE]='{3}', TTL_CT='{4}'," +
                            " GROSS_WT='{5}', REQUEST_NUM={6}, LOT_QTY={7}, WEIGHT='{8}', RECEIPT='{9}', CONSIGNEE='{10}', EMPLOYEE='{11}' where REQUEST_NUM={11} and [DATE]='{12}'",
                        tb_BCustName.Text, tb_BCustCode.Text, tb_BLineCode.Text, dtB.Text, tb_BTTL.Text, tb_BGross.Text, tb_BRequest.Text, tb_BQTY.Text,
                        tb_BWeight.Text, tb_BRecipient.Text, tb_BSender.Text, tb_BRequest.Text, dtB.Text, string.Format("{0}({1})", BankHost_main.strOperator, BankHost_main.strID));

                        run_sql_command(query);

                        DestFilePath = string.Join(@"\", saveFileDialog1.FileName.Split('\\'), 0, saveFileDialog1.FileName.Split('\\').Length - 1);

                        Properties.Settings.Default.SCRAP_DEFAULT_PATH = DestFilePath;
                        Properties.Settings.Default.Save();

                        if (System.IO.Directory.Exists(DestFilePath) == false)
                            System.IO.Directory.CreateDirectory(DestFilePath);

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
                        ((Range)worksheet1.Cells[18, 5]).Value2 = tb_BSpec.Text;

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
                        ((Range)worksheet1.Cells[31, 5]).Value2 = tb_SSpec.Text;

                        worksheet1.SaveAs(DestFilePath + string.Format("\\SCRAP MATL 입고증 {0}_{1}.xlsx", sRequest, DateTime.Now.ToString("yyyyMMdd")));

                        workbook.Close();

                        //MessageBox.Show(string.Format("{2}\\SCRAP MATL 입고증 {0}_{1}.xlsx \n에 저장 되었습니다.", sRequest, DateTime.Now.ToString("yyyyMMdd"), DestFilePath));

                        Close();
                    }
                    else
                    {
                        if(DialogResult.Yes == MessageBox.Show("기존 입고증으로 출력하시겠습니까?", "기존 입고증", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                        {
                            string ReceiptQuery = string.Format("select [CUSTOMER_NAME], [CUSTOMER_CODE], [LINE_CODE], [DATE], [TTL_CT], [GROSS_WT], [REQUEST_NUM], " +
                                "[LOT_QTY], [WEIGHT], [RECEIPT], [CONSIGNEE] from TB_SCRAP_RECEIPT with(NOLOCK) where [REQUEST_NUM]={0} and [DATE]='{1}'", tb_BRequest.Text, dtB.Text);
                            DataSet dt = SearchData(ReceiptQuery);

                            DestFilePath = string.Join(@"\", saveFileDialog1.FileName.Split('\\'), 0, saveFileDialog1.FileName.Split('\\').Length - 1);

                            Properties.Settings.Default.SCRAP_DEFAULT_PATH = DestFilePath;
                            Properties.Settings.Default.Save();

                            if (System.IO.Directory.Exists(DestFilePath) == false)
                                System.IO.Directory.CreateDirectory(DestFilePath);

                            Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                            Workbook workbook = application.Workbooks.Open(System.Windows.Forms.Application.StartupPath + "\\Excel file\\SCRAP MATL 입고증.xlsx");
                            Worksheet worksheet1 = workbook.Worksheets.get_Item(1);
                            application.Visible = false;

                            ((Range)worksheet1.Cells[12, 2]).Value2 = dt.Tables[0].Rows[0][0].ToString();
                            ((Range)worksheet1.Cells[12, 3]).Value2 = dt.Tables[0].Rows[0][1].ToString();
                            ((Range)worksheet1.Cells[12, 4]).Value2 = dt.Tables[0].Rows[0][2].ToString();
                            ((Range)worksheet1.Cells[12, 5]).Value2 = dt.Tables[0].Rows[0][3].ToString();
                            ((Range)worksheet1.Cells[14, 2]).Value2 = dt.Tables[0].Rows[0][4].ToString();
                            ((Range)worksheet1.Cells[14, 3]).Value2 = dt.Tables[0].Rows[0][5].ToString();
                            ((Range)worksheet1.Cells[14, 4]).Value2 = dt.Tables[0].Rows[0][6].ToString();
                            ((Range)worksheet1.Cells[14, 5]).Value2 = dt.Tables[0].Rows[0][7].ToString();
                            ((Range)worksheet1.Cells[14, 6]).Value2 = dt.Tables[0].Rows[0][8].ToString();
                            ((Range)worksheet1.Cells[16, 2]).Value2 = dt.Tables[0].Rows[0][9].ToString();
                            ((Range)worksheet1.Cells[16, 5]).Value2 = dt.Tables[0].Rows[0][10].ToString();
                            ((Range)worksheet1.Cells[18, 5]).Value2 = tb_BSpec.Text;

                            ((Range)worksheet1.Cells[25, 2]).Value2 = dt.Tables[0].Rows[0][0].ToString();
                            ((Range)worksheet1.Cells[25, 3]).Value2 = dt.Tables[0].Rows[0][1].ToString();
                            ((Range)worksheet1.Cells[25, 4]).Value2 = dt.Tables[0].Rows[0][2].ToString();
                            ((Range)worksheet1.Cells[25, 5]).Value2 = dt.Tables[0].Rows[0][3].ToString();
                            ((Range)worksheet1.Cells[27, 2]).Value2 = dt.Tables[0].Rows[0][4].ToString();
                            ((Range)worksheet1.Cells[27, 3]).Value2 = dt.Tables[0].Rows[0][5].ToString();
                            ((Range)worksheet1.Cells[27, 4]).Value2 = dt.Tables[0].Rows[0][6].ToString();
                            ((Range)worksheet1.Cells[27, 5]).Value2 = dt.Tables[0].Rows[0][7].ToString();
                            ((Range)worksheet1.Cells[27, 6]).Value2 = dt.Tables[0].Rows[0][8].ToString();
                            ((Range)worksheet1.Cells[29, 2]).Value2 = dt.Tables[0].Rows[0][9].ToString();
                            ((Range)worksheet1.Cells[29, 5]).Value2 = dt.Tables[0].Rows[0][10].ToString();
                            ((Range)worksheet1.Cells[31, 5]).Value2 = tb_SSpec.Text;

                            worksheet1.SaveAs(DestFilePath + string.Format("\\SCRAP MATL 입고증 {0}_{1}.xlsx", sRequest, DateTime.Now.ToString("yyyyMMdd")));
                            workbook.Close();                           
                            Close();

                            //MessageBox.Show(string.Format("{2}\\SCRAP MATL 입고증 {0}_{1}.xlsx \n에 저장 되었습니다.", sRequest, DateTime.Now.ToString("yyyyMMdd"), DestFilePath));
                        }
                    }
                }
            }
        }


        public void run_sql_command(string sql)
        {
            try
            {
                //lock (this)
                {
                    using (SqlConnection ssconn = new SqlConnection("server = 10.135.200.35; uid = amm; pwd = amm@123; database = GR_Automation"))
                    {
                        ssconn.Open();
                        using (SqlCommand scom = new SqlCommand(sql, ssconn))
                        {
                            scom.CommandType = System.Data.CommandType.Text;
                            scom.CommandText = sql;
                            scom.ExecuteReader();
                        }
                    }
                    //ssconn.Close();
                    //ssconn.Dispose();
                    //scom.Dispose();
                }
                //frm_Main.save_log(string.Format("Call:{0} -> Function:{1}, Param:{2}", System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql));
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private System.Data.DataSet SearchData(string sql)
        {
            System.Data.DataSet dt = new System.Data.DataSet();

            try
            {
                using (SqlConnection c = new SqlConnection("server = 10.135.200.35; uid = amm; pwd = amm@123; database = GR_Automation"))
                {
                    c.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, c))
                    {
                        using (SqlDataAdapter adt = new SqlDataAdapter(cmd))
                        {
                            adt.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public int run_count(string sql_str)
        {
            int res = -1;
            try
            {
                using (SqlConnection ssconn = new SqlConnection("server = 10.135.200.35; uid = amm; pwd = amm@123; database = GR_Automation"))
                {
                    ssconn.Open();
                    using (SqlCommand scom = new SqlCommand(sql_str, ssconn))
                    {
                        scom.CommandType = System.Data.CommandType.Text;
                        scom.CommandText = sql_str;
                        res = (int)scom.ExecuteScalar();
                    }
                }
                
                return res;
            }
            catch (Exception ex)
            {

            }

            return res;
        }
    }

}
