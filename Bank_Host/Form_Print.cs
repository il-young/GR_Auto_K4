﻿using System;
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
    public partial class Form_Print : Form
    {
        public TCP SocketManager = null;
        public TCP.EnumConnectStatus SocketState = TCP.EnumConnectStatus.None;
        public string strLogfilePath = "";
        public string strReceivedata = "", strSocketStatus = "";

        string STX = string.Format("{0}", (char)0x02);
        string ETX = string.Format("{0}", (char)0x03);

        ZXing.BarcodeWriter barcodeWriter = new ZXing.BarcodeWriter();
        IntPtr hPrinter = new IntPtr(0);
        DOCINFOA di = new DOCINFOA();

        public static bool bPrintState = false;
        public static string strPrinterName = "", strPrintComType = "";
        public static bool bPrintUse = true;

        public Form_Print()
        {
            InitializeComponent();
            Fnc_Init();
        }

        public void Fnc_Init()
        {
            //di.pDocName = "EXP - QR Barcode Print";
            //di.pDataType = "RAW";

            strPrinterName = ConfigurationManager.AppSettings["Print_Name"];
            label_printname.Text = strPrinterName;

            strPrintComType = ConfigurationManager.AppSettings["Print_Communication"];
            strPrintComType = strPrintComType.ToUpper();

            string strGetinfo = ConfigurationManager.AppSettings["Print_Use"];

            if (strGetinfo == "0")
            {
                bPrintUse = true;
                label_Printuse.Text = "프린트 사용 가능";
                label_Printuse.ForeColor = Color.DarkBlue;
            }
            else
            {
                bPrintUse = false;
                label_Printuse.Text = "프린트 안함";
                label_Printuse.ForeColor = Color.Red;
            }

            if (strPrintComType != "ETHERNET")
            {
                bPrintState = OpenPrinter(strPrinterName.Normalize(), out hPrinter, IntPtr.Zero);
            }
            else
            {
                Socket_Init();
            }


           barcodeWriter.Format = ZXing.BarcodeFormat.QR_CODE;

            barcodeWriter.Options.Width = pictureBox_bcr.Width;
            barcodeWriter.Options.Height = pictureBox_bcr.Height;

            textBox_cust.Focus();
        }

        public void Socket_Init()
        {
            try
            {
                if (SocketManager != null)
                    return;

                if (ConfigurationManager.AppSettings["CommunicationType"] != "Socket")
                    return;

                SocketManager = new TCP();

                SocketManager.Config.ConnectMode = (TCPConfig.EnumConnectMode)Enum.Parse(typeof(TCPConfig.EnumConnectMode), ConfigurationManager.AppSettings["ConnectType2"], true);
                SocketManager.Config.IpAddress = ConfigurationManager.AppSettings["IPAddress2"];
                SocketManager.Config.Port = int.Parse(ConfigurationManager.AppSettings["Port2"]);
                SocketManager.Config.EquipmentId = ConfigurationManager.AppSettings["EquipmentId2"];
                SocketManager.Config.ReconnectTimer = int.Parse(ConfigurationManager.AppSettings["RetryTime2"]);

                SocketManager.OnConnectStatus += new TCP.OnConnectStatusEvent(socketManager_OnConnectStatus);
                SocketManager.OnReceivedStringMessage += new TCP.OnReceivedStringMessageEvent(socketManager_OnReceivedStringMessage);

                SocketManager.Open();

                string strMsg = string.Format("Keyence: {0}: {1}", SocketManager.Config.IpAddress, SocketManager.Config.Port);
                Fnc_SaveLog(strMsg);
                Fnc_SaveLog("SocketManager OK!");

                timer1.Start();
            }
            catch (Exception ex)
            {
                Fnc_SaveLog(ex.ToString());
            }
        }

        public void Socket_Close()
        {
            if (SocketManager != null)
            {
                string strMsg = string.Format("SocketManager Close: {0}", SocketManager.Config.IpAddress);
                Fnc_SaveLog(strMsg);
                SocketManager.Close();
            }

            timer1.Stop();
        }
        public void Socket_MessageSend(string strData)
        {
            SocketManager.SendMessage(STX + strData + ETX);
        }

        public void socketManager_OnConnectStatus(TCP.EnumConnectStatus connectStatus)
        {
            try
            {
                SocketState = connectStatus;

                if (connectStatus == TCP.EnumConnectStatus.Connected)
                {

                }

                string strMsg = string.Format("socketManager_OnConnectStatus: {0}", connectStatus);
                Fnc_SaveLog(strMsg);
            }
            catch (Exception ex)
            {
                Fnc_SaveLog(ex.ToString());
            }
        }

        public void socketManager_OnReceivedStringMessage(string message)
        {
            try
            {
                strReceivedata = message;
                Fnc_SaveLog(message);
            }
            catch (Exception ex)
            {
                Fnc_SaveLog(ex.ToString());
            }
        }
        public void Fnc_SaveLog(string strLog) ///설비별 개별 로그 저장
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath + @"\Log");
            if (!di.Exists) { di.Create(); }
            strLogfilePath = di.ToString();

            string strPath = "";

            strPath = strLogfilePath + "\\Printer_";

            string strToday = string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string strHead = string.Format(",{0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            strPath = strPath + strToday + ".txt";
            strHead = strToday + strHead;

            string strSave;
            strSave = strHead + ',' + strLog;
            Fnc_WriteFile(strPath, strSave);
        }
        public void Fnc_WriteFile(string strFileName, string strLine)
        {
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(strFileName, true))
            {
                file.WriteLine(strLine);
            }
        }


        public void Fnc_Exit()
        {
            if (strPrintComType != "ETHERNET")
            {
                ClosePrinter(hPrinter);
            }
            else
            {
                Socket_Close();
            }

            this.Hide();

            //barcodeWriter = null;

            this.Dispose();
            GC.Collect();
        }

        public bool Fnc_Print(AmkorBcrInfo AmkorBcr, int nType, int nIndex, int nttl)
        {
            string strBarcodeInfo = "", strCovert_dieqty = "";

            
            strCovert_dieqty = AmkorBcr.strDiettl.PadLeft(10, '0');

            string strCovert_wfrqty = "";

            if (BankHost_main.nScanMode == 1)
            {
                strCovert_wfrqty = AmkorBcr.strWfrttl.PadLeft(5, '0');
            }
            else
            {
                if (nIndex > 0 && nttl > 1)
                {
                    strCovert_wfrqty = AmkorBcr.strWfrttl.PadLeft(5, '0');
                }
                else
                {
                    strCovert_wfrqty = AmkorBcr.strWfrQty.PadLeft(5, '0');
                }
            }              

            string strCovert_cust = AmkorBcr.strCust.PadLeft(5, '0');
            string strCovert_amkorid = AmkorBcr.strAmkorid.PadLeft(10, '0');
            string strCovert_dcc = "";

            if(AmkorBcr.strLotDcc != "")
            {
                strCovert_dcc = AmkorBcr.strLotDcc.PadLeft(2, '0');
            }

            strBarcodeInfo = string.Format("{0} :{1} :{2} :{3}:{4}:{5}:{6}:{7}", AmkorBcr.strLotNo, strCovert_dcc, AmkorBcr.strDevice, strCovert_dieqty, strCovert_wfrqty, strCovert_amkorid, strCovert_cust,AmkorBcr.strWaferLotNo);

            if (pictureBox_bcr.Image != null)
            {
                pictureBox_bcr.Image.Dispose();
                pictureBox_bcr.Image = null;
            }

            //ZXing.BarcodeWriter barcodeWriter = new ZXing.BarcodeWriter();
            //barcodeWriter.Format = ZXing.BarcodeFormat.QR_CODE;

            //barcodeWriter.Options.Width = this.pictureBox_bcr.Width;
            //barcodeWriter.Options.Height = this.pictureBox_bcr.Height;

            pictureBox_bcr.Image = barcodeWriter.Write(strBarcodeInfo);

            //int nType = BankHost_main.Host.Host_Get_PrintType(AmkorBcr.strCust);

            string strPrint = "";

            if(BankHost_main.nAmkorBcrType == 0)
                strPrint = Fnc_Get_PrintFormat(nType, strBarcodeInfo, AmkorBcr, nIndex, nttl);
            else
                strPrint = Fnc_Get_PrintFormat_JAR(nType, strBarcodeInfo, AmkorBcr, nIndex, nttl);
            //string printer = "ZDesigner ZD420-203dpi ZPL";
            //string printerName = "ZDesigner ZT410-203dpi ZPL (1 복사)"; //다이뱅크에서 실제 사용중인 프린터


            if (strPrintComType != "ETHERNET")
            {
                bPrintState = SendStringToPrinter(strPrinterName, strPrint);
            }
            else
            {
                Socket_MessageSend(strPrint);
                /*
                while (true)
                {
                    if (Frm_Scanner.strReceivedata == "OK,FTUNE")
                        break;

                    Thread.Sleep(1);

                }
                */
                bPrintState = true;
            }

            return bPrintState;
        }

        public void Fnc_Print_Billinfo(string strBill)
        {
            string strbill1 = strBill;

            strbill1 += string.Format("({0}/{1})", DateTime.Now.Month, DateTime.Now.Day);
            string P_SC_1 = "^XA\r\n";
            string P_SC_2 = "^BY,,10\r\n";
            string P_SC_3 = string.Format("^FO 17,70^A0N,95^FD{0}^FS\r\n", strbill1);
            string P_SC_END = "^XZ\r\n";

            string P_OUT = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_END;

            if (strPrintComType != "ETHERNET")
            {
                bPrintState = SendStringToPrinter(strPrinterName, P_OUT);
            }
            else
            {
                Socket_MessageSend(P_OUT);
                /*
                while (true)
                {
                    if (Frm_Scanner.strReceivedata == "OK,FTUNE")
                        break;

                    Thread.Sleep(1);

                }
                */
                bPrintState = true;
            }

        }

        public string Fnc_Get_PrintFormat(int nType, string strBcrinfo, AmkorBcrInfo AmkorBarcode, int nIndex, int nttl)
        {
            //변경 처리 하는 부분
            string strLine1 = "", strLine2 = "", strLine3 = "", strLine4 = "", strLine5 = "", strLine6 = "";

            string strwfrqty = "";

            if (BankHost_main.nScanMode == 1)
            {
                strwfrqty = AmkorBarcode.strWfrttl;
            }
            else
            {
                if (nIndex > 0 && nttl > 1)
                {
                    strwfrqty = AmkorBarcode.strWfrttl;
                }
                else
                {
                    strwfrqty = AmkorBarcode.strWfrQty;
                }
            }               

            string P_SC_1 = "^XA\r\n";
            string P_SC_2 = "^BY,,10\r\n";
            string P_SC_3 = "^FO 690,50\r\n";
            string P_SC_4 = "^BQN,2,3\r\n";
            string P_SC_5 = "^FDM," + strBcrinfo + "^FS\r\n"; //FDMM  두개를 넣으면 앞에 0이 붙고 안붙고 한다. 주의 
            string strData1_1 = string.Format("CUST : {0}     QTY : {1}  /  {2}\t\t*", AmkorBarcode.strCust, AmkorBarcode.strDiettl, strwfrqty);
            string strData1_2 = string.Format("CUST : {0}     QTY : {1}  /  {2}      ( {3} of {4} )\t\t", AmkorBarcode.strCust, AmkorBarcode.strDiettl, strwfrqty, nIndex.ToString(), nttl.ToString());

            if (BankHost_main.nScanMode == 1)
            {
                strLine1 = string.Format("^FO 17,40^A0N,30^FD{0}^FS", strData1_1);
            }
            else
            {
                if (nIndex > 0 && nttl > 1)
                {
                    strLine1 = string.Format("^FO 17,40^A0N,30^FD{0}^FS", strData1_2);
                }
                else
                {
                    strLine1 = string.Format("^FO 17,40^A0N,30^FD{0}^FS", strData1_1);
                }
            }

            string strData2 = "";
            if (AmkorBarcode.strLotDcc != "")
                strData2 = string.Format("LOT# : {0}  /  {1}", AmkorBarcode.strLotNo, AmkorBarcode.strLotDcc);
            else
                strData2 = string.Format("LOT# : {0}", AmkorBarcode.strLotNo);

            strLine2 = string.Format("^FO 17,75^A0N,30^FD{0}^FS", strData2);

            string strData3 = string.Format("DEVICE : {0}", AmkorBarcode.strDevice);
            strLine3 = string.Format("^FO 17,110^A0N,30^FD{0}^FS", strData3);

            string strData4 = string.Format("RCV-DATE : {0}     BILL# : {1}", AmkorBarcode.strRcvdate, AmkorBarcode.strBillNo);
            strLine4 = string.Format("^FO 20,145^ADN,18,10^FD{0}^FS", strData4);

            string P_SC_END = "^XZ\r\n";

            string dados = "";

            //nType = 3;

            if (nType == 1)
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4;                
            }
            else if(nType == 2)
            {
                string strData5 = string.Format("LOT TYPE : {0}", AmkorBarcode.strLotType);
                strLine5 = string.Format("^FO 20,165^ADN,18,10^FD{0}^FS", strData5);

                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5;
            }
            else if (nType == 3)
            {
                if (AmkorBarcode.strCust == "948")
                    AmkorBarcode.strLotType = "PROTO";
                else if(AmkorBarcode.strCust == "575")
                    AmkorBarcode.strLotType = "PRO";

                string strData5 = string.Format("LOT TYPE : {0}", AmkorBarcode.strLotType);
                strLine5 = string.Format("^FO 20,165^ADN,18,10^FD{0}^FS", strData5);

                string strData6 = string.Format("WAFER LOT NO : {0}", AmkorBarcode.strWaferLotNo);
                strLine6 = string.Format("^FO 20,185^ADN,18,10^FD{0}^FS", strData6);

                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5 + strLine6;
            }
            else if (nType == 4)
            {
                string strData5 = string.Format("COO : {0}", AmkorBarcode.strCoo);
                strLine5 = string.Format("^FO 20,165^ADN,18,10^FD{0}^FS", strData5);

                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5;
            }
            else
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4;
            }

            dados = dados + P_SC_END;

            return dados;
        }

        public string Fnc_Get_PrintFormat_JAR(int nType, string strBcrinfo, AmkorBcrInfo AmkorBarcode, int nIndex, int nttl)
        {
            //변경 처리 하는 부분
            string strLine1 = "", strLine2 = "", strLine3 = "";

            string strwfrqty = "";

            if (BankHost_main.nScanMode == 1)
            {
                strwfrqty = AmkorBarcode.strWfrttl;
            }
            else
            {
                if (nIndex > 0 && nttl > 1)
                {
                    strwfrqty = AmkorBarcode.strWfrttl;
                }
                else
                {
                    strwfrqty = AmkorBarcode.strWfrQty;
                }
            }

            string P_SC_1 = "^XA\r\n";
            string P_SC_2 = "^BY,,10\r\n";
            string P_SC_3 = "^FO 630,2\r\n";
            string P_SC_4 = "^BQN,2,2\r\n";
            string P_SC_5 = "^FDM," + strBcrinfo + "^FS\r\n"; //FDMM  두개를 넣으면 앞에 0이 붙고 안붙고 한다. 주의 
            string strData1_1 = string.Format("LOT# : {0}       QTY : {1}  /  {2}", AmkorBarcode.strLotNo, AmkorBarcode.strDiettl, strwfrqty);
            strLine1 = string.Format("^FO 75,20^A0N,28^FD{0}^FS", strData1_1);

            //string strData2 = "";
            //strData2 = string.Format("QTY : {0}  /  {1}", AmkorBarcode.strDiettl, strwfrqty);
            //strLine2 = string.Format("^FO 97,38^A0N,25^FD{0}^FS", strData2);

            string strData3 = string.Format("CUST : {0}         DEVICE : {1}", AmkorBarcode.strCust, AmkorBarcode.strDevice);
            strLine3 = string.Format("^FO 75,60^A0N,20^FD{0}^FS", strData3);

            string P_SC_END = "^XZ\r\n";

            string dados = "";

            //nType = 3;
            dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine3;

            dados = dados + P_SC_END;

            return dados;
        }

        public bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;

            // How many characters are in the string?
            // Fix from Nicholas Piasecki:
            // dwCount = szString.Length;
            dwCount = (szString.Length + 1) * Marshal.SystemMaxDBCSCharSize;

            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            bool bJudge = SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return bJudge;
        }

        //For USB Print 추가되는 부분
        public bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            //IntPtr hPrinter = new IntPtr(0);
            //DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.
            //di.pDocName = "EXP - QR Barcode Print";
            //di.pDataType = "RAW";

            // Open the printer.
            if (bPrintState)
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        //Thread.Sleep(200);
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                //ClosePrinter(hPrinter);
            }

            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        //For USB Print 추가되는 부분
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        private void button_Print_Click(object sender, EventArgs e)
        {
            //Fnc_Print_Billinfo("AATPE2012193");
            //return;

            if (textBox_lotno.Text == "" || textBox_device.Text == "" || textBox_dieqty.Text == "" || textBox_wfrqty.Text =="" 
                || textBox_cust.Text == "" || textBox_rvcdate.Text == "" || textBox_billno.Text == "" || textBox_amkorid.Text == "")
            {
                MessageBox.Show("정보를 모두 입력 하여 주십시오!");
                return;
            }

            int nType = BankHost_main.Host.Host_Get_PrintType(textBox_cust.Text);
            //int nType = 1;

            if (nType == 2)
            {
                if(textBox_LotType.Text == "")
                {
                    MessageBox.Show("해당 고객은 Lot Type 이 입력 되어야 합니다.");
                    return;
                }
            }
            else if (nType == 3)
            {
                if (textBox_LotType.Text == "" || textBox_wfrLot.Text == "")
                {
                    MessageBox.Show("해당 고객은 Lot Type과 Wafer Lot # 가 입력 되어야 합니다.");
                    return;
                }
            }
            else if (nType == 4)
            {
                if (textBox_coo.Text == "")
                {
                    MessageBox.Show("해당 고객은 COO 가 입력 되어야 합니다.");
                    return;
                }
            }

            AmkorBcrInfo Amkor = new AmkorBcrInfo();

            Amkor.strLotNo = textBox_lotno.Text;
            Amkor.strDevice = textBox_device.Text;
            Amkor.strDieQty = textBox_dieqty.Text;
            Amkor.strDiettl = textBox_dieqty.Text;
            Amkor.strWfrQty = textBox_wfrqty.Text;
            Amkor.strWfrttl = textBox_wfrqty.Text;
            Amkor.strAmkorid = textBox_amkorid.Text;
            Amkor.strCust = textBox_cust.Text;
            Amkor.strRcvdate = textBox_rvcdate.Text;
            Amkor.strBillNo = textBox_billno.Text;
            Amkor.strLotDcc = textBox_dcc.Text;
            Amkor.strLotType = textBox_LotType.Text;
            Amkor.strWaferLotNo = textBox_wfrLot.Text;
            Amkor.strCoo = textBox_coo.Text;
            Amkor.strOperator = "";

            Fnc_Print(Amkor, nType, 0, 0);
            /*
            textBox_lotno.Text = "";
            textBox_device.Text = "";
            textBox_dieqty.Text = "";
            textBox_wfrqty.Text = "";
            textBox_cust.Text = "";
            textBox_rvcdate.Text = "";
            textBox_billno.Text = "";
            textBox_dcc.Text = "";
            textBox_LotType.Text = "";
            textBox_wfrLot.Text = "";
            textBox_coo.Text = "";
            */
        }

        private void Form_Print_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox_cust_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_lotno.Focus();
            }
        }

        private void textBox_lotno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_dcc.Focus();
            }
        }

        private void textBox_device_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_rvcdate.Focus();
            }
        }

        private void textBox_rvcdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_dieqty.Focus();
            }
        }

        private void textBox_dieqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_wfrqty.Focus();
            }
        }

        private void textBox_wfrqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_billno.Focus();
            }
        }

        private void textBox_billno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_amkorid.Focus();
            }
        }

        private void textBox_dcc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_device.Focus();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox_receivedata.Text = strReceivedata;
            label_state.Text = SocketState.ToString();

            /*
            if (label_state.Text.ToLower() == "connected")
            {
                label_state.BackColor = Color.Green;
                //BankHost_main.bVisionConnect = true;
            }
            else
            {
                label_state.BackColor = Color.Red;
                //BankHost_main.bVisionConnect = false;
            }
            */
        }

        private void textBox_amkorid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button1.Focus();
            }
        }

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
    }

}