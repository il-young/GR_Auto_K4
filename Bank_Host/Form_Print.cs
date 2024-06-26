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
using System.Speech.Synthesis;
using System.Data.SqlClient;

namespace Bank_Host
{
    public partial class Form_Print : Form
    {
        public TCP SocketManager = null;
        public TCP.EnumConnectStatus SocketState = TCP.EnumConnectStatus.None;

        public TCP QualcommSocketManager = null;
        public TCP.EnumConnectStatus QualcommSocketState = TCP.EnumConnectStatus.None;

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
        SpeechSynthesizer speech = new SpeechSynthesizer();
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

        public void QualcommSocket_Init()
        {
            try
            {
                if (QualcommSocketManager != null)
                    return;

                

                QualcommSocketManager = new TCP();

                QualcommSocketManager.Config.ConnectMode = (TCPConfig.EnumConnectMode)Enum.Parse(typeof(TCPConfig.EnumConnectMode), ConfigurationManager.AppSettings["ConnectType2"], true);
                QualcommSocketManager.Config.IpAddress = Properties.Settings.Default.QualcommPrinterIP;
                QualcommSocketManager.Config.Port = int.Parse(ConfigurationManager.AppSettings["Port2"]);
                QualcommSocketManager.Config.EquipmentId = ConfigurationManager.AppSettings["EquipmentId2"];
                QualcommSocketManager.Config.ReconnectTimer = int.Parse(ConfigurationManager.AppSettings["RetryTime2"]);
                
                QualcommSocketManager.OnConnectStatus += new TCP.OnConnectStatusEvent(socketManager_OnConnectStatus);
                QualcommSocketManager.OnReceivedStringMessage += new TCP.OnReceivedStringMessageEvent(socketManager_OnReceivedStringMessage);
                
                QualcommSocketManager.Open();

                string strMsg = string.Format("Keyence: {0}: {1}", SocketManager.Config.IpAddress, SocketManager.Config.Port);
                Fnc_SaveLog(strMsg);
                Fnc_SaveLog("QualcommSocketManager OK!");

                timer1.Start();
            }
            catch (Exception ex)
            {
                Fnc_SaveLog(ex.ToString());
            }
        }

        public void Socket_Init()
        {
            try
            {
                if (SocketManager != null)
                    return;

                if (ConfigurationManager.AppSettings["CommunicationType2"] != "Socket")
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

        public void QualcomSocket_MessageSend(string strData)
        {
           QualcommSocketManager.SendMessage(STX + strData + ETX);
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
            try
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(strFileName, true))
                {
                    file.WriteLine(strLine);
                }
            }
            catch (Exception)
            {

                //throw;
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

        public bool Fnc_Print(Form_Sort.stAmkor_Label label_info)
        {
            string strBarcodeInfo = "", strCovert_dieqty = "";
            Form_Sort.stAmkor_Label temp = new Form_Sort.stAmkor_Label();


            temp = label_info;

            strCovert_dieqty = temp.DQTY;

            string strCovert_wfrqty = "";

            if (BankHost_main.nScanMode == 1)
            {
                strCovert_wfrqty = temp.WQTY;
            }
            else
            {
                strCovert_wfrqty = temp.WQTY;
            }

            string strCovert_cust = temp.CUST;
            string strCovert_amkorid = temp.AMKOR_ID;
            string strCovert_dcc = "";

            if (temp.DCC != "")
            {
                strCovert_dcc = temp.DCC;
            }

            strBarcodeInfo = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}", temp.Lot, strCovert_dcc, temp.Device, strCovert_dieqty, strCovert_wfrqty, strCovert_amkorid, strCovert_cust, temp.Wafer_ID);

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

            if(strBarcodeInfo.Substring(strBarcodeInfo.Length-1, 1) ==":")
            {
                strBarcodeInfo = strBarcodeInfo.Substring(0, strBarcodeInfo.Length-1);
            }

            strPrint = Fnc_Get_PrintFormat(1, strBarcodeInfo, temp);

            //if (BankHost_main.nAmkorBcrType == 0)
            //    strPrint = Fnc_Get_PrintFormat(1, strBarcodeInfo, temp);
            //else
            //    strPrint = Fnc_Get_PrintFormat_JAR(1, strBarcodeInfo, temp, nIndex, nttl);
            ////string printer = "ZDesigner ZD420-203dpi ZPL";
            ////string printerName = "ZDesigner ZT410-203dpi ZPL (1 복사)"; //다이뱅크에서 실제 사용중인 프린터


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

        public bool Fnc_Print(Form_Sort.stAmkor_Label label_info, int cnt, int ttl)
        {
            string strBarcodeInfo = "", strCovert_dieqty = "";
            Form_Sort.stAmkor_Label temp = new Form_Sort.stAmkor_Label();

            if (Properties.Settings.Default.LabelCopy == true)
            {
                for (int i = 0; i < ttl; i++)
                {

                    temp = label_info;

                    strCovert_dieqty = temp.DQTY;

                    string strCovert_wfrqty = "";

                    if (BankHost_main.nScanMode == 1)
                    {
                        strCovert_wfrqty = temp.WQTY;
                    }
                    else
                    {
                        strCovert_wfrqty = temp.WQTY;
                    }

                    string strCovert_cust = temp.CUST;
                    string strCovert_amkorid = temp.AMKOR_ID;
                    string strCovert_dcc = "";

                    if (temp.DCC != "")
                    {
                        strCovert_dcc = temp.DCC;
                    }

                    strBarcodeInfo = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}", temp.Lot, strCovert_dcc, temp.Device, strCovert_dieqty, strCovert_wfrqty, strCovert_amkorid, strCovert_cust, temp.Wafer_ID);

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

                    if (strBarcodeInfo.Substring(strBarcodeInfo.Length - 1, 1) == ":")
                    {
                        strBarcodeInfo = strBarcodeInfo.Substring(0, strBarcodeInfo.Length - 1);
                    }

                    strPrint = Fnc_Get_PrintFormat(1, strBarcodeInfo, temp, i + 1, ttl);

                    //if (BankHost_main.nAmkorBcrType == 0)
                    //    strPrint = Fnc_Get_PrintFormat(1, strBarcodeInfo, temp);
                    //else
                    //    strPrint = Fnc_Get_PrintFormat_JAR(1, strBarcodeInfo, temp, nIndex, nttl);
                    ////string printer = "ZDesigner ZD420-203dpi ZPL";
                    ////string printerName = "ZDesigner ZT410-203dpi ZPL (1 복사)"; //다이뱅크에서 실제 사용중인 프린터


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
                }
            }
            else
            {
                temp = label_info;

                strCovert_dieqty = temp.DQTY;

                string strCovert_wfrqty = "";

                if (BankHost_main.nScanMode == 1)
                {
                    strCovert_wfrqty = temp.WQTY;
                }
                else
                {
                    strCovert_wfrqty = temp.WQTY;
                }

                string strCovert_cust = temp.CUST;
                string strCovert_amkorid = temp.AMKOR_ID;
                string strCovert_dcc = "";

                if (temp.DCC != "")
                {
                    strCovert_dcc = temp.DCC;
                }

                strBarcodeInfo = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}", temp.Lot, strCovert_dcc, temp.Device, strCovert_dieqty, strCovert_wfrqty, strCovert_amkorid, strCovert_cust, temp.Wafer_ID);

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

                if (strBarcodeInfo.Substring(strBarcodeInfo.Length - 1, 1) == ":")
                {
                    strBarcodeInfo = strBarcodeInfo.Substring(0, strBarcodeInfo.Length - 1);
                }

                strPrint = Fnc_Get_PrintFormat(1, strBarcodeInfo, temp, cnt, ttl);

                //if (BankHost_main.nAmkorBcrType == 0)
                //    strPrint = Fnc_Get_PrintFormat(1, strBarcodeInfo, temp);
                //else
                //    strPrint = Fnc_Get_PrintFormat_JAR(1, strBarcodeInfo, temp, nIndex, nttl);
                ////string printer = "ZDesigner ZD420-203dpi ZPL";
                ////string printerName = "ZDesigner ZT410-203dpi ZPL (1 복사)"; //다이뱅크에서 실제 사용중인 프린터


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
            }


            return bPrintState;
        }

        public bool Fnc_Print(AmkorBcrInfo AmkorBcr, int nType, int nIndex, int nttl)
        {
            string strBarcodeInfo = "", strCovert_dieqty = "";

            
            strCovert_dieqty = AmkorBcr.strDiettl.PadLeft(10, '0');

            AmkorBcr.strWfrttl = Math.Max(int.Parse(AmkorBcr.strWfrttl), int.Parse(AmkorBcr.strWfrQty)).ToString();

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

            strBarcodeInfo = $"{ AmkorBcr.strLotNo}:{strCovert_dcc}:{AmkorBcr.strDevice}:{strCovert_dieqty}:{strCovert_wfrqty}:{strCovert_amkorid}:{strCovert_cust}:{AmkorBcr.strWaferLotNo}:{AmkorBcr.strWSN}:{AmkorBcr.strRID}:{AmkorBcr.strReelDCC}:";

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

            if (BankHost_main.nAmkorBcrType == 0)
                strPrint = Fnc_Get_PrintFormat(nType, strBarcodeInfo, AmkorBcr, nIndex, nttl);
            else if (BankHost_main.nAmkorBcrType == 1)
                strPrint = Fnc_Get_PrintFormat_JAR(nType, strBarcodeInfo, AmkorBcr, nIndex, nttl);
            

            //string printer = "ZDesigner ZD420-203dpi ZPL";
            //string printerName = "ZDesigner ZT410-203dpi ZPL (1 복사)"; //다이뱅크에서 실제 사용중인 프린터


            if (strPrintComType != "ETHERNET")
            {
                bPrintState = SendStringToPrinter(strPrinterName, strPrint);
            }
            else
            {//20221021

                string[] CustNameTemp = Properties.Settings.Default.SecondPrinterCustName.Split(';');
                bool isIn = false;

                for(int  i = 0; i < CustNameTemp.Length; i++)
                {
                    if (CustNameTemp[i] == BankHost_main.strCustName)
                    {                        
                        isIn = true;
                        break;
                    }
                }


                if (isIn == false || BankHost_main.strCustName =="")
                {
                    if(AmkorBcr.strReelDCC != "")
                    {
                        Socket_MessageSend(strPrint);
                    }
                    else
                    {
                        Socket_MessageSend(strPrint);
                    }
                    
                }
                else
                {
                    if(Properties.Settings.Default.GreenLabelPrint == true)
                    {
                        Socket_MessageSend(strPrint);
                    }
                    else
                    {
                        speech.SpeakAsync("라벨 출력이 금지 되어 있습니다.             프린트 설정을 확인 하세요");                        
                    }
                }
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

        public void Fnc_Print_MSG_1Line_Max(string MSG)
        {
            string[] strbill1 = MSG.Split(';');
            int[] Width = new int[strbill1.Length];

            for(int i = 0; i < strbill1.Length; i++)
            {
                if (strbill1[i].Length > 21)
                    Width[i] = 1680 / strbill1[i].Length;
            }
            
            string P_SC_1 = "^XA\r\n";
            string P_SC_2 = "^BY,,10\r\n";
            string P_SC_3 = "";

            for (int i = 0; i < strbill1.Length; i++)
            {
                P_SC_3 += string.Format("^FO{0},{1}^A0N,80,{3} ^FD{2}^FS\r\n", 2 + Properties.Settings.Default.PrintOffsetX, 15 + (110*i) + Properties.Settings.Default.PrintOffsetY, strbill1[i], Width[i]);
            }
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

        public void Fnc_Print_Billinfo(string strBill)
        {
            string strbill1 = strBill;

            strbill1 += string.Format("({0}/{1})", DateTime.Now.Month, DateTime.Now.Day);
            string P_SC_1 = "^XA\r\n";
            string P_SC_2 = "^BY,,10\r\n";
            string P_SC_3 = string.Format("^FO{0},{1}^A0N,80^FD{2}^FS\r\n",17 + Properties.Settings.Default.PrintOffsetX,70 + Properties.Settings.Default.PrintOffsetY, strbill1);
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

        public string Fnc_Get_PrintFormat(int nType, string strBcrinfo, Form_Sort.stAmkor_Label AmkorBarcode)
        {
            //변경 처리 하는 부분
            string strLine1 = "", strLine2 = "", strLine3 = "", strLine4 = "", strLine5 = "", strLine6 = "";

            string strwfrqty = "";

            if (BankHost_main.nScanMode == 1)
            {
                strwfrqty = int.Parse(AmkorBarcode.WQTY).ToString();
            }
            else
            {

                strwfrqty = int.Parse(AmkorBarcode.WQTY).ToString();
                //if (nIndex > 0 && nttl > 1)
                //{
                //    strwfrqty = AmkorBarcode.WQTY;
                //}
                //else
                //{
                //    strwfrqty = AmkorBarcode.WQTY;
                //}
            }

            string P_SC_1 = "^XA\r\n";
            string P_SC_2 = "^BY,,10\r\n";
            string P_SC_3 = string.Format("^FO {0},{1}\r\n",690 + Properties.Settings.Default.PrintOffsetX, 10 + Properties.Settings.Default.PrintOffsetY);
            string P_SC_4 = "^BQN,2,3\r\n";
            string P_SC_5 = "^FDM," + strBcrinfo + "^FS\r\n"; //FDMM  두개를 넣으면 앞에 0이 붙고 안붙고 한다. 주의 
            string strData1_1 = string.Format("CUST : {0}     QTY : {1}  /  {2}\t\t*", int.Parse(AmkorBarcode.CUST).ToString(), int.Parse(AmkorBarcode.DQTY).ToString(), strwfrqty);
            string strData1_2 = string.Format("CUST : {0}     QTY : {1}  /  {2}\t\t", int.Parse(AmkorBarcode.CUST).ToString(), int.Parse(AmkorBarcode.DQTY).ToString(), strwfrqty);

            strLine1 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 40 + Properties.Settings.Default.PrintOffsetY, strData1_1);

            //if (BankHost_main.nScanMode == 1)
            //{
            //    strLine1 = string.Format("^FO 17,40^A0N,30^FD{0}^FS", strData1_1);
            //}
            //else
            //{
            //    if (nIndex > 0 && nttl > 1)
            //    {
            //        strLine1 = string.Format("^FO 17,40^A0N,30^FD{0}^FS", strData1_2);
            //    }
            //    else
            //    {
            //        strLine1 = string.Format("^FO 17,40^A0N,30^FD{0}^FS", strData1_1);
            //    }
            //}

            string strData2 = "";
            if (AmkorBarcode.DCC != "")
                strData2 = string.Format("LOT# : {0}  /  {1}",AmkorBarcode.Lot, AmkorBarcode.DCC);
            else
                strData2 = string.Format("LOT# : {0}", AmkorBarcode.Lot);

            strLine2 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS",17 + Properties.Settings.Default.PrintOffsetX, 75 + Properties.Settings.Default.PrintOffsetY, strData2);

            string strData3 = string.Format("DEVICE : {0}", AmkorBarcode.Device);
            strLine3 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 110 + Properties.Settings.Default.PrintOffsetY, strData3);

            string strData6 = string.Format("WAFER LOT NO : {0}", AmkorBarcode.Wafer_ID);
            strLine6 = string.Format("^FO {0},{1}^AON,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 145 + Properties.Settings.Default.PrintOffsetY, strData6);
            //string strData4 = string.Format("RCV-DATE : {0}     BILL# : {1}", AmkorBarcode.strRcvdate, AmkorBarcode.strBillNo);
            //strLine4 = string.Format("^FO 20,145^ADN,18,10^FD{0}^FS", strData4);

            string P_SC_END = "^XZ\r\n";

            string dados = "";

            //nType = 3;

            if (nType == 1)
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine6;
            }
            else if (nType == 2)
            {
                //string strData5 = string.Format("LOT TYPE : {0}", AmkorBarcode.strLotType);
                //strLine5 = string.Format("^FO 20,165^ADN,18,10^FD{0}^FS", strData5);

                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5;
            }
            else if (nType == 3)
            {
                //if (AmkorBarcode.CUST == "948")
                //    AmkorBarcode. = "PROTO";
                //else if (AmkorBarcode.strCust == "575")
                //    AmkorBarcode.strLotType = "PRO";

                //string strData5 = string.Format("LOT TYPE : {0}", AmkorBarcode.strLotType);
                //strLine5 = string.Format("^FO 20,165^ADN,18,10^FD{0}^FS", strData5);

                //string strData6 = string.Format("WAFER LOT NO : {0}", AmkorBarcode.strWaferLotNo);
                //strLine6 = string.Format("^FO 20,185^ADN,18,10^FD{0}^FS", strData6);

                //dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5 + strLine6;
            }
            else if (nType == 4)
            {
                //string strData5 = string.Format("COO : {0}", AmkorBarcode.strCoo);
                //strLine5 = string.Format("^FO 20,165^ADN,18,10^FD{0}^FS", strData5);

                //dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5;
            }
            else
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4;
            }

            

            dados = dados +  P_SC_END;

            return dados;
        }


        public string Fnc_Get_PrintFormat(int nType, string strBcrinfo, Form_Sort.stAmkor_Label AmkorBarcode, int cnt, int ttl)
        {
            //변경 처리 하는 부분
            string strLine1 = "", strLine2 = "", strLine3 = "", strLine4 = "", strLine5 = "", strLine6 = "";

            string strwfrqty = "";

            if (BankHost_main.nScanMode == 1)
            {
                strwfrqty = (AmkorBarcode.WQTY == "") ? "0" : int.Parse(AmkorBarcode.WQTY).ToString();
            }
            else
            {

                strwfrqty = int.Parse(AmkorBarcode.WQTY).ToString();
                //if (nIndex > 0 && nttl > 1)
                //{
                //    strwfrqty = AmkorBarcode.WQTY;
                //}
                //else
                //{
                //    strwfrqty = AmkorBarcode.WQTY;
                //}
            }

            string P_SC_1 = "^XA\r\n";
            string P_SC_2 = "^BY,,10\r\n";
            string P_SC_3 = string.Format("^FO {0},{1}\r\n", 690 + Properties.Settings.Default.PrintOffsetX, 10 + Properties.Settings.Default.PrintOffsetY);
            string P_SC_4 = "^BQN,2,3\r\n";
            string P_SC_5 = "^FDM," + strBcrinfo + "^FS\r\n"; //FDMM  두개를 넣으면 앞에 0이 붙고 안붙고 한다. 주의 
            string strData1_1 = string.Format("CUST : {0}     QTY : {1}  /  {2}\t\t*", int.Parse(AmkorBarcode.CUST).ToString(), int.Parse(AmkorBarcode.DQTY).ToString(), strwfrqty);
            string strData1_2 = string.Format("CUST : {0}     QTY : {1}  /  {2}\t\t", int.Parse(AmkorBarcode.CUST).ToString(), int.Parse(AmkorBarcode.DQTY).ToString(), strwfrqty);

            strLine1 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 40 + Properties.Settings.Default.PrintOffsetY, strData1_1);

            //if (BankHost_main.nScanMode == 1)
            //{
            //    strLine1 = string.Format("^FO 17,40^A0N,30^FD{0}^FS", strData1_1);
            //}
            //else
            //{
            //    if (nIndex > 0 && nttl > 1)
            //    {
            //        strLine1 = string.Format("^FO 17,40^A0N,30^FD{0}^FS", strData1_2);
            //    }
            //    else
            //    {
            //        strLine1 = string.Format("^FO 17,40^A0N,30^FD{0}^FS", strData1_1);
            //    }
            //}

            string strData2 = "";
            if (AmkorBarcode.DCC != "")
                strData2 = string.Format("LOT# : {0}  /  {1}", AmkorBarcode.Lot, AmkorBarcode.DCC);
            else
                strData2 = string.Format("LOT# : {0}", AmkorBarcode.Lot);

            strLine2 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 75 + Properties.Settings.Default.PrintOffsetY, strData2);

            string strData3 = string.Format("DEVICE : {0}", AmkorBarcode.Device);
            strLine3 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 110 + Properties.Settings.Default.PrintOffsetY, strData3);

            //string strData6 = string.Format("WAFER LOT NO : {0}", AmkorBarcode.Wafer_ID);
            //strLine6 = string.Format("^FO {0},{1}^AON,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 145 + Properties.Settings.Default.PrintOffsetY, strData6);
            //string strData4 = string.Format("RCV-DATE : {0}     BILL# : {1}", AmkorBarcode.strRcvdate, AmkorBarcode.strBillNo);
            //strLine4 = string.Format("^FO 20,145^ADN,18,10^FD{0}^FS", strData4);

            string P_SC_END = "^XZ\r\n";

            string dados = "";

            //nType = 3;

            if (nType == 1)
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine6;
            }
            else if (nType == 2)
            {
                //string strData5 = string.Format("LOT TYPE : {0}", AmkorBarcode.strLotType);
                //strLine5 = string.Format("^FO 20,165^ADN,18,10^FD{0}^FS", strData5);

                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5;
            }
            else if (nType == 3)
            {
                //if (AmkorBarcode.CUST == "948")
                //    AmkorBarcode. = "PROTO";
                //else if (AmkorBarcode.strCust == "575")
                //    AmkorBarcode.strLotType = "PRO";

                //string strData5 = string.Format("LOT TYPE : {0}", AmkorBarcode.strLotType);
                //strLine5 = string.Format("^FO 20,165^ADN,18,10^FD{0}^FS", strData5);

                //string strData6 = string.Format("WAFER LOT NO : {0}", AmkorBarcode.strWaferLotNo);
                //strLine6 = string.Format("^FO 20,185^ADN,18,10^FD{0}^FS", strData6);

                //dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5 + strLine6;
            }
            else if (nType == 4)
            {
                //string strData5 = string.Format("COO : {0}", AmkorBarcode.strCoo);
                //strLine5 = string.Format("^FO 20,165^ADN,18,10^FD{0}^FS", strData5);

                //dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5;
            }
            else
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4;
            }

            if(ttl > 1)
                dados += string.Format("^FO {0},{1}^A0N,80 ^FD{2}/{3}", 760 - ((ttl.ToString().Length + cnt.ToString().Length)*35), 140, cnt, ttl);

            dados = dados + P_SC_END;

            return dados;
        }

        public string MakeQualcommLabel(string code)
        {
            // 0                    1               2            3              4       5     6     
            //1JUN144356508KDM05NFM,PCD90-10670-12C,1THM9545U011,30T15/16/17/18,10D2231,Q7844,14D30-JULY-2023
            //1JUN144356508ABM616XX,1PQLN-2830-0-39BBD-S,1TTESTA431605001,9D2302,Q5000,30P<0x09>Data Matrix	판독
 
            string[] temp = code.Split(',');
            string res = "^XA";

            string LPN = "", MCN ="", lot = "", WaferID = "", DC = "", qty = "", Exp = "", ItemID = "";
            bool isFG = false;

            for(int i = 0; i < temp.Length; i++)
            {
                if(temp[i].Substring(0,2) == "9D")
                {
                    isFG = true;
                    break;
                }
            }


            if (isFG == true)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Substring(0, 2) == "1J")
                        LPN = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 2) == "1T")
                        lot = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 2) == "1P")
                        ItemID = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 2) == "9D")
                        DC = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 1) == "Q")
                        qty = temp[i].Substring(1, temp[i].Length - 1);
                }
            }
            else
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Substring(0, 2) == "1J")
                        LPN = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 2) == "1T")
                        lot = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 3) == "30T")
                        WaferID = temp[i].Substring(3, temp[i].Length - 3);
                    else if (temp[i].Substring(0, 3) == "10D")
                        DC = temp[i].Substring(3, temp[i].Length - 3);
                    else if (temp[i].Substring(0, 3) == "14D")
                        Exp = temp[i].Substring(3, temp[i].Length - 3);
                    else if (temp[i].Substring(0, 1) == "Q")
                        qty = temp[i].Substring(1, temp[i].Length - 1);
                    else if(temp[i].Substring(0,1) == "P")
                        MCN = temp[i].Substring(1, temp[i].Length - 1);
                }
            }
            


            

            

            res += $"^FO{15 + Properties.Settings.Default.PrintOffsetX},{30 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0B,30,30";
            res += $"^FDQualcomm";
            res += $"^FS";


            if (isFG == true)
            {
                res += $"^FO{50 + Properties.Settings.Default.PrintOffsetX},{30 + Properties.Settings.Default.PrintOffsetY}";
                res += "^BXN,5,200";
                res += $"^FD{code}";
                res += "^FS";

                res += $"^FO{250 + Properties.Settings.Default.PrintOffsetX},{20 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,25,25";
                res += $"^FD(1J)LPN: {LPN}";
                res += $"^FS";
                
                res += $"^FO{250 + Properties.Settings.Default.PrintOffsetX},{90 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0,25,25";
                res += $"^FD(1T)Lot Code: {lot}";
                res += $"^FS";
                
                res += $"^FO{250 + Properties.Settings.Default.PrintOffsetX},{160 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0,25,25";
                res += $"^FD(1P) Item ID: {ItemID}";
                res += $"^FS";
                
                res += $"^FO{950 + Properties.Settings.Default.PrintOffsetX},{20 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,20,20";
                res += $"^FD(Q)Quantity: {qty}";
                res += $"^FS";
                
                res += $"^FO{950 + Properties.Settings.Default.PrintOffsetX},{70 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,20,20";
                res += $"^FD(9D) D/C: {DC}";
                res += $"^FS";
                
                res += $"^FO{950 + Properties.Settings.Default.PrintOffsetX},{115 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,20,20";
                res += $"^FDMSL: {Properties.Settings.Default.QualcommMSL}";
                res += $"^FS";
                
                res += $"^FO{1050 + Properties.Settings.Default.PrintOffsetX},{115 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,20,20";
                res += $"^FD2nd LI: {Properties.Settings.Default.Qualcomm2nd}";
                res += $"^FS";
                
                res += $"^FO{950 + Properties.Settings.Default.PrintOffsetX},{160 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,20,20";
                res += $"^FD1118";
                res += $"^FS";
            }
            else
            {
                res += $"^FO{50 + Properties.Settings.Default.PrintOffsetX},{30 + Properties.Settings.Default.PrintOffsetY}";
                res += "^BXN,4.5,200";
                res += $"^FD{code}";
                res += "^FS";

                res += $"^FO{250 + Properties.Settings.Default.PrintOffsetX},{20 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,25,25";
                res += $"^FD(1J)LPN: {LPN}";
                res += $"^FS";

                res += $"^FO{650 + Properties.Settings.Default.PrintOffsetX},{20 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,25,25";
                res += $"^FD(30T)Wafer ID(s): {WaferID}";
                res += $"^FS";

                res += $"^FO{1000 + Properties.Settings.Default.PrintOffsetX},{20 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,25,25";
                res += $"^FD(10D)D/C: {DC}";
                res += $"^FS";

                res += $"^FO{250 + Properties.Settings.Default.PrintOffsetX},{90 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,25,25";
                res += $"^FD(P)MCN: {MCN}";
                res += $"^FS";

                res += $"^FO{650 + Properties.Settings.Default.PrintOffsetX},{90 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,25,25";
                res += $"^FD(Q)Quantity: {qty}";
                res += $"^FS";

                res += $"^FO{850 + Properties.Settings.Default.PrintOffsetX},{90 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,25,25";
                res += $"^FDDry Pack Exp: {Exp}";
                res += $"^FS";

                res += $"^FO{250 + Properties.Settings.Default.PrintOffsetX},{160 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0,25,25";
                res += $"^FD(1T)Lot Code: {lot}";
                res += $"^FS";

                res += $"^FO{1050 + Properties.Settings.Default.PrintOffsetX},{160 + Properties.Settings.Default.PrintOffsetY}";
                res += $"^A0N,25,25";
                res += $"^FD1118";
                res += $"^FS";
            }

            res = res + "^XZ";

            return res;
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
            string P_SC_3 = string.Format("^FO {0},{1}\r\n",690 + Properties.Settings.Default.PrintOffsetX, 20 + Properties.Settings.Default.PrintOffsetY);
            string P_SC_4 = "^BQN,2,3\r\n";
            string P_SC_5 = "^FDM," + strBcrinfo + "^FS\r\n"; //FDMM  두개를 넣으면 앞에 0이 붙고 안붙고 한다. 주의 
            string strData1_1 = string.Format("CUST : {0}     QTY : {1}  /  {2}\t\t*", AmkorBarcode.strCust, AmkorBarcode.strDiettl, strwfrqty);
            string strData1_2 = string.Format("CUST : {0}     QTY : {1}  /  {2}\t\t*", AmkorBarcode.strCust, AmkorBarcode.strDiettl, strwfrqty, nIndex.ToString(), nttl.ToString());

            if (BankHost_main.nScanMode == 1)
            {
                strLine1 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS",17 + Properties.Settings.Default.PrintOffsetX, 10 + Properties.Settings.Default.PrintOffsetY, strData1_1);
            }
            else
            {
                if (nIndex > 0 && nttl > 1)
                {
                    strLine1 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 10 + Properties.Settings.Default.PrintOffsetY, strData1_2);
                }
                else
                {
                    strLine1 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 10 + Properties.Settings.Default.PrintOffsetY, strData1_1);
                }
            }

            


            string strData2 = "";
            if (AmkorBarcode.strLotDcc != "")
                strData2 = string.Format("LOT# : {0}  /  {1}", AmkorBarcode.strLotNo, AmkorBarcode.strLotDcc);
            else
                strData2 = string.Format("LOT# : {0}", AmkorBarcode.strLotNo);

            strLine2 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 40 + Properties.Settings.Default.PrintOffsetY, strData2);

            string strData3 = string.Format("DEVICE : {0}", AmkorBarcode.strDevice);
            strLine3 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 75 + Properties.Settings.Default.PrintOffsetY, strData3);

            string addData10 = $"^FO{17 + Properties.Settings.Default.PrintOffsetX},{110 + Properties.Settings.Default.PrintOffsetY}^ADN,20,10^FDR/D : {AmkorBarcode.strRcvdate}^FS";            
            string addData6 = $"^FO{400 + Properties.Settings.Default.PrintOffsetX},{110 + Properties.Settings.Default.PrintOffsetY}^ADN,20,10^FDB/L : {AmkorBarcode.strBillNo}^FS";

            string addData7 = $"^FO{17 + Properties.Settings.Default.PrintOffsetX},{135 + Properties.Settings.Default.PrintOffsetY}^ADN,20,10^FDWafer LOT : {AmkorBarcode.strWaferLotNo}^FS";
            string addData8 = $"^FO{400 + Properties.Settings.Default.PrintOffsetX},{135 + Properties.Settings.Default.PrintOffsetY}^ADN,20,10^FDWSN : {AmkorBarcode.strWSN}^FS";
                        
            string addData9 = $"^FO{17 + Properties.Settings.Default.PrintOffsetX},{160 + Properties.Settings.Default.PrintOffsetY}^ADN,20,10^FDR ID : {AmkorBarcode.strRID} / {AmkorBarcode.strReelDCC}^FS";
            string addData5 = $"^FO{690 + Properties.Settings.Default.PrintOffsetX},{160 + Properties.Settings.Default.PrintOffsetY}^ADN,20,10^FDL/T : {AmkorBarcode.strLotType}^FS";




            string strWSN = "";

            if (Checkdev(AmkorBarcode.strDevice) == true)
                strWSN = $"^FO{Properties.Settings.Default.PrintOffsetX + 250},{Properties.Settings.Default.PrintOffsetY + 165}^A0N,20^FDWSN: {AmkorBarcode.strWSN}^FS";

            string P_SC_END = "^XZ\r\n";

            string dados = "";

            //nType = 3;

            if (nType == 1)
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + addData5 + addData6 + addData7 + addData8 + addData9 + addData10;                 
            }
            else if(nType == 2)
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + addData5 + addData6 + addData7 + addData8 + addData9 + addData10;
                //string strData5 = string.Format("LOT TYPE : {0}", AmkorBarcode.strLotType);
                //strLine5 = string.Format("^FO {0},{1}^ADN,18,10^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 165 + Properties.Settings.Default.PrintOffsetY, strData5);

                //if(AmkorBarcode.strWSN == "")
                //    dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4;
                //else
                //    dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strWSN + strLine5;
            }
            else if (nType == 3)
            {
                //if (AmkorBarcode.strCust == "948")
                //    AmkorBarcode.strLotType = "PROTO";
                //else if(AmkorBarcode.strCust == "575")
                //    AmkorBarcode.strLotType = "PRO";

                //string strData5 = string.Format("LOT TYPE : {0}", AmkorBarcode.strLotType);
                //strLine5 = string.Format("^FO {0},{1}^ADN,18,10^FD{2}^FS", 20 + Properties.Settings.Default.PrintOffsetX, 165 + Properties.Settings.Default.PrintOffsetY, strData5);

                //string strData6 = "";
                //if (Checkdev(AmkorBarcode.strDevice) == true)
                //    strData6 = BankHost_main.strCust.Contains("WSN") == true ? $"WAFER LOT NO : {AmkorBarcode.strWaferLotNo} WSN : {AmkorBarcode.strWSN}" : $"WAFER LOT NO : {AmkorBarcode.strWaferLotNo}";
                //else
                //    strData6 = $"WAFER LOT NO : {AmkorBarcode.strWaferLotNo}";

                //strLine6 = string.Format("^FO {0},{1}^ADN,18,10^FD{2}^FS", 20 + Properties.Settings.Default.PrintOffsetX, 185 + Properties.Settings.Default.PrintOffsetY, strData6);

                //dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + (AmkorBarcode.strWSN == "" ? "" : strWSN) + strLine5 + strLine6;

                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + addData5 + addData6 + addData7 + addData8 + addData9 + addData10;
            }
            else if (nType == 4)
            {
                string strData5 = string.Format("COO : {0}", AmkorBarcode.strCoo);
                strLine5 = string.Format("^FO {0},{1}^ADN,18,10^FD{2}^FS", 20 + Properties.Settings.Default.PrintOffsetX, 165 + Properties.Settings.Default.PrintOffsetY, strData5);

                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5;
            }
            else
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4;
            }

            if (BankHost_main.strMultiLot == "YES")
            {
                if(nttl > 1)
                {
                    dados += string.Format("^FO600,130^A0,90,90^FD{0}/{1}", nIndex, nttl);
                }                
            }

            if (nttl > 1)
                dados += string.Format("^FO {0},{1}^A0N,80 ^FD{2}/{3}", 760 - ((nttl.ToString().Length + nIndex.ToString().Length) * 35), 140, nIndex, nttl);

            dados = dados + P_SC_END;

            return dados;
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

        public bool Checkdev(string dev)
        {
            bool res = false;
            DataSet ds = SearchData("select Source_Device from TB_QORVO_WSN_DEVICE with(nolock)");

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row["Source_Device"].ToString() == dev)
                {
                    return true;
                }
            }

            return res;
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
            string P_SC_3 = string.Format("^FO {0},{1}\r\n", 630 + Properties.Settings.Default.PrintOffsetX, 2 + Properties.Settings.Default.PrintOffsetY);
            string P_SC_4 = "^BQN,2,2\r\n";
            string P_SC_5 = "^FDM," + strBcrinfo + "^FS\r\n"; //FDMM  두개를 넣으면 앞에 0이 붙고 안붙고 한다. 주의 

            string strData1_1 = "";

            if (AmkorBarcode.strLotDcc != "")
                strData1_1 = string.Format("LOT# : {0} / {1}\tQTY : {2} / {3}", AmkorBarcode.strLotNo, AmkorBarcode.strLotDcc, AmkorBarcode.strDiettl, strwfrqty);
            else
                strData1_1 = string.Format("LOT# : {0}       QTY : {1}  /  {2}", AmkorBarcode.strLotNo, AmkorBarcode.strDiettl, strwfrqty);


            strLine1 = string.Format("^FO {0},{1}^A0N,28^FD{2}^FS", 75 + Properties.Settings.Default.PrintOffsetX, 20 + Properties.Settings.Default.PrintOffsetY, strData1_1);
            
            //string strData2 = "";
            //strData2 = string.Format("QTY : {0}  /  {1}", AmkorBarcode.strDiettl, strwfrqty);
            //strLine2 = string.Format("^FO 97,38^A0N,25^FD{0}^FS", strData2);

            string strData3 = string.Format("CUST : {0}         DEVICE : {1}", AmkorBarcode.strCust, AmkorBarcode.strDevice);
            strLine3 = string.Format("^FO {0},{1}^A0N,20^FD{2}^FS", 75 + Properties.Settings.Default.PrintOffsetX, 60 + Properties.Settings.Default.PrintOffsetY, strData3);

            string P_SC_END = "^XZ\r\n";

            string dados = "";

            //nType = 3;
            dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine3;

            dados = dados + P_SC_END;

            return dados;
        }

        public string Fnc_Get_PrintFormat_MSG(int nType, string msg)
        {
            //변경 처리 하는 부분
            string strLine1 = "", strLine2 = "", strLine3 = "";

            string strwfrqty = "";


            string P_SC_1 = "^XA\r\n";      // 시작            
            string P_SC_2 = string.Format("^FO {0},{1}\r\n", 2 + Properties.Settings.Default.PrintOffsetX, 2 + Properties.Settings.Default.PrintOffsetY); // 출력 시작 위치
            string P_SC_3 = "^A0,N,160,100";
            //string P_SC_4 = "^BQN,2,2\r\n";
            string P_SC_4 = "^FDM," + msg + "^FS\r\n"; //FDMM  두개를 넣으면 앞에 0이 붙고 안붙고 한다. 주의             
            

            //string strData2 = "";
            //strData2 = string.Format("QTY : {0}  /  {1}", AmkorBarcode.strDiettl, strwfrqty);
            //strLine2 = string.Format("^FO 97,38^A0N,25^FD{0}^FS", strData2);

            string P_SC_END = "^XZ\r\n";

            string dados = "";

            //nType = 3;
            dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 +  strLine1 + strLine3 + P_SC_END;            

            return dados;
        }

        public bool SendStringToPrinter(string szString)
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
            bool bJudge = SendBytesToPrinter(strPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return bJudge;
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

        private void OffsetX_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PrintOffsetX = (int)OffsetX.Value;
            Properties.Settings.Default.Save();
        }

        private void OffsetY_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.PrintOffsetY = (int)OffsetY.Value;
            Properties.Settings.Default.Save();
        }

        private void Form_Print_Load(object sender, EventArgs e)
        {
            OffsetX.Value = Properties.Settings.Default.PrintOffsetX;
            OffsetY.Value = Properties.Settings.Default.PrintOffsetY;

            SecondPrintOffsetX.Value = Properties.Settings.Default.SecondPrinterOffsetX;
            SecondPrintOffsetY.Value = Properties.Settings.Default.SecondPrinterOffsetY;

            tb_2ndPrinterIP.Text = Properties.Settings.Default.SecondPrinterIP;

            cb_GreenLabelPrint.Checked = Properties.Settings.Default.GreenLabelPrint;

            tb_QualcommPrinter.Text = Properties.Settings.Default.QualcommPrinterIP;

            if(Properties.Settings.Default.SecondPrinterCustName != "")
            {
                string[] temp = Properties.Settings.Default.SecondPrinterCustName.Split(';');

                for(int i = 0; i < temp.Length; i++)
                {
                    lb_CustName.Items.Add(temp[i]);
                }
            }
        }

        private void btn_CustNameAdd_Click(object sender, EventArgs e)
        {
            bool isit = false;

            for(int i = 0; i < lb_CustName.Items.Count; i++)
            {
                if (lb_CustName.Items[i].ToString() == tb_CustName.Text)
                {
                    isit = true;
                    break;
                }
            }

            if (isit == false)
            {
                lb_CustName.Items.Add(tb_CustName.Text);
                tb_CustName.Text = "";
            }
            else
            {
                MessageBox.Show("동일한 고객명이 존재 합니다.");
            }
        }

        private void btn_CustNameDel_Click(object sender, EventArgs e)
        {
            if(lb_CustName.SelectedIndex != -1)
            {
                lb_CustName.Items.RemoveAt(lb_CustName.SelectedIndex);
            }
            else
            {
                if(lb_CustName.Items.Count > 0)
                    lb_CustName.Items.RemoveAt(0);
            }
        }

        private void btn_2ndPrinterSave_Click(object sender, EventArgs e)
        {
            string CustNames = "";

            Properties.Settings.Default.SecondPrinterIP = tb_2ndPrinterIP.Text.Replace(" " , "");

            Properties.Settings.Default.GreenLabelPrint = cb_GreenLabelPrint.Checked;

            for(int i = 0; i< lb_CustName.Items.Count; i++)
            {
                CustNames += lb_CustName.Items[i] + ";";
            }

            Properties.Settings.Default.SecondPrinterCustName = CustNames;

            Properties.Settings.Default.SecondPrinterOffsetX = (int)SecondPrintOffsetX.Value;
            Properties.Settings.Default.SecondPrinterOffsetY = (int)SecondPrintOffsetY.Value;

            Properties.Settings.Default.Save();
        }

        private void tb_QualcommPrinter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                Properties.Settings.Default.QualcommPrinterIP = tb_QualcommPrinter.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void SecondPrintOffsetX_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Fnc_Print_Billinfo("AATPE2012193");
            //return;

            if (textBox_lotno.Text == "" || textBox_device.Text == "" || textBox_dieqty.Text == "" || textBox_wfrqty.Text == ""
                || textBox_cust.Text == "" || textBox_rvcdate.Text == "" || textBox_billno.Text == "" || textBox_amkorid.Text == "")
            {
                MessageBox.Show("정보를 모두 입력 하여 주십시오!");
                return;
            }

            int nType = BankHost_main.Host.Host_Get_PrintType(textBox_cust.Text);
            //int nType = 1;

            if (nType == 2)
            {
                if (textBox_LotType.Text == "")
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

            AmkorBcrInfo AmkorBarcode = new AmkorBcrInfo();

            int nIndex = 1, nttl = 1;

            AmkorBarcode.strLotNo = textBox_lotno.Text;
            AmkorBarcode.strDevice = textBox_device.Text;
            AmkorBarcode.strDieQty = textBox_dieqty.Text;
            AmkorBarcode.strDiettl = textBox_dieqty.Text;
            AmkorBarcode.strWfrQty = textBox_wfrqty.Text;
            AmkorBarcode.strWfrttl = textBox_wfrqty.Text;
            AmkorBarcode.strAmkorid = textBox_amkorid.Text;
            AmkorBarcode.strCust = textBox_cust.Text;
            AmkorBarcode.strRcvdate = textBox_rvcdate.Text;
            AmkorBarcode.strBillNo = textBox_billno.Text;
            AmkorBarcode.strLotDcc = textBox_dcc.Text;
            AmkorBarcode.strLotType = textBox_LotType.Text;
            AmkorBarcode.strWaferLotNo = textBox_wfrLot.Text;
            AmkorBarcode.strCoo = textBox_coo.Text;
            AmkorBarcode.strWSN = tb_WSN.Text;
            AmkorBarcode.strRID = tb_rId.Text;
            AmkorBarcode.strOperator = "";

            string strCovert_cust = AmkorBarcode.strCust.PadLeft(5, '0');
            string strCovert_amkorid = AmkorBarcode.strAmkorid.PadLeft(10, '0');
            string strCovert_dcc = "";
            string strCovert_dieqty = "";
            Form_Sort.stAmkor_Label temp = new Form_Sort.stAmkor_Label();

            string strCovert_wfrqty = "";

            if (BankHost_main.nScanMode == 1)
            {
                strCovert_wfrqty = temp.WQTY;
            }
            else
            {
                strCovert_wfrqty = temp.WQTY;
            }

            strCovert_dieqty = temp.DQTY;


            string strLine1 = "", strLine2 = "", strLine3 = "", strLine4 = "", strLine5 = "", strLine6 = "";

            string strwfrqty = "";

            AmkorBarcode.strCust = textBox_cust.Text;
            AmkorBarcode.strDieQty = textBox_dieqty.Text;
            AmkorBarcode.strWaferLotNo = textBox_wfrLot.Text;
            AmkorBarcode.strWfrQty = textBox_wfrqty.Text;
            AmkorBarcode.strAmkorid = textBox_cust.Text;
            AmkorBarcode.strDevice = textBox_device.Text;
            AmkorBarcode.strLotDcc = textBox_dcc.Text;
            AmkorBarcode.strLotNo = textBox_lotno.Text;
            AmkorBarcode.strLotType = textBox_LotType.Text;
            AmkorBarcode.strRID = tb_rId.Text;
            AmkorBarcode.strWSN = tb_WSN.Text;
            AmkorBarcode.strBillNo = textBox_billno.Text;
            AmkorBarcode.strCoo = textBox_coo.Text;
            AmkorBarcode.strRcvdate = textBox_rvcdate.Text;



            string strBarcodeInfo = $"{textBox_lotno.Text}:{textBox_dcc.Text}:{textBox_device.Text}:{textBox_dieqty.Text}:{textBox_wfrqty.Text}:{textBox_amkorid.Text}:{textBox_cust.Text}:{textBox_wfrLot.Text}:{tb_WSN.Text}:{tb_rId.Text}::{tb_rQTY.Text}";

            string P_SC_1 = "^XA\r\n";
            string P_SC_2 = "^BY,,10\r\n";
            string P_SC_3 = string.Format("^FO {0},{1}\r\n", 690 + Properties.Settings.Default.PrintOffsetX, 10 + Properties.Settings.Default.PrintOffsetY);
            string P_SC_4 = "^BQN,2,3\r\n";
            string P_SC_5 = "^FDM," + strBarcodeInfo + "^FS\r\n"; //FDMM  두개를 넣으면 앞에 0이 붙고 안붙고 한다. 주의 
            string strData1_1 = string.Format("CUST : {0}     QTY : {1}  /  {2}\tR/QTY : {3}", AmkorBarcode.strCust, AmkorBarcode.strDiettl, textBox_wfrqty.Text, tb_rQTY.Text);
            string strData1_2 = string.Format("CUST : {0}     QTY : {1}  /  {2}\t\t*", AmkorBarcode.strCust, AmkorBarcode.strDiettl, textBox_wfrqty.Text, nIndex.ToString(), nttl.ToString());

            if (BankHost_main.nScanMode == 1)
            {
                strLine1 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 40 + Properties.Settings.Default.PrintOffsetY, strData1_1);
            }
            else
            {
                if (nIndex > 0 && nttl > 1)
                {
                    strLine1 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 40 + Properties.Settings.Default.PrintOffsetY, strData1_2);
                }
                else
                {
                    strLine1 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 40 + Properties.Settings.Default.PrintOffsetY, strData1_1);
                }
            }

            strLine1 += $"^FO{650 + Properties.Settings.Default.PrintOffsetX},{40 + Properties.Settings.Default.PrintOffsetY}^ A0N,80^FD*^FS";



            string strData2 = "";
            if (AmkorBarcode.strLotDcc != "")
                strData2 = string.Format("LOT# : {0}  /  {1}", AmkorBarcode.strLotNo, AmkorBarcode.strLotDcc);
            else
                strData2 = string.Format("LOT# : {0}", AmkorBarcode.strLotNo);

            strLine2 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 75 + Properties.Settings.Default.PrintOffsetY, strData2);

            string strData3 = string.Format("DEV# : {0}", AmkorBarcode.strDevice);
            strLine3 = string.Format("^FO {0},{1}^A0N,30^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 110 + Properties.Settings.Default.PrintOffsetY, strData3);

            string strData4 = $"R/D : {AmkorBarcode.strRcvdate}";
            string strbill = $"BILL : {AmkorBarcode.strBillNo}";
            strLine4 = string.Format("^FO{0},{1}^ADN,18,10^FD{2}^FS", 580 + Properties.Settings.Default.PrintOffsetX, 205 + Properties.Settings.Default.PrintOffsetY, strData4);
            strLine4 += string.Format("^FO{0},{1}^ADN,18,10^FD{2}^FS", 400 + Properties.Settings.Default.PrintOffsetX, 145 + Properties.Settings.Default.PrintOffsetY, strbill);

            string strWSN = "";

            string P_SC_END = "^XZ\r\n";

            string dados = "";

            nType = 3;

            if (nType == 1)
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4;
            }
            else if (nType == 2)
            {
                string strData5 = string.Format("LOT TYPE : {0}", AmkorBarcode.strLotType);
                strLine5 = string.Format("^FO {0},{1}^ADN,18,10^FD{2}^FS", 17 + Properties.Settings.Default.PrintOffsetX, 165 + Properties.Settings.Default.PrintOffsetY, strData5);

                if (AmkorBarcode.strWSN == "")
                    dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4;
                else
                    dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strWSN + strLine5;
            }
            else if (nType == 3)
            {
                if (AmkorBarcode.strCust == "948")
                    AmkorBarcode.strLotType = "PROTO";
                else if (AmkorBarcode.strCust == "575")
                    AmkorBarcode.strLotType = "PRO";

                string strData5 = string.Format("LOT TYPE : {0}", AmkorBarcode.strLotType);
                strLine5 = string.Format("^FO {0},{1}^ADN,18,10^FD{2}^FS", 20 + Properties.Settings.Default.PrintOffsetX, 145 + Properties.Settings.Default.PrintOffsetY, strData5);

                string strData6 = $"^FO{17 + Properties.Settings.Default.PrintOffsetX},{175 + Properties.Settings.Default.PrintOffsetY}^ADN,18,10^FDWFR LOT : {textBox_wfrLot.Text}^FS";
                string strwsn = $"^FO{400 + Properties.Settings.Default.PrintOffsetX},{175 + Properties.Settings.Default.PrintOffsetY}^ADN,18,10^FDWSN : {AmkorBarcode.strWSN}^FS";

                string rid = $"^FO{17 + Properties.Settings.Default.PrintOffsetX},{205 + Properties.Settings.Default.PrintOffsetY}^ADN,18,10^FDR/ID : {AmkorBarcode.strRID}^FS";

                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5 + strData6 + strwsn  + rid;
            }
            else if (nType == 4)
            {
                string strData5 = string.Format("COO : {0}", AmkorBarcode.strCoo);
                strLine5 = string.Format("^FO {0},{1}^ADN,18,10^FD{2}^FS", 20 + Properties.Settings.Default.PrintOffsetX, 165 + Properties.Settings.Default.PrintOffsetY, strData5);

                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4 + strLine5;
            }
            else
            {
                dados = P_SC_1 + P_SC_2 + P_SC_3 + P_SC_4 + P_SC_5 + strLine1 + strLine2 + strLine3 + strLine4;
            }

            if (BankHost_main.strMultiLot == "YES")
            {
                if (nttl > 1)
                {
                    dados += string.Format("^FO600,130^A0,90,90^FD{0}/{1}", nIndex, nttl);
                }
            }

            if (nttl > 1)
                dados += string.Format("^FO {0},{1}^A0N,80 ^FD{2}/{3}", 760 - ((nttl.ToString().Length + nIndex.ToString().Length) * 35), 140, nIndex, nttl);

            dados = dados + P_SC_END;

            if (false == false || BankHost_main.strCustName == "")
            {
                Socket_MessageSend(dados);
            }
            else
            {
                if (Properties.Settings.Default.GreenLabelPrint == true)
                {
                    Socket_MessageSend(dados);
                }
                else
                {
                    speech.SpeakAsync("라벨 출력이 금지 되어 있습니다.             프린트 설정을 확인 하세요");
                }
            }
        }

        private void SecondPrintOffsetY_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void btn_returnLabel_Click(object sender, EventArgs e)
        {
            AmkorBcrInfo AmkorBarcode = new AmkorBcrInfo();

            int nIndex = 1, nttl = 1;

            AmkorBarcode.strLotNo = textBox_lotno.Text;
            AmkorBarcode.strDevice = textBox_device.Text;
            AmkorBarcode.strDieQty = textBox_dieqty.Text;
            AmkorBarcode.strDiettl = textBox_dieqty.Text;
            AmkorBarcode.strWfrQty = textBox_wfrqty.Text;
            AmkorBarcode.strWfrttl = textBox_wfrqty.Text;
            AmkorBarcode.strAmkorid = textBox_amkorid.Text;
            AmkorBarcode.strCust = textBox_cust.Text;
            AmkorBarcode.strRcvdate = textBox_rvcdate.Text;
            AmkorBarcode.strBillNo = textBox_billno.Text;
            AmkorBarcode.strLotDcc = textBox_dcc.Text;
            AmkorBarcode.strLotType = textBox_LotType.Text;
            AmkorBarcode.strWaferLotNo = textBox_wfrLot.Text;
            AmkorBarcode.strCoo = textBox_coo.Text;
            AmkorBarcode.strWSN = tb_WSN.Text;
            AmkorBarcode.strRID = tb_rId.Text;
            AmkorBarcode.strOperator = "";

            string strCovert_cust = AmkorBarcode.strCust.PadLeft(5, '0');
            string strCovert_amkorid = AmkorBarcode.strAmkorid.PadLeft(10, '0');
            string strCovert_dcc = "";
            string strCovert_dieqty = "";
            Form_Sort.stAmkor_Label temp = new Form_Sort.stAmkor_Label();

            string strCovert_wfrqty = "";

            if (BankHost_main.nScanMode == 1)
            {
                strCovert_wfrqty = temp.WQTY;
            }
            else
            {
                strCovert_wfrqty = temp.WQTY;
            }

            strCovert_dieqty = temp.DQTY;


            string strLine1 = "", strLine2 = "", strLine3 = "", strLine4 = "", strLine5 = "", strLine6 = "";

            string strwfrqty = "";

            //AmkorBarcode.strCust = textBox_cust.Text;
            //AmkorBarcode.strDieQty = textBox_dieqty.Text;
            //AmkorBarcode.strWaferLotNo = textBox_wfrLot.Text;
            //AmkorBarcode.strWfrQty = textBox_wfrqty.Text;
            //AmkorBarcode.strAmkorid = textBox_cust.Text;
            //AmkorBarcode.strDevice = textBox_device.Text;
            //AmkorBarcode.strLotDcc = textBox_dcc.Text;
            //AmkorBarcode.strLotNo = textBox_lotno.Text;
            //AmkorBarcode.strLotType = textBox_LotType.Text;
            //AmkorBarcode.strRID = tb_rId.Text;
            //AmkorBarcode.strWSN = tb_WSN.Text;
            //AmkorBarcode.strBillNo = textBox_billno.Text;
            //AmkorBarcode.strCoo = textBox_coo.Text;
            //AmkorBarcode.strRcvdate = textBox_rvcdate.Text;



            string strBarcodeInfo = $"{textBox_lotno.Text}:{textBox_dcc.Text}:{textBox_device.Text}:{textBox_dieqty.Text}:{textBox_wfrqty.Text}:{textBox_amkorid.Text}:{textBox_cust.Text}:{textBox_wfrLot.Text}:{tb_WSN.Text}:{tb_rId.Text}:{tb_reelDCC.Text}:{tb_returnQTY.Text}";

            string P_SC_1 = "^XA\r\n" +
                            $"^FO{10 + Properties.Settings.Default.PrintOffsetX},{10 + Properties.Settings.Default.PrintOffsetY} ^FR ^GB500,300,3 ^FS\r\n" +
                            $"^FO{10 + Properties.Settings.Default.PrintOffsetX},{10 + Properties.Settings.Default.PrintOffsetY} ^FR ^GB130,130,3 ^FS\r\n" +
                            $"^FO{140 + Properties.Settings.Default.PrintOffsetX},{57 + Properties.Settings.Default.PrintOffsetY} ^GB370,3,3 ^FS\r\n" +
                            $"^FO{140 + Properties.Settings.Default.PrintOffsetX},{97 + Properties.Settings.Default.PrintOffsetY} ^GB370,3,3 ^FS\r\n" +
                            $"^FO{140 + Properties.Settings.Default.PrintOffsetX},{137 + Properties.Settings.Default.PrintOffsetY} ^GB370,3,3 ^FS\r\n" +
                            $"^FO{340 + Properties.Settings.Default.PrintOffsetX},{57 + Properties.Settings.Default.PrintOffsetY} ^GB3,80,3 ^FS\r\n" +
                            $"^FO{10 + Properties.Settings.Default.PrintOffsetX},{179 + Properties.Settings.Default.PrintOffsetY} ^GB500,3,3 ^FS\r\n" +
                            $"^FO{10 + Properties.Settings.Default.PrintOffsetX},{221 + Properties.Settings.Default.PrintOffsetY} ^GB500,3,3 ^FS\r\n" +
                            $"^FO{10 + Properties.Settings.Default.PrintOffsetX},{263 + Properties.Settings.Default.PrintOffsetY} ^GB500,3,3 ^FS\r\n";
            P_SC_1 += "^BY,,10\r\n";
            P_SC_1 += string.Format("^FO {0},{1}\r\n", 20 + Properties.Settings.Default.PrintOffsetX, 0 + Properties.Settings.Default.PrintOffsetY);
            P_SC_1 += "^BQN,2,3\r\n";
            P_SC_1 += "^FDM," + strBarcodeInfo + "^FS\r\n"; //FDMM  두개를 넣으면 앞에 0이 붙고 안붙고 한다. 주의 

            P_SC_1 += "^CF0,20"+
                        $"^FO{45 + Properties.Settings.Default.PrintOffsetX},{120 + Properties.Settings.Default.PrintOffsetY}^FDQ:{tb_returnQTY.Text}^FS"+
                        $"^CF0,20"+
                        $"^FO{145 + Properties.Settings.Default.PrintOffsetX},{25 + Properties.Settings.Default.PrintOffsetY}^FDMOD:D03 MC:M1 LOC:1 T/R:3^FS"+
                        $"^FO{145 + Properties.Settings.Default.PrintOffsetX},{70 + Properties.Settings.Default.PrintOffsetY}^FDID:123456^FS"+
                        $"^FO{350 + Properties.Settings.Default.PrintOffsetX},{70 + Properties.Settings.Default.PrintOffsetY}^FDL:AJ54100^FS"+
                        $"^FO{145 + Properties.Settings.Default.PrintOffsetX},{110 + Properties.Settings.Default.PrintOffsetY}^FDD:{DateTime.Now.ToString("yy.MM.dd hh:mm")}^FS"+
                        $"^FO{350 + Properties.Settings.Default.PrintOffsetX},{110 + Properties.Settings.Default.PrintOffsetY}^FDWSN:{AmkorBarcode.strWSN}^FS"+
                        $"^FO{13 + Properties.Settings.Default.PrintOffsetX},{147 + Properties.Settings.Default.PrintOffsetY}^FDR.ID:{AmkorBarcode.strRID} / {tb_reelDCC.Text} ^FS"+
                        $"^FO{13 + Properties.Settings.Default.PrintOffsetX},{192 + Properties.Settings.Default.PrintOffsetY}^FDC:{textBox_cust.Text}^FS"+
                        $"^FO{103 + Properties.Settings.Default.PrintOffsetX},{192 + Properties.Settings.Default.PrintOffsetY}^FDT/QTY:{AmkorBarcode.strDieQty} / {AmkorBarcode.strWfrQty}^FS"+
                        $"^FO{13 + Properties.Settings.Default.PrintOffsetX},{235 + Properties.Settings.Default.PrintOffsetY}^FDDEV:{AmkorBarcode.strDevice}^FS"+
                        $"^FO{13 + Properties.Settings.Default.PrintOffsetX},{275 + Properties.Settings.Default.PrintOffsetY}^FDA/L:{AmkorBarcode.strLotNo} / {AmkorBarcode.strLotDcc}^FS"+
                        $"^XZ";

            Socket_MessageSend(P_SC_1);
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
