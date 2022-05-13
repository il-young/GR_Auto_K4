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

namespace Bank_Host
{
    public partial class Form_InBill : Form
    {
        

        public Form_InBill()
        {
            InitializeComponent();
        }

        Label title1 = new Label { Text = "SCRAP MAT'L 입고증 / K4 BANK", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 16, FontStyle.Bold) };
        Label lCust = new Label { Text = "CUSTOMER", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        Label ldate = new Label { Text = "DATE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        TextBox tb_BCustName = new TextBox { Text = "tb_BCustName", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_BCustCode = new TextBox { Text = "tb_BCustCode", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_BLineCode = new TextBox { Text = "tb_BLineCode", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        DateTimePicker dtB = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd", Dock = DockStyle.Fill, Margin = Padding.Empty };
        TextBox tb_BTTL = new TextBox { Text = "tb_BTTL", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_BGross = new TextBox { Text = "tb_BGross", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_BRequest = new TextBox { Text = "tb_BRequest", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_BQTY = new TextBox { Text = "tb_BQTY", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_BWeight = new TextBox { Text = "tb_BWeight", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        Label lBRecipient = new Label { Text = "인  수  자", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        Label lBSender = new Label { Text = "입  고  자", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        TextBox tb_BRecipient = new TextBox { Text = "인  수  자", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty};
        TextBox tb_BSender = new TextBox { Text = "입  고  자", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };

        Label lStore = new Label { Text = "SCRAP MAT'L 입고증 / K4 STORE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 16, FontStyle.Bold) };
        Label lSCust = new Label { Text = "CUSTOMER", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        Label lSdate = new Label { Text = "DATE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        TextBox tb_SCustName = new TextBox { Text = "tb_BCustName", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_SCustCode = new TextBox { Text = "tb_BCustCode", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_SLineCode = new TextBox { Text = "tb_BLineCode", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        DateTimePicker dtS = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd", Dock = DockStyle.Fill, Margin = Padding.Empty };
        TextBox tb_STTL = new TextBox { Text = "tb_BTTL", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_SGross = new TextBox { Text = "tb_BGross", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_SRequest = new TextBox { Text = "tb_BRequest", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_SQTY = new TextBox { Text = "tb_BQTY", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_SWeight = new TextBox { Text = "tb_BWeight", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        Label lSRecipient = new Label { Text = "인  수  자", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        Label lSSender = new Label { Text = "입  고  자", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
        TextBox tb_SRecipient = new TextBox { Text = "인  수  자", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };
        TextBox tb_SSender = new TextBox { Text = "입  고  자", Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Margin = Padding.Empty };

        private void Form_InBill_Load(object sender, EventArgs e)
        {            
            tp.Controls.Add(title1, 0, 0);
            tp.SetColumnSpan(title1, 5);
            
            tp.Controls.Add(lCust, 0, 1);
            tp.SetColumnSpan(lCust, 2);

            tp.Controls.Add(new Label { Text = "LINE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, 1);
            
            tp.Controls.Add(ldate, 3, 1);
            tp.SetColumnSpan(ldate, 2);

            tp.Controls.Add(tb_BCustName, 0, 2);
            tp.Controls.Add(tb_BCustCode, 1, 2);
            tp.Controls.Add(tb_BLineCode, 2, 2);

            tp.Controls.Add(dtB, 3, 2);
            tp.SetColumnSpan(dtB, 2);

            tp.Controls.Add(new Label { Text = "TTL C/T", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter}, 0, 3);
            tp.Controls.Add(new Label { Text = "Gross W/T", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter}, 1, 3);
            tp.Controls.Add(new Label { Text = "Request#", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, 3);
            tp.Controls.Add(new Label { Text = "Lot Qty", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 3, 3);
            tp.Controls.Add(new Label { Text = "Net WEIGHT", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 4, 3);

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


            tp.Controls.Add(lStore, 0, 9);
            tp.SetColumn(lStore, 5);


            tp.Controls.Add(lSCust, 0, 1);
            tp.SetColumnSpan(lSCust, 2);

            tp.Controls.Add(new Label { Text = "LINE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, 1);

            tp.Controls.Add(lSdate, 3, 1);
            tp.SetColumnSpan(lSdate, 2);

            tp.Controls.Add(tb_SCustName, 0, 2);
            tp.Controls.Add(tb_SCustCode, 1, 2);
            tp.Controls.Add(tb_SLineCode, 2, 2);

            tp.Controls.Add(dtS, 3, 2);
            tp.SetColumnSpan(dtS, 2);

            tp.Controls.Add(new Label { Text = "TTL C/T", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 0, 3);
            tp.Controls.Add(new Label { Text = "Gross W/T", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 1, 3);
            tp.Controls.Add(new Label { Text = "Request#", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, 3);
            tp.Controls.Add(new Label { Text = "Lot Qty", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 3, 3);
            tp.Controls.Add(new Label { Text = "Net WEIGHT", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 4, 3);

            tp.Controls.Add(tb_STTL, 0, 4);
            tp.Controls.Add(tb_SGross, 1, 4);
            tp.Controls.Add(tb_SRequest, 2, 4);
            tp.Controls.Add(tb_SQTY, 3, 4);
            tp.Controls.Add(tb_SWeight, 4, 4);

            tp.Controls.Add(lSSender, 0, 5);
            tp.Controls.Add(lSRecipient, 3, 5);

            tp.SetColumnSpan(lSSender, 3);
            tp.SetColumnSpan(lSRecipient, 2);

            tp.Controls.Add(tb_SSender, 0, 6);
            tp.Controls.Add(tb_SRecipient, 3, 6);

            tp.SetColumnSpan(tb_SSender, 3);
            tp.SetColumnSpan(tb_SRecipient, 2);

        }
    }

}
