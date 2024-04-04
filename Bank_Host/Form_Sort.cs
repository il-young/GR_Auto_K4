using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Threading;
using System.Speech.Synthesis;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.Devices;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

using Zebra.Sdk.Comm;
using Microsoft.Win32;
using Application = System.Windows.Forms.Application;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bank_Host
{
    public partial class Form_Sort : Form
    {
        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr IParam);

        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("imm32.dll")]
        public static extern Boolean ImmSetConversionStatus(IntPtr hIMC, Int32 fdwConversion, Int32 fdwSentence);

        public const int IME_CMODE_ALPHANUMERIC = 0x0000;
        private const int WM_IME_CONTROL = 643;

        public string strBcrType = "";

        public bool LotSPR = false;



        public static readonly NLog.Logger ReaderLog = NLog.LogManager.GetLogger("ReaderLog");
        public static readonly NLog.Logger InkjetLog = NLog.LogManager.GetLogger("InkjetLog");
        public static readonly NLog.Logger Wlog = NLog.LogManager.GetLogger("WebThread");
        public static readonly NLog.Logger Dlog = NLog.LogManager.GetLogger("DBThread");
        public static readonly NLog.Logger Slog = NLog.LogManager.GetLogger("SEQLog");
        public static readonly NLog.Logger Blog = NLog.LogManager.GetLogger("BUTTONLog");
        public static readonly NLog.Logger GRLog = NLog.LogManager.GetLogger("GRLog");
        public static readonly NLog.Logger Joblog = NLog.LogManager.GetLogger("JOBLog");
        public static readonly NLog.Logger Splitlog = NLog.LogManager.GetLogger("SPLITLog");

        Form_InfoBoard InfoBoard = new Form_InfoBoard();
        public static SaveLog LogSave = new SaveLog();

        List<Dictionary<string, string>> cust = new List<Dictionary<string, string>>();
        List<Dictionary<string, string>> selectCust = new List<Dictionary<string, string>>();

        public class SaveLog
        {
            public delegate void EvtInsertLog(string type, string msg);
            public event EvtInsertLog InsertLogEvent;

            //public ListBox lb_SysLog = new ListBox();

            public SaveLog()
            {

            }

            public void Save(string LogType, string MsgType, string msg)
            {
                NLog.Logger logger = null;
                string jobType = "LOG";

                switch (LogType.ToUpper())
                {
                    case "SLOG": // Sequence Log
                        logger = Form_Sort.Slog;
                        break;
                    case "WLOG": // Web Service Log
                        logger = Form_Sort.Wlog;
                        break;
                    case "DLOG": // Database Log
                        logger = Form_Sort.Dlog;
                        break;
                    case "BLOG": // Button Log
                        logger = Form_Sort.Blog;
                        break;
                    case "READERLOG": // Reader Log
                        logger = Form_Sort.ReaderLog;
                        break;
                    case "INKJETLOG": // Inkjet Log
                        logger = Form_Sort.InkjetLog;
                        break;
                    case "JOBLOG":
                        jobType = "JOB";
                        logger = Form_Sort.Joblog;
                        break;
                    case "SPLITLOG":
                        logger = Form_Sort.Splitlog;
                        break;
                    default:
                        break;
                }

                if (logger != null)
                {
                    switch (MsgType.ToUpper())
                    {
                        case "INFO":
                            logger.Info(msg);
                            break;
                        case "DEBUG":
                            logger.Debug(msg);
                            break;
                        case "ERROR":
                            logger.Error(msg);
                            break;
                        default:
                            break;
                    }
                }

                InsertLogEvent?.Invoke(jobType, msg);
            }
        }

        public enum SecondLabel
        {
            LineHeight = 73,
            StartHeight = 137,
            LotStartWidth = 30,
            QTYStartWidth = 230,

            QRStartWidth = 335,
            QRStartHeight = 105,

            LotStartWidth2 = 415,
            QTYStartWidth2 = 620,

            QRStartWidth2 = 720,
        }

        #region VirtualKey 
        public enum VKeys : int
        {
            VK_LBUTTON = 0x01, //Left mouse button 
            VK_RBUTTON = 0x02, //Right mouse button 
            VK_CANCEL = 0x03, //Control-break processing 
            VK_MBUTTON = 0x04, //Middle mouse button (three-button mouse) 
            VK_BACK = 0x08, //BACKSPACE key 
            VK_TAB = 0x09, //TAB key 
            VK_CLEAR = 0x0C, //CLEAR key 
            VK_RETURN = 0x0D, //ENTER key 
            VK_SHIFT = 0x10, //SHIFT key 
            VK_CONTROL = 0x11, //CTRL key 
            VK_MENU = 0x12, //ALT key 
            VK_PAUSE = 0x13, //PAUSE key 
            VK_CAPITAL = 0x14, //CAPS LOCK key 
            VK_HANGUL = 0x15,
            VK_ESCAPE = 0x1B, //ESC key 
            VK_SPACE = 0x20, //SPACEBAR 
            VK_PRIOR = 0x21, //PAGE UP key 
            VK_NEXT = 0x22, //PAGE DOWN key 
            VK_END = 0x23, //END key 
            VK_HOME = 0x24, //HOME key 
            VK_LEFT = 0x25, //LEFT ARROW key 
            VK_UP = 0x26, //UP ARROW key 
            VK_RIGHT = 0x27, //RIGHT ARROW key 
            VK_DOWN = 0x28, //DOWN ARROW key 
            VK_SELECT = 0x29, //SELECT key 
            VK_PRINT = 0x2A, //PRINT key 
            VK_EXECUTE = 0x2B, //EXECUTE key 
            VK_SNAPSHOT = 0x2C, //PRINT SCREEN key 
            VK_INSERT = 0x2D, //INS key 
            VK_DELETE = 0x2E, //DEL key 
            VK_HELP = 0x2F, //HELP key 
            VK_0 = 0x30, //0 key 
            VK_1 = 0x31, //1 key 
            VK_2 = 0x32, //2 key 
            VK_3 = 0x33, //3 key 
            VK_4 = 0x34, //4 key 
            VK_5 = 0x35, //5 key
            VK_6 = 0x36, //6 key 
            VK_7 = 0x37, //7 key 
            VK_8 = 0x38, //8 key 
            VK_9 = 0x39, //9 key 
            VK_A = 0x41, //A key 
            VK_B = 0x42, //B key 
            VK_C = 0x43, //C key 
            VK_D = 0x44, //D key 
            VK_E = 0x45, //E key 
            VK_F = 0x46, //F key 
            VK_G = 0x47, //G key 
            VK_H = 0x48, //H key 
            VK_I = 0x49, //I key 
            VK_J = 0x4A, //J key 
            VK_K = 0x4B, //K key 
            VK_L = 0x4C, //L key 
            VK_M = 0x4D, //M key 
            VK_N = 0x4E, //N key 
            VK_O = 0x4F, //O key 
            VK_P = 0x50, //P key 
            VK_Q = 0x51, //Q key 
            VK_R = 0x52, //R key 
            VK_S = 0x53, //S key 
            VK_T = 0x54, //T key 
            VK_U = 0x55, //U key 
            VK_V = 0x56, //V key 
            VK_W = 0x57, //W key 
            VK_X = 0x58, //X key 
            VK_Y = 0x59, //Y key 
            VK_Z = 0x5A, //Z key 
            VK_NUMPAD0 = 0x60, //Numeric keypad 0 key 
            VK_NUMPAD1 = 0x61, //Numeric keypad 1 key 
            VK_NUMPAD2 = 0x62, //Numeric keypad 2 key 
            VK_NUMPAD3 = 0x63, //Numeric keypad 3 key 
            VK_NUMPAD4 = 0x64, //Numeric keypad 4 key 
            VK_NUMPAD5 = 0x65, //Numeric keypad 5 key 
            VK_NUMPAD6 = 0x66, //Numeric keypad 6 key 
            VK_NUMPAD7 = 0x67, //Numeric keypad 7 key 
            VK_NUMPAD8 = 0x68, //Numeric keypad 8 key 
            VK_NUMPAD9 = 0x69, //Numeric keypad 9 key 
            VK_SEPARATOR = 0x6C, //Separator key 
            VK_SUBTRACT = 0x6D, //Subtract key 
            VK_DECIMAL = 0x6E, //Decimal key 
            VK_DIVIDE = 0x6F, //Divide key 
            VK_F1 = 0x70, //F1 key 
            VK_F2 = 0x71, //F2 key 
            VK_F3 = 0x72, //F3 key 
            VK_F4 = 0x73, //F4 key 
            VK_F5 = 0x74, //F5 key 
            VK_F6 = 0x75, //F6 key 
            VK_F7 = 0x76, //F7 key 
            VK_F8 = 0x77, //F8 key 
            VK_F9 = 0x78, //F9 key 
            VK_F10 = 0x79, //F10 key 
            VK_F11 = 0x7A, //F11 key 
            VK_F12 = 0x7B, //F12 key 
            VK_SCROLL = 0x91, //SCROLL LOCK key 
            VK_LSHIFT = 0xA0, //Left SHIFT key 
            VK_RSHIFT = 0xA1, //Right SHIFT key 
            VK_LCONTROL = 0xA2, //Left CONTROL key 
            VK_RCONTROL = 0xA3, //Right CONTROL key 
            VK_LMENU = 0xA4, //Left MENU key 
            VK_RMENU = 0xA5, //Right MENU key 
            VK_PLAY = 0xFA, //Play key 
            VK_ZOOM = 0xFB, //Zoom key 
        }
        #endregion




        public struct stAmkor_Label
        {
            public string Lot;
            public string DCC;
            public string Device;
            public string DQTY;
            public string WQTY;
            public string AMKOR_ID;
            public string CUST;
            public string Wafer_ID;
        }

        public struct st2ndSumLabelInfo
        {
            public string Lot;
            public string DCC;
            public string DEV;
            public string QTY;
            public string WFTQTY;
            public string AmkorID;
        }

        public struct stWaferReturnWebInfo
        {
            public string CustCode;
            public string Status;
            public string ReturnNum;
            public string InputDate;
            public string RequestDate;
            public string UserID;
            public int BoxQty;
            public string Remark;

            public void SetData(string cust, string st, string returncode, string indate, string redate, string id, int qty, string remark)
            {
                CustCode = cust;
                Status = st;
                ReturnNum = returncode;
                InputDate = indate;
                RequestDate = redate;
                UserID = id;
                BoxQty = qty;
                Remark = remark;
            }
        }

        public struct stWaferReturnExcelInfo
        {
            public int cust;
            public string ReturnNum;
            public string Seq;
            public string PDL;
            public string DeviceName;
            public string LotNum;
            public string Dcc;
            public int DsQty;
            public int ReturnQty;
            public string Remark;
            public string Loc;
            public string SL;

            public void Setdata(string returnNum, string seq, string pdl, string deviceName, string lotNum, string dcc, int dsQty, int returnQty, string remark, string loc, string sl, string custcode)
            {

                ReturnNum = returnNum;
                Seq = seq;
                PDL = pdl;
                DeviceName = deviceName;
                LotNum = lotNum;
                Dcc = dcc;
                DsQty = dsQty;
                ReturnQty = returnQty;
                Remark = remark;
                Loc = loc;
                SL = sl;
                cust = int.Parse(custcode);
            }
        }

        public struct stWaferReturnInfo
        {
            public stWaferReturnWebInfo WebInfo;
            public List<stWaferReturnExcelInfo> ExcelInfo;

            public void init()
            {
                WebInfo.SetData("", "", "", "", "", "", -1, "");
                ExcelInfo = new List<stWaferReturnExcelInfo>();
            }

            public void ExcelInfoInit()
            {
                ExcelInfo = new List<stWaferReturnExcelInfo>();
            }


            public void AddExcelInfo(stWaferReturnExcelInfo info)
            {
                if (ExcelInfo == null)
                    ExcelInfo = new List<stWaferReturnExcelInfo>();

                ExcelInfo.Add(info);
            }


        }

        public List<stWaferReturnInfo> WaferReturnInfo = new List<stWaferReturnInfo>();

        public const string ZPL_START = "^XA";
        public const string ZPL_END = "^XZ";

        public List<stAmkor_Label> label_list = new List<stAmkor_Label>();
        public List<string> split_log_lowdata = new List<string>();
        public List<string> split_log_cust = new List<string>();
        public List<string> split_log_Linecode = new List<string>();
        private string split_log_input_return_val = "";

        Form_InputEmpNum inputEmpNum = new Form_InputEmpNum();

        Form_Progress Frm_Process = new Form_Progress();

        public string strExcutionPath = "", strWorkFileName = "", strWorkCust = "";
        string strSelDevice = "";
        public static string strNewLotname = "", strPrintName = "";
        public static bool bPrintUse = false;
        public static int nProcess = 0, nResult = 0;
        public static string strValDevice = "", strValLot = "", strValDcc = "", strValWfrcount = "", strValReadfile = "", strWSN = "";
        public static string strGR_Device = "", strGR_Lot = "", strGR_AmkorID = "";
        public static int nValDiettl = 0, nValDieQty = 0, nValWfrttl = 0, nValWfrQty = 0, nLabelcount = 0, nLabelttl = 0;
        public static bool bupdate = false, bRun = false, bGridViewUpdate = false, bunprinted_device = false, bGRrun = false;
        public static string[] strSelBillno = new string[20] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        public static string strSelCust = "", strSelBill = "", strInputBill = "", strSelJobName = "", strSelLPN;

        public int TTLWafer = 0;
        public int TTLWaferCnt = 0;

        public int AmkorLabelCnt = 1;

        public int real_index = -1;

        private int tot_die = -1, tot_wfr = -1, tot_lots = -1;
        private int com_die = -1, com_wfr = -1, com_lots = -1;

        public bool SecondPrinterMode = false;

        const string PRD_AutoGRConfirm = "aak1ws01";
        const string TEST_AutoGRConfirm = "10.101.1.37:9080";
        const string PRD_MES = "10.101.14.130:8180";
        const string TEST_MES = "10.101.5.130:8980";
        bool btimeOut = false;

        private Color ShelfCompleteColor = Color.BlueViolet;
        private Color ShelfValidationCompColor = Color.Blue;

        SpeechSynthesizer speech = new SpeechSynthesizer();

        Form_Print Frm_Print = new Form_Print();

        public Form_Sort()
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath);

            strExcutionPath = di.ToString();

            InitializeComponent();
        }

        public void Fnc_Init()
        {
            dataGridView_Lot.DefaultCellStyle.SelectionBackColor = Color.Yellow;    // 2021-10-28 선택 셀 배경색 수정
            dataGridView_Lot.DefaultCellStyle.SelectionForeColor = Color.Black;     // 2021-10-28 선택 셀 글자         색 수정
            tabControl_Sort.SelectedIndex = 0;
            comboBox_mode.SelectedIndex = -1;



            strPrintName = Form_Print.strPrinterName;
            bPrintUse = Form_Print.bPrintUse;

            Fnc_Get_Information();

            timer1.Start();

            Frm_Process.Owner = this;
        }

        public void Fnc_Get_Information()
        {
            //var dt_list = BankHost_main.Host.Host_Get_BCRFormat();
            cust = WAS2CUST(GetWebServiceData($"http://10.131.10.84:8080/api/diebank/bcr-master/k4/json"));

            if (cust.Count == 0)
                return;

            string strCust = "", strName = "";

            comboBox_cust.Items.Clear();
            comboBox_Name.Items.Clear();
            comboBox_inch.Items.Clear();

            comboBox_Name.Items.Add("모델명을 입력 하세요!");

            for (int n = 0; n < cust.Count; n++)
            {
                //WorkInfo AWork = new WorkInfo();

                //AWork.strCust =         cust[n]["CUST_CODE"].ToString(); AWork.strCust = AWork.strCust.Trim();
                //AWork.strBank =         cust[n]["BANK_NO"].ToString(); AWork.strBank = AWork.strBank.Trim();
                //AWork.strDevicePos =    cust[n]["DEVICE"].ToString(); AWork.strDevicePos = AWork.strDevicePos.Trim();
                //AWork.strLotidPos =     cust[n]["LOTID"].ToString(); AWork.strLotidPos = AWork.strLotidPos.Trim();
                //AWork.strLotDigit =     cust[n]["LOT_DIGIT"].ToString(); AWork.strLotDigit = AWork.strLotDigit.Trim();
                //AWork.strQtyPos =       cust[n]["WFR_QTY"].ToString(); AWork.strQtyPos = AWork.strQtyPos.Trim();
                //AWork.strSPR =          cust[n]["SPR"].ToString(); AWork.strSPR = AWork.strSPR.Trim();
                //AWork.strMultiLot =     cust[n]["MULTI_LOT"].ToString(); AWork.strMultiLot = AWork.strMultiLot.Trim();
                //AWork.strModelName =    cust[n]["NAME"].ToString(); AWork.strModelName = AWork.strModelName.Trim();
                //AWork.strMtlType =      cust[n]["MTL_TYPE"].ToString(); AWork.strMtlType = AWork.strMtlType.Trim();
                //AWork.strLot2Wfr =      cust[n]["LOT2WFR"].ToString(); AWork.strLot2Wfr = AWork.strLot2Wfr.Trim();
                //AWork.strWSN =          cust[n]["WSN"].ToString(); AWork.strWSN = AWork.strWSN.Trim();
                //AWork.strExcelOut =     cust[n]["EXCEL_OUT"].ToString(); AWork.strExcelOut = AWork.strExcelOut.Trim();

                if (strCust != cust[n]["CUST_CODE"].ToString() && cust[n]["USE"].ToString() == "Y")
                {
                    //strCust = AWork.strCust;

                    int ncount = comboBox_cust.Items.Count;

                    bool bAdd = false;

                    for (int i = 0; i < ncount; i++)
                    {
                        string str = comboBox_cust.Items[i].ToString();
                        if (str == strCust)
                            bAdd = true;
                    }

                    if (!bAdd)
                    {
                        comboBox_cust.Items.Add(cust[n]["CUST_CODE"].ToString());
                    }
                }
                else
                {

                }

                if (strName != cust[n]["CUST_NAME"].ToString() && cust[n]["USE"].ToString() == "Y")
                {
                    strName = cust[n]["CUST_NAME"].ToString();
                    int ncount = comboBox_Name.Items.Count;

                    bool bAdd = false;
                    for (int i = 1; i < ncount; i++)
                    {
                        string str = comboBox_Name.Items[i].ToString();
                        if (str == strName)
                            bAdd = true;
                    }
                    if (!bAdd)
                    {
                        if (BankHost_main.nMaterial_type == 1)
                        {
                            if (cust[n].ContainsKey("WAFER_TYPE") == true)
                            {
                                if (cust[n]["WAFER_TYPE"].ToString() == "FOSB")
                                    comboBox_Name.Items.Add(strName);
                            }
                            else
                                comboBox_Name.Items.Add(strName);
                        }
                        else
                        {
                            if (cust[n].ContainsKey("WAFER_TYPE") == true)
                            {
                                if (cust[n]["WAFER_TYPE"].ToString() != "FOSB")
                                    comboBox_Name.Items.Add(strName);
                            }
                            else
                                comboBox_Name.Items.Add(strName);
                        }
                    }
                }
            }
        }

        private List<Dictionary<string, string>> WAS2CUST(string data)
        {
            List<Dictionary<string, string>> cust = new List<Dictionary<string, string>>();
            string[] s = data.Split(new string[] { "},{" }, StringSplitOptions.None);
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    s[i] = s[i].Replace("\"", "");
                    s[i] = s[i].Replace("{", "");
                    s[i] = s[i].Replace("[", "");
                    s[i] = s[i].Replace("]", "");
                    s[i] = s[i].Replace("SPLITER:,", "SPLITER:COMMA");

                    foreach (string t in s[i].Split(','))
                    {
                        if (t != "")
                            temp.Add(t.Split(':')[0], t.Split(':')[1] == "COMMA" ? "," : t.Split(':')[1]);
                    }
                    cust.Add(temp);
                }

                return cust;
            }
            catch (Exception ex)
            {
                return new List<Dictionary<string, string>>();
            }
            return cust;
        }


        public void Fnc_Get_Information_Model(string strCust, ComboBox NameBox)
        {
            List<Dictionary<string, string>> cust = WAS2CUST(GetWebServiceData($"http://10.131.10.84:8080/api/diebank/bcr-master/k4/json?CUST_CODE={strCust}"));

            //var dt_list = BankHost_main.Host.Host_Get_BCRFormat();

            if (cust.Count == 0)
                return;

            //if (dt_list.Rows.Count == 0)
            //    return;

            string strName = "";
            WorkInfo AWork;

            NameBox.Items.Clear();
            NameBox.Items.Add("모델명을 입력 하세요!");

            selectCust = cust.Cast<Dictionary<string, string>>().Where(r => r["CUST_CODE"].ToString() == strCust).ToList();

            for (int i = 0; i < selectCust.Count; i++)
            {
                AWork = new WorkInfo();

                AWork.strCust = selectCust[i]["CUST_CODE"].ToString(); AWork.strCust = AWork.strCust.Trim();
                AWork.strModelName = selectCust[i]["CUST_NAME"].ToString(); AWork.strModelName = AWork.strModelName.Trim();

                if (strCust == AWork.strCust)
                {
                    if (strName != AWork.strModelName && cust[i]["USE"].ToString() == "Y")
                    {
                        strName = AWork.strModelName;
                        int ncount = NameBox.Items.Count;

                        bool bAdd = false;
                        for (int j = 1; j < ncount; j++)
                        {
                            string str = NameBox.Items[j].ToString();
                            if (str == strName)
                                bAdd = true;
                        }

                        if (!bAdd)
                        {
                            if (BankHost_main.nMaterial_type == 1)
                            {
                                if (selectCust[i].ContainsKey("WAFER_TYPE") == true)
                                {
                                    if (selectCust[i]["WAFER_TYPE"] == "FOSB")
                                        NameBox.Items.Add(strName);
                                }
                                else
                                {
                                    NameBox.Items.Add(strName);
                                }
                            }
                            else
                            {
                                if (selectCust[i].ContainsKey("WAFER_TYPE") == true)
                                {
                                    if (selectCust[i]["WAFER_TYPE"] != "FOSB")
                                        NameBox.Items.Add(strName);
                                }
                                else
                                {
                                    NameBox.Items.Add(strName);
                                }
                            }

                        }
                    }
                }

            }

            for (int n = 0; n < cust.Count; n++)
            {
                //WorkInfo AWork = new WorkInfo();
                //if(cust[n]["CUST_CODE"].ToString() == strCust)
                //{

                //}
                //AWork.strCust = dt_list.Rows[n]["CUST"].ToString(); AWork.strCust = AWork.strCust.Trim();
                //AWork.strBank = dt_list.Rows[n]["BANK_NO"].ToString(); AWork.strBank = AWork.strBank.Trim();
                //AWork.strDevicePos = dt_list.Rows[n]["DEVICE"].ToString(); AWork.strDevicePos = AWork.strDevicePos.Trim();
                //AWork.strLotidPos = dt_list.Rows[n]["LOTID"].ToString(); AWork.strLotidPos = AWork.strLotidPos.Trim();
                //AWork.strLotDigit = dt_list.Rows[n]["LOT_DIGIT"].ToString(); AWork.strLotDigit = AWork.strLotDigit.Trim();
                //AWork.strQtyPos = dt_list.Rows[n]["WFR_QTY"].ToString(); AWork.strQtyPos = AWork.strQtyPos.Trim();
                //AWork.strSPR = dt_list.Rows[n]["SPR"].ToString(); AWork.strSPR = AWork.strSPR.Trim();
                //AWork.strMultiLot = dt_list.Rows[n]["MULTI_LOT"].ToString(); AWork.strMultiLot = AWork.strMultiLot.Trim();
                //AWork.strModelName = dt_list.Rows[n]["NAME"].ToString(); AWork.strModelName = AWork.strModelName.Trim();
                //AWork.strMtlType = dt_list.Rows[n]["MTL_TYPE"].ToString(); AWork.strMtlType = AWork.strMtlType.Trim();
                //AWork.strLot2Wfr = dt_list.Rows[n]["LOT2WFR"].ToString(); AWork.strLot2Wfr = AWork.strLot2Wfr.Trim();
                //AWork.strTTLWFR = dt_list.Rows[n]["TTLWFR"].ToString().Trim();

                //if (strCust == AWork.strCust)
                //{
                //    if (strName != AWork.strModelName)
                //    {
                //        strName = AWork.strModelName;
                //        int ncount = comboBox_Name.Items.Count;

                //        bool bAdd = false;
                //        for (int i = 1; i < ncount; i++)
                //        {
                //            string str = comboBox_Name.Items[i].ToString();
                //            if (str == strName)
                //                bAdd = true;
                //        }

                //        if (!bAdd)
                //        {
                //            if (BankHost_main.nMaterial_type == 1)
                //            {
                //                if (AWork.strMtlType == "FOSB")
                //                    comboBox_Name.Items.Add(strName);
                //            }
                //            else
                //            {
                //                if (AWork.strMtlType != "FOSB")
                //                    comboBox_Name.Items.Add(strName);
                //            }

                //        }
                //    }
                //}
            }
        }

        public string PostWebServiceData(string url)
        {
            string responseText = string.Empty;
            LogSave.Save("WLOG", "INFO", url);
            try
            {
                byte[] arr = new byte[10];

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(Properties.Settings.Default.USER_NAME + ":" + Properties.Settings.Default.USER_PW)));
                //request.Headers.Add("Authorization", "inbound:inbound@123");
                //request.ContentLength = url.Length - 29;

                //byte[] bytes = Encoding.UTF8.GetBytes(url.Substring(29));
                byte[] bytes = Encoding.UTF8.GetBytes(url.Substring(url.IndexOf('?') + 1));
                request.ContentLength = bytes.Length;// url.Length - url.IndexOf('?');
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);

                        responseText = reader.ReadToEnd();
                    }
                }

                return responseText;
            }
            catch (WebException ex)
            {
                string errorMessage = string.Empty;
                LogSave.Save("WLOG", "ERROR", ex.StackTrace);
                LogSave.Save("WLOG", "ERROR", ex.Message);
                if (ex.Response != null)
                {
                    using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                    {
                        Stream dataStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        errorMessage = GetErrorMSG(reader.ReadToEnd());
                        LogSave.Save("WLOG", "ERROR", errorMessage);
                    }
                }

                return errorMessage;
            }
        }

        private static string GetErrorMSG(string msg)
        {
            string res = "";
            msg = msg.Replace("\"", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");

            string[] temp = msg.Split(',');

            Dictionary<string, string> result = new Dictionary<string, string>();
            int bkeyindex = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Split(':').Length < 2)
                {
                    result[temp[bkeyindex].Split(':')[0]] = result[temp[bkeyindex].Split(':')[0]] + " " + temp[i];
                }
                else
                {
                    bkeyindex = i;
                    result.Add(temp[i].Split(':')[0], temp[i].Split(':')[1]);
                }
            }

            return result["MESSAGE"];
        }

        public async Task<string> InsertReelID(string strKey)
        {
            string str = "";

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(strKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/HY"));

                    HttpResponseMessage response = client.GetAsync("").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var contents = await response.Content.ReadAsStringAsync();
                        str = response.ReasonPhrase;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return str;
        }

        public async Task<string> Fnc_RunAsync(string strKey)
        {
            string str = "";

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(strKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/HY"));

                    HttpResponseMessage response = client.GetAsync("").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var contents = await response.Content.ReadAsStringAsync();
                        str = contents;
                    }
                }
            }
            catch (WebException ex)
            {

                throw;
            }

            return str;
        }


        public static string GetWebServiceData(string url)
        {
            string responseText = string.Empty;

            try
            {
                byte[] arr = new byte[10];

                //new frm_InboundMain().SaveLog("WLOG", "INFO","GET : " + url);

                LogSave.Save("WLOG", "INFO", url);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 5000;
                request.Method = "GET";
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(Properties.Settings.Default.USER_NAME + ":" + Properties.Settings.Default.USER_PW)));
                if (url.Contains("ALL") == true) request.Headers.Add("Query-Type", "ALL");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        responseText = reader.ReadToEnd();
                        // do something with the response data
                    }
                }

                LogSave.Save("WLOG", "INFO", responseText);

                //if (url.Contains("diebank/bcr-master") == true)
                //    return "[{\"CUST_CODE\":488,\"CUST_NAME\":\"AVAGO_CHIPBOND\",\"BCR_TYPE\":\"DATAMATRIX\",\"SPLITER\":\";\",\"USE\":\"Y\",\"BCD01\":\"DEVICE\",\"BCD02\":\"\",\"BCD03\":\"LOT\",\"BCD04\":\"QTY\",\"BCD05\":\"\",\"BCD06\":\"\",\"BCD07\":\"\",\"BCD08\":\"\",\"BCD09\":\"\",\"BCD10\":\"\",\"BCD11\":\"\",\"BCD12\":\"\",\"REGISTER\":\"\",\"REG_TIME\":\"2024-02-01T16:01:30\",\"EDITOR\":\"\",\"EDIT_TIME\":\"1900-01-01T00:00:00\",\"REMARK\":\"\",\"ROW_NUM\":1,\"RESULT\":\"SUCCESS\",\"MESSAGE\":\"select\",\"REEL_ID\":\"LOT\"},{\"CUST_CODE\":379,\"CUST_NAME\":\"SKYWORKS_SING\",\"BCR_TYPE\":\"DATAMATRIX\",\"SPLITER\":\"+\",\"USE\":\"Y\",\"BCD01\":\"DEVICE/L1\",\"BCD02\":\"LOT/L1\",\"BCD03\":\"QTY/L1\",\"BCD04\":\"\",\"BCD05\":\"\",\"BCD06\":\"\",\"BCD07\":\"\",\"BCD08\":\"\",\"BCD09\":\"\",\"BCD10\":\"\",\"BCD11\":\"\",\"BCD12\":\"\",\"REGISTER\":\"\",\"REG_TIME\":\"2024-02-02T13:37:08\",\"EDITOR\":\"\",\"EDIT_TIME\":\"1900-01-01T00:00:00\",\"REMARK\":\"\",\"ROW_NUM\":2,\"RESULT\":\"SUCCESS\",\"MESSAGE\":\"select\",\"REEL_ID\":\"LOT\"},{\"CUST_CODE\":1,\"CUST_NAME\":\"ATK\",\"BCR_TYPE\":\"QR\",\"SPLITER\":\";\",\"USE\":\"Y\",\"BCD01\":\"AMKOR_PO\",\"BCD02\":\"\",\"BCD03\":\"\",\"BCD04\":\"BOX_SEQ\",\"BCD05\":\"TOTAL_BOX_CNT\",\"BCD06\":\"INVOICE_QTY\",\"BCD07\":\"\",\"BCD08\":\"VENDOR_LOT\",\"BCD09\":\"INVOICE\",\"BCD10\":\"\",\"BCD11\":\"PACKING_ID\",\"BCD12\":null,\"REGISTER\":\"\",\"REG_TIME\":\"2023-07-05T15:13:26\",\"EDITOR\":\"\",\"EDIT_TIME\":\"2023-07-12T11:37:33\",\"REMARK\":\"\",\"ROW_NUM\":3,\"RESULT\":\"SUCCESS\",\"MESSAGE\":\"select\",\"REEL_ID\":\"LOT\"}]";
                //else
                //return responseText;

                return responseText;
            }
            catch (WebException ex)
            {
                string errorMessage = string.Empty;

                LogSave.Save("WLOG", "ERROR", ex.Message);

                if (ex.Response != null)
                {
                    using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                    {
                        Stream dataStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        errorMessage = reader.ReadToEnd();


                        return errorMessage;
                        //new frm_InboundMain().SaveLog("WLOG","ERROR",errorMessage);
                    }
                }
                else if (ex.Message != "")
                {
                    //frm_Messageboard brd = new frm_Messageboard(ex.Message, Color.Red, Color.Yellow, "", "", "", "OK");
                    //brd.ButtonClickEvent += Brd_ButtonClickEvent1;
                    //brd.ShowDialog();
                }
            }
            return "EMPTY";
        }


        public void Fnc_PrintShow()
        {
            Frm_Print.Hide();
            Frm_Print.Show();
        }

        public void Fnc_PrintHide()
        {
            Frm_Print.Hide();
        }

        public void Fnc_PrintExit()
        {
            Frm_Print.Fnc_Exit();
            Frm_Print = null;
        }

        public void Fnc_Print_Start(AmkorBcrInfo amkorBcrInfo, int nBcrType, bool bAutorun, int nIndex, int nttl)
        {
            if (!Form_Print.bPrintUse)
            {
                if (!bAutorun)
                {
                    label_printstate.Text = "프린트 사용 안함";
                    label_printstate.ForeColor = Color.Red;
                }
                return;
            }

            bool bJudge = false;


            if (BankHost_main.strMultiLot == "YES" && BankHost_main.strCustName != "CHIPBOND_MULTI")
            {
                for (int i = 0; i <= (int.Parse(amkorBcrInfo.strWfrQty) / BankHost_main.LabelAddVal); i++)
                {
                    Frm_Print.Fnc_Print(amkorBcrInfo, nBcrType, i + 1, (int.Parse(amkorBcrInfo.strWfrQty) / BankHost_main.LabelAddVal) + 1);
                }
            }
            else if (BankHost_main.strTTLWFR == "TRUE")
            {
                if (GetAmkorLabelcnt() >= GetNumericValue())
                {
                    string waferttl = "";

                    if (DialogResult.OK == InputBox("Wafer 수량을 입력 하세요", "Wafer 수량을 입력 하세요", ref waferttl))
                    {
                        SetnumeriValue(int.Parse(waferttl));
                        SetAmkorlabelcnt(1);

                        //Amkor_label_Print_Process(textBox1.Text.ToUpper(), AmkorLabelCnt);
                        Frm_Print.Fnc_Print(amkorBcrInfo, 2, GetAmkorLabelcnt(), GetNumericValue());
                    }
                    else
                    {
                        Form_Board b = new Form_Board("Wafer 수량을 입력해야만 합니다.");
                        return;
                    }
                }
                else
                {
                    int cnt = GetAmkorLabelcnt();
                    ++cnt;
                    SetAmkorlabelcnt(cnt);
                    Frm_Print.Fnc_Print(amkorBcrInfo, 2, GetAmkorLabelcnt(), GetNumericValue());
                }
            }
            else
            {
                bJudge = Frm_Print.Fnc_Print(amkorBcrInfo, nBcrType, nIndex, nttl);

                if (SecondPrinterMode == true)
                    PrintSummary(amkorBcrInfo);
            }

            if (bJudge)
            {
                if (!bAutorun)
                {
                    label_printstate.Text = "출력 OK";
                    label_printstate.ForeColor = Color.DarkBlue;
                }
            }
            else
            {
                if (!bAutorun)
                {
                    label_printstate.Text = "출력 NG";
                    label_printstate.ForeColor = Color.Red;
                }
            }
        }
        private void button_sel_Click(object sender, EventArgs e)
        {
            if (!BankHost_main.bVisionConnect)
            {
                string strMsg = string.Format("카메라 연결이 되지 않았습니다.\n\n연결 상태를 확인 하시고 프로그램을 재시작 하세요");
                Frm_Process.Form_Show(strMsg);
                Frm_Process.Form_Display_Warning(strMsg);
                Thread.Sleep(3000);
                Frm_Process.Form_Hide();
                return;
            }

            int nSel = comboBox_mode.SelectedIndex;

            if (nSel == -1)
            {
                string strMsg = string.Format("모드가 선택 되지 않았습니다.\n\n모드를 먼저 선택 하세요");
                Frm_Process.Form_Show(strMsg);
                Frm_Process.Form_Display_Warning(strMsg);
                Thread.Sleep(3000);
                Frm_Process.Form_Hide();
                return;
            }

            //////////////////
            ///작업자 사번 입력 
            Form_Input Frm_Input = new Form_Input();

            Frm_Input.Fnc_Init(nSel);
            Frm_Input.ShowDialog();

            if (BankHost_main.strOperator == "")
                return;

            label_opinfo.Text = BankHost_main.strOperator;

            if (nSel == 0 || nSel == 1) //Auto GR
            {
                /*
                if (!BankHost_main.bHost_connect)
                    return;

                string strMsg = string.Format("\n\n작업 정보를 가져 옵니다.");
                Frm_Process.Form_Show(strMsg);

                var taskResut = BankHost_main.Host.Fnc_GetLotInformation();

                try
                {
                    strMsg = string.Format("\n\n작업 정보를 분석 합니다.");
                    Frm_Process.Form_Display(strMsg);

                    if (taskResut.Status.ToString() == "Faulted")
                    {
                        strMsg = string.Format("\n\n작업 정보를 가져오는데 실패 하였습니다.");
                        Frm_Process.Form_Display_Warning(strMsg);
                        Thread.Sleep(3000);
                        Frm_Process.Form_Hide();

                        return;
                    }

                    Fnc_Get_Worklist(taskResut.Result);
                }
                catch (Exception ex)
                {
                    string str = string.Format("{0}", ex);

                    strMsg = string.Format("\n\n작업 정보를 가져오는데 실패 하였습니다.");
                    Frm_Process.Form_Display_Warning(strMsg);

                    Thread.Sleep(3000);
                    Frm_Process.Form_Hide();
                }
                */
            }
            else /// 파일 선택
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if (nSel == 1)
                {
                    openFileDialog.DefaultExt = ".txt";
                    openFileDialog.ShowDialog();
                    string strName = openFileDialog.FileName;
                    if (strName.Length > 0)
                    {
                        string str = strName.Substring(strName.Length - 3, 3);
                        if (str != "txt")
                        {
                            MessageBox.Show("JOB 파일이 아닙니다. 로드 실패!");
                            return;
                        }

                        string[] strSplit = strName.Split('\\');
                        int nLength = strSplit.Length;

                        strWorkFileName = strSplit[nLength - 1].Substring(0, strSplit[nLength - 1].Length - 4);
                        strWorkFileName = strWorkFileName.Trim();
                        Fnc_WorkView(strWorkFileName);
                    }
                }
                else if (nSel == 2)
                {
                    openFileDialog.DefaultExt = ".xlsx";
                    openFileDialog.ShowDialog();

                    string strName = openFileDialog.FileName;
                    if (strName.Length > 0)
                    {
                        string str = strName.Substring(strName.Length - 4, 4);
                        if (str != "xlsx")
                        {
                            MessageBox.Show("액셀 파일이 아닙니다. 로드 실패!");
                            return;
                        }

                        string strSavepath = "", strSetFileName = "";

                        string[] strSplit = strName.Split('\\');
                        int nLength = strSplit.Length;

                        strWorkFileName = strSplit[nLength - 1].Substring(0, strSplit[nLength - 1].Length - 5);
                        strWorkFileName = strWorkFileName.Trim();
                        strSetFileName = strWorkFileName + ".txt";

                        strSavepath = strExcutionPath + "\\Work\\" + strSetFileName;

                        /////.ini 파일 만들기
                        System.IO.FileInfo fi = new System.IO.FileInfo(strSavepath);

                        if (fi.Exists)
                        {
                            Fnc_WorkView(strWorkFileName);

                        }
                        else
                            Fnc_ExcelDownload2(strName);
                    }
                }

                BankHost_main.Host.Host_Set_Ready(BankHost_main.strEqid, "WAIT", "");
                BankHost_main.nWorkMode = 0;
                BankHost_main.strWork_Lotinfo = "";
            }
            label_cust.Text = strSelCust;
            Fnc_Get_Information_Model(strSelCust, comboBox_Name);
        }

        public int Fnc_Get_Worklist(string strData)
        {
            /////////////////////////////////////////////////
            ///파일 이름: JOB\CUST_JOBNO_DATE , ex) WORK\JOB_102_2008060835.txt
            ///파일 이름 설정
            string[] strList = strData.Split('\n'); //index 1 부터 데이터 받아야 함.
            int nArryLength = strList.Length;

            string[] strCol = strList[0].Split('\t');
            int nColcnt = strCol.Length;

            List<StorageData> list = new List<StorageData>();

            int nCount = 0;

            string strMsg = string.Format("\n\n작업 정보를 분석 중 입니다.");
            Frm_Process.Form_Show(strMsg);

            for (int i = 1; i < nArryLength; i++)
            {
                nCount++;
                strMsg = string.Format("\n\n데이터 Read {0} / {1}", nCount, nArryLength - 1);
                Frm_Process.Form_Display(strMsg);

                string[] strJobInfo = strList[i].Split('\t');

                StorageData data = new StorageData();

                for (int j = 0; j < nColcnt; j++)
                {
                    var strType = strJobInfo[j];

                    string str = "";
                    if (strType != null)
                    {
                        if (j != 4)
                            str = strType.ToString();
                        else
                        {
                            str = strType.ToString();

                            DateTime conv = DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                            //DateTime conv = DateTime.FromOADate(double.Parse(strType));
                            str = string.Format("{0}/{1}/{2}", conv.Year, conv.Month, conv.Day);
                        }
                    }

                    if (j == 0) //Cust
                    {
                        if (str == null)
                            str = "";

                        str = str.Trim();
                        data.Cust = str;
                        strWorkCust = str;
                    }
                    else if (j == 3) //Device
                    {
                        str = str.Trim();
                        data.Device = str;
                    }
                    else if (j == 1) //Lot#
                    {
                        str = str.Trim();
                        data.Lot = str;
                    }
                    else if (j == 2)//DCC
                    {
                        str = str.Trim();
                        data.Lot_Dcc = str;
                    }
                    else if (j == 5) //DieQty
                    {
                        str = str.Trim();
                        //string strnQty = string.Format("{0:0,0}", Int32.Parse(str));
                        data.Rcv_Qty = str;
                    }
                    else if (j == 6) //Wafer Qty
                    {
                        str = str.Trim();
                        //data.Rcv_WQty = str;
                        data.Rcv_WQty = "0";
                        data.Default_WQty = str;
                    }
                    else if (j == 4) //RCV date
                    {
                        str = str.Trim();
                        data.Rcvddate = str;
                    }
                    else if (j == 9) //Lot Type
                    {
                        str = str.Trim();
                        data.Lot_type = str;
                    }
                    else if (j == 7) //Bill
                    {
                        str = str.Trim();
                        data.Bill = str;
                    }
                    else if (j == 8) //Amkor id
                    {
                        str = str.Trim();
                        data.Amkorid = str;
                    }
                    else if (j == 10) //wfr lot
                    {
                        str = str.Trim();
                        data.Wafer_lot = str;
                    }
                    else if (j == 11) //coo
                    {
                        str = str.Trim();
                        data.strCoo = str;
                    }
                }
                list.Add(data);
            }

            nCount = 0;

            list.Sort(CompareStorageData);

            string strSavepath = "", strSetFileName = "", strSetFolder = "";

            string strYear = DateTime.Now.Year.ToString().Substring(2, 2);
            strSetFolder = string.Format("JOB_{0}_{1:00}{2:00}{3:00}{4:00}{5:00}", strWorkCust, strYear, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
            strSetFileName = strSetFolder + ".txt";
            strWorkFileName = strSetFolder;
            strSavepath = strExcutionPath + "\\Work\\" + strSetFileName;

            string sDirFileNamePath = "", sDirDeviceNamePath = "";
            sDirFileNamePath = strExcutionPath + "\\Work\\" + strSetFolder;

            /////.txt 파일 만들기
            System.IO.FileInfo fi = new System.IO.FileInfo(strSavepath);

            if (fi.Exists)
            {
                File.Delete(strSavepath);
                /////폴더,폴더, 파일 삭제
                DirectoryInfo dir = new DirectoryInfo(sDirFileNamePath);
                dir.Delete(true);

            }
            ///파일 이름 폴더 만들기            
            DirectoryInfo di = new DirectoryInfo(sDirFileNamePath);
            if (di.Exists == false)
            {
                di.Create();
            }
            ////////////////////////////////////////            

            string strDevicecheck = "";
            foreach (var item in list)
            {
                item.state = "Waiting";
                item.strop = "";
                item.Die_Qty = "0";
                item.strGRstatus = "Ready";

                string strTxtline = item.Cust + "\t" + item.Device + "\t" + item.Lot + "\t" + item.Lot_Dcc + "\t" + item.Rcv_Qty + "\t" + item.Die_Qty + "\t" +
                    item.Rcv_WQty + "\t" + item.Rcvddate + "\t" + item.Lot_type + "\t" + item.Bill + "\t" + item.Amkorid + "\t" + item.Wafer_lot + "\t" + item.strCoo + "\t" +
                    item.state + "\t" + item.strop + "\t" + item.strGRstatus + "\t" + item.Default_WQty + "\t" + item.shipment + $"\t{item.ReelID}\t{item.ReelDCC}\t";

                if (strDevicecheck != item.Device)
                {
                    Fnc_WriteFile(strSavepath, item.Device);
                    strDevicecheck = item.Device;
                }

                /////////////////////////////////////Device 폴더 생성
                sDirDeviceNamePath = sDirFileNamePath + "\\" + item.Device;
                DirectoryInfo diinfo = new DirectoryInfo(sDirDeviceNamePath);
                if (diinfo.Exists == false)
                {
                    diinfo.Create();
                }
                diinfo = null;
                /////////////////////////////////////File 저장
                string strLotsavepath = sDirDeviceNamePath + "\\" + item.Device + ".txt";
                Fnc_WriteFile(strLotsavepath, strTxtline);
                ////////////////////////////////////

                nCount++;
                strMsg = string.Format("\n\n 작업 준비 중 입니다. {0} / {1}", nCount, nArryLength - 1);
                Frm_Process.Form_Display(strMsg);

                System.Windows.Forms.Application.DoEvents();
            }

            int nReturn = Fnc_WorkView(strWorkFileName);

            Frm_Process.Form_Display("\n작업을 마침니다.");
            Frm_Process.Hide();

            return nReturn;
        }

        public int Fnc_Get_Worklist_2(string strData)
        {
            /////////////////////////////////////////////////
            ///파일 이름: JOB\CUST_JOBNO_DATE , ex) WORK\JOB_102_2008060835.txt
            ///파일 이름 설정
            string[] strList = strData.Split('\n'); //index 1 부터 데이터 받아야 함.
            int nArryLength = strList.Length;

            string[] strCol = strList[0].Split('\t');
            int nColcnt = strCol.Length;

            List<StorageData> list = new List<StorageData>();

            int nCount = 0;

            string strMsg = string.Format("\n\n작업 정보를 분석 중 입니다.");
            Frm_Process.Form_Show(strMsg);

            for (int i = 1; i < nArryLength; i++)
            {
                nCount++;
                strMsg = string.Format("\n\n데이터 Read {0} / {1}", nCount, nArryLength - 1);
                Frm_Process.Form_Display(strMsg);

                string[] strJobInfo = strList[i].Split('\t');

                StorageData data = new StorageData();

                for (int j = 0; j < nColcnt; j++)
                {
                    var strType = strJobInfo[j];

                    string str = "";
                    if (strType != null)
                    {
                        if (j != 4)
                            str = strType.ToString();
                        else
                        {
                            str = strType.ToString();

                            DateTime conv = DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                            //DateTime conv = DateTime.FromOADate(double.Parse(strType));
                            str = string.Format("{0}/{1}/{2}", conv.Year, conv.Month, conv.Day);
                        }
                    }

                    if (j == 0) //Cust
                    {
                        if (str == null)
                            str = "";

                        str = str.Trim();
                        data.Cust = str;
                        strWorkCust = str;
                    }
                    else if (j == 3) //Device
                    {
                        str = str.Trim();
                        data.Device = str;
                    }
                    else if (j == 1) //Lot#
                    {
                        str = str.Trim();
                        data.Lot = str;
                    }
                    else if (j == 2)//DCC
                    {
                        str = str.Trim();
                        data.Lot_Dcc = str;
                    }
                    else if (j == 5) //DieQty
                    {
                        str = str.Trim();
                        //string strnQty = string.Format("{0:0,0}", Int32.Parse(str));
                        data.Rcv_Qty = str;
                    }
                    else if (j == 6) //Wafer Qty
                    {
                        str = str.Trim();
                        //data.Rcv_WQty = str;
                        data.Rcv_WQty = "0";
                        data.Default_WQty = str;
                    }
                    else if (j == 4) //RCV date
                    {
                        str = str.Trim();
                        data.Rcvddate = str;
                    }
                    else if (j == 9) //Lot Type
                    {
                        str = str.Trim();
                        data.Lot_type = str;
                    }
                    else if (j == 7) //Bill
                    {
                        str = str.Trim();
                        data.Bill = str;
                    }
                    else if (j == 8) //Amkor id
                    {
                        str = str.Trim();
                        data.Amkorid = str;
                    }
                    else if (j == 10) //wfr lot
                    {
                        str = str.Trim();
                        data.Wafer_lot = str;
                    }
                    else if (j == 11) //coo
                    {
                        str = str.Trim();
                        data.strCoo = str;
                    }
                    else if (j == 12) ////Shipment 추가
                    {
                        str = str.Trim();
                        data.shipment = str;
                    }
                }
                list.Add(data);
            }

            list.Sort(CompareStorageData_Bill);

            dataGridView_worklist.Columns.Clear();
            dataGridView_worklist.Rows.Clear();
            dataGridView_worklist.Refresh();

            dataGridView_worklist.Columns.Add("#", "#");
            dataGridView_worklist.Columns.Add("CUST", "CUST");
            dataGridView_worklist.Columns.Add("DEVICE", "DEVICE");
            dataGridView_worklist.Columns.Add("LOT#", "LOT#");
            dataGridView_worklist.Columns.Add("DCC", "DCC");
            dataGridView_worklist.Columns.Add("DIE_QTY", "DIE_QTY");
            dataGridView_worklist.Columns.Add("WFR TTL", "WFR TTL");
            dataGridView_worklist.Columns.Add("REV_DATE", "REV_DATE");
            dataGridView_worklist.Columns.Add("LOT_TYPE", "LOT_TYPE");
            dataGridView_worklist.Columns.Add("BILL#", "BILL#");
            dataGridView_worklist.Columns.Add("AMKOR_ID", "AMKOR_ID");
            dataGridView_worklist.Columns.Add("WAFER_LOT", "WAFER_LOT");
            dataGridView_worklist.Columns.Add("SHIPMENT", "SHIPMENT");
            dataGridView_worklist.Columns.Add("REELID", "REEL_ID");
            dataGridView_worklist.Columns.Add("REELDCC", "REEL_DCC");


            dataGridView_worklist.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[12].SortMode = DataGridViewColumnSortMode.NotSortable;

            if (list.Count == 0)
            {
                dataGridView_worklist.Columns.Clear();
                dataGridView_worklist.Rows.Clear();
                dataGridView_worklist.Refresh();

                dataGridView_worklist.Columns.Add("데이터가 없습니다. 다시 선택해 주십시오.", "데이터가 없습니다. 다시 선택해 주십시오.");

                System.Windows.Forms.Application.DoEvents();
            }

            nCount = 1;

            foreach (var item in list)
            {
                dataGridView_worklist.Rows.Add(new object[] { nCount, item.Cust, item.Device, item.Lot, item.Lot_Dcc, item.Rcv_Qty, item.Default_WQty, item.Rcvddate,
                    item.Lot_type, item.Bill, item.Amkorid, item.Wafer_lot, item.shipment, item.ReelID, item.ReelDCC });

                nCount++;
            }

            Frm_Process.Form_Display("\n작업을 마침니다.");
            Frm_Process.Hide();

            //Thread insertThread = new Thread(InsertWAS);
            //mesData = strData;
            //insertThread.Start();

            return list.Count;
        }


        StorageData tempData = new StorageData();

        List<StorageData> FailInsertData = new List<StorageData>();
        List<FailURLData> FailWebDatas = new List<FailURLData>();

        private void InsertWebdata(string url)
        {
            string res = GetWebServiceData(url);

            if (res.ToUpper().Contains("OK") || res.ToUpper().Contains("SUCCESS"))
            {
                System.Threading.Thread WebdataRetry = new Thread(retryWebdata);
                WebdataRetry.Start();
            }
            else
            {
                FailURLData faildata = new FailURLData();

                faildata.URL = url;
                faildata.Retry = 0;
                faildata.filaMSG = "";

                FailWebDatas.Add(faildata);
            }
        }

        private void retryWebdata()
        {
            string res = "";

            for (int i = 0; i < FailWebDatas.Count; i++)
            {
                res = GetWebServiceData(FailWebDatas[i].URL);

                if (res.ToUpper().Contains("OK") || res.ToUpper().Contains("SUCCESS"))
                {
                    FailWebDatas.RemoveAt(i);
                    i--;
                }
                else if (res.ToUpper().Contains("FAIL") == true)
                {
                    FailWebDatas[i].Retry += 1;

                    if (FailWebDatas[i].Retry > 5)
                    {
                        writeWebFailData(FailWebDatas[i]);
                        FailWebDatas.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        private void readWebFailData()
        {
            FailWebDatas.Clear();

            string dir = $"{Application.StartupPath}\\FailURL\\";
            string fileName = "FailURL.txt";


            if (File.Exists($"{dir}\\{fileName}") == true)
            {
                string[] temp = System.IO.File.ReadAllLines($"{dir}\\{fileName}");

                foreach (string s in temp)
                {
                    string[] t = s.Split('\t');

                    FailWebDatas.Add(new FailURLData()
                    {
                        URL = t[0],
                        Retry = 0,
                        filaMSG = ""
                    });
                }

                System.IO.File.WriteAllText($"{dir}\\{fileName}", "");
            }
        }

        private void writeWebFailData(FailURLData fdata)
        {
            string dir = $"{Application.StartupPath}\\FailURL\\";
            string fileName = "FailURL.txt";

            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);

            System.IO.StreamWriter st = System.IO.File.AppendText($"{dir}\\{fileName}");

            st.WriteLine(fdata.URL, fdata.filaMSG);
        }

        private void InsertWAS()
        {
            string res = PostWebServiceData($"http://10.131.10.84:8080/api/diebank/gr-info/{Properties.Settings.Default.LOCATION}/json?" +
                $"AMKOR_ID={tempData.Amkorid}&AMKOR_BATACH=&AMKOR_BATCH_PREV_BIZ=&BINDING_NO=&COO={tempData.strCoo}&CUR_MATERIAL_NO=" +
                $"&CUST_CODE={tempData.Cust}&CUST_INFO=&CUST_NAME={BankHost_main.strCustName}&CUSTOMER_LOT_NO={tempData.Lot}&LOT_DCC={tempData.Lot_Dcc}&DEVICE_SCAN=&" +
                $"DIE_BANK_EOH=&DIE_QTY={tempData.Die_Qty}&DS_DT=1900-01-01 00:00.000&FAB_SITE={Properties.Settings.Default.LOCATION}&HAWB={tempData.Bill}&HOLD_COMMENT=&HOLD_STATUS=&" +
                $"INVOICE={tempData.Invoice}&LAST_ISSUE_TIME=&LOCATION=&LOT_TYPE={tempData.Lot_type}&LOT_NO={tempData.Lot}&MATERIAL=&MES_UPLOAD_TIME={DateTime.Now.ToString()}&MES_UPLOAD_ID={BankHost_main.strMESID}" +
                $"&ON_HAND_WAFER_ID=&PDL=&PDL_SEPARATOR={"%2F"}&PLANT={Properties.Settings.Default.LOCATION}&PRICE=&RCV_DATE={tempData.Rcvddate}0&RCV_DIE_QTY=&RCV_WAFER_QTY=" +
                $"&REEL_ID={tempData.ReelID}&REEL_DCC={(tempData.ReelDCC == "" ? "" : tempData.ReelDCC)}&RSLT=&SCAN_TIME=&SIZE=&SOURCE_DEVICE={tempData.Device}&SPECIFIC_DATA=&UNIT=&USER_ID=&VALIDATION_ID=" +
                $"&VALIDATION_SCAN_TIME=&WAFER_INFORM=&WAFER_LOT_NO=&WAFER_QTY=&WAFER_STATUS=&WAFER_TYPE=&HOST_NAME=&BADGE=&OPER_NAME={BankHost_main.strID}");

            if (res.Contains("\"RESULT\":\"SUCCESS\"") == true)
            {
                if (FailInsertData.Count > 0)
                {
                    System.Threading.Thread retryThread = new Thread(insertRetry);
                    retryThread.Start();
                }
            }
            else
            {
                FailInsertData.Add(tempData);
            }

            //data.Cust = strCol[0];                    
            //data.Device = strCol[3];     
            //data.Lot = strCol[1];                
            //data.Lot_Dcc = strCol[2];                
            //data.Rcv_Qty = strCol[5];                
            //data.Default_WQty = strCol[6];                
            //data.Rcvddate = strCol[4];                
            //data.Lot_type = strCol[9];                
            //data.Bill = strCol[7];                
            //data.Amkorid = strCol[8];                
            //data.Wafer_lot = strCol[10];                
            //data.strCoo = strCol[11];                
            //data.shipment = strCol[12];                


        }

        private void insertRetry()
        {
            for (int i = 0; i < FailInsertData.Count; i++)
            {
                string res = PostWebServiceData($"http://10.131.10.84:8080/api/diebank/gr-info/{Properties.Settings.Default.LOCATION}/json?" +
                    $"AMKOR_ID={FailInsertData[i].Amkorid}&AMKOR_BATACH=&AMKOR_BATCH_PREV_BIZ=&BINDING_NO=&COO={FailInsertData[i].strCoo}&CUR_MATERIAL_NO=" +
                    $"&CUST_CODE={FailInsertData[i].Cust}&CUST_INFO=&CUST_NAME={BankHost_main.strCustName}&CUSTOMER_LOT_NO={FailInsertData[i].Lot}&LOT_DCC={FailInsertData[i].Lot_Dcc}&DEVICE_SCAN=" +
                    $"DIE_BANK_EOH=&DIE_QTY={FailInsertData[i].Die_Qty}&DS_DT=1900-01-01 00:00.000&FAB_SITE={Properties.Settings.Default.LOCATION}&HAWB={FailInsertData[i].Bill}&HOLD_COMMENT=&HOLD_STATUS=&" +
                    $"INVOICE={FailInsertData[i].Invoice}&LAST_ISSUE_TIME=&LOCATION=&LOT_TYPE={FailInsertData[i].Lot_type}&LOT_NO={FailInsertData[i].Lot}&MATERIAL=&MES_UPLOAD_TIME={DateTime.Now.ToString()}&MES_UPLOAD_ID={BankHost_main.strMESID}" +
                    $"&ON_HAND_WAFER_ID=&PDL=&PDL_SEPARATOR={"%2F"}&PLANT={Properties.Settings.Default.LOCATION}&PRICE=&RCV_DATE={FailInsertData[i].Rcvddate}0&RCV_DIE_QTY=&RCV_WAFER_QTY=" +
                    $"&REEL_ID={FailInsertData[i].ReelID}&REEL_DCC={(FailInsertData[i].ReelDCC == "" ? "" : FailInsertData[i].ReelDCC)}&RSLT=&SCAN_TIME=&SIZE=&SOURCE_DEVICE={FailInsertData[i].Device}&SPECIFIC_DATA=&UNIT=&USER_ID=&VALIDATION_ID=" +
                    $"&VALIDATION_SCAN_TIME=&WAFER_INFORM=&WAFER_LOT_NO=&WAFER_QTY=&WAFER_STATUS=&WAFER_TYPE=&HOST_NAME=&BADGE=&OPER_NAME={BankHost_main.strID}");

                if (res.Contains("\"RESULT\":\"SUCCESS\"") == true)
                {
                    FailInsertData.RemoveAt(i);
                    --i;
                }
                else
                {
                    if (FailInsertData[i].Retry < 5)
                    {
                        FailInsertData[i].Retry++;
                        FailInsertData[i].FailMSG += res + "\n";
                    }
                    else
                    {

                        writeWASInsertFail(FailInsertData[i]);
                        FailInsertData.RemoveAt(i);
                        --i;
                    }
                }
            }
        }

        private void readWASInsertFail()
        {
            string dir = $"{Application.StartupPath}\\WAS\\";
            string fileName = "InsertFail.txt";


            if (File.Exists($"{dir}\\{fileName}") == true)
            {
                string[] temp = System.IO.File.ReadAllLines($"{dir}\\{fileName}");

                foreach (string s in temp)
                {
                    string[] t = s.Split('\t');

                    FailInsertData.Add(new StorageData()
                    {
                        Plant = t[0],
                        Cust = t[1],
                        Device = t[2],
                        Lot = t[3],
                        Lot_Dcc = t[4],
                        Rcv_Qty = t[5],
                        Die_Qty = t[6],
                        Rcv_WQty = t[7],
                        Rcvddate = t[8],
                        Lot_type = t[9],
                        Bill = t[10],
                        Amkorid = t[11],
                        Wafer_lot = t[12],
                        strCoo = t[13],
                        state = t[14],
                        strop = t[15],
                        strGRstatus = t[16],
                        Default_WQty = t[17],
                        shipment = t[18],
                        Invoice = t[19],
                        Loc = t[20],
                        Hawb = t[21],
                        WSN = t[22],
                        ReadFile = t[23],
                        ReelID = t[24],
                        ReelDCC = t[25]
                    });
                }

                System.IO.File.WriteAllText($"{dir}\\{fileName}", "");
            }
        }



        private void writeWASInsertFail(StorageData data)
        {
            string dir = $"{Application.StartupPath}\\WAS\\";
            string fileName = "InsertFail.txt";

            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);

            System.IO.StreamWriter st = System.IO.File.AppendText($"{dir}\\{fileName}");

            st.WriteLine($"{data.Plant}\t{data.Cust}\t{data.Device}\t{data.Lot}\t{data.Lot_Dcc}\t{data.Rcv_Qty}\t{data.Die_Qty}\t{data.Rcv_WQty}\t{data.Rcvddate}\t" +
                $"{data.Lot_type}\t{data.Bill}\t{data.Amkorid}\t{data.Wafer_lot}\t{data.strCoo}\t{data.state}\t{data.strop}\t{data.strGRstatus}\t{data.Default_WQty}\t" +
                $"{data.shipment}\t{data.Invoice}\t{data.Loc}\t{data.Hawb}\t{data.WSN}\t{data.ReadFile}\t{data.ReelID}\t{data.ReelDCC}\t{data.FailMSG}");
        }

        public int Fnc_Get_Worklist_3(string strData)
        {
            /////////////////////////////////////////////////
            ///파일 이름: JOB\CUST_JOBNO_DATE , ex) WORK\JOB_102_2008060835.txt
            ///파일 이름 설정
            string[] strList = strData.Split('\n'); //index 1 부터 데이터 받아야 함.
            int nArryLength = strList.Length;

            string[] strCol = strList[0].Split('\t');
            int nColcnt = strCol.Length;

            List<StorageData> list = new List<StorageData>();

            int nCount = 0;

            string strMsg = string.Format("\n\n작업 정보를 분석 중 입니다.");
            Frm_Process.Form_Show(strMsg);

            for (int i = 1; i < nArryLength; i++)
            {
                nCount++;
                strMsg = string.Format("\n\n데이터 Read {0} / {1}", nCount, nArryLength - 1);
                Frm_Process.Form_Display(strMsg);

                string[] strJobInfo = strList[i].Split('\t');

                StorageData data = new StorageData();

                for (int j = 0; j < nColcnt; j++)
                {
                    var strType = strJobInfo[j];

                    string str = "";
                    if (strType != null)
                    {
                        if (j != 6)
                            str = strType.ToString();
                        else
                        {
                            str = strType.ToString();

                            DateTime conv = DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                            //DateTime conv = DateTime.FromOADate(double.Parse(strType));
                            str = string.Format("{0}/{1}/{2}", conv.Year, conv.Month, conv.Day);
                        }
                    }

                    if (j == 0) //Cust
                    {
                        if (str == null)
                            str = "";

                        str = str.Trim();
                        data.Cust = str;
                        strWorkCust = str;
                    }
                    else if (j == 1) //Device
                    {
                        str = str.Trim();
                        data.Device = str;
                    }
                    else if (j == 2) //Lot#
                    {
                        str = str.Trim();
                        data.Lot = str;
                    }
                    else if (j == 3)//DCC
                    {
                        str = str.Trim();
                        data.Lot_Dcc = str;
                    }
                    else if (j == 4) //DieQty
                    {
                        str = str.Trim();
                        //string strnQty = string.Format("{0:0,0}", Int32.Parse(str));
                        data.Rcv_Qty = str;
                    }
                    else if (j == 5) //Wafer Qty
                    {
                        str = str.Trim();
                        //data.Rcv_WQty = str;
                        data.Rcv_WQty = "0";
                        data.Default_WQty = str;
                    }
                    else if (j == 6) //RCV date
                    {
                        str = str.Trim();
                        data.Rcvddate = str;
                    }
                    else if (j == 7) //Lot Type
                    {
                        str = str.Trim();
                        data.Lot_type = str;
                    }
                    else if (j == 8) //Bill
                    {
                        str = str.Trim();
                        data.Bill = str;
                    }
                    else if (j == 9) //Amkor id
                    {
                        str = str.Trim();
                        data.Amkorid = str;
                    }
                    else if (j == 10) //wfr lot
                    {
                        str = str.Trim();
                        data.Wafer_lot = str;
                    }
                    else if (j == 11) //coo
                    {
                        str = str.Trim();
                        data.strCoo = str;
                    }
                }
                list.Add(data);
            }

            list.Sort(CompareStorageData);

            dataGridView_worklist.Columns.Clear();
            dataGridView_worklist.Rows.Clear();
            dataGridView_worklist.Refresh();

            dataGridView_worklist.Columns.Add("#", "#");
            dataGridView_worklist.Columns.Add("CUST", "CUST");
            dataGridView_worklist.Columns.Add("DEVICE", "DEVICE");
            dataGridView_worklist.Columns.Add("LOT#", "LOT#");
            dataGridView_worklist.Columns.Add("DCC", "DCC");
            dataGridView_worklist.Columns.Add("DIE_QTY", "DIE_QTY");
            dataGridView_worklist.Columns.Add("WFR TTL", "WFR TTL");
            dataGridView_worklist.Columns.Add("REV_DATE", "REV_DATE");
            dataGridView_worklist.Columns.Add("LOT_TYPE", "LOT_TYPE");
            dataGridView_worklist.Columns.Add("BILL#", "BILL#");
            dataGridView_worklist.Columns.Add("AMKOR_ID", "AMKOR_ID");
            dataGridView_worklist.Columns.Add("WAFER_LOT", "WAFER_LOT");
            dataGridView_worklist.Columns.Add("SHIPMENT", "SHIPMENT");
            dataGridView_worklist.Columns.Add("REELID", "REEL_ID");
            dataGridView_worklist.Columns.Add("REELDCC", "REEL_DCC");


            dataGridView_worklist.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[12].SortMode = DataGridViewColumnSortMode.NotSortable;

            if (list.Count == 0)
            {
                dataGridView_worklist.Columns.Clear();
                dataGridView_worklist.Rows.Clear();
                dataGridView_worklist.Refresh();

                dataGridView_worklist.Columns.Add("데이터가 없습니다. 다시 선택해 주십시오.", "데이터가 없습니다. 다시 선택해 주십시오.");

                System.Windows.Forms.Application.DoEvents();
            }

            nCount = 1;

            foreach (var item in list)
            {
                strSelCust = item.Cust;

                dataGridView_worklist.Rows.Add(new object[] {nCount, item.Cust, item.Device, item.Lot, item.Lot_Dcc, item.Rcv_Qty, item.Default_WQty, item.Rcvddate,
                    item.Lot_type, item.Bill, item.Amkorid, item.Wafer_lot, item.shipment, item.ReelID, item.ReelDCC });

                nCount++;
            }

            Frm_Process.Form_Display("\n작업을 마침니다.");
            Frm_Process.Hide();

            return list.Count;
        }

        public int Fnc_Get_Worklist_lot_history(string strData)
        {
            /////////////////////////////////////////////////
            ///파일 이름: JOB\CUST_JOBNO_DATE , ex) WORK\JOB_102_2008060835.txt
            ///파일 이름 설정
            string[] strList = strData.Split('\n'); //index 1 부터 데이터 받아야 함.
            int nArryLength = strList.Length;

            string[] strCol = strList[0].Split('\t');
            int nColcnt = strCol.Length;

            List<StorageData> list = new List<StorageData>();

            int nCount = 0;

            string strMsg = string.Format("\n\n작업 정보를 분석 중 입니다.");
            Frm_Process.Form_Show(strMsg);

            for (int i = 1; i < nArryLength; i++)
            {
                nCount++;
                strMsg = string.Format("\n\n데이터 Read {0} / {1}", nCount, nArryLength - 1);
                Frm_Process.Form_Display(strMsg);

                string[] strJobInfo = strList[i].Split('\t');

                StorageData data = new StorageData();

                for (int j = 0; j < nColcnt; j++)
                {
                    var strType = strJobInfo[j];

                    string str = "";
                    if (strType != null)
                    {
                        if (j != 6)
                            str = strType.ToString();
                        else
                        {
                            str = strType.ToString();

                            DateTime conv = DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                            //DateTime conv = DateTime.FromOADate(double.Parse(strType));
                            str = string.Format("{0}/{1}/{2}", conv.Year, conv.Month, conv.Day);
                        }
                    }

                    if (j == 0) //Plant
                    {
                        if (str == null)
                            str = "";

                        str = str.Trim();
                        data.Plant = str;
                    }
                    else if (j == 1) //Cust
                    {
                        str = str.Trim();
                        strWorkCust = str;
                    }
                    else if (j == 2) //Loc
                    {
                        str = str.Trim();
                        data.Loc = str;
                    }
                    else if (j == 3)//Hawb#
                    {
                        str = str.Trim();
                        data.Hawb = str;
                    }
                    else if (j == 4) //Invoice#
                    {
                        str = str.Trim();
                        data.Invoice = str;
                        //string strnQty = string.Format("{0:0,0}", Int32.Parse(str));
                    }
                    else if (j == 5) //Device
                    {
                        str = str.Trim();
                        //data.Rcv_WQty = str;
                        data.Device = str;
                    }
                    else if (j == 6) //Lot#
                    {
                        str = str.Trim();
                        data.Lot = str;
                    }
                    else if (j == 7) //DCC
                    {
                        str = str.Trim();
                        data.Lot_Dcc = str;
                    }
                    else if (j == 8) //Die Qty
                    {
                        str = str.Trim();

                        data.Die_Qty = str;
                    }
                    else if (j == 9) //Wfr Qty
                    {
                        str = str.Trim();
                        data.Rcv_WQty = "0";
                        data.Default_WQty = str;
                    }
                    else if (j == 10) //Rev Date
                    {
                        str = str.Trim();
                        data.Rcvddate = str;
                    }
                }
                list.Add(data);
            }

            //list.Sort(CompareStorageData);

            list = list.OrderBy(X => X.Loc).ThenBy(X => X.Lot).ToList();

            dataGridView_worklist.Columns.Clear();
            dataGridView_worklist.Rows.Clear();
            dataGridView_worklist.Refresh();


            dgv_loc.Columns[0].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_loc.Columns[1].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_loc.Columns[2].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_loc.Columns[3].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_loc.Columns[4].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_loc.Columns[5].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_loc.Columns[6].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_loc.Columns[7].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_loc.Columns[8].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_loc.Columns[9].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv_loc.Columns[10].SortMode = DataGridViewColumnSortMode.Programmatic;

            if (list.Count == 0)
            {
                dgv_loc.Columns.Clear();
                dgv_loc.Rows.Clear();
                dgv_loc.Refresh();

                dgv_loc.Columns.Add("데이터가 없습니다. 다시 선택해 주십시오.", "데이터가 없습니다. 다시 선택해 주십시오.");

                System.Windows.Forms.Application.DoEvents();
            }

            nCount = 0;

            foreach (var item in list)
            {
                strSelCust = item.Cust;

                dgv_loc.Rows.Add(new object[11] { item.Plant, item.Cust, item.Loc, item.Hawb, item.Invoice, item.Device, item.Lot, item.Lot_Dcc, item.Die_Qty, item.Default_WQty, item.Rcv_WQty });

                if (item.Loc == "")
                {
                    dgv_loc.Rows[nCount].DefaultCellStyle.BackColor = Color.Yellow;
                    dgv_loc.Rows[nCount].DefaultCellStyle.ForeColor = Color.Black;
                }

                nCount++;
            }

            Frm_Process.Form_Display("\n작업을 마침니다.");
            Frm_Process.Hide();

            return list.Count;
        }


        public void Fnc_Set_Workfile(string[] strBillData)
        {
            /////////////////////////////////////////////////
            ///파일 이름: JOB\CUST_JOBNO_DATE , ex) WORK\JOB_102_2008060835.txt
            ///파일 이름 설정

            string strMsg = string.Format("\n\n작업 정보를 생성 합니다.");
            Frm_Process.Form_Show(strMsg);

            System.Data.DataTable dt = new System.Data.DataTable();

            foreach (DataGridViewColumn col in dataGridView_worklist.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dataGridView_worklist.Rows)
            {
                DataRow dRow = dt.NewRow();

                if (row.Cells["BILL#"].Value.ToString() == strBillData[0])
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dRow[cell.ColumnIndex] = cell.Value;
                    }
                    dt.Rows.Add(dRow);
                }
            }

            dataGridView_worklist.Columns.Clear();
            dataGridView_worklist.Rows.Clear();
            dataGridView_worklist.Refresh();

            dataGridView_worklist.Columns.Add("#", "#");
            dataGridView_worklist.Columns.Add("CUST", "CUST");
            dataGridView_worklist.Columns.Add("DEVICE", "DEVICE");
            dataGridView_worklist.Columns.Add("LOT#", "LOT#");
            dataGridView_worklist.Columns.Add("DCC", "DCC");
            dataGridView_worklist.Columns.Add("DIE_QTY", "DIE_QTY");
            dataGridView_worklist.Columns.Add("WFR TTL", "WFR TTL");
            dataGridView_worklist.Columns.Add("REV_DATE", "REV_DATE");
            dataGridView_worklist.Columns.Add("LOT_TYPE", "LOT_TYPE");
            dataGridView_worklist.Columns.Add("BILL#", "BILL#");
            dataGridView_worklist.Columns.Add("AMKOR_ID", "AMKOR_ID");
            dataGridView_worklist.Columns.Add("WAFER_LOT", "WAFER_LOT");
            dataGridView_worklist.Columns.Add("SHIPMENT", "SHIPMENT");
            dataGridView_worklist.Columns.Add("REELID", "REEL_ID");
            dataGridView_worklist.Columns.Add("REELDCC", "REEL_DCC");

            dataGridView_worklist.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[12].SortMode = DataGridViewColumnSortMode.NotSortable;

            List<StorageData> list = new List<StorageData>();

            string strFileName_addBill = "", strFileCust = "";

            int nCount = 0, nIdex = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nCount++;
                strMsg = string.Format("\n\n데이터 Read {0} / {1}", nCount, dt.Rows.Count);
                Frm_Process.Form_Display(strMsg);

                StorageData data = new StorageData();

                data.Cust = dt.Rows[i]["CUST"].ToString();
                data.Device = dt.Rows[i]["DEVICE"].ToString();
                data.Lot = dt.Rows[i]["LOT#"].ToString();
                data.Lot_Dcc = dt.Rows[i]["DCC"].ToString();
                data.Rcv_Qty = dt.Rows[i]["DIE_QTY"].ToString();
                data.Default_WQty = dt.Rows[i]["WFR TTL"].ToString();
                data.Rcvddate = dt.Rows[i]["REV_DATE"].ToString();
                data.Lot_type = dt.Rows[i]["LOT_TYPE"].ToString();
                data.Bill = dt.Rows[i]["BILL#"].ToString();
                data.Amkorid = dt.Rows[i]["AMKOR_ID"].ToString();
                data.Wafer_lot = dt.Rows[i]["WAFER_LOT"].ToString();
                data.shipment = dt.Rows[i]["SHIPMENT"].ToString();
                data.ReelID = dt.Rows[i]["REELID"].ToString();
                data.ReelDCC = dt.Rows[i]["REELDCC"].ToString();

                bool bSave = false;

                for (int j = 0; j < strBillData.Length; j++)
                {
                    if (data.Bill == strBillData[j])
                    {
                        bSave = true;
                    }

                    if (i == 0)
                    {
                        if (strBillData[j] != "")
                            strFileName_addBill = strFileName_addBill + strBillData[j].Substring(strBillData[j].Length - 4, 4);
                    }
                }

                if (bSave)
                {
                    strFileCust = data.Cust;

                    nIdex++;
                    dataGridView_worklist.Rows.Add(new object[] { nCount, data.Cust, data.Device, data.Lot, data.Lot_Dcc, data.Rcv_Qty, data.Default_WQty, data.Rcvddate,
                    data.Lot_type, data.Bill, data.Amkorid, data.Wafer_lot, data.shipment, data.ReelID, data.ReelDCC });

                    list.Add(data);
                }
            }

            list.Sort(CompareStorageData);

            string strSavepath = "", strSetFileName = "", strSetFolder = "";

            string strYear = DateTime.Now.Year.ToString().Substring(2, 2);
            strSetFolder = string.Format("JOB_{0}_{1:00}{2:00}{3:00}_{4}", strFileCust, strYear, DateTime.Now.Month, DateTime.Now.Day, strFileName_addBill);
            strSetFileName = strSetFolder + ".txt";
            strWorkFileName = strSetFolder;
            strSavepath = strExcutionPath + "\\Work\\" + strSetFileName;

            string sDirFileNamePath = "", sDirDeviceNamePath = "";
            sDirFileNamePath = strExcutionPath + "\\Work\\" + strSetFolder;

            /////.txt 파일 만들기
            System.IO.FileInfo fi = new System.IO.FileInfo(strSavepath);

            if (fi.Exists)
            {
                File.Delete(strSavepath);
                /////폴더,폴더, 파일 삭제
                DirectoryInfo dir = new DirectoryInfo(sDirFileNamePath);
                dir.Delete(true);

            }
            ///파일 이름 폴더 만들기            
            DirectoryInfo di = new DirectoryInfo(sDirFileNamePath);
            if (di.Exists == false)
            {
                di.Create();
            }
            ////////////////////////////////////////            
            nCount = 0;

            string strDevicecheck = "";
            foreach (var item in list)
            {
                item.state = "Waiting";
                item.strop = "";
                item.Die_Qty = "0";
                item.Rcv_WQty = "0";
                item.strGRstatus = "Ready";

                string strTxtline = item.Cust + "\t" + item.Device + "\t" + item.Lot + "\t" + item.Lot_Dcc + "\t" + item.Rcv_Qty + "\t" + item.Die_Qty + "\t" +
                    item.Rcv_WQty + "\t" + item.Rcvddate + "\t" + item.Lot_type + "\t" + item.Bill + "\t" + item.Amkorid + "\t" + item.Wafer_lot + "\t" + item.strCoo + "\t" +
                    item.state + "\t" + item.strop + "\t" + item.strGRstatus + "\t" + item.Default_WQty + "\t" + item.shipment + $"\t{item.ReelID}\t{item.ReelDCC}\t";

                if (strDevicecheck != item.Device)
                {
                    Fnc_WriteFile(strSavepath, item.Device);
                    strDevicecheck = item.Device;
                }

                /////////////////////////////////////Device 폴더 생성
                sDirDeviceNamePath = sDirFileNamePath + "\\" + item.Device;
                DirectoryInfo diinfo = new DirectoryInfo(sDirDeviceNamePath);
                if (diinfo.Exists == false)
                {
                    diinfo.Create();
                }
                diinfo = null;
                /////////////////////////////////////File 저장
                string strLotsavepath = sDirDeviceNamePath + "\\" + item.Device + ".txt";
                Fnc_WriteFile(strLotsavepath, strTxtline);
                ////////////////////////////////////

                nCount++;
                strMsg = string.Format("\n\n 작업 준비 중 입니다. {0} / {1}", nCount, list.Count);
                Frm_Process.Form_Display(strMsg);

                System.Windows.Forms.Application.DoEvents();
            }

            BankHost_main.Host.Host_Set_Jobname(BankHost_main.strEqid, strWorkFileName);

            Frm_Process.Form_Display("\n작업을 마침니다.");
            Frm_Process.Hide();
        }
        public void Fnc_Set_Workfile_NoDevice(string[] strBillData)
        {
            /////////////////////////////////////////////////
            ///파일 이름: JOB\CUST_JOBNO_DATE , ex) WORK\JOB_102_2008060835.txt
            ///파일 이름 설정

            string strMsg = string.Format("\n\n작업 정보를 생성 합니다.");
            Frm_Process.Form_Show(strMsg);

            System.Data.DataTable dt = new System.Data.DataTable();

            foreach (DataGridViewColumn col in dataGridView_worklist.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dataGridView_worklist.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }

            dataGridView_worklist.Columns.Clear();
            dataGridView_worklist.Rows.Clear();
            dataGridView_worklist.Refresh();

            dataGridView_worklist.Columns.Add("#", "#");
            dataGridView_worklist.Columns.Add("CUST", "CUST");
            dataGridView_worklist.Columns.Add("DEVICE", "DEVICE");
            dataGridView_worklist.Columns.Add("LOT#", "LOT#");
            dataGridView_worklist.Columns.Add("DCC", "DCC");
            dataGridView_worklist.Columns.Add("DIE_QTY", "DIE_QTY");
            dataGridView_worklist.Columns.Add("WFR TTL", "WFR TTL");
            dataGridView_worklist.Columns.Add("REV_DATE", "REV_DATE");
            dataGridView_worklist.Columns.Add("LOT_TYPE", "LOT_TYPE");
            dataGridView_worklist.Columns.Add("BILL#", "BILL#");
            dataGridView_worklist.Columns.Add("AMKOR_ID", "AMKOR_ID");
            dataGridView_worklist.Columns.Add("WAFER_LOT", "WAFER_LOT");


            dataGridView_worklist.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;

            List<StorageData> list = new List<StorageData>();

            string strFileName_addBill = "", strFileCust = "";

            int nCount = 0, nIdex = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nCount++;
                strMsg = string.Format("\n\n데이터 Read {0} / {1}", nCount, dt.Rows.Count);
                Frm_Process.Form_Display(strMsg);

                StorageData data = new StorageData();

                data.Cust = dt.Rows[i]["CUST"].ToString();
                data.Device = dt.Rows[i]["DEVICE"].ToString();
                data.Lot = dt.Rows[i]["LOT#"].ToString();
                data.Lot_Dcc = dt.Rows[i]["DCC"].ToString();
                data.Rcv_Qty = dt.Rows[i]["DIE_QTY"].ToString();
                data.Default_WQty = dt.Rows[i]["WFR TTL"].ToString();
                data.Rcvddate = dt.Rows[i]["REV_DATE"].ToString();
                data.Lot_type = dt.Rows[i]["LOT_TYPE"].ToString();
                data.Bill = dt.Rows[i]["BILL#"].ToString();
                data.Amkorid = dt.Rows[i]["AMKOR_ID"].ToString();
                data.Wafer_lot = dt.Rows[i]["WAFER_LOT"].ToString();

                bool bSave = false;

                for (int j = 0; j < strBillData.Length; j++)
                {
                    if (data.Bill == strBillData[j])
                    {
                        bSave = true;
                    }

                    if (i == 0)
                    {
                        if (strBillData[j] != "")
                            strFileName_addBill = strFileName_addBill + strBillData[j].Substring(strBillData[j].Length - 4, 4);
                    }
                }

                if (bSave)
                {
                    strFileCust = data.Cust;

                    nIdex++;
                    dataGridView_worklist.Rows.Add(new object[12] { nIdex, data.Cust, data.Device, data.Lot, data.Lot_Dcc, data.Rcv_Qty, data.Default_WQty, data.Rcvddate,
                    data.Lot_type, data.Bill, data.Amkorid, data.Wafer_lot });

                    list.Add(data);
                }
            }

            list.Sort(CompareStorageData);

            string strDeviceName = "", strTotalDevice = "";
            for (int n = 0; n < list.Count; n++)
            {
                string str = list[n].Device;

                if (strDeviceName != str)
                {
                    strTotalDevice = strTotalDevice + str + Environment.NewLine;
                    strDeviceName = str;
                }
            }
            strTotalDevice = strTotalDevice.Substring(0, strTotalDevice.Length - 2);

            string strSavepath = "", strSetFileName = "", strSetFolder = "";

            string strYear = DateTime.Now.Year.ToString().Substring(2, 2);
            strSetFolder = string.Format("JOB_{0}_{1:00}{2:00}{3:00}_{4}", strFileCust, strYear, DateTime.Now.Month, DateTime.Now.Day, strFileName_addBill);
            strSetFileName = strSetFolder + ".txt";
            strWorkFileName = strSetFolder;
            strSavepath = strExcutionPath + "\\Work\\" + strSetFileName;

            string sDirFileNamePath = "", sDirDeviceNamePath = "";
            sDirFileNamePath = strExcutionPath + "\\Work\\" + strSetFolder;

            /////.txt 파일 만들기
            System.IO.FileInfo fi = new System.IO.FileInfo(strSavepath);

            if (fi.Exists)
            {
                File.Delete(strSavepath);
                /////폴더,폴더, 파일 삭제
                DirectoryInfo dir = new DirectoryInfo(sDirFileNamePath);
                dir.Delete(true);

            }
            ///파일 이름 폴더 만들기            
            DirectoryInfo di = new DirectoryInfo(sDirFileNamePath);
            if (di.Exists == false)
            {
                di.Create();
            }
            ////////////////////////////////////////            
            nCount = 0;

            //string strDevicecheck = "";

            Fnc_WriteFile(strSavepath, strTotalDevice);
            foreach (var item in list)
            {
                item.state = "Waiting";
                item.strop = "";
                item.Die_Qty = "0";
                item.Rcv_WQty = "0";
                item.strGRstatus = "Ready";

                string strTxtline = item.Cust + "\t" + item.Device + "\t" + item.Lot + "\t" + item.Lot_Dcc + "\t" + item.Rcv_Qty + "\t" + item.Die_Qty + "\t" +
                    item.Rcv_WQty + "\t" + item.Rcvddate + "\t" + item.Lot_type + "\t" + item.Bill + "\t" + item.Amkorid + "\t" + item.Wafer_lot + "\t" + item.strCoo + "\t" +
                    item.state + "\t" + item.strop + "\t" + item.strGRstatus + "\t" + item.Default_WQty + "\t" + item.shipment + $"\t{item.ReelID}\t{item.ReelDCC}\t";

                //if (strDevicecheck != item.Device)
                //{
                //    Fnc_WriteFile(strSavepath, strTotalDevice);
                //    strDevicecheck = item.Device;
                //}

                /////////////////////////////////////Device 폴더 생성
                //sDirDeviceNamePath = sDirFileNamePath + "\\" + item.Device;
                sDirDeviceNamePath = sDirFileNamePath + "\\" + item.Device;
                DirectoryInfo diinfo = new DirectoryInfo(sDirDeviceNamePath);
                if (diinfo.Exists == false)
                {
                    diinfo.Create();
                }
                diinfo = null;
                /////////////////////////////////////File 저장
                string strLotsavepath = sDirDeviceNamePath + "\\" + item.Device + ".txt";
                Fnc_WriteFile(strLotsavepath, strTxtline);
                ////////////////////////////////////

                nCount++;
                strMsg = string.Format("\n\n 작업 준비 중 입니다. {0} / {1}", nCount, list.Count);
                Frm_Process.Form_Display(strMsg);

                System.Windows.Forms.Application.DoEvents();
            }

            BankHost_main.Host.Host_Set_Jobname(BankHost_main.strEqid, strWorkFileName);

            Frm_Process.Form_Display("\n작업을 마침니다.");
            Frm_Process.Hide();
        }
        public int Fnc_WorkView(string strWorkName)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkName + ".txt";

            string[] data = Fnc_ReadFile(strFileName);

            int nLength = 0;

            dataGridView_worklist.Columns.Clear();
            dataGridView_worklist.Rows.Clear();
            dataGridView_worklist.Refresh();

            dataGridView_worklist.Columns.Add("#", "#");
            dataGridView_worklist.Columns.Add("CUST", "CUST");
            dataGridView_worklist.Columns.Add("DEVICE", "DEVICE");
            dataGridView_worklist.Columns.Add("LOT#", "LOT#");
            dataGridView_worklist.Columns.Add("DCC", "DCC");
            dataGridView_worklist.Columns.Add("DIE_QTY", "DIE_QTY");
            dataGridView_worklist.Columns.Add("WFR TTL", "WFR TTL");
            dataGridView_worklist.Columns.Add("REV_DATE", "REV_DATE");
            dataGridView_worklist.Columns.Add("LOT_TYPE", "LOT_TYPE");
            dataGridView_worklist.Columns.Add("BILL#", "BILL#");
            dataGridView_worklist.Columns.Add("AMKOR_ID", "AMKOR_ID");
            dataGridView_worklist.Columns.Add("WAFER_LOT", "WAFER_LOT");
            dataGridView_worklist.Columns.Add("SHIPMENT", "SHIPMENT");
            dataGridView_worklist.Columns.Add("REELID", "REEL_ID");
            dataGridView_worklist.Columns.Add("REELDCC", "REEL_DCC");


            dataGridView_worklist.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[12].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[13].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[14].SortMode = DataGridViewColumnSortMode.NotSortable;

            if (data != null)
            {
                nLength = data.Length;
            }
            else
            {
                dataGridView_worklist.Columns.Clear();
                dataGridView_worklist.Rows.Clear();
                dataGridView_worklist.Refresh();

                dataGridView_worklist.Columns.Add("데이터가 없습니다. 다시 선택해 주십시오.", "데이터가 없습니다. 다시 선택해 주십시오.");

                System.Windows.Forms.Application.DoEvents();

                return 0;
            }
            List<StorageData> list_Job = new List<StorageData>();

            string strReadfolder = strFileName.Substring(0, strFileName.Length - 4);

            for (int n = 0; n < nLength; n++)
            {
                string strReadfile = strReadfolder + "\\" + data[n] + "\\" + data[n] + ".txt";
                string[] info = Fnc_ReadFile(strReadfile);

                if (info == null)
                    return 0;

                if (info.Length < 1)
                    return 0;

                for (int m = 0; m < info.Length; m++)
                {
                    StorageData st = new StorageData();

                    string[] strSplit_data = info[m].Split('\t');

                    st.Cust = strSplit_data[0];
                    strWorkCust = st.Cust;
                    st.Device = strSplit_data[1];
                    st.Lot = strSplit_data[2];
                    st.Lot_Dcc = strSplit_data[3];
                    st.Rcv_Qty = strSplit_data[4];
                    st.Rcv_WQty = strSplit_data[6];
                    st.Default_WQty = strSplit_data[16];
                    st.Rcvddate = strSplit_data[7];
                    st.Lot_type = strSplit_data[8];
                    st.Bill = strSplit_data[9];
                    st.Amkorid = strSplit_data[10];
                    st.Wafer_lot = strSplit_data[11];

                    if (strSplit_data.Length > 17)
                    {
                        st.shipment = strSplit_data[17];
                        st.ReelID = strSplit_data[19];
                        st.ReelDCC = strSplit_data[20];
                    }
                    else
                    {
                        st.shipment = "";
                        st.ReelID = "";
                        st.ReelDCC = "";
                    }

                    list_Job.Add(st);
                }
            }

            list_Job.Sort(CompareStorageData);

            int nCount = 1;
            foreach (var item in list_Job)
            {
                strSelCust = item.Cust;

                dataGridView_worklist.Rows.Add(new object[] { nCount, item.Cust, item.Device, item.Lot, item.Lot_Dcc, item.Rcv_Qty, item.Default_WQty, item.Rcvddate,
                    item.Lot_type, item.Bill, item.Amkorid, item.Wafer_lot, item.shipment, item.ReelID, item.ReelDCC });

                nCount++;
            }

            return nCount - 1;
        }


        public int Split_lot(string strWorkName)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkName + ".txt";

            string[] data = Fnc_ReadFile(strFileName);

            int nLength = 0;

            dataGridView_worklist.Columns.Clear();
            dataGridView_worklist.Rows.Clear();
            dataGridView_worklist.Refresh();

            dataGridView_worklist.Columns.Add("#", "#");
            dataGridView_worklist.Columns.Add("CUST", "CUST");
            dataGridView_worklist.Columns.Add("DEVICE", "DEVICE");
            dataGridView_worklist.Columns.Add("LOT#", "LOT#");
            dataGridView_worklist.Columns.Add("DCC", "DCC");
            dataGridView_worklist.Columns.Add("DIE_QTY", "DIE_QTY");
            dataGridView_worklist.Columns.Add("WFR TTL", "WFR TTL");
            dataGridView_worklist.Columns.Add("REV_DATE", "REV_DATE");
            dataGridView_worklist.Columns.Add("LOT_TYPE", "LOT_TYPE");
            dataGridView_worklist.Columns.Add("BILL#", "BILL#");
            dataGridView_worklist.Columns.Add("AMKOR_ID", "AMKOR_ID");
            dataGridView_worklist.Columns.Add("WAFER_LOT", "WAFER_LOT");
            dataGridView_worklist.Columns.Add("SHIPMENT", "SHIPMENT");
            dataGridView_worklist.Columns.Add("REELID", "REEL_ID");
            dataGridView_worklist.Columns.Add("REELDCC", "REEL_DCC");

            dataGridView_worklist.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[12].SortMode = DataGridViewColumnSortMode.NotSortable;

            if (data != null)
            {
                nLength = data.Length;
            }
            else
            {
                dataGridView_worklist.Columns.Clear();
                dataGridView_worklist.Rows.Clear();
                dataGridView_worklist.Refresh();

                dataGridView_worklist.Columns.Add("데이터가 없습니다. 다시 선택해 주십시오.", "데이터가 없습니다. 다시 선택해 주십시오.");

                System.Windows.Forms.Application.DoEvents();

                return 0;
            }
            List<StorageData> list_Job = new List<StorageData>();

            string strReadfolder = strFileName.Substring(0, strFileName.Length - 4);

            for (int n = 0; n < nLength; n++)
            {
                string strReadfile = strReadfolder + "\\" + data[n] + "\\" + data[n] + ".txt";
                string[] info = Fnc_ReadFile(strReadfile);

                if (info == null)
                    return 0;

                if (info.Length < 1)
                    return 0;

                for (int m = 0; m < info.Length; m++)
                {
                    StorageData st = new StorageData();

                    string[] strSplit_data = info[m].Split('\t');

                    st.Cust = strSplit_data[0];
                    strWorkCust = st.Cust;
                    st.Device = strSplit_data[1];
                    st.Lot = strSplit_data[2];
                    st.Lot_Dcc = strSplit_data[3];
                    st.Rcv_Qty = strSplit_data[4];
                    st.Rcv_WQty = strSplit_data[6];
                    st.Default_WQty = strSplit_data[16];
                    st.Rcvddate = strSplit_data[7];
                    st.Lot_type = strSplit_data[8];
                    st.Bill = strSplit_data[9];
                    st.Amkorid = strSplit_data[10];
                    st.Wafer_lot = strSplit_data[11];

                    if (strSplit_data.Length > 17)
                        st.shipment = strSplit_data[17];
                    else
                        st.shipment = "";

                    list_Job.Add(st);
                }
            }

            list_Job.Sort(CompareStorageData);

            int nCount = 1;
            foreach (var item in list_Job)
            {
                strSelCust = item.Cust;

                dataGridView_worklist.Rows.Add(new object[] { nCount, item.Cust, item.Device, item.Lot, item.Lot_Dcc, item.Rcv_Qty, item.Default_WQty, item.Rcvddate,
                    item.Lot_type, item.Bill, item.Amkorid, item.Wafer_lot, item.shipment, item.ReelID, item.ReelDCC });

                nCount++;
            }

            return nCount - 1;
        }


        public void Fnc_WorkFileLoad()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".xlsx";
            openFileDialog.ShowDialog();

            string strName = openFileDialog.FileName;
            if (strName.Length > 0)
            {
                string str = strName.Substring(strName.Length - 4, 4);
                if (str != "xlsx")
                {
                    MessageBox.Show("액셀 파일이 아닙니다. 로드 실패!");
                    return;
                }

                string strSavepath = "", strSetFileName = "";

                string[] strSplit = strName.Split('\\');
                int nLength = strSplit.Length;

                strWorkFileName = strSplit[nLength - 1].Substring(0, strSplit[nLength - 1].Length - 5);
                strWorkFileName = strWorkFileName.Trim();
                strSetFileName = strWorkFileName + ".txt";

                strSavepath = strExcutionPath + "\\Work\\" + strSetFileName;

                /////.ini 파일 만들기
                System.IO.FileInfo fi = new System.IO.FileInfo(strSavepath);

                if (fi.Exists)
                {
                    Fnc_WorkDownload(strWorkFileName);
                    /*
                    DialogResult dialogResult1 = MessageBox.Show("작업 이력이 있습니다.\n\n이어서 시작 합니다.", "Alart", MessageBoxButtons.YesNo);
                    if (dialogResult1 == DialogResult.Yes)
                    {
                        //Fnc_ExcelDownlown(strName);
                        ///ini 파일 로드 해서 실행
                        Fnc_WorkDownload(strWorkFileName);
                    }
                    else
                    {
                        return;
                    }
                    */
                }
                else
                    Fnc_ExcelDownload2(strName);

                label_filename.Text = strWorkFileName;
                label_filename2.Text = strWorkFileName;
                label_hist_filename.Text = strWorkFileName;

                Fnc_SetState(0);
                tabControl_Sort.SelectedIndex = 2;
            }
        }

        public void Fnc_SetState(int nState)
        {
            if (nState == 0) //대기
            {
                label_state.BackColor = Color.FromArgb(150, 150, 150);
                label_state.ForeColor = Color.White;
                label_state.Text = "대기";
            }
            else if (nState == 1) //작업중
            {
                label_state.BackColor = Color.DarkGreen;
                label_state.ForeColor = Color.White;
                label_state.Text = "작업중";
            }
            else if (nState == 2) //알람
            {
                label_state.BackColor = Color.Red;
                label_state.ForeColor = Color.White;
                label_state.Text = "알람";
            }
        }

        List<StorageData> GRReadyList = new List<StorageData>();

        public void Fnc_WorkDownload(string strWorkName)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkName + ".txt";

            string[] data = Fnc_ReadFile(strFileName);
            GRReadyList.Clear();

            int nLength = 0;

            dataGridView_sort.Columns.Clear();
            dataGridView_sort.Rows.Clear();
            dataGridView_sort.Refresh();

            dataGridView_sort.Columns.Add("#", "#");
            dataGridView_sort.Columns.Add("CUST", "CUST");
            dataGridView_sort.Columns.Add("DEVICE", "DEVICE");
            dataGridView_sort.Columns.Add("LOT#", "LOT#");
            //dataGridView_sort.Columns.Add("LOT_DCC", "LOT_DCC");
            dataGridView_sort.Columns.Add("DIE_TTL", "DIE_TTL");
            dataGridView_sort.Columns.Add("DIE_QTY", "DIE_QTY");
            dataGridView_sort.Columns.Add("WFR_TTL", "WFR_TTL");
            dataGridView_sort.Columns.Add("WFR_QTY", "WFR_QTY");
            dataGridView_sort.Columns.Add("RCVD-DATE", "RCVD-DATE");
            dataGridView_sort.Columns.Add("LOT_TYPE", "LOT_TYPE");
            dataGridView_sort.Columns.Add("BILL#", "BILL#");
            dataGridView_sort.Columns.Add("AMKOR_ID", "AMKOR_ID");
            dataGridView_sort.Columns.Add("WFR_LOT", "WFR_LOT");
            dataGridView_sort.Columns.Add("COO", "COO");
            dataGridView_sort.Columns.Add("STATE", "STATE");
            dataGridView_sort.Columns.Add("작업자", "작업자");
            dataGridView_sort.Columns.Add("GR처리", "GR처리");
            dataGridView_sort.Columns.Add("SHIPMENT", "SHIPMENT");
            dataGridView_sort.Columns.Add("REELID", "Reel ID");
            dataGridView_sort.Columns.Add("REELDCC", "Reel DCC");

            if (BankHost_main.strCustName.Contains("WSN") == true)
            {
                dataGridView_sort.Columns.Add("WSN", "WSN");
            }

            for (int i = 0; i < dataGridView_sort.Columns.Count; i++)
            {
                dataGridView_sort.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if (data != null)
            {
                nLength = data.Length;
            }
            else
            {
                dataGridView_sort.Columns.Clear();
                dataGridView_sort.Rows.Clear();
                dataGridView_sort.Refresh();

                dataGridView_sort.Columns.Add("데이터가 없습니다. 다시 선택해 주십시오.", "데이터가 없습니다. 다시 선택해 주십시오.");

                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(1500);

                tabControl_Sort.SelectedIndex = 0;

                return;
            }


            string strReadfolder = strFileName.Substring(0, strFileName.Length - 4);

            dataGridView_Device.Columns.Clear();
            dataGridView_Device.Rows.Clear();
            dataGridView_Device.Refresh();

            device_row_num = 0;
            lot_row_num = 0;

            dataGridView_Device.Columns.Add("#", "#");
            dataGridView_Device.Columns.Add("DEVICE", "DEVICE");

            dataGridView_Device.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_Device.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            for (int n = 0; n < nLength; n++)
            {
                string strReadfile = strReadfolder + "\\" + data[n] + "\\" + data[n] + ".txt";
                string[] info = Fnc_ReadFile(strReadfile);

                dataGridView_Device.Rows.Add(new object[2] { n + 1, data[n] });

                for (int m = 0; m < info.Length; m++)
                {
                    StorageData st = new StorageData();

                    string[] strSplit_data = info[m].Split('\t');

                    st.Cust = strSplit_data[0];

                    st.Device = strSplit_data[1];
                    st.Lot = strSplit_data[2];
                    st.Lot_Dcc = strSplit_data[3];
                    st.Rcv_Qty = strSplit_data[4];
                    st.Die_Qty = strSplit_data[5];
                    st.Rcv_WQty = strSplit_data[6];
                    st.Rcvddate = strSplit_data[7];
                    st.Lot_type = strSplit_data[8];
                    st.Bill = strSplit_data[9];
                    st.Amkorid = strSplit_data[10];
                    st.Wafer_lot = strSplit_data[11];
                    st.strCoo = strSplit_data[12];
                    st.state = strSplit_data[13];
                    st.strop = strSplit_data[14];
                    st.strGRstatus = strSplit_data[15];
                    st.Default_WQty = strSplit_data[16];
                    st.ReelID = strSplit_data[19];
                    st.ReelDCC = strSplit_data[20];

                    if (strSplit_data.Length > 17)
                        st.shipment = strSplit_data[17];
                    else
                        st.shipment = "";

                    GRReadyList.Add(st);
                }
            }

            GRReadyList.Sort(CompareStorageData);

            int nCount = 1, nWait = 0, nWork = 0, nComplete = 0, nError = 0, nGR = 0;
            foreach (var item in GRReadyList)
            {
                dataGridView_sort.Rows.Add(new object[] { nCount, item.Cust, item.Device, item.Lot, item.Rcv_Qty, item.Die_Qty, item.Default_WQty, item.Rcv_WQty, item.Rcvddate,
                    item.Lot_type, item.Bill, item.Amkorid, item.Wafer_lot, item.strCoo, item.state, item.strop, item.strGRstatus, item.shipment, item.ReelID, item.ReelDCC });

                if (item.state == "Waiting")
                {
                    dataGridView_sort.Rows[nCount - 1].DefaultCellStyle.BackColor = Color.LightGray;
                    dataGridView_sort.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.Black;

                    nWait++;
                }
                else if (item.state == "Working")
                {
                    dataGridView_sort.Rows[nCount - 1].DefaultCellStyle.BackColor = Color.Green;
                    dataGridView_sort.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.White;

                    nWork++;
                }
                else if (item.state == "Complete")
                {
                    dataGridView_sort.Rows[nCount - 1].DefaultCellStyle.BackColor = Color.Blue;
                    dataGridView_sort.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.White;

                    nComplete++;
                }
                else if (item.state == "Error" || item.strGRstatus == "ERROR")
                {
                    dataGridView_sort.Rows[nCount - 1].DefaultCellStyle.BackColor = Color.Red;
                    dataGridView_sort.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.White;

                    nError++;
                }

                if (item.strGRstatus == "COMPLETE" || item.strGRstatus == "Complete")
                {
                    dataGridView_sort.Rows[nCount - 1].DefaultCellStyle.BackColor = Color.DarkBlue;
                    dataGridView_sort.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.White;
                    nGR++;
                }

                //Application.DoEvents();
                //Thread.Sleep(1);

                nCount++;
            }

            label_wait.Text = nWait.ToString();
            label_work.Text = nWork.ToString();
            label_complete.Text = nComplete.ToString();
            label_error.Text = nError.ToString();

            label_filename.Text = strWorkFileName;
            label_filename2.Text = strWorkFileName;
            label_hist_filename.Text = strWorkFileName;

            dataGridView_sort.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_sort.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_sort.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_sort.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_sort.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_sort.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_sort.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_sort.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView_sort.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView_sort.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView_sort.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView_sort.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView_sort.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView_sort.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView_sort.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView_sort.Columns[15].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_sort.Columns[16].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView_Device.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_Device.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            DataSet ds = SearchData($"select Source_Device from TB_QORVO_WSN_DEVICE with(nolock)");

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                foreach (DataGridViewRow dev in dataGridView_Device.Rows)
                {
                    if (row["Source_Device"].ToString() == dev.Cells["DEVICE"].Value.ToString())
                    {
                        tc_WSN.Visible = true;
                        tb_wsn.Text = Properties.Settings.Default.QorvoWSN;
                        tc_WSN.SelectedIndex = 0;

                        return;
                    }
                }
            }

            if (BankHost_main.strCustName == "QUALCOMM_SPLIT")
            {
                tc_WSN.Visible = true;
                tc_WSN.SelectedIndex = 1;
                nup_Wlabel.Value = Properties.Settings.Default.SplitWLabel;
            }
        }

        public void Fnc_UpdateCount(string strWorkName)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkName + ".txt";

            string[] data = Fnc_ReadFile(strFileName);

            int nLength = 0;

            if (data != null)
            {
                nLength = data.Length;
            }
            else
            {
                label_scan_wait.Text = "-";
                label_scan_work.Text = "-";
                label_scan_complete.Text = "-";
                label_scan_error.Text = "-";

                return;
            }

            List<StorageData> list_Job = new List<StorageData>();

            string strReadfolder = strFileName.Substring(0, strFileName.Length - 4);

            for (int n = 0; n < nLength; n++)
            {
                string strReadfile = strReadfolder + "\\" + data[n] + "\\" + data[n] + ".txt";
                string[] info = Fnc_ReadFile(strReadfile);

                for (int m = 0; m < info.Length; m++)
                {
                    StorageData st = new StorageData();

                    string[] strSplit_data = info[m].Split('\t');

                    st.Cust = strSplit_data[0];

                    st.Device = strSplit_data[1];
                    st.Lot = strSplit_data[2];
                    st.Lot_Dcc = strSplit_data[3];
                    st.Rcv_Qty = strSplit_data[4];
                    st.Die_Qty = strSplit_data[5];
                    st.Rcv_WQty = strSplit_data[6];
                    st.Rcvddate = strSplit_data[7];
                    st.Lot_type = strSplit_data[8];
                    st.Bill = strSplit_data[9];
                    st.Amkorid = strSplit_data[10];
                    st.Wafer_lot = strSplit_data[11];
                    st.strCoo = strSplit_data[12];
                    st.state = strSplit_data[13];
                    st.strop = strSplit_data[14];
                    st.strGRstatus = strSplit_data[15];
                    st.Default_WQty = strSplit_data[16];

                    list_Job.Add(st);
                }
            }

            list_Job.Sort(CompareStorageData);

            int nCount = 1, nWait = 0, nWork = 0, nComplete = 0, nError = 0, nGR = 0;
            foreach (var item in list_Job)
            {
                if (item.state == "Waiting")
                {
                    nWait++;
                }
                else if (item.state == "Working")
                {
                    nWork++;
                }
                else if (item.state == "Complete")
                {
                    nComplete++;
                }
                else if (item.state == "Error")
                {
                    nError++;
                }

                if (item.strGRstatus == "COMPLETE" || item.strGRstatus == "Complete")
                {
                    nGR++;
                }

                nCount++;
            }

            label_scan_wait.Text = nWait.ToString();
            label_scan_work.Text = nWork.ToString();
            label_scan_complete.Text = nComplete.ToString();
            label_scan_error.Text = nError.ToString();
        }
        public int Gr_GetBillInfo()
        {
            dataGridView_workbill.Columns.Clear();
            dataGridView_workbill.Rows.Clear();
            dataGridView_workbill.Refresh();

            dataGridView_workbill.Columns.Add("#", "#");
            dataGridView_workbill.Columns.Add("Bill#", "Bill#");

            dataGridView_workbill.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workbill.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            var dtWorkinfo = BankHost_main.Host.Host_Get_Workinfo_All();

            int nCount = dtWorkinfo.Rows.Count;

            if (nCount < 1)
                return 0;

            int nBillcount = 0;

            string strToday = string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            for (int n = 0; n < nCount; n++)
            {
                string strDate = dtWorkinfo.Rows[n]["DATETIME"].ToString(); strDate = strDate.Trim();
                strDate = strDate.Substring(0, 8);


                string strEqid = dtWorkinfo.Rows[n]["EQID"].ToString(); strEqid = strEqid.Trim();
                string strHAWB = dtWorkinfo.Rows[n]["HAWB"].ToString(); strHAWB = strHAWB.Trim();

                if (strEqid == BankHost_main.strEqid && strToday == strDate && strHAWB != "") // 0505                
                {
                    nBillcount++;
                    dataGridView_workbill.Rows.Add(new object[2] { nBillcount, strHAWB });
                }
            }

            dataGridView_workbill.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_workbill.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView_workinfo.Columns.Clear();
            dataGridView_workinfo.Rows.Clear();
            dataGridView_workinfo.Refresh();

            label_error.Text = "";
            label_wait.Text = "";
            label_work.Text = "";
            label_complete.Text = "";
            label_gr.Text = "";

            return nCount;
        }

        public void Gr_GetInfo(string strBill)
        {
            dataGridView_workinfo.Columns.Clear();
            dataGridView_workinfo.Rows.Clear();
            dataGridView_workinfo.Refresh();

            //dataGridView_workinfo.Columns.Add("#", "#");
            dataGridView_workinfo.Columns.Add("BILL#", "BILL#");
            dataGridView_workinfo.Columns.Add("CUST", "CUST");
            dataGridView_workinfo.Columns.Add("DEVICE", "DEVICE");
            dataGridView_workinfo.Columns.Add("LOT#", "LOT#");
            dataGridView_workinfo.Columns.Add("DIE_TTL", "DIE_TTL");
            dataGridView_workinfo.Columns.Add("WFR_QTY", "WFR_QTY");
            dataGridView_workinfo.Columns.Add("WFR_TTL", "WFR_TTL");
            dataGridView_workinfo.Columns.Add("AMKOR_ID", "AMKOR_ID");
            dataGridView_workinfo.Columns.Add("Validation", "Validation");
            dataGridView_workinfo.Columns.Add("GR처리", "GR처리");
            dataGridView_workinfo.Columns.Add("SHIPMENT", "SHIPMENT");
            dataGridView_workinfo.Columns.Add("REELID", "Reel ID");
            dataGridView_workinfo.Columns.Add("REELDCC", "Reel DCC");

            dataGridView_workinfo.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[12].SortMode = DataGridViewColumnSortMode.NotSortable;

            string strFileName = "";

            strFileName = BankHost_main.Host.Host_Get_JobfileName(BankHost_main.strEqid, strBill);
            if (strFileName != "")
                Fnc_WorkDownload(strFileName);
            else
            {
                MessageBox.Show("작업 이력을 불러 올 수 없습니다.!");
                return;
            }

            System.Windows.Forms.Application.DoEvents();

            int nLotcount = dataGridView_sort.Rows.Count;

            int nCount = 0;
            int nWait = 0, nWork = 0, nComplete = 0, nError = 0, nGr = 0;

            for (int n = 0; n < nLotcount; n++)
            {
                string strGetBill = dataGridView_sort.Rows[n].Cells[10].Value.ToString();
                string strGetCust = dataGridView_sort.Rows[n].Cells[1].Value.ToString();
                string strGetDevice = dataGridView_sort.Rows[n].Cells[2].Value.ToString();
                string strGetLot = dataGridView_sort.Rows[n].Cells[3].Value.ToString();
                string strGetDiettl = dataGridView_sort.Rows[n].Cells[4].Value.ToString();
                string strGetWfrttl = dataGridView_sort.Rows[n].Cells[6].Value.ToString();
                string strGetWfrqty = dataGridView_sort.Rows[n].Cells[7].Value.ToString();
                string strGetAmkorid = dataGridView_sort.Rows[n].Cells[11].Value.ToString();
                string strGetVali = dataGridView_sort.Rows[n].Cells[14].Value.ToString();
                string strGetGr = dataGridView_sort.Rows[n].Cells[16].Value.ToString();
                string strGetShipment = dataGridView_sort.Rows[n].Cells[17].Value.ToString();
                string strGetReelID = dataGridView_sort.Rows[n].Cells[18].Value.ToString();
                string strGetReelDCC = dataGridView_sort.Rows[n].Cells[19].Value.ToString();


                if (strGetBill == strBill)
                {
                    nCount++;
                    dataGridView_workinfo.Rows.Add(new object[] { strGetBill, strGetCust, strGetDevice, strGetLot, strGetDiettl,
                        strGetWfrqty, strGetWfrttl,strGetAmkorid, strGetVali,strGetGr, strGetShipment, strGetReelID, strGetReelDCC});

                    if (strGetVali == "Waiting")
                    {
                        dataGridView_workinfo.Rows[n].DefaultCellStyle.BackColor = Color.LightGray;
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.Black;

                        nWait++;
                    }
                    else if (strGetVali == "Working")
                    {
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.BackColor = Color.LightGray;
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.White;

                        nWork++;
                    }
                    else if (strGetVali == "Complete")
                    {
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.BackColor = Color.Blue;
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.White;

                        nComplete++;
                    }
                    else if (strGetVali == "Error")
                    {
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.BackColor = Color.Red;
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.White;

                        nError++;
                    }

                    if (strGetGr == "COMPLETE")
                    {
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.BackColor = Color.DarkBlue;
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.White;

                        nGr++;
                    }
                    else if (strGetGr == "ERROR")
                    {
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.BackColor = Color.Red;
                        dataGridView_workinfo.Rows[nCount - 1].DefaultCellStyle.ForeColor = Color.White;
                    }
                }
            }

            dataGridView_workinfo.Sort(this.dataGridView_workinfo.Columns["SHIPMENT"], ListSortDirection.Ascending);

            dataGridView_workinfo.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_workinfo.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_workinfo.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_workinfo.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            //dataGridView_workinfo.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;

            label_error.Text = nError.ToString();
            label_wait.Text = nWait.ToString();
            label_work.Text = nWork.ToString();
            label_complete.Text = nComplete.ToString();
            label_gr.Text = nGr.ToString();
        }

        public void Gr_GetInfo_Shipment(string strBill)
        {
            dataGridView_shipment.Columns.Clear();
            dataGridView_shipment.Rows.Clear();
            dataGridView_shipment.Refresh();

            //dataGridView_workinfo.Columns.Add("#", "#");
            dataGridView_shipment.Columns.Add("Shipment", "Shipment");

            dataGridView_shipment.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "선택";
            checkBoxColumn.Width = 30;
            checkBoxColumn.Name = "checkBoxColumn";

            string strFileName = "";

            strFileName = BankHost_main.Host.Host_Get_JobfileName(BankHost_main.strEqid, strBill);
            if (strFileName != "")
                Fnc_WorkDownload(strFileName);
            else
            {
                MessageBox.Show("작업 이력을 불러 올 수 없습니다.!");
                return;
            }

            int nLotcount = dataGridView_sort.Rows.Count;
            int nColumnCount = dataGridView_sort.Columns.Count;

            string strShipment = "-1";
            int nAddcount = 0;
            for (int n = 0; n < nLotcount; n++)
            {
                if (nColumnCount > 12)
                {
                    string strGetBill = dataGridView_sort.Rows[n].Cells[10].Value.ToString();
                    string strGetShipment = dataGridView_sort.Rows[n].Cells[17].Value.ToString();

                    if (strGetBill == strBill)
                    {
                        if (strShipment != strGetShipment)
                        {
                            strShipment = strGetShipment;
                            int nCurrentcount = dataGridView_shipment.Rows.Count;

                            bool bDuplicate = false;

                            if (nCurrentcount > 0)
                            {
                                for (int p = 0; p < nCurrentcount; p++)
                                {
                                    string strGetShip = dataGridView_shipment.Rows[p].Cells[0].Value.ToString();

                                    if (strGetShip == strShipment)
                                    {
                                        bDuplicate = true;
                                        p = nCurrentcount;
                                    }
                                }
                            }

                            if (!bDuplicate)
                            {
                                dataGridView_shipment.Rows.Add(new object[1] { strShipment });

                                int nCheck = Gr_ShipmentCheckValidation(strShipment);
                                if (nCheck == 2)
                                {
                                    //dataGridView_shipment.Rows[nAddcount].Cells[0].Value = true;
                                    dataGridView_shipment.Rows[nAddcount].DefaultCellStyle.BackColor = Color.DarkBlue;
                                    dataGridView_shipment.Rows[nAddcount].DefaultCellStyle.ForeColor = Color.White;
                                }
                                nAddcount++;
                            }
                        }
                    }
                }
            }

            if (dataGridView_shipment.Rows.Count > 0)
            {
                dataGridView_shipment.Columns.Insert(0, checkBoxColumn);
                dataGridView_shipment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                dataGridView_shipment.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView_shipment.Sort(this.dataGridView_shipment.Columns["SHIPMENT"], ListSortDirection.Ascending);

                for (int k = 0; k < dataGridView_shipment.Rows.Count; k++)
                {
                    if (dataGridView_shipment.Rows[k].DefaultCellStyle.BackColor == Color.DarkBlue)
                        dataGridView_shipment.Rows[k].Cells[0].Value = true;
                    else
                        dataGridView_shipment.Rows[k].Cells[0].Value = false;
                }

                dataGridView_shipment.ClearSelection();
            }

            Fnc_GetGrList();
        }

        public int Gr_ShipmentCheckValidation(string strShip)
        {
            int nCount = dataGridView_shipment.RowCount;

            List<StorageData> list = new List<StorageData>();

            string strFileName = "";

            int n = dataGridView_workbill.CurrentCell.RowIndex;

            if (n < 0)
            {
                string strMsg = string.Format("Bill이 선택 되지 않았습니다.\n\n먼저 Bill을 선택 하세요");
                Frm_Process.Form_Show(strMsg);
                Frm_Process.Form_Display_Warning(strMsg);
                Thread.Sleep(3000);
                Frm_Process.Form_Hide();
                return 0;
            }

            string strBill = dataGridView_workbill.Rows[n].Cells[1].Value.ToString();

            strFileName = BankHost_main.Host.Host_Get_JobfileName(BankHost_main.strEqid, strBill);
            if (strFileName != "")
                Fnc_WorkDownload(strFileName);

            for (int i = 0; i < nCount; i++)
            {
                int nLotcount = dataGridView_sort.Rows.Count;
                for (int j = 0; j < nLotcount; j++)
                {
                    StorageData data = new StorageData();

                    data.Bill = dataGridView_sort.Rows[j].Cells[10].Value.ToString();
                    data.Cust = dataGridView_sort.Rows[j].Cells[1].Value.ToString();
                    data.Device = dataGridView_sort.Rows[j].Cells[2].Value.ToString();
                    data.Lot = dataGridView_sort.Rows[j].Cells[3].Value.ToString();
                    data.Die_Qty = dataGridView_sort.Rows[j].Cells[4].Value.ToString();
                    data.Default_WQty = dataGridView_sort.Rows[j].Cells[6].Value.ToString();
                    data.Rcv_WQty = dataGridView_sort.Rows[j].Cells[7].Value.ToString();
                    data.Amkorid = dataGridView_sort.Rows[j].Cells[11].Value.ToString();
                    data.state = dataGridView_sort.Rows[j].Cells[14].Value.ToString();
                    data.strGRstatus = dataGridView_sort.Rows[j].Cells[16].Value.ToString();
                    data.shipment = dataGridView_sort.Rows[j].Cells[17].Value.ToString();

                    if (data.Bill == strBill && data.shipment == strShip)
                    {
                        if (data.state == "Waiting" || data.state == "Error")
                            return 1;
                    }
                }
            }

            return 2;
        }
        public bool Gr_Process(string strInDevice, string strInLot)
        {
            bool bjudge = false;

            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
            string strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strInDevice + "\\" + strInDevice;

            string strGRinfo = Fnc_Update_GR(strInDevice, strInLot, "START");

            if (strGRinfo == "")
            {
                Fnc_Update_GR(strGR_Device, strGR_Lot, "ERROR");
                return false;
            }

            try
            {
                var taskResut = Task.Run(async () =>
                {
                    return await BankHost_main.Host.Fnc_AutoGR(strGRinfo);
                });

                string strResult = taskResut.Result;

                if (strResult.Contains("SUCCESS") || strResult.Contains("DUPLICATE"))
                {
                    Fnc_Update_GR(strInDevice, strInLot, "COMPLETE");
                    bjudge = true;
                }
                else
                {
                    var taskResut2 = Task.Run(async () =>
                    {
                        return await BankHost_main.Host.Fnc_AutoGR(strGRinfo);
                    });

                    strResult = taskResut2.Result;

                    if (strResult.Contains("SUCCESS") || strResult.Contains("DUPLICATE"))
                    {
                        Fnc_Update_GR(strInDevice, strInLot, "COMPLETE");
                        bjudge = true;
                    }
                    else
                    {
                        Fnc_Update_GR(strInDevice, strInLot, "ERROR");
                        bjudge = false;
                    }
                }
            }
            catch
            {
                var taskResut = Task.Run(async () =>
                {
                    return await BankHost_main.Host.Fnc_AutoGR(strGRinfo);
                });

                string strResult = taskResut.Result;

                if (strResult.Contains("SUCCESS") || strResult.Contains("DUPLICATE"))
                {
                    Fnc_Update_GR(strInDevice, strInLot, "COMPLETE");
                    bjudge = true;
                }
                else
                {
                    Fnc_Update_GR(strInDevice, strInLot, "ERROR");
                    bjudge = false;
                }
            }

            return bjudge;
        }

        public bool Gr_Process_Direct(string strDevice, string strLot, string strAmkorid, string strDieQty, string strWfrQty)
        {
            bool bjudge = false;

            string strgr = string.Format("{0};{1};{2}", strAmkorid, strDieQty, strWfrQty);

            try
            {
                //var taskResut = Task.Run(async () =>
                //{
                //    return await BankHost_main.Host.Fnc_AutoGR(strgr);//PRD
                //});
                //string strResult = taskResut.Result;

                string strResult = GetWebServiceData($"http://10.101.1.37:9080/eMES/diebank/lotEntryMultiLotCreateAll.do?serviceRequestor=AutoGR&GR_INFO={strgr}");//$"http://10.131.201.33:9080/eMES/diebank/lotEntryMultiLotCreateAll.do?serviceRequestor=AutoGR&GR_INFO={strgr}");



                if (strResult.Contains("SUCCESS"))
                {
                    Gr_Process_Update(strDevice, strLot);
                    bjudge = true;
                }
                else
                {
                    var taskResut2 = Task.Run(async () =>
                    {
                        return await BankHost_main.Host.Fnc_AutoGR(strgr);
                    });

                    string strResult2 = taskResut2.Result;

                    if (strResult.Contains("SUCCESS"))
                    {
                        Fnc_Update_GR(strDevice, strLot, "COMPLETE");
                        bjudge = true;
                    }
                    else
                    {
                        Fnc_Update_GR(strDevice, strLot, "ERROR");
                        bjudge = false;
                    }
                }
            }
            catch
            {
                var taskResut = Task.Run(async () =>
                {
                    return await BankHost_main.Host.Fnc_AutoGR(strgr);
                });

                string strResult = taskResut.Result;

                if (strResult.Contains("SUCCESS") || strResult.Contains("STATION"))
                {
                    Fnc_Update_GR(strDevice, strLot, "COMPLETE");
                    bjudge = true;
                }
                else
                {
                    Fnc_Update_GR(strDevice, strLot, "ERROR");
                    bjudge = false;
                }
            }

            return bjudge;
        }

        public bool Gr_Process_Direct(string strDevice, string strLot, string strAmkorid, string strDieQty, string strWfrQty, string strReelID, string strReelDCC)
        {
            bool bjudge = false;

            string strgr = string.Format("{0};{1};{2}", strAmkorid, strDieQty, strWfrQty);

            try
            {
                //var taskResut = Task.Run(async () =>
                //{
                //    return await BankHost_main.Host.Fnc_AutoGR(strgr);//PRD
                //});
                //string strResult = taskResut.Result;

                //string strResult = GetWebServiceData($"http://10.101.1.37:9080/eMES/diebank/lotEntryMultiLotCreateAll.do?serviceRequestor=AutoGR&GR_INFO={strgr}");//$"http://10.131.201.33:9080/eMES/diebank/lotEntryMultiLotCreateAll.do?serviceRequestor=AutoGR&GR_INFO={strgr}");

                string strResult = "";// GetWebServiceData($"http://10.101.1.130:8080/eMES_Webservice/diebank_automation_service/rec_reel_inf/{strAmkorid},{strReelID},{strReelDCC},{strDieQty},{BankHost_main.strID}");

                strResult = InsertReelID($"http://{(Properties.Settings.Default.TestMode == true ? TEST_MES : PRD_MES)}/eMES_Webservice/diebank_automation_service/rec_reel_inf/{strAmkorid},{strReelID},{(strReelDCC == "" ? " " : strReelDCC)},{strDieQty},{BankHost_main.strID}").Result;

                //try
                //{
                //    using (var client = new HttpClient())
                //    {
                //        client.BaseAddress = new Uri($"http://10.101.1.130:8080/eMES_Webservice/diebank_automation_service/rec_reel_inf/{strAmkorid},{strReelID},{strReelDCC},{strDieQty},{BankHost_main.strID}");
                //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/HY"));

                //        HttpResponseMessage response = client.GetAsync("").Result;
                //        if (response.IsSuccessStatusCode)
                //        {
                //            var contents = response.Content.ReadAsStringAsync();
                //            strResult = contents.ToString();
                //        }
                //    }
                //}
                //catch (WebException ex)
                //{

                //    throw;
                //}



                if (strResult.ToUpper() == "OK")
                {
                    string res = Fnc_RunAsync($"http://{(Properties.Settings.Default.TestMode == true ? TEST_AutoGRConfirm : PRD_AutoGRConfirm)}/eMES/diebank/lotEntryMultiLotCreateAll.do?serviceRequestor=AutoGR&GR_INFO={strgr}").Result;
                    if (res.Contains("SUCCESS") == true)
                    {
                        Gr_Process_Update(strDevice, strLot);
                        bjudge = true;
                    }
                }
                else
                {

                    //var taskResut2 = Task.Run(async () =>
                    //{
                    //    return await BankHost_main.Host.Fnc_AutoGR(strgr);
                    //});

                    //string strResult2 = taskResut2.Result;

                    //if (strResult.Contains("SUCCESS"))
                    //{
                    //    Fnc_Update_GR(strDevice, strLot, "COMPLETE");
                    //    bjudge = true;
                    //}
                    //else
                    //{
                    //    Fnc_Update_GR(strDevice, strLot, "ERROR");
                    //    bjudge = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                //var taskResut = Task.Run(async () =>
                //{
                //    return await BankHost_main.Host.Fnc_AutoGR(strgr);
                //});

                //string strResult = taskResut.Result;

                //if (strResult.Contains("SUCCESS"))
                //{
                //    Fnc_Update_GR(strDevice, strLot, "COMPLETE");
                //    bjudge = true;
                //}
                //else
                //{
                //    Fnc_Update_GR(strDevice, strLot, "ERROR");
                //    bjudge = false;
                //}
            }

            return bjudge;
        }
        public string Gr_Process_Update(string strDevice, string strLot)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
            string strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + ".txt";
            strValReadfile = strFileName + "\\" + strDevice + "\\" + strDevice + ".txt";

            string strSaveFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
            string strSaveFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strDevice + "\\" + strDevice;
            string strlog = "";

            int dataIndex = Fnc_Getline_GR(strValReadfile, strLot, "", "", false);
            int deviceindex = Fnc_Getline_GR(strFileName_Device, strDevice, "", "", false);

            string strSpeak = "";

            if (dataIndex == -1 || deviceindex == -1)
            {
                label_msg.Text = "리스트에 없는 자재 입니다.";

                //if (BankHost_main.nInputMode == 1)
                //{
                strSpeak = string.Format("리스트에 없는 자재 입니다.");
                speech.SpeakAsync(strSpeak);
                //}

                return "";
            }

            string[] info = Fnc_ReadFile(strValReadfile);
            string[] strSplit_data = info[dataIndex].Split('\t');

            StorageData st = new StorageData();

            st.Cust = strSplit_data[0];
            st.Device = strSplit_data[1];
            st.Lot = strSplit_data[2];
            st.Lot_Dcc = strSplit_data[3];
            st.Rcv_Qty = strSplit_data[4];
            st.Die_Qty = strSplit_data[5];
            st.Rcv_WQty = strSplit_data[6];
            st.Rcvddate = strSplit_data[7];
            st.Lot_type = strSplit_data[8];
            st.Bill = strSplit_data[9];
            st.Amkorid = strSplit_data[10];
            st.Wafer_lot = strSplit_data[11];
            st.strCoo = strSplit_data[12];
            st.state = strSplit_data[13];
            st.strop = strSplit_data[14];
            st.strGRstatus = "COMPLETE"; //상태 업데이트
            st.Default_WQty = strSplit_data[16];


            if (strSplit_data.Length >= 19)
            {
                st.WSN = strSplit_data[18];
                st.ReelID = strSplit_data[19];
                st.ReelDCC = strSplit_data[20];
            }

            st.strop = BankHost_main.strOperator;


            if (strSplit_data.Length > 17)
                st.shipment = strSplit_data[17];

            strlog = string.Format("GR+{0}+{1}+{2}+{3}+{4}+{5}+{6}", strDevice, strLot, st.Die_Qty, st.Rcv_Qty, st.Default_WQty, "COMPLETE", BankHost_main.strOperator);

            ////DB Save
            string[] strSaveInfo = new string[10];
            strSaveInfo[0] = BankHost_main.strEqid;
            strSaveInfo[1] = "GR_COMPLETE";
            strSaveInfo[2] = st.Bill;
            strSaveInfo[3] = strDevice;
            strSaveInfo[4] = strLot;
            strSaveInfo[5] = st.Die_Qty;
            strSaveInfo[6] = st.Rcv_Qty;
            strSaveInfo[7] = st.Rcv_WQty;
            strSaveInfo[8] = st.Default_WQty;
            strSaveInfo[9] = BankHost_main.strOperator;

            Fnc_SaveLog_Work(strSaveFileName_Device, strlog, strSaveInfo, 1);

            string strTxtline = st.Cust + "\t" + st.Device + "\t" + st.Lot + "\t" + st.Lot_Dcc + "\t" + st.Rcv_Qty + "\t" + st.Die_Qty + "\t" +
                    st.Rcv_WQty + "\t" + st.Rcvddate + "\t" + st.Lot_type + "\t" + st.Bill + "\t" + st.Amkorid + "\t" + st.Wafer_lot + "\t" +
                    st.strCoo + "\t" + st.state + "\t" + st.strop + "\t" + st.strGRstatus + "\t" + st.Default_WQty + "\t" + st.shipment + "\t" + st.WSN + "\t" + st.ReelID + "\t" + st.ReelDCC;

            info[dataIndex] = strTxtline;
            File.WriteAllLines(strValReadfile, info);

            return "OK";
        }

        int CompareStorageData(StorageData obj1, StorageData obj2)
        {
            return obj1.Device.CompareTo(obj2.Device);
        }
        int CompareStorageData_Lot(StorageData obj1, StorageData obj2)
        {
            return obj1.Lot.CompareTo(obj2.Lot);
        }

        int CompareStorageData_Bill(StorageData obj1, StorageData obj2)
        {
            return obj1.Bill.CompareTo(obj2.Bill);
        }

        public void Fnc_ExcelDownload(string strFileName)
        {
            Frm_Process.Form_Show("\n\n작업 조회 시작 합니다.");
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            string strToday = string.Format("{0}{1:00}{2:00}_", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string strTime = string.Format("{0:00}{1:00}{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            string strFileVersion = strToday + strTime;

            if (xlApp == null)
            {
                MessageBox.Show("Excel is NOT properly installed!!");
                return;
            }

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Open(strFileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            range = xlWorkSheet.UsedRange;
            int rw = range.Rows.Count;
            int cl = range.Columns.Count;

            List<StorageData> list = new List<StorageData>();

            int nCount = 0;

            string strMsg = string.Format("\n\nSorting 시작");
            Frm_Process.Form_Display(strMsg);

            for (int i = 2; i <= rw; i++)
            {
                nCount++;
                strMsg = string.Format("\n\n데이터 Read {0} / {1}", nCount, rw - 1);
                Frm_Process.Form_Display(strMsg);

                StorageData data = new StorageData();

                for (int j = 1; j <= 11; j++)
                {

                    var strType = (range.Cells[i, j] as Excel.Range).Value2;
                    //string str = (string)(range.Cells[i, j] as Excel.Range).Value2;

                    string str = "";
                    if (strType != null)
                    {
                        if (j != 7)
                            str = strType.ToString();
                        else
                        {
                            DateTime conv = DateTime.FromOADate(strType);
                            str = string.Format("{0}/{1}/{2}", conv.Year, conv.Month, conv.Day);
                        }
                    }

                    if (j == 1) //Cust
                    {
                        if (str == null)
                            str = "";

                        str = str.Trim();
                        data.Cust = str;
                    }
                    else if (j == 2) //Device
                    {
                        str = str.Trim();
                        data.Device = str;
                    }
                    else if (j == 3) //Lot#
                    {
                        str = str.Trim();
                        data.Lot = str;
                    }
                    else if (j == 4)//DCC
                    {
                        str = str.Trim();
                        data.Lot_Dcc = str;
                    }
                    else if (j == 5) //DieQty
                    {
                        str = str.Trim();
                        //string strnQty = string.Format("{0:0,0}", Int32.Parse(str));
                        data.Rcv_Qty = str;
                    }
                    else if (j == 6) //Wafer Qty
                    {
                        str = str.Trim();
                        //data.Rcv_WQty = str;
                        data.Rcv_WQty = "0";
                        data.Default_WQty = str;
                    }
                    else if (j == 7) //RCV date
                    {
                        str = str.Trim();
                        data.Rcvddate = str;
                    }
                    else if (j == 8) //Lot Type
                    {
                        str = str.Trim();
                        data.Lot_type = str;
                    }
                    else if (j == 9) //Bill
                    {
                        str = str.Trim();
                        data.Bill = str;
                    }
                    else if (j == 10) //Amkor id
                    {
                        str = str.Trim();
                        data.Amkorid = str;
                    }
                    else if (j == 11) //wfr lot
                    {
                        str = str.Trim();
                        data.Wafer_lot = str;
                    }
                    else if (j == 12) //coo
                    {
                        str = str.Trim();
                        data.strCoo = str;
                    }
                }

                list.Add(data);
            }

            nCount = 0;

            list.Sort(CompareStorageData);

            string strSavepath = "", strSetFileName = "", strSetFolder = "";

            string[] strSplit = strFileName.Split('\\');
            int nLength = strSplit.Length;

            strSetFolder = strSplit[nLength - 1].Substring(0, strSplit[nLength - 1].Length - 5);
            strSetFolder = strSetFolder.Trim();
            strSetFileName = strSetFolder + ".txt";

            strSavepath = strExcutionPath + "\\Work\\" + strSetFileName;

            string sDirFileNamePath = "", sDirDeviceNamePath = "";
            sDirFileNamePath = strExcutionPath + "\\Work\\" + strSetFolder;

            /////.txt 파일 만들기
            System.IO.FileInfo fi = new System.IO.FileInfo(strSavepath);

            if (fi.Exists)
            {
                File.Delete(strSavepath);
                /////폴더,폴더, 파일 삭제
                DirectoryInfo dir = new DirectoryInfo(sDirFileNamePath);
                dir.Delete(true);

            }
            ///파일 이름 폴더 만들기            
            DirectoryInfo di = new DirectoryInfo(sDirFileNamePath);
            if (di.Exists == false)
            {
                di.Create();
            }
            ////////////////////////////////////////            

            string strDevicecheck = "";
            foreach (var item in list)
            {
                item.state = "Waiting";
                item.strop = "";
                item.Die_Qty = "0";
                item.strGRstatus = "Ready";

                string strTxtline = item.Cust + "\t" + item.Device + "\t" + item.Lot + "\t" + item.Lot_Dcc + "\t" + item.Rcv_Qty + "\t" + item.Die_Qty + "\t" +
                    item.Rcv_WQty + "\t" + item.Rcvddate + "\t" + item.Lot_type + "\t" + item.Bill + "\t" + item.Amkorid + "\t" + item.Wafer_lot + "\t" + item.strCoo + "\t" +
                    item.state + "\t" + item.strop + "\t" + item.strGRstatus + "\t" + item.Default_WQty + "\t" + item.shipment + $"\t{item.ReelID}\t{item.ReelDCC}\t";

                if (strDevicecheck != item.Device)
                {
                    Fnc_WriteFile(strSavepath, item.Device);
                    strDevicecheck = item.Device;
                }

                /////////////////////////////////////Device 폴더 생성
                sDirDeviceNamePath = sDirFileNamePath + "\\" + item.Device;
                DirectoryInfo diinfo = new DirectoryInfo(sDirDeviceNamePath);
                if (diinfo.Exists == false)
                {
                    diinfo.Create();
                }
                diinfo = null;
                /////////////////////////////////////File 저장
                string strLotsavepath = sDirDeviceNamePath + "\\" + item.Device + ".txt";
                Fnc_WriteFile(strLotsavepath, strTxtline);
                ////////////////////////////////////

                nCount++;
                strMsg = string.Format("\n\n 작업 준비 중 입니다. {0} / {1}", nCount, rw - 1);
                Frm_Process.Form_Display(strMsg);

                System.Windows.Forms.Application.DoEvents();
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Fnc_WorkView(strWorkFileName);

            Frm_Process.Form_Display("\n작업을 마침니다.");
            Frm_Process.Hide();
        }

        public void Fnc_ExcelDownload2(string strFileName)
        {
            Frm_Process.Form_Show("\n\n작업 조회 시작 합니다.");
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            string strToday = string.Format("{0}{1:00}{2:00}_", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string strTime = string.Format("{0:00}{1:00}{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            string strFileVersion = strToday + strTime;

            if (xlApp == null)
            {
                MessageBox.Show("Excel is NOT properly installed!!");
                return;
            }

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Open(strFileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            range = xlWorkSheet.UsedRange;
            int rw = range.Rows.Count;
            int cl = range.Columns.Count;

            List<StorageData> list = new List<StorageData>();

            int nCount = 0;

            string strMsg = string.Format("\n\nSorting 시작");
            Frm_Process.Form_Display(strMsg);

            for (int i = 2; i <= rw; i++)
            {
                nCount++;
                strMsg = string.Format("\n\n데이터 Read {0} / {1}", nCount, rw - 1);
                Frm_Process.Form_Display(strMsg);

                StorageData data = new StorageData();

                for (int j = 1; j <= 35; j++)
                {

                    var strType = (range.Cells[i, j] as Excel.Range).Value2;
                    //string str = (string)(range.Cells[i, j] as Excel.Range).Value2;

                    string str = "";
                    if (strType != null)
                    {
                        if (j != 13)
                            str = strType.ToString();
                        else
                        {
                            DateTime conv = DateTime.FromOADate(strType);
                            str = string.Format("{0}/{1}/{2}", conv.Year, conv.Month, conv.Day);
                        }
                    }

                    if (j == 3) //Cust
                    {
                        if (str == null)
                            str = "";

                        str = str.Trim();
                        data.Cust = str;
                    }
                    else if (j == 5) //Device
                    {
                        str = str.Trim();
                        data.Device = str;
                    }
                    else if (j == 7) //Lot#
                    {
                        str = str.Trim();
                        data.Lot = str;
                    }
                    else if (j == 8)//DCC
                    {
                        str = str.Trim();
                        data.Lot_Dcc = str;
                    }
                    else if (j == 10) //DieQty
                    {
                        str = str.Trim();
                        //string strnQty = string.Format("{0:0,0}", Int32.Parse(str));
                        data.Rcv_Qty = str;
                    }
                    else if (j == 11) //Wafer Qty
                    {
                        str = str.Trim();
                        //data.Rcv_WQty = str;
                        data.Rcv_WQty = "0";
                        data.Default_WQty = str;
                    }
                    else if (j == 13) //RCV date
                    {
                        str = str.Trim();
                        data.Rcvddate = str;
                    }
                    else if (j == 23) //Lot Type
                    {
                        str = str.Trim();
                        data.Lot_type = str;
                    }
                    else if (j == 25) //Bill
                    {
                        str = str.Trim();
                        data.Bill = str;
                    }
                    else if (j == 34) //Amkor id
                    {
                        str = str.Trim();
                        data.Amkorid = str;
                    }
                    else if (j == 32) //wfr lot
                    {
                        str = str.Trim();
                        data.Wafer_lot = "";
                    }
                    else if (j == 33) //coo
                    {
                        str = str.Trim();
                        data.strCoo = "";
                    }
                }

                list.Add(data);
                System.Windows.Forms.Application.DoEvents();
            }

            nCount = 0;

            list.Sort(CompareStorageData);

            string strSavepath = "", strSetFileName = "", strSetFolder = "";

            string[] strSplit = strFileName.Split('\\');
            int nLength = strSplit.Length;

            strSetFolder = strSplit[nLength - 1].Substring(0, strSplit[nLength - 1].Length - 5);
            strSetFolder = strSetFolder.Trim();
            strSetFileName = strSetFolder + ".txt";

            strSavepath = strExcutionPath + "\\Work\\" + strSetFileName;

            string sDirFileNamePath = "", sDirDeviceNamePath = "";
            sDirFileNamePath = strExcutionPath + "\\Work\\" + strSetFolder;

            /////.txt 파일 만들기
            System.IO.FileInfo fi = new System.IO.FileInfo(strSavepath);

            if (fi.Exists)
            {
                File.Delete(strSavepath);
                /////폴더,폴더, 파일 삭제
                DirectoryInfo dir = new DirectoryInfo(sDirFileNamePath);
                dir.Delete(true);

            }
            ///파일 이름 폴더 만들기            
            DirectoryInfo di = new DirectoryInfo(sDirFileNamePath);
            if (di.Exists == false)
            {
                di.Create();
            }
            ////////////////////////////////////////            

            string strDevicecheck = "";
            foreach (var item in list)
            {
                item.state = "Waiting";
                item.strop = "";
                item.Die_Qty = "0";
                item.strGRstatus = "Ready";

                strSelCust = item.Cust;

                string strTxtline = item.Cust + "\t" + item.Device + "\t" + item.Lot + "\t" + item.Lot_Dcc + "\t" + item.Rcv_Qty + "\t" + item.Die_Qty + "\t" +
                    item.Rcv_WQty + "\t" + item.Rcvddate + "\t" + item.Lot_type + "\t" + item.Bill + "\t" + item.Amkorid + "\t" + item.Wafer_lot + "\t" + item.strCoo + "\t" +
                    item.state + "\t" + item.strop + "\t" + item.strGRstatus + "\t" + item.Default_WQty + "\t" + item.shipment + $"\t{item.ReelID}\t{item.ReelDCC}\t";

                if (strDevicecheck != item.Device)
                {
                    Fnc_WriteFile(strSavepath, item.Device);
                    strDevicecheck = item.Device;
                }

                /////////////////////////////////////Device 폴더 생성
                sDirDeviceNamePath = sDirFileNamePath + "\\" + item.Device;
                DirectoryInfo diinfo = new DirectoryInfo(sDirDeviceNamePath);
                if (diinfo.Exists == false)
                {
                    diinfo.Create();
                }
                diinfo = null;
                /////////////////////////////////////File 저장
                string strLotsavepath = sDirDeviceNamePath + "\\" + item.Device + ".txt";
                Fnc_WriteFile(strLotsavepath, strTxtline);
                ////////////////////////////////////

                nCount++;
                strMsg = string.Format("\n\n 작업 준비 중 입니다. {0} / {1}", nCount, rw - 1);
                Frm_Process.Form_Display(strMsg);

                System.Windows.Forms.Application.DoEvents();
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Fnc_WorkView(strWorkFileName);

            Frm_Process.Form_Display("\n작업을 마침니다.");
            Frm_Process.Hide();
        }

        public void Fnc_WriteFile(string strFileName, string strLine)
        {
            strLine = strLine.Replace("\n", "");

            try
            {
                if (System.IO.File.Exists(strFileName) == false)
                {
                    FileInfo fi = new FileInfo(strFileName);

                    fi.Create().Close();
                    //System.IO.File.Create(strFileName).Close();
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFileName, true))
                {
                    file.WriteLine(strLine);
                }
            }
            catch
            {

            }
        }

        public string[] Fnc_ReadFile(string strPath)
        {
            if (!System.IO.File.Exists(strPath))
            {
                return null;
            }
            else
            {
                string[] lines = System.IO.File.ReadAllLines(strPath);
                int nLength = lines.Length;

                if (nLength != 0)
                    return lines;
                else
                    return null;
            }
        }

        private void textBox_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                //Find
                Fnc_Find(textBox_search.Text);
            }
        }

        private void tabControl_Sort_SelectedIndexChanged(object sender, EventArgs e)
        {
            strBcrType = "";

            Properties.Settings.Default.LabelCopy = false;
            Properties.Settings.Default.Save();

            if (bGRrun)
                return;

            nLabelcount = 0;
            int n = tabControl_Sort.SelectedIndex;
            BankHost_main.nSortTabNo = n;
            AmkorLabelCnt = 1;



            if (bselected_mode_index == true)
            {
                tabControl_Sort.SelectedIndex = 5;
                speech.SpeakAsyncCancelAll();
                speech.SpeakAsync("라벨출력 모드 종료 후 이동 할 수 있습니다.");
                return;
            }


            if (bmode6 == true && n != 6)
            {
                tabControl_Sort.SelectedIndex = 6;
                speech.SpeakAsyncCancelAll();
                speech.SpeakAsync("라벨출력 모드 종료 후 이동 할 수 있습니다.");
                return;
            }

            if (bmode7 == true && n != 7)
            {
                tabControl_Sort.SelectedIndex = 7;
                speech.SpeakAsyncCancelAll();
                speech.SpeakAsync("Split 모드 종료 후 이동 할 수 있습니다.");
                return;
            }

            if (BankHost_main.nWorkMode == 0 && n < 3)
            {
                tabControl_Sort.SelectedIndex = 0;
                return;
            }

            if (n == 0)
            {
                if (BankHost_main.nWorkMode != 0)
                    tabControl_Sort.SelectedIndex = 2;
            }
            else if (n == 1)
            {
                if (BankHost_main.IsGRrun && BankHost_main.nWorkMode != 0)
                {
                    label_wait.Text = "";
                    label_work.Text = "";
                    label_complete.Text = "";
                    label_error.Text = "";
                    label_gr.Text = "";

                    Frm_Process.Form_Show("\n\n데이터 업데이트 진행 중 입니다.");

                    System.Windows.Forms.Application.DoEvents();
                    //Fnc_WorkDownload(strWorkFileName);

                    Gr_GetBillInfo();
                    Frm_Process.Hide();
                }
                else
                {
                    if (BankHost_main.nWorkMode == 0)
                        tabControl_Sort.SelectedIndex = 0;
                    else
                        tabControl_Sort.SelectedIndex = 2;
                }
            }
            else if (n == 2)
            {
                if (strWorkFileName == "" || BankHost_main.strOperator == "")
                {
                    tabControl_Sort.SelectedIndex = 0;
                    return;
                }

                Fnc_WorkDownload(strWorkFileName);
                label_op.Text = BankHost_main.strOperator;


                string[] strSplit_data = Form_Print.strPrinterName.Split(' ');

                if (strSplit_data.Length > 2)
                    label_printinfo.Text = strSplit_data[1];

                if (!Form_Print.bPrintUse)
                {
                    label_printstate.Text = "프린트 사용 안함";
                    label_printstate.ForeColor = Color.Red;
                }

                string strDevice = dataGridView_Device.Rows[0].Cells[1].Value.ToString();

                while (bGridViewUpdate)
                {
                    Thread.Sleep(1);
                    //Application.DoEvents();
                }

                try
                {
                    if (strSelCust == "940")
                    {
                        strSelDevice = strDevice;
                    }

                    Fnc_GetDeviceData(strDevice);
                    BankHost_main.IsGRrun = false;
                }
                catch
                {
                    return;
                }

                textBox_Readdata.Focus();
                textBox_Readdata.ImeMode = ImeMode.Alpha;
            }
            else if (n == 3)
            {
                /*
                if (strWorkFileName == "" || BankHost_main.strOperator == "")
                {
                    tabControl_Sort.SelectedIndex = 0;
                    return;
                }

                Fnc_Hist_DeviceLoad();
                */
                Fnc_Hist_Init();
            }
            else if (n == 4)
            {
                Fnc_Get_Unprinted_Deviceinfo();
                textBox_unprinted_device.Text = "";
            }
            else if (n == 5)
            {
                if (bselected_mode_index == false)
                {
                    tabControl_Sort.SelectedIndex = 0;
                    return;
                }
                else
                {
                    tabControl_Sort.SelectedIndex = 5;
                    return;
                }
            }
            else if (n == 9)
            {
                sdt.Value = DateTime.Now.AddDays(-1);
                edt.Value = DateTime.Now;



            }
            else if (n == 10)
            {
                bDownloadComp = true;
            }
            else if (n == 12)
            {
                BankHost_main.nAmkorBcrType = 0;
                cb_splitMode.SelectedIndex = 0;
                tb_splitScan.Focus();
            }
        }

        public void Fnc_Find(string strSearch)
        {
            dataGridView_sort.ClearSelection();

            int nCount_row = dataGridView_sort.RowCount;
            int nCount_column = dataGridView_sort.ColumnCount;

            bool bfind = false;

            for (int m = 1; m < nCount_column; m++)
            {
                for (int n = 0; n < nCount_row; n++)
                {
                    string str = dataGridView_sort.Rows[n].Cells[m].Value.ToString();

                    if (str == strSearch)
                    {
                        dataGridView_sort.Rows[n].Cells[m].Selected = true;
                        dataGridView_sort.FirstDisplayedScrollingRowIndex = n;
                        bfind = true;
                        n = nCount_row; m = nCount_column;
                    }
                }
            }

            if (bfind)
                return;

            for (int m = 1; m < nCount_column; m++)
            {
                for (int n = 0; n < nCount_row; n++)
                {
                    string str = dataGridView_sort.Rows[n].Cells[m].Value.ToString();

                    if (str.Contains(strSearch))
                    {
                        dataGridView_sort.Rows[n].Cells[m].Selected = true;
                        dataGridView_sort.FirstDisplayedScrollingRowIndex = n;
                        bfind = true;
                        n = nCount_row; m = nCount_column;
                    }
                }
            }
        }

        private void dataGridView_Device_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;

            if (colIndex != 0)
                colIndex = 0;

            if (rowIndex == -1)
                return;

            string strDevice = dataGridView_Device.Rows[rowIndex].Cells[1].Value.ToString();

            while (bGridViewUpdate)
            {
                Thread.Sleep(1);
                System.Windows.Forms.Application.DoEvents();
            }

            try
            {
                if (strSelCust == "940")
                {
                    strSelDevice = strDevice;
                }

                Fnc_GetDeviceData(strDevice);

            }
            catch
            {
                return;
            }
        }

        private void dataGridView_Lot_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView_Lot.Columns["재작업"] != null)
            {
                if (e.ColumnIndex == dataGridView_Lot.Columns["재작업"].Index)
                {
                    //Do something with your button.
                    int rowIndex = e.RowIndex;
                    int colIndex = e.ColumnIndex;

                    if (colIndex != 0)
                        colIndex = 0;

                    if (rowIndex == -1)
                        return;

                    string strGrState = dataGridView_Lot.Rows[rowIndex].Cells[11].Value.ToString();

                    if (strGrState == "COMPLETE")
                        return;

                    DialogResult dialogResult1 = MessageBox.Show("작업 이력이 초기화 됩니다. \n\n처음부터 다시 작업을 하시겠습니까?", "Warning", MessageBoxButtons.YesNo);
                    if (dialogResult1 == DialogResult.Yes)
                    {
                        string strDevice = strSelDevice;
                        string strLot = dataGridView_Lot.Rows[rowIndex].Cells[1].Value.ToString();
                        string strDcc = dataGridView_Lot.Rows[rowIndex].Cells[2].Value.ToString();
                        string strDiettl = dataGridView_Lot.Rows[rowIndex].Cells[3].Value.ToString();
                        string strWfrttl = dataGridView_Lot.Rows[rowIndex].Cells[5].Value.ToString();
                        string strBillNo = dataGridView_Lot.Rows[rowIndex].Cells[9].Value.ToString();

                        Fnc_UpdateDeviceInfo(strDevice, strLot, strDcc, Int32.Parse(strDiettl), 0, Int32.Parse(strWfrttl), false, false);
                        BankHost_main.Host.Host_Delete_BcrReadinfo(BankHost_main.strEqid, strLot, 0);

                        string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
                        string strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strDevice + "\\" + strDevice;
                        string strlog = string.Format("RESET+{0}+{1}+{2}+{3}+{4}+{5}+{6}", strDevice, strLot, 0, strDiettl, strWfrttl, "RESET", BankHost_main.strOperator);

                        ////DB Save
                        string[] strSaveInfo = new string[10];
                        strSaveInfo[0] = BankHost_main.strEqid;
                        strSaveInfo[1] = "RESET";
                        strSaveInfo[2] = strBillNo;
                        strSaveInfo[3] = strDevice;
                        strSaveInfo[4] = strLot;
                        strSaveInfo[5] = "0";
                        strSaveInfo[6] = strDiettl;
                        strSaveInfo[7] = "0";
                        strSaveInfo[8] = strWfrttl;
                        strSaveInfo[9] = BankHost_main.strOperator;

                        //Fnc_SaveLog_Work(strFileName, strlog, strSaveInfo, 0);
                        Fnc_SaveLog_Work(strFileName_Device, strlog, strSaveInfo, 1);

                        string[] printinfo = { "", "" };
                        printinfo[0] = "1"; printinfo[1] = "";
                        BankHost_main.Host.Host_Set_Print_Data(BankHost_main.strEqid, printinfo);

                        nLabelcount = 0;
                        nLabelttl = 0;

                        --AmkorLabelCnt;
                        //if(strGrState == "Working")
                        //{ 
                        BankHost_main.strWork_Lotinfo = "";
                        //}

                        textBox_Readdata.Focus();
                    }
                }
            }
            /*
            if (e.ColumnIndex == dataGridView_Lot.Columns["재출력"].Index)
            {
                //Do something with your button.
                int rowIndex = e.RowIndex;
                int colIndex = e.ColumnIndex;

                if (colIndex != 0)
                    colIndex = 0;

                if (rowIndex == -1)
                    return;

                string strState = dataGridView_Lot.Rows[rowIndex].Cells[5].Value.ToString();

                if (strState != "Complete" || bPrintUse == false)
                    return;

                DialogResult dialogResult1 = MessageBox.Show("Amkor 바코드 출력을 시작 합니다. \n\n진행 하시겠습니까?", "Print", MessageBoxButtons.YesNo);
                if (dialogResult1 == DialogResult.Yes)
                {
                    string strCust = "379";
                    string strLotno = dataGridView_Lot.Rows[rowIndex].Cells[1].Value.ToString();
                    string strDeviceno = strSelDevice;
                    string strRcvD = dataGridView_Lot.Rows[rowIndex].Cells[9].Value.ToString();
                    string strDcc = "";
                    string strDieQty = dataGridView_Lot.Rows[rowIndex].Cells[2].Value.ToString();
                    string strWfrQty = dataGridView_Lot.Rows[rowIndex].Cells[4].Value.ToString();
                    string strBillno = dataGridView_Lot.Rows[rowIndex].Cells[8].Value.ToString();

                    Fnc_Print_Start(strLotno, strDeviceno, strDieQty, strWfrQty, "0000000000", strCust, strRcvD, strBillno, strDcc);                   
                }
            }
            */
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            //Fnc_UpdateDeviceInfo("BA005NA2593D", "6271208.8", 2000);
            speech.SpeakAsync("1 의 1");


        }

        private void button_End_Click(object sender, EventArgs e)
        {

        }

        public int Fnc_UpdateDeviceInfo(string strDevice, string strLot, string strDcc, int nDiettl, int nDieQty, int nWfrttl, bool bupdate, bool bunprint)
        {
            while (bGridViewUpdate)
            {
                Thread.Sleep(1);
            }

            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName;
            string strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + ".txt";
            string strSaveFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
            string strSaveFileName_Device = "";


            if (strSelCust == "940")
            {
                strValReadfile = strFileName + "\\" + strSelDevice + "\\" + strSelDevice + ".txt";
                strSaveFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strSelDevice + "\\" + strSelDevice;
            }
            else
            {
                strValReadfile = strFileName + "\\" + strDevice + "\\" + strDevice + ".txt";

                if (strDevice == "")
                {
                    strValReadfile = find_dev(strValReadfile);
                }

                strSaveFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strValDevice + "\\" + strValDevice;
            }

            if (System.IO.File.Exists(strValReadfile) == false)
                strValReadfile = find_dev(strValReadfile);

            string strlog = "";

            bool bReset = false;
            if (bupdate)
                bReset = false;
            else
                bReset = true;

            int dataIndex = 0;
            if (BankHost_main.strWork_QtyPos == "-1" ? false : nDiettl == nDieQty)
                dataIndex = Fnc_Getline_Revision(strValReadfile, strLot, nDiettl.ToString());
            else
                dataIndex = Fnc_Getline(strValReadfile, strLot, strDcc, nDieQty.ToString(), bReset);

            string strSpeak = "";

            if (dataIndex == 9999)
            {
                label_msg.Text = "Error";

                if (Properties.Settings.Default.CameraType != "COGNEX")
                {
                    strSpeak = string.Format("에러");
                    speech.SpeakAsync(strSpeak);
                }
                return -1;
            }

            int deviceindex = 0;

            if (strSelCust != "940")
            {
                deviceindex = Fnc_Getline(strFileName_Device, strDevice, strDcc, nDieQty.ToString(), bReset);
            }
            else
            {
                deviceindex = 0;
            }

            if (dataGridView_Device.Rows.Count == 0)
            {
                label_msg.Text = "리스트에 없는 자재 입니다.";

                strSpeak = string.Format("리스트에 없는 자재 입니다.");
                speech.SpeakAsync(strSpeak);

                return -1;
            }
            int nIndex = -1;

            try
            {
                nIndex = dataGridView_Device.CurrentCell.RowIndex;
            }
            catch (Exception ex)
            {
                string str = ex.ToString();

            }

            if (deviceindex != nIndex)
            {
                while (bGridViewUpdate)
                {
                    Thread.Sleep(1);
                }

                try
                {
                    Fnc_GetDeviceData(strDevice);
                }
                catch
                {
                    return -1;
                }
            }

            if (dataIndex == -1 || deviceindex == -1)
            {
                label_msg.Text = "리스트에 없는 자재 입니다.";

                strSpeak = string.Format("리스트에 없는 자재 입니다.");
                speech.SpeakAsync(strSpeak);

                return -1;
            }

            int Realindex = 0;

            if (BankHost_main.strWork_QtyPos == "-1" ? false : nDiettl == nDieQty)
                Realindex = Fnc_GetLotindex_Revision(strLot, nDiettl.ToString());
            else
                Realindex = Fnc_GetLotindex(strLot, strDcc, nDieQty.ToString(), bupdate);

            if (Realindex == -1)
            {
                label_msg.Text = "리스트에 없는 자재 입니다.";

                strSpeak = string.Format("리스트에 없는 자재 입니다.");
                speech.SpeakAsync(strSpeak);

                return -1;
            }

            string[] info = Fnc_ReadFile(strValReadfile);
            string[] strSplit_data = info[dataIndex].Split('\t'); //before : dataIndex

            StorageData st = new StorageData();

            st.Cust = strSplit_data[0];
            st.Device = strSplit_data[1];
            st.Lot = strSplit_data[2];
            st.Lot_Dcc = strSplit_data[3];
            st.Rcv_Qty = strSplit_data[4];
            st.Die_Qty = strSplit_data[5];
            st.Rcv_WQty = strSplit_data[6];
            st.Rcvddate = strSplit_data[7];
            st.Lot_type = strSplit_data[8];
            st.Bill = strSplit_data[9];
            st.Amkorid = strSplit_data[10];
            st.Wafer_lot = strSplit_data[11];
            st.strCoo = strSplit_data[12];
            st.state = strSplit_data[13];
            st.strop = strSplit_data[14];
            st.strGRstatus = strSplit_data[15];
            st.Default_WQty = strSplit_data[16];
            st.WSN = strWSN;


            if (strSplit_data.Length > 17)
            {
                st.shipment = strSplit_data[17];
                st.ReelID = strSplit_data[19];
                st.ReelDCC = strSplit_data[20];
            }
            else
                st.shipment = "";

            st.strop = BankHost_main.strOperator;

            strValDcc = st.Lot_Dcc;

            if (st.Die_Qty == "0")
                BankHost_main.strWork_Lotinfo = st.Lot;

            if (BankHost_main.strWork_Shot1Lot == "YES" && st.state == "Error")
            {
                st.Die_Qty = "0";
                st.Rcv_WQty = "0";
            }

            int nQty = Int32.Parse(st.Die_Qty) + nDieQty;
            int nttl = Int32.Parse(st.Rcv_Qty);
            int nWfrQry = Int32.Parse(st.Rcv_WQty) + BankHost_main.nWorkBcrcount;

            if (BankHost_main.nMaterial_type == 1)
            {
                strValWfrcount = st.Default_WQty.ToString();
                nWfrQry = Int32.Parse(strValWfrcount);
            }
            else
            {
                strValWfrcount = nWfrQry.ToString();
            }

            if (bupdate == false)
            {
                st.Die_Qty = "0";
                st.Rcv_WQty = "";
                st.state = "Waiting";

                nQty = 0;
                nWfrQry = 0;
                st.strop = "";
            }

            if (st.state == "Complete")
            {
                label_msg.Text = "완료 된 자재 입니다";


                return -1;
            }

            label_info_dieqty.ForeColor = Color.Blue;

            ////1Shot 1Lot 확인, Cust, inch , Name
            bool bWorkComplete = false;

            if (BankHost_main.strWork_QtyPos == "-1" ? false : nQty == 0)
            {
                label_info.Text = string.Format("{0} - {1}", deviceindex + 1, Realindex + 1);
                label_info.BackColor = Color.DarkGray;
                label_info.ForeColor = Color.White;
                st.state = "Waiting";
            }
            else if (BankHost_main.strWork_QtyPos == "-1" ? true : nQty == nttl)
            {
                label_info.Text = string.Format("{0} - {1} 완료", deviceindex + 1, Realindex + 1);
                label_info.BackColor = Color.Blue;
                label_info.ForeColor = Color.White;
                st.state = "Complete";

                //nLabelttl = int.Parse(dataGridView_Lot.Rows[Realindex].Cells[6].Value.ToString());

                strSpeak = string.Format("{0} 완료", Realindex + 1);

                strlog = string.Format("UPDATE+{0}+{1}+{2}+{3}+{4}+{5}+{6}", strDevice, strLot, nDieQty, nDiettl, nWfrttl, "COMPLETE", BankHost_main.strOperator);

                ////DB Save
                string[] strSaveInfo = new string[10];
                strSaveInfo[0] = BankHost_main.strEqid;
                strSaveInfo[1] = "VAL_COMPLETE";
                strSaveInfo[2] = st.Bill;
                strSaveInfo[3] = strDevice;
                strSaveInfo[4] = strLot;
                strSaveInfo[5] = nDieQty.ToString();
                strSaveInfo[6] = nDiettl.ToString();
                strSaveInfo[7] = strValWfrcount;
                strSaveInfo[8] = nWfrttl.ToString();
                strSaveInfo[9] = BankHost_main.strOperator;

                Fnc_SaveLog_Work(strSaveFileName_Device, strlog, strSaveInfo, 1);

                bWorkComplete = true;
            }
            else if (nQty < nttl)
            {
                if (BankHost_main.strWork_Shot1Lot == "YES")
                {
                    label_info.Text = string.Format("{0} - {1} 에러", deviceindex + 1, Realindex + 1);
                    label_info.BackColor = Color.Red;
                    label_info.ForeColor = Color.White;
                    st.state = "Error";
                    label_info_dieqty.ForeColor = Color.Red;
                    strSpeak = string.Format("{0} 에러", Realindex + 1);

                    strlog = string.Format("UPDATE+{0}+{1}+{2}+{3}+{4}+{5}+{6}", strDevice, strLot, nDieQty, nDiettl, nWfrttl, "ERROR", BankHost_main.strOperator);

                    ////DB Save
                    string[] strSaveInfo = new string[10];
                    strSaveInfo[0] = BankHost_main.strEqid;
                    strSaveInfo[1] = "ERROR";
                    strSaveInfo[2] = st.Bill;
                    strSaveInfo[3] = strDevice;
                    strSaveInfo[4] = strLot;
                    strSaveInfo[5] = nQty.ToString();
                    strSaveInfo[6] = nDiettl.ToString();
                    strSaveInfo[7] = strValWfrcount;
                    strSaveInfo[8] = nWfrttl.ToString();
                    strSaveInfo[9] = BankHost_main.strOperator;

                    Fnc_SaveLog_Work(strSaveFileName_Device, strlog, strSaveInfo, 1);
                }
                else
                {
                    label_info.Text = string.Format("{0} - {1} 진행 중", deviceindex + 1, Realindex + 1);
                    label_info.BackColor = Color.Green;
                    label_info.ForeColor = Color.White;
                    st.state = "Working";

                    strSpeak = string.Format("{0} !", Realindex + 1);

                    strlog = string.Format("UPDATE+{0}+{1}+{2}+{3}+{4}+{5}+{6}", strDevice, strLot, nDieQty, nDiettl, nWfrttl, "WORK", BankHost_main.strOperator);

                    ////DB Save
                    string[] strSaveInfo = new string[10];
                    strSaveInfo[0] = BankHost_main.strEqid;
                    strSaveInfo[1] = "VAL_OK";
                    strSaveInfo[2] = st.Bill;
                    strSaveInfo[3] = strDevice;
                    strSaveInfo[4] = strLot;
                    strSaveInfo[5] = nDieQty.ToString();
                    strSaveInfo[6] = nDiettl.ToString();
                    strSaveInfo[7] = strValWfrcount;
                    strSaveInfo[8] = nWfrttl.ToString();
                    strSaveInfo[9] = BankHost_main.strOperator;

                    Fnc_SaveLog_Work(strSaveFileName_Device, strlog, strSaveInfo, 1);
                }
            }
            else if (nQty == nttl)
            {
                label_info.Text = string.Format("{0} - {1} 완료", deviceindex + 1, Realindex + 1);
                label_info.BackColor = Color.Blue;
                label_info.ForeColor = Color.White;
                st.state = "Complete";


                //nLabelttl = int.Parse(dataGridView_Lot.Rows[Realindex].Cells[6].Value.ToString());

                strSpeak = string.Format("{0} 완료", Realindex + 1);

                strlog = string.Format("UPDATE+{0}+{1}+{2}+{3}+{4}+{5}+{6}", strDevice, strLot, nDieQty, nDiettl, nWfrttl, "COMPLETE", BankHost_main.strOperator);

                ////DB Save
                string[] strSaveInfo = new string[10];
                strSaveInfo[0] = BankHost_main.strEqid;
                strSaveInfo[1] = "VAL_COMPLETE";
                strSaveInfo[2] = st.Bill;
                strSaveInfo[3] = strDevice;
                strSaveInfo[4] = strLot;
                strSaveInfo[5] = nDieQty.ToString();
                strSaveInfo[6] = nDiettl.ToString();
                strSaveInfo[7] = strValWfrcount;
                strSaveInfo[8] = nWfrttl.ToString();
                strSaveInfo[9] = BankHost_main.strOperator;

                Fnc_SaveLog_Work(strSaveFileName_Device, strlog, strSaveInfo, 1);

                bWorkComplete = true;
            }
            else
            {
                label_info.Text = string.Format("{0} - {1} 에러", deviceindex + 1, Realindex + 1);
                label_info.BackColor = Color.Red;
                label_info.ForeColor = Color.White;
                st.state = "Error";
                label_info_dieqty.ForeColor = Color.Red;

                strSpeak = string.Format("{0} 에러", Realindex + 1);

                strlog = string.Format("UPDATE+{0}+{1}+{2}+{3}+{4}+{5}+{6}", strDevice, strLot, nDieQty, nDiettl, nWfrttl, "ERROR", BankHost_main.strOperator);

                ////DB Save
                string[] strSaveInfo = new string[10];
                strSaveInfo[0] = BankHost_main.strEqid;
                strSaveInfo[1] = "ERROR";
                strSaveInfo[2] = st.Bill;
                strSaveInfo[3] = strDevice;
                strSaveInfo[4] = strLot;
                strSaveInfo[5] = nDieQty.ToString();
                strSaveInfo[6] = nDiettl.ToString();
                strSaveInfo[7] = strValWfrcount;
                strSaveInfo[8] = nWfrttl.ToString();
                strSaveInfo[9] = BankHost_main.strOperator;

                Fnc_SaveLog_Work(strSaveFileName_Device, strlog, strSaveInfo, 1);
            }

            if (BankHost_main.nInputMode == 1)
            {
                speech.SpeakAsync(strSpeak);
            }

            st.Die_Qty = nQty.ToString();
            st.Rcv_WQty = nWfrQry.ToString();

            if (st.state == "Working" || st.state == "Complete")
            {
                string[] t = ReelIDUpdate(st);

                st.ReelID = t[0];
                st.ReelDCC = t[1];
            }

            string strTxtline = st.Cust + "\t" + st.Device + "\t" + st.Lot + "\t" + st.Lot_Dcc + "\t" + st.Rcv_Qty + "\t" + st.Die_Qty + "\t" +
                    st.Rcv_WQty + "\t" + st.Rcvddate + "\t" + st.Lot_type + "\t" + st.Bill + "\t" + st.Amkorid + "\t" + st.Wafer_lot + "\t" +
                    st.strCoo + "\t" + st.state + "\t" + st.strop + "\t" + st.strGRstatus + "\t" + st.Default_WQty + "\t" + st.shipment + "\t" + st.WSN + "\t" + st.ReelID + "\t" + st.ReelDCC;

            info[dataIndex] = strTxtline;
            File.WriteAllLines(strValReadfile, info);

            label_info_device.Text = strDevice;
            label_info_lot.Text = strLot;
            label_info_diettl.Text = string.Format("{0:0,0}", Int32.Parse(st.Rcv_Qty));
            label_info_dieqty.Text = string.Format("{0:0,0}", Int32.Parse(st.Die_Qty));
            label_info_wfrqty.Text = string.Format("{0:0,0}", Int32.Parse(st.Rcv_WQty));

            while (bGridViewUpdate)
            {
                Thread.Sleep(1);
                System.Windows.Forms.Application.DoEvents();
            }

            try
            {
                Fnc_GetDeviceData(strDevice);

            }
            catch
            {
                return -1;
            }

            dataGridView_Device.Rows[deviceindex].Cells[1].Selected = true;
            dataGridView_Device.FirstDisplayedScrollingRowIndex = deviceindex;

            /////////////////////////////////////////////////////////////////////////////////////////////   
            //Application.DoEvents();
            ///////////////////////////////////////////////////////////////////////////////////
            //dataGridView_Lot.Rows[Realindex].Cells[1].Selected = true;
            dataGridView_Lot.Rows[Realindex].Selected = true;
            dataGridView_Lot.CurrentCell = dataGridView_Lot.Rows[Realindex].Cells[0];
            dataGridView_Lot.Rows[Realindex].Cells["ReelID"].Value = st.ReelID;
            dataGridView_Lot.Rows[Realindex].Cells["ReelDCC"].Value = st.ReelDCC;

            int nRows = dataGridView_Lot.Rows.Count;
            if (nRows == Realindex + 1)
            {
                dataGridView_Lot.FirstDisplayedCell = dataGridView_Lot.Rows[Realindex].Cells[1];
            }
            else
                dataGridView_Lot.FirstDisplayedScrollingRowIndex = Realindex;

            if (st.state == "Complete")
            {
                label_msg.Text = "완료";

                BankHost_main.strWork_Lotinfo = "";
                BankHost_main.Host.Host_Delete_BcrReadinfo(BankHost_main.strEqid, strLot, 0);

                if (comboBox_mode.SelectedIndex == 2 || comboBox_mode.SelectedIndex == 3)
                {
                    string url = $"http://{(Properties.Settings.Default.TestMode == true ? TEST_MES : PRD_MES)}/eMES_Webservice/diebank_automation_service/rec_reel_inf/{st.Amkorid},{st.ReelID},{(st.ReelDCC == "" ? " " : st.ReelDCC)},{st.Die_Qty},{BankHost_main.strID}";
                    string res = InsertReelID(url).Result;

                    if (res.ToUpper() != "OK")
                    {
                        InsertWebdata(url);
                    }
                }


                Thread insertThread = new Thread(InsertWAS);
                tempData = st;
                insertThread.Start();

                // GR 처리 필요
                //
                //


                return 2;
            }
            else if (st.state == "Error")
            {
                label_msg.Text = "ERROR";

                BankHost_main.strWork_Lotinfo = "";
                return -2;
            }
            else if (st.state == "Working")
            {
                label_msg.Text = "READ OK";

                int nMaxPack = BankHost_main.nMaxpack;
                int nCurWaferqty = Int32.Parse(st.Rcv_WQty);

                int nHeadttl = nWfrttl / nMaxPack;
                int nHead = nCurWaferqty / nMaxPack;
                int nRemain = nCurWaferqty % nMaxPack;
                int nRemainttl = nWfrttl % nMaxPack;

                //////HY 20200914
                if (nLabelcount == 0)
                {
                    if (nRemainttl < 3 && nHeadttl > 0)
                    {
                        nLabelttl = nHeadttl;
                    }
                    else
                    {
                        if (nMaxPack == nWfrttl)
                            nLabelttl = nHeadttl;
                        else
                            nLabelttl = nHeadttl + 1;
                    }
                }
                ///////
                if (bWorkComplete)
                    return 2;

                if (nRemain == 0 && nHead < nHeadttl)
                {
                    if (nWfrttl < 6 && nDieQty != nDiettl)
                        return 0;

                    if (bunprint)
                        return 0;

                    return 1;
                }
                else
                {
                    if (nCurWaferqty < nMaxPack)
                    {
                        return 0;
                    }

                    if (nHead == nHeadttl && nRemain < nRemainttl)
                    {
                        int nInclude = nRemainttl - nRemain;
                        if (nInclude < 3)
                            return 0;
                        else
                        {
                            if (nHead == nHeadttl && nCurWaferqty % nMaxPack != 0)
                                return 0;
                            else
                            {
                                if (nWfrttl < 6 && nDieQty != nDiettl)
                                    return 0;

                                if (bunprint)
                                    return 0;

                                return 1;
                            }
                        }
                    }

                    return 0;
                }
            }

            return 0;
        }

        public Dictionary<string, string> Bcr2Dic(StorageData info)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            res.Add("DEVICE", info.Device);

            res.Add("LOT", info.Lot);
            res.Add("QTY", info.Die_Qty);
            //res.Add("WFRQTY", info.WfrQty);
            res.Add("STATE", info.state);
            res.Add("WSN", info.WSN);
            res.Add("LPN", info.LPN);

            return res;
        }

        private bool dupCheckReelID(string ReelID)
        {
            return GetWebServiceData($"http://{(Properties.Settings.Default.TestMode == true ? TEST_MES : PRD_MES)}/eMES_Webservice/diebank_automation_service/chk_dup_reel_inf/{ReelID}, ").ToUpper() == "TRUE" ? true : false;
        }

        private String checkDCC(string ReelID)
        {
            string DCC = GetWebServiceData($"http://{(Properties.Settings.Default.TestMode == true ? TEST_MES : PRD_MES)}/eMES_Webservice/diebank_automation_service/inq_last_reel_dcc/{ReelID}");

            if (DCC != "")
                DCC = DCC == "     " ? "01" : $"{int.Parse(DCC) + 1}".PadLeft(2, '0');

            return DCC;
        }

        public string[] ReelIDUpdate(StorageData info)
        {
            string ReelID = "";
            string DCC = "";
            try
            {
                Dictionary<string, string> selcust = GetReelIDRule();
                Dictionary<string, string> bcrDic = Bcr2Dic(info);


                int res = -1;

                if (selcust["REEL_ID"] == "ALL")
                {
                    ReelID = BankHost_main.strScanData.Replace("\r", "");
                }
                else if (int.TryParse(selcust["REEL_ID"].Split('/')[0], out res) == true)
                {
                    ReelID = BankHost_main.nScanMode == 1 ? BankHost_main.strScanData.Split(new string[] { selcust["SPLITER"] }, StringSplitOptions.None)[res - 1] : BankHost_main.ReaderData.Remove(BankHost_main.ReaderData.Length - 1, 1).Split(new string[] { selcust["SPLITER"] }, StringSplitOptions.None)[res - 1];

                    string[] rule = selcust["REEL_ID"].Split('/');

                    if (rule.Length == 2)
                    {
                        if (rule[1].Contains("L") == true)
                        {
                            ReelID = ReelID.Remove(0, int.Parse(rule[1].Remove(0, 1)));
                        }
                        else if (rule[1].Contains("R") == true)
                        {
                            ReelID = ReelID.Remove(ReelID.Length - int.Parse(rule[1].Remove(0, 1)), int.Parse(rule[1].Remove(0, 1)));
                        }
                    }
                }
                else
                {
                    string[] r = selcust["REEL_ID"].Split(',');

                    foreach (string t in r)
                    {
                        if (t == "LPN")
                        {
                            ReelID += $"{strSelLPN}{selcust["SPLITER"]}";
                        }
                        else
                        {
                            ReelID += $"{bcrDic[t]}{selcust["SPLITER"]}";
                        }
                    }

                    ReelID = ReelID.Remove(ReelID.Length - 1, 1);
                }


                if (GetWebServiceData($"http://{(Properties.Settings.Default.TestMode == true ? TEST_MES : PRD_MES)}/eMES_Webservice/diebank_automation_service/chk_dup_reel_inf/{ReelID}, ").ToUpper() == "TRUE")
                {
                    DCC = GetWebServiceData($"http://{(Properties.Settings.Default.TestMode == true ? TEST_MES : PRD_MES)}/eMES_Webservice/diebank_automation_service/inq_last_reel_dcc/{ReelID}");

                    if (DCC != "")
                        DCC = DCC == "     " ? "01" : $"{int.Parse(DCC) + 1}".PadLeft(2, '0');
                }


                return new string[2] { ReelID, DCC };
            }
            catch (Exception ex)
            {


                return new string[2] { ReelID, DCC };
            }


        }


        public string Fnc_Update_GR(string strDevice, string strLot, string state)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
            string strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + ".txt";
            strValReadfile = strFileName + "\\" + strDevice + "\\" + strDevice + ".txt";

            string strSaveFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
            string strSaveFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strDevice + "\\" + strDevice;
            string strlog = "";

            int dataIndex = Fnc_Getline_GR(strValReadfile, strLot, "", "", false);
            int deviceindex = Fnc_Getline_GR(strFileName_Device, strDevice, "", "", false);

            string strSpeak = "";

            if (dataIndex == -1 || deviceindex == -1)
            {
                label_msg.Text = "리스트에 없는 자재 입니다.";

                strSpeak = string.Format("리스트에 없는 자재 입니다.");
                speech.SpeakAsync(strSpeak);

                return "";
            }

            string[] info = Fnc_ReadFile(strValReadfile);
            string[] strSplit_data = info[dataIndex].Split('\t');

            StorageData st = new StorageData();

            st.Cust = strSplit_data[0];
            st.Device = strSplit_data[1];
            st.Lot = strSplit_data[2];
            st.Lot_Dcc = strSplit_data[3];
            st.Rcv_Qty = strSplit_data[4];
            st.Die_Qty = strSplit_data[5];
            st.Rcv_WQty = strSplit_data[6];
            st.Rcvddate = strSplit_data[7];
            st.Lot_type = strSplit_data[8];
            st.Bill = strSplit_data[9];
            st.Amkorid = strSplit_data[10];
            st.Wafer_lot = strSplit_data[11];
            st.strCoo = strSplit_data[12];
            st.state = strSplit_data[13];
            st.strop = strSplit_data[14];
            st.strGRstatus = state; //상태 업데이트
            st.Default_WQty = strSplit_data[16];

            if (strSplit_data.Length > 17)
                st.shipment = strSplit_data[17];
            else
                st.shipment = "";

            st.strop = BankHost_main.strOperator;

            strlog = string.Format("GR+{0}+{1}+{2}+{3}+{4}+{5}+{6}", strDevice, strLot, st.Die_Qty, st.Rcv_Qty, st.Default_WQty, state, BankHost_main.strOperator);

            ////DB Save
            string[] strSaveInfo = new string[10];
            strSaveInfo[0] = BankHost_main.strEqid;
            strSaveInfo[1] = "";
            strSaveInfo[2] = st.Bill;
            strSaveInfo[3] = strDevice;
            strSaveInfo[4] = strLot;
            strSaveInfo[5] = st.Die_Qty;
            strSaveInfo[6] = st.Rcv_Qty;
            strSaveInfo[7] = st.Rcv_WQty;
            strSaveInfo[8] = st.Default_WQty;
            strSaveInfo[9] = BankHost_main.strOperator;

            if (state == "ERROR")
            {
                strSaveInfo[1] = "GR_ERROR";
            }
            else if (state == "COMPLETE")
            {
                strSaveInfo[1] = "GR_COMPLETE";
            }
            else if (state == "PROCESSING")
            {
                strSaveInfo[1] = "PROCESSING";
            }
            else
            {
                strSaveInfo[1] = "GR_START";
            }

            Fnc_SaveLog_Work(strSaveFileName_Device, strlog, strSaveInfo, 1);

            string strTxtline = st.Cust + "\t" + st.Device + "\t" + st.Lot + "\t" + st.Lot_Dcc + "\t" + st.Rcv_Qty + "\t" + st.Die_Qty + "\t" +
                    st.Rcv_WQty + "\t" + st.Rcvddate + "\t" + st.Lot_type + "\t" + st.Bill + "\t" + st.Amkorid + "\t" + st.Wafer_lot + "\t" +
                    st.strCoo + "\t" + st.state + "\t" + st.strop + "\t" + st.strGRstatus + "\t" + st.Default_WQty + "\t" + st.shipment + "\t" + st.WSN + "\t" + st.ReelID + "\t" + st.ReelDCC;

            info[dataIndex] = strTxtline;
            File.WriteAllLines(strValReadfile, info);

            string strgr = string.Format("{0};{1};{2}", st.Amkorid, st.Die_Qty, st.Rcv_WQty);
            return strgr;
        }

        public int Fnc_ChangeLotName(string strDevice, string strLot_org, string strLot_new)
        {
            /*
            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
            string strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + ".txt";
            string strReadfile = strFileName + "\\" + strDevice + "\\" + strDevice + ".txt";

            int dataIndex = Fnc_Getline(strReadfile, strLot_org);
            int deviceindex = Fnc_Getline(strFileName_Device, strDevice);

            string strSpeak = "";

            if (dataIndex == -1 || deviceindex == -1)
            {
                strSpeak = string.Format("파일 리스트에 없는 자재 입니다.");
                speech.SpeakAsync(strSpeak);

                return -1;
            }

            Application.DoEvents();

            string[] info = Fnc_ReadFile(strReadfile);
            string[] strSplit_data = info[dataIndex].Split('\t');

            StorageData st = new StorageData();

            st.Bill = strSplit_data[0];
            st.Invoice = strSplit_data[1];
            st.lot = strSplit_data[2];
            st.Device = strSplit_data[3];
            st.Diettl = strSplit_data[4];
            st.Dieqty = strSplit_data[5];
            st.Wfrqty = strSplit_data[6];
            st.Price = strSplit_data[7];
            st.Wfrsize = strSplit_data[8];
            st.Rcvddate = strSplit_data[9];
            st.state = strSplit_data[10];

            st.strop = BankHost_main.strOperator;

            int nQty = Int32.Parse(st.Dieqty);
            int nttl = Int32.Parse(st.Diettl);
            int nWfrQry = Int32.Parse(st.Wfrqty);

            st.lot = strLot_new;
            st.Dieqty = nQty.ToString();
            st.Wfrqty = nWfrQry.ToString();

            string strTxtline = st.Bill + "\t" + st.Invoice + "\t" + st.lot + "\t" + st.Device + "\t" + st.Diettl + "\t" + st.Dieqty + "\t" +
                    st.Wfrqty + "\t" + st.Price + "\t" + st.Wfrsize + "\t" + st.Rcvddate + "\t" + st.state + "\t" + st.strop;

            info[dataIndex] = strTxtline;
            File.WriteAllLines(strReadfile, info);

            strSpeak = "";

            strSpeak = string.Format("랏트 이름이 변경 되었습니다.");

            speech.SpeakAsync(strSpeak);

            Application.DoEvents();

            Fnc_GetDeviceData(strDevice);

            int Realindex = Fnc_GetLotindex(strLot_new);

            ///////////////////////////////////////////////////////////////////////////////////            
            dataGridView_Lot.Rows[Realindex].Cells[1].Selected = true;
            dataGridView_Lot.Rows[Realindex].DefaultCellStyle.ForeColor = Color.Red;

            int nRows = dataGridView_Lot.Rows.Count;
            if (nRows == Realindex + 1)
            {
                //int firstDisplayed = dataGridView_Lot.FirstDisplayedScrollingRowIndex;

                //dataGridView_Lot.FirstDisplayedScrollingRowIndex = nRows - 1;

                dataGridView_Lot.FirstDisplayedCell = dataGridView_Lot.Rows[Realindex].Cells[1];
            }
            else
                dataGridView_Lot.FirstDisplayedScrollingRowIndex = Realindex;
            /////////////////////////////////////////////////////////////////////////////////////////////   
            */
            return 0;
        }

        public int Fnc_Getline(string strfilepath, string strData, string strDcc, string strDie, bool bReset)
        {
            if (System.IO.File.Exists(strfilepath) == false)
            {
                strfilepath = find_dev(strfilepath);
            }

            string[] info = Fnc_ReadFile(strfilepath);

            if (info == null)
                return -1;

            StorageData st = new StorageData();

            int nCount = 0;

            for (int m = 0; m < info.Length; m++)
            {
                string[] strSplit_data = info[m].Split('\t');

                if (strSplit_data.Length == 1)
                {
                    st.Device = strSplit_data[0];

                    if (strData == st.Device)
                    {
                        return m;
                    }
                }
                else
                {
                    st.Cust = strSplit_data[0];
                    st.Device = strSplit_data[1];
                    st.Lot = strSplit_data[2];
                    st.Lot_Dcc = strSplit_data[3];
                    st.Rcv_Qty = strSplit_data[4];
                    st.state = strSplit_data[13];
                    st.state = st.state.ToLower();


                    if (strData == st.Lot)
                    {
                        if (BankHost_main.strWork_Shot1Lot == "YES" && BankHost_main.strWork_DevicePos == "-1" && !bReset)
                        {
                            if (st.state == "waiting" && (BankHost_main.strWork_QtyPos == "-1" ? true : st.Rcv_Qty == strDie))
                                return m;
                        }
                        else
                        {
                            if (bReset)
                            {
                                if (st.Lot_Dcc == strDcc)
                                    return m;
                            }
                            else
                            {
                                nCount++;

                                if (st.state != "complete" && st.state != "error")
                                    return m;
                            }
                        }
                    }
                }
            }

            if (nCount > 0)
                return 9999;

            return -1;
        }
        public int Fnc_Getline_GR(string strfilepath, string strData, string strDcc, string strDie, bool bReset)
        {
            string[] info = Fnc_ReadFile(strfilepath);

            if (info == null)
                return -1;

            StorageData st = new StorageData();

            for (int m = 0; m < info.Length; m++)
            {
                string[] strSplit_data = info[m].Split('\t');

                if (strSplit_data.Length == 1)
                {
                    st.Device = strSplit_data[0];

                    if (strData == st.Device)
                    {
                        return m;
                    }
                }
                else
                {
                    st.Cust = strSplit_data[0];
                    st.Device = strSplit_data[1];
                    st.Lot = strSplit_data[2];
                    st.Lot_Dcc = strSplit_data[3];
                    st.Rcv_Qty = strSplit_data[4];
                    st.state = strSplit_data[13];
                    st.state = st.state.ToLower();

                    if (strData == st.Lot)
                    {
                        if (BankHost_main.strWork_Shot1Lot == "YES" && BankHost_main.strWork_DevicePos == "-1" && !bReset)
                        {
                            if (st.state == "waiting" && st.Rcv_Qty == strDie)
                                return m;
                        }
                        else
                        {
                            if (bReset)
                            {
                                if (st.Lot_Dcc == strDcc)
                                    return m;
                            }
                            else
                            {
                                return m;
                            }
                        }
                    }
                }
            }

            return -1;
        }


        /// <summary>
        /// Device ID 없이 조회 할 때 금일 작업한 커스터머 기준으로 모든 Lot를 검사하여 Lot Diectory return
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string find_dev(string path)
        {
            string res = "";

            try
            {
                if (System.IO.File.Exists(path) == false)
                {
                    if (path.Contains("\\\\") == true)
                    {
                        string[] file_path = path.Replace(@"\\", @"\").Split('\\');

                        for (int i = 0; i < file_path.Length - 1; i++)
                        {
                            if (i == file_path.Length - 2)
                            {
                                res += file_path[i];
                            }
                            else
                            {
                                res += file_path[i] + "\\";
                            }
                        }

                        string dev = "";

                        if (System.IO.Directory.Exists(res) == true)
                        {
                            DirectoryInfo di = new DirectoryInfo(res);

                            string[] dirs = Directory.GetDirectories(res + "\\");

                            for (int i = 0; i < dirs.Length; i++)
                            {
                                string[] files = Directory.GetFiles(dirs[i] + "\\");

                                for (int j = 0; j < files.Length; j++)
                                {
                                    dev = find_lot(files[j]);

                                    if (dev != "")
                                    {
                                        res = files[j];
                                        strValDevice = dev;
                                        break;
                                    }
                                }

                                if (dev != "")
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return res;
        }

        string find_lot(string file_path)
        {
            string res = "";

            try
            {
                if (!System.IO.File.Exists(file_path))
                {
                    return "";
                }
                else
                {
                    string[] lines = System.IO.File.ReadAllLines(file_path);
                    string[] datas;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        datas = lines[i].Split('\t');

                        if (BankHost_main.strLot2Wfr == "TRUE")
                        {
                            if (datas[11] == strValLot)
                            {
                                res = datas[1];
                                real_index = i;
                                strValLot = datas[2];
                                break;
                            }
                        }
                        else
                        {
                            if (datas.Length > 2 ? datas[2] == strValLot : false)
                            {
                                res = datas[1];
                                real_index = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return res;
        }

        public int Fnc_Getline_Revision(string strfilepath, string strData, string strCprQty)
        {
            if (System.IO.File.Exists(strfilepath) == false)
                strfilepath = find_dev(strfilepath);

            string[] info = Fnc_ReadFile(strfilepath);


            if (info == null)
                return -1;

            StorageData st = new StorageData();

            for (int m = 0; m < info.Length; m++)
            {
                string[] strSplit_data = info[m].Split('\t');

                if (strSplit_data.Length == 1)
                {
                    st.Device = strSplit_data[0];

                    if (strData == st.Device)
                    {
                        return m;
                    }
                }
                else
                {
                    st.Cust = strSplit_data[0];
                    st.Device = strSplit_data[1];
                    st.Lot = strSplit_data[2];
                    st.Lot_Dcc = strSplit_data[3];
                    st.Rcv_Qty = strSplit_data[4];
                    st.state = strSplit_data[13];

                    if (BankHost_main.strLot2Wfr == "TRUE")
                    {
                        st.Lot = strSplit_data[11];

                        if (strData == st.Lot)
                        {
                            if (BankHost_main.strWork_Shot1Lot == "YES" && BankHost_main.strWork_DevicePos == "-1" && bupdate)
                            {
                                if (st.state == "Waiting")
                                    return m;
                            }
                            else
                                return m;
                        }
                    }
                    else if (BankHost_main.strWork_QtyPos == "-1" && BankHost_main.strWork_DevicePos == "-1" && bupdate)
                    {
                        if (strData == st.Lot)
                        {
                            if (BankHost_main.strWork_Shot1Lot == "YES" && BankHost_main.strWork_DevicePos == "-1" && bupdate)
                            {
                                if (st.state == "Waiting")
                                    return m;
                            }
                            else
                                return m;
                        }
                    }
                    else if (strData == st.Lot && strCprQty == st.Rcv_Qty)
                    {
                        if (BankHost_main.strWork_Shot1Lot == "YES" && BankHost_main.strWork_DevicePos == "-1" && bupdate)
                        {
                            if (st.state == "Waiting")
                                return m;
                        }
                        else
                            return m;
                    }
                }

            }

            return -1;
        }

        public int Fnc_GetLotindex(string strData, string strDcc, string strDieqty, bool bupdate)
        {
            int nCount = dataGridView_Lot.Rows.Count;

            if (nCount < 0)
                return -1;

            string strLotno = "", strGetDiettl = "", strGetDcc = "", strGetState = "";

            int nLotcont = 0;
            for (int n = 0; n < nCount; n++)
            {
                strLotno = dataGridView_Lot.Rows[n].Cells[1].Value.ToString();
                strGetDcc = dataGridView_Lot.Rows[n].Cells[2].Value.ToString();
                strGetDiettl = dataGridView_Lot.Rows[n].Cells[3].Value.ToString();
                strGetState = dataGridView_Lot.Rows[n].Cells[7].Value.ToString();
                strGetState = strGetState.ToLower();

                if (strData == strLotno)
                {
                    if (BankHost_main.strWork_Shot1Lot == "YES" && BankHost_main.strWork_DevicePos == "-1" && bupdate)
                    {
                        if ((BankHost_main.strWork_DevicePos == "-1" ? true : strGetDiettl == strDieqty) && strGetState != "complete")
                            return n;
                    }
                    else
                    {
                        nLotcont++;

                        if (!bupdate)
                        {
                            if (strGetDcc == strDcc)
                                return n;
                        }
                        else
                        {
                            if (strGetState != "complete" && strGetState != "error")
                                return n;
                        }
                    }
                }
            }

            return -1;
        }

        public int Fnc_GetLotindex2(string strfilepath, string strData, string strDieqty, bool bupdate)
        {
            string[] info = Fnc_ReadFile(strfilepath);

            if (info == null)
                return -1;

            StorageData st = new StorageData();

            for (int m = 0; m < info.Length; m++)
            {
                string[] strSplit_data = info[m].Split('\t');

                if (strSplit_data.Length == 1)
                {
                    st.Device = strSplit_data[0];

                    if (strData == st.Device)
                    {
                        return m;
                    }
                }
                else
                {
                    st.Cust = strSplit_data[0];
                    st.Device = strSplit_data[1];
                    st.Lot = strSplit_data[2];
                    st.Lot_Dcc = strSplit_data[3];
                    st.Rcv_Qty = strSplit_data[4];
                    st.state = strSplit_data[13];

                    if (strData == st.Lot)
                    {
                        if (BankHost_main.strWork_Shot1Lot == "YES" && BankHost_main.strWork_DevicePos == "-1" && bupdate)
                        {
                            if (strDieqty == st.Rcv_Qty && st.state == "Waiting")
                                return m;
                        }
                        else
                            return m;
                    }
                }

            }

            return -1;
        }

        public int Fnc_GetTTL(string strDevice, string strLot, int nType)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
            string strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + ".txt";
            strValReadfile = strFileName + strDevice + "\\" + strDevice + ".txt";

            string[] info = Fnc_ReadFile(strValReadfile);

            if (info == null)
                return -1;

            StorageData st = new StorageData();

            for (int m = 0; m < info.Length; m++)
            {
                string[] strSplit_data = info[m].Split('\t');

                st.Cust = strSplit_data[0];
                st.Device = strSplit_data[1];
                st.Lot = strSplit_data[2];
                st.Lot_Dcc = strSplit_data[3];
                st.Rcv_Qty = strSplit_data[4];
                st.Default_WQty = strSplit_data[16];

                if (strLot == st.Lot)
                {
                    if (nType == 0)
                        return Int32.Parse(st.Rcv_Qty);
                    else
                        return Int32.Parse(st.Default_WQty);
                }
            }

            return 0;
        }

        public int Fnc_GetLot_TTL(string strData, int nType)  // 0: Die , 1: Wfr
        {
            int nCount = dataGridView_sort.Rows.Count;

            string strLotno = "";

            for (int n = 0; n < nCount; n++)
            {
                strLotno = dataGridView_sort.Rows[n].Cells[3].Value.ToString();
                if (strData == strLotno)
                {
                    string strLotTTL = "";
                    if (nType == 0)
                    {
                        strLotTTL = dataGridView_sort.Rows[n].Cells[5].Value.ToString(); //Die                        
                    }
                    else
                    {
                        strLotTTL = dataGridView_sort.Rows[n].Cells[6].Value.ToString(); //Wafer      
                    }

                    return Int32.Parse(strLotTTL);
                }
            }

            return -1;
        }
        public string Fnc_Get_Device(string strfilepath, string strLot)
        {
            string[] info = Fnc_ReadFile(strfilepath);

            if (info == null)
                return "";

            StorageData st = new StorageData();

            for (int m = 0; m < info.Length; m++)
            {
                string[] strSplit_data = info[m].Split('\t');

                st.Cust = strSplit_data[0];
                st.Device = strSplit_data[1];
                st.Lot = strSplit_data[2];
                st.Lot_Dcc = strSplit_data[3];
                st.Rcv_Qty = strSplit_data[4];
                st.state = strSplit_data[13];

                if (strLot == st.Lot)
                {
                    return st.Device;
                }

            }

            return "";
        }
        public AmkorBcrInfo Fnc_GetAmkorBcrInfo(string strfilepath, string strLot, string strDcc, string strDevice)
        {
            string[] info = Fnc_ReadFile(strfilepath);
            ClickTime();

            if (info == null)
                return null;

            StorageData st = new StorageData();

            for (int m = 0; m < info.Length; m++)
            {
                string[] strSplit_data = info[m].Split('\t');

                st.Cust = strSplit_data[0];
                st.Device = strSplit_data[1];
                st.Lot = strSplit_data[2];
                st.Lot_Dcc = strSplit_data[3];
                st.Rcv_Qty = strSplit_data[4];
                st.Die_Qty = strSplit_data[5];
                st.Rcv_WQty = strSplit_data[6];
                st.Rcvddate = strSplit_data[7];
                st.Lot_type = strSplit_data[8];
                st.Bill = strSplit_data[9];
                st.Amkorid = strSplit_data[10];
                st.Wafer_lot = strSplit_data[11];
                st.strCoo = strSplit_data[12];
                st.state = strSplit_data[13];
                st.strop = strSplit_data[14];
                st.strGRstatus = strSplit_data[15];
                st.Default_WQty = strSplit_data[16];

                if (strSplit_data.Length >= 19)
                {
                    st.WSN = strSplit_data[18];
                    st.ReelID = strSplit_data[19];
                    st.ReelDCC = strSplit_data[20];
                }

                if (strDevice == st.Device && strLot == st.Lot && st.Lot_Dcc == strDcc)
                {
                    AmkorBcrInfo Amkor = new AmkorBcrInfo();

                    Amkor.strLotNo = st.Lot;
                    Amkor.strDevice = st.Device;
                    Amkor.strDieQty = st.Die_Qty;
                    Amkor.strDiettl = st.Rcv_Qty;
                    Amkor.strWfrQty = st.Rcv_WQty;
                    Amkor.strWfrttl = st.Default_WQty;
                    Amkor.strAmkorid = st.Amkorid;
                    Amkor.strCust = st.Cust;
                    Amkor.strRcvdate = st.Rcvddate;
                    Amkor.strBillNo = st.Bill;
                    Amkor.strLotDcc = st.Lot_Dcc;
                    Amkor.strLotType = st.Lot_type;
                    Amkor.strWaferLotNo = st.Wafer_lot;
                    Amkor.strCoo = st.strCoo;
                    Amkor.strOperator = st.strop;
                    Amkor.strWSN = st.WSN;
                    Amkor.strRID = st.ReelID;
                    Amkor.strReelDCC = st.ReelDCC;

                    return Amkor;
                }

            }

            return null;
        }

        public AmkorBcrInfo Fnc_GetAmkorBcrInfo(string strfilepath, string strLot, string strDcc, string strDevice, string strWSN)
        {
            string[] info = Fnc_ReadFile(strfilepath);

            if (info == null)
                return null;

            StorageData st = new StorageData();

            for (int m = 0; m < info.Length; m++)
            {
                string[] strSplit_data = info[m].Split('\t');

                st.Cust = strSplit_data[0];
                st.Device = strSplit_data[1];
                st.Lot = strSplit_data[2];
                st.Lot_Dcc = strSplit_data[3];
                st.Rcv_Qty = strSplit_data[4];
                st.Die_Qty = strSplit_data[5];
                st.Rcv_WQty = strSplit_data[6];
                st.Rcvddate = strSplit_data[7];
                st.Lot_type = strSplit_data[8];
                st.Bill = strSplit_data[9];
                st.Amkorid = strSplit_data[10];
                st.Wafer_lot = strSplit_data[11];
                st.strCoo = strSplit_data[12];
                st.state = strSplit_data[13];
                st.strop = strSplit_data[14];
                st.strGRstatus = strSplit_data[15];
                st.Default_WQty = strSplit_data[16];



                if (strSplit_data.Length >= 19)
                {
                    st.WSN = strSplit_data[18];
                    st.ReelID = strSplit_data[19];
                    st.ReelDCC = strSplit_data[20];
                }

                if (strDevice == st.Device && strLot == st.Lot && st.Lot_Dcc == strDcc && st.WSN == strWSN)
                {
                    AmkorBcrInfo Amkor = new AmkorBcrInfo();

                    Amkor.strLotNo = st.Lot;
                    Amkor.strDevice = st.Device;
                    Amkor.strDieQty = st.Die_Qty;
                    Amkor.strDiettl = st.Rcv_Qty;
                    Amkor.strWfrQty = st.Rcv_WQty;
                    Amkor.strWfrttl = st.Default_WQty;
                    Amkor.strAmkorid = st.Amkorid;
                    Amkor.strCust = st.Cust;
                    Amkor.strRcvdate = st.Rcvddate;
                    Amkor.strBillNo = st.Bill;
                    Amkor.strLotDcc = st.Lot_Dcc;
                    Amkor.strLotType = st.Lot_type;
                    Amkor.strWaferLotNo = st.Wafer_lot;
                    Amkor.strCoo = st.strCoo;
                    Amkor.strOperator = st.strop;
                    Amkor.strWSN = st.WSN;
                    Amkor.strRID = st.ReelID;
                    Amkor.strReelDCC = st.ReelDCC;

                    return Amkor;
                }

            }

            return null;
        }

        public int Fnc_GetLotindex_Revision(string strData, string strQty)
        {
            int nCount = dataGridView_Lot.Rows.Count;

            if (nCount < 0)
                return -1;

            string strLotno = "", strDieQty = "";

            for (int n = 0; n < nCount; n++)
            {
                strDieQty = dataGridView_Lot.Rows[n].Cells[3].Value.ToString();
                strLotno = dataGridView_Lot.Rows[n].Cells[1].Value.ToString();

                if (BankHost_main.strLot2Wfr == "TRUE")
                {
                    if (BankHost_main.strWork_QtyPos == "-1" && BankHost_main.strWork_WfrQtyPos == "-1")
                    {
                        return real_index;
                    }
                }
                else if (BankHost_main.strWork_QtyPos == "-1" ? true : strDieQty == strQty
                    && strLotno == strData)
                {
                    return n;
                }
                else
                {
                    strLotno = dataGridView_Lot.Rows[n].Cells[1].Value.ToString();
                    strDieQty = dataGridView_Lot.Rows[n].Cells[3].Value.ToString();



                    if (strData == strLotno && strDieQty == strQty)
                    {
                        return n;
                    }
                }
            }

            return -1;
        }


        private void textBox_Readdata_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (textBox_Readdata.ImeMode != ImeMode.Alpha)
                {
                    textBox_Readdata.ImeMode = ImeMode.Alpha;
                }

                if (BankHost_main.nScanMode == 1) // gun scaner mode
                {
                    BankHost_main.strScanData = textBox_Readdata.Text;

                    BankHost_main.bGunRingMode_Run = true;

                    while (BankHost_main.bGunRingMode_Run)
                    {
                        Thread.Sleep(1);
                        System.Windows.Forms.Application.DoEvents();
                    }

                    textBox_Readdata.Text = "";
                    textBox_Readdata.Focus();
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                ClickTime();

                {
                    Amkor_label_Print_Process(textBox1.Text.ToUpper(), AmkorLabelCnt);
                    textBox1.Text = "";
                }
            }
        }

        private void Amkor_label_Print_Process(string strBcr)
        {
            stAmkor_Label temp = new stAmkor_Label();
            string[] str_temp = strBcr.Replace(':', ',').Split(',');

            if (str_temp.Length == 7)
            {
                temp.Lot = str_temp[0];
                temp.DCC = str_temp[1];
                temp.Device = str_temp[2];
                temp.DQTY = string.Format("{0:%D10}", str_temp[3]);
                temp.WQTY = string.Format("{0:%D5}", str_temp[4]);
                temp.AMKOR_ID = string.Format("{0:%D10}", str_temp[5]);
                temp.CUST = string.Format("{0:D10}", str_temp[6]);
                temp.Wafer_ID = "";

                if (check_duplicate(temp.AMKOR_ID) == false)
                {
                    label_list.Add(temp);


                    tot_lots++;
                    dataGridView_label.Rows.Add(tot_lots.ToString(), temp.Lot, temp.DCC, temp.Device, temp.DQTY, temp.WQTY, temp.AMKOR_ID, temp.CUST, temp.Wafer_ID);

                    tot_die += int.Parse(str_temp[3]);
                    tot_wfr += int.Parse(str_temp[4]);
                    Frm_Print.Fnc_Print(temp);
                    speech.SpeakAsyncCancelAll();
                    speech.SpeakAsync(tot_lots.ToString());



                    lprinted_lots.Text = tot_lots.ToString();
                    ldie.Text = tot_die.ToString();
                    lwfr.Text = tot_wfr.ToString();
                }
                else
                {
                    speech.SpeakAsyncCancelAll();
                    speech.SpeakAsync("중복된 라벨 입니다.");

                }
            }
            else if (str_temp.Length == 8)
            {
                temp.Lot = str_temp[0];
                temp.DCC = str_temp[1];
                temp.Device = str_temp[2];
                temp.DQTY = string.Format("{0:%D10}", str_temp[3]);
                temp.WQTY = string.Format("{0:%D5}", str_temp[4]);
                temp.AMKOR_ID = string.Format("{0:%D10}", str_temp[5]);
                temp.CUST = string.Format("{0:D10}", str_temp[6]);
                temp.Wafer_ID = str_temp[7];

                if (check_duplicate(temp.AMKOR_ID) == false)
                {
                    label_list.Add(temp);

                    tot_lots++;
                    dataGridView_label.Rows.Add(tot_lots.ToString(), temp.Lot, temp.DCC, temp.Device, temp.DQTY, temp.WQTY, temp.AMKOR_ID, temp.CUST, temp.Wafer_ID);
                    tot_die += int.Parse(str_temp[3]);
                    tot_wfr += int.Parse(str_temp[4]);
                    Frm_Print.Fnc_Print(temp);
                    speech.SpeakAsyncCancelAll();
                    speech.SpeakAsync(tot_lots.ToString());

                    lprinted_lots.Text = tot_lots.ToString();
                    ldie.Text = tot_die.ToString();
                    lwfr.Text = tot_wfr.ToString();
                }
                else
                {
                    speech.SpeakAsyncCancelAll();
                    speech.SpeakAsync("중복된 라벨 입니다.");
                }
            }
        }

        private void Amkor_label_Print_Process(string strBcr, int cnt)
        {

            string[] str_temp = strBcr.Replace(':', ',').Split(',');

            if (str_temp.Length == 7)
            {
                stAmkor_Label temp = new stAmkor_Label();

                temp.Lot = str_temp[0];
                temp.DCC = str_temp[1];
                temp.Device = str_temp[2];
                temp.DQTY = string.Format("{0:%D10}", str_temp[3]);
                temp.WQTY = string.Format("{0:%D5}", str_temp[4]);
                temp.AMKOR_ID = string.Format("{0:%D10}", str_temp[5]);
                temp.CUST = string.Format("{0:D10}", str_temp[6]);
                temp.Wafer_ID = "";

                bool pass = false;

                pass = check_duplicate(temp.AMKOR_ID);



                if (pass == false)
                {
                    label_list.Add(temp);


                    tot_lots++;
                    dataGridView_label.Rows.Add(cnt, temp.Lot, temp.DCC, temp.Device, temp.DQTY, temp.WQTY, temp.AMKOR_ID, temp.CUST, temp.Wafer_ID);

                    tot_die += int.Parse(str_temp[3]);
                    tot_wfr += int.Parse(str_temp[4]);
                    Frm_Print.Fnc_Print(temp, cnt, int.Parse(numericUpDown1.Value.ToString()));
                    speech.SpeakAsyncCancelAll();
                    speech.SpeakAsync(tot_lots.ToString());



                    lprinted_lots.Text = tot_lots.ToString();
                    ldie.Text = tot_die.ToString();
                    lwfr.Text = tot_wfr.ToString();

                    tb_next.Text = (++AmkorLabelCnt).ToString();
                }
                else
                {
                    speech.SpeakAsyncCancelAll();
                    speech.SpeakAsync("중복된 라벨 입니다.");

                }
            }
            else if (str_temp.Length == 8)
            {
                stAmkor_Label temp = new stAmkor_Label();

                temp.Lot = str_temp[0];
                temp.DCC = str_temp[1];
                temp.Device = str_temp[2];
                temp.DQTY = string.Format("{0:%D10}", str_temp[3]);
                temp.WQTY = string.Format("{0:%D5}", str_temp[4]);
                temp.AMKOR_ID = string.Format("{0:%D10}", str_temp[5]);
                temp.CUST = string.Format("{0:D10}", str_temp[6]);
                temp.Wafer_ID = str_temp[7];

                if (check_duplicate(temp.AMKOR_ID) == false)
                {
                    label_list.Add(temp);


                    tot_lots++;
                    dataGridView_label.Rows.Add(cnt, temp.Lot, temp.DCC, temp.Device, temp.DQTY, temp.WQTY, temp.AMKOR_ID, temp.CUST, temp.Wafer_ID);
                    tot_die += int.Parse(str_temp[3]);
                    tot_wfr += int.Parse(str_temp[4]);
                    Frm_Print.Fnc_Print(temp, cnt, int.Parse(numericUpDown1.Value.ToString()));
                    speech.SpeakAsyncCancelAll();
                    speech.SpeakAsync(tot_lots.ToString());

                    lprinted_lots.Text = tot_lots.ToString();
                    ldie.Text = tot_die.ToString();
                    lwfr.Text = tot_wfr.ToString();

                    tb_next.Text = (++AmkorLabelCnt).ToString();
                }
                else
                {
                    speech.SpeakAsyncCancelAll();
                    speech.SpeakAsync("중복된 라벨 입니다.");
                }
            }
            else if (str_temp.Length == 9)
            {
                AmkorBcrInfo temp = new AmkorBcrInfo();

                temp.strLotNo = str_temp[0];
                temp.strLotDcc = str_temp[1];
                temp.strDevice = str_temp[2];
                temp.strDieQty = string.Format("{0:%D10}", str_temp[3]);
                temp.strWfrttl = temp.strWfrQty = string.Format("{0:%D5}", str_temp[4]);
                temp.strAmkorid = string.Format("{0:%D10}", str_temp[5]);
                temp.strCust = string.Format("{0:D10}", str_temp[6]);
                temp.strWaferLotNo = str_temp[7];
                temp.strWSN = str_temp[8];

                if (check_duplicate(temp.strAmkorid) == false)
                {
                    tot_lots++;
                    dataGridView_label.Rows.Add(cnt, temp.strLotNo, temp.strLotDcc, temp.strDevice, temp.strDieQty, temp.strWfrQty, temp.strAmkorid, temp.strCust, temp.strWaferLotNo);
                    tot_die += int.Parse(str_temp[3]);
                    tot_wfr += int.Parse(str_temp[4]);
                    Frm_Print.Fnc_Print(temp, 2, cnt, GetNumericValue());
                    speech.SpeakAsyncCancelAll();
                    speech.SpeakAsync(tot_lots.ToString());

                    lprinted_lots.Text = tot_lots.ToString();
                    ldie.Text = tot_die.ToString();
                    lwfr.Text = tot_wfr.ToString();

                    tb_next.Text = (++AmkorLabelCnt).ToString();
                }
                else
                {
                    speech.SpeakAsyncCancelAll();
                    speech.SpeakAsync("중복된 라벨 입니다.");
                }
            }

        }

        private stAmkor_Label getLabelInfo(string strBcr)
        {
            stAmkor_Label temp = new stAmkor_Label();
            string[] str_temp = strBcr.Replace(':', ',').Split(',');

            //3808013.2           :01   :ZT003 - J1            :0000004230:00001:0011106429:00379
            //3808013.2:01:ZT003-J1:0000004230:00001:0011106429:00379
            //FH513P005-03.01::FH513-2501-P-C250W-4KN4:8422:1::699


            if (str_temp.Length == 7)
            {
                temp.Lot = str_temp[0].Trim();
                temp.DCC = str_temp[1].Trim();
                temp.Device = str_temp[2].Trim();
                temp.DQTY = str_temp[3].Trim();// string.Format("{0:%D10}", str_temp[3]);
                temp.WQTY = str_temp[4].Trim();// string.Format("{0:%D5}", str_temp[4]);
                temp.AMKOR_ID = str_temp[5].Trim();// string.Format("{0:%D10}", str_temp[5]);
                temp.CUST = str_temp[6].Trim();// string.Format("{0:D10}", str_temp[6]);
                temp.Wafer_ID = "";
            }
            else //if (str_temp.Length == 8)
            {
                temp.Lot = str_temp[0].Trim();
                temp.DCC = str_temp[1].Trim();
                temp.Device = str_temp[2].Trim();
                temp.DQTY = string.Format("{0:%D10}", str_temp[3].Trim());
                temp.WQTY = string.Format("{0:%D5}", str_temp[4].Trim());
                temp.AMKOR_ID = string.Format("{0:%D10}", str_temp[5].Trim());
                temp.CUST = string.Format("{0:D10}", str_temp[6].Trim());
                temp.Wafer_ID = temp.CUST == "379" ? "" : str_temp[7].Trim();
            }

            return temp;
        }


        int WaferReturnSEQNum = -1;

        private void WaferReturn_label_Print_Process(string strBcr, int cnt)
        {
            stAmkor_Label temp = new stAmkor_Label();
            string[] str_temp = strBcr.Replace(':', ',').Split(',');

            //3808013.2           :01   :ZT003 - J1            :0000004230:00001:0011106429:00379
            //3808013.2:01:ZT003-J1:0000004230:00001:0011106429:00379
            //FH513P005-03.01::FH513-2501-P-C250W-4KN4:8422:1::699

            if (cb_Qualcomm.Checked == false)
            {
                if (str_temp.Length == 6)
                {
                    temp.Lot = str_temp[0].Trim();
                    temp.DCC = str_temp[1].Trim();
                    temp.Device = str_temp[2].Trim();
                    temp.DQTY = str_temp[3].Trim();// string.Format("{0:%D10}", str_temp[3]);
                    temp.WQTY = str_temp[4].Trim();// string.Format("{0:%D5}", str_temp[4]);
                    temp.AMKOR_ID = str_temp[5].Trim();// string.Format("{0:%D10}", str_temp[5]);
                    //temp.CUST = str_temp[6].Trim();// string.Format("{0:D10}", str_temp[6]);
                    temp.Wafer_ID = "";

                    bool pass = false;

                    pass = check_WaferReturnDuplicate(temp);

                    if (pass == false)
                    {
                        if (cb_WaferReturnPrint.Checked == false)
                            Frm_Print.Fnc_Print(temp, WaferReturnSEQNum, dgv_ReturnWafer.RowCount);

                    }
                    else
                    {

                    }
                }
                else if (str_temp.Length == 7)
                {
                    temp.Lot = str_temp[0].Trim();
                    temp.DCC = str_temp[1].Trim();
                    temp.Device = str_temp[2].Trim();
                    temp.DQTY = str_temp[3].Trim();// string.Format("{0:%D10}", str_temp[3]);
                    temp.WQTY = str_temp[4].Trim();// string.Format("{0:%D5}", str_temp[4]);
                    temp.AMKOR_ID = str_temp[5].Trim();// string.Format("{0:%D10}", str_temp[5]);
                    temp.CUST = str_temp[6].Trim();// string.Format("{0:D10}", str_temp[6]);
                    temp.Wafer_ID = "";

                    bool pass = false;

                    pass = check_WaferReturnDuplicate(temp);

                    if (pass == false)
                    {
                        if (cb_WaferReturnPrint.Checked == false)
                            Frm_Print.Fnc_Print(temp, WaferReturnSEQNum, dgv_ReturnWafer.RowCount);

                    }
                    else
                    {

                    }
                }
                else //if (str_temp.Length == 8)
                {
                    temp.Lot = str_temp[0].Trim();
                    temp.DCC = str_temp[1].Trim();
                    temp.Device = str_temp[2].Trim();
                    temp.DQTY = string.Format("{0:%D10}", str_temp[3].Trim());
                    temp.WQTY = string.Format("{0:%D5}", str_temp[4].Trim());
                    temp.AMKOR_ID = string.Format("{0:%D10}", str_temp[5].Trim());
                    temp.CUST = string.Format("{0:D10}", str_temp[6].Trim());
                    temp.Wafer_ID = temp.CUST == "00379" ? "" : str_temp[7].Trim();

                    bool pass = false;

                    pass = check_WaferReturnDuplicate(temp);

                    if (pass == false)
                    {
                        if (cb_WaferReturnPrint.Checked == false)
                            Frm_Print.Fnc_Print(temp, WaferReturnSEQNum, dgv_ReturnWafer.RowCount);


                    }
                    else
                    {

                    }
                }
            }
            else
            {
                bool pass = false;
                temp = transCode(tb_WaferReturnScan.Text.ToUpper());
                pass = check_QualcommWaferReturnDuplicate(ref temp);

                if (pass == false)
                {
                    if (cb_WaferReturnPrint.Checked == false)
                        Frm_Print.Fnc_Print(temp, WaferReturnSEQNum, dgv_ReturnWafer.RowCount);

                    string res = Frm_Print.MakeQualcommLabel(tb_WaferReturnScan.Text.ToUpper());

                    Frm_Print.QualcomSocket_MessageSend(res);
                    Frm_Print.QualcomSocket_MessageSend(res);
                    Frm_Print.QualcomSocket_MessageSend(res);
                }




            }

        }


        bool check_duplicate(string amkor_id)
        {
            bool res = false;

            for (int i = 0; i < dataGridView_label.RowCount; i++)
            {
                if (dataGridView_label.Rows[i].Cells["AMKOR_ID"].Value.ToString() == amkor_id)
                {
                    dataGridView_label.Rows[i].Selected = true;
                    dataGridView_label.FirstDisplayedScrollingRowIndex = i;
                    res = true;
                    break;
                }
            }

            return res;
        }

        public bool check_WaferReturnDuplicate(stAmkor_Label amkorLabel)
        {
            bool res = false;
            bool duplicate = false;
            bool isFail = true;

            for (int i = 0; i < dgv_ReturnWafer.RowCount; i++)
            {
                //   0         1         2     3      4          5     6     7            8                 9                  10                 11              12        13
                // [SEQ],[DEVICE_NAME],[LOT],[DCC],[RETURN_QTY],[LOC],[SL],[REMARK],[SCAN_TIME_1st],[SACN_USER_NAME_1st],[SCAN_TIME_2nd],[SACN_USER_NAME_2nd],[AMKOR_ID],[CUST_CODE]

                if (dgv_ReturnWafer.Rows[i].Cells[1].Value.ToString() == amkorLabel.Device || amkorLabel.Device == null)
                {
                    //FH513P005 - 03.01::FH513 - 2501 - P - C250W - 4KN4: 8422:1::699
                    if (dgv_ReturnWafer.Rows[i].Cells[2].Value.ToString() == amkorLabel.Lot)
                    {
                        if (int.Parse(dgv_ReturnWafer.Rows[i].Cells[3].Value.ToString() == "" ? "0" : dgv_ReturnWafer.Rows[i].Cells[3].Value.ToString()) == (int.Parse(amkorLabel.DCC == "" || amkorLabel.DCC == null ? "0" : amkorLabel.DCC)))
                        {
                            if (int.Parse(dgv_ReturnWafer.Rows[i].Cells[4].Value.ToString()) == int.Parse(amkorLabel.DQTY))
                            {
                                if (int.Parse(amkorLabel.CUST == null ? dgv_ReturnWafer.Rows[i].Cells[13].Value.ToString() : amkorLabel.CUST) == int.Parse(dgv_ReturnWafer.Rows[i].Cells[13].Value.ToString()))
                                {
                                    isFail = false;
                                    if (dgv_ReturnWafer.Rows[i].DefaultCellStyle.BackColor == Color.Blue)
                                    {
                                        res = true;
                                        duplicate = true;


                                        speech.SpeakAsync("중복");

                                    }
                                    else if (dgv_ReturnWafer.Rows[i].DefaultCellStyle.BackColor == Color.Yellow)        // 2차 검수
                                    {
                                        if (dgv_ReturnWafer.Rows[i].Cells[9].Value.ToString() == BankHost_main.strMESID)
                                        {
                                            speech.SpeakAsync("검수자 중복");

                                        }
                                        else
                                        {
                                            dgv_ReturnWafer.FirstDisplayedScrollingRowIndex = i;
                                            dgv_ReturnWafer.Rows[i].DefaultCellStyle.BackColor = Color.Blue;

                                            dgv_ReturnWafer.Rows[i].Cells[10].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                                            dgv_ReturnWafer.Rows[i].Cells[11].Value = BankHost_main.strMESID;

                                            dgv_ReturnWafer.Rows[i].Cells[12].Value = amkorLabel.AMKOR_ID;
                                            res = false;
                                            isFail = false;

                                            string q = string.Format("update [TB_RETURN_WAFER] set [SCAN_TIME_2nd]='{0}',[SCAN_USER_NAME_2nd]='{1}', [AMKOR_ID]='{6}' where [DEVICE_NAME]='{2}' and [LOT]='{3}' and [DCC]='{4}' and [RETURN_QTY]={5}",
                                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                                BankHost_main.strMESID,
                                                amkorLabel.Device,
                                                amkorLabel.Lot,
                                                amkorLabel.DCC,
                                                int.Parse(amkorLabel.DQTY).ToString(),
                                                amkorLabel.AMKOR_ID
                                                );
                                            run_sql_command(q);

                                            int a = dgv_ReturnWafer.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[11].Value.ToString() != "").Count();

                                            l_WaferReturnCount.Text = string.Format("{0} / {1}", a, dgv_ReturnWafer.RowCount);

                                            speech.SpeakAsync(string.Format("{0}", dgv_ReturnWafer.Rows[i].Cells[0].Value));

                                            InfoBoard.Set(dgv_ReturnWafer.Rows[i].Cells[0].Value.ToString(), Color.Yellow, Color.Blue);
                                            InfoBoard.Show();

                                            tb_WaferReturnScan.Focus();

                                            WaferReturnSEQNum = int.Parse(dgv_ReturnWafer.Rows[i].Cells[0].Value.ToString());

                                            if (a == dgv_ReturnWafer.RowCount)
                                            {
                                                speech.SpeakAsync(string.Format("{0} 라트 이차 검수 완료 되었습니다.", dgv_ReturnWafer.RowCount));
                                            }
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        dgv_ReturnWafer.FirstDisplayedScrollingRowIndex = i;
                                        dgv_ReturnWafer.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;

                                        dgv_ReturnWafer.Rows[i].Cells[8].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                                        dgv_ReturnWafer.Rows[i].Cells[9].Value = BankHost_main.strMESID;

                                        dgv_ReturnWafer.Rows[i].Cells[12].Value = amkorLabel.AMKOR_ID;
                                        res = true;
                                        isFail = false;

                                        string q = string.Format("update [TB_RETURN_WAFER] set [SCAN_TIME_1st]='{0}',[SCAN_USER_NAME_1st]='{1}', [AMKOR_ID]='{6}' where [DEVICE_NAME]='{2}' and [LOT]='{3}' and [DCC]='{4}' and [RETURN_QTY]={5}",
                                            DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                            BankHost_main.strMESID,
                                            amkorLabel.Device,
                                            amkorLabel.Lot,
                                            amkorLabel.DCC,
                                            int.Parse(amkorLabel.DQTY).ToString(),
                                            amkorLabel.AMKOR_ID
                                            );
                                        run_sql_command(q);

                                        int a = dgv_ReturnWafer.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[9].Value.ToString() != "").Count();

                                        l_WaferReturnCount.Text = string.Format("{0} / {1}", a, dgv_ReturnWafer.RowCount);

                                        speech.SpeakAsync(string.Format("{0}", dgv_ReturnWafer.Rows[i].Cells[0].Value));

                                        InfoBoard.Set(dgv_ReturnWafer.Rows[i].Cells[0].Value.ToString(), Color.Yellow, Color.Blue);
                                        InfoBoard.Show();

                                        tb_WaferReturnScan.Focus();

                                        if (a == dgv_ReturnWafer.RowCount)
                                        {
                                            speech.SpeakAsync(string.Format("{0} 라트 일차 검수 완료 되었습니다.", dgv_ReturnWafer.RowCount));
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (isFail == true)
            {
                speech.SpeakAsync("잘 못된 라트 입니다.");
                res = true;

                if (duplicate == true)
                {
                    Form_Board _Board = new Form_Board("Validation fail", Color.Black, Color.Red);
                    _Board.ShowDialog();
                }
            }

            return res;
        }

        public bool check_QualcommWaferReturnDuplicate(ref stAmkor_Label amkorLabel)
        {
            bool res = false;
            bool duplicate = false;
            bool isFail = true;

            for (int i = 0; i < dgv_ReturnWafer.RowCount; i++)
            {
                //   0         1         2     3      4          5     6     7            8                 9                  10                 11              12        13
                // [SEQ],[DEVICE_NAME],[LOT],[DCC],[RETURN_QTY],[LOC],[SL],[REMARK],[SCAN_TIME_1st],[SACN_USER_NAME_1st],[SCAN_TIME_2nd],[SACN_USER_NAME_2nd],[AMKOR_ID],[CUST_CODE]

                if (dgv_ReturnWafer.Rows[i].Cells[1].Value.ToString() == amkorLabel.Device || amkorLabel.Device == null)
                {
                    //FH513P005 - 03.01::FH513 - 2501 - P - C250W - 4KN4: 8422:1::699
                    if (dgv_ReturnWafer.Rows[i].Cells[2].Value.ToString() == amkorLabel.Lot)
                    {
                        if (int.Parse(dgv_ReturnWafer.Rows[i].Cells[3].Value.ToString() == "" ? "0" : dgv_ReturnWafer.Rows[i].Cells[3].Value.ToString()) == (int.Parse(amkorLabel.DCC == "" || amkorLabel.DCC == null ? "0" : amkorLabel.DCC)))
                        {
                            if (int.Parse(dgv_ReturnWafer.Rows[i].Cells[4].Value.ToString()) == int.Parse(amkorLabel.DQTY))
                            {
                                if (int.Parse(amkorLabel.CUST == null ? dgv_ReturnWafer.Rows[i].Cells[13].Value.ToString() : amkorLabel.CUST) == int.Parse(dgv_ReturnWafer.Rows[i].Cells[13].Value.ToString()))
                                {
                                    isFail = false;
                                    if (dgv_ReturnWafer.Rows[i].DefaultCellStyle.BackColor == Color.Blue)
                                    {
                                        res = true;
                                        duplicate = true;


                                        speech.SpeakAsync("중복");

                                    }
                                    else if (dgv_ReturnWafer.Rows[i].DefaultCellStyle.BackColor == Color.Yellow)        // 2차 검수
                                    {
                                        if (dgv_ReturnWafer.Rows[i].Cells[9].Value.ToString() == BankHost_main.strMESID)
                                        {
                                            speech.SpeakAsync("검수자 중복");

                                        }
                                        else
                                        {
                                            dgv_ReturnWafer.FirstDisplayedScrollingRowIndex = i;
                                            dgv_ReturnWafer.Rows[i].DefaultCellStyle.BackColor = Color.Blue;

                                            dgv_ReturnWafer.Rows[i].Cells[10].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                                            dgv_ReturnWafer.Rows[i].Cells[11].Value = BankHost_main.strMESID;

                                            dgv_ReturnWafer.Rows[i].Cells[12].Value = amkorLabel.AMKOR_ID;
                                            res = false;
                                            isFail = false;

                                            string q = string.Format("update [TB_RETURN_WAFER] set [SCAN_TIME_2nd]='{0}',[SCAN_USER_NAME_2nd]='{1}', [AMKOR_ID]='{6}' where [DEVICE_NAME]='{2}' and [LOT]='{3}' and [DCC]='{4}' and [RETURN_QTY]={5}",
                                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                                BankHost_main.strMESID,
                                                amkorLabel.Device,
                                                amkorLabel.Lot,
                                                amkorLabel.DCC,
                                                int.Parse(amkorLabel.DQTY).ToString(),
                                                amkorLabel.AMKOR_ID
                                                );
                                            run_sql_command(q);

                                            int a = int.Parse(l_WaferReturnCount.Text.Split('/')[0]);

                                            l_WaferReturnCount.Text = string.Format("{0} / {1}", ++a, dgv_ReturnWafer.RowCount);

                                            speech.SpeakAsync(string.Format("{0}", dgv_ReturnWafer.Rows[i].Cells[0].Value));

                                            InfoBoard.Set(dgv_ReturnWafer.Rows[i].Cells[0].Value.ToString(), Color.Yellow, Color.Blue);
                                            InfoBoard.Show();

                                            tb_WaferReturnScan.Focus();

                                            WaferReturnSEQNum = int.Parse(dgv_ReturnWafer.Rows[i].Cells[0].Value.ToString());

                                            if (a == dgv_ReturnWafer.RowCount)
                                            {
                                                speech.SpeakAsync(string.Format("{0} 라트 이차 검수 완료 되었습니다.", dgv_ReturnWafer.RowCount));
                                            }

                                            amkorLabel.AMKOR_ID = dgv_ReturnWafer.Rows[i].Cells[12].Value.ToString();
                                            amkorLabel.CUST = dgv_ReturnWafer.Rows[i].Cells[13].Value.ToString();
                                            amkorLabel.DCC = dgv_ReturnWafer.Rows[i].Cells[3].Value.ToString();
                                            amkorLabel.WQTY = dgv_ReturnWafer.Rows[i].Cells[4].Value.ToString();
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        dgv_ReturnWafer.FirstDisplayedScrollingRowIndex = i;
                                        dgv_ReturnWafer.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;

                                        dgv_ReturnWafer.Rows[i].Cells[8].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                                        dgv_ReturnWafer.Rows[i].Cells[9].Value = BankHost_main.strMESID;

                                        dgv_ReturnWafer.Rows[i].Cells[12].Value = amkorLabel.AMKOR_ID;
                                        res = true;
                                        isFail = false;

                                        string q = string.Format("update [TB_RETURN_WAFER] set [SCAN_TIME_1st]='{0}',[SCAN_USER_NAME_1st]='{1}', [AMKOR_ID]='{6}' where [DEVICE_NAME]='{2}' and [LOT]='{3}' and [DCC]='{4}' and [RETURN_QTY]={5}",
                                            DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                            BankHost_main.strMESID,
                                            amkorLabel.Device,
                                            amkorLabel.Lot,
                                            amkorLabel.DCC,
                                            int.Parse(amkorLabel.DQTY).ToString(),
                                            amkorLabel.AMKOR_ID
                                            );
                                        run_sql_command(q);

                                        int a = int.Parse(l_WaferReturnCount.Text.Split('/')[0]);

                                        l_WaferReturnCount.Text = string.Format("{0} / {1}", ++a, dgv_ReturnWafer.RowCount);

                                        speech.SpeakAsync(string.Format("{0}", dgv_ReturnWafer.Rows[i].Cells[0].Value));

                                        InfoBoard.Set(dgv_ReturnWafer.Rows[i].Cells[0].Value.ToString(), Color.Yellow, Color.Blue);
                                        InfoBoard.Show();

                                        tb_WaferReturnScan.Focus();

                                        if (a == dgv_ReturnWafer.RowCount)
                                        {
                                            speech.SpeakAsync(string.Format("{0} 라트 1차 검수 완료 되었습니다.", dgv_ReturnWafer.RowCount));
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (isFail == true)
            {
                speech.SpeakAsync("잘 못된 라트 입니다.");

                if (duplicate == true)
                {
                    Form_Board _Board = new Form_Board("Validation fail", Color.Black, Color.Red);
                    _Board.ShowDialog();
                }
            }

            return res;
        }

        public int GetQualcomSplitCopys()
        {
            return (int)nud_splitCopys.Value;
        }

        public List<StorageData> GetSplitData(string strLot)
        {
            List<StorageData> bcrinfos = new List<StorageData>();

            //if (GetQualCommSplitGreenLabel() == false)
            {
                foreach (DataGridViewRow row in dataGridView_Device.Rows)
                {
                    string strDevice = row.Cells[1].Value.ToString();

                    string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
                    string strReadfile = "";

                    strReadfile = strFileName + "\\" + strDevice + "\\" + strDevice + ".txt";

                    string[] info = Fnc_ReadFile(strReadfile);

                    for (int i = 0; i < info.Length; i++)
                    {
                        string[] strSplit_data = info[i].Split('\t');
                        StorageData Binfo = new StorageData();

                        if (strSplit_data[2].Substring(3, strSplit_data[2].Length - 3) == strLot.Split(',')[1].Substring(3, strLot.Split(',')[1].Length - 3))
                        {
                            //if (strSplit_data[13] == "Waiting")
                            {

                                Form_Sort.strValReadfile = strReadfile;
                                Form_Sort.strValDevice = Binfo.Device = strSplit_data[1];
                                Form_Sort.strValLot = Binfo.Lot = strSplit_data[2];
                                Form_Sort.strValDcc = Binfo.Lot_Dcc = strSplit_data[3];
                                Binfo.ReadFile = strValReadfile;
                                Binfo.Rcv_Qty = strSplit_data[4];
                                Binfo.Die_Qty = strSplit_data[5];
                                Binfo.Rcv_WQty = strSplit_data[6];
                                Binfo.Rcvddate = strSplit_data[7];
                                Binfo.Lot_type = strSplit_data[8];
                                Binfo.Bill = strSplit_data[9];
                                Binfo.Amkorid = strSplit_data[10];
                                Binfo.Wafer_lot = strSplit_data[11];
                                Binfo.strCoo = strSplit_data[12];
                                Binfo.state = strSplit_data[13] = "Complete";
                                Binfo.strop = strSplit_data[14];
                                Binfo.strGRstatus = strSplit_data[15];
                                Binfo.Default_WQty = strSplit_data[16];

                                bcrinfos.Add(Binfo);

                                info[i] = string.Join("\t", strSplit_data);

                                File.WriteAllLines(strReadfile, info);


                                run_sql_command($"insert into TB_QUALCOMM_SPLIT_LOG values (getdate(), '{BankHost_main.strWork_Cust}', '{Binfo.Lot}', '{Binfo.Lot_Dcc}', '{Binfo.Device}', '{Binfo.Rcv_Qty}', '{Binfo.Default_WQty}', '{Binfo.Rcvddate}', '{Binfo.Bill}', '{Binfo.Amkorid}', 'Complete', '','', '{BankHost_main.strOperator}')");
                            }
                        }
                        //else if (strSplit_data[2].Length == (strSplit_data[2].LastIndexOf(strLot.Split(',')[1]) + strLot.Split(',')[1].Length))
                        //{
                        //    Binfo.Device = strSplit_data[1];
                        //    Binfo.Lot = strSplit_data[2];
                        //    Binfo.Lot_Dcc = strSplit_data[3];
                        //    Binfo.Rcv_Qty = strSplit_data[4];
                        //    Binfo.Die_Qty = strSplit_data[5];
                        //    Binfo.Rcv_WQty = strSplit_data[6];
                        //    Binfo.Rcvddate = strSplit_data[7];
                        //    Binfo.Lot_type = strSplit_data[8];
                        //    Binfo.Bill = strSplit_data[9];
                        //    Binfo.Amkorid = strSplit_data[10];
                        //    Binfo.Wafer_lot = strSplit_data[11];
                        //    Binfo.strCoo = strSplit_data[12];
                        //    Binfo.state = strSplit_data[13] = "Complete";
                        //    Binfo.strop = strSplit_data[14];
                        //    Binfo.strGRstatus = strSplit_data[15];
                        //    Binfo.Default_WQty = strSplit_data[16];

                        //    bcrinfos.Add(Binfo);

                        //    info[i] = string.Join("\t", strSplit_data);

                        //    File.WriteAllLines(strReadfile, info);

                        //    run_sql_command($"insert into TB_QUALCOMM_SPLIT_LOG values (getdate(), '{BankHost_main.strWork_Cust}', '{Binfo.Lot}', '{Binfo.Lot_Dcc}', '{Binfo.Device}', '{Binfo.Rcv_Qty}', '{Binfo.Default_WQty}', '{Binfo.Rcvddate}', '{Binfo.Bill}', '{Binfo.Amkorid}', 'Complete', '','', '{BankHost_main.strOperator}')");
                        //}
                    }
                }
            }

            return bcrinfos;
        }

        public Bcrinfo Fnc_Bcr_Parsing(string strBcr)
        {
            strBcrType = strBcrType == "" ? selectCust.Cast<Dictionary<string, string>>().Where(r => r["CUST_NAME"] == BankHost_main.strCustName).ToList()[0]["BCR_TYPE"] : strBcrType;

            Bcrinfo info = new Bcrinfo();
            if (Properties.Settings.Default.CameraType == "KEYENCE" || BankHost_main.nScanMode == 1 || BankHost_main.nScanMode == 3)
            {
                if (Properties.Settings.Default.LOCATION == "K4")
                {
                    info = K4_Parsing(strBcr.Replace('\r', ' '));
                }
                else if (Properties.Settings.Default.LOCATION == "K5")
                {
                    info = K5_parsing(strBcr);
                }
            }
            else if (Properties.Settings.Default.CameraType == "COGNEX")
            {


                if (Properties.Settings.Default.LOCATION == "K4")
                {
                    string[] temp = strBcr.Split('\t');
                    string code = "";

                    if (temp[0] == "NG")
                        return info;

                    if (strBcrType.Contains("CODE") == true)
                    {
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].Contains(BankHost_main.strWork_SPR) == false)
                            {
                                code += temp[i] + BankHost_main.strWork_SPR;
                            }
                        }

                        while (code.LastIndexOf(BankHost_main.strWork_SPR) == code.Length)
                        {
                            code = code.Substring(0, code.Length - 1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].Contains(BankHost_main.strWork_SPR) == true)
                            {
                                code = temp[i];
                                break;
                            }
                        }
                    }


                    info = K4_Parsing(code.Replace('\r', ' '));
                }
                else if (Properties.Settings.Default.LOCATION == "K5")
                {
                    //info = K5_parsing(strBcr);
                }
            }

            return info;
        }

        private Bcrinfo K5_parsing(string strBcr)
        {
            //nWorkBcrcount 확인 할 것, 고객별 바코드 형식도 확인이 필요할 듯!
            if (strBcr.Contains("LON") || strBcr.Contains("ERROR") || strBcr.Contains("BLOAD"))
                return null;

            ///BCR count check
            Bcrinfo bcr = new Bcrinfo();

            string[] strSplit_DevicePos = new string[2];
            string[] strSplit_LotPos = new string[2];
            string[] strSplit_QtyPos = new string[2];

            int nDevicePos = -1, nLotPos = -1, nQtyPos = -1;

            if (BankHost_main.nProcess == 4001)
            {
                string[] temp = strBcr.Split(':');
                Amkor_label_Print_Process(strBcr);


                return bcr;
            }

            if (BankHost_main.strWork_DevicePos.Contains(','))
            {
                strSplit_DevicePos = BankHost_main.strWork_DevicePos.Split(',');
                nDevicePos = Int32.Parse(strSplit_DevicePos[0]);
            }
            else
                nDevicePos = Int32.Parse(BankHost_main.strWork_DevicePos);

            if (BankHost_main.strWork_LotidPos.Contains(','))
            {
                strSplit_LotPos = BankHost_main.strWork_LotidPos.Split(',');
                nLotPos = Int32.Parse(strSplit_LotPos[0]);
            }
            else
                nLotPos = Int32.Parse(BankHost_main.strWork_LotidPos);

            if (BankHost_main.strWork_QtyPos.Contains(','))
            {
                strSplit_QtyPos = BankHost_main.strWork_QtyPos.Split(',');
                nQtyPos = Int32.Parse(strSplit_QtyPos[0]);
            }
            else
                nQtyPos = Int32.Parse(BankHost_main.strWork_QtyPos);

            char seperator = BankHost_main.strWork_SPR == "" ? '\0' : char.Parse(BankHost_main.strWork_SPR);
            bool bmultibcr = false;

            //1D Scan 인지 확인
            strBcrType = BankHost_main.Host.Host_Get_BcrType(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
            string str1Dbcrcount = "0";
            bool b1Dbcr = false;

            if (strBcrType == "CODE39" || strBcrType == "CODE128")
            {
                b1Dbcr = true;
                str1Dbcrcount = BankHost_main.Host.Host_Get_Bcrcount(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
            }

            if (strBcr.Contains(',') && !b1Dbcr && strBcrType != "PDF417" && BankHost_main.strWork_Shot1Lot == "YES")
            {
                if (BankHost_main.strWork_Cust != "453" || BankHost_main.strWork_Cust != "734")
                    bmultibcr = true;
            }

            string strWaferID = "";
            int nUdigitPos = 0;
            string[] strUdigit = null;

            if (BankHost_main.strWork_Udigit != "")
                strUdigit = BankHost_main.strWork_Udigit.Split(',');
            else
            {
                strUdigit = new string[2];
                strUdigit[0] = "D";
                strUdigit[1] = nLotPos.ToString();
            }

            if (strUdigit[0] == "D")
            {
                nUdigitPos = Int32.Parse(strUdigit[1]);
            }

            if (bmultibcr)//INARI
            {
                string[] strSplit_Bcr1 = strBcr.Split(',');
                int nLength = strSplit_Bcr1.Length;

                BankHost_main.nWorkBcrcount = nLength;

                int nTotalDieQty = 0;
                for (int n = 0; n < nLength; n++)
                {
                    string[] strSplit_Bcr2 = strSplit_Bcr1[n].Split(seperator);
                    if (strSplit_Bcr2.Length < 3)
                        return null;

                    bcr.Device = strSplit_Bcr2[nDevicePos]; bcr.Device = bcr.Device.Trim();

                    if (strSplit_DevicePos[1] != null)
                    {
                        if (strSplit_DevicePos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                            bcr.Device = bcr.Device.Substring(nDigit, bcr.Device.Length - nDigit);
                        }
                        else if (strSplit_DevicePos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                            bcr.Device = bcr.Device.Substring(0, bcr.Device.Length - nDigit);
                        }
                    }

                    bcr.Lot = strSplit_Bcr2[nLotPos]; bcr.Lot = bcr.Lot.Trim();

                    if (strSplit_LotPos[1] != null)
                    {
                        if (strSplit_LotPos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                            bcr.Lot = bcr.Lot.Substring(nDigit, bcr.Lot.Length - nDigit);
                        }
                        else if (strSplit_LotPos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                            bcr.Lot = bcr.Lot.Substring(0, bcr.Lot.Length - nDigit);
                        }
                    }

                    bcr.DieQty = strSplit_Bcr2[nQtyPos]; bcr.DieQty = bcr.DieQty.Trim();

                    if (strSplit_QtyPos[1] != null)
                    {
                        if (strSplit_QtyPos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_QtyPos[1].Substring(1, 1));
                            bcr.DieQty = bcr.DieQty.Substring(nDigit, bcr.DieQty.Length - nDigit);
                        }
                        else if (strSplit_QtyPos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_QtyPos[1].Substring(1, 1));
                            bcr.DieQty = bcr.DieQty.Substring(0, bcr.DieQty.Length - nDigit);
                        }
                    }

                    strWaferID = strSplit_Bcr2[nUdigitPos];

                    nTotalDieQty = nTotalDieQty + Int32.Parse(bcr.DieQty);
                }

                bcr.DieQty = nTotalDieQty.ToString();
            }
            else
            {
                BankHost_main.nWorkBcrcount = 1;

                string[] strSplit_Bcr = strBcr.Split(seperator);

                if (b1Dbcr)
                {
                    if (strSplit_Bcr.Length < Int32.Parse(str1Dbcrcount))
                        return null;

                    string strID = "";
                    for (int n = 0; n < strSplit_Bcr.Length; n++)
                    {
                        string strBarcode = strSplit_Bcr[n];
                        if (strBarcode != "")
                        {
                            if (Properties.Settings.Default.LOCATION == "K4")
                            {
                                if (strBarcode.Substring(0, 2) == "1T")
                                {
                                    //strWaferID = strBarcode;
                                    bcr.Lot = strBarcode.Substring(2, strBarcode.Length - 2);
                                    bcr.Lot = bcr.Lot.Trim();
                                }
                                else if (strBarcode.Substring(0, 1) == "P" && strBcrType == "CODE128")
                                {
                                    bcr.Device = strBarcode.Substring(1, strBarcode.Length - 1);
                                    bcr.Device = bcr.Device.Trim();
                                }
                                else if (strBarcode.Substring(0, 1) == "Q")
                                {
                                    bcr.DieQty = strBarcode.Substring(1, strBarcode.Length - 1);
                                    bcr.DieQty = bcr.DieQty.Trim();
                                }
                                else if (strBarcode.Substring(0, 3) == "P30" && strBcrType == "CODE39")
                                {
                                    bcr.Device = strBarcode.Substring(3, strBarcode.Length - 3);
                                    bcr.Device = bcr.Device.Trim();
                                }

                                if (strUdigit[1] == strBarcode.Substring(0, strUdigit.Length))
                                {
                                    strID = strBarcode;
                                }
                            }
                            else if (Properties.Settings.Default.LOCATION == "K5")
                            {
                                string[] strSplit_Bcr2 = strBcr.Split(seperator);
                                if (strSplit_Bcr2.Length < 3)
                                    return null;

                                if (BankHost_main.strWork_DevicePos != "-1")
                                {
                                    if (strSplit_DevicePos[1] != null)
                                    {
                                        if (strSplit_DevicePos[1].Substring(0, 1) == "L")
                                        {
                                            int nDigit = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                                            bcr.Device = bcr.Device.Substring(nDigit, bcr.Device.Length - nDigit);
                                        }
                                        else if (strSplit_DevicePos[1].Substring(0, 1) == "R")
                                        {
                                            int nDigit = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                                            bcr.Device = bcr.Device.Substring(0, bcr.Device.Length - nDigit);
                                        }
                                    }
                                    else
                                    {
                                        bcr.Device = strSplit_Bcr2[nDevicePos];
                                    }
                                }

                                if (BankHost_main.strWork_LotidPos != "-1")
                                {
                                    bcr.Lot = strSplit_Bcr2[nLotPos]; bcr.Lot = bcr.Lot.Trim();

                                    if (strSplit_LotPos[1] != null)
                                    {
                                        if (strSplit_LotPos[1].Substring(0, 1) == "L")
                                        {
                                            int nDigit = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                                            bcr.Lot = bcr.Lot.Substring(nDigit, bcr.Lot.Length - nDigit);
                                        }
                                        else if (strSplit_LotPos[1].Substring(0, 1) == "R")
                                        {
                                            int nDigit = Int32.Parse(strSplit_LotPos[1].Substring(1, strSplit_LotPos[1].Length - 1));
                                            bcr.Lot = bcr.Lot.Substring(0, bcr.Lot.Length - nDigit);
                                        }
                                    }
                                }

                                if (BankHost_main.strWork_QtyPos != "-1")
                                {
                                    bcr.DieQty = strSplit_Bcr2[nQtyPos]; bcr.DieQty = bcr.DieQty.Trim();

                                    if (strSplit_QtyPos[1] != null)
                                    {
                                        if (strSplit_QtyPos[1].Substring(0, 1) == "L")
                                        {
                                            int nDigit = Int32.Parse(strSplit_QtyPos[1].Substring(1, 1));
                                            bcr.DieQty = bcr.DieQty.Substring(nDigit, bcr.DieQty.Length - nDigit);
                                        }
                                        else if (strSplit_QtyPos[1].Substring(0, 1) == "R")
                                        {
                                            int nDigit = Int32.Parse(strSplit_QtyPos[1].Substring(1, 1));
                                            bcr.DieQty = bcr.DieQty.Substring(0, bcr.DieQty.Length - nDigit);
                                        }
                                    }
                                }


                                //if (n==0)
                                //{
                                //    bcr.Device = strBarcode.Trim();
                                //}
                                //else if(n==1)
                                //{
                                //    bcr.Lot = strBarcode.Trim();
                                //}
                                //else if(n==2)
                                //{
                                //    bcr.DieQty = strBarcode.Trim();                                    
                                //}
                                //else if(n==3)
                                //{
                                //    bcr.WfrQty = strBarcode.Trim();
                                //}
                            }
                        }
                    }

                    strWaferID = string.Format("{0}_{1}", bcr.Lot, strID);
                }
                else
                {
                    if (strSplit_Bcr.Length < 3)
                        return null;

                    bcr.Device = strSplit_Bcr[nDevicePos]; bcr.Device = bcr.Device.Trim();

                    if (strSplit_DevicePos[1] != null)
                    {
                        if (strSplit_DevicePos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                            bcr.Device = bcr.Device.Substring(nDigit, bcr.Device.Length - nDigit);
                        }
                        else if (strSplit_DevicePos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                            bcr.Device = bcr.Device.Substring(0, bcr.Device.Length - nDigit);
                        }
                    }

                    bcr.Lot = strSplit_Bcr[nLotPos]; bcr.Lot = bcr.Lot.Trim();

                    if (strSplit_LotPos[1] != null)
                    {
                        if (strSplit_LotPos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                            bcr.Lot = bcr.Lot.Substring(nDigit, bcr.Lot.Length - nDigit);
                        }
                        else if (strSplit_LotPos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                            bcr.Lot = bcr.Lot.Substring(0, bcr.Lot.Length - nDigit);
                        }
                    }

                    bcr.DieQty = strSplit_Bcr[nQtyPos]; bcr.DieQty = bcr.DieQty.Trim();

                    if (strSplit_QtyPos[1] != null)
                    {
                        if (strSplit_QtyPos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_QtyPos[1].Substring(1, 1));
                            bcr.DieQty = bcr.DieQty.Substring(nDigit, bcr.DieQty.Length - nDigit);
                        }
                        else if (strSplit_QtyPos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_QtyPos[1].Substring(1, 1));
                            bcr.DieQty = bcr.DieQty.Substring(0, bcr.DieQty.Length - nDigit);
                        }
                    }

                    strWaferID = string.Format("{0}", strSplit_Bcr[nUdigitPos]);

                    strWaferID = strWaferID.Trim();

                    if (strWaferID.Contains(","))
                    {
                        string[] strSplit = strWaferID.Split(',');
                        strWaferID = strSplit[0];
                    }

                    //strWaferID = strSplit_Bcr[nLotPos];
                }
            }

            if (BankHost_main.strWork_LotDigit.Contains("."))
            {
                int st = bcr.Lot.Length - 5;
                int index = bcr.Lot.IndexOf('.', st);
                bcr.Lot = bcr.Lot.Substring(0, index);
            }

            if (BankHost_main.strWork_LotDigit.Contains("-"))
            {
                string strindex = BankHost_main.strWork_LotDigit.Replace("-", "");
                int st = bcr.Lot.Length;
                int index = st - bcr.Lot.IndexOf('-', 0);
                bcr.Lot = bcr.Lot.Substring(0, st - index);
            }

            nValWfrQty = BankHost_main.Host.Host_Get_BcrRead_Wfrcount(BankHost_main.strEqid, bcr.Lot);



            if (((BankHost_main.strWork_QtyPos == "-1" ? false : bcr.DieQty == "") && (BankHost_main.strWork_WfrQtyPos == "-1" ? false : bcr.WfrQty == "")) || bcr.Lot == "")
                return null;

            int nDieTTL = 0, nWfrTTL = 0;
            string strFileName = "", strFileName_Device = "";
            if (bcr.Device == "")
            {
                //디바이스 정보 없는 자재인 경우
                //bcr.Device = strSelDevice; //21.02.17
                string strFile = strExcutionPath + "\\Work\\" + strWorkFileName; //HY210315
                string strReadfile = strFile + "\\" + strSelDevice + "\\" + strSelDevice + ".txt";
                string str = Fnc_Get_Device(strReadfile, bcr.Lot);
                bcr.Device = str;

                nDieTTL = Fnc_GetTTL(strSelDevice, bcr.Lot, 0);
                nWfrTTL = Fnc_GetTTL(strSelDevice, bcr.Lot, 1);

                strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
                strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strSelDevice + "\\" + strSelDevice;
            }
            else
            {
                nDieTTL = Fnc_GetTTL(bcr.Device, bcr.Lot, 0);
                nWfrTTL = Fnc_GetTTL(bcr.Device, bcr.Lot, 1);

                strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
                strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + bcr.Device + "\\" + bcr.Device;
            }

            int nQty = bcr.DieQty == "" ? 0 : Int32.Parse(bcr.DieQty);

            bcr.DieTTL = nDieTTL.ToString();
            bcr.WfrTTL = nWfrTTL.ToString();

            string strSetID = strWaferID + "_" + bcr.DieQty;
            string strGet = BankHost_main.Host.Host_Set_BcrReadInfo(BankHost_main.strEqid, bcr.Device, bcr.Lot, strSetID);

            if (strGet == "True")
            {
                bcr.result = "DUPLICATE";
            }
            else
            {
                if (BankHost_main.strWork_Lotinfo == "")
                {
                    bcr.result = "OK";
                }
                else if (BankHost_main.strWork_Lotinfo != bcr.Lot)
                {
                    bcr.result = "MISSMATCH";
                }
                else
                {
                    bcr.result = "OK";
                }
            }

            bool isUnPrint = false;

            if (BankHost_main.Host.Host_Check_Unprinted_Device(bcr.Device) > 0)
                isUnPrint = true;

            if (BankHost_main.strCustName == "QUALCOMM STD Multi-2D")
                isUnPrint = false;



            if (isUnPrint == true)
                bcr.unprinted_device = true;

            string strlog = string.Format("PARSING+{0}+{1}+{2}+{3}+{4}+{5}+{6}", bcr.Device, bcr.Lot, bcr.DieQty, bcr.DieTTL, bcr.WfrTTL, bcr.result, BankHost_main.strOperator);

            ////DB Save
            string[] strSaveInfo = new string[10];
            strSaveInfo[0] = BankHost_main.strEqid;
            strSaveInfo[1] = "VAL_READ_DATA";
            strSaveInfo[2] = "";
            strSaveInfo[3] = bcr.Device;
            strSaveInfo[4] = bcr.Lot;
            strSaveInfo[5] = bcr.DieQty;
            strSaveInfo[6] = bcr.DieTTL;
            strSaveInfo[7] = nValWfrQty.ToString();
            strSaveInfo[8] = bcr.WfrTTL;
            strSaveInfo[9] = BankHost_main.strOperator;

            // Fnc_SaveLog_Work(strFileName, strlog, strSaveInfo, 0);
            Fnc_SaveLog_Work(strFileName_Device, strlog, strSaveInfo, 1);

            return bcr;
        }

        private int FindCodePos(string rule, string bcr)
        {
            int res = -1;

            string[] temp = bcr.Split(',');

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].IndexOf(rule) == 0)
                {
                    res = i;
                    break;
                }
            }

            return res;
        }

        private Bcrinfo K4_Parsing(string strBcr)
        {
            //nWorkBcrcount 확인 할 것, 고객별 바코드 형식도 확인이 필요할 듯!
            if (strBcr.Contains("LON") || strBcr.Contains("ERROR") || strBcr.Contains("BLOAD"))
                return null;

            BankHost_main.strScanData = strBcr;

            ///BCR count check
            Bcrinfo bcr = new Bcrinfo();

            string[] strSplit_DevicePos = new string[2];
            string[] strSplit_LotPos = new string[2];
            string[] strSplit_QtyPos = new string[2];
            string[] strSplit_WSNPos = new string[2];
            string[] strSplit_LPNPos = new string[2];


            int nDevicePos = -1, nLotPos = -1, nQtyPos = -1, nWSNPos = -1, nLPNPos;

            if (BankHost_main.nProcess == 4001)
            {
                string[] temp = strBcr.Split(':');
                Amkor_label_Print_Process(strBcr);


                return bcr;
            }

            if (BankHost_main.strWork_DevicePos.Contains('/'))
            {
                strSplit_DevicePos = BankHost_main.strWork_DevicePos.Split('/');

                if (Int32.TryParse(strSplit_DevicePos[0], out nDevicePos) == false)
                    nDevicePos = FindCodePos(strSplit_DevicePos[0], strBcr);
            }
            else
                nDevicePos = Int32.Parse(BankHost_main.strWork_DevicePos);

            if (BankHost_main.strWork_LotidPos.Contains('/'))
            {
                strSplit_LotPos = BankHost_main.strWork_LotidPos.Split('/');

                if (Int32.TryParse(strSplit_LotPos[0], out nLotPos) == false)
                    nLotPos = FindCodePos(strSplit_LotPos[0], strBcr);
            }
            else
                nLotPos = Int32.Parse(BankHost_main.strWork_LotidPos);

            if (BankHost_main.strWork_QtyPos.Contains('/'))
            {
                strSplit_QtyPos = BankHost_main.strWork_QtyPos.Split('/');

                if (Int32.TryParse(strSplit_QtyPos[0], out nQtyPos) == false)
                    nQtyPos = FindCodePos(strSplit_QtyPos[0], strBcr);
            }
            else
                nQtyPos = Int32.Parse(BankHost_main.strWork_QtyPos);




            if (BankHost_main.strWork_WSNPos.Contains('/'))
            {
                strSplit_WSNPos = BankHost_main.strWork_WSNPos.Split('/');

                if (Int32.TryParse(strSplit_WSNPos[0], out nWSNPos) == false)
                    nWSNPos = FindCodePos(strSplit_WSNPos[0], strBcr);
            }
            else if (BankHost_main.strWork_WSNPos != "")
                nWSNPos = Int32.Parse(BankHost_main.strWork_WSNPos);


            if (BankHost_main.strWork_LPNPos.Contains('/'))
            {
                strSplit_LPNPos = BankHost_main.strWork_LPNPos.Split('/');

                if (Int32.TryParse(strSplit_LPNPos[0], out nLPNPos) == false)
                    nLPNPos = FindCodePos(strSplit_LPNPos[0], strBcr);
            }
            else
                nLPNPos = Int32.Parse(BankHost_main.strWork_LPNPos);

            char seperator = char.Parse(BankHost_main.strWork_SPR);
            bool bmultibcr = false;

            //1D Scan 인지 확인
            //string strBcrType = BankHost_main.Host.Host_Get_BcrType(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
            string str1Dbcrcount = "0";
            bool b1Dbcr = false;

            if (strBcrType == "CODE39" || strBcrType == "CODE128")
            {
                b1Dbcr = true;
                str1Dbcrcount = AWork.nBcrcount.ToString();// BankHost_main.Host.Host_Get_Bcrcount(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
            }

            if (strBcr.Contains(',') && !b1Dbcr && strBcrType != "PDF417" && BankHost_main.strWork_Shot1Lot == "YES")
            {
                if (BankHost_main.strWork_Cust != "453" || BankHost_main.strWork_Cust != "734")
                    if (BankHost_main.strWork_Cust == "488")
                        bmultibcr = true;
            }

            string strWaferID = "";
            int nUdigitPos = 0;
            string[] strUdigit = null;

            if (BankHost_main.strWork_Udigit != "")
                strUdigit = BankHost_main.strWork_Udigit.Split(',');
            else
            {
                strUdigit = new string[2];
                strUdigit[0] = "D";
                strUdigit[1] = nLotPos.ToString();
            }

            if (strUdigit[0] == "D")
            {
                nUdigitPos = Int32.Parse(strUdigit[1]);
            }

            if (bmultibcr)//INARI
            {
                string[] strSplit_Bcr1 = strBcr.Split(',');
                int nLength = strSplit_Bcr1.Length;

                BankHost_main.nWorkBcrcount = nLength;

                int nTotalDieQty = 0;
                for (int n = 0; n < nLength; n++)
                {
                    string[] strSplit_Bcr2 = strSplit_Bcr1[n].Split(seperator);
                    if (strSplit_Bcr2.Length < 3)
                        return null;

                    bcr.Device = strSplit_Bcr2[nDevicePos]; bcr.Device = bcr.Device.Trim();

                    if (strSplit_DevicePos[1] != null)
                    {
                        if (strSplit_DevicePos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                            bcr.Device = bcr.Device.Substring(nDigit, bcr.Device.Length - nDigit);
                        }
                        else if (strSplit_DevicePos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                            bcr.Device = bcr.Device.Substring(0, bcr.Device.Length - nDigit);
                        }
                        else
                        {
                            for (int i = 0; i < strSplit_Bcr2.Length; i++)
                            {
                                if (strSplit_Bcr2[i].IndexOf(strSplit_DevicePos[1].Trim()) == 0)
                                {
                                    bcr.Device = strSplit_Bcr2[i].Remove(0, strSplit_DevicePos[1].Trim().Length).Trim();
                                    break;
                                }
                            }
                        }
                    }

                    bcr.Lot = strSplit_Bcr2[nLotPos]; bcr.Lot = bcr.Lot.Trim();

                    if (strSplit_LotPos[1] != null && !LotSPR)
                    {
                        if (strSplit_LotPos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                            bcr.Lot = bcr.Lot.Substring(nDigit, bcr.Lot.Length - nDigit);
                        }
                        else if (strSplit_LotPos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                            bcr.Lot = bcr.Lot.Substring(0, bcr.Lot.Length - nDigit);
                        }
                        else
                        {
                            for (int i = 0; i < strSplit_Bcr2.Length; i++)
                            {
                                if (strSplit_Bcr2[i].IndexOf(strSplit_LotPos[1].Trim()) == 0)
                                {
                                    bcr.Lot = strSplit_Bcr2[i].Remove(0, strSplit_LotPos[1].Trim().Length).Trim();
                                    break;
                                }
                            }
                        }
                    }
                    else if (strSplit_LotPos[1] != null && LotSPR)
                    {
                        bcr.Lot = bcr.Lot;
                    }

                    bcr.DieQty = strSplit_Bcr2[nQtyPos]; bcr.DieQty = bcr.DieQty.Trim();

                    if (strSplit_QtyPos[1] != null)
                    {
                        if (strSplit_QtyPos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_QtyPos[1].Substring(1, 1));
                            bcr.DieQty = bcr.DieQty.Substring(nDigit, bcr.DieQty.Length - nDigit);
                        }
                        else if (strSplit_QtyPos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_QtyPos[1].Substring(1, 1));
                            bcr.DieQty = bcr.DieQty.Substring(0, bcr.DieQty.Length - nDigit);
                        }
                        else
                        {
                            for (int i = 0; i < strSplit_Bcr2.Length; i++)
                            {
                                if (strSplit_Bcr2[i].IndexOf(strSplit_QtyPos[1].Trim()) == 0)
                                {
                                    bcr.DieQty = strSplit_Bcr2[i].Remove(0, strSplit_QtyPos[1].Trim().Length).Trim();
                                    break;
                                }
                            }
                        }
                    }

                    strWaferID = strSplit_Bcr2[nUdigitPos];

                    nTotalDieQty = nTotalDieQty + Int32.Parse(bcr.DieQty);
                }

                bcr.DieQty = nTotalDieQty.ToString();
            }
            else
            {
                BankHost_main.nWorkBcrcount = 1;

                string[] strSplit_Bcr = strBcr.Split(seperator);

                if (b1Dbcr)
                {
                    //if (strSplit_Bcr.Length < Int32.Parse(str1Dbcrcount))
                    //    return null;

                    string strID = "";
                    if (BankHost_main.strWork_Model != "QUALCOMM_SPI")
                    {

                        if (BankHost_main.strWork_Model.Contains("WSN") == true && strSplit_DevicePos[0] != null)
                        {
                            for (int n = 0; n < strSplit_Bcr.Length; n++)
                            {
                                if (nDevicePos == -1)
                                {
                                    string res = BarcodeRule2Str(strSplit_DevicePos, strSplit_Bcr[n].Trim());

                                    if (res != "EMPTY")
                                    {
                                        bcr.Device = res.Substring(strSplit_DevicePos[0].Length, strSplit_Bcr[n].Trim().Length - strSplit_DevicePos[0].Length);
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < strSplit_Bcr.Length; i++)
                                    {
                                        if (strSplit_Bcr[i].IndexOf(strSplit_DevicePos[1].Trim()) == 0)
                                        {
                                            bcr.Device = strSplit_Bcr[i].Remove(0, strSplit_DevicePos[1].Trim().Length).Trim();
                                            break;
                                        }
                                    }
                                }

                                if (nLotPos == -1)
                                {
                                    string res = BarcodeRule2Str(strSplit_LotPos, strSplit_Bcr[n].Trim());

                                    if (res != "EMPTY")
                                    {
                                        bcr.Lot = res.Substring(strSplit_LotPos[0].Length, strSplit_Bcr[n].Trim().Length - strSplit_LotPos[0].Length);
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < strSplit_Bcr.Length; i++)
                                    {
                                        if (strSplit_Bcr[i].IndexOf(strSplit_LotPos[1].Trim()) == 0)
                                        {
                                            bcr.Lot = strSplit_Bcr[i].Remove(0, strSplit_LotPos[1].Trim().Length).Trim();
                                            break;
                                        }
                                    }
                                }

                                if (nQtyPos == -1)
                                {
                                    string res = BarcodeRule2Str(strSplit_QtyPos, strSplit_Bcr[n].Trim());

                                    if (res != "EMPTY")
                                    {
                                        bcr.DieQty = res.Substring(strSplit_QtyPos[0].Length, strSplit_Bcr[n].Trim().Length - strSplit_QtyPos[0].Length);
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < strSplit_Bcr.Length; i++)
                                    {
                                        if (strSplit_Bcr[i].IndexOf(strSplit_QtyPos[1].Trim()) == 0)
                                        {
                                            bcr.DieQty = strSplit_Bcr[i].Remove(0, strSplit_QtyPos[1].Trim().Length).Trim();
                                            break;
                                        }
                                    }
                                }

                                if (tc_WSN.Visible == true)
                                {
                                    string res = BarcodeRule2Str(new string[] { Properties.Settings.Default.QorvoWSN, "C" }, strSplit_Bcr[n].Trim());

                                    if (res != "EMPTY")
                                    {
                                        if (bcr.WSN == "")
                                        {
                                            BankHost_main.strWork_WSN = res;
                                            bcr.WSN = res;
                                        }
                                        else
                                        {
                                            speech.Speak("WSN 중복");
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            if (Checkdev(bcr.Device) == true)
                            {
                                if (bcr.Device == "" || bcr.Lot == "" || bcr.DieQty == "" || strSplit_WSNPos[0] == null ? false : bcr.WSN == "")
                                    return null;
                            }
                            else
                            {
                                if (bcr.Device == "" || bcr.Lot == "" || bcr.DieQty == "")
                                    return null;
                            }
                        }
                        else
                        {
                            for (int n = 0; n < strSplit_Bcr.Length; n++)
                            {
                                string strBarcode = strSplit_Bcr[n];

                                if (strBarcode != "")
                                {
                                    if (strBarcode.Substring(0, 2) == "1T")
                                    {
                                        //strWaferID = strBarcode;
                                        bcr.Lot = strBarcode.Substring(2, strBarcode.Length - 2);
                                        bcr.Lot = bcr.Lot.Trim();
                                    }
                                    else if (strBarcode.Substring(0, 1) == "P" && strBcrType == "CODE128")
                                    {
                                        bcr.Device = strBarcode.Substring(1, strBarcode.Length - 1);
                                        bcr.Device = bcr.Device.Trim();
                                    }
                                    else if (strBarcode.Substring(0, 1) == "Q")
                                    {
                                        bcr.DieQty = strBarcode.Substring(1, strBarcode.Length - 1);
                                        bcr.DieQty = bcr.DieQty.Trim();
                                    }
                                    else if (strBarcode.Substring(0, 2) == "WQ" && BankHost_main.strCustName.Contains("AMS") == true)
                                    {
                                        bcr.WfrQty = strBarcode.Substring(2, strBarcode.Length - 2);
                                        bcr.WfrQty = bcr.WfrQty.Trim();
                                    }
                                    else if (strBarcode.Substring(0, 3) == "P30" && strBcrType == "CODE39")
                                    {
                                        bcr.Device = strBarcode.Substring(3, strBarcode.Length - 3);
                                        bcr.Device = bcr.Device.Trim();
                                    }


                                    if (strUdigit[1] == strBarcode.Substring(0, strUdigit.Length))
                                    {
                                        strID = strBarcode;
                                    }
                                }
                            }
                        }
                    }
                    else if (BankHost_main.strWork_Model == "QUALCOMM_SPI")
                    {
                        bcr.Lot = strSplit_Bcr[1].Substring(2, strSplit_Bcr[1].Length - 2);
                        bcr.Lot = bcr.Lot.Trim();

                        bcr.DieQty = strSplit_Bcr[2].Substring(1, strSplit_Bcr[2].Length - 1);
                        bcr.DieQty = bcr.DieQty.Trim();

                        bcr.Device = strSplit_Bcr[0].Substring(3, strSplit_Bcr[0].Length - 3);
                        bcr.Device = bcr.Device.Trim();
                    }

                    strWaferID = string.Format("{0}_{1}", bcr.Lot, strID);
                }
                else
                {
                    if (strSplit_Bcr.Length < 3)
                        return null;
                    string[] strSplit_Bcr1 = strBcr.Split(seperator);

                    bcr.Device = strSplit_Bcr[nDevicePos]; bcr.Device = bcr.Device.Trim();

                    if (strSplit_DevicePos[1] != null)
                    {
                        if (strSplit_DevicePos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                            bcr.Device = bcr.Device.Substring(nDigit, bcr.Device.Length - nDigit);
                        }
                        else if (strSplit_DevicePos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                            bcr.Device = bcr.Device.Substring(0, bcr.Device.Length - nDigit);
                        }
                        else
                        {
                            for (int i = 0; i < strSplit_Bcr1.Length; i++)
                            {
                                if (strSplit_Bcr1[i].IndexOf(strSplit_DevicePos[1].Trim()) == 0)
                                {
                                    bcr.Device = strSplit_Bcr1[i].Remove(0, strSplit_DevicePos[1].Trim().Length).Trim();
                                    break;
                                }
                            }
                        }

                    }

                    bcr.Lot = strSplit_Bcr[nLotPos]; bcr.Lot = bcr.Lot.Trim();

                    if (strSplit_LotPos[1] != null)
                    {
                        if (strSplit_LotPos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                            bcr.Lot = bcr.Lot.Substring(nDigit, bcr.Lot.Length - nDigit);
                        }
                        else if (strSplit_LotPos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                            bcr.Lot = bcr.Lot.Substring(0, bcr.Lot.Length - nDigit);
                        }
                        else
                        {
                            for (int i = 0; i < strSplit_Bcr1.Length; i++)
                            {
                                if (strSplit_Bcr1[i].IndexOf(strSplit_LotPos[1].Trim()) == 0)
                                {
                                    bcr.Lot = strSplit_Bcr1[i].Remove(0, strSplit_LotPos[1].Trim().Length).Trim();
                                    break;
                                }
                            }
                        }
                    }

                    bcr.DieQty = strSplit_Bcr[nQtyPos]; bcr.DieQty = bcr.DieQty.Trim();

                    if (strSplit_QtyPos[1] != null)
                    {
                        if (strSplit_QtyPos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_QtyPos[1].Substring(1, 1));
                            bcr.DieQty = bcr.DieQty.Substring(nDigit, bcr.DieQty.Length - nDigit);
                        }
                        else if (strSplit_QtyPos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_QtyPos[1].Substring(1, 1));
                            bcr.DieQty = bcr.DieQty.Substring(0, bcr.DieQty.Length - nDigit);
                        }
                        else
                        {
                            for (int i = 0; i < strSplit_Bcr1.Length; i++)
                            {
                                if (strSplit_Bcr1[i].IndexOf(strSplit_QtyPos[1].Trim()) == 0)
                                {
                                    bcr.DieQty = strSplit_Bcr1[i].Remove(0, strSplit_QtyPos[1].Trim().Length).Trim();
                                    break;
                                }
                            }
                        }
                    }

                    if (strSplit_LPNPos[1] != null)
                    {
                        if (strSplit_LPNPos[1].Substring(0, 1) == "L")
                        {
                            int nDigit = Int32.Parse(strSplit_LPNPos[1].Substring(1, 1));
                            bcr.LPN = bcr.LPN.Substring(nDigit, bcr.LPN.Length - nDigit);
                        }
                        else if (strSplit_LPNPos[1].Substring(0, 1) == "R")
                        {
                            int nDigit = Int32.Parse(strSplit_LPNPos[1].Substring(1, 1));
                            bcr.LPN = bcr.LPN.Substring(0, bcr.LPN.Length - nDigit);
                        }
                        else
                        {
                            for (int i = 0; i < strSplit_Bcr1.Length; i++)
                            {
                                if (strSplit_Bcr1[i].IndexOf(strSplit_LPNPos[1].Trim()) == 0)
                                {
                                    bcr.LPN = strSplit_Bcr1[i].Remove(0, strSplit_LPNPos[1].Trim().Length).Trim();
                                    strSelLPN = bcr.LPN;
                                    break;
                                }
                            }
                        }
                    }

                    strWaferID = string.Format("{0}", strSplit_Bcr[nUdigitPos]);

                    strWaferID = strWaferID.Trim();

                    if (strWaferID.Contains(","))
                    {
                        string[] strSplit = strWaferID.Split(',');
                        strWaferID = strSplit[0];
                    }

                    //strWaferID = strSplit_Bcr[nLotPos];
                }
            }

            if (BankHost_main.strWork_LotDigit.Contains("."))
            {
                int st = bcr.Lot.Length - 5;
                int index = bcr.Lot.IndexOf('.', st);
                bcr.Lot = bcr.Lot.Substring(0, index);
            }

            if (BankHost_main.strWork_LotDigit.Contains("-"))
            {
                string strindex = BankHost_main.strWork_LotDigit.Replace("-", "");
                int st = bcr.Lot.Length;
                int index = st - bcr.Lot.IndexOf('-', 0);
                bcr.Lot = bcr.Lot.Substring(0, st - index);
            }

            nValWfrQty = AWork.nBcrcount;//BankHost_main.Host.Host_Get_BcrRead_Wfrcount(BankHost_main.strEqid, bcr.Lot);

            if (BankHost_main.strCustName.Contains("QUALCOMM_SPLIT") == false)
                if ((bcr.DieQty == "" || bcr.Lot == "") && (bcr.WfrQty == "" || bcr.Lot == ""))
                    return null;



            int nDieTTL = 0, nWfrTTL = 0;
            string strFileName = "", strFileName_Device = "";
            if (bcr.Device == "")
            {
                //디바이스 정보 없는 자재인 경우
                //bcr.Device = strSelDevice; //21.02.17
                string strFile = strExcutionPath + "\\Work\\" + strWorkFileName; //HY210315
                string strReadfile = strFile + "\\" + strSelDevice + "\\" + strSelDevice + ".txt";
                string str = Fnc_Get_Device(strReadfile, bcr.Lot);
                bcr.Device = str;

                nDieTTL = Fnc_GetTTL(strSelDevice, bcr.Lot, 0);
                nWfrTTL = Fnc_GetTTL(strSelDevice, bcr.Lot, 1);

                strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
                strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strSelDevice + "\\" + strSelDevice;
            }
            else
            {
                nDieTTL = Fnc_GetTTL(bcr.Device, bcr.Lot, 0);
                nWfrTTL = Fnc_GetTTL(bcr.Device, bcr.Lot, 1);

                strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
                strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + bcr.Device + "\\" + bcr.Device;
            }

            int nQty = Int32.Parse(bcr.DieQty == "" ? "0" : bcr.DieQty);

            bcr.DieTTL = nDieTTL.ToString();
            bcr.WfrTTL = nWfrTTL.ToString();

            string strSetID = strWaferID + "_" + bcr.DieQty;
            string strGet = BankHost_main.Host.Host_Set_BcrReadInfo(BankHost_main.strEqid, bcr.Device, (bcr.LPN == "" ? bcr.Lot : bcr.LPN), strSetID);
             
            if (strGet == "True")
            {
                bcr.result = "DUPLICATE";
            }
            else
            {
                if (Checkdev(BankHost_main.strDeviceNo) == true && bcr.WSN == "")
                {
                    bcr.result = "WSN ERROR";
                }
                else if (BankHost_main.strWork_Lotinfo == "")
                {
                    if ((Checkdev(bcr.Device) == true && bcr.WSN != ""))
                        bcr.result = "OK";
                    else if (Checkdev(bcr.Device) == false)
                        bcr.result = "OK";
                }
                else if (BankHost_main.strWork_Lotinfo != bcr.Lot)
                {
                    bcr.result = "MISSMATCH";
                }
                else
                {
                    if ((Checkdev(bcr.Device) == true && bcr.WSN != ""))
                        bcr.result = "OK";
                    else if (Checkdev(bcr.Device) == false)
                        bcr.result = "OK";
                }
            }

            bool isUnPrint = false;

            if (BankHost_main.Host.Host_Check_Unprinted_Device(bcr.Device) > 0)
                isUnPrint = true;


            if (isUnPrint == true)
                bcr.unprinted_device = true;

            string strlog = $"PARSING+{bcr.Device}+{bcr.Lot}+{bcr.DieQty}+{bcr.DieTTL}+{bcr.WfrTTL}+{bcr.WSN}+{bcr.result}+{BankHost_main.strOperator}";

            ////DB Save
            string[] strSaveInfo = new string[11];
            strSaveInfo[0] = BankHost_main.strEqid;
            strSaveInfo[1] = "VAL_READ_DATA";
            strSaveInfo[2] = "";
            strSaveInfo[3] = bcr.Device;
            strSaveInfo[4] = bcr.Lot;
            strSaveInfo[5] = bcr.DieQty;
            strSaveInfo[6] = bcr.DieTTL;
            strSaveInfo[7] = nValWfrQty.ToString();
            strSaveInfo[8] = bcr.WfrTTL;
            strSaveInfo[9] = bcr.WSN;
            strSaveInfo[10] = BankHost_main.strOperator;

            // Fnc_SaveLog_Work(strFileName, strlog, strSaveInfo, 0);
            Fnc_SaveLog_Work(strFileName_Device, strlog, strSaveInfo, 1);

            return bcr;
        }

        public string BarcodeRule2Str(string[] rule, string brc)
        {
            string res = "EMPTY";

            string s = rule[0];
            string r = rule[1];
            if (brc.Length > rule[0].Length)
            {
                if (brc.Substring(0, rule[0].Length) == rule[0])
                    return brc;
            }
            else
            {
                return res;
            }


            if (r.Substring(0, 1) == "C")
            {
                if (brc.Substring(0, s.Length) == s)
                    res = brc;
            }
            else if (r.Substring(0, 1) == "L")
            {
                int startIndex = int.Parse(r.Substring(1, r.Length - 1));

                res = brc.Substring(startIndex, brc.Length - startIndex);
            }
            else if (r.Substring(0, 1) == "R")
            {
                int endIndex = int.Parse(r.Substring(1, r.Length - 1));

                res = brc.Substring(0, endIndex);
            }

            return res;
        }

        public Bcrinfo Fnc_Bcr_Parsing_Fosb(string strBcr)
        {
            //nWorkBcrcount 확인 할 것, 고객별 바코드 형식도 확인이 필요할 듯!
            if (strBcr.Contains("LON") || strBcr.Contains("ERROR") || strBcr.Contains("BLOAD"))
                return null;

            ///BCR count check
            Bcrinfo bcr = new Bcrinfo();

            //Fosb , Device 0, Lot 1, DieQty 2, WfyQty 3 고정

            int nDevicePos = 0, nLotPos = 1, nDieQtyPos = 2, nWfrQtyPos = 3;

            if (BankHost_main.strWork_SPR == "SPACE")
                BankHost_main.strWork_SPR = " ";

            char seperator = char.Parse(BankHost_main.strWork_SPR);
            string[] strSplit_Bcr = strBcr.Split(seperator);
            int nLength = strSplit_Bcr.Length;

            if (nLength < 4)
            {
                if (strWorkCust == "736")
                {

                }
                else
                {
                    return null;
                }
            }


            bcr.Device = strSplit_Bcr[nDevicePos];
            //bcr.Lot = strSplit_Bcr[int.Parse(BankHost_main.strWork_LotidPos) == -1 ? 0 : int.Parse(BankHost_main.strWork_LotidPos)];
            bcr.Lot = strSplit_Bcr[nLotPos];
            bcr.DieQty = strSplit_Bcr[nDieQtyPos];
            bcr.WfrQty = strSplit_Bcr[nWfrQtyPos];

            nValWfrQty = BankHost_main.Host.Host_Get_BcrRead_Wfrcount(BankHost_main.strEqid, bcr.Lot);

            if (bcr.Lot == "")
                return null;

            int nDieTTL = 0, nWfrTTL = 0;
            string strFileName = "", strFileName_Device = "";

            string[] strSplit_DevicePos = new string[2];
            string[] strSplit_LotPos = new string[2];
            string[] strSplit_DieQtyPos = new string[2];
            string[] strSplit_WfrQtyPos = new string[2];

            if (BankHost_main.strWork_DevicePos.Contains(','))
            {
                strSplit_DevicePos = BankHost_main.strWork_DevicePos.Split(',');
            }
            else
                strSplit_DevicePos[1] = "";

            if (BankHost_main.strWork_LotidPos.Contains(','))
            {
                strSplit_LotPos = BankHost_main.strWork_LotidPos.Split(',');
            }
            else
                strSplit_LotPos[1] = "";

            if (BankHost_main.strWork_QtyPos.Contains(','))
            {
                strSplit_DieQtyPos = BankHost_main.strWork_QtyPos.Split(',');
            }
            else
                strSplit_DieQtyPos[1] = "";

            if (BankHost_main.strWork_WfrQtyPos.Contains(','))
            {
                strSplit_WfrQtyPos = BankHost_main.strWork_WfrQtyPos.Split(',');
            }
            else
                strSplit_WfrQtyPos[1] = "";

            if (strSplit_DevicePos[1] != "")
            {
                if (strSplit_DevicePos[1].Substring(0, 1) == "L")
                {
                    int n = Int32.Parse(strSplit_DevicePos[1].Substring(1, strSplit_DevicePos[1].Length - 1));
                    bcr.Device = bcr.Device.Substring(n, bcr.Device.Length - n);
                }
                else
                {
                    int n = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
                    bcr.Device = bcr.Device.Substring(0, bcr.Device.Length - n);
                }
            }

            if (BankHost_main.nMaterial_type == 1)
            {
                string str = bcr.Device;
                //Device rename 확인
                if (BankHost_main.Host.CheckDeviceRename(str) == "EXIST")
                {
                    bcr.Device = BankHost_main.Host.Get_Device_Rename(str);
                }
            }

            if (strSplit_LotPos[1] != "")
            {
                if (strSplit_LotPos[1].Substring(0, 1) == "L")
                {
                    int n = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                    bcr.Lot = bcr.Lot.Substring(n, bcr.Lot.Length - n);
                }
                else
                {
                    int n = Int32.Parse(strSplit_LotPos[1].Substring(1, 1));
                    bcr.Lot = bcr.Lot.Substring(0, bcr.Lot.Length - n);
                }
            }

            if (strSplit_DieQtyPos[1] != "")
            {
                if (strSplit_DieQtyPos[1].Substring(0, 1) == "L")
                {
                    int n = Int32.Parse(strSplit_DieQtyPos[1].Substring(1, 1));
                    bcr.DieQty = bcr.DieQty.Substring(n, bcr.DieQty.Length - n);
                }
                else
                {
                    int n = Int32.Parse(strSplit_DieQtyPos[1].Substring(1, 1));
                    bcr.DieQty = bcr.DieQty.Substring(0, bcr.DieQty.Length - n);
                }
            }

            if (strSplit_WfrQtyPos[1] != "")
            {
                if (strSplit_WfrQtyPos[1].Substring(0, 1) == "L")
                {
                    int n = Int32.Parse(strSplit_WfrQtyPos[1].Substring(1, 1));
                    bcr.WfrQty = bcr.WfrQty.Substring(n, bcr.WfrQty.Length - n);
                }
                else
                {
                    int n = Int32.Parse(strSplit_WfrQtyPos[1].Substring(1, 1));
                    bcr.WfrQty = bcr.WfrQty.Substring(0, bcr.WfrQty.Length - n);
                }
            }

            nDieTTL = Fnc_GetTTL(bcr.Device, bcr.Lot, 0);
            nWfrTTL = Fnc_GetTTL(bcr.Device, bcr.Lot, 1);

            if (bcr.WfrQty != "")
                BankHost_main.nWorkBcrcount = Int32.Parse(bcr.WfrQty);

            strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
            strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + bcr.Device + "\\" + bcr.Device;

            bcr.DieTTL = nDieTTL.ToString();
            bcr.WfrTTL = nWfrTTL.ToString();

            if (bcr.DieQty == "")
                bcr.DieQty = bcr.DieTTL;

            if (bcr.WfrQty == "")
                bcr.WfrQty = bcr.WfrTTL;

            string strSetID = "";

            if (BankHost_main.strLot2Wfr == "TRUE")
            {
                strSetID = bcr.Lot;
            }
            else
            {
                strSetID = bcr.Lot + "_" + bcr.DieQty;
            }

            string strGet = BankHost_main.Host.Host_Set_BcrReadInfo(BankHost_main.strEqid, bcr.Device, bcr.Lot, strSetID);

            if (strGet == "True")
            {
                bcr.result = "DUPLICATE";
            }
            else
            {
                if (BankHost_main.strWork_Lotinfo == "")
                {
                    bcr.result = "OK";
                }
                else if (BankHost_main.strWork_Lotinfo != bcr.Lot)
                {
                    bcr.result = "MISSMATCH";
                }
                else
                {
                    bcr.result = "OK";
                }
            }

            int nCheckUnprint = BankHost_main.Host.Host_Check_Unprinted_Device(bcr.Device);
            if (nCheckUnprint > 0)
                bcr.unprinted_device = true;

            string strlog = string.Format("PARSING+{0}+{1}+{2}+{3}+{4}+{5}+{6}", bcr.Device, bcr.Lot, bcr.DieQty, bcr.DieTTL, bcr.WfrTTL, bcr.result, BankHost_main.strOperator);

            ////DB Save
            string[] strSaveInfo = new string[10];
            strSaveInfo[0] = BankHost_main.strEqid;
            strSaveInfo[1] = "VAL_READ_DATA";
            strSaveInfo[2] = "";
            strSaveInfo[3] = bcr.Device;
            strSaveInfo[4] = bcr.Lot;
            strSaveInfo[5] = bcr.DieQty;
            strSaveInfo[6] = bcr.DieTTL;
            strSaveInfo[7] = nValWfrQty.ToString();
            strSaveInfo[8] = bcr.WfrTTL;
            strSaveInfo[9] = BankHost_main.strOperator;

            // Fnc_SaveLog_Work(strFileName, strlog, strSaveInfo, 0);
            Fnc_SaveLog_Work(strFileName_Device, strlog, strSaveInfo, 1);

            return bcr;
        }

        private void button_download_Click(object sender, EventArgs e)
        {
            string strFilename;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "저장 경로 설정";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.DefaultExt = ".xlsx";
            saveFileDialog.Filter = "Xlsx files(*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                strFilename = saveFileDialog.FileName;
                Fnc_ExcelCreate(strFilename);
            }
        }

        public void Fnc_ExcelCreate(string strFileName)
        {
            Frm_Process.Form_Show("\n\n다운로드를 시작 합니다.");
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            string strToday = string.Format("{0}{1:00}{2:00}_", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string strTime = string.Format("{0:00}{1:00}{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;

            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = strWorkFileName;

            int nGcount = dataGridView_sort.RowCount;
            int nCellcount = 0;

            xlWorkSheet.Cells[1, 1] = "#";
            xlWorkSheet.Cells[1, 2] = "BILL#";
            xlWorkSheet.Cells[1, 3] = "INVOICE#";
            xlWorkSheet.Cells[1, 4] = "LOT#";
            xlWorkSheet.Cells[1, 5] = "DEVICE";
            xlWorkSheet.Cells[1, 6] = "DIE TTL";
            xlWorkSheet.Cells[1, 7] = "DIE QTY";
            xlWorkSheet.Cells[1, 8] = "WFR QTY";
            xlWorkSheet.Cells[1, 9] = "PRICE";
            xlWorkSheet.Cells[1, 10] = "WFR SIZE";
            xlWorkSheet.Cells[1, 11] = "RCVD-DATE";
            xlWorkSheet.Cells[1, 12] = "STATE";
            xlWorkSheet.Cells[1, 13] = "작업자";

            for (int i = 0; i < nGcount; i++)
            {
                xlWorkSheet.Cells[2 + nCellcount, 1] = dataGridView_sort.Rows[i].Cells[0].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 2] = dataGridView_sort.Rows[i].Cells[1].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 3] = dataGridView_sort.Rows[i].Cells[2].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 4] = "'" + dataGridView_sort.Rows[i].Cells[3].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 5] = dataGridView_sort.Rows[i].Cells[4].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 6] = dataGridView_sort.Rows[i].Cells[5].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 7] = dataGridView_sort.Rows[i].Cells[6].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 8] = dataGridView_sort.Rows[i].Cells[7].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 9] = dataGridView_sort.Rows[i].Cells[8].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 10] = dataGridView_sort.Rows[i].Cells[9].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 11] = dataGridView_sort.Rows[i].Cells[10].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 12] = dataGridView_sort.Rows[i].Cells[11].Value.ToString();
                xlWorkSheet.Cells[2 + nCellcount, 13] = dataGridView_sort.Rows[i].Cells[12].Value.ToString();

                nCellcount++;

                string strMsg = string.Format("\n\n파일 쓰는 중 {0} / {1}", nCellcount, nGcount);
                Frm_Process.Form_Display(strMsg);

                System.Windows.Forms.Application.DoEvents();
            }

            xlWorkSheet.Columns.AutoFit();

            xlWorkBook.SaveAs(strFileName, Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            Frm_Process.Hide();
        }

        public void Fnc_ExcelCreate_Lotinfo(string strFileName, string strDevice)
        {
            Frm_Process.Form_Show("\n\n다운로드를 시작 합니다.");
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            string strToday = string.Format("{0}{1:00}{2:00}_", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string strTime = string.Format("{0:00}{1:00}{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;

            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = strWorkFileName;

            int nGcount = dataGridView_sort.RowCount;
            int nCellcount = 0;

            xlWorkSheet.Cells[1, 1] = "BILL#";
            xlWorkSheet.Cells[1, 2] = "INVOICE#";
            xlWorkSheet.Cells[1, 3] = "LOT#";
            xlWorkSheet.Cells[1, 4] = "DEVICE";
            xlWorkSheet.Cells[1, 5] = "DIE QTY";
            xlWorkSheet.Cells[1, 6] = "WFR QTY";
            xlWorkSheet.Cells[1, 7] = "PRICE";
            xlWorkSheet.Cells[1, 8] = "WFR SIZE";
            xlWorkSheet.Cells[1, 9] = "RCVD-DATE";

            for (int i = 0; i < nGcount; i++)
            {
                if (strDevice == dataGridView_sort.Rows[i].Cells[4].Value.ToString())
                {
                    xlWorkSheet.Cells[2 + nCellcount, 1] = dataGridView_sort.Rows[i].Cells[1].Value.ToString();
                    xlWorkSheet.Cells[2 + nCellcount, 2] = dataGridView_sort.Rows[i].Cells[2].Value.ToString();
                    xlWorkSheet.Cells[2 + nCellcount, 3] = "'" + dataGridView_sort.Rows[i].Cells[3].Value.ToString();
                    xlWorkSheet.Cells[2 + nCellcount, 4] = dataGridView_sort.Rows[i].Cells[4].Value.ToString();
                    xlWorkSheet.Cells[2 + nCellcount, 5] = dataGridView_sort.Rows[i].Cells[5].Value.ToString();
                    xlWorkSheet.Cells[2 + nCellcount, 6] = dataGridView_sort.Rows[i].Cells[7].Value.ToString();
                    xlWorkSheet.Cells[2 + nCellcount, 7] = dataGridView_sort.Rows[i].Cells[8].Value.ToString();
                    xlWorkSheet.Cells[2 + nCellcount, 8] = dataGridView_sort.Rows[i].Cells[9].Value.ToString();
                    xlWorkSheet.Cells[2 + nCellcount, 9] = dataGridView_sort.Rows[i].Cells[10].Value.ToString();

                    nCellcount++;
                }

                string strMsg = string.Format("\n\n파일 쓰는 중 {0}", nCellcount);
                Frm_Process.Form_Display(strMsg);

                System.Windows.Forms.Application.DoEvents();
            }

            xlWorkSheet.Columns.AutoFit();

            xlWorkBook.SaveAs(strFileName, Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            Frm_Process.Hide();
        }

        private void button_workend_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("작업 종료\n\n작업을 마치시겠습니까?", "Alart", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.Yes)
            {
                strWorkFileName = "";
                BankHost_main.strOperator = "";

                dataGridView_worklist.Columns.Clear();
                dataGridView_worklist.Rows.Clear();
                dataGridView_worklist.Refresh();

                label_opinfo.Text = "-";

                BankHost_main.Host.Host_Set_Ready(BankHost_main.strEqid, "WAIT", "");
                BankHost_main.nWorkMode = 0;
                BankHost_main.strWork_Lotinfo = "";

                label_info.Text = "";
                label_info.BackColor = Color.DarkGray;
                label_info.ForeColor = Color.White;

                tabControl_Sort.SelectedIndex = 0;

                stopLogOutTimer();
            }
            else
            {
                textBox_Readdata.Focus();
                return;
            }
        }

        private void button_autogr_Click(object sender, EventArgs e)
        {
            BankHost_main.IsGRrun = true;
            strSelBill = "";

            string strGrMethod = BankHost_main.strWork_Cust.Contains("QUALCOMM") == true ? "INTRANSIT" : "ADE";//BankHost_main.Host.Host_Get_GrMethod(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
            label_GRmethod.Text = strGrMethod;

            if (strGrMethod == "ADE")
            {
                dataGridView_shipment.Visible = false;
                button_Getlist.Visible = false;
            }
            else
            {
                dataGridView_shipment.Visible = true;
                button_Getlist.Visible = true;
            }

            stopLogOutTimer();
            tabControl_Sort.SelectedIndex = 1;
        }

        private void dataGridView_workbill_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bGRrun)
                return;

            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;

            if (colIndex != 0)
                colIndex = 0;

            if (rowIndex == -1)
                return;

            string strBill = dataGridView_workbill.Rows[rowIndex].Cells[1].Value.ToString();

            try
            {
                strSelBill = strBill;

                bGRrun = true;

                if (label_GRmethod.Text == "ADE")
                    Gr_GetInfo(strSelBill);
                else
                    Gr_GetInfo_Shipment(strSelBill);

                bGRrun = false;

            }
            catch
            {
                return;
            }
        }

        private void button_register_Click(object sender, EventArgs e)
        {
            if (textBox_unprinted_device.Text == "")
            {
                MessageBox.Show("디바이스 정보를 입력 하세요.");
                textBox_unprinted_device.Focus();
                return;
            }

            if (textBox_unpinrted_custcode.Text == "")
            {
                MessageBox.Show("고객 정보를 입력 하세요.");
                textBox_unpinrted_custcode.Focus();
                return;
            }

            textBox_unprinted_device.Text = textBox_unprinted_device.Text.Trim();
            textBox_unpinrted_custcode.Text = textBox_unpinrted_custcode.Text.Trim();

            string strJudge = BankHost_main.Host.Host_Set_Unprinted_Device(textBox_unprinted_device.Text, textBox_unpinrted_custcode.Text);

            if (strJudge == "OK")
            {
                ///DB Save
                string[] strSaveInfo = new string[10];
                strSaveInfo[0] = BankHost_main.strEqid;
                strSaveInfo[1] = "SAVE";
                strSaveInfo[2] = textBox_unprinted_device.Text;
                strSaveInfo[3] = textBox_unpinrted_custcode.Text;
                strSaveInfo[4] = BankHost_main.strOperator;

                if (BankHost_main.bHost_connect)
                    BankHost_main.Host.Host_Hist_Unprint(strSaveInfo);
                /////

                Fnc_Get_Unprinted_Deviceinfo();
            }
            else
                MessageBox.Show("저장 실패!");

            textBox_unprinted_device.Text = "";
        }

        public void Fnc_Get_Unprinted_Deviceinfo()
        {
            System.Data.DataTable dt = BankHost_main.Host.Host_Get_Unprinted_Device();

            dataGridView_unprintedinfo.Columns.Clear();
            dataGridView_unprintedinfo.Rows.Clear();
            dataGridView_unprintedinfo.Refresh();

            Thread.Sleep(300);

            dataGridView_unprintedinfo.DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 15);
            dataGridView_unprintedinfo.Columns.Add("ID", "ID");
            dataGridView_unprintedinfo.Columns.Add("Device", "Device");
            dataGridView_unprintedinfo.Columns.Add("Cust", "Cust");

            int nCount = dt.Rows.Count;
            for (int n = 0; n < nCount; n++)
            {
                string strDev = dt.Rows[n]["DEVICE"].ToString();
                string strCust = dt.Rows[n]["CUST_CODE"].ToString();

                dataGridView_unprintedinfo.Rows.Add(new object[3] { n + 1, strDev, strCust });
            }

            dataGridView_unprintedinfo.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_unprintedinfo.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_unprintedinfo.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            int nIndex = dataGridView_unprintedinfo.CurrentCell.RowIndex;

            if (nIndex < 0)
                return;

            DialogResult dialogResult1 = MessageBox.Show("삭제 하시 겠습니까?", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.No)
            {
                return;
            }

            string strDev = dataGridView_unprintedinfo.Rows[nIndex].Cells[1].Value.ToString();
            string strCust = dataGridView_unprintedinfo.Rows[nIndex].Cells[2].Value.ToString();

            string strJudge = BankHost_main.Host.Host_Delete_Unprinted_Device(strDev);
            if (strJudge == "OK")
            {
                ///DB Save
                string[] strSaveInfo = new string[10];
                strSaveInfo[0] = BankHost_main.strEqid;
                strSaveInfo[1] = "DELETE";
                strSaveInfo[2] = strDev;
                strSaveInfo[3] = strCust;
                strSaveInfo[4] = BankHost_main.strOperator;

                if (BankHost_main.bHost_connect)
                    BankHost_main.Host.Host_Hist_Unprint(strSaveInfo);

                Fnc_Get_Unprinted_Deviceinfo();
            }
            else
                MessageBox.Show("삭제 실패!");
        }

        private void ToolStripMenuItem_delete_Click(object sender, EventArgs e)
        {
            int nIndex = dataGridView_unprintedinfo.CurrentCell.RowIndex;

            if (nIndex < 0)
                return;

            string strDev = dataGridView_unprintedinfo.Rows[nIndex].Cells[1].Value.ToString();
            string strCust = dataGridView_unprintedinfo.Rows[nIndex].Cells[2].Value.ToString();

            string strJudge = BankHost_main.Host.Host_Delete_Unprinted_Device(strDev);
            if (strJudge == "OK")
            {
                ///DB Save
                string[] strSaveInfo = new string[10];
                strSaveInfo[0] = BankHost_main.strEqid;
                strSaveInfo[1] = "DELETE";
                strSaveInfo[2] = strDev;
                strSaveInfo[3] = strCust;
                strSaveInfo[4] = BankHost_main.strOperator;

                if (BankHost_main.bHost_connect)
                    BankHost_main.Host.Host_Hist_Unprint(strSaveInfo);

                Fnc_Get_Unprinted_Deviceinfo();
            }
            else
                MessageBox.Show("삭제 실패!");
        }

        private void button_Autofocus_Click(object sender, EventArgs e)
        {
            if (!BankHost_main.IsAutoFocus)
                BankHost_main.IsAutoFocus = true;
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            int nSel = comboBox_hist_device.SelectedIndex;

            if (nSel > 0)
            {
                dataGridView_hist.Columns.Clear();
                dataGridView_hist.Rows.Clear();
                dataGridView_hist.Refresh();

                Thread.Sleep(300);
            }

            if (nSel == 1) //시간별 조회
            {
                Fnc_Get_History();
            }
            else if (nSel == 2) //Bill# 기준
            {
                if (textBox_input.Text == "")
                {
                    MessageBox.Show("Bill# 를 입력 하세요!");
                    textBox_input.Focus();
                    return;
                }

                Fnc_Get_History_Bill(textBox_input.Text);
            }
            else if (nSel == 3) //Device 기준
            {
                if (textBox_input.Text == "")
                {
                    MessageBox.Show("디바이스를 입력 하세요!");
                    textBox_input.Focus();
                    return;
                }

                Fnc_Get_History_Device(textBox_input.Text);
            }
        }

        public void Fnc_Get_History()
        {
            string strTimeset_date_st = string.Format("{0}{1:00}{2:00}", dateTimePicker_st.Value.Year, dateTimePicker_st.Value.Month, dateTimePicker_st.Value.Day);
            string strTimeset_date_ed = string.Format("{0}{1:00}{2:00}", dateTimePicker_ed.Value.Year, dateTimePicker_ed.Value.Month, dateTimePicker_ed.Value.Day);

            string strTimeset_hour_st = comboBox_Hour_st.Text;
            string strTimeset_hour_ed = comboBox_Hour_ed.Text;
            string strTimeset_Min_st = comboBox_Min_st.Text;
            string strTimeset_Min_ed = comboBox_Min_ed.Text;

            string strDate_st = "", strDate_ed = "";

            strDate_st = strTimeset_date_st + strTimeset_hour_st + strTimeset_Min_st;
            strDate_ed = strTimeset_date_ed + strTimeset_hour_ed + strTimeset_Min_ed;

            var dt = BankHost_main.Host.Host_Get_Histinfo_Job(BankHost_main.strEqid, Double.Parse(strDate_st), Double.Parse(strDate_ed));

            dataGridView_hist.DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 13);
            dataGridView_hist.Columns.Add("NO", "NO");
            dataGridView_hist.Columns.Add("일자", "일자");
            dataGridView_hist.Columns.Add("시간", "시간");
            dataGridView_hist.Columns.Add("위치", "위치");
            dataGridView_hist.Columns.Add("작업", "작업");
            dataGridView_hist.Columns.Add("Bill#", "Bill#");
            dataGridView_hist.Columns.Add("디바이스", "디바이스");
            dataGridView_hist.Columns.Add("LOT", "LOT");
            dataGridView_hist.Columns.Add("Die 수량", "Die 수량");
            dataGridView_hist.Columns.Add("Die 합계", "Die 합계");
            dataGridView_hist.Columns.Add("Wfr 수량", "Wfr 수량");
            dataGridView_hist.Columns.Add("Wfr 합계", "Wfr 합계");
            dataGridView_hist.Columns.Add("작업자", "작업자");

            int nCount = dt.Rows.Count;
            for (int n = 0; n < nCount; n++)
            {
                string strDatetime = dt.Rows[n]["DATETIME"].ToString(); strDatetime = strDatetime.Trim();
                string strDate = strDatetime.Substring(0, 8);
                string strTime = strDatetime.Substring(8, 6);
                strTime = strTime.Substring(0, 2) + ":" + strTime.Substring(2, 2) + ":" + strTime.Substring(4, 2);

                string strLocation = dt.Rows[n]["LOCATION"].ToString(); strLocation = strLocation.Trim();
                string strWork = dt.Rows[n]["WORK_TYPE"].ToString(); strWork = strWork.Trim();
                string strBill = dt.Rows[n]["HAWB"].ToString(); strBill = strBill.Trim();
                string strDevice = dt.Rows[n]["DEVICE"].ToString(); strDevice = strDevice.Trim();
                string strLot = dt.Rows[n]["LOT"].ToString(); strLot = strLot.Trim();
                string strDieqty = dt.Rows[n]["DIE_QTY"].ToString(); strDieqty = strDieqty.Trim();
                string strDiettl = dt.Rows[n]["DIE_TTL"].ToString(); strDiettl = strDiettl.Trim();
                string strWfrqty = dt.Rows[n]["WFR_QTY"].ToString(); strWfrqty = strWfrqty.Trim();
                string strWfrttl = dt.Rows[n]["WFR_TTL"].ToString(); strWfrttl = strWfrttl.Trim();
                string strOp = dt.Rows[n]["OP_NAME"].ToString(); strOp = strOp.Trim();

                dataGridView_hist.Rows.Add(new object[13] { n + 1, strDate, strTime, strLocation, strWork, strBill,
                    strDevice, strLot, strDieqty, strDiettl, strWfrqty, strWfrttl, strOp });
            }
        }

        private void button_email_Click(object sender, EventArgs e)
        {
            int nIndex = dataGridView_workbill.CurrentCell.RowIndex;
            string strBill = dataGridView_workbill.Rows[nIndex].Cells[1].Value.ToString();

            int nLotCount = dataGridView_workinfo.Rows.Count;
            string strCust = dataGridView_workinfo.Rows[0].Cells[1].Value.ToString();
            string strDevice = dataGridView_workinfo.Rows[0].Cells[2].Value.ToString();

            int nCheckUnprint = BankHost_main.Host.Host_Check_Unprinted_Device(strDevice);
            if (nCheckUnprint == 0)
            {
                DialogResult dialogResult1 = MessageBox.Show("컴바인 자재가 아닙니다.\n\n그래도 이메일을 보내시겠습니까?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult1 == DialogResult.No)
                {
                    return;
                }
            }

            int nDieTotalQty = 0, nWaferTotalQty = 0;
            for (int n = 0; n < nLotCount; n++)
            {
                string strDieqty = dataGridView_workinfo.Rows[n].Cells[4].Value.ToString();
                string strWaferqty = dataGridView_workinfo.Rows[n].Cells[6].Value.ToString();

                nDieTotalQty = nDieTotalQty + Int32.Parse(strDieqty);
                nWaferTotalQty = nWaferTotalQty + Int32.Parse(strWaferqty);
            }

            string strBase = string.Format("안녕하세요.\n\n금일 반입된 하기 자재 확인 하시어 컴바인 요청 바랍니다.\n\n");
            string strHawb = string.Format("(1) HAWB#: {0}\n", strBill);
            string strCustNo = string.Format("(2) CUST: {0}\n", strCust);
            string strLots = string.Format("(3) Lots: {0} EA\n", nLotCount);
            string strTotalQty = string.Format("(4) Die Total Qty: {0} EA\n", nDieTotalQty);
            string strWaferTotalQty = string.Format("(5) Wafer Total Qty: {0} EA\n", nWaferTotalQty);
            string strBase2 = string.Format("\n감사합니다.\n");
            string strMsg = strBase + strHawb + strCustNo + strLots + strTotalQty + strWaferTotalQty + strBase2;

            string strSubject = string.Format("#{0} - 컴바인 요청", strCust);

            Form_Email Frm_Email = new Form_Email();

            Frm_Email.Fnc_Init(strSubject, strMsg);
            Frm_Email.ShowDialog();
        }

        private void dataGridView_shipment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int nCount = dataGridView_shipment.RowCount;

            if (nCount < 1)
                return;

            int nIndex = dataGridView_shipment.CurrentCell.RowIndex;
            if (dataGridView_shipment.Rows[nIndex].Cells[0].Value == null)
            {
                dataGridView_shipment.Rows[nIndex].Cells[0].Value = true;
            }
            else
            {
                string strSel = dataGridView_shipment.Rows[nIndex].Cells[0].Value.ToString();

                if (strSel == "False")
                    dataGridView_shipment.Rows[nIndex].Cells[0].Value = true;
                else
                    dataGridView_shipment.Rows[nIndex].Cells[0].Value = false;
            }

            dataGridView_shipment.ClearSelection();
        }

        public string MakeTOTLabelTemplete110X170_2()
        {
            string msg = ZPL_START +

            // 박스 그리기



            string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 103 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 176 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 249 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 322 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 395 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 468 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 541 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 614 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 687 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 760 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 833 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 906 + Properties.Settings.Default.SecondPrinterOffsetY) +


                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 979 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1052 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1125 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,60,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1198 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,60,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1256 + Properties.Settings.Default.SecondPrinterOffsetY) +

                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 103 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 178 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 253 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 328 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 403 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 478 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 553 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 628 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 703 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 803 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 849 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 922 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 995 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1068 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1141 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1214 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1330 + Properties.Settings.Default.SecondPrinterOffsetY) +

                // 세로줄 그리기
                //Lot <-> QTY 앞
                string.Format("^FO{0},{1}^GB2,1285,2^FS", 220 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +

                //QTY <-> QR 앞
                string.Format("^FO{0},{1}^GB2,1021,2^FS", 325 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +

                // 중간
                string.Format("^FO{0},{1}^GB2,1096,2^FS", 405 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB2,73,2^FS", 405 + Properties.Settings.Default.SecondPrinterOffsetX, 1200 + Properties.Settings.Default.SecondPrinterOffsetY) +


                //Lot <-> QTY 뒤
                string.Format("^FO{0},{1}^GB2,1096,2^FS", 610 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB2,73,2^FS", 610 + Properties.Settings.Default.SecondPrinterOffsetX, 1200 + Properties.Settings.Default.SecondPrinterOffsetY) +

                //QTY <-> QR 뒤
                string.Format("^FO{0},{1}^GB2,1021,2^FS", 710 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +


                //Header
                //Lot
                string.Format("^FO{0},{1}^AO,15,10^FDLOT#/DCC^FS", Properties.Settings.Default.SecondPrinterOffsetX + 30, 60 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //QTY
                string.Format("^FO{0},{1}^AO,15,10^FDDie^FS", Properties.Settings.Default.SecondPrinterOffsetX + 235, 40 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FDQ'TY^FS", Properties.Settings.Default.SecondPrinterOffsetX + 235, 70 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //QR
                string.Format("^FO{0},{1}^AO,15,10^FDQR^FS", Properties.Settings.Default.SecondPrinterOffsetX + 340, 40 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FDCODE^FS", Properties.Settings.Default.SecondPrinterOffsetX + 340, 70 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //Lot
                string.Format("^FO{0},{1}^AO,15,10^FDLOT#/DCC^FS", Properties.Settings.Default.SecondPrinterOffsetX + 420, 60 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //QTY
                string.Format("^FO{0},{1}^AO,15,10^FDDie^FS", Properties.Settings.Default.SecondPrinterOffsetX + 625, 40 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FDQ'TY^FS", Properties.Settings.Default.SecondPrinterOffsetX + 625, 70 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //QR
                string.Format("^FO{0},{1}^AO,15,10^FDQR^FS", Properties.Settings.Default.SecondPrinterOffsetX + 725, 40 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FDCODE^FS", Properties.Settings.Default.SecondPrinterOffsetX + 725, 70 + Properties.Settings.Default.SecondPrinterOffsetY) +



                //CUST
                string.Format("^FO{0},{1}^AO,15,10^FDCUST^FS", 45 + Properties.Settings.Default.SecondPrinterOffsetX, 1082 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FD{2}^FS", 235 + Properties.Settings.Default.SecondPrinterOffsetX, 1082 + Properties.Settings.Default.SecondPrinterOffsetY, BankHost_main.strCust) +

                //Device                     
                string.Format("^FO{0},{1}^AO,15,10^FDDEVICE^FS", 45 + Properties.Settings.Default.SecondPrinterOffsetX, 1155 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FD{2}^FS", 325 + Properties.Settings.Default.SecondPrinterOffsetX, 1155 + Properties.Settings.Default.SecondPrinterOffsetY, strValDevice) +

                //DATE                       
                string.Format("^FO{0},{1}^AO,15,10^FDRCV-DATE^FS", 45 + Properties.Settings.Default.SecondPrinterOffsetX, 1220 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FD{2}^FS", 235 + Properties.Settings.Default.SecondPrinterOffsetX, 1220 + Properties.Settings.Default.SecondPrinterOffsetY, DateTime.Now.ToString("yyyy-MM-dd")) +

                //DWafer QTY
                string.Format("^FO{0},{1}^AO,15,10^FDWAFER^FS", 420 + Properties.Settings.Default.SecondPrinterOffsetX, 1070 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FDQ'TY^FS", 420 + Properties.Settings.Default.SecondPrinterOffsetX, 1097 + Properties.Settings.Default.SecondPrinterOffsetY) +

                //Bill
                string.Format("^FO{0},{1}^AO,15,10^FDBILL^FS", 45 + Properties.Settings.Default.SecondPrinterOffsetX, 1281 + Properties.Settings.Default.SecondPrinterOffsetY);


            return msg;
        }


        public string MakeTOTLabelTemplete110X170_1()
        {
            string msg = ZPL_START +

            // 박스 그리기



            string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 103 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 176 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 249 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 322 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 395 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 468 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 541 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 614 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 687 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 760 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 833 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 906 + Properties.Settings.Default.SecondPrinterOffsetY) +


                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 979 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1052 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1125 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,60,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1198 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB770,60,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1256 + Properties.Settings.Default.SecondPrinterOffsetY) +

                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 103 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 178 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 253 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 328 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 403 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 478 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 553 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 628 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 703 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 803 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 849 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 922 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 995 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1068 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1141 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1214 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB770,75,2^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1330 + Properties.Settings.Default.SecondPrinterOffsetY) +

                // 세로줄 그리기
                //Lot <-> QTY 앞
                string.Format("^FO{0},{1}^GB2,1285,2^FS", 220 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +

                //QTY <-> QR 앞
                string.Format("^FO{0},{1}^GB2,1021,2^FS", 325 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +

                // 중간
                string.Format("^FO{0},{1}^GB2,1096,2^FS", 405 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB2,73,2^FS", 405 + Properties.Settings.Default.SecondPrinterOffsetX, 1200 + Properties.Settings.Default.SecondPrinterOffsetY) +


                //Lot <-> QTY 뒤
                string.Format("^FO{0},{1}^GB2,1096,2^FS", 610 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB2,73,2^FS", 610 + Properties.Settings.Default.SecondPrinterOffsetX, 1200 + Properties.Settings.Default.SecondPrinterOffsetY) +

                //QTY <-> QR 뒤
                string.Format("^FO{0},{1}^GB2,1021,2^FS", 710 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +


                //Header
                //Lot
                string.Format("^FO{0},{1}^AO,15,10^FDLOT#/DCC^FS", Properties.Settings.Default.SecondPrinterOffsetX + 30, 60 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //QTY
                string.Format("^FO{0},{1}^AO,15,10^FDDie^FS", Properties.Settings.Default.SecondPrinterOffsetX + 235, 40 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FDQ'TY^FS", Properties.Settings.Default.SecondPrinterOffsetX + 235, 70 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //QR
                string.Format("^FO{0},{1}^AO,15,10^FDQR^FS", Properties.Settings.Default.SecondPrinterOffsetX + 340, 40 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FDCODE^FS", Properties.Settings.Default.SecondPrinterOffsetX + 340, 70 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //Lot
                string.Format("^FO{0},{1}^AO,15,10^FDLOT#/DCC^FS", Properties.Settings.Default.SecondPrinterOffsetX + 420, 60 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //QTY
                string.Format("^FO{0},{1}^AO,15,10^FDDie^FS", Properties.Settings.Default.SecondPrinterOffsetX + 625, 40 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FDQ'TY^FS", Properties.Settings.Default.SecondPrinterOffsetX + 625, 70 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //QR
                string.Format("^FO{0},{1}^AO,15,10^FDQR^FS", Properties.Settings.Default.SecondPrinterOffsetX + 725, 40 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FDCODE^FS", Properties.Settings.Default.SecondPrinterOffsetX + 725, 70 + Properties.Settings.Default.SecondPrinterOffsetY) +



                //CUST
                string.Format("^FO{0},{1}^AO,15,10^FDCUST^FS", 45 + Properties.Settings.Default.SecondPrinterOffsetX, 1082 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FD{2}^FS", 235 + Properties.Settings.Default.SecondPrinterOffsetX, 1082 + Properties.Settings.Default.SecondPrinterOffsetY, BankHost_main.strCust) +

                //Device                     
                string.Format("^FO{0},{1}^AO,15,10^FDDEVICE^FS", 45 + Properties.Settings.Default.SecondPrinterOffsetX, 1155 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FD{2}^FS", 325 + Properties.Settings.Default.SecondPrinterOffsetX, 1155 + Properties.Settings.Default.SecondPrinterOffsetY, strValDevice) +

                //DATE                       
                string.Format("^FO{0},{1}^AO,15,10^FDRCV-DATE^FS", 45 + Properties.Settings.Default.SecondPrinterOffsetX, 1220 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FD{2}^FS", 235 + Properties.Settings.Default.SecondPrinterOffsetX, 1220 + Properties.Settings.Default.SecondPrinterOffsetY, DateTime.Now.ToString("yyyy-MM-dd")) +

                //DWafer QTY
                string.Format("^FO{0},{1}^AO,15,10^FDWAFER^FS", 420 + Properties.Settings.Default.SecondPrinterOffsetX, 1070 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,15,10^FDQ'TY^FS", 420 + Properties.Settings.Default.SecondPrinterOffsetX, 1097 + Properties.Settings.Default.SecondPrinterOffsetY) +

                //Bill
                string.Format("^FO{0},{1}^AO,15,10^FDBILL^FS", 45 + Properties.Settings.Default.SecondPrinterOffsetX, 1281 + Properties.Settings.Default.SecondPrinterOffsetY);


            return msg;
        }

        public string MakeTOTLabelTemplete130X200()
        {
            string msg = ZPL_START +

                // 박스 그리기
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 129 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 228 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 327 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 426 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 525 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 624 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 723 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 822 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 921 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1020 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1119 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1218 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1317 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^GB970,103,4^FS", 20 + Properties.Settings.Default.SecondPrinterOffsetX, 1416 + Properties.Settings.Default.SecondPrinterOffsetY) +


                // 세로줄 그리기
                string.Format("^FO{0},{1}^GB4,1593,4^FS", 553 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB4,99,4^FS", 403 + Properties.Settings.Default.SecondPrinterOffsetX, 1222 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB4,990,4^FS", 300 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB4,990,4^FS", 670 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB4,99,4^FS", 573 + Properties.Settings.Default.SecondPrinterOffsetX, 1222 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB4,1325,4^FS", 190 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +
                //string.Format("^FO{0},{1}^GB4,1093,4^FS", 573 + Properties.Settings.Default.SecondPrinterOffsetX, 30 + Properties.Settings.Default.SecondPrinterOffsetY) +

                //Header
                string.Format("^FO{0},{1}^AO,30,15^FDLOT#/DCC^FS", Properties.Settings.Default.SecondPrinterOffsetX + 30, 70 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FDDie^FS", Properties.Settings.Default.SecondPrinterOffsetX + 200, 60 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FDQ'TY^FS", Properties.Settings.Default.SecondPrinterOffsetX + 200, 90 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FDQR^FS", Properties.Settings.Default.SecondPrinterOffsetX + 310, 60 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FDCODE^FS", Properties.Settings.Default.SecondPrinterOffsetX + 310, 90 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FDLOT#/DCC^FS", Properties.Settings.Default.SecondPrinterOffsetX + 410, 70 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FDDie^FS", Properties.Settings.Default.SecondPrinterOffsetX + 590, 60 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FDQ'TY^FS", Properties.Settings.Default.SecondPrinterOffsetX + 590, 90 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FDQR^FS", Properties.Settings.Default.SecondPrinterOffsetX + 680, 60 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FDCODE^FS", Properties.Settings.Default.SecondPrinterOffsetX + 680, 90 + Properties.Settings.Default.SecondPrinterOffsetY) +

                //CUST
                string.Format("^FO{0},{1}^AO,30,15^FDCUST^FS", 30 + Properties.Settings.Default.SecondPrinterOffsetX, 1070 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FD{2}^FS", 200 + Properties.Settings.Default.SecondPrinterOffsetX, 1070 + Properties.Settings.Default.SecondPrinterOffsetY, BankHost_main.strCust) +

                //Device
                string.Format("^FO{0},{1}^AO,30,15^FDDEVICE^FS", 30 + Properties.Settings.Default.SecondPrinterOffsetX, 1170 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FD{2}^FS", 300 + Properties.Settings.Default.SecondPrinterOffsetX, 1170 + Properties.Settings.Default.SecondPrinterOffsetY, strValDevice) +

                //DATE
                string.Format("^FO{0},{1}^AO,30,15^FDRCV-^FS", 30 + Properties.Settings.Default.SecondPrinterOffsetX, 1240 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FDDATE^FS", 30 + Properties.Settings.Default.SecondPrinterOffsetX, 1270 + Properties.Settings.Default.SecondPrinterOffsetY) +
                string.Format("^FO{0},{1}^AO,30,15^FD{2}^FS", 200 + Properties.Settings.Default.SecondPrinterOffsetX, 1250 + Properties.Settings.Default.SecondPrinterOffsetY, DateTime.Now.ToString("yyyy-MM-dd")) +

            //DWafer QTY
            string.Format("^FO{0},{1}^AO,30,15^FDWAFER^FS", 410 + Properties.Settings.Default.SecondPrinterOffsetX, 1050 + Properties.Settings.Default.SecondPrinterOffsetY) +
            string.Format("^FO{0},{1}^AO,30,15^FDQ'TY^FS", 410 + Properties.Settings.Default.SecondPrinterOffsetX, 1080 + Properties.Settings.Default.SecondPrinterOffsetY);

            return msg;
        }



        public void Fnc_GetGrList()
        {
            int nCount = dataGridView_shipment.RowCount;

            List<StorageData> list = new List<StorageData>();

            string strFileName = "";

            int n = dataGridView_workbill.CurrentCell.RowIndex;

            if (n < 0)
            {
                string strMsg = string.Format("Bill이 선택 되지 않았습니다.\n\n먼저 Bill을 선택 하세요");
                Frm_Process.Form_Show(strMsg);
                Frm_Process.Form_Display_Warning(strMsg);
                Thread.Sleep(3000);
                Frm_Process.Form_Hide();
                return;
            }

            string strBill = dataGridView_workbill.Rows[n].Cells[1].Value.ToString();

            strFileName = BankHost_main.Host.Host_Get_JobfileName(BankHost_main.strEqid, strBill);
            if (strFileName != "")
                Fnc_WorkDownload(strFileName);

            for (int i = 0; i < nCount; i++)
            {
                if (dataGridView_shipment.Rows[i].Cells[0].Value == null)
                    dataGridView_shipment.Rows[i].Cells[0].Value = "False";

                string strSel = dataGridView_shipment.Rows[i].Cells[0].Value.ToString();
                string strShipment = dataGridView_shipment.Rows[i].Cells[1].Value.ToString();
                int nLotcount = dataGridView_sort.Rows.Count;

                if (strSel != "False")
                {
                    for (int j = 0; j < nLotcount; j++)
                    {
                        StorageData data = new StorageData();

                        data.Bill = dataGridView_sort.Rows[j].Cells[10].Value.ToString();
                        data.Cust = dataGridView_sort.Rows[j].Cells[1].Value.ToString();
                        data.Device = dataGridView_sort.Rows[j].Cells[2].Value.ToString();
                        data.Lot = dataGridView_sort.Rows[j].Cells[3].Value.ToString();
                        data.Die_Qty = dataGridView_sort.Rows[j].Cells[4].Value.ToString();
                        data.Default_WQty = dataGridView_sort.Rows[j].Cells[6].Value.ToString();
                        data.Rcv_WQty = dataGridView_sort.Rows[j].Cells[7].Value.ToString();
                        data.Amkorid = dataGridView_sort.Rows[j].Cells[11].Value.ToString();
                        data.state = dataGridView_sort.Rows[j].Cells[14].Value.ToString();
                        data.strGRstatus = dataGridView_sort.Rows[j].Cells[16].Value.ToString();
                        data.shipment = dataGridView_sort.Rows[j].Cells[17].Value.ToString();

                        if (data.Bill == strBill && data.shipment == strShipment)
                        {
                            list.Add(data);
                        }
                    }
                }
            }

            dataGridView_workinfo.Columns.Clear();
            dataGridView_workinfo.Rows.Clear();
            dataGridView_workinfo.Refresh();

            //dataGridView_workinfo.Columns.Add("#", "#");
            dataGridView_workinfo.Columns.Add("BILL#", "BILL#");
            dataGridView_workinfo.Columns.Add("CUST", "CUST");
            dataGridView_workinfo.Columns.Add("DEVICE", "DEVICE");
            dataGridView_workinfo.Columns.Add("LOT#", "LOT#");
            dataGridView_workinfo.Columns.Add("DIE_TTL", "DIE_TTL");
            dataGridView_workinfo.Columns.Add("WFR_QTY", "WFR_QTY");
            dataGridView_workinfo.Columns.Add("WFR_TTL", "WFR_TTL");
            dataGridView_workinfo.Columns.Add("AMKOR_ID", "AMKOR_ID");
            dataGridView_workinfo.Columns.Add("Validation", "Validation");
            dataGridView_workinfo.Columns.Add("GR처리", "GR처리");
            dataGridView_workinfo.Columns.Add("SHIPMENT", "SHIPMENT");

            dataGridView_workinfo.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_workinfo.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;

            int nListcount = list.Count;

            nCount = 0;
            int nWait = 0, nWork = 0, nComplete = 0, nError = 0, nGr = 0;

            for (int m = 0; m < nListcount; m++)
            {
                string strGetBill = list[m].Bill;
                string strGetCust = list[m].Cust;
                string strGetDevice = list[m].Device;
                string strGetLot = list[m].Lot;
                string strGetDiettl = list[m].Die_Qty;
                string strGetWfrttl = list[m].Default_WQty;
                string strGetWfrqty = list[m].Rcv_WQty;
                string strGetAmkorid = list[m].Amkorid;
                string strGetVali = list[m].state;
                string strGetGr = list[m].strGRstatus;
                string strGetShipment = list[m].shipment;

                dataGridView_workinfo.Rows.Add(new object[11] { strGetBill, strGetCust, strGetDevice, strGetLot, strGetDiettl,
                        strGetWfrqty, strGetWfrttl,strGetAmkorid, strGetVali,strGetGr, strGetShipment});

                if (strGetVali == "Waiting")
                {
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.BackColor = Color.LightGray;
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.ForeColor = Color.Black;

                    nWait++;
                }
                else if (strGetVali == "Working")
                {
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.BackColor = Color.LightGray;
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.ForeColor = Color.White;

                    nWork++;
                }
                else if (strGetVali == "Complete")
                {
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.BackColor = Color.Blue;
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.ForeColor = Color.White;

                    nComplete++;
                }
                else if (strGetVali == "Error")
                {
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.BackColor = Color.Red;
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.ForeColor = Color.White;

                    nError++;
                }

                if (strGetGr == "COMPLETE")
                {
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.BackColor = Color.DarkBlue;
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.ForeColor = Color.White;

                    nGr++;
                }
                else if (strGetGr == "ERROR")
                {
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.BackColor = Color.Red;
                    dataGridView_workinfo.Rows[m].DefaultCellStyle.ForeColor = Color.White;
                }
            }

            dataGridView_workinfo.Sort(this.dataGridView_workinfo.Columns["SHIPMENT"], ListSortDirection.Ascending);

            dataGridView_workinfo.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_workinfo.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_workinfo.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_workinfo.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_workinfo.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            //dataGridView_workinfo.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;

            label_error.Text = nError.ToString();
            label_wait.Text = nWait.ToString();
            label_work.Text = nWork.ToString();
            label_complete.Text = nComplete.ToString();
            label_gr.Text = nGr.ToString();
        }
        private void button_Getlist_Click(object sender, EventArgs e)
        {
            Fnc_GetGrList();
        }

        private void button_option_Click(object sender, EventArgs e)
        {
            Form_Option Frm_Option = new Form_Option();
            Frm_Option.Fnc_Init_image();
            Frm_Option.Fnc_Init_image2();

            Frm_Option.ShowDialog();
        }

        string input;
        int searched_row;

        private void button2_Click(object sender, EventArgs e)
        {
            int Realindex = -1;

            input = Microsoft.VisualBasic.Interaction.InputBox("무엇을 검색하시겠습니까?", "Search", "", -1, -1);

            if (input == "")
                return;

            searched_row = 0;

            for (int n = 0; n < dataGridView_Lot.RowCount; n++)
            {
                if (dataGridView_Lot.Rows[n].Cells[1].Value.ToString().IndexOf(input) != -1)
                {
                    dataGridView_Lot.Rows[n].Selected = true;
                    dataGridView_Lot.FirstDisplayedScrollingRowIndex = n;
                    dataGridView_Lot.CurrentCell = dataGridView_Lot.Rows[n].Cells[0];
                    searched_row = n;
                    break;
                }




                if (dataGridView_Lot.Rows[n].Cells[3].Value.ToString().Contains(input) == true)
                {
                    dataGridView_Lot.Rows[n].Selected = true;
                    dataGridView_Lot.FirstDisplayedScrollingRowIndex = n;
                    dataGridView_Lot.CurrentCell = dataGridView_Lot.Rows[n].Cells[0];
                    searched_row = n;
                    break;
                }

                if (n == dataGridView_Lot.RowCount - 1)
                    MessageBox.Show("지정된 문자열을 찾을 수 없습니다.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int n = 0; n < dataGridView_Lot.RowCount; n++)
            {
                if (dataGridView_Lot.Rows[n].Cells[1].Value.ToString().Contains(input) == true)
                {
                    if (searched_row < n)
                    {
                        dataGridView_Lot.Rows[n].Selected = true;
                        dataGridView_Lot.FirstDisplayedScrollingRowIndex = n;
                        dataGridView_Lot.CurrentCell = dataGridView_Lot.Rows[n].Cells[0];
                        searched_row = n;
                        break;
                    }
                }


                if (dataGridView_Lot.Rows[n].Cells[3].Value.ToString().Contains(input) == true)
                {
                    if (searched_row < n)
                    {
                        dataGridView_Lot.Rows[n].Selected = true;
                        dataGridView_Lot.FirstDisplayedScrollingRowIndex = n;
                        dataGridView_Lot.CurrentCell = dataGridView_Lot.Rows[n].Cells[0];
                        searched_row = n;
                        break;
                    }
                }

                if (n == dataGridView_Lot.RowCount - 1)
                {

                    MessageBox.Show("지정된 문자열을 찾을 수 없습니다.");

                }
            }
        }

        int device_row_num = 0;
        int lot_row_num = 0;

        private void label_scan_wait_Click(object sender, EventArgs e)
        {
            int lot_row = -1;

            for (int i = device_row_num; i < dataGridView_Device.RowCount; i++)
            {
                lot_row = get_wait_position(dataGridView_Device.Rows[i].Cells[1].Value.ToString(), lot_row_num);

                if (lot_row > -1)
                {
                    device_row_num = i;
                    lot_row_num = lot_row + 1;

                    dataGridView_Device_CellClick(i, 0);

                    dataGridView_Lot.Rows[lot_row_num].Selected = true;
                    dataGridView_Lot.FirstDisplayedScrollingRowIndex = lot_row_num;
                }
                else
                {
                    device_row_num = 0;
                    lot_row_num = 0;
                }
            }
        }

        private int get_wait_position(string dev_name, int start_lot)
        {
            string res = "";

            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
            string strReadfile = "";

            if (strSelCust != "940")
            {
                strSelDevice = dev_name;
                strReadfile = strFileName + "\\" + dev_name + "\\" + dev_name + ".txt";
            }
            else
                strReadfile = strFileName + "\\" + dev_name + "\\" + dev_name + ".txt";

            string[] info = Fnc_ReadFile(strReadfile);
            string state = "";

            if (info == null)
                return -1;

            for (int m = start_lot + 1; m < info.Length; m++)
            {
                string[] strSplit_data = info[m].Split('\t');

                state = strSplit_data[13];

                if (state == "Waiting")
                {
                    return m;
                }
            }

            return -1;
        }

        private void dataGridView_Device_CellClick(int r, int c)
        {
            int rowIndex = r;
            int colIndex = c;

            if (colIndex != 0)
                colIndex = 0;

            if (rowIndex == -1)
                return;

            string strDevice = dataGridView_Device.Rows[rowIndex].Cells[1].Value.ToString();

            while (bGridViewUpdate)
            {
                Thread.Sleep(1);
                System.Windows.Forms.Application.DoEvents();
            }

            try
            {
                if (strSelCust == "940")
                {
                    strSelDevice = strDevice;
                }

                Fnc_GetDeviceData(strDevice);

            }
            catch
            {
                return;
            }
        }

        int clicked_label_row = -1;

        private void button_printbill_Click(object sender, EventArgs e)
        {
            Frm_Print.Fnc_Print_Billinfo(strSelBill);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClickTime();
            if (!BankHost_main.IsAutoFocus)
                BankHost_main.IsAutoFocus = true;
        }



        public void Fnc_Get_History_Bill(string strGetBill)
        {
            var dt = BankHost_main.Host.Host_Get_Histinfo_Job_Bill(strGetBill);

            dataGridView_hist.DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 13);
            dataGridView_hist.Columns.Add("일자", "일자");
            dataGridView_hist.Columns.Add("위치", "위치");
            dataGridView_hist.Columns.Add("작업", "작업");
            dataGridView_hist.Columns.Add("Bill#", "Bill#");
            dataGridView_hist.Columns.Add("디바이스", "디바이스");
            dataGridView_hist.Columns.Add("LOT", "LOT");
            dataGridView_hist.Columns.Add("Die 수량", "Die 수량");
            dataGridView_hist.Columns.Add("Die 합계", "Die 합계");
            dataGridView_hist.Columns.Add("Wfr 수량", "Wfr 수량");
            dataGridView_hist.Columns.Add("Wfr 합계", "Wfr 합계");
            dataGridView_hist.Columns.Add("작업자", "작업자");

            int nCount = dt.Rows.Count;
            for (int n = 0; n < nCount; n++)
            {
                string strDatetime = dt.Rows[n]["DATETIME"].ToString(); strDatetime = strDatetime.Trim();
                string strDate = strDatetime.Substring(0, 8);
                string strTime = strDatetime.Substring(8, 6);
                strTime = strTime.Substring(0, 2) + ":" + strTime.Substring(2, 2) + ":" + strTime.Substring(4, 2);

                string strLocation = dt.Rows[n]["LOCATION"].ToString(); strLocation = strLocation.Trim();
                string strWork = dt.Rows[n]["WORK_TYPE"].ToString(); strWork = strWork.Trim();
                string strBill = dt.Rows[n]["HAWB"].ToString(); strBill = strBill.Trim();
                string strDevice = dt.Rows[n]["DEVICE"].ToString(); strDevice = strDevice.Trim();
                string strLot = dt.Rows[n]["LOT"].ToString(); strLot = strLot.Trim();
                string strDieqty = dt.Rows[n]["DIE_QTY"].ToString(); strDieqty = strDieqty.Trim();
                string strDiettl = dt.Rows[n]["DIE_TTL"].ToString(); strDiettl = strDiettl.Trim();
                string strWfrqty = dt.Rows[n]["WFR_QTY"].ToString(); strWfrqty = strWfrqty.Trim();
                string strWfrttl = dt.Rows[n]["WFR_TTL"].ToString(); strWfrttl = strWfrttl.Trim();
                string strOp = dt.Rows[n]["OP_NAME"].ToString(); strOp = strOp.Trim();

                strDate = strDate + " " + strTime;

                dataGridView_hist.Rows.Add(new object[11] {strDate, strLocation, strWork, strBill,
                    strDevice, strLot, strDieqty, strDiettl, strWfrqty, strWfrttl, strOp });
            }

            dataGridView_hist.Sort(dataGridView_hist.Columns["일자"], ListSortDirection.Ascending);
        }

        public void Fnc_Get_History_Device(string strGetDevice)
        {
            var dt = BankHost_main.Host.Host_Get_Histinfo_Job_Device(strGetDevice);

            dataGridView_hist.DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 13);
            dataGridView_hist.Columns.Add("일자", "일자");
            dataGridView_hist.Columns.Add("위치", "위치");
            dataGridView_hist.Columns.Add("작업", "작업");
            dataGridView_hist.Columns.Add("Bill#", "Bill#");
            dataGridView_hist.Columns.Add("디바이스", "디바이스");
            dataGridView_hist.Columns.Add("LOT", "LOT");
            dataGridView_hist.Columns.Add("Die 수량", "Die 수량");
            dataGridView_hist.Columns.Add("Die 합계", "Die 합계");
            dataGridView_hist.Columns.Add("Wfr 수량", "Wfr 수량");
            dataGridView_hist.Columns.Add("Wfr 합계", "Wfr 합계");
            dataGridView_hist.Columns.Add("작업자", "작업자");

            int nCount = dt.Rows.Count;
            for (int n = 0; n < nCount; n++)
            {
                string strDatetime = dt.Rows[n]["DATETIME"].ToString(); strDatetime = strDatetime.Trim();
                string strDate = strDatetime.Substring(0, 8);
                string strTime = strDatetime.Substring(8, 6);
                strTime = strTime.Substring(0, 2) + ":" + strTime.Substring(2, 2) + ":" + strTime.Substring(4, 2);

                string strLocation = dt.Rows[n]["LOCATION"].ToString(); strLocation = strLocation.Trim();
                string strWork = dt.Rows[n]["WORK_TYPE"].ToString(); strWork = strWork.Trim();
                string strBill = dt.Rows[n]["HAWB"].ToString(); strBill = strBill.Trim();
                string strDevice = dt.Rows[n]["DEVICE"].ToString(); strDevice = strDevice.Trim();
                string strLot = dt.Rows[n]["LOT"].ToString(); strLot = strLot.Trim();
                string strDieqty = dt.Rows[n]["DIE_QTY"].ToString(); strDieqty = strDieqty.Trim();
                string strDiettl = dt.Rows[n]["DIE_TTL"].ToString(); strDiettl = strDiettl.Trim();
                string strWfrqty = dt.Rows[n]["WFR_QTY"].ToString(); strWfrqty = strWfrqty.Trim();
                string strWfrttl = dt.Rows[n]["WFR_TTL"].ToString(); strWfrttl = strWfrttl.Trim();
                string strOp = dt.Rows[n]["OP_NAME"].ToString(); strOp = strOp.Trim();

                strDate = strDate + " " + strTime;
                dataGridView_hist.Rows.Add(new object[11] {strDate, strLocation, strWork, strBill,
                    strDevice, strLot, strDieqty, strDiettl, strWfrqty, strWfrttl, strOp });
            }

            dataGridView_hist.Sort(dataGridView_hist.Columns["일자"], ListSortDirection.Ascending);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (blabel_save == true)
            {
                bTimeOutSt = true;
                bselected_mode_index = false;
                tabControl_Sort.SelectedIndex = 0;
                blabel_save = false;

                dataGridView_label.Rows.Clear();
            }
            else
            {
                DialogResult res = MessageBox.Show("저장 하지 않았습니다. 종료 하시겠습니까?", "종료", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    bTimeOutSt = true;
                    bselected_mode_index = false;
                    tabControl_Sort.SelectedIndex = 0;
                    blabel_save = false;
                    dataGridView_label.Rows.Clear();
                }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string file_path = "";
            saveFileDialog1.InitialDirectory = Properties.Settings.Default.file_save_path;
            saveFileDialog1.Filter = "CSV file(*.csv)|";

            ClickTime();
            DialogResult res = saveFileDialog1.ShowDialog();

            if (res == DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.Length - 3, 3).ToUpper() != "CSV")
                {
                    file_path = saveFileDialog1.FileName + ".csv";
                }
                else
                {
                    file_path = saveFileDialog1.FileName;
                }

                Properties.Settings.Default.file_save_path = file_path;
                Properties.Settings.Default.Save();

                make_csv(file_path);
            }

            MessageBox.Show("Excel Export 완료 되었습니다.");

        }

        bool blabel_save = false;

        public void make_csv(string path)
        {
            try
            {
                string str_temp = "No.,LOT,DCC,Device,Lot_QTY,Wafer_QTY,Amkor_ID,Cust,Wafer_Lot";
                System.IO.StreamWriter st = System.IO.File.AppendText(path);

                st.WriteLine(str_temp);

                for (int i = 0; i < dataGridView_label.RowCount; i++)
                {
                    str_temp = dataGridView_label.Rows[i].Cells[0].Value.ToString() + ",";
                    str_temp += dataGridView_label.Rows[i].Cells[1].Value.ToString() + ",";
                    str_temp += dataGridView_label.Rows[i].Cells[2].Value.ToString() + ",";
                    str_temp += dataGridView_label.Rows[i].Cells[3].Value.ToString() + ",";
                    str_temp += dataGridView_label.Rows[i].Cells[4].Value.ToString() + ",";
                    str_temp += dataGridView_label.Rows[i].Cells[5].Value.ToString() + ",";
                    str_temp += dataGridView_label.Rows[i].Cells[6].Value.ToString() + ",";
                    str_temp += dataGridView_label.Rows[i].Cells[7].Value.ToString();

                    st.WriteLine(str_temp);
                    Thread.Sleep(10);
                }


                st.Write(string.Format("Lot Qty : ,{0},Die Qty : ,{1},Wfr QTY :,{2}", tot_lots, tot_die, tot_wfr));
                st.Close();
                st.Dispose();
                blabel_save = true;
            }
            catch (Exception ex)
            {

            }
        }

        public void make_loc_csv(string path)
        {
            try
            {
                string str_temp = "Plant,Cust,Loc,Hawb#,Invoice#,Device,Cust Lost#,DCC,Die Qty,Wfr Qty,Rcv Date";
                System.IO.StreamWriter st = System.IO.File.AppendText(path);

                st.WriteLine(str_temp);

                for (int i = 0; i < dgv_loc.RowCount; i++)
                {
                    str_temp = (i + 1).ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[0].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[1].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[2].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[3].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[4].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[5].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[6].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[7].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[8].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[9].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[10].Value.ToString() + ",";
                    str_temp += dgv_loc.Rows[i].Cells[11].Value.ToString();

                    st.WriteLine(str_temp);
                }

                st.Close();
                st.Dispose();
                blabel_save = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView_label_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox1.Text = e.KeyChar.ToString();
            textBox1.Select(textBox1.TextLength, 0);
            textBox1.Focus();
        }

        string bShipment = "";
        string nShipment = "";

        private void button_grstart_Click(object sender, EventArgs e)
        {
            if (bGRrun)
                return;

            int nWait = 0, nWork = 0, nComplete = 0, nError = 0;
            string strSpeak = "";

            int nLotCount = dataGridView_workinfo.Rows.Count;

            if (nLotCount < 1)
                return;

            nWait = Int32.Parse(label_wait.Text);
            nWork = Int32.Parse(label_work.Text);
            nComplete = Int32.Parse(label_complete.Text);
            nError = Int32.Parse(label_error.Text);

            string strGrMethod = BankHost_main.strWork_Cust.Contains("Qualcomm") == true ? "INTRANSIT" : "ADE"; //BankHost_main.Host.Host_Get_GrMethod(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);


            string strMsg = "";

            if (nWait > 0 || nWork > 0 || nError > 0)
            {
                if (strGrMethod == "ADE")
                {
                    //Shipment check
                    DialogResult dialogResult1 = MessageBox.Show("완료 되지 않은 자재가 있습니다.. \n\n그래도 GR 처리 하시겠습니까? (완료 된 자재만 GR처리 됩니다.)", "Warning", MessageBoxButtons.YesNo);
                    if (dialogResult1 == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    strMsg = string.Format("GR 처리 불가 합니다.\n\n자재가 Validation 완료 되어야 합니다.");
                    Frm_Process.Form_Show(strMsg);
                    Frm_Process.Form_Display_Warning(strMsg);
                    Thread.Sleep(2000);
                    Frm_Process.Form_Hide();

                    return;
                }
            }

            int nGrcount = 0;

            bGRrun = true;
            strMsg = string.Format("\n\nGR 처리를 시작 합니다.");
            Frm_Process.Form_Show(strMsg);

            strSpeak = string.Format("지알 시작");
            speech.SpeakAsync(strSpeak);

            int nGRNG = 0;

            bool isPopUp = false;

            for (int n = 0; n < nLotCount; n++)
            {
                bool bcheck = false;
                // Qualcomm일 때 미완료 Reel이 있을 경우 

                List<DataGridViewRow> rows = dataGridView_workinfo.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Validation"].Value.ToString().ToUpper() != "COMPLETE" && r.Cells["Shipment"].Value.ToString() == dataGridView_workinfo.Rows[n].Cells["Shipment"].Value.ToString()).ToList();
                List<DataGridViewRow> CompRows = dataGridView_workinfo.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Validation"].Value.ToString().ToUpper() == "COMPLETE" && r.Cells["Shipment"].Value.ToString() == dataGridView_workinfo.Rows[n].Cells["Shipment"].Value.ToString()).ToList();


                if (BankHost_main.strCustName.ToUpper().Contains("QUALCOMM") == true)
                {
                    if (rows.Count != 0)
                    {
                        if (bShipment == "" || bShipment != dataGridView_workinfo.Rows[n].Cells["Shipment"].Value.ToString())
                        {
                            isPopUp = false;
                            bShipment = dataGridView_workinfo.Rows[n].Cells["Shipment"].Value.ToString();
                        }

                        if (isPopUp == false && rows.Count > 0 && CompRows.Count > 0)
                        {
                            //MessageBox.Show($"Qualcomm : 미완료 된 Reel 있음!!!\nShipment : {bShipment}", "미완료!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isPopUp = true;
                        }
                    }
                    else
                    {
                        isPopUp = false;

                        string strDevice = dataGridView_workinfo.Rows[n].Cells[2].Value.ToString();
                        string strLot = dataGridView_workinfo.Rows[n].Cells[3].Value.ToString();
                        string strDieqty = dataGridView_workinfo.Rows[n].Cells[4].Value.ToString();
                        string strWfrqty = dataGridView_workinfo.Rows[n].Cells[5].Value.ToString();
                        string strWfrttl = dataGridView_workinfo.Rows[n].Cells[6].Value.ToString();
                        string strAmkorid = dataGridView_workinfo.Rows[n].Cells[7].Value.ToString();
                        string strVal = dataGridView_workinfo.Rows[n].Cells[8].Value.ToString();
                        string strGr = dataGridView_workinfo.Rows[n].Cells[9].Value.ToString();
                        string strReelID = dataGridView_workinfo.Rows[n].Cells[11].Value.ToString();
                        string strReelDCC = dataGridView_workinfo.Rows[n].Cells[12].Value.ToString();

                        strVal = strVal.ToUpper();

                        if (Int32.Parse(strWfrqty) != Int32.Parse(strWfrttl))
                        {
                            if (strVal == "COMPLETE" && strGr != "COMPLETE")
                            {
                                bcheck = true;
                                DialogResult dialogResult1 = MessageBox.Show("워이퍼 수량이 전산 데이터와 실제 수량이 상이 합니다.\n\n계속 진행 하시겠습니까?", "Warning", MessageBoxButtons.YesNo);
                                if (dialogResult1 == DialogResult.Yes)
                                {
                                    bcheck = false;
                                }
                            }
                        }

                        if (!bcheck)
                        {
                            bool bJudge = true;

                            if (strVal == "COMPLETE" && strGr != "COMPLETE")
                            {
                                nGrcount++;
                                strMsg = string.Format("\n\nGR 진행 중. 현재 Lot:{0}\nGR 처리 수량:{1}", strLot, nGrcount);
                                Frm_Process.Form_Display(strMsg);

                                bJudge = Gr_Process_Direct(strDevice, strLot, strAmkorid, strDieqty, strWfrqty, strReelID, strReelDCC);

                                if (!bJudge)
                                {
                                    strSpeak = string.Format("지알 실패!");
                                    speech.SpeakAsync(strSpeak);

                                    strMsg = string.Format("GR 처리 실패 Lot:{0}", strLot);
                                    Frm_Process.Form_Display_Warning(strMsg);

                                    nGRNG++;
                                }
                            }
                        }
                    }
                    Thread.Sleep(30);
                }
                else
                {
                    isPopUp = false;

                    string strDevice = dataGridView_workinfo.Rows[n].Cells[2].Value.ToString();
                    string strLot = dataGridView_workinfo.Rows[n].Cells[3].Value.ToString();
                    string strDieqty = dataGridView_workinfo.Rows[n].Cells[4].Value.ToString();
                    string strWfrqty = dataGridView_workinfo.Rows[n].Cells[5].Value.ToString();
                    string strWfrttl = dataGridView_workinfo.Rows[n].Cells[6].Value.ToString();
                    string strAmkorid = dataGridView_workinfo.Rows[n].Cells[7].Value.ToString();
                    string strVal = dataGridView_workinfo.Rows[n].Cells[8].Value.ToString();
                    string strGr = dataGridView_workinfo.Rows[n].Cells[9].Value.ToString();
                    string strReelID = dataGridView_workinfo.Rows[n].Cells[11].Value.ToString();
                    string strReelDCC = dataGridView_workinfo.Rows[n].Cells[12].Value.ToString();

                    strVal = strVal.ToUpper();

                    if (Int32.Parse(strWfrqty) != Int32.Parse(strWfrttl))
                    {
                        if (strVal == "COMPLETE" && strGr != "COMPLETE")
                        {
                            bcheck = true;
                            DialogResult dialogResult1 = MessageBox.Show("워이퍼 수량이 전산 데이터와 실제 수량이 상이 합니다.\n\n계속 진행 하시겠습니까?", "Warning", MessageBoxButtons.YesNo);
                            if (dialogResult1 == DialogResult.Yes)
                            {
                                bcheck = false;
                            }
                        }
                    }

                    if (!bcheck)
                    {
                        bool bJudge = true;

                        if (strVal == "COMPLETE" && strGr != "COMPLETE")
                        {
                            nGrcount++;
                            strMsg = string.Format("\n\nGR 진행 중. 현재 Lot:{0}\nGR 처리 수량:{1}", strLot, nGrcount);
                            Frm_Process.Form_Display(strMsg);

                            bJudge = Gr_Process_Direct(strDevice, strLot, strAmkorid, strDieqty, strWfrqty, strReelID, strReelDCC);

                            if (!bJudge)
                            {
                                strSpeak = string.Format("지알 실패!");
                                speech.SpeakAsync(strSpeak);

                                strMsg = string.Format("GR 처리 실패 Lot:{0}", strLot);
                                Frm_Process.Form_Display_Warning(strMsg);

                                nGRNG++;
                            }
                        }
                    }
                }
                Thread.Sleep(30);
            }

            strMsg = string.Format("\n\nGR 진행 Lot 수량: OK - {0}, NG - {1}", nGrcount - nGRNG, nGRNG);

            if (nGRNG > 0)
                Frm_Process.Form_Display_Warning(strMsg);
            else
                Frm_Process.Form_Display(strMsg);

            strSpeak = string.Format("작업을 마침니다.");
            speech.SpeakAsync(strSpeak);

            Thread.Sleep(3000);

            Gr_GetInfo(strSelBill);

            Frm_Process.Form_Display("\n작업을 마침니다.");
            Frm_Process.Hide();

            bGRrun = false;

            if (label_complete.Text == label_gr.Text)
                tabControl_Sort.SelectedIndex = 2;
        }

        private void btn_output_Click(object sender, EventArgs e)
        {
            string file_path = "";
            saveFileDialog1.InitialDirectory = Properties.Settings.Default.Loc_file_save_path;
            saveFileDialog1.Filter = "CSV file(*.csv)|";

            DialogResult res = saveFileDialog1.ShowDialog();

            if (res == DialogResult.OK)
            {


                if (saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.Length - 3, 3).ToUpper() != "CSV")
                {
                    file_path = saveFileDialog1.FileName + ".csv";
                }
                else
                {
                    file_path = saveFileDialog1.FileName;
                }

                Properties.Settings.Default.Loc_file_save_path = file_path;
                Properties.Settings.Default.Save();

                make_loc_csv(file_path);
            }
        }

        public int Fnc_GetDeviceData(string strDevice)
        {
            try
            {
                while (bGridViewUpdate)
                {
                    Thread.Sleep(1);
                }

                string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
                string strReadfile = "";

                if (strSelCust != "940")
                {
                    strSelDevice = strDevice;
                    strReadfile = strFileName + "\\" + strDevice + "\\" + strDevice + ".txt";
                }
                else
                    strReadfile = strFileName + "\\" + strSelDevice + "\\" + strSelDevice + ".txt";

                string[] info = Fnc_ReadFile(strReadfile);

                if (info == null)
                    return -1;

                bGridViewUpdate = true;

                dataGridView_Lot.Columns.Clear();
                dataGridView_Lot.Rows.Clear();
                dataGridView_Lot.Refresh();

                Thread.Sleep(300);

                dataGridView_Lot.Columns.Add("#", "#");
                dataGridView_Lot.Columns.Add("LOT#", "Lot#");
                dataGridView_Lot.Columns.Add("DCC", "DCC");
                dataGridView_Lot.Columns.Add("Die TTL", "Die TTL");
                dataGridView_Lot.Columns.Add("Die Qty", "Die Qty");
                dataGridView_Lot.Columns.Add("Wfr TTL", "Wfr TTL");
                dataGridView_Lot.Columns.Add("Wfr Qty", "Wfr Qty");
                dataGridView_Lot.Columns.Add("State", "State");
                dataGridView_Lot.Columns.Add("작업자", "작업자");
                dataGridView_Lot.Columns.Add("Bill#", "Bill#");
                dataGridView_Lot.Columns.Add("GR처리", "GR처리");
                dataGridView_Lot.Columns.Add("Shipment", "Shipment");
                dataGridView_Lot.Columns.Add("AmkorID", "AmkorID");
                dataGridView_Lot.Columns.Add("WSN", "WSN");
                dataGridView_Lot.Columns.Add("ReelID", "Reel ID");
                dataGridView_Lot.Columns.Add("ReelDCC", "Reel DCC");


                dataGridView_Lot.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[1].SortMode = DataGridViewColumnSortMode.Programmatic;
                dataGridView_Lot.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[12].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[13].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[14].SortMode = DataGridViewColumnSortMode.NotSortable;

                //dataGridView_Lot.Columns["WSN"].Visible = false;

                StorageData st;



                int nWaitcount = 0, nWorkcount = 0, nCompletecount = 0, nErrorcount = 0;

                for (int m = 0; m < info.Length; m++)
                {
                    st = new StorageData();
                    string[] strSplit_data = info[m].Split('\t');

                    st.Cust = strSplit_data[0];
                    st.Device = strSplit_data[1];
                    st.Lot = strSplit_data[2];
                    st.Lot_Dcc = strSplit_data[3];
                    st.Rcv_Qty = strSplit_data[4];
                    st.Die_Qty = strSplit_data[5];
                    st.Rcv_WQty = strSplit_data[6];
                    //st.Rcvddate = strSplit_data[7];
                    st.Lot_type = strSplit_data[8];
                    st.Bill = strSplit_data[9];
                    st.Amkorid = strSplit_data[10];
                    st.Wafer_lot = strSplit_data[11];
                    st.strCoo = strSplit_data[12];
                    st.state = strSplit_data[13];
                    st.strop = strSplit_data[14];
                    st.strGRstatus = strSplit_data[15];
                    st.Default_WQty = strSplit_data[16];

                    if (strSplit_data.Length > 17)
                    {
                        st.shipment = strSplit_data[17];
                        st.ReelID = strSplit_data[19];
                        st.ReelDCC = strSplit_data[20];
                    }
                    else
                        st.shipment = "";

                    dataGridView_Lot.Rows.Add(new object[] { m + 1, st.Lot, st.Lot_Dcc, st.Rcv_Qty, st.Die_Qty, st.Default_WQty, st.Rcv_WQty, st.state, st.strop, st.Bill, st.strGRstatus, st.shipment, st.Amkorid, st.WSN, st.ReelID, st.ReelDCC });

                    if (st.state == "Waiting")
                    {
                        dataGridView_Lot.Rows[m].DefaultCellStyle.BackColor = Color.LightGray;
                        dataGridView_Lot.Rows[m].DefaultCellStyle.ForeColor = Color.Black;

                        nWaitcount++;
                    }
                    else if (st.state == "Working")
                    {
                        dataGridView_Lot.Rows[m].DefaultCellStyle.BackColor = Color.Green;
                        dataGridView_Lot.Rows[m].DefaultCellStyle.ForeColor = Color.White;

                        nWorkcount++;
                    }
                    else if (st.state == "Complete")
                    {
                        if (st.strGRstatus == "COMPLETE")
                        {
                            dataGridView_Lot.Rows[m].DefaultCellStyle.BackColor = Color.DarkBlue;
                            dataGridView_Lot.Rows[m].DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (st.strGRstatus == "ERROR")
                        {
                            dataGridView_Lot.Rows[m].DefaultCellStyle.BackColor = Color.Yellow;
                            dataGridView_Lot.Rows[m].DefaultCellStyle.ForeColor = Color.Red;
                        }
                        else if (st.strGRstatus == "PROCESSING")
                        {
                            dataGridView_Lot.Rows[m].DefaultCellStyle.BackColor = Color.Green;
                            dataGridView_Lot.Rows[m].DefaultCellStyle.ForeColor = Color.White;
                        }
                        else
                        {
                            dataGridView_Lot.Rows[m].DefaultCellStyle.BackColor = Color.Blue;
                            dataGridView_Lot.Rows[m].DefaultCellStyle.ForeColor = Color.White;

                            if (strSplit_data.Length >= 19)
                                dataGridView_Lot.Rows[m].Cells["WSN"].Value = strSplit_data[18];
                        }

                        nCompletecount++;
                    }
                    else if (st.state == "Error")
                    {
                        dataGridView_Lot.Rows[m].DefaultCellStyle.BackColor = Color.Red;
                        dataGridView_Lot.Rows[m].DefaultCellStyle.ForeColor = Color.White;

                        nErrorcount++;
                    }
                }


                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "재작업";
                buttonColumn.UseColumnTextForButtonValue = true;
                buttonColumn.Text = "리셋";
                dataGridView_Lot.Columns.Insert(10, buttonColumn);

                Fnc_UpdateCount(strWorkFileName); //20.11.16.01                

                dataGridView_Lot.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                dataGridView_Lot.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                dataGridView_Lot.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_Lot.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                dataGridView_Lot.Sort(dataGridView_Lot.Columns[1], ListSortDirection.Ascending);
            }
            catch
            {
                bGridViewUpdate = false;
            }

            bGridViewUpdate = false;
            return 0;
        }


        private void btn_exit_Click(object sender, EventArgs e)
        {
            bTimeOutSt = true;
            bmode6 = false;
            dgv_loc.Rows.Clear();
            tabControl_Sort.SelectedIndex = 0;
        }

        private void btn_excleout_Click(object sender, EventArgs e)
        {
            string nowDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string pathFilename = string.Empty;
            ClickTime();

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Title = "Excel 파일 저장",
                FileName = $"Location_History_{nowDateTime}.xlsx",
                DefaultExt = "xlsx",
                Filter = "Xlsx files(*.xlsx)|*.xlsx"
            };


            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                pathFilename = saveFile.FileName.ToString();
                Properties.Settings.Default.Loc_file_save_path = pathFilename;
                Properties.Settings.Default.Save();

                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                excel.DisplayAlerts = false;

                //1. 워크시트 선택
                //처음에는 Sheet1로 1개 있음
                Worksheet worksheet = workbook.Worksheets.Item["Sheet1"];
                //여러 시트를 하려면 인덱스를 추가해서 받아서 사용 (2번째 부터는)
                //workbook.Worksheets.Add(After: workbook.Worksheets[index - 1]);
                //Worksheet worksheet = workbook.Worksheets.Item[index];

                //2. 필요시 시트 이름 변경
                worksheet.Name = DateTime.Now.ToLongDateString();

                //3. 컬럼 별로 너비 변경
                Range ModRange = worksheet.Columns[1];
                ModRange.ColumnWidth = 10;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[2];
                ModRange.ColumnWidth = 15;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                //넘버포맷을 사용하면 뒤 컬럼부터는 숫자형식으로 적용                
                ModRange = worksheet.Columns[3];
                ModRange.ColumnWidth = 10;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[4];
                ModRange.ColumnWidth = 20;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[5];
                ModRange.ColumnWidth = 15;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[6];
                ModRange.ColumnWidth = 20;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[7];
                ModRange.ColumnWidth = 30;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[11];
                ModRange.ColumnWidth = 20;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                Microsoft.Office.Interop.Excel.Range date = worksheet.Range["K:K"];

                date.NumberFormat = "0";

                //4. 첫번째 줄 타이틀 생성 - 예쁘게 하기 위해
                //Range는 엑셀을 실행해서 참고하기 좋음 (첫줄이라 1라인)
                ModRange = (Range)worksheet.get_Range("A1", "D1");
                ModRange.Merge(true); //병합하고
                ModRange.Value = $"Location History"; //이름 입력하고
                ModRange.Font.Size = 16; //폰트 키우고
                ModRange.Font.Bold = true; //Bold 주고
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter; //좌측 정렬
                                                                        //테두리 까지 끝
                ModRange.BorderAround2(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic, XlColorIndex.xlColorIndexAutomatic);

                //5. 2번째 줄에는 리포트 기간 및 파일 설명 추가
                ModRange = (Range)worksheet.get_Range("A2", "D2");
                ModRange.Merge(true);
                //DateTimePicker의 값을 그대로 넣어서 정보로 활용할 수 있음
                ModRange.Value = $"출력일 : {DateTime.Now:yyyy-MM-dd hh:mm:ss}";
                //2번째 설명은 우측 정렬
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignRight;

                //ex. 테두리를 위해 그리드 축 개수를 담아두고
                int columnCount = dgv_loc.Columns.Count;
                int rowCount = dgv_loc.Rows.Count;

                //5. 헤드열 추가
                //cell은 1부터 row나 column은 일반적인 0부터라 차이가 있는 점 주의
                for (int i = 0; i < columnCount; i++)
                {
                    ModRange = (Range)worksheet.Cells[3, 1 + i];
                    ModRange.Value = dgv_loc.Columns[i].HeaderText;
                    ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                    //data 테두리
                    ModRange.BorderAround2(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, XlColorIndex.xlColorIndexAutomatic);
                    ModRange.Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlMedium; //위 테두리
                    if (i == 0) //시작 컬럼에서 왼쪽 테두리
                    {
                        ModRange.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlMedium;
                    }
                    else if (i == (columnCount - 1)) //마지막 컬럼에서 우측 테두리
                    {
                        ModRange.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlMedium;
                    }
                    //아래 2줄 얇은 테두리
                    ModRange.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDouble;
                    ModRange.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThick;
                }

                //6. 데이터 열 추가
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        //타이틀, 추가설명, 헤드, 0->1 때문에 i에 4를 더함
                        ModRange = (Range)worksheet.Cells[4 + i, 1 + j];
                        ModRange.Value = dgv_loc[j, i].Value == null ? string.Empty : dgv_loc[j, i].Value.ToString();

                        //data 테두리
                        ModRange.BorderAround2(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, XlColorIndex.xlColorIndexAutomatic);
                        if (j == 0) //시작 컬럼에서 왼쪽 테두리
                        {
                            ModRange.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlMedium;
                        }
                        else if (j == (columnCount - 1)) //마지막 컬럼에서 우측 테두리
                        {
                            ModRange.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlMedium;
                        }
                        if (i == (rowCount - 1)) //마지막 로우에서 우측 테두리
                        {
                            ModRange.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlMedium;
                            //결산 같은 마지막 줄 값이 존재하면 이걸 사용합니다.
                            //ModRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlDouble;
                        }
                    }
                }

                //7. 상단 고정필드 설정
                worksheet.Application.ActiveWindow.SplitRow = 1;
                worksheet.Application.ActiveWindow.FreezePanes = true;
                worksheet.Application.ActiveWindow.SplitRow = 2;
                worksheet.Application.ActiveWindow.FreezePanes = true;
                worksheet.Application.ActiveWindow.SplitRow = 3;
                worksheet.Application.ActiveWindow.FreezePanes = true;

                //8. 파일 저장 (앞선 SaveFileDialog로 만들어진 pathFilename 경로로 파일 저장
                workbook.SaveAs(Filename: pathFilename);
                workbook.Close();
                MessageBox.Show("출력 완료.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void btn_mail_Click(object sender, EventArgs e)
        {
            ClickTime();
            Form1_Split_email email = new Form1_Split_email();
            email.ShowDialog();
        }


        public void Fnc_SaveLog_Work(string strSavePath, string strLog, string[] strinfo, int nMode) ///설비별 개별 로그 저장
        {
            //strSavePath는 device 또는 파일이름으로 로그 남김
            string strPath = "";

            strPath = strSavePath;

            string strToday = string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string strHead = string.Format(",{0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            strPath = strPath + "_Worklog" + ".txt";
            strHead = strToday + strHead;

            string strSave;
            strSave = strHead + ',' + strLog;
            Fnc_WriteFile(strPath, strSave);

            if (nMode == 1)
            {
                if (BankHost_main.bHost_connect)
                    BankHost_main.Host.Host_Hist_Job(strinfo);
            }
        }

        private void comboBox_hist_device_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nSel = comboBox_hist_device.SelectedIndex;

            if (nSel == 0)
            {
                label_histsel.Text = "-";
                textBox_input.Enabled = false;
                dateTimePicker_st.Enabled = false;
                dateTimePicker_ed.Enabled = false;
                comboBox_Hour_st.Enabled = false;
                comboBox_Hour_ed.Enabled = false;
                comboBox_Min_st.Enabled = false;
                comboBox_Min_ed.Enabled = false;
            }
            else if (nSel == 1)
            {
                label_histsel.Text = "-";
                textBox_input.Enabled = false;
                dateTimePicker_st.Enabled = true;
                dateTimePicker_ed.Enabled = true;
                comboBox_Hour_st.Enabled = true;
                comboBox_Hour_ed.Enabled = true;
                comboBox_Min_st.Enabled = true;
                comboBox_Min_ed.Enabled = true;
            }
            else if (nSel == 2)
            {
                label_histsel.Text = "Bill#";
                textBox_input.Enabled = true;
                dateTimePicker_st.Enabled = false;
                dateTimePicker_ed.Enabled = false;
                comboBox_Hour_st.Enabled = false;
                comboBox_Hour_ed.Enabled = false;
                comboBox_Min_st.Enabled = false;
                comboBox_Min_ed.Enabled = false;
                textBox_input.Focus();
            }
            else if (nSel == 3)
            {
                label_histsel.Text = "Device";
                textBox_input.Enabled = true;
                dateTimePicker_st.Enabled = false;
                dateTimePicker_ed.Enabled = false;
                comboBox_Hour_st.Enabled = false;
                comboBox_Hour_ed.Enabled = false;
                comboBox_Min_st.Enabled = false;
                comboBox_Min_ed.Enabled = false;
                textBox_input.Focus();
            }
        }

        private void comboBox_cust_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = comboBox_cust.Text;
            Fnc_Get_Information_Model(str, comboBox_Name);
        }

        private void button_workend2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("작업 종료\n\n작업을 마치시겠습니까?", "Alart", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.Yes)
            {
                strWorkFileName = "";
                BankHost_main.strOperator = "";

                dataGridView_worklist.Columns.Clear();
                dataGridView_worklist.Rows.Clear();
                dataGridView_worklist.Refresh();

                label_opinfo.Text = "-";

                BankHost_main.Host.Host_Set_Ready(BankHost_main.strEqid, "WAIT", "");
                BankHost_main.nWorkMode = 0;
                BankHost_main.strWork_Lotinfo = "";

                label_info.Text = "";
                label_info.BackColor = Color.DarkGray;
                label_info.ForeColor = Color.White;

                tabControl_Sort.SelectedIndex = 0;
            }
            else
            {
                textBox_Readdata.Focus();
                return;
            }
        }

        private void comboBox_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            BankHost_main.strCustName = comboBox_Name.Text;



            if (bmode7 == true)
            {
                Split_data_display();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            bTimeOutSt = true;
            label26.Text = "작업 모델";
            bmode7 = false;

            tabControl_Sort.SelectedIndex = 0;
        }

        public Dictionary<string, string> GetReelIDRule()
        {
            return selectCust[0];
        }

        public void runLogOutTimer()
        {
            if (bgw_timeout.IsBusy == false)
            {
                btimeOut = true;
                //bgw_timeout.RunWorkerAsync();

            }
        }

        public void stopLogOutTimer()
        {
            if (bgw_timeout.IsBusy == true)
            {
                btimeOut = false;
                bgw_timeout.CancelAsync();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectCust = selectCust.Cast<Dictionary<string, string>>().Where(r => r["CUST_NAME"] == comboBox_Name.Text).ToList();

            int nMode = comboBox_mode.SelectedIndex;

            if (BankHost_main.nScanMode == 0)
            {
                if (!BankHost_main.bVisionConnect)
                {
                    string strMsg = string.Format("카메라 연결이 되지 않았습니다.\n\n연결 상태를 확인 하시고 프로그램을 재시작 하세요");
                    Frm_Process.Form_Show(strMsg);
                    Frm_Process.Form_Display_Warning(strMsg);
                    Thread.Sleep(3000);
                    Frm_Process.Form_Hide();
                    return;
                }
            }

            string[] SecondPrinterNames = Properties.Settings.Default.SecondPrinterCustName.Split(';');

            SecondPrinterMode = false;

            for (int i = 0; i < SecondPrinterNames.Length; i++)
            {
                if (SecondPrinterNames[i] == comboBox_Name.Text)
                {
                    SecondPrinterMode = true;
                    break;
                }
            }

            if (BankHost_main.strOperator == "")
            {
                MessageBox.Show("작업 설정이 완료 되지 않았습니다.");
                return;
            }

            int nList = dataGridView_worklist.Rows.Count;

            if (nList < 1)
            {
                MessageBox.Show("작업 리스트가 없습니다.");
                return;
            }

            if (comboBox_Name.Text == "" && comboBox_Name.SelectedIndex == -1 && label_cust.Text != "ALL")
            {
                MessageBox.Show("모델 선택 하여 주십시오.");
                return;
            }

            BankHost_main.strWork_Cust = label_cust.Text;
            BankHost_main.strWork_Model = comboBox_Name.Text;

            try
            {
                if (nMode != 6)
                    BankHost_main.strWork_Shot1Lot = BankHost_main.Host.Host_Get_Shot1Lot(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
            }
            catch
            {
                BankHost_main.strWork_Shot1Lot = "NO";
            }

            if (BankHost_main.strWork_Model == "INARI" && BankHost_main.nScanMode == 1)
            {
                BankHost_main.strWork_Shot1Lot = "NO";
            }

            string str = "";
            if (nMode == 0 || nMode == 1 || nMode == 2 || nMode == 3)
                runLogOutTimer();


            if (nMode == 0 || nMode == 1)
            {

                str = string.Format("\n\n작업을 시작 합니다. AUTO GR 모드 ");
                //상태 변경//
                BankHost_main.Host.Host_Set_Ready(BankHost_main.strEqid, "OK", "1");
                BankHost_main.nWorkMode = 1;

                for (int n = 0; n < strSelBillno.Length; n++)
                {
                    if (strSelBillno[n] != "" && strSelBillno[n].Length > 5)
                    {
                        string strJudge = BankHost_main.Host.Host_Set_Workinfo(BankHost_main.strEqid, strWorkFileName, strSelBillno[n], " ", "WORK");
                        if (strJudge != "OK")
                        {
                            MessageBox.Show("Bill 정보 DB 저장 실패!");
                        }
                    }
                }

                button_autogr.Enabled = true;
            }
            else if (nMode == 6)
            {
                str = string.Format("\n\nSplit Lot Vaildation 모드");
                com_die = 0;
                com_wfr = 0;
                com_lots = 0;

                tot_die = 0;
                tot_lots = 0;
                tot_wfr = 0;

                LastClickTime = DateTime.Now;
                runLogOutTimer();

                Set_split_lot_data();

                tabControl_Sort.SelectedIndex = 7;

                if (GetIME() == true)
                {
                    ChangeIME(tb_split);
                }
                tb_split.Focus();

                bmode7 = true;
            }
            else
            {
                str = string.Format("\n\n작업을 시작 합니다. Validation 모드");
                //상태 변경//
                BankHost_main.Host.Host_Set_Ready(BankHost_main.strEqid, "OK", "2");
                BankHost_main.nWorkMode = 2;

                BankHost_main.Host.Host_Set_Workinfo(BankHost_main.strEqid, strWorkFileName, strSelBillno[0], "", "WORK");

                LastClickTime = DateTime.Now;

                if (bgw_timeout.IsBusy == false)
                    runLogOutTimer();

                //button_autogr.BackColor = Color.LightGray;
                button_autogr.Enabled = false;
            }

            Frm_Process.Form_Show(str);

            //필요한 정보만 가져오기
            Frm_Process.Hide();



            BankHost_main.nProcess = 1000; //스캔 대기
            if (nMode != 6)
            {
                Fnc_WorkDownload(strWorkFileName);

                //tabControl_Sort.SelectedIndex = 1;
                tabControl_Sort.SelectedIndex = 2;

                BankHost_main.Host.Host_Set_Jobname(BankHost_main.strEqid, strWorkFileName);

                ////Work Bcr info Update
                string strModel = comboBox_Name.Text;
                Fnc_Get_WorkBcrInfo(BankHost_main.strWork_Cust, strModel);

                if (Form_Print.bPrintState && Form_Print.bPrintUse)
                {
                    label_printstate.Text = "프린트 사용 OK";
                    label_printstate.ForeColor = Color.Blue;
                }
                else
                {
                    label_printstate.Text = "프린트 사용 안함";
                    label_printstate.ForeColor = Color.Red;
                }

                nLabelcount = 0;
                nLabelttl = 0;

                string[] printinfo = { "", "" };
                printinfo[0] = "1"; printinfo[1] = "";
                BankHost_main.Host.Host_Set_Print_Data(BankHost_main.strEqid, printinfo);
                BankHost_main.Host.Host_Delete_BcrReadinfoAll(BankHost_main.strEqid);
            }
        }

        private void comboBox_mode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (label_cust.Text != "ALL")
                    comboBox_Name.Focus();
                else
                    button1_Click(sender, e);
            }

        }


        private void comboBox_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
                button1_Click(sender, e);
        }

        string Split_Scandata = "";
        private void tb_split_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                ClickTime();
                Split_Scandata = tb_split.Text;

                if (Split_Scandata == "")
                    return;

                tb_split.Text = "";
                Search_data();

                tb_com_lots.Text = com_lots.ToString();
                tb_com_die.Text = com_die.ToString();
                tb_com_wfr.Text = com_wfr.ToString();
            }
        }

        private void Search_data()
        {
            bool is_in = false;

            if (Split_Scandata.Split(':').Length < 6)
                return;

            string[] scandata = Split_Scandata.Split(':');

            for (int i = 0; i < scandata.Length; i++)
            {
                scandata[i] = scandata[i].Trim();
            }

            for (int i = 0; i < dgv_split_log.RowCount; i++)
            {
                if (dgv_split_log.Rows[i].Cells[4].Value.ToString() == scandata[2] &&   //DEV  
                    dgv_split_log.Rows[i].Cells[5].Value.ToString() == scandata[0] &&   //LOT                    
                    dgv_split_log.Rows[i].Cells[6].Value.ToString() == scandata[1])     //DCC   
                {
                    if (dgv_split_log.Rows[i].Cells[1].Value.ToString() == scandata[6]) //cust
                    {
                        speech.SpeakAsync("고객 코드가 틀립니다.");
                        return;
                    }

                    if (int.Parse(dgv_split_log.Rows[i].Cells[7].Value.ToString()) == int.Parse(scandata[3]) && //Die Qty
                    int.Parse(dgv_split_log.Rows[i].Cells[8].Value.ToString()) == int.Parse(scandata[4]))       // Wfr Qty
                    {
                        if (dgv_split_log.Rows[i].Cells[11].Value != null)
                        {
                            dgv_split_log.Rows[i].Selected = true;
                            dgv_split_log.FirstDisplayedScrollingRowIndex = i;

                            if (dgv_split_log.Rows[i].Cells[11].Value.ToString() == "COMPLETE")
                            {
                                speech.SpeakAsync("이미 완료된 자재 입니다.");
                            }
                            else
                            {
                                is_in = true;
                                dgv_split_log.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                                dgv_split_log.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                                dgv_split_log.Rows[i].Cells[11].Value = "COMPLETE";
                                dgv_split_log.Rows[i].Cells[12].Value = BankHost_main.strOperator;
                                dgv_split_log.Rows[i].Cells[13].Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                speech.SpeakAsync((i + 1).ToString() + "완료");

                                com_lots++;
                                com_die += int.Parse(dgv_split_log.Rows[i].Cells[7].Value.ToString());
                                com_wfr += int.Parse(dgv_split_log.Rows[i].Cells[8].Value.ToString());

                                Write_split_data(i, "COMPLETE");
                            }
                        }
                        else
                        {
                            dgv_split_log.Rows[i].Selected = true;
                            dgv_split_log.FirstDisplayedScrollingRowIndex = i;

                            is_in = true;
                            dgv_split_log.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            dgv_split_log.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                            speech.SpeakAsync((i + 1).ToString() + "완료");

                            com_lots++;
                            com_die += int.Parse(dgv_split_log.Rows[i].Cells[7].Value.ToString());
                            com_wfr += int.Parse(dgv_split_log.Rows[i].Cells[8].Value.ToString());

                            Write_split_data(i, "COMPLETE");
                        }
                    }
                    else
                    {
                        is_in = true;
                        dgv_split_log.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        dgv_split_log.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                        speech.SpeakAsync("수량 틀림");
                    }
                }
            }

            if (is_in == false)
            {
                speech.SpeakAsync("리스트에 없는 자재 입니다.");
            }
        }

        private void Write_split_data(int cnt, string msg)
        {
            string folderpath = strExcutionPath + "\\Work\\Split_log";
            string strFileName = string.Format("{0}\\Work\\Split_log\\{1}.txt", strExcutionPath, DateTime.Now.ToShortDateString());
            bool bdata = false;
            List<string> added_string = new List<string>();
            List<string> Split_list = new List<string>();

            string[] temp = System.IO.File.ReadAllLines(strFileName);

            for (int i = 0; i < temp.Length; i++)
            {
                string[] arr = temp[i].Split('\t');

                if (arr[1] == dgv_split_log.Rows[cnt].Cells[2].Value.ToString() &&                                     // CUST
                    arr[3] == dgv_split_log.Rows[cnt].Cells[4].Value.ToString() &&  // DEV
                    arr[4] == dgv_split_log.Rows[cnt].Cells[5].Value.ToString() &&  // LOT
                    arr[5] == dgv_split_log.Rows[cnt].Cells[6].Value.ToString() &&  // DCC
                    arr[6] == dgv_split_log.Rows[cnt].Cells[7].Value.ToString() &&  // Die Qty
                    arr[7] == dgv_split_log.Rows[cnt].Cells[8].Value.ToString())    // Wft Qty
                {
                    bdata = true;
                    //temp[0] = "Line\tCust\tBinding#\tDevice#\tCust\tLot#\tDcc\tReturn Qty\tReturn Wafer\tReturn Date\tLoc\tStatus\tOper\tScantime";

                    if (temp[i].Split('\t').Length == 10)
                        temp[i] += string.Format("\t{0}\t{1}\t{2}", msg, BankHost_main.strOperator, dgv_split_log.Rows[cnt].Cells[13].Value.ToString());
                    else
                    {
                        string[] split_temp = temp[i].Split('\t');

                        split_temp[10] = msg;
                        split_temp[11] = BankHost_main.strOperator;
                        split_temp[12] = dgv_split_log.Rows[cnt].Cells[13].Value.ToString();

                        temp[i] = string.Join("\t", split_temp);
                    }
                    break;
                }
            }

            if (bdata == true)
                Split_log_new_file_save(string.Join("\n", temp));
        }

        public string GetDicKeyVal(Dictionary<string, string> dic, string name)
        {
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                if (kvp.Value.Contains(name) == true)
                {
                    return kvp.Key;
                }
            }

            return "EMPTY";
        }

        WorkInfo AWork = new WorkInfo();

        public void Fnc_Get_WorkBcrInfo(string strGetCust, string strModelName)
        {
            //var dt_list = BankHost_main.Host.Host_Get_BCRFormat();
            //List<Dictionary<string, string>> cust = WAS2CUST(GetWebServiceData($"http://10.131.10.84:8080/api/diebank/bcr-master/k4/json"));

            if (selectCust.Count == 0)
                return;

            for (int n = 0; n < selectCust.Count; n++)
            {


                if (strGetCust == selectCust[n]["CUST_CODE"] && strModelName == selectCust[n]["CUST_NAME"])
                {
                    WorkInfo AWork = new WorkInfo();

                    AWork.strCust = selectCust[n]["CUST_CODE"].ToString().Trim();
                    AWork.strBank = selectCust[n].ContainsKey("BANK_NO") == true ? selectCust[n]["BANK_NO"].ToString().Trim() : "";
                    AWork.strSPR = selectCust[n]["SPLITER"].ToString().Trim();
                    AWork.strModelName = selectCust[n]["CUST_NAME"].ToString().Trim();
                    AWork.strMtlType = selectCust[n].ContainsKey("MTL_TYPE") == true ? selectCust[n]["MTL_TYPE"].ToString() : "";
                    AWork.strMultiLot = selectCust[n].ContainsKey("MULTI_LOT") == true ? selectCust[n]["MULTI_LOT"].ToString() : "";
                    BankHost_main.strWork_BcdType = selectCust[n]["BCR_TYPE"];

                    AWork.strDevicePos = GetDicKeyVal(selectCust[n], "DEVICE") == "EMPTY" ? "-1" : $"{(int.Parse(GetDicKeyVal(selectCust[n], "DEVICE").Replace("BCD", "")) - 1).ToString()}{(selectCust[n][GetDicKeyVal(selectCust[n], "DEVICE")].ToString().Contains('/') == true ? "/" + selectCust[n][GetDicKeyVal(selectCust[n], "DEVICE")].ToString().Split('/')[1].ToString() : "")} ";
                    AWork.strLotidPos = $"{(int.Parse(GetDicKeyVal(selectCust[n], "LOT").Replace("BCD", "")) - 1).ToString()}{(selectCust[n][GetDicKeyVal(selectCust[n], "LOT")].ToString().Contains('/') == true ? "/" + selectCust[n][GetDicKeyVal(selectCust[n], "LOT")].ToString().Split('/')[1].ToString() : "")}";
                    AWork.strLotDigit = "";
                    AWork.strQtyPos = GetDicKeyVal(selectCust[n], "QTY") == "EMPTY" ? "-1" : $"{(int.Parse(GetDicKeyVal(selectCust[n], "QTY").Replace("BCD", "")) - 1).ToString()}{(selectCust[n][GetDicKeyVal(selectCust[n], "QTY")].ToString().Contains('/') == true ? "/" + selectCust[n][GetDicKeyVal(selectCust[n], "QTY")].ToString().Split('/')[1].ToString() : "")}";
                    AWork.strUdigit = "";// cust[n]["UDIGIT"].ToString(); AWork.strUdigit = AWork.strUdigit.Trim();
                    AWork.strWfrPos = "";//e cust[n]["TTL_WFR_QTY"].ToString(); AWork.strWfrPos = AWork.strWfrPos.Trim();
                    AWork.strLot2Wfr = "";// cust[n]["LOT2WFR"].ToString(); AWork.strLot2Wfr = AWork.strLot2Wfr.Trim();
                    AWork.strTTLWFR = "";// cust[n]["TTLWFR"].ToString().Trim().ToUpper();
                    AWork.strWSN = "";// cust[n]["WSN"].ToString().Trim().ToUpper();
                    AWork.strLPN = GetDicKeyVal(selectCust[n], "LPN") == "EMPTY" ? "-1" : $"{(int.Parse(GetDicKeyVal(selectCust[n], "LPN").Replace("BCD", "")) - 1).ToString()}{(selectCust[n][GetDicKeyVal(selectCust[n], "LPN")].ToString().Contains('/') == true ? "/" + selectCust[n][GetDicKeyVal(selectCust[n], "LPN")].ToString().Split('/')[1].ToString() : "")}";

                    int nType = BankHost_main.Host.Host_Get_PrintType(AWork.strCust);
                    AWork.nBcrPrintType = nType;

                    BankHost_main.Process_GetWorkInformation(AWork);
                }
            }
        }

        private void button_lotdownload_Click(object sender, EventArgs e)
        {
            int nWait = Int32.Parse(label_scan_wait.Text);
            int nWork = Int32.Parse(label_scan_work.Text);
            int nError = Int32.Parse(label_scan_error.Text);
            int nCount = nWait + nWork + nError;

            if (nWait > 0)
            {
                string str = string.Format("{0} 개 Lot가 완료 되지 않았습니다.\n\n그래도 저장 하시 겠습니까?", nCount);
                DialogResult dialogResult1 = MessageBox.Show(str, "Alart", MessageBoxButtons.YesNo);
                if (dialogResult1 == DialogResult.No)
                {
                    return;
                }
            }
            label_wait.Text = "";
            label_work.Text = "";
            label_complete.Text = "";
            label_error.Text = "";

            int nIndex_Device = dataGridView_Device.CurrentCell.RowIndex;
            string strDevice = dataGridView_Device.Rows[nIndex_Device].Cells[1].Value.ToString();

            string strFilename;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "저장 경로 설정";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.DefaultExt = ".xlsx";
            saveFileDialog.Filter = "Xlsx files(*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                strFilename = saveFileDialog.FileName;

                tabControl_Sort.SelectedIndex = 1;
                tabControl_Sort.SelectedIndex = 2;

                dataGridView_Device.Rows[nIndex_Device].Cells[1].Selected = true;

                while (bGridViewUpdate)
                {
                    Thread.Sleep(1);
                    System.Windows.Forms.Application.DoEvents();
                }

                try
                {
                    Fnc_GetDeviceData(strDevice);

                }
                catch
                {
                    return;
                }

                Fnc_ExcelCreate_Lotinfo(strFilename, strDevice);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string nowDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string pathFilename = string.Empty;
            ClickTime();

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Title = "Excel 파일 저장",
                FileName = $"Split_Log_{BankHost_main.strWork_Cust}_{BankHost_main.strWork_Model}_{nowDateTime}.xlsx",
                DefaultExt = "xlsx",
                Filter = "Xlsx files(*.xlsx)|*.xlsx"
            };

            saveFile.InitialDirectory = Properties.Settings.Default.SPLIT_LOG_SAVE_PATH;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                pathFilename = saveFile.FileName.ToString();
                Properties.Settings.Default.SPLIT_LOG_SAVE_PATH = pathFilename;
                Properties.Settings.Default.Save();

                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                excel.DisplayAlerts = false;

                //1. 워크시트 선택
                //처음에는 Sheet1로 1개 있음
                Worksheet worksheet = workbook.Worksheets.Item["Sheet1"];
                //여러 시트를 하려면 인덱스를 추가해서 받아서 사용 (2번째 부터는)
                //workbook.Worksheets.Add(After: workbook.Worksheets[index - 1]);
                //Worksheet worksheet = workbook.Worksheets.Item[index];

                //2. 필요시 시트 이름 변경
                worksheet.Name = BankHost_main.strWork_Cust + "_" + BankHost_main.strWork_Model;

                //3. 컬럼 별로 너비 변경
                Range ModRange = worksheet.Columns[1];
                ModRange.ColumnWidth = 10;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[2];
                ModRange.ColumnWidth = 15;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                //넘버포맷을 사용하면 뒤 컬럼부터는 숫자형식으로 적용                
                ModRange = worksheet.Columns[3];
                ModRange.ColumnWidth = 10;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[4];
                ModRange.ColumnWidth = 25;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[5];
                ModRange.ColumnWidth = 15;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[6];
                ModRange.ColumnWidth = 20;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[7];
                ModRange.ColumnWidth = 10;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[8];
                ModRange.ColumnWidth = 10;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[9];
                ModRange.ColumnWidth = 13;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[10];
                ModRange.ColumnWidth = 15;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignRight;
                ModRange = worksheet.Columns[11];
                ModRange.ColumnWidth = 10;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[12];
                ModRange.ColumnWidth = 12;
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ModRange = worksheet.Columns[14];
                ModRange.ColumnWidth = 20;

                Microsoft.Office.Interop.Excel.Range date = worksheet.Range["J:J"];

                date.NumberFormat = "0";

                //4. 첫번째 줄 타이틀 생성 - 예쁘게 하기 위해
                //Range는 엑셀을 실행해서 참고하기 좋음 (첫줄이라 1라인)
                ModRange = (Range)worksheet.get_Range("A1", "D1");
                ModRange.Merge(true); //병합하고
                ModRange.Value = $"{BankHost_main.strWork_Cust}_{BankHost_main.strWork_Model}Split Log"; //이름 입력하고
                ModRange.Font.Size = 16; //폰트 키우고
                ModRange.Font.Bold = true; //Bold 주고
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter; //좌측 정렬
                                                                        //테두리 까지 끝
                ModRange.BorderAround2(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic, XlColorIndex.xlColorIndexAutomatic);

                //5. 2번째 줄에는 리포트 기간 및 파일 설명 추가
                ModRange = (Range)worksheet.get_Range("A2", "D2");
                ModRange.Merge(true);
                //DateTimePicker의 값을 그대로 넣어서 정보로 활용할 수 있음
                ModRange.Value = $"출력일 : {DateTime.Now:yyyy-MM-dd_HH:mm:ss}";
                //2번째 설명은 우측 정렬
                ModRange.HorizontalAlignment = XlHAlign.xlHAlignRight;

                //ex. 테두리를 위해 그리드 축 개수를 담아두고
                int columnCount = dgv_split_log.Columns.Count;
                int rowCount = dgv_split_log.Rows.Count;

                //5. 헤드열 추가
                //cell은 1부터 row나 column은 일반적인 0부터라 차이가 있는 점 주의
                for (int i = 0; i < columnCount; i++)
                {
                    ModRange = (Range)worksheet.Cells[3, 1 + i];
                    ModRange.Value = dgv_split_log.Columns[i].HeaderText;
                    ModRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                    //data 테두리
                    ModRange.BorderAround2(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, XlColorIndex.xlColorIndexAutomatic);
                    ModRange.Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlMedium; //위 테두리
                    if (i == 0) //시작 컬럼에서 왼쪽 테두리
                    {
                        ModRange.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlMedium;
                    }
                    else if (i == (columnCount - 1)) //마지막 컬럼에서 우측 테두리
                    {
                        ModRange.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlMedium;
                    }
                    //아래 2줄 얇은 테두리
                    ModRange.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlDouble;
                    ModRange.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThick;
                }

                int row_cnt = 0;

                //6. 데이터 열 추가
                for (int i = 0; i < rowCount; i++)
                {
                    if (dgv_split_log.Rows[i].Cells[11].Value.ToString() == "COMPLETE")
                    {
                        for (int j = 0; j < columnCount; j++)
                        {
                            if (j == 0)
                            {
                                ModRange = (Range)worksheet.Cells[4 + row_cnt, 1 + j];
                                ModRange.Value = (row_cnt + 1).ToString();
                            }
                            else
                            {
                                ModRange = (Range)worksheet.Cells[4 + row_cnt, 1 + j];
                                ModRange.Value = dgv_split_log[j, i].Value == null ? string.Empty : dgv_split_log[j, i].Value.ToString();
                            }
                            //타이틀, 추가설명, 헤드, 0->1 때문에 i에 4를 더함


                            //data 테두리
                            ModRange.BorderAround2(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, XlColorIndex.xlColorIndexAutomatic);
                            if (j == 0) //시작 컬럼에서 왼쪽 테두리
                            {
                                ModRange.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlMedium;
                            }
                            else if (j == (columnCount - 1)) //마지막 컬럼에서 우측 테두리
                            {
                                ModRange.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlMedium;
                            }
                            if (i == (rowCount - 1)) //마지막 로우에서 우측 테두리
                            {
                                ModRange.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlMedium;
                                //결산 같은 마지막 줄 값이 존재하면 이걸 사용합니다.
                                //ModRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlDouble;
                            }
                        }
                        row_cnt++;
                    }
                }

                ModRange = (Range)worksheet.Cells[4 + row_cnt, 6];
                ModRange.Value = "Complete :";
                ModRange.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThin;
                ModRange.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
                ModRange.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;
                ModRange = (Range)worksheet.Cells[4 + row_cnt, 7];
                ModRange.Value = com_lots.ToString();
                ModRange.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;
                ModRange.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
                ModRange = (Range)worksheet.Cells[4 + row_cnt, 8];
                ModRange.Value = com_die.ToString();
                ModRange.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;
                ModRange.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
                ModRange = (Range)worksheet.Cells[4 + row_cnt, 9];
                ModRange.Value = com_wfr.ToString();
                ModRange.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
                ModRange.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;


                //7. 상단 고정필드 설정
                worksheet.Application.ActiveWindow.SplitRow = 1;
                worksheet.Application.ActiveWindow.FreezePanes = true;
                worksheet.Application.ActiveWindow.SplitRow = 2;
                worksheet.Application.ActiveWindow.FreezePanes = true;
                worksheet.Application.ActiveWindow.SplitRow = 3;
                worksheet.Application.ActiveWindow.FreezePanes = true;
                workbook.SaveAs(Filename: pathFilename);

                //worksheet.PageSetup.PrintArea = string.Format("A1:n{0}",4+row_cnt);
                worksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;



                MessageBox.Show("출력 완료.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //if(DialogResult.Yes == MessageBox.Show("PDF로 저장 하시겠습니까?","PDF", MessageBoxButtons.YesNo))
                //{
                //    workbook.ExportAsFixedFormat(
                //        Excel.XlFixedFormatType.xlTypePDF,
                //        pathFilename.Split('.')[0],
                //        Excel.XlFixedFormatQuality.xlQualityStandard,
                //        true,
                //        true,
                //        1,
                //        10,
                //        false);
                //    // workbook.SaveAs(Filename: pathFilename.Split('.')[0], FileFormat: "Pdf");
                //}
                //8. 파일 저장 (앞선 SaveFileDialog로 만들어진 pathFilename 경로로 파일 저장

                workbook.Close();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ClickTime();
            dataGridView_label.Rows.Clear();
            tot_lots = 0;
            tot_die = 0;
            tot_wfr = 0;
            AmkorLabelCnt = 1;

            tb_next.Text = "";
            numericUpDown1.Value = 0;

            lprinted_lots.Text = tot_lots.ToString();
            ldie.Text = tot_die.ToString();
            lwfr.Text = tot_wfr.ToString();
        }


        private void dataGridView_label_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            tot_lots = dataGridView_label.RowCount;
            tot_die = 0;
            tot_wfr = 0;

            int row_cnt = 1;

            foreach (DataGridViewRow row in dataGridView_label.Rows)
            {
                tot_die += int.Parse(row.Cells[4].Value.ToString());
                tot_wfr += int.Parse(row.Cells[5].Value.ToString());

                row.Cells[0].Value = row_cnt.ToString();
                row_cnt++;
            }

            lprinted_lots.Text = tot_lots.ToString();
            ldie.Text = tot_die.ToString();
            lwfr.Text = tot_wfr.ToString();
        }

        private void dataGridView_label_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        /// <summary>
        /// dgv_split_log의 row index 받아서 string으로 변환해서 구분자 ","로 반환
        /// </summary>
        /// <param name="index">row index</param>
        /// <returns></returns>
        private string GetDGVRow2Str(int index)
        {
            string res = "";

            for (int i = 0; i < 11; i++)
            {
                if (dgv_split_log.Rows[index].Cells[i].Value != null)
                {
                    res += dgv_split_log.Rows[index].Cells[i].Value.ToString() + ",";
                }
                else
                {
                    res += ",";
                }
            }

            res = res.Remove(res.Length - 1, 1);

            return res;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string dgv_str_val = "";
            ClickTime();

            for (int i = 0; i < dgv_split_log.RowCount; i++)
            {
                if (dgv_split_log.Rows[i].Cells[11].Value != null)
                {
                    if (dgv_split_log.Rows[i].Cells[11].Value.ToString() == "")
                    {
                        dgv_str_val += GetDGVRow2Str(i) + ";";
                    }
                }
                else
                {
                    dgv_str_val += GetDGVRow2Str(i) + ";";
                }
            }

            dgv_str_val = dgv_str_val.Remove(dgv_str_val.Length - 1, 1);
            Form_email_review review_form = new Form_email_review(dgv_str_val);

            review_form.Show();
        }



        private void btn_search_Click(object sender, EventArgs e)
        {
            ClickTime();

            if (bDownloadComp == false)
            {
                SetProgressba("조회를 시작 합니다.", 0);

                Thread ExcelDownThread = new Thread(ScrapExcelDown);
                ExcelDownThread.Start();
            }

            tb_scrapinput.Focus();
        }

        private void ExcelImport()
        {
            try
            {
                if (cb_download.Checked == false)
                {
                    SetProgressba("Excel Data를 Memory에 복사 중 입니다.", 1);
                    Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                    Workbook workbook = application.Workbooks.Open(Filename: System.Windows.Forms.Application.StartupPath + "\\SCRAP\\" + file_name);
                    Worksheet worksheet1 = workbook.Worksheets.get_Item(1);
                    application.Visible = checkBox1.Checked;
                    SetProgressba("Excel Data를 Memory에 복사 완료 하였습니다.", 2);

                    Range range = worksheet1.UsedRange;
                    double dd = 0.0;
                    List<string> data = new List<string>();
                    string excelrow = "";

                    progressBar1.Maximum = range.Rows.Count * range.Columns.Count;

                    for (int i = 1; i <= range.Rows.Count - 2; ++i)
                    {
                        excelrow = "";

                        for (int j = 1; j <= range.Columns.Count; ++j)
                        {
                            SetProgressba("Excel Data 정리 중입니다 : " + (range.Cells[i, j] as Range).Value2, i * j);

                            //if (j == 13 || j == 14 || j == 26)
                            //{
                            //    if ((range.Cells[i, j] as Range).Value2 != null)
                            //    {
                            //        if (double.TryParse((range.Cells[i, j] as Range).Value2.ToString(), out dd))
                            //            excelrow += ((range.Cells[i, j] as Range).Value2 != null ? DateTime.FromOADate(dd) + "," : ",");
                            //        else
                            //            excelrow += ((range.Cells[i, j] as Range).Value2 != null ? (range.Cells[i, j] as Range).Value2.ToString() + "," : ",");
                            //    }
                            //}
                            //else 
                            if (j == 9)
                            {
                                if (double.TryParse((range.Cells[i, j] as Range).Value2.ToString(), out dd))
                                    excelrow += ((range.Cells[i, j] as Range).Value2 != null ? DateTime.FromOADate(dd) + "," : ",");
                                else
                                    excelrow += ((range.Cells[i, j] as Range).Value2 != null ? (range.Cells[i, j] as Range).Value2.ToString() + "," : ",");
                            }
                            else if (j != range.Columns.Count)
                                excelrow += ((range.Cells[i, j] as Range).Value2 != null ? (range.Cells[i, j] as Range).Value2.ToString() + "," : ",");
                            else
                                excelrow += ((range.Cells[i, j] as Range).Value2 != null ? (range.Cells[i, j] as Range).Value2.ToString() : "");
                        }

                        data.Add(excelrow);
                    }

                    /*메모리 할당 해제*/
                    Marshal.ReleaseComObject(range);
                    Marshal.ReleaseComObject(worksheet1);
                    workbook.Close();
                    Marshal.ReleaseComObject(workbook);
                    application.Quit();
                    Marshal.ReleaseComObject(application);

                    string[] datatemp;
                    string sqlstr = "";
                    int DBrowcount = -1;
                    string datastr = "";
                    int next_no = int.Parse(SearchData("select max(No) from TB_SCRAP2").Tables[0].Rows[0][0].ToString()) + 1;

                    progressBar1.Maximum = data.Count;

                    for (int i = 1; i < data.Count; i++)
                    {
                        datatemp = data[i].Split(',');
                        SetProgressba("Database와 비교 중입니다 : " + datatemp[3], i);

                        sqlstr = string.Format("select count(*) from TB_SCRAP2 with(NOLOCK) where [DEVICE]='{0}' and [LOT]='{1}' and [DIE]='{2}' and [WAFER]='{3}' and [CUST]='{4}'",
                            datatemp[2], datatemp[4], datatemp[5], datatemp[6], datatemp[1]);

                        DBrowcount = run_count(sqlstr);

                        if (DBrowcount == 0)
                        {
                            datastr = "";

                            for (int j = 0; j < datatemp.Length; j++)
                            {
                                if (j == 6 && j == 5)
                                {
                                    datastr += string.Format("{0},", datatemp[j].Substring(0, 1) == "'" ? datatemp[j].Substring(1, datatemp[j].Length - 1) : datatemp[j]);
                                }
                                else
                                {
                                    if (datatemp[j] != "")
                                        datastr += string.Format("'{0}',", datatemp[j].Substring(0, 1) == "'" ? datatemp[j].Substring(1, datatemp[j].Length - 1) : datatemp[j]);
                                    else
                                        datastr += string.Format("'{0}',", datatemp[j]);
                                }
                            }

                            SetProgressba("Database 삽입 중", i);

                            sqlstr = string.Format("Set IDENTITY_INSERT TB_SCRAP2 ON; Insert into TB_SCRAP2 " +
                                "(No,[DATE],[REQUEST],[CUST],[DEVICE],[P_D_L],[LOT],[DIE],[WAFER],[LOCATION],[REQUEST_ON],[REQUEST_BY],[CERITIFICATE],[1ST],[2ND],[3RD]) " +
                                "values({0},GETDATE(),{1}'','','') Set IDENTITY_INSERT TB_SCRAP2 OFF;", next_no++, datastr);
                            run_sql_command(sqlstr);
                        }
                    }

                    SetProgressba("Data 검증 완료", progressBar1.Maximum);
                    datastr = "";

                    //datastr = string.Format("[CHG_DATE_TIME] >= {0}", sdt.Value.ToString("yyyy"));
                }

                ReadScrapData();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        System.Data.DataSet dtScrap;

        int n1stCnt = 0;
        int n2ndCnt = 0;
        int n3rdCnt = 0;
        int nTotLot = 0;
        int nTotDie = 0;
        int nTotWfr = 0;


        private void ReadScrapData()
        {
            string datastr = "";
            n1stCnt = 0;
            n2ndCnt = 0;
            n3rdCnt = 0;
            nTotLot = 0;
            nTotDie = 0;
            nTotWfr = 0;
            //request number 선택 할 수 있게
            // 
            try
            {
                DataSet request = SearchData(string.Format("select DISTINCT [REQUEST] from TB_SCRAP2 with(NOLOCK)  where [DATE] >= '{0}' and [DATE] <= '{1}'", sdt.Value.ToString("yyyyMMdd"), edt.Value.AddDays(1).ToString("yyyyMMdd")));
                List<string> RequestID = new List<string>();

                cbRequest.Items.Clear();

                for (int i = 0; i < request.Tables[0].Rows.Count; i++)
                {
                    RequestID.Add(request.Tables[0].Rows[i][0].ToString());
                    cbRequest.Items.Add(request.Tables[0].Rows[i][0].ToString());
                }

                string SelRequest = SelectRequest(RequestID, "Vaildation할 Request를 선택해 주세요");

                if (SelRequest == "EMPTY")
                {
                    return;
                }

                ReadScrapDBData(SelRequest);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void ReadScrapDBData(string SelectRequte)
        {
            string datastr = string.Format("select [REQUEST],[CUST],[DEVICE],[LOT],[DIE],[WAFER],[1st],[2nd],[LOCATION],[CERITIFICATE] from TB_SCRAP2 with(NOLOCK) where [DATE] >= '{0}' and [DATE] <= '{1}' and [REQUEST]='{2}' order by [LOT]",
                sdt.Value.ToString("yyyyMMdd"), edt.Value.AddDays(1).ToString("yyyyMMdd"), SelectRequte);
            dtScrap = SearchData(datastr);


            n1stCnt = 0;
            n2ndCnt = 0;
            nTotLot = 0;
            nTotDie = 0;
            nTotWfr = 0;

            //dgv_scrap = new DataGridView();

            if (dgv_scrap.DataSource != null)
                dgv_scrap.DataSource = null;

            dgv_scrap.DataSource = dtScrap.Tables[0];

            dgv_scrap.Columns[1].Width = 50;
            dgv_scrap.Columns[3].Width = 130;
            dgv_scrap.Columns[4].Width = 70;
            dgv_scrap.Columns[5].Width = 40;

            bDownloadComp = false;

            dgv_scrap.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            nTotLot = dgv_scrap.RowCount;

            for (int i = 0; i < dgv_scrap.RowCount; i++)
            {
                nTotDie += (int)dtScrap.Tables[0].Rows[i][4];
                nTotWfr += (int)dtScrap.Tables[0].Rows[i][5];
                if (dtScrap.Tables[0].Rows[i][6].ToString() != "" && dtScrap.Tables[0].Rows[i][7].ToString() == "")
                {
                    dgv_scrap.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    n1stCnt++;
                }
                else if (dtScrap.Tables[0].Rows[i][6].ToString() != "" && dtScrap.Tables[0].Rows[i][7].ToString() != "")
                {
                    dgv_scrap.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                    n2ndCnt++;
                }
            }



            l1stComp.Text = n1stCnt.ToString();
            l2ndComp.Text = n2ndCnt.ToString();

            lTOTLot.Text = string.Format("Total Lot : {0}", nTotLot);
            lDieCnt.Text = string.Format("{0}", nTotDie);
            lTOTWfr.Text = string.Format("Total Wfr : {0}", nTotWfr);

            if (nTotLot != n1stCnt && n2ndCnt == 0)
            {
                ScrapMode = 1;
                SetProgressba("1차 검수 완료 후 2차 검수 진행 가능 합니다.", 0);

                using (Form_Board board = new Form_Board("1차 검수 완료 후 2차 검수 진행 가능 합니다.", Color.Black, Color.Red))
                {
                    board.ShowDialog();
                }
            }
            else if (nTotLot == n1stCnt)
            {
                ScrapMode = 2;
                SetProgressba("2차 검수 진행 가능 합니다.", 0);
            }

            ShowComment(dgv_scrap.Rows[0].Cells[1].Value.ToString());

            button16.Enabled = true;
        }

        private void ShowComment(string code)
        {
            DataSet ds = SearchData(string.Format("select COMMENT from TB_SCRAP_COMMENT with(nolock) where [CUST]='{0}'", code));
            if (ds.Tables[0].Rows.Count > 0)
            {
                using (Form_Board board = new Form_Board(string.Format("고객 정보 : \r\n{0}", ds.Tables[0].Rows[0][0].ToString()), Color.Orange, Color.LightGray))
                {
                    board.ShowDialog();
                }
            }
            else
            {
                using (Form_Board board = new Form_Board(string.Format("고객 정보 : \r\nComment를 등록 하세요"), Color.Orange, Color.LightGray))
                {
                    board.ShowDialog();
                }
            }
        }

        string SelectedRequest = "";

        private string SelectRequest(List<string> RequestID, string msg)
        {
            Form_Request RequestSelecter = new Form_Request(RequestID, msg);
            RequestSelecter.PressOK_Event += RequestSelecter_PressOK_Event;
            RequestSelecter.PressCancel_Event += RequestSelecter_PressCancel_Event;
            SelectedRequest = "";

            RequestSelecter.ShowDialog();

            if (SelectedRequest == "")
            {
                MessageBox.Show("Request를 선택 하세요");
                return "EMPTY";
            }

            return SelectedRequest;
        }

        private void RequestSelecter_PressCancel_Event()
        {
            SelectedRequest = "EMPTY";
        }

        private void RequestSelecter_PressOK_Event(string RequestNum)
        {
            SelectedRequest = RequestNum;
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

        private ChromeDriverService _driverService = null;
        private ChromeOptions _options = null;
        private ChromeDriver _driver = null;
        string sUserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string sDownloadPath = "";
        string file_path = "";
        string file_name = "";
        string sScrapFileDIR = System.Windows.Forms.Application.StartupPath + "\\SCRAP";
        bool bDownloadComp = false;


        private void ScrapExcelDown()
        {
            string id = BankHost_main.strMESID;
            string pw = BankHost_main.strMESPW;
            string badge = BankHost_main.strID;
            sDownloadPath = Path.Combine(sUserPath, "Downloads");

            try
            {
                if (cb_download.Checked == false)
                {
                    bDownloadComp = false;

                    _driverService = ChromeDriverService.CreateDefaultService();
                    _driverService.HideCommandPromptWindow = true;

                    _options = new ChromeOptions();
                    _options.AddArgument("disable-gpu");

                    if (checkBox1.Checked == false)
                    {
                        _options.AddArgument("headless");
                        _options.AddUserProfilePreference("download.default_directory", sDownloadPath);
                        _options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
                    }


                    _driver = new ChromeDriver(_driverService, _options);
                    _driver.Navigate().GoToUrl("http://aak1ws01/eMES/index.jsp");  // 웹 사이트에 접속합니다. 
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                    progressBar1.Maximum = 15;
                    progressBar1.Value = 1;

                    SetProgressba("eMes에 접속 중입니다.", 1);
                    _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[3]/td[2]/p/font/span/input")).SendKeys(id);    // ID 입력          
                    _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[4]/td[2]/p/font/span/input")).SendKeys(pw);   // PW 입력            
                    _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[5]/td[2]/font/span/input")).SendKeys(badge);   // 사번 입력         
                    _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/p/input")).Click();   // Main 로그인 버튼            
                    SetProgressba("Login 확인 중", 2);



                    System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement> temp = _driver.FindElements(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/center/font"));



                    if (temp.Count != 0)
                    {
                        if (_driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/center/font")).Text == "Invalid Username or Password !!!")
                        {
                            MessageBox.Show("ID or 비밀번호 or 사번이 틀립니다.\n ID, 비밀번호, 사번을 확인해 주세요");
                            return;
                        }
                        else if (_driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/center/font")).Text == "User ID can't be used.")
                        {
                            MessageBox.Show("해당 ID로 접속 할 수 없습니다.\n ID 및 Network 상태를 점검해 주세요");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("알수 없는 에러가 발생하였습니다.");
                            return;
                        }
                    }

                    _driver.Navigate().GoToUrl("http://aak1ws01/eMES/diebank/PCSScrapRequest.jsp");   // Scrap request 항목으로 이동
                    SetProgressba("Scrap 메뉴로 이동 중입니다.", 3);


                    while (_driver.Url != "http://aak1ws01/eMES/diebank/PCSScrapRequest.jsp")
                    {
                        _driver.Navigate().GoToUrl("http://aak1ws01/eMES/diebank/PCSScrapRequest.jsp");   // Scrap request 항목으로 이동
                        Thread.Sleep(500);
                    }

                    SetProgressba("시작 날짜 설정", 4);
                    //_driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr[1]/td[2]/p/font/span/span/input[1]").Clear();   // 시작 날짜
                    //_driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr[1]/td[2]/p/font/span/span/input[1]").SendKeys(sdt.Value.ToString("yyyyMMdd"));

                    SetProgressba("종료 날짜 설정", 5);
                    //_driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr[1]/td[2]/p/font/span/span/input[3]").Clear();   // 종료 날짜
                    //_driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr[1]/td[2]/p/font/span/span/input[3]").SendKeys(edt.Value.ToString("yyyyMMdd"));

                    //_driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr[1]/td[2]/p/font/span/span/input[4]").Clear();   // 종료 시간
                    //_driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr[1]/td[2]/p/font/span/span/input[4]").SendKeys("235959");


                    //SetProgressba("ComboBox 설정", 6);
                    //_driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr[2]/td[4]/p/font/select").SendKeys("SCRAP"); // ComboBox 설정

                    SetProgressba("데이터 조회 중입니다.", 7);
                    //_driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td/div/table/tbody/tr/td[2]/p/a/img").Click();    //Find 버튼 누름
                    _driver.FindElement(By.Name("find")).Click();


                    ReadOnlyCollection<IWebElement> links = _driver.FindElements(By.TagName("a"));


                    SetProgressba("Excel File Down 중 입니다.", 8);
                    _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/div/table/tbody/tr/td[4]/a/img")).Click();  // Excel Down 누름                    

                    Thread.Sleep(1000);

                    System.IO.DirectoryInfo di = new DirectoryInfo(sDownloadPath);

                    FileInfo[] fi = di.GetFiles("*.*.crdownload");

                    DateTime dCrdownloadChecktime = DateTime.Now;

                    while (fi.Length != 0)
                    {
                        fi = di.GetFiles("*.*.crdownload");
                        Console.WriteLine((DateTime.Now - dCrdownloadChecktime).TotalSeconds);

                        if ((DateTime.Now - dCrdownloadChecktime).TotalSeconds >= 120)
                            SetProgressba("Download 시간을 초과 했습니다.", progressBar1.Maximum);
                        Thread.Sleep(100);
                    }

                    _driver.Close();

                    SetProgressba("Excel File Down 완료", 9);

                    fi = di.GetFiles("WaitingForScrap*.xls");

                    DateTime lastdate = new DateTime();

                    for (int i = 0; i < fi.Length; i++)
                    {
                        if (fi[i].CreationTime > lastdate)
                        {
                            file_path = fi[i].DirectoryName;
                            file_name = fi[i].Name;
                            lastdate = fi[i].CreationTime;

                            SetProgressba(String.Format("최신파일 검사중입니다 {0}/{1}", i, fi.Length), 10);
                        }
                    }

                    SetProgressba("Directory 확인중 입니다.", 11);

                    if (System.IO.Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\SCRAP") == false)
                    {
                        SetProgressba("Directory 생성 중 입니다.", 12);
                        System.IO.Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\SCRAP");
                    }

                    if (System.IO.File.Exists(file_path + "\\" + file_name) == true)
                    {
                        if (System.IO.File.Exists(System.Windows.Forms.Application.StartupPath + "\\SCRAP\\" + file_name) == true)
                        {
                            SetProgressba("기존 Excel File을 삭제 합니다.", 13);
                            System.IO.File.Delete(System.Windows.Forms.Application.StartupPath + "\\SCRAP\\" + file_name);
                        }
                        SetProgressba("Excel File을 복사 중 입니다.", 14);
                        System.IO.File.Move(file_path + "\\" + file_name, System.Windows.Forms.Application.StartupPath + "\\SCRAP\\" + file_name);
                    }
                    else
                    {
                        ReadScrapData();
                    }

                    bDownloadComp = true;

                    SetProgressba("Excel File 복사 완료하였습니다.", 15);
                }


                Thread tExcelImport = new Thread(ExcelImport);
                if (bDownloadComp == true || cb_download.Checked == true)
                    tExcelImport.Start();


            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147024864)   // 파일 사용 중
                {

                }
                else if (ex.HResult == -2146233088)  // eMes 응답 없음
                {

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

        public int run_count(string sql_str)
        {
            int res = -1;
            try
            {
                SqlConnection ssconn = new SqlConnection("server = 10.135.200.35; uid = amm; pwd = amm@123; database = GR_Automation");
                ssconn.Open();
                SqlCommand scom = new SqlCommand(sql_str, ssconn);
                scom.CommandType = System.Data.CommandType.Text;
                scom.CommandText = sql_str;
                res = (int)scom.ExecuteScalar();

                ssconn.Close();
                ssconn.Dispose();
                scom.Dispose();

                return res;
            }
            catch (Exception ex)
            {

            }

            return res;
        }

        private void SetProgressba(string msg, int val)
        {
            tb_ScrapSt.Text = msg;
            progressBar1.Value = val > progressBar1.Maximum ? progressBar1.Maximum : val;
        }

        private void SetWaferReturnProgressba(string msg, int val)
        {
            l_WaferReturnST.Text = msg;
            pb_WaferReturn.Value = val > pb_WaferReturn.Maximum ? pb_WaferReturn.Maximum : val;
        }

        private void Form_Sort_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            cb_dup.Checked = Properties.Settings.Default.LabelCopy;
            inputEmpNum.ReturnEmpnumEvent += InputEmpNum_ReturnEmpnumEvent;
            InfoBoard.Hide();

            readWASInsertFail();
            readWebFailData();
            SetWaferReturnControl(false);

        }

        private void SpeakST(string MSG)
        {
            tb_ScrapSt.Text = MSG;
            speech.SpeakAsync(MSG);
        }


        int ScrapMode = 0;

        private void tb_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                string[] inputstr = tb_scrapinput.Text.Split(':');   // 0: Lot, 1: Empty, 2: DEV, 3: QTY, 4: WFR, 5: ??, 6: CUST



                if (tb_scrapinput.Text == "")
                {
                    return;
                }

                int selectedindex = CheckScrapLOT(inputstr);
                tb_scrapinput.Text = "";

                if (dgv_scrap.RowCount == 0)
                {
                    SpeakST("검색을 먼저 진행해 주세요");

                    return;
                }


                if (selectedindex != -1)
                {
                    Color c = new Color();
                    if (dtScrap.Tables[0].Rows[selectedindex][6].ToString() != "" && dtScrap.Tables[0].Rows[selectedindex][7].ToString() != "")  // 검수 완료 된 자제
                    {// 검수 완료된 자네
                        SpeakST("완료된 자제");
                    }
                    else
                    {
                        if (dtScrap.Tables[0].Rows[selectedindex][6].ToString() == "" && dtScrap.Tables[0].Rows[selectedindex][7].ToString() == "")
                        {//1st
                            dtScrap.Tables[0].Rows[selectedindex][6] = string.Format("{0}({1})", BankHost_main.strOperator, BankHost_main.strID);
                            c = Color.Yellow;
                            dgv_scrap.Rows[selectedindex].DefaultCellStyle.BackColor = c;
                            SpeakST("일차 완료");

                            n1stCnt++;
                            ScrapDataUpdate(selectedindex);
                            dgv_scrap.Rows[selectedindex].Selected = true;
                            dgv_scrap.FirstDisplayedScrollingRowIndex = selectedindex;
                        }
                        else if (dtScrap.Tables[0].Rows[selectedindex][6].ToString() != "" && dtScrap.Tables[0].Rows[selectedindex][7].ToString() == "")
                        {//2nd
                            if (ScrapMode == 2)
                            {
                                if (dtScrap.Tables[0].Rows[selectedindex][6].ToString().Contains(BankHost_main.strID) == false)
                                {
                                    dtScrap.Tables[0].Rows[selectedindex][7] = string.Format("{0}({1})", BankHost_main.strOperator, BankHost_main.strID);
                                    c = Color.Green;
                                    dgv_scrap.Rows[selectedindex].DefaultCellStyle.BackColor = c;
                                    SpeakST("이차 완료");
                                    ScrapDataUpdate(selectedindex);
                                    dgv_scrap.Rows[selectedindex].Selected = true;
                                    dgv_scrap.FirstDisplayedScrollingRowIndex = selectedindex;
                                    n2ndCnt++;
                                }
                                else
                                {
                                    SpeakST("검수자 중복");
                                }
                            }
                            else
                            {
                                SetProgressba("1차 검수 완료 후 2차 검수 진행 할 수 있습니다.", 0);
                                SpeakST("1차 먼저 완료 해야 합니다.");
                            }
                        }

                        ScrapDataUpdate(selectedindex);
                    }
                }
                else
                {
                    SpeakST("스크랩 자재가 아닙니다.");
                    Form_Board warring = new Form_Board("스크랩 자재가 아닙니다.", Color.Black, Color.Red);
                    warring.ShowDialog();
                }

            }

            l1stComp.Text = n1stCnt.ToString();
            l2ndComp.Text = n2ndCnt.ToString();
        }


        private void ScrapDataUpdate(int index)
        {   // 0         1      2        3       4     5    6        7      8    9     10          11
            //[REQUEST],[CUST],[DEVICE],[P_D_L],[LOT],[DIE],[WAFER],[1st],[2nd],[3rd],[LOCATION],[CERITIFICATE]   
            string sqlstring = string.Format("update TB_SCRAP2 set [1st]='{0}',[2nd]='{1}' " +
                "where [CUST]={3} and [DEVICE]='{4}' and [LOT]='{5}' and [DIE]={6} and [WAFER]={7}",
                dtScrap.Tables[0].Rows[index][6],   //1
                dtScrap.Tables[0].Rows[index][7],   //2
                dtScrap.Tables[0].Rows[index][8],   //3
                dtScrap.Tables[0].Rows[index][1],   //CUST
                dtScrap.Tables[0].Rows[index][2],   //DEV//
                dtScrap.Tables[0].Rows[index][3],   //LOT
                dtScrap.Tables[0].Rows[index][4],   // DIE
                dtScrap.Tables[0].Rows[index][5]);  //WAFER

            run_sql_command(sqlstring);
        }
        private int CheckScrapLOT(string[] inputstr)
        {
            int res = -1;
            try
            {
                if (inputstr.Length < 7)
                    return res;

                for (int i = 0; i < dtScrap.Tables[0].Rows.Count; i++)
                {
                    if (int.Parse(dtScrap.Tables[0].Rows[i][1].ToString()) == int.Parse(inputstr[6]))    // CUST
                    {
                        string scrapTemp = dtScrap.Tables[0].Rows[i][3].ToString().Trim();

                        if (scrapTemp == inputstr[0].Trim())   // LOT
                        {
                            if (dtScrap.Tables[0].Rows[i][2].ToString().Trim() == inputstr[2].Trim())   // DEV
                            {
                                if (int.Parse(dtScrap.Tables[0].Rows[i][4].ToString().Trim()) == int.Parse(inputstr[3].Trim()))   // QTY
                                {
                                    if (int.Parse(dtScrap.Tables[0].Rows[i][5].ToString().Trim()) == int.Parse(inputstr[4].Trim()))   //WFR
                                    {
                                        res = i;
                                        return res;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return res;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            dgv_scrap.Rows[0].DefaultCellStyle.BackColor = Color.Yellow;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            ClickTime();
            ShowRequest("Excel 출력할 Request를 선택해 주세요.");
            string res = ScrapDataVaildation();

            if (res != "SUCCESS")
            {
                MessageBox.Show(string.Format("{0} 검수자 항목이 일치 하지 않습니다.\n확인 후 재 시도 하세요", res));
                return;
            }


            ScrapExcelExport();
        }


        DataGridView ScrapGrid = new DataGridView();

        private string ScrapDataVaildation()
        {
            string res = "SUCCESS";
            string s1st = "", s2nd = "", s3rd = "";


            for (int i = 0; i < dgv_scrap.ColumnCount; i++)
            {
                ScrapGrid.Columns.Add(dgv_scrap.Columns[i].Name, dgv_scrap.Columns[i].HeaderText);
            }
            //ScrapGrid.Rows.Clear();

            //for (int i = 0; i < dtScrap.Tables[0].Columns.Count; i++)
            //{
            //    ScrapGrid.Columns.Add(dtScrap.Tables[0].Columns[i].ColumnName, dtScrap.Tables[0].Columns[i].Caption);
            //}

            ScrapGrid.Rows.Clear();
            ScrapGrid.AllowUserToAddRows = false;

            for (int i = 0; i < dgv_scrap.Rows.Count; i++)
            {
                //      0       1      2         3     4    5       6      7    8     9        10           11
                // [REQUEST],[CUST],[DEVICE],[P_D_L],[LOT],[DIE],[WAFER],[1st],[2nd],[3rd],[LOCATION],[CERITIFICATE]
                if ((string)dtScrap.Tables[0].Rows[i][0] == RequestSelectNum)
                {
                    if (s1st != "" && s1st != (string)dtScrap.Tables[0].Rows[i][6])
                        return "1st";
                    else
                        s1st = (string)dtScrap.Tables[0].Rows[i][6];

                    if (s2nd != "" && s2nd != (string)dtScrap.Tables[0].Rows[i][7])
                        return "2nd";
                    else
                        s2nd = (string)dtScrap.Tables[0].Rows[i][7];



                    string[] rows = new string[dgv_scrap.ColumnCount];

                    for (int j = 0; j < dgv_scrap.ColumnCount; j++)
                    {
                        rows[j] = dgv_scrap.Rows[i].Cells[j].Value.ToString();
                    }

                    ScrapGrid.Rows.Add(rows);
                }
            }

            return res;
        }

        private List<string> GetScrapCommentList()
        {
            List<string> ltemp = new List<string>();

            string CustCode = ScrapGrid.Rows[0].Cells[1].Value.ToString();

            System.Data.DataSet dt = SearchData(string.Format("SELECT [COMMENT] FROM [GR_Automation].[dbo].[TB_SCRAP_COMMENT] with(nolock) where [CUST]= '{0}'", CustCode));

            //if(dt.Tables[0].Rows. == 0)
            //{
            //    MessageBox.Show(string.Format("고객 코드 : {0}\n에 등록된 Comment가 없습니다.", string.Join(",", CustCode)));
            //    return ltemp;
            //}

            if (dt.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show(string.Format("고객 코드 : {0}\n에 등록된 Comment가 없습니다.", string.Join(",", CustCode)));
                return ltemp;
            }

            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {

                ltemp.Add((string)dt.Tables[0].Rows[i][0]);
            }

            return ltemp;
        }


        private void copyfile(string scr, string des)
        {
            File.Copy(scr, des);
        }

        string SelectedComment = "";

        private async Task<string> ScrapExcelExport()
        {
            List<string> CommentTemp = GetScrapCommentList();

            if (CommentTemp.Count == 1)
            {
                SelectedComment = CommentTemp[0];
            }
            else
            {
                using (Form_CommentSelecter comment = new Form_CommentSelecter(CommentTemp))
                {
                    comment.UnSelect_event += Comment_UnSelect_event;
                    comment.SelectedComment_event += Comment_SelectedComment_event;

                    comment.ShowDialog();
                }
            }

            string DestFilePath = "";

            if (Properties.Settings.Default.SCRAP_EXCEL_EXPORT_PATH == "")
                DestFilePath = System.Windows.Forms.Application.StartupPath + "\\Scrap Validation\\" + String.Format("Scrap Validation_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd"));
            else
                DestFilePath = Properties.Settings.Default.SCRAP_EXCEL_EXPORT_PATH;

            saveFileDialog1.InitialDirectory = DestFilePath;
            saveFileDialog1.FileName = String.Format("Scrap Validation {0}_{1}.xlsx", RequestSelectNum, DateTime.Now.ToString("yyyyMMdd"));

            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                DestFilePath = saveFileDialog1.FileName;
                Properties.Settings.Default.SCRAP_EXCEL_EXPORT_PATH = string.Join("\\", saveFileDialog1.FileName.Split('\\'), 0, saveFileDialog1.FileName.Split('\\').Length - 1);
                Properties.Settings.Default.Save();

                if (DestFilePath.Substring(DestFilePath.Length - 4, 4) != "xlsx")
                {
                    DestFilePath = DestFilePath + ".xlsx";
                }

                if (System.IO.Directory.Exists(Properties.Settings.Default.SCRAP_EXCEL_EXPORT_PATH) == false)
                    System.IO.Directory.CreateDirectory(Properties.Settings.Default.SCRAP_EXCEL_EXPORT_PATH);


                //copyfile(System.Windows.Forms.Application.StartupPath + "\\Excel file\\Scrap Validation List.xlsx", DestFilePath);


                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                Workbook workbook = application.Workbooks.Open(System.Windows.Forms.Application.StartupPath + "\\Excel file\\Scrap Validation List.xlsx");
                Worksheet worksheet1 = workbook.Worksheets.get_Item(1);
                application.Visible = false;

                string[] saLotTemp;
                int totdie = 0, totwfr = 0;

                Excel.Range copyrow = worksheet1.Range["A5:I5"].EntireRow;


                //if (ScrapGrid.Rows.Count <= 10)
                {
                    //      0       1      2         3     4    5       6      7    8     9        10           11
                    // [REQUEST],[CUST],[DEVICE],[P_D_L],[LOT],[DIE],[WAFER],[1st],[2nd],[3rd],[LOCATION],[CERITIFICATE]
                    for (int i = 0; i < ScrapGrid.Rows.Count; i++)
                    {
                        saLotTemp = ((string)ScrapGrid.Rows[i].Cells[4].Value).Split('/');

                        if (i >= 10)
                        {
                            Excel.Range insetrow = worksheet1.Range[string.Format("A{0}", 4 + i)].EntireRow;
                            insetrow.Insert(Excel.XlInsertShiftDirection.xlShiftDown, copyrow.Copy(Type.Missing));
                        }

                        ((Range)worksheet1.Cells[(4 + i), 1]).Value2 = (string)ScrapGrid.Rows[i].Cells[0].Value;    // request#
                        ((Range)worksheet1.Cells[(4 + i), 2]).Value2 = (string)ScrapGrid.Rows[i].Cells[1].Value;    // cust code
                        ((Range)worksheet1.Cells[(4 + i), 3]).Value2 = (string)ScrapGrid.Rows[i].Cells[2].Value;    // device
                        ((Range)worksheet1.Cells[(4 + i), 4]).Value2 = saLotTemp[0].Trim();                            // lot#
                        ((Range)worksheet1.Cells[(4 + i), 5]).Value2 = saLotTemp.Length > 1 ? saLotTemp[1] : "";       //dcc
                        ((Range)worksheet1.Cells[(4 + i), 6]).Value2 = (string)ScrapGrid.Rows[i].Cells[4].Value;    // scrap die qty
                        ((Range)worksheet1.Cells[(4 + i), 7]).Value2 = (string)ScrapGrid.Rows[i].Cells[5].Value;    // wafer
                        ((Range)worksheet1.Cells[(4 + i), 8]).Value2 = (string)ScrapGrid.Rows[i].Cells[9].Value;    // location
                        ((Range)worksheet1.Cells[(4 + i), 9]).Value2 = "";    // status

                        totdie += int.Parse((string)ScrapGrid.Rows[i].Cells[4].Value);
                        totwfr += int.Parse((string)ScrapGrid.Rows[i].Cells[5].Value);

                        SetProgressba(string.Format("{0}번째 줄을 출력 중입니다.", i), i);
                    }

                    ((Range)worksheet1.Cells[4 + ScrapGrid.Rows.Count + 1, 2]).Value2 = String.Format("TOTAL LOT : {0}", ScrapGrid.Rows.Count);
                    ((Range)worksheet1.Cells[4 + ScrapGrid.Rows.Count + 1, 4]).Value2 = String.Format("TOTAL DIE Q'TY : {0}", totdie);
                    ((Range)worksheet1.Cells[4 + ScrapGrid.Rows.Count + 1, 6]).Value2 = String.Format("TOTAL WAFER Q'TY : {0}", totwfr);

                    ((Range)worksheet1.Cells[4 + ScrapGrid.Rows.Count + 8, 1]).Value2 = (string)ScrapGrid.Rows[0].Cells[6].Value;   //1st
                    ((Range)worksheet1.Cells[4 + ScrapGrid.Rows.Count + 8, 3]).Value2 = (string)ScrapGrid.Rows[0].Cells[7].Value;   //2nd  

                    ((Range)worksheet1.Cells[4 + ScrapGrid.Rows.Count + 5, 5]).Value2 = SelectedComment;
                }

                /*          
                else        // 10개 단위로 자름
                {
                    //Range sourceRange = worksheet1.get_Range("A1:I22");
                    //sourceRange.Copy();
                  

                    Excel.Range copycells = worksheet1.Range["A1:I25"].EntireRow;

                    int len = ScrapGrid.Rows.Count / 10;

                    for (int i = 1; i < len + 1; i++)
                    {
                        //Range last = worksheet1.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
                        //Range destinationRange = worksheet1.get_Range(string.Format("A{0}", 26 * i), last);
                        //destinationRange.PasteSpecial(XlPasteType.xlPasteFormats);

                        Excel.Range inserrow1 = worksheet1.Range[string.Format("A{0}", (25 * i)+1)].EntireRow;

                        inserrow1.Insert(Excel.XlInsertShiftDirection.xlShiftDown, copycells.Copy(Type.Missing));                    
                    }

                    
                    for (int i = 0; i < ScrapGrid.Rows.Count; i++)
                    {
                        saLotTemp = ((string)ScrapGrid.Rows[i].Cells[4].Value).Split('/');

                        ((Range)worksheet1.Cells[((i / 10) * 25) + 4 + (i % 10), 1]).Value2 = (string)ScrapGrid.Rows[i].Cells[0].Value;    // request#
                        ((Range)worksheet1.Cells[((i / 10) * 25) + 4 + (i % 10), 2]).Value2 = (string)ScrapGrid.Rows[i].Cells[1].Value;    // cust code
                        ((Range)worksheet1.Cells[((i / 10) * 25) + 4 + (i % 10), 3]).Value2 = (string)ScrapGrid.Rows[i].Cells[2].Value;    // device
                        ((Range)worksheet1.Cells[((i / 10) * 25) + 4 + (i % 10), 4]).Value2 = saLotTemp[0].Trim();                            // lot#
                        ((Range)worksheet1.Cells[((i / 10) * 25) + 4 + (i % 10), 5]).Value2 = saLotTemp.Length > 1 ? saLotTemp[1] : "";       //dcc
                        ((Range)worksheet1.Cells[((i / 10) * 25) + 4 + (i % 10), 6]).Value2 = (string)ScrapGrid.Rows[i].Cells[4].Value;    // scrap die qty
                        ((Range)worksheet1.Cells[((i / 10) * 25) + 4 + (i % 10), 7]).Value2 = (string)ScrapGrid.Rows[i].Cells[5].Value;    // wafer
                        ((Range)worksheet1.Cells[((i / 10) * 25) + 4 + (i % 10), 8]).Value2 = (string)ScrapGrid.Rows[i].Cells[9].Value;    // location
                        ((Range)worksheet1.Cells[((i / 10) * 25) + 4 + (i % 10), 9]).Value2 = "";    // status

                        totdie += int.Parse((string)ScrapGrid.Rows[i].Cells[4].Value);
                        totwfr += int.Parse((string)ScrapGrid.Rows[i].Cells[5].Value);
                    }

                    
                    for (int i = 0; i < len + 1; i++)
                    {
                        ((Range)worksheet1.Cells[i * 25 + 15, 2]).Value2 = String.Format("TOTAL LOT : {0}", ScrapGrid.Rows.Count);
                        ((Range)worksheet1.Cells[i * 25 + 15, 4]).Value2 = String.Format("TOTAL DIE Q'TY : {0}", totdie);
                        ((Range)worksheet1.Cells[i * 25 + 15, 6]).Value2 = String.Format("TOTAL WAFER Q'TY : {0}", totwfr);
                        ((Range)worksheet1.Cells[i * 25 + 22, 1]).Value2 = (string)ScrapGrid.Rows[0].Cells[6].Value;   //1st
                        ((Range)worksheet1.Cells[i * 25 + 22, 3]).Value2 = (string)ScrapGrid.Rows[0].Cells[7].Value;   //2nd 
                        ((Range)worksheet1.Cells[i * 25 + 19, 5]).Value2 = SelectedComment;
                    }
                }
                */

                worksheet1.SaveAs(DestFilePath);
                workbook.Close(false);
                workbook = null;
                application.Quit();
                application = null;

                SetProgressba("Excel 출력이 완료 되었습니다.", 100);
                MessageBox.Show("Scrap Vaildation File 출력이 완료 되었습니다.");
            }
            return "";
        }

        private void Comment_SelectedComment_event(string msg)
        {
            SelectedComment = msg;
        }

        private void Comment_UnSelect_event()
        {
            SelectedComment = "";
        }

        private void btn_ExcelOut_MouseDown(object sender, MouseEventArgs e)
        {
            btn_ExcelOut.BackColor = Color.CadetBlue;
        }

        private void btn_ExcelOut_MouseUp(object sender, MouseEventArgs e)
        {
            btn_ExcelOut.BackColor = Color.Transparent;
        }

        private void ShowRequest()
        {
            List<string> Request = new List<string>();

            for (int i = 0; i < dgv_scrap.RowCount; i++)
            {
                if (Request.Contains(dgv_scrap.Rows[i].Cells[0].Value.ToString()) == false)
                {
                    Request.Add(dgv_scrap.Rows[i].Cells[0].Value.ToString());
                }
            }


            Request.Add("DataBase");

            Form_Request re = new Form_Request(Request);
            re.PressCancel_Event += Re_PressCancel_Event;
            re.PressOK_Event += Re_PressOK_Event;

            re.ShowDialog();
        }

        private void ShowRequest(string msg)
        {
            List<string> Request = new List<string>();

            for (int i = 0; i < dgv_scrap.RowCount; i++)
            {
                if (Request.Contains(dgv_scrap.Rows[i].Cells[0].Value.ToString()) == false)
                {
                    Request.Add(dgv_scrap.Rows[i].Cells[0].Value.ToString());
                }
            }

            Form_Request re = new Form_Request(Request, msg);
            re.PressCancel_Event += Re_PressCancel_Event;
            re.PressOK_Event += Re_PressOK_Event;

            re.ShowDialog();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            ClickTime();
            ShowRequest();

            if (RequestSelectCancel == false)
            {
                //[REQUEST],[CUST],[DEVICE],[P_D_L],[LOT],[DIE],[WAFER],[1st],[2nd],[3rd],[LOCATION],[CERITIFICATE]
                //     0       1     2         3       4   5      6       7     8      9     10        11

                if (RequestSelectNum != "DataBase")
                {
                    List<string> custcode = new List<string>(), custname = new List<string>();
                    string weight = "", requestnum = RequestSelectNum;
                    int ttl = 0, wt = 0, qty = 0;

                    for (int i = 0; i < dgv_scrap.RowCount; i++)
                    {
                        if (dgv_scrap.Rows[i].Cells[0].Value.ToString() == RequestSelectNum)
                        {
                            if (custcode.Contains(dgv_scrap.Rows[i].Cells[1].Value.ToString()) == false)
                                custcode.Add(dgv_scrap.Rows[i].Cells[1].Value.ToString());

                            qty += 1;
                        }
                    }

                    custname = GetCustName(custcode);

                    //                         string CustCode , string CustName, string TTL, string WT, string Request, string QTY, string Weight
                    Form_InBill biil = new Form_InBill(custcode, custname, ttl.ToString(), wt.ToString(), requestnum, qty.ToString(), weight);
                    biil.Show();
                }
                else
                {
                    Form_ReceiptDB receiptDB = new Form_ReceiptDB();

                    receiptDB.ShowDialog();
                }

            }
        }

        private List<string> GetCustName(List<string> Codes)
        {
            List<string> res = new List<string>();
            string where = "";

            for (int i = 0; i < Codes.Count; i++)
            {
                if (i != Codes.Count - 1)
                {
                    where += string.Format("[CUST_CODE]={0} or ", Codes[i]);
                }
                else
                {
                    where += string.Format("[CUST_CODE]={0}", Codes[i]);
                }
            }

            string sql = string.Format("select [CUST_CODE], [CUST_NAME] from TB_SCRAP_CUST with(NOLOCK) where {0}", where);
            DataSet ds = SearchData(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (res.Contains(row[1].ToString().Split('_')[0]) == false)
                    res.Add(row[1].ToString().Split('_')[0]);
            }

            if (res.Count == 0)
                res.Add("EMPTY");

            return res;
        }

        private async Task<string> GetCustName(string Codes)
        {
            string res = "";
            string where = "";


            string sql = string.Format("select [CUST_CODE], [CUST_NAME] from TB_SCRAP_CUST with(NOLOCK) where [CUST_CODE]={0}", Codes);
            DataSet ds = SearchData(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                res = row[1].ToString().Split('_')[0].ToString();
            }

            return res;
        }

        bool RequestSelectCancel = false;
        string RequestSelectNum = "";

        private void Re_PressOK_Event(string RequestNum)
        {
            RequestSelectCancel = false;
            RequestSelectNum = RequestNum;
        }

        private void Re_PressCancel_Event()
        {
            RequestSelectNum = "";
            RequestSelectCancel = true;
        }

        private void btn_CommentEdit_Click(object sender, EventArgs e)
        {
            ClickTime();
            using (Form_ScrapComment comment = new Form_ScrapComment())
            {
                comment.ShowDialog();
            }
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            //dgv_scrap.Rows.Clear();
            bTimeOutSt = true;

            if (dtScrap != null)
            {
                ShowComment(dgv_scrap.Rows[0].Cells[1].Value.ToString());
                dtScrap.Tables[0].Rows.Clear();
            }

            tabControl_Sort.SelectedIndex = 0;
            //BankHost_main.strAdminID = "";
            //BankHost_main.strAdminPW = "";
            //BankHost_main.strAmkorID = "";
            //BankHost_main.strCust = "";
            //BankHost_main.strOperator = "";
            //BankHost_main.strID = "";



        }

        private void cbRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClickTime();
            if (DialogResult.Yes == MessageBox.Show("Request를 변경 하시겠습니까?", "Request 변경", MessageBoxButtons.YesNo, MessageBoxIcon.Information)) ;
            ReadScrapDBData(cbRequest.Text);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            ClickTime();
            ShowRequest("출력할 Request를 선택해 주세요");

            string CustNum = "";

            if (RequestSelectNum != "")
            {
                for (int i = 0; i < dgv_scrap.RowCount; i++)
                {
                    if (dgv_scrap.Rows[i].Cells[0].Value.ToString() == RequestSelectNum)
                    {
                        CustNum = dgv_scrap.Rows[i].Cells[1].Value.ToString();
                        break;
                    }
                }

                var taskResut = Task.Run(async () =>
                {
                    return await GetCustName(CustNum);
                });

                string custname = taskResut.Result;

                Frm_Print.Fnc_Print_MSG_1Line_Max(string.Format("Requset# : {0};{1}({2}) SCRAP", RequestSelectNum, custname, CustNum));
            }
        }

        bool bTimeOutSt = false;
        DateTime LastClickTime = new DateTime();

        private void ClickTime()
        {
            LastClickTime = DateTime.Now;
            bTimeOutSt = true;
        }

        private void bgw_timeout_DoWork(object sender, DoWorkEventArgs e)
        {

            TimeSpan Time_remaining = new TimeSpan();

            while (btimeOut)
            {
                if ((DateTime.Now - LastClickTime).TotalMinutes >= 0.5)//Properties.Settings.Default.TimeOutMin)
                {
                    int mode = comboBox_mode.SelectedIndex;
                    if (mode == 0 || mode == 1 || mode == 2 || mode == 3)
                    {
                        //inputEmpNum.setEmpNum(BankHost_main.strID);

                        //button19_Click_2(sender,e);
                        //inputEmpNum.ShowDialog();



                    }
                    else
                    {
                        bTimeOutSt = false;
                        BankHost_main.nWorkMode = 0;

                        ClickTime();
                        tabControl_Sort.SelectedIndex = 0;
                        BankHost_main.strAdminID = "";
                        BankHost_main.strAdminPW = "";
                        BankHost_main.strAmkorID = "";
                        BankHost_main.strCust = "";
                        BankHost_main.strOperator = "";
                        BankHost_main.strID = "";
                        break;
                    }
                    
                }
                else
                {
                    Time_remaining = TimeSpan.FromMinutes(Properties.Settings.Default.TimeOutMin) - (DateTime.Now - LastClickTime);
                    l_timouTime.Text = $": {Time_remaining.Minutes} : {Time_remaining.Seconds}";
                }

                System.Threading.Thread.Sleep(1000);
            }
        }


        private void InputEmpNum_ReturnEmpnumEvent(string empnum)
        {
            bTimeOutSt = false;
            BankHost_main.nWorkMode = 0;

            ClickTime();
            tabControl_Sort.SelectedIndex = 0;
            BankHost_main.strAdminID = "";
            BankHost_main.strAdminPW = "";
            BankHost_main.strAmkorID = "";
            BankHost_main.strCust = "";
            BankHost_main.strOperator = "";
            BankHost_main.strID = "";
        }

        private void cb_download_CheckStateChanged(object sender, EventArgs e)
        {
            ClickTime();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ClickTime();
        }

        private void dgv_split_log_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            ClickTime();
        }

        private void dgv_split_log_KeyDown(object sender, KeyEventArgs e)
        {
            tb_split.Text = e.KeyCode.ToString();
            tb_split.Select(textBox1.TextLength, 0);
            tb_split.Focus();

            if (GetIME() == true)
            {
                ChangeIME(tb_split);
            }
        }



        private void comboBox_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView_worklist.Columns.Clear();
            dataGridView_worklist.Rows.Clear();
            dataGridView_worklist.Refresh();

            BankHost_main.strOperator = "";
            label_opinfo.Text = "-";
            label_cust.Text = "-";
            strSelCust = "";

            int nSel = comboBox_mode.SelectedIndex;
            ClickTime();

            if (nSel == 0) //GR Mode
            {
                button_sel.Enabled = false;
                button_sel.Text = "GR 리스트 다운로드";

                if (!BankHost_main.bHost_connect)
                    return;

                string strMsg = string.Format("\n\n작업 정보를 가져 옵니다.");
                Frm_Process.Form_Show(strMsg);

                //var taskResut = Fnc_RunAsync( $"http://10.101.14.130:8180/eMES_Webservice/lot_info_list/getAutoGRLotListOnReadyStatus_eMES/{Properties.Settings.Default.LOCATION}");
                var taskResut = Fnc_RunAsync($"http://{(Properties.Settings.Default.TestMode == true ? TEST_MES : PRD_MES)}/eMES_Webservice/diebank_automation_service/inq_auto_gr_rdy_list/{Properties.Settings.Default.LOCATION}");
                try
                {
                    strMsg = string.Format("\n\n작업 정보를 분석 합니다.");
                    Frm_Process.Form_Display(strMsg);

                    if (taskResut.Status.ToString() == "Faulted")
                    {
                        strMsg = string.Format("작업 정보를 가져오는데 실패 하였습니다.");
                        Frm_Process.Form_Display_Warning(strMsg);
                        Thread.Sleep(3000);
                        Frm_Process.Form_Hide();

                        return;
                    }

                    int nCount = Fnc_Get_Worklist_2(taskResut.Result);

                    if (nCount > 0)
                        Fnc_Information_Init();
                }
                catch (Exception ex)
                {
                    string str = string.Format("{0}", ex);

                    strMsg = string.Format("작업 정보를 가져오는데 실패 하였습니다.");
                    Frm_Process.Form_Display_Warning(strMsg);

                    Thread.Sleep(3000);
                    Frm_Process.Form_Hide();
                }
            }
            else if (nSel == 1)
            {
                button_sel.Enabled = false;
                button_sel.Text = "GR Job File 선택";

                string strGetJobName = BankHost_main.Host.Host_Get_JobName(BankHost_main.strEqid);


                if (strGetJobName == "")
                {
                    MessageBox.Show("진행 중인 파일이 없습니다!");
                    return;
                }

                ///작업자 사번 입력 
                Form_Input Frm_Input = new Form_Input();

                //Frm_Input.Fnc_Init(nSel);
                Fnc_Information_Init2(1);
                //Frm_Input.ShowDialog();

                if (BankHost_main.strOperator == "")
                    return;

                label_opinfo.Text = BankHost_main.strOperator;

                strSelJobName = strSelJobName + ".txt";
                string strName = strSelJobName;
                if (strName.Length > 0)
                {
                    string str = strName.Substring(strName.Length - 3, 3);
                    if (str != "txt")
                    {
                        MessageBox.Show("JOB 파일이 아닙니다. 로드 실패!");
                        return;
                    }

                    string[] strSplit = strName.Split('\\');
                    int nLength = strSplit.Length;

                    strWorkFileName = strSplit[nLength - 1].Substring(0, strSplit[nLength - 1].Length - 4);
                    strWorkFileName = strWorkFileName.Trim();
                    Fnc_WorkView(strWorkFileName);

                    label_cust.Text = strSelCust;
                    Fnc_Get_Information_Model(strSelCust, comboBox_Name);

                    comboBox_Name.SelectedIndex = 0;
                }
            }
            else if (nSel == 2)
            {
                button_sel.Enabled = false;
                button_sel.Text = "Validation Webservice";

                strInputBill = "";
                ///작업자 사번 입력 
                Form_Input Frm_Input = new Form_Input();

                Frm_Input.Fnc_Init(nSel);
                Frm_Input.ShowDialog();

                if (BankHost_main.strOperator == "" || strInputBill == "")
                    return;

                label_opinfo.Text = BankHost_main.strOperator;
                if (!BankHost_main.bHost_connect)
                    return;

                string strMsg = string.Format("\n\n작업 정보를 가져 옵니다.");
                Frm_Process.Form_Show(strMsg);

                var taskResut = BankHost_main.Host.Fnc_GetBillInformation(Properties.Settings.Default.LOCATION, strInputBill);
                //var taskResut = Fnc_RunAsync($"http://{(Properties.Settings.Default.TestMode == true ? TEST_MES : PRD_MES)}/SmartConsoleWebService/lot_list/AutoGRLotList/{Properties.Settings.Default.LOCATION},{strInputBill}");
                try
                {
                    strMsg = string.Format("\n\n작업 정보를 분석 합니다.");
                    Frm_Process.Form_Display(strMsg);

                    string res = taskResut.Status.ToString();

                    //if (res == "Faulted")
                    //{
                    //    strMsg = string.Format("작업 정보를 가져오는데 실패 하였습니다.");
                    //    Frm_Process.Form_Display_Warning(strMsg);
                    //    Thread.Sleep(3000);
                    //    Frm_Process.Form_Hide();

                    //    return;
                    //}


                    int nCount = Fnc_Get_Worklist_3(taskResut.Result);

                    if (nCount < 1)
                    {
                        dataGridView_worklist.Columns.Clear();
                        dataGridView_worklist.Rows.Clear();
                        dataGridView_worklist.Refresh();
                    }
                    else
                    {
                        label_cust.Text = strSelCust;
                        Fnc_Get_Information_Model(strSelCust, comboBox_Name);

                        strSelBillno[0] = strInputBill;

                        if (strSelCust == "940")
                        {
                            Fnc_Set_Workfile_NoDevice(strSelBillno); //HY210315
                        }
                        else
                            Fnc_Set_Workfile(strSelBillno);

                        comboBox_Name.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    string str = string.Format("{0}", ex);

                    strMsg = string.Format("작업 정보를 가져오는데 실패 하였습니다.");
                    Frm_Process.Form_Display_Warning(strMsg);

                    Thread.Sleep(3000);
                    Frm_Process.Form_Hide();
                }
            }
            else if (nSel == 3)
            {
                button_sel.Enabled = true;
                button_sel.Text = "Validation 파일 선택";

                Fnc_Information_Init2(3);

                if (BankHost_main.strOperator == "")
                    return;



                label_opinfo.Text = BankHost_main.strOperator;
                label_cust.Text = strSelCust;

                string strGetJobName = BankHost_main.Host.Host_Get_JobName(BankHost_main.strEqid);

                strGetJobName = strSelJobName + ".txt";
                string strName = strGetJobName;
                if (strName.Length > 0)
                {
                    string str = strName.Substring(strName.Length - 3, 3);
                    if (str != "txt")
                    {
                        MessageBox.Show("JOB 파일이 아닙니다. 로드 실패!");
                        return;
                    }

                    string[] strSplit = strName.Split('\\');
                    int nLength = strSplit.Length;

                    strWorkFileName = strSplit[nLength - 1].Substring(0, strSplit[nLength - 1].Length - 4);
                    strWorkFileName = strWorkFileName.Trim();
                    Fnc_WorkView(strWorkFileName);

                    label_cust.Text = strSelCust;
                    Fnc_Get_Information_Model(strSelCust, comboBox_Name);

                    comboBox_Name.SelectedIndex = 0;
                }
            }
            else if (nSel == 4)
            {
                BankHost_main.nScanMode = 1;
                BankHost_main.bGunRingMode_Run = true;
                label_list.Clear();
                BankHost_main.nProcess = 4001;

                tabControl_Sort.SelectedIndex = 5;
                bselected_mode_index = true;
                textBox1.Focus();
                LastClickTime = DateTime.Now;
                runLogOutTimer();

                tot_lots = 0;
                tot_wfr = 0;
                tot_die = 0;

                lprinted_lots.Text = "0";
                ldie.Text = "0";
                lwfr.Text = "0";
                if (GetIME() == true)
                {
                    ChangeIME(textBox1);
                }
            }
            else if (nSel == 5)
            {
                if (Properties.Settings.Default.LOCATION == "K4")
                {
                    dgv_loc.Rows.Clear();

                    Form_Input Frm_Input = new Form_Input();

                    // strInputBill Bill# 입력 변수.

                    Frm_Input.Fnc_Init(nSel);
                    Frm_Input.ShowDialog();

                    if (BankHost_main.strOperator == "" || strInputBill == "")
                        return;

                    label_opinfo.Text = BankHost_main.strOperator;
                    if (!BankHost_main.bHost_connect)
                        return;

                    string strMsg = string.Format("\n\n작업 정보를 가져 옵니다.");
                    Frm_Process.Form_Show(strMsg);

                    var taskResut = Fnc_RunAsync("http://10.101.5.38:8080/EETPackingLabelValidation.asmx/BANKSplitLog?pPlant=K4");

                    try
                    {
                        strMsg = string.Format("\n\n작업 정보를 분석 합니다.");
                        Frm_Process.Form_Display(strMsg);

                        string res = taskResut.Result;

                        BankHost_main.Fnc_SaveLog("SplitLog Low Data", 1);
                        BankHost_main.Fnc_SaveLog(res, 1);
                        location_data_sorting(res);

                        saveFileDialog1.InitialDirectory = Properties.Settings.Default.Loc_file_save_path;

                        tabControl_Sort.SelectedIndex = 6;

                        bmode6 = true;
                        LastClickTime = DateTime.Now;
                        runLogOutTimer();
                        Frm_Process.Form_Hide();
                    }
                    catch (Exception ex)
                    {
                        string str = string.Format("{0}", ex);

                        strMsg = string.Format("작업 정보를 가져오는데 실패 하였습니다.");
                        Frm_Process.Form_Display_Warning(strMsg);

                        Thread.Sleep(3000);
                        Frm_Process.Form_Hide();
                    }
                }
                else if(Properties.Settings.Default.LOCATION == "K5")
                {
                    tabControl_Sort.SelectedIndex = 5;

                    tb_PreFix.Text = Properties.Settings.Default.ShelfPreFix;
                    tb_StartShelf.Text = Properties.Settings.Default.ShelfStartShelf;
                    tb_EndShelf.Text = Properties.Settings.Default.ShelfEndShelf;
                    tb_StartBox.Text = Properties.Settings.Default.ShelfStartBox;
                    tb_EndBox.Text = Properties.Settings.Default.ShelfEndBox;

                    cb_ShelfCust.SelectedIndex = Properties.Settings.Default.ShelfCust;
                    cb_ShelfCustName.SelectedIndex = Properties.Settings.Default.ShelfCustName;
                    cb_ShelfIgnoQTY.Checked = false;
                }
            }
            else if (nSel == 6)
            {
                dgv_split_log.Rows.Clear();

                string strMsg = string.Format("\n\n작업 정보를 가져 옵니다.");
                Frm_Process.Form_Show(strMsg);

                var taskResut = Fnc_RunAsync("http://10.101.5.38:8080/EETPackingLabelValidation.asmx/BANKSplitLog?pPlant=K4");

                try
                {
                    strMsg = string.Format("\n\n작업 정보를 분석 합니다.");
                    Frm_Process.Form_Display(strMsg);

                    string res = taskResut.Result;

                    BankHost_main.Fnc_SaveLog("Split Log Low Data", 1);
                    BankHost_main.Fnc_SaveLog(res, 1);
                    SplitLogFileSave(res);
                    Split_data_sorting(res);

                    saveFileDialog1.InitialDirectory = Properties.Settings.Default.SPLIT_LOG_SAVE_PATH;

                    bmode7 = true;
                    Frm_Process.Form_Hide();

                    Form_Splitlog_Input input = new Form_Splitlog_Input(split_log_cust, split_log_Linecode);
                    input.return_select_event += Input_return_select_event;
                    input.ShowDialog();

                    Split_data_display();

                    if (GetIME() == true)
                    {
                        ChangeIME(tb_split);
                    }
                    btn_CommentEdit.Text = "  Comment\nEdit";


                    tb_split.Focus();
                }
                catch (Exception ex)
                {
                    string str = string.Format("{0}", ex);

                    strMsg = string.Format("작업 정보를 가져오는데 실패 하였습니다.");
                    Frm_Process.Form_Display_Warning(strMsg);

                    Thread.Sleep(3000);
                    Frm_Process.Form_Hide();
                }
            }
            else if (nSel == 7)
            {
                BankHost_main.strOperator = "";
                dgv_split_log.Rows.Clear();

                Form_Input Frm_Input = new Form_Input();
                Frm_Input.Fnc_Init(nSel);
                Frm_Input.ShowDialog();

                if (BankHost_main.strOperator != "")
                {
                    bmode8 = true;
                    tabControl_Sort.SelectedIndex = 9;

                    if (GetIME() == true)
                    {
                        ChangeIME(tb_scrapinput);
                    }

                    LastClickTime = DateTime.Now;
                    runLogOutTimer();
                }
            }
            else if (nSel == 8)
            {
                BankHost_main.strOperator = "";
                Form_Input Frm_Input = new Form_Input();

                Frm_Input.Fnc_Init(7);
                Frm_Input.ShowDialog();

                tb_ReturnWafer.Text = Properties.Settings.Default.WaferReturnCode;

                if (BankHost_main.strOperator != "")
                {
                    bmode9 = true;
                    tabControl_Sort.SelectedIndex = 10;

                    cb_Qualcomm.Checked = Properties.Settings.Default.ReturnQualcomm;

                    if (GetIME() == true)
                    {
                        ChangeIME(tb_WaferReturnScan);
                    }

                    tb_Year.Text = DateTime.Now.Year.ToString();
                    tb_WaferReturnScan.Focus();

                    toolTip1.SetToolTip(btn_WaferReturnExcel, string.Format("{0}\n경로 변경 : 마우스 오른쪽 클릭", Properties.Settings.Default.WaferReturnExcelOutPath));

                    SetWaferReturnControl(true);

                    tb_MSL.Text = Properties.Settings.Default.QualcommMSL;
                    tb_2ndLI.Text = Properties.Settings.Default.Qualcomm2nd;



                    LastClickTime = DateTime.Now;

                    if (bgw_timeout.IsBusy == false)
                        runLogOutTimer();
                }
            }
            else if (nSel == 9)
            {
                tabControl_Sort.SelectedIndex = 13;
                
                tb_PreFix.Text = Properties.Settings.Default.ShelfPreFix;
                tb_StartShelf.Text = Properties.Settings.Default.ShelfStartShelf;
                tb_EndShelf.Text = Properties.Settings.Default.ShelfEndShelf;
                tb_StartBox.Text = Properties.Settings.Default.ShelfStartBox;
                tb_EndBox.Text = Properties.Settings.Default.ShelfEndBox;

                cb_ShelfCust.SelectedIndex = Properties.Settings.Default.ShelfCust;
                cb_ShelfCustName.SelectedIndex = Properties.Settings.Default.ShelfCustName;
                cb_ShelfIgnoQTY.Checked = false;
            }
            else if (nSel == 10)
            {
                tabControl_Sort.SelectedIndex = 13;

                tb_PreFix.Text = Properties.Settings.Default.ShelfPreFix;
                tb_StartShelf.Text = Properties.Settings.Default.ShelfStartShelf;
                tb_EndShelf.Text = Properties.Settings.Default.ShelfEndShelf;
                tb_StartBox.Text = Properties.Settings.Default.ShelfStartBox;
                tb_EndBox.Text = Properties.Settings.Default.ShelfEndBox;

                cb_ShelfCust.SelectedIndex = Properties.Settings.Default.ShelfCust;
                cb_ShelfCustName.SelectedIndex = Properties.Settings.Default.ShelfCustName;
                cb_ShelfIgnoQTY.Checked = true;
            }

            string strJudge = BankHost_main.Host.Host_Set_Ready(BankHost_main.strEqid, "WAIT", "");

            if (strJudge != "OK")
            {
                BankHost_main.bHost_connect = false;
                MessageBox.Show("DB 업데이트 실패!");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.GreenLabelPrint = cb_GreenLabel.Checked;
            Properties.Settings.Default.Save();
        }

        private void Input_return_select_event(string val)
        {
            label26.Text = "Line Code";
            split_log_input_return_val = val;
            label_cust.Text = val.Split(';')[0];
            comboBox_Name.Items.Clear();

            string[] temp = val.Split(';')[1].Split(':');
            if (label_cust.Text != "ALL")
            {
                comboBox_Name.Enabled = true;

                for (int i = 0; i < temp.Length; i++)
                {
                    comboBox_Name.Items.Add(temp[i]);

                    if (temp[0] == temp[i])
                    {
                        comboBox_Name.SelectedIndex = i - 1;
                    }
                }
            }
            else
            {
                comboBox_Name.Enabled = false;
            }

            BankHost_main.strOperator = label_opinfo.Text = val.Split(';')[2];
            BankHost_main.strOperator = label_opinfo.Text;
        }

        private void cb_dup_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LabelCopy = cb_dup.Checked;
            Properties.Settings.Default.Save();
        }

        private void tb_next_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void tb_next_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13)
            //{
            //    int i = 0;

            //    if (int.TryParse(tb_next.Text, out i) == true)
            //    {
            //        AmkorLabelCnt = i - 1;
            //    }
            //    else
            //    {
            //        Form_Board board = new Form_Board("숫자만 입력 가능 합니다.");

            //        board.ShowDialog();
            //    }
            //}
            //else
            //{
            //    int i = 0;

            //    if (int.TryParse(tb_next.Text, out i) == true)
            //    {
            //        AmkorLabelCnt = i - 1;
            //    }
            //    else
            //    {
            //        Form_Board board = new Form_Board("숫자만 입력 가능 합니다.");

            //        board.ShowDialog();
            //    }
            //}
        }

        private void tb_next_TextChanged(object sender, EventArgs e)
        {
            if (tb_next.Text != "")
            {
                int i = 0;

                if (int.TryParse(tb_next.Text, out i) == true)
                {
                    AmkorLabelCnt = i;
                }
                else
                {
                    Form_Board board = new Form_Board("숫자만 입력 가능 합니다.");

                    board.ShowDialog();
                }
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView_label_MouseClick(object sender, MouseEventArgs e)
        {

        }

        int AmkorLabelSelectedRow = -1;

        private void dataGridView_label_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AmkorLabelSelectedRow = e.RowIndex;
            if (e.Button == MouseButtons.Right)
            {
                AmkorLabelSelectedRow = e.RowIndex;

                CMS_AmkorLabel.Items.Clear();

                CMS_AmkorLabel.Items.Add("출력");
                CMS_AmkorLabel.Items[0].Click += Form_Sort_Click;

                CMS_AmkorLabel.PointToScreen(new System.Drawing.Point(e.X, e.Y));

                CMS_AmkorLabel.Show();

            }
        }

        private void Form_Sort_Click(object sender, EventArgs e)
        {
            stAmkor_Label temp = new stAmkor_Label();


            try
            {
                temp.Lot = dataGridView_label.Rows[AmkorLabelSelectedRow].Cells[1].Value.ToString();
                temp.DCC = dataGridView_label.Rows[AmkorLabelSelectedRow].Cells[2].Value.ToString();
                temp.Device = dataGridView_label.Rows[AmkorLabelSelectedRow].Cells[3].Value.ToString();
                temp.DQTY = dataGridView_label.Rows[AmkorLabelSelectedRow].Cells[4].Value.ToString();
                temp.WQTY = dataGridView_label.Rows[AmkorLabelSelectedRow].Cells[5].Value.ToString();
                temp.AMKOR_ID = dataGridView_label.Rows[AmkorLabelSelectedRow].Cells[6].Value.ToString();
                temp.CUST = dataGridView_label.Rows[AmkorLabelSelectedRow].Cells[7].Value.ToString();
                temp.Wafer_ID = dataGridView_label.Rows[AmkorLabelSelectedRow].Cells[8].Value.ToString();

                Frm_Print.Fnc_Print(temp, int.Parse(dataGridView_label.Rows[AmkorLabelSelectedRow].Cells[0].Value.ToString()), int.Parse(numericUpDown1.Value.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void location_data_sorting(string data)
        {
            try
            {
                List<string[]> location_list = new List<string[]>();

                string[] temp = data.Split('\n');

                for (int i = 0; i < temp.Length; i++)
                {
                    location_list.Add(temp[i].Split('\t'));
                }

                tot_die = 0;
                tot_wfr = 0;
                tot_lots = location_list.Count - 1;

                for (int i = 1; i < location_list.Count - 1; i++)
                {
                    dgv_loc.Rows.Add(location_list[i][0], location_list[i][1], location_list[i][9], "", "", location_list[i][3], location_list[i][4], location_list[i][5], location_list[i][6], location_list[i][7], location_list[i][8]);

                    if (dgv_loc.Rows[i - 1].Cells[2].Value.ToString() == "")
                    {
                        dgv_loc.Rows[i - 1].DefaultCellStyle.BackColor = Color.Yellow;
                        dgv_loc.Rows[i - 1].DefaultCellStyle.ForeColor = Color.Red;
                    }


                    tot_die += int.Parse(location_list[i][6]);
                    tot_wfr += int.Parse(location_list[i][7]);
                }

                dgv_loc.Sort(dgv_loc.Columns[2], ListSortDirection.Ascending);

                BankHost_main.Fnc_SaveLog(string.Format("TOT_Lot : {2}, TOT_Die : {0}, TOT_Wfr : {1}", tot_die, tot_wfr, tot_lots), 1);
                tot_lots--;

                tb_totaldie.Text = tot_die.ToString();
                tb_totalwafer.Text = tot_wfr.ToString();
                tb_totalot.Text = tot_lots.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void Refresh_split_lot_data()
        {


            //List<string[]> Split_list = new List<string[]>();
            //string strFileName = string.Format("{0}\\Work\\Split_log\\{1}.txt", strExcutionPath, DateTime.Now.ToShortDateString());

            //string[] temp = System.IO.File.ReadAllLines(strFileName);

            //dgv_split_log.Rows.Clear();

            //for (int i = 1; i < temp.Length; i++)
            //{
            //    string[] row = new string[14];
            //    string[] row_temp = temp[i].Split('\t');


            //    if (label_cust.Text != "ALL")
            //    {
            //        if (label_cust.Text == row_temp[1] && comboBox_Name.Text == row_temp[0])
            //        {
            //            row[0] = (dgv_split_log.RowCount + 1).ToString();
            //            for (int j = 0; j < row_temp.Length; j++)
            //            {
            //                if (row_temp[j] != null)
            //                    row[j + 1] = row_temp[j];
            //                else
            //                    row[j + 1] = "";
            //            }

            //            dgv_split_log.Rows.Add(row);

            //            if (row[11] == "COMPLETE")
            //            {
            //                dgv_split_log.Rows[dgv_split_log.RowCount - 1].DefaultCellStyle.BackColor = Color.Yellow;
            //                dgv_split_log.Rows[dgv_split_log.RowCount - 1].DefaultCellStyle.ForeColor = Color.Black;
            //            }
            //        }
            //    }
            //    else
            //    {

            //    }
            //    }
        }

        private void Set_split_lot_data()
        {
            List<string[]> Split_list = new List<string[]>();
            string strFileName = string.Format("{0}\\Work\\Split_log\\{1}.txt", strExcutionPath, DateTime.Now.ToShortDateString());

            string[] temp = System.IO.File.ReadAllLines(strFileName);

            dgv_split_log.Rows.Clear();
            dgv_split_log.Columns.Clear();


            dgv_split_log.Columns.Add("No", "No.");
            dgv_split_log.Columns.Add("Line", "Line");
            dgv_split_log.Columns.Add("Cust", "Cust");
            dgv_split_log.Columns.Add("Biunding", "Biunding#");
            dgv_split_log.Columns.Add("Device", "Device#");
            dgv_split_log.Columns.Add("Cust_Lot", "Cust Lot#");
            dgv_split_log.Columns.Add("Dcc", "Dcc");
            dgv_split_log.Columns.Add("Return_Qty", "Return Qty");
            dgv_split_log.Columns.Add("Return_Wafer", "Return Wafer");
            dgv_split_log.Columns.Add("Return_Date", "Return Date");
            dgv_split_log.Columns.Add("Loc", "Loc");
            dgv_split_log.Columns.Add("Status", "Status");
            dgv_split_log.Columns.Add("Oper", "Oper");
            dgv_split_log.Columns.Add("Scantime", "Scantime");

            dgv_split_log.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[12].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_split_log.Columns[13].SortMode = DataGridViewColumnSortMode.NotSortable;

            tot_die = 0;
            tot_wfr = 0;

            for (int i = 0; i < dataGridView_worklist.Rows.Count; i++)
            {
                string[] row = new string[14];
                row[0] = (i + 1).ToString();
                for (int j = 0; j < dataGridView_worklist.Rows[i].Cells.Count; j++)
                {
                    if (dataGridView_worklist.Rows[i].Cells[j].Value != null)
                        row[j + 1] = dataGridView_worklist.Rows[i].Cells[j].Value.ToString();
                    else
                        row[j + 1] = "";
                }

                dgv_split_log.Rows.Add(row);

                if (row[11] == "COMPLETE")
                {
                    dgv_split_log.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    dgv_split_log.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }

                tot_die += int.Parse(dataGridView_worklist.Rows[i].Cells[6].Value.ToString());
                tot_wfr += int.Parse(dataGridView_worklist.Rows[i].Cells[7].Value.ToString());

            }

            tot_lots = dgv_split_log.RowCount;

            dataGridView_worklist.Columns.Clear();
            dataGridView_worklist.Rows.Clear();

            tb_split_tot_lot.Text = tot_lots.ToString();
            tb_split_tot_die.Text = tot_die.ToString();
            tb_split_tot_wfr.Text = tot_wfr.ToString();
        }
        private void Split_data_display()
        {
            List<string[]> Split_list = new List<string[]>();
            string strFileName = string.Format("{0}\\Work\\Split_log\\{1}.txt", strExcutionPath, DateTime.Now.ToShortDateString());

            string[] temp = System.IO.File.ReadAllLines(strFileName);

            dataGridView_worklist.Columns.Clear();
            dataGridView_worklist.Rows.Clear();

            dataGridView_worklist.Columns.Add("Line", "Line");
            dataGridView_worklist.Columns.Add("Cust", "Cust");
            dataGridView_worklist.Columns.Add("Biunding", "Biunding#");
            dataGridView_worklist.Columns.Add("Device", "Device#");
            dataGridView_worklist.Columns.Add("Cust_Lot", "Cust Lot#");
            dataGridView_worklist.Columns.Add("Dcc", "Dcc");
            dataGridView_worklist.Columns.Add("Return_Qty", "Return Qty");
            dataGridView_worklist.Columns.Add("Return_Wafer", "Return Wafer");
            dataGridView_worklist.Columns.Add("Return_Date", "Return Date");
            dataGridView_worklist.Columns.Add("Loc", "Loc");
            dataGridView_worklist.Columns.Add("Status", "Status");
            dataGridView_worklist.Columns.Add("Oper", "Oper");

            dataGridView_worklist.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_worklist.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;

            if (label_cust.Text != "ALL")
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    string[] data_temp = temp[i].Split('\t');

                    if (data_temp[0] == comboBox_Name.Text && data_temp[1] == label_cust.Text)
                        dataGridView_worklist.Rows.Add(temp[i].Split('\t'));
                }
            }
            else
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Split('\t').Length > 10)
                    {
                        if (temp[i].Split('\t')[10] != "COMPLETE")
                            dataGridView_worklist.Rows.Add(temp[i].Split('\t'));
                    }
                    else
                    {
                        dataGridView_worklist.Rows.Add(temp[i].Split('\t'));
                    }
                }
            }
        }

        private void Split_data_sorting(string split_data)
        {
            try
            {
                List<string[]> Split_list = new List<string[]>();

                string[] temp = split_data.Split('\n');

                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] != "")
                    {
                        temp[i].Remove(temp[i].Length - 1, 1);
                        Split_list.Add(temp[i].Split('\t'));
                    }
                }

                for (int i = 1; i < Split_list.Count - 1; i++)
                {
                    split_log_lowdata.Add(string.Join(";", Split_list[i]));

                    if (split_log_cust.Contains(Split_list[i][1]) == false)
                    {
                        split_log_cust.Add(Split_list[i][1]);
                        split_log_Linecode.Add(Split_list[i][1] + ";" + Split_list[i][0]);
                    }
                    else
                    {
                        if (split_log_Linecode.Contains(Split_list[i][0]) == false)
                        {
                            split_log_Linecode.Add(Split_list[i][1] + ";" + Split_list[i][0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btn_WaferReturnFind_Click(object sender, EventArgs e)
        {
            ClickTime();

            if (bDownloadComp == true)
            {
                SetProgressba("조회를 시작 합니다.", 0);

                Thread ExcelDownThread = new Thread(WaferReturnExcelDown);
                ExcelDownThread.Start();
            }

            tb_WaferReturnScan.Focus();
        }

        void ChromeDriverUpdater()
        {
            string chromeversion_txt_path = $"{Application.StartupPath}\\chromedriver_version.txt";

            if (!System.IO.File.Exists(chromeversion_txt_path))
            {
                using (StreamWriter sw = new StreamWriter(System.IO.File.Open(chromeversion_txt_path, FileMode.Create), Encoding.UTF8))
                {
                }
            }

            System.Net.WebClient webClient = new WebClient();
            string install_version = System.IO.File.ReadAllText(chromeversion_txt_path);
            string chromedriver_version = webClient.DownloadString("https://chromedriver.storage.googleapis.com/LATEST_RELEASE");
            if (install_version != chromedriver_version)
            {
                DialogResult msgresult = MessageBox.Show("크롬 드라이버가 최신버전이 아닙니다.\n최신으로 업데이트 하시겠습니까?", "크롬 드라이버 업데이트", MessageBoxButtons.YesNo);
                if (msgresult == DialogResult.Yes)
                {
                    try
                    {
                        Console.WriteLine("크롬 드라이버를 다운로드 중 입니다.");
                        Console.WriteLine("잠시만 기다려 주세요.");
                        webClient.DownloadFile($"https://chromedriver.storage.googleapis.com/{chromedriver_version}/chromedriver_win32.zip", Application.StartupPath + @"\chromedriver_win32.zip");

                        ExtractZipfile(Application.StartupPath + @"\chromedriver_win32.zip", Application.StartupPath);
                        System.IO.File.Delete(Application.StartupPath + @"\chromedriver_win32.zip");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("크롬 드라이버 업데이트 완료!");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        System.IO.File.WriteAllText(chromeversion_txt_path, chromedriver_version);
                        MessageBox.Show("크롬 드라이버 업데이트가 완료되었습니다.");
                    }
                    catch
                    {
                        MessageBox.Show("크롬 드라이버 업데이트 중 오류가 발생했습니다.\n수동으로 업데이트 해주세요." + Environment.NewLine +
                                        "----수동 업데이트 방법----" + Environment.NewLine +
                                        "1. https://chromedriver.chromium.org/downloads 접속" + Environment.NewLine +
                                        "2. 파란색 큰 글씨로 된 ChromeDriver xx.x.xxxx.xx 형식 글씨 클릭" + Environment.NewLine +
                                        "3. chromedriver_win32.zip 다운로드 및 xx.x.xxxx.xx 로 된 버전내용 Ctrl+C(복사)" + Environment.NewLine +
                                        "4. 압축파일은 압축해제 후 프로그램 실행경로에 덮어쓰기" + Environment.NewLine +
                                        "   복사한 버전내용은 프로그램 실행경로에 chromedriver_version.txt 안에 붙여넣고 저장" + Environment.NewLine +
                                        "5. 프로그램 재 실행");
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("크롬 드라이버를 업데이트하지 않으면 프로그램을 이용할 수 없습니다.");
                    Application.Exit();
                }
            }
        }

        void ExtractZipfile(string sourceFilePath, string targetPath)
        {
            try
            {
                foreach (Process process in Process.GetProcessesByName("chromedriver"))
                {
                    process.Kill();
                }

                File.Delete(targetPath + @"\chromedriver.exe");
                System.IO.Compression.ZipFile.ExtractToDirectory(sourceFilePath, targetPath);
            }
            catch (Exception ex)
            {

            }

            //Encoding ibm437 = Encoding.GetEncoding("IBM437");
            //Encoding euckr = Encoding.GetEncoding("euc-kr");
            //using (ZipFile zip = new ZipFile(sourceFilePath))
            //{
            //    foreach (ZipEntry entry in zip.Entries)
            //    {
            //        byte[] ibm437_byte = ibm437.GetBytes(entry.FileName);
            //        string euckr_fileName = euckr.GetString(ibm437_byte);
            //        entry.FileName = euckr_fileName;
            //        entry.Extract(targetPath, ExtractExistingFileAction.OverwriteSilently);
            //    }
            //}
        }

        //private ChromeDriverService _driverService = null;
        //private ChromeOptions _options = null;
        //private ChromeDriver _driver = null;
        //string sUserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string sWaferReturnFileDIR = System.Windows.Forms.Application.StartupPath + "\\WaferReturn";
        //string sScrapFileDIR = System.Windows.Forms.Application.StartupPath + "\\SCRAP";
        bool bWaferReturnDownloadComp = false;

        private void WaferReturnExcelDown()
        {
            string id = BankHost_main.strMESID;
            string pw = BankHost_main.strMESPW;
            string badge = BankHost_main.strID;
            sDownloadPath = Path.Combine(System.Environment.CurrentDirectory, "WaferReturn\\Excel\\");



            try
            {
                ChromeDriverUpdater();

                //if (cb_WaferReturnExcel.Checked == false)
                {
                    if (System.IO.Directory.Exists(sDownloadPath) == false)
                    {
                        SetWaferReturnProgressba("Directory 생성 중 입니다.", 9);
                        System.IO.Directory.CreateDirectory(sDownloadPath);
                    }
                    else
                    {
                        try
                        {
                            System.IO.DirectoryInfo di1 = new System.IO.DirectoryInfo(sDownloadPath);

                            FileInfo[] fi1 = di1.GetFiles();

                            for (int i = 0; i < fi1.Length; i++)
                            {
                                fi1[i].Delete();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    bDownloadComp = false;

                    _driverService = ChromeDriverService.CreateDefaultService();

                    _driverService.HideCommandPromptWindow = true;

                    _options = new ChromeOptions();



                    _options.AddArgument("--disable-gpu");
                    _options.AddUserProfilePreference("download.default_directory", sDownloadPath);
                    _options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
                    _options.AddUserProfilePreference("safebrowsing.enabled", false);

                    /* test server
                    _driver = new ChromeDriver(_driverService, _options);
                    _driver.Navigate().GoToUrl("http://10.101.1.37:9080/eMES/");  // 웹 사이트에 접속합니다. 
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                    progressBar1.Maximum = 15;
                    progressBar1.Value = 1;

                    SetProgressba("eMes에 접속 중입니다.", 1);
                    _driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[3]/td[2]/p/font/span/input").SendKeys("abc4");    // ID 입력          
                    _driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[4]/td[2]/p/font/span/input").SendKeys("abc4");   // PW 입력            
                    _driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[5]/td[2]/font/span/input").SendKeys("362808");   // 사번 입력         
                    _driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/p/input").Click();   // Main 로그인 버튼            
                    SetProgressba("Login 확인 중", 2);

                    _driver.Navigate().GoToUrl("http://10.101.1.37:9080/eMES/diebank/PCSScrapRequest.jsp");   // Scrap request 항목으로 이동
                    SetProgressba("Scrap 메뉴로 이동 중입니다.", 3);


                    while (_driver.Url != "http://10.101.1.37:9080/eMES/diebank/PCSScrapRequest.jsp")
                    {
                        _driver.Navigate().GoToUrl("http://10.101.1.37:9080/eMES/diebank/PCSScrapRequest.jsp");   // Scrap request 항목으로 이동
                        Thread.Sleep(500);
                    }
                    */


                    _driver = new ChromeDriver(_driverService, _options);
                    _driver.Navigate().GoToUrl("http://aak1ws01/eMES/index.jsp");  // 웹 사이트에 접속합니다. 
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                    pb_WaferReturn.Maximum = 15;
                    pb_WaferReturn.Value = 1;

                    SetWaferReturnProgressba("eMes에 접속 중입니다.", 1);
                    _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[3]/td[2]/p/font/span/input")).SendKeys(id);    // ID 입력          
                    _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[4]/td[2]/p/font/span/input")).SendKeys(pw);   // PW 입력            
                    _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[5]/td[2]/font/span/input")).SendKeys(badge);   // 사번 입력         
                    _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/p/input")).Click();   // Main 로그인 버튼            
                    SetWaferReturnProgressba("Login 확인 중", 2);

                    System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement> temp = _driver.FindElements(By.XPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[1]/img"));

                    if (temp.Count != 0)
                    {
                        if (_driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/center/font")).Text == "Invalid Username or Password !!!")
                        {
                            MessageBox.Show("ID or 비밀번호 or 사번이 틀립니다.\n ID, 비밀번호, 사번을 확인해 주세요");
                            return;
                        }
                        else if (_driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/center/font")).Text == "User ID can't be used.")
                        {
                            MessageBox.Show("해당 ID로 접속 할 수 없습니다.\n ID 및 Network 상태를 점검해 주세요");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("알수 없는 에러가 발생하였습니다.");
                            return;
                        }
                    }

                    _driver.Navigate().GoToUrl("http://aak1ws01/eMES/diebank/ttsReturnFind.do");   // Scrap request 항목으로 이동
                    SetWaferReturnProgressba("Wafer Return 메뉴로 이동 중입니다.", 3);


                    while (_driver.Url != "http://aak1ws01/eMES/diebank/ttsReturnFind.do")
                    {
                        _driver.Navigate().GoToUrl("http://aak1ws01/eMES/diebank/ttsReturnFind.do");   // Scrap request 항목으로 이동
                        Thread.Sleep(500);
                    }

                    SetWaferReturnProgressba("년도 설정", 4);
                    _driver.FindElement(By.XPath("/html/body/form[1]/table/tbody/tr[2]/td/table/tbody/tr[4]/td[2]/input")).Clear();
                    _driver.FindElement(By.XPath("/html/body/form[1]/table/tbody/tr[2]/td/table/tbody/tr[4]/td[2]/input")).SendKeys(tb_Year.Text);

                    SetWaferReturnProgressba("데이터 조회 중입니다.", 7);
                    _driver.FindElement(By.XPath("/html/body/form[1]/table/tbody/tr[3]/td/div/table/tbody/tr/td/p/span/b/font/input")).Click();    //Find 버튼 누름
                    //_driver.FindElementByName("find").Click();


                    temp = _driver.FindElements(By.Name("selected"));

                    if (temp.Count == 0)
                    {
                        SetWaferReturnProgressba("조회된 데이터가 없습니다.", 100);
                        return;
                    }
                    else
                    {
                        temp = _driver.FindElements(By.PartialLinkText("K4"));

                        WaferReturnInfo = new List<stWaferReturnInfo>();

                        for (int i = 0; i < temp.Count; i++)
                        {
                            WaferReturnInfo.Add(new stWaferReturnInfo());
                        }

                        WaferReturnDataSort(_driver.FindElement(By.XPath("/html/body/form[2]/table")));
                    }

                    SetWaferReturnProgressba("Directory 확인중 입니다.", 8);



                    _driver.FindElement(By.Name("checkAll")).Click();

                    SetWaferReturnProgressba("Excel File Down 중 입니다.", 10);
                    _driver.FindElement(By.Name("excelDisplay")).Click();       //Excel Down Click

                    Thread.Sleep(1000);

                    System.IO.DirectoryInfo di = new DirectoryInfo(sDownloadPath);

                    FileInfo[] fi = di.GetFiles("*.*.crdownload");

                    DateTime dCrdownloadChecktime = DateTime.Now;

                    while (fi.Length != 0)
                    {
                        fi = di.GetFiles("*.*.crdownload");
                        Console.WriteLine((DateTime.Now - dCrdownloadChecktime).TotalSeconds);

                        if ((DateTime.Now - dCrdownloadChecktime).TotalSeconds >= 120)
                            SetWaferReturnProgressba("Download 시간을 초과 했습니다.", progressBar1.Maximum);
                        Thread.Sleep(100);
                    }


                    while (!File.Exists(sDownloadPath + @"\WaferReturnList.xls"))
                    {
                        Thread.Sleep(1000); // Wait for 1 second
                    }


                    SetWaferReturnProgressba("Excel File Down 완료", 9);
                    _driver.Close();


                    fi = di.GetFiles();

                    DateTime lastdate = new DateTime();

                    for (int i = 0; i < fi.Length; i++)
                    {
                        if (fi[i].CreationTime > lastdate)
                        {
                            file_path = fi[i].DirectoryName;
                            file_name = fi[i].Name;
                            lastdate = fi[i].CreationTime;

                            SetWaferReturnProgressba(String.Format("최신파일 검사중입니다 {0}/{1}", i, fi.Length), 10);
                        }
                    }
                    WriteWaferReturnData();
                    //ReadScrapData();

                    bDownloadComp = true;

                    //SetWaferReturnProgressba("Excel File 복사 완료하였습니다.", 15);

                    //button19_Click(btn_WaferReturnFind, new EventArgs());
                }




            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147024864)   // 파일 사용 중
                {

                }
                else if (ex.HResult == -2146233088)  // eMes 응답 없음
                {

                }
                else if (ex.Message.Contains("The chromedriver.exe file does not exist in the current directory") == true)
                {
                    SetWaferReturnProgressba("Chromedriver 파일을 찾을 수 없습니다.", 0);
                }

            }
        }

        private void WriteWaferReturnData()
        {
            //if (cb_WaferReturnExcel.Checked == false)
            {
                SetProgressba("Excel Data를 Memory에 복사 중 입니다.", 1);
                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                Workbook workbook = application.Workbooks.Open(Filename: sDownloadPath + "\\" + file_name);
                Worksheet worksheet1 = workbook.Worksheets.get_Item(1);
                application.Visible = checkBox1.Checked;
                SetProgressba("Excel Data를 Memory에 복사 완료 하였습니다.", 2);


                Excel.Range range = worksheet1.UsedRange;

                List<string> data = new List<string>();
                string excelrow = "";



                int HeaderRow = -1;
                int returnNumCnt = -1;
                int seqCnt = -1;
                int rowOffset = 4;

                stWaferReturnExcelInfo excelInfo = new stWaferReturnExcelInfo();
                string[] temp1 = new string[11];

                object[,] excelData = new object[range.Rows.Count, range.Columns.Count];

                excelData = (object[,])range.Value2;
                pb_WaferReturn.Maximum = range.Rows.Count;
                SetWaferReturnProgressba("", 0);

                for (int i = 1; i <= range.Rows.Count - 2; ++i)
                {
                    excelrow = "";

                    if (excelData[i, 1].ToString() == "WAFER RETURN LIST")
                    {
                        HeaderRow = i;
                        string cust = excelData[HeaderRow + 2, 1].ToString().Split(':')[1];
                        ++returnNumCnt;
                        seqCnt = -1;
                        rowOffset = 5;

                        if (WaferReturnInfo.Count < returnNumCnt + 1)
                        {
                            stWaferReturnInfo test = new stWaferReturnInfo();
                            test.ExcelInfo = new List<stWaferReturnExcelInfo>();
                            //test.ExcelInfo = new List<stWaferReturnExcelInfo>();
                            WaferReturnInfo.Add(new stWaferReturnInfo());
                            WaferReturnInfo[WaferReturnInfo.Count - 1] = test;
                        }

                        while (excelData[HeaderRow + rowOffset, 1] != null)
                        {
                            temp1[0] = excelData[HeaderRow + rowOffset, 1] == null ? "" : excelData[HeaderRow + rowOffset, 1].ToString();
                            temp1[1] = excelData[HeaderRow + rowOffset, 2] == null ? "" : excelData[HeaderRow + rowOffset, 2].ToString();
                            temp1[2] = excelData[HeaderRow + rowOffset, 3] == null ? "" : excelData[HeaderRow + rowOffset, 3].ToString();
                            temp1[3] = excelData[HeaderRow + rowOffset, 4] == null ? "" : excelData[HeaderRow + rowOffset, 4].ToString();
                            temp1[4] = excelData[HeaderRow + rowOffset, 5] == null ? "" : excelData[HeaderRow + rowOffset, 5].ToString();
                            temp1[5] = excelData[HeaderRow + rowOffset, 6] == null ? "" : excelData[HeaderRow + rowOffset, 6].ToString();
                            temp1[6] = excelData[HeaderRow + rowOffset, 7] == null ? "" : excelData[HeaderRow + rowOffset, 7].ToString();
                            temp1[7] = excelData[HeaderRow + rowOffset, 8] == null ? "" : excelData[HeaderRow + rowOffset, 8].ToString();
                            temp1[8] = excelData[HeaderRow + rowOffset, 9] == null ? "" : excelData[HeaderRow + rowOffset, 9].ToString();
                            temp1[9] = excelData[HeaderRow + rowOffset, 10] == null ? "" : excelData[HeaderRow + rowOffset, 10].ToString();
                            temp1[10] = excelData[HeaderRow + rowOffset, 11] == null ? "" : excelData[HeaderRow + rowOffset, 11].ToString();

                            excelInfo.Setdata(temp1[0],
                                temp1[1],
                                temp1[2],
                                temp1[3],
                                temp1[4],
                                temp1[5],
                                int.Parse(temp1[6] ?? "0"),
                                int.Parse(temp1[7] ?? "0"),
                                temp1[8],
                                temp1[9],
                                temp1[10],
                                cust
                                );

                            if (WaferReturnInfo[returnNumCnt].ExcelInfo == null)
                                WaferReturnInfo[returnNumCnt].ExcelInfoInit();

                            WaferReturnInfo[returnNumCnt].ExcelInfo.Add(excelInfo);

                            ++seqCnt;
                            ++rowOffset;

                            SetWaferReturnProgressba(string.Format("{0},{1},{2},{3}", temp1[3], temp1[4], temp1[6], temp1[7]), HeaderRow + rowOffset);

                            if (HeaderRow + rowOffset > range.Rows.Count)
                            {
                                break;
                            }
                        }
                        i = HeaderRow + rowOffset;
                    }
                }

                SetWaferReturnProgressba("Excel Read Complete", range.Rows.Count);

                /*메모리 할당 해제*/
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(worksheet1);
                workbook.Close();
                Marshal.ReleaseComObject(workbook);
                application.Quit();
                Marshal.ReleaseComObject(application);

                WaferReturnData2DB();
            }
        }

        private void WaferReturnData2DB()
        {
            string query = "";
            int max = 0;
            int cnt = 0;
            int passCnt = 0;
            int insertCnt = 0;


            try
            {
                for (int i = 0; i < WaferReturnInfo.Count; i++)
                {
                    if (WaferReturnInfo[i].ExcelInfo != null)
                        max += WaferReturnInfo[i].ExcelInfo.Count;
                }


                pb_WaferReturn.Maximum = max;

                for (int i = 0; i < WaferReturnInfo.Count; i++)
                {


                    //if (ds.Tables[0].Rows.Count == 0)
                    {
                        if (WaferReturnInfo[i].ExcelInfo != null)
                        {

                            for (int j = 0; j < WaferReturnInfo[i].ExcelInfo.Count; j++)
                            {
                                string q = string.Format("select [RETURN_NO] from [TB_RETURN_WAFER] with(nolock) where [RETURN_NO]= '{0}-{1}' and [SEQ]='{2}' and [LOT]='{3}'", tb_Year.Text, WaferReturnInfo[i].WebInfo.ReturnNum == null ? WaferReturnInfo[i].ExcelInfo[j].ReturnNum.Split('-')[1] : WaferReturnInfo[i].WebInfo.ReturnNum, WaferReturnInfo[i].ExcelInfo[j].Seq, WaferReturnInfo[i].ExcelInfo[j].LotNum);
                                DataSet ds = SearchData(q);

                                Thread.Sleep(10);


                                if (ds.Tables[0].Rows.Count == 0)
                                {
                                    ++insertCnt;



                                    string re = WaferReturnInfo[i].ExcelInfo[j].ReturnNum;//tb_Year.Text + "-" + WaferReturnInfo[i].WebInfo.ReturnNum == null ? WaferReturnInfo[i].ExcelInfo[j].ReturnNum.Split('-')[1] : WaferReturnInfo[i].WebInfo.ReturnNum;
                                    query = String.Format("Insert INTO TB_RETURN_WAFER values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, {7}, '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', {16}, '{17}', '{18}', '{19}', {20}, '{21}', '{22}')",
                                    re,
                                    WaferReturnInfo[i].ExcelInfo[j].Seq,
                                    WaferReturnInfo[i].ExcelInfo[j].PDL,
                                    WaferReturnInfo[i].ExcelInfo[j].DeviceName,
                                    WaferReturnInfo[i].ExcelInfo[j].LotNum,
                                    WaferReturnInfo[i].ExcelInfo[j].Dcc,
                                    WaferReturnInfo[i].ExcelInfo[j].DsQty,
                                    WaferReturnInfo[i].ExcelInfo[j].ReturnQty,
                                    WaferReturnInfo[i].ExcelInfo[j].Remark,
                                    WaferReturnInfo[i].ExcelInfo[j].Loc,
                                    WaferReturnInfo[i].ExcelInfo[j].SL,
                                    "",
                                    "",
                                    WaferReturnInfo[i].WebInfo.InputDate == null ? "" : WaferReturnInfo[i].WebInfo.InputDate,
                                    WaferReturnInfo[i].WebInfo.RequestDate == null ? "" : WaferReturnInfo[i].WebInfo.RequestDate,
                                    WaferReturnInfo[i].WebInfo.UserID == null ? "" : WaferReturnInfo[i].WebInfo.UserID,
                                    WaferReturnInfo[i].WebInfo.BoxQty,
                                    WaferReturnInfo[i].WebInfo.Remark == null ? "" : WaferReturnInfo[i].WebInfo.Remark,
                                    "",
                                    "",
                                    WaferReturnInfo[i].WebInfo.CustCode == null ? WaferReturnInfo[i].ExcelInfo[j].cust.ToString() : WaferReturnInfo[i].WebInfo.CustCode,
                                    "",
                                    ""
                                    );

                                    run_sql_command(query);
                                    SetWaferReturnProgressba(string.Format("Add : {0}, {1}", WaferReturnInfo[i].ExcelInfo[j].Seq, WaferReturnInfo[i].ExcelInfo[j].LotNum), ++cnt);
                                }
                                else
                                {
                                    ++passCnt;
                                    SetWaferReturnProgressba(string.Format("Pass:{0},{1}", WaferReturnInfo[i].ExcelInfo[j].Seq, WaferReturnInfo[i].ExcelInfo[j].LotNum), ++cnt);
                                }

                                Thread.Sleep(10);
                            }
                        }
                        else
                        {

                        }
                    }
                    //else
                    //{
                    //    cnt += WaferReturnInfo[i].ExcelInfo.Count;
                    //    SetWaferReturnProgressba(string.Format("{0} is Exist", WaferReturnInfo[i].WebInfo.ReturnNum), cnt);
                    //}
                }

                SetWaferReturnProgressba(string.Format("Insert:{0},Pass:{1}", insertCnt, passCnt), pb_WaferReturn.Maximum);
            }
            catch (Exception ex)
            {

            }

        }

        private void WaferReturnDataSort(IWebElement webElement)
        {
            string[] tableText = webElement.Text.Replace("\r", "").Split('\n');

            for (int i = 0; i < WaferReturnInfo.Count; i++)
            {
                stWaferReturnInfo returnInfo = new stWaferReturnInfo();

                //string cust, string st, string returncode, string indate, string redate, string id, int qty, string remark
                stWaferReturnWebInfo webInfo = new stWaferReturnWebInfo();
                webInfo.CustCode = tableText[11 + (i * 6)].Trim();
                webInfo.Status = tableText[12 + (i * 6)].Trim();
                webInfo.ReturnNum = tableText[13 + (i * 6)].Trim();
                webInfo.InputDate = tableText[14 + (i * 6)].Trim();
                webInfo.RequestDate = tableText[15 + (i * 6)].Trim();
                webInfo.UserID = tableText[16 + (i * 6)].Trim();
                webInfo.BoxQty = -1;
                webInfo.Remark = "";

                returnInfo.WebInfo = webInfo;
                returnInfo.ExcelInfo = new List<stWaferReturnExcelInfo>();

                WaferReturnInfo[i] = returnInfo;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            dgv_ReturnWafer.DataSource = null;

            string temp = string.Format("select [SEQ],[DEVICE_NAME],[LOT],[DCC],[RETURN_QTY],[LOC],[SL],[REMARK],[SCAN_TIME_1st],[SCAN_USER_NAME_1st],[SCAN_TIME_2nd],[SCAN_USER_NAME_2nd],[AMKOR_ID],[CUST_CODE] from TB_RETURN_WAFER with(nolock) where [RETURN_NO]='{0}-{1}{2}' order by cast([SEQ] as int)", tb_Year.Text, tb_ReturnWafer.Text, Properties.Settings.Default.LOCATION);
            dgv_ReturnWafer.DataSource = SearchData(temp).Tables[0];

            for (int i = 0; i < dgv_ReturnWafer.RowCount; i++)
            {
                if (dgv_ReturnWafer.Rows[i].Cells[9].Value.ToString() != "" && dgv_ReturnWafer.Rows[i].Cells[11].Value.ToString() == "")
                    dgv_ReturnWafer.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                else if (dgv_ReturnWafer.Rows[i].Cells[9].Value.ToString() != "" && dgv_ReturnWafer.Rows[i].Cells[11].Value.ToString() != "")
                    dgv_ReturnWafer.Rows[i].DefaultCellStyle.BackColor = Color.Blue;
            }

            bWaferReturnNumChange = false;

            tb_WaferReturnScan.Focus();
        }

        private stAmkor_Label transCode(string msg)
        {
            string[] temp = msg.Split(',');
            stAmkor_Label codeInfo = new stAmkor_Label();

            if (checkFG(msg) == true)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Substring(0, 2) == "1J") { }
                    //LPN = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 2) == "1T")
                        codeInfo.Lot = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 2) == "1P")
                        codeInfo.Wafer_ID = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 2) == "9D")
                        codeInfo.DCC = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 1) == "Q")
                        codeInfo.DQTY = temp[i].Substring(1, temp[i].Length - 1);
                }
            }
            else
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Substring(0, 2) == "1J")
                    { }// LPN = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 2) == "1T")
                        codeInfo.Lot = temp[i].Substring(2, temp[i].Length - 2);
                    else if (temp[i].Substring(0, 3) == "30T")
                        codeInfo.Wafer_ID = temp[i].Substring(3, temp[i].Length - 3);
                    else if (temp[i].Substring(0, 3) == "10D")
                    { }//codeInfo.DCC = temp[i].Substring(3, temp[i].Length - 3);
                    else if (temp[i].Substring(0, 3) == "14D")
                    { }// codeInfo.Exp = temp[i].Substring(3, temp[i].Length - 3);
                    else if (temp[i].Substring(0, 1) == "Q")
                        codeInfo.DQTY = temp[i].Substring(1, temp[i].Length - 1);
                    else if (temp[i].Substring(0, 1) == "P")
                        codeInfo.Device = temp[i].Substring(1, temp[i].Length - 1);// MCN = temp[i].Substring(1, temp[i].Length - 1);
                }
            }

            //bool pass = check_WaferReturnDuplicate(codeInfo);

            //if(pass == false)
            //{

            //}

            return codeInfo;
        }


        private bool checkFG(string msg)
        {
            bool res = false;

            string[] temp = msg.Split(':');

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Substring(0, 2) == "9D")
                {
                    res = true;
                    break;
                }
            }

            return res;
        }

        private void tb_WaferReturnScan_KeyDown(object sender, KeyEventArgs e)
        {
            InfoBoard.Hide();

            if (Convert.ToInt32(e.KeyCode) == 13)
            {
                ClickTime();
                if (cb_Qualcomm.Checked == false)
                {
                    WaferReturn_label_Print_Process(tb_WaferReturnScan.Text.ToUpper(), 1);

                }
                else if (cb_Qualcomm.Checked == true)
                {
                    //transCode(tb_WaferReturnScan.Text.ToUpper());
                    WaferReturn_label_Print_Process(tb_WaferReturnScan.Text.ToUpper(), 1);
                }

                tb_WaferReturnScan.Text = "";
            }
        }

        int WaferReturnSelectedRow = -1;

        private void dgv_ReturnWafer_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            WaferReturnSelectedRow = e.RowIndex;
            if (e.Button == MouseButtons.Right)
            {
                WaferReturnSelectedRow = e.RowIndex;

                WaferReturnMenu.Items.Clear();

                WaferReturnMenu.Items.Add("출력");
                WaferReturnMenu.Items[0].Click += WaferReturnLabelPrint;

                WaferReturnMenu.PointToScreen(new System.Drawing.Point(e.X, e.Y));


                WaferReturnMenu.Show(Control.MousePosition);

            }
        }

        private void WaferReturnLabelPrint(object sender, EventArgs e)
        {
            stAmkor_Label temp = new stAmkor_Label();


            try
            {
                //   0         1         2     3      4          5     6     7        8             9
                // [SEQ],[DEVICE_NAME],[LOT],[DCC],[RETURN_QTY],[LOC],[SL],[REMARK],[SCAN_TIME],[SACN_USER_NAME]

                if (dgv_ReturnWafer.Rows[WaferReturnSelectedRow].DefaultCellStyle.BackColor == Color.Blue)
                {
                    temp.Lot = dgv_ReturnWafer.Rows[WaferReturnSelectedRow].Cells[2].Value.ToString();
                    temp.DCC = dgv_ReturnWafer.Rows[WaferReturnSelectedRow].Cells[3].Value.ToString();
                    temp.Device = dgv_ReturnWafer.Rows[WaferReturnSelectedRow].Cells[1].Value.ToString();
                    temp.DQTY = dgv_ReturnWafer.Rows[WaferReturnSelectedRow].Cells[4].Value.ToString();
                    temp.CUST = dgv_ReturnWafer.Rows[WaferReturnSelectedRow].Cells[13].Value.ToString();
                    temp.AMKOR_ID = dgv_ReturnWafer.Rows[WaferReturnSelectedRow].Cells[12].Value.ToString();
                    temp.Wafer_ID = "";//dgv_ReturnWafer.Rows[WaferReturnSelectedRow].Cells[8].Value.ToString();
                    temp.WQTY = "1";

                    string inputData = "";
                    int cnt = -1;
                    InputBox("순번입력", "번호", ref inputData);

                    if (int.TryParse(inputData, out cnt) == true)
                    {
                        //if (cnt <= int.Parse(l_WaferReturnCount.Text.Split('/')[0]) || l_WaferReturnCount.Text.Split('/')[0].Trim() == "0")
                        {
                            Frm_Print.Fnc_Print(temp, cnt, dgv_ReturnWafer.RowCount);
                            speech.SpeakAsync("라벨 출력");
                        }
                        //else
                        //{
                        //    speech.SpeakAsync("스캔된 갯수보다 큰 값을 입력 할 수 없습니다.");
                        //}
                    }
                    else
                    {
                        speech.SpeakAsync("숫자만 입력 가능 합니다.");
                    }
                }
                else
                {
                    speech.SpeakAsync("스캔 되지 않은 라트 입니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void btn_WaferReturnExcel_Click(object sender, EventArgs e)
        {


        }

        private void WaferReturnExcelOut()
        {
            pb_WaferReturn.Value = 0;
            pb_WaferReturn.Maximum = 10;
            SetWaferReturnProgressba("Excel 생성 중...", 1);

            Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = application.Workbooks.Add();// Filename: string.Format("{0}\\{1}", System.Environment.CurrentDirectory, @"\WaferReturn\WaferReturnOutTemp.xlsx"));

            Worksheet worksheet1 = workbook.Worksheets.get_Item(1);
            object misValue = System.Reflection.Missing.Value;

            application.Visible = false;


            worksheet1.Name = "WaferRetrunList";


            SetWaferReturnProgressba("Data Loading...", 2);

            string temp = string.Format("select [RETURN_NO],[SEQ],[DEVICE_NAME],[LOT],[DCC],[DS_QTY],[RETURN_QTY],[REMARK],[LOC],[SL],[SCAN_TIME_1st],[SCAN_USER_NAME_1st],[SCAN_TIME_2nd],[SCAN_USER_NAME_2nd],[AMKOR_ID],[CUST_CODE] from TB_RETURN_WAFER with(nolock) where [RETURN_NO]='{0}-{1}{2}' order by cast([SEQ] as int)", tb_Year.Text, tb_ReturnWafer.Text, Properties.Settings.Default.LOCATION);

            System.Data.DataTable MtlList = SearchData(temp).Tables[0];//(System.Data.DataTable)dgv_ReturnWafer.DataSource;

            if (dgv_ReturnWafer.DataSource != null)
            {
                string[,] item = new string[MtlList.Rows.Count, MtlList.Columns.Count - 2];
                string[] columns = new string[MtlList.Columns.Count];
                string cust = "";
                string returnnum = "";
                string totlot = "";

                SetWaferReturnProgressba("엑셀 양식 작성 중...", 3);

                Range rd = worksheet1.Range[worksheet1.Cells[1, 1], worksheet1.Cells[1, 14]];
                rd.Merge();
                rd.Value2 = "WAFER RETURN LIST";
                rd.Font.Bold = true;
                rd.Font.Size = 12.0;


                worksheet1.get_Range("A1").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                rd = worksheet1.Range[worksheet1.Cells[3, 3], worksheet1.Cells[4, 11]];
                rd.Font.Color = Color.Red;
                rd.Font.Size = 20.0;
                rd.Merge();
                rd.HorizontalAlignment = HorizontalAlignment.Center;
                rd.Value2 = "★고객 요청 사항 확인★";
                worksheet1.get_Range("D3").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


                rd = worksheet1.Range[worksheet1.Cells[4, 12], worksheet1.Cells[4, 12]];
                rd.Font.Color = Color.Red;
                rd.Font.Size = 20.0;
                //rd.Merge();
                rd.HorizontalAlignment = HorizontalAlignment.Center;
                rd.Value2 = "Total QTY";
                //worksheet1.get_Range("D3").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                int QtyCnt = 0;


                if (MtlList.Rows.Count > 0)
                {
                    cust = string.Format("Customer : {0}", MtlList.Rows[0][MtlList.Columns.Count - 1].ToString());
                    returnnum = string.Format("Return# : {0}-{1}{2}", tb_Year.Text, tb_ReturnWafer.Text, Properties.Settings.Default.LOCATION);
                    totlot = string.Format("Total Lot : {0}", MtlList.Rows.Count);

                    for (int c = 0; c < MtlList.Columns.Count; c++)
                    {
                        //컬럼 위치값을 가져오기
                        columns[c] = ExcelColumnIndexToName(c);
                    }

                    for (int rowNo = 0; rowNo < MtlList.Rows.Count; rowNo++)
                    {
                        for (int colNo = 0; colNo < MtlList.Columns.Count - 2; colNo++)
                        {

                            item[rowNo, colNo] = MtlList.Rows[rowNo][colNo].ToString();
                        }
                        QtyCnt += int.Parse(MtlList.Rows[rowNo]["RETURN_QTY"].ToString());
                    }
                }

                //해당위치에 컬럼명을 담기
                //worksheet1.get_Range("A1", columns[MtlList.Columns.Count - 1] + "1").Value2 = headers;
                //해당위치부터 데이터정보를 담기

                rd = worksheet1.Range[worksheet1.Cells[4, 13], worksheet1.Cells[4, 14]];
                rd.Font.Color = Color.Black;
                rd.Font.Size = 20.0;
                rd.Merge();
                rd.HorizontalAlignment = HorizontalAlignment.Center;
                rd.Value2 = QtyCnt;
                worksheet1.get_Range("M4").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


                worksheet1.get_Range("A3").Value = cust;
                worksheet1.get_Range("A4").Value = returnnum;
                worksheet1.get_Range("B4").Value = totlot;
                worksheet1.get_Range("A5").Value2 = "Return No";
                worksheet1.get_Range("B5").Value2 = "Seq";
                worksheet1.get_Range("C5").Value2 = "Device Name";
                worksheet1.get_Range("D5").Value2 = "Lot Number";
                worksheet1.get_Range("E5").Value2 = "Dcc";
                worksheet1.get_Range("F5").Value2 = "D/S Qty";
                worksheet1.get_Range("G5").Value2 = "Return-Q";
                worksheet1.get_Range("H5").Value2 = "Remark";
                worksheet1.get_Range("I5").Value2 = "Loc";
                worksheet1.get_Range("J5").Value2 = "SL";
                worksheet1.get_Range("K5").Value2 = "Scan Time1";
                worksheet1.get_Range("L5").Value2 = "Scan User1";
                worksheet1.get_Range("M5").Value2 = "Scan Time2";
                worksheet1.get_Range("N5").Value2 = "Scan User2";

                rd = worksheet1.Range["A5", "N5"];
                //rd.BorderAround2(XlLineStyle.xlDash);
                //rd.Borders[XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlContinuous;
                //rd.Borders[XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlContinuous;

                rd.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                rd.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThick;
                rd.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThick;

                SetWaferReturnProgressba("엑셀 양식 작성 완료...", 4);

                SetWaferReturnProgressba("Data 입력 중...", 5);
                worksheet1.get_Range("A6", columns[MtlList.Columns.Count - 3] + (MtlList.Rows.Count + 5).ToString()).Value = item;
                worksheet1.get_Range("A6", columns[MtlList.Columns.Count - 3] + (MtlList.Rows.Count + 5).ToString()).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet1.Cells.NumberFormat = @"@";
                worksheet1.Columns.AutoFit();

                worksheet1.get_Range("L3").Value = "MBB / DESICCANT / HUMDITY CARD 확인 요망";

                SetWaferReturnProgressba("Sheet Page Setup...", 6);
                worksheet1.PageSetup.PrintArea = string.Format("A1:{0}", columns[MtlList.Columns.Count - 3] + (MtlList.Rows.Count + 5).ToString());
                worksheet1.PageSetup.Zoom = false;
                worksheet1.PageSetup.FitToPagesWide = 1;        // Zoom이 False일 때만 적용 됨


                string filePath = "";

                SetWaferReturnProgressba("파일 저장 중...", 7);

                if (Properties.Settings.Default.WaferReturnExcelOutPath != "")
                {
                    filePath = string.Format("{0}\\WaferReturnOut_{1}.xlsx", Properties.Settings.Default.WaferReturnExcelOutPath, DateTime.Now.ToString("yyyyMMddhhmmss"));
                    workbook.SaveAs(filePath, Excel.XlFileFormat.xlOpenXMLWorkbook, System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                }
                else
                {
                    filePath = string.Format("{0}\\WaferReturnOut_{1}.xlsx", System.Environment.CurrentDirectory + "\\WaferReturn", DateTime.Now.ToString("yyyyMMddhhmmss"));
                    workbook.SaveAs(filePath, Excel.XlFileFormat.xlOpenXMLWorkbook, System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                }

                speech.SpeakAsync("엑셀 저장이 완료 되었습니다.");
                SetWaferReturnProgressba("파일 저장 완료", 8);

                workbook.Close();
                application.Quit();

                releaseObject(application);
                releaseObject(worksheet1);
                releaseObject(workbook);

                SetWaferReturnProgressba("Excel 종료", 9);

                if (DialogResult.Yes == MessageBox.Show("파일을 여시겠습니까?", "file open?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    ProcessStartInfo info = new ProcessStartInfo("excel.exe", filePath);
                    Process.Start(info);
                }


                SetWaferReturnProgressba("Excel 실행 완료", 10);

            }
            else
            {
                MessageBox.Show("데이터가 없습니다.");
            }
        }

        private string ExcelColumnIndexToName(int Index)
        {
            string range = "";
            if (Index < 0) return range;
            for (int i = 1; Index + i > 0; i = 0)
            {
                range = ((char)(65 + Index % 26)).ToString() + range;
                Index /= 26;
            }
            if (range.Length > 1) range = ((char)((int)range[0] - 1)).ToString() + range.Substring(1);
            return range;
        }

        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception e)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        private void btn_WaferReturnExcel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();

                fd.ShowNewFolderButton = true;

                if (Properties.Settings.Default.WaferReturnExcelOutPath == "")
                    fd.SelectedPath = Environment.SpecialFolder.Desktop.ToString();
                else
                    fd.SelectedPath = Properties.Settings.Default.WaferReturnExcelOutPath;

                if (DialogResult.OK == fd.ShowDialog())
                {
                    Properties.Settings.Default.WaferReturnExcelOutPath = fd.SelectedPath;
                    Properties.Settings.Default.Save();

                    toolTip1.SetToolTip(btn_WaferReturnExcel, string.Format("{0}\n경로 변경 : 마우스 오른쪽 클릭", Properties.Settings.Default.WaferReturnExcelOutPath));
                }
            }
            else
            {
                if (dgv_ReturnWafer.Rows.Count != 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("Excel 저장 하시겠습니까?", "Excel 출력", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        WaferReturnExcelOut();
                    }
                }
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("Excel Load 하시겠습니까?", "Excel Load", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        openFileDialog1.InitialDirectory = Application.StartupPath;
                    //openFileDialog1.Filter = "Excel|*.xls";

                    if (DialogResult.OK == openFileDialog1.ShowDialog())
                    {
                        sDownloadPath = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                        file_name = System.IO.Path.GetFileName(openFileDialog1.FileName);
                        Thread th = new Thread(WriteWaferReturnData);
                        th.Start();
                    }
                }
            }
        }

        private void tb_ReturnWafer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                Properties.Settings.Default.WaferReturnCode = tb_ReturnWafer.Text;
                Properties.Settings.Default.Save();

                button19_Click(sender, e);
            }
        }

        private void btn_WaferReturnReset_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(string.Format("{0}-{1}{2}을 초기화 하시겠습니까?", tb_Year.Text, tb_ReturnWafer.Text, Properties.Settings.Default.LOCATION), "초기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                if (bWaferReturnNumChange == false)
                {
                    if (dgv_ReturnWafer.RowCount != 0)
                    {
                        for (int i = 0; i < dgv_ReturnWafer.RowCount; i++)
                        {
                            string q = string.Format("update TB_RETURN_WAFER set [SCAN_TIME_1st]='',[SCAN_USER_NAME_1st]='', [SCAN_TIME_2nd] ='', [SCAN_USER_NAME_2nd]='' where [RETURN_NO]='{0}'", string.Format("{0}-{1}{2}", tb_Year.Text, tb_ReturnWafer.Text, Properties.Settings.Default.LOCATION));

                            run_sql_command(q);
                        }
                    }
                    else
                    {
                        MessageBox.Show("조회된 데이터가 없습니다.", "데이터 없음!!!");
                    }
                }
                else
                {
                    MessageBox.Show("Return# 이 변경 되었습니다.\n다시 조회 후 초기화 하세요", "재 조회 후 초기화");
                }

                l_WaferReturnCount.Text = string.Format("{0} / {1}", 0, dgv_ReturnWafer.RowCount);

                button19_Click(sender, e);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("종료 하시겠습니까?", "Wafer Raturn Mode 종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                tb_Year.Text = "";
                tb_ReturnWafer.Text = "";
                dgv_ReturnWafer.DataSource = 0;

                SetWaferReturnControl(false);

                tabControl_Sort.SelectedIndex = 0;
            }
        }

        private void SetWaferReturnControl(bool b)
        {
            searched_row = 0;
            input = "";
            l_WaferReturnCount.Text = "0";

            tb_WaferReturnScan.Enabled = b;
            tb_ReturnWafer.Enabled = b;
            tb_Year.Enabled = b;

            btn_WaferReturnFind.Enabled = b;
            btn_WaferReturnReadDB.Enabled = b;
            btn_WaferReturnReset.Enabled = b;
            btn_WaferReturnExcel.Enabled = b;


            tb_WaferReturnScan.ImeMode = ImeMode.Alpha;



        }

        private void btn_Find_Click(object sender, EventArgs e)
        {
            int Realindex = -1;

            input = Microsoft.VisualBasic.Interaction.InputBox("무엇을 검색하시겠습니까?", "Search", "", -1, -1);

            if (input == "")
                return;

            searched_row = 0;

            for (int n = 0; n < dgv_ReturnWafer.RowCount; n++)
            {
                if (dgv_ReturnWafer.Rows[n].Cells["LOT"].Value.ToString().IndexOf(input) != -1)
                {
                    dgv_ReturnWafer.Rows[n].Selected = true;
                    dgv_ReturnWafer.FirstDisplayedScrollingRowIndex = n;
                    dgv_ReturnWafer.CurrentCell = dgv_ReturnWafer.Rows[n].Cells[0];
                    searched_row = n;
                    break;
                }


                if (dgv_ReturnWafer.Rows[n].Cells[3].Value.ToString().Contains(input) == true)
                {
                    dgv_ReturnWafer.Rows[n].Selected = true;
                    dgv_ReturnWafer.FirstDisplayedScrollingRowIndex = n;
                    dgv_ReturnWafer.CurrentCell = dgv_ReturnWafer.Rows[n].Cells[0];
                    searched_row = n;
                    break;
                }

                if (n == dgv_ReturnWafer.RowCount - 1)
                    MessageBox.Show("지정된 문자열을 찾을 수 없습니다.");
            }
        }

        private void btn_WaferReturnFindNext_Click(object sender, EventArgs e)
        {
            for (int n = 0; n < dgv_ReturnWafer.RowCount; n++)
            {
                if (dgv_ReturnWafer.Rows[n].Cells["LOT"].Value.ToString().Contains(input) == true)
                {
                    if (searched_row < n)
                    {
                        dgv_ReturnWafer.Rows[n].Selected = true;
                        dgv_ReturnWafer.FirstDisplayedScrollingRowIndex = n;
                        dgv_ReturnWafer.CurrentCell = dgv_ReturnWafer.Rows[n].Cells[0];
                        searched_row = n;
                        break;
                    }
                }

                if (n == dgv_ReturnWafer.RowCount - 1)
                {
                    searched_row = -1; ;
                    MessageBox.Show("지정된 문자열을 찾을 수 없습니다.");

                }
            }
        }

        private void tb_WaferReturnScan_MouseDown(object sender, MouseEventArgs e)
        {
            tb_WaferReturnScan.ImeMode = ImeMode.Alpha;
        }

        bool bWaferReturnNumChange = false;

        private void tb_ReturnWafer_TextChanged(object sender, EventArgs e)
        {
            bWaferReturnNumChange = true;
        }

        string ReturnWaferNum = "";

        private void btn_Accept_Click(object sender, EventArgs e)
        {
            ReturnWaferNum = $"{tb_ReturnWafer.Text}{ Properties.Settings.Default.LOCATION}";

            if (DialogResult.Yes == MessageBox.Show($"{tb_Year.Text}-{tb_ReturnWafer.Text}{Properties.Settings.Default.LOCATION}을 Accept 하시겠습니까?", "Accept", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Thread AcceptThread = new Thread(Accpetthread);
                AcceptThread.Start();
            }
        }

        private void cb_Qualcomm_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ReturnQualcomm = cb_Qualcomm.Checked;
            Properties.Settings.Default.Save();


            l_msl.Enabled = cb_Qualcomm.Checked;
            l_2nd.Enabled = cb_Qualcomm.Checked;
            tb_MSL.Enabled = cb_Qualcomm.Checked;
            tb_2ndLI.Enabled = cb_Qualcomm.Checked;

            btn_QualdommSave.Enabled = cb_Qualcomm.Checked;

            if (cb_Qualcomm.Checked == true)
                if (Frm_Print.QualcommSocketManager == null)
                    Frm_Print.QualcommSocket_Init();
        }

        private void tb_MSL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                Properties.Settings.Default.QualcommMSL = tb_MSL.Text;
                Properties.Settings.Default.Save();

                tb_2ndLI.Focus();
            }
        }

        private void tb_2ndLI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                Properties.Settings.Default.Qualcomm2nd = tb_2ndLI.Text;
                Properties.Settings.Default.Save();

                btn_QualdommSave.Focus();
            }
        }

        private void btn_QualdommSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.QualcommMSL = tb_MSL.Text;
            Properties.Settings.Default.Qualcomm2nd = tb_2ndLI.Text;
            Properties.Settings.Default.Save();

            MessageBox.Show($"MSL : {Properties.Settings.Default.QualcommMSL}\n2nd L.I : {Properties.Settings.Default.Qualcomm2nd}\n 저장 되었습니다.", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tb_wsn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                Properties.Settings.Default.QorvoWSN = tb_wsn.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void btn_WSNSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.QorvoWSN = tb_wsn.Text;
            Properties.Settings.Default.Save();
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

        private void btn_WSNExcel_Click(object sender, EventArgs e)
        {
            string ExcelData = "";
            List<string> ExcelList = new List<string>();



            for (int i = 0; i < dataGridView_Device.RowCount; i++)
            {
                if (Checkdev(dataGridView_Device.Rows[i].Cells[1].Value.ToString()) == true)
                {
                    string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
                    string strReadfile = strFileName + "\\" + dataGridView_Device.Rows[i].Cells[1].Value.ToString() + "\\" + dataGridView_Device.Rows[i].Cells[1].Value.ToString() + ".txt";

                    string[] info = Fnc_ReadFile(strReadfile);

                    for (int m = 0; m < info.Length; m++)
                    {
                        string[] strSplit_data = info[m].Split('\t');

                        if (strSplit_data[13].ToUpper() == "COMPLETE")
                        {
                            ExcelData = strSplit_data[2];
                            ExcelData += "\t" + strSplit_data[3];
                            ExcelData += "\t" + strSplit_data[18];

                            ExcelList.Add(ExcelData);
                        }
                    }
                }
            }

            uint excelProcessId = 0;
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                // Execute Excel application
                excelApp = new Excel.Application();



                // 엑셀파일 열기 or 새로 만들기
                string filepath = $"{Application.StartupPath}\\QorvoWSN\\QorvoWSN__{DateTime.Now.ToString("yyMMdd HHmm")}.xlsx";
                bool isFileExist = File.Exists(filepath);
                wb = isFileExist ? excelApp.Workbooks.Open(filepath, ReadOnly: false, Editable: true) : excelApp.Workbooks.Add();

                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;
                // Worksheet 이름 설정하기
                // ws.Name = targetWorksheetName ;

                int row = ExcelList.Count;
                int column = 3;

                object[,] data = new object[row, column];

                //for (int r = 0; r < row; r++)
                //{
                //    for (int c = 0; c < column; c++)
                //    {
                //        data[r, c] = ExcelList[r].Split('\t')[c];
                //    }
                //}

                ws.Cells[1, 1] = "MotherLot#";
                ws.Cells[1, 2] = "MotherDcc";
                ws.Cells[1, 3] = "MOO";


                for (int i = 1; i <= ExcelList.Count; i++)
                {
                    ws.Cells[i + 1, 1] = ExcelList[i - 1].Split('\t')[0];
                    ws.Cells[i + 1, 2] = ExcelList[i - 1].Split('\t')[1];
                    ws.Cells[i + 1, 3] = ExcelList[i - 1].Split('\t')[2];

                    Application.DoEvents();
                }

                // row, column 번호로 Cell 접근
                //Excel.Range rng = ws.Range[ws.Cells[1, 1], ws.Cells[row, column]];

                //Excel.Range rng = ws.get_Range("A1");
                //rng = rng.get_Resize(row, column);

                // 저장하는 여러 방법 중 두가지
                // rng.Value = data;
                //rng.set_Value(Missing.Value, data);

                if (isFileExist)
                {
                    wb.Save(); // 덮어쓰기
                }
                else
                {
                    if (Directory.Exists($"{Application.StartupPath}\\QorvoWSN\\") == false)
                        Directory.CreateDirectory($"{Application.StartupPath}\\QorvoWSN\\");

                    wb.SaveCopyAs(filepath); // 새 파일 만들기
                }

                if (DialogResult.Yes == MessageBox.Show("Excel 생성이 완료 되었습니다.\n폴더를 Open 하시겠습니까?", "저장 완료", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    System.Diagnostics.Process.Start($"{Application.StartupPath}\\QorvoWSN\\");
                }

                wb.Close(false);
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                if (wb != null)
                {
                    wb.Close(SaveChanges: false);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                }
            }
            finally
            {
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);

                if (excelApp != null && excelProcessId > 0)
                {
                    Process.GetProcessById((int)excelProcessId).Kill();
                }
            }
        }

        private static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception)
            {
                obj = null;
                throw;
            }
            finally
            {
                GC.Collect();
            }
        }


        private void tabPage13_Click(object sender, EventArgs e)
        {

        }


        private void Accpetthread()
        {
            ChromeDriverUpdater();

            string id = BankHost_main.strMESID;
            string pw = BankHost_main.strMESPW;
            string badge = BankHost_main.strID;
            sDownloadPath = Path.Combine(System.Environment.CurrentDirectory, "WaferReturn\\Excel\\");

            try
            {
                ChromeDriverUpdater();


                if (System.IO.Directory.Exists(sDownloadPath) == false)
                {
                    SetWaferReturnProgressba("Directory 생성 중 입니다.", 9);
                    System.IO.Directory.CreateDirectory(sDownloadPath);
                }
                else
                {
                    try
                    {
                        System.IO.DirectoryInfo di1 = new System.IO.DirectoryInfo(sDownloadPath);

                        FileInfo[] fi1 = di1.GetFiles();

                        for (int i = 0; i < fi1.Length; i++)
                        {
                            fi1[i].Delete();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                bDownloadComp = false;

                _driverService = ChromeDriverService.CreateDefaultService();

                _driverService.HideCommandPromptWindow = true;

                _options = new ChromeOptions();



                _options.AddArgument("--disable-gpu");
                _options.AddUserProfilePreference("download.default_directory", sDownloadPath);
                _options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
                _options.AddUserProfilePreference("safebrowsing.enabled", false);

                /* test server
                _driver = new ChromeDriver(_driverService, _options);
                _driver.Navigate().GoToUrl("http://10.101.1.37:9080/eMES/");  // 웹 사이트에 접속합니다. 
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                progressBar1.Maximum = 15;
                progressBar1.Value = 1;

                SetProgressba("eMes에 접속 중입니다.", 1);
                _driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[3]/td[2]/p/font/span/input").SendKeys("abc4");    // ID 입력          
                _driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[4]/td[2]/p/font/span/input").SendKeys("abc4");   // PW 입력            
                _driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[5]/td[2]/font/span/input").SendKeys("362808");   // 사번 입력         
                _driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/p/input").Click();   // Main 로그인 버튼            
                SetProgressba("Login 확인 중", 2);

                _driver.Navigate().GoToUrl("http://10.101.1.37:9080/eMES/diebank/PCSScrapRequest.jsp");   // Scrap request 항목으로 이동
                SetProgressba("Scrap 메뉴로 이동 중입니다.", 3);


                while (_driver.Url != "http://10.101.1.37:9080/eMES/diebank/PCSScrapRequest.jsp")
                {
                    _driver.Navigate().GoToUrl("http://10.101.1.37:9080/eMES/diebank/PCSScrapRequest.jsp");   // Scrap request 항목으로 이동
                    Thread.Sleep(500);
                }
                */


                _driver = new ChromeDriver(_driverService, _options);
                _driver.Navigate().GoToUrl("http://aak1ws01/eMES/index.jsp");  // 웹 사이트에 접속합니다. 
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                pb_WaferReturn.Maximum = 15;
                pb_WaferReturn.Value = 1;

                SetWaferReturnProgressba("eMes에 접속 중입니다.", 1);
                _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[3]/td[2]/p/font/span/input")).SendKeys(id);    // ID 입력          
                _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[4]/td[2]/p/font/span/input")).SendKeys(pw);   // PW 입력            
                _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[5]/td[2]/font/span/input")).SendKeys(badge);   // 사번 입력         
                _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/p/input")).Click();   // Main 로그인 버튼            
                SetWaferReturnProgressba("Login 확인 중", 2);

                System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement> temp = _driver.FindElements(By.XPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[1]/img"));

                if (temp.Count != 0)
                {
                    if (_driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/center/font")).Text == "Invalid Username or Password !!!")
                    {
                        MessageBox.Show("ID or 비밀번호 or 사번이 틀립니다.\n ID, 비밀번호, 사번을 확인해 주세요");
                        return;
                    }
                    else if (_driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/center/font")).Text == "User ID can't be used.")
                    {
                        MessageBox.Show("해당 ID로 접속 할 수 없습니다.\n ID 및 Network 상태를 점검해 주세요");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("알수 없는 에러가 발생하였습니다.");
                        return;
                    }
                }

                _driver.Navigate().GoToUrl("http://aak1ws01/eMES/diebank/ttsReturnFind.do");   // Scrap request 항목으로 이동
                SetWaferReturnProgressba("Wafer Return 메뉴로 이동 중입니다.", 3);


                while (_driver.Url != "http://aak1ws01/eMES/diebank/ttsReturnFind.do")
                {
                    _driver.Navigate().GoToUrl("http://aak1ws01/eMES/diebank/ttsReturnFind.do");   // Scrap request 항목으로 이동
                    Thread.Sleep(500);
                }

                SetWaferReturnProgressba("년도 설정", 4);
                _driver.FindElement(By.XPath("/html/body/form[1]/table/tbody/tr[2]/td/table/tbody/tr[4]/td[2]/input")).Clear();
                _driver.FindElement(By.XPath("/html/body/form[1]/table/tbody/tr[2]/td/table/tbody/tr[4]/td[2]/input")).SendKeys(tb_Year.Text);

                SetWaferReturnProgressba("데이터 조회 중입니다.", 7);
                _driver.FindElement(By.XPath("/html/body/form[1]/table/tbody/tr[3]/td/div/table/tbody/tr/td/p/span/b/font/input")).Click();    //Find 버튼 누름


                temp = _driver.FindElements(By.Name("selected"));

                if (temp.Count == 0)
                {
                    SetWaferReturnProgressba("조회된 데이터가 없습니다.", 100);
                    return;
                }
                else
                {
                    temp = _driver.FindElements(By.PartialLinkText("K4"));

                    WaferReturnInfo = new List<stWaferReturnInfo>();

                    for (int i = 0; i < temp.Count; i++)
                    {
                        WaferReturnInfo.Add(new stWaferReturnInfo());
                    }

                    WaferReturnDataSort(_driver.FindElement(By.XPath("/html/body/form[2]/table")));

                }
                IReadOnlyList<IWebElement> cbs = _driver.FindElements(By.Name("selected"));

                for (int i = 0; i < WaferReturnInfo.Count; i++)
                {
                    if (WaferReturnInfo[i].WebInfo.ReturnNum == ReturnWaferNum)
                    {
                        cbs[i].Click();
                        break;
                    }
                }

                _driver.FindElement(By.XPath("/html/body/form[2]/table/tbody/tr[2]/td/div/table/tbody/tr/td[4]/p/span/b/font/span/b/input")).Click();

                _driver.SwitchTo().Window(_driver.WindowHandles.Last());

                _driver.FindElement(By.Name("checkbox1")).Click();
                _driver.FindElement(By.Name("submitAct")).Click();
            }
            catch (Exception ex)
            {
                //< input type = "checkbox" name = "selected" value = "2023:70234K4:0" >
            }
        }

        public bool GetQualCommSplitGreenLabel()
        {
            return cb_GreenLabel.Checked;
        }

        private void cb_NomalLabel_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btn_WaferReturnExcel_Click_1(object sender, EventArgs e)
        {

        }

        private void nup_Wlabel_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SplitWLabel = (int)nup_Wlabel.Value;
            Properties.Settings.Default.Save();
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            Frm_Print.QualcommSocket_Init();
            Frm_Print.QualcomSocket_MessageSend("^XA^FO25,0^A0B,30,30^FDQualcomm^FS^FO60,0^BXN,4.5,200^FD1JUN144356508PAN10UCL,PCD90-PT675-3RN,1T000FK326DJT.K500#GG7Y72.QRS,30T17/18/19,10D2326,Q2167,14D10-07-2027^FS^FO260,-10^A0N,25,25^FD(1J)LPN: UN144356508PAN10UCL^FS^FO660,-10^A0N,25,25^FD(30T)Wafer ID(s): 17/18/19^FS^FO1010,-10^A0N,25,25^FD(10D)D/C: 2326^FS^FO260,60^A0N,25,25^FD(P)MCN: CD90-PT675-3RN^FS^FO660,60^A0N,25,25^FD(Q)Quantity: 2167^FS^FO860,60^A0N,25,25^FDDry Pack Exp: 10-07-2027^FS^FO260,130^A0,25,25^FD(1T)Lot Code: 000FK326DJT.K500#GG7Y72.QRS^FS^FO1060,130^A0N,25,25^FD1118^FS^XZ");
        }

        private void button19_Click_2(object sender, EventArgs e)
        {
            string s = GetWebServiceData("http://10.131.10.84:8080/api/diebank/gr-info/k4?ReelID=8063-6056A.01");
            //string t = GetWebServiceData($"http://{(Properties.Settings.Default.TestMode == true ? TEST_MES : PRD_MES)}/eMES_Webservice/diebank_automation_service/chk_dup_reel_inf/7900-7277A.1").ToUpper();
            inputEmpNum.ShowDialog();
        }

        private void btn_loadExcel_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);

            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                Workbook workbook = application.Workbooks.Open(openFileDialog1.FileName);
                Worksheet worksheet1 = workbook.Worksheets.get_Item(1);
                application.Visible = false;

                Excel.Range range = worksheet1.UsedRange;

                List<string> data = new List<string>();
                string excelrow = "";

                int HeaderRow = -1;
                int returnNumCnt = -1;
                int seqCnt = -1;
                int rowOffset = 4;

                stWaferReturnExcelInfo excelInfo = new stWaferReturnExcelInfo();
                string[] temp1 = new string[9];

                object[,] excelData = new object[range.Rows.Count, range.Columns.Count];

                excelData = (object[,])range.Value2;
                pb_WaferReturn.Maximum = range.Rows.Count;
                SetWaferReturnProgressba("", 0);

                for (int i = 1; i <= range.Rows.Count - 2; ++i)
                {
                    excelrow = "";

                    temp1[0] = excelData[i + 1, 1] == null ? "" : excelData[i + 1, 1].ToString();
                    temp1[1] = excelData[i + 1, 2] == null ? "" : excelData[i + 1, 2].ToString();
                    temp1[2] = excelData[i + 1, 3] == null ? "" : excelData[i + 1, 3].ToString();
                    temp1[3] = excelData[i + 1, 4] == null ? "" : excelData[i + 1, 4].ToString();
                    temp1[4] = excelData[i + 1, 5] == null ? "" : excelData[i + 1, 5].ToString();
                    temp1[5] = excelData[i + 1, 6] == null ? "" : excelData[i + 1, 6].ToString();
                    temp1[6] = excelData[i + 1, 7] == null ? "" : excelData[i + 1, 7].ToString();
                    temp1[7] = excelData[i + 1, 8] == null ? "" : excelData[i + 1, 8].ToString();
                    temp1[8] = excelData[i + 1, 9] == null ? "" : excelData[i + 1, 9].ToString();

                    dgv_ATVLabel.Rows.Add(new object[] { temp1[0], temp1[1], temp1[2], temp1[3], temp1[4], temp1[5], temp1[6], temp1[7], temp1[8] });
                }

                /*메모리 할당 해제*/
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(worksheet1);
                workbook.Close();
                Marshal.ReleaseComObject(workbook);
                application.Quit();
                Marshal.ReleaseComObject(application);
            }
        }

        private void t_LogOut_Tick(object sender, EventArgs e)
        {

        }

        private void bgw_timeout_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void dataGridView_Device_MouseClick(object sender, MouseEventArgs e)
        {
            ClickTime();
        }

        private void tb_splitScan_MouseClick(object sender, MouseEventArgs e)
        {
            if (tb_splitScan.Text == "Input here")
            {
                tb_splitScan.Text = "";
            }
        }

        int splitMainNum = 0;

        private void tb_splitScan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                //Split_log_file_save

                SplitLogFileSave(tb_splitScan.Text);

                if (tb_splitScan.Text.Contains(':') == true)
                {
                    splitMainNum++;

                    //lot 0      DCC1   DEV  2        QTY 3     REEL4      5     cust6
                    //3507-P056A:    :2UA3-8233-TR1C:0000054325:00004:0012183533:00488::
                    string[] temp = tb_splitScan.Text.Split(':');

                    dgv_split.Rows.Add(new object[] { splitMainNum, temp[2].Trim(), temp[0].Trim(), temp[1].Trim(), temp[3].Trim(), temp[4].Trim() });

                    for (int i = 1; i < int.Parse(temp[4]) + 1; ++i)
                    {
                        dgv_split.Rows.Add(new object[] { $"{splitMainNum}-{i}", temp[2].Trim(), $"{temp[0].Trim()}", "", "", "" });
                    }
                    SpeakST($"{splitMainNum} 추가");
                }
                else
                {
                    if (cb_splitMode.SelectedIndex == 0) //AVAGO
                    {
                        if (tb_splitScan.Text.Contains(';') == true)
                        {
                            // 0 DEV          1 Lot      2 WFR#      3 QTY
                            //2UA3-8233-TR1C;ARUA3A3507;3507-P056A.04;9326;2346;Chipbond

                            string[] temp = tb_splitScan.Text.Split(';');

                            List<DataGridViewRow> selectRows = dgv_split.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["splitDevice"].Value.ToString() == temp[0] && row.Cells["splitNo"].Value.ToString().Contains('-') == true && row.Cells["splitQTY"].Value.ToString() == "").ToList();

                            if (selectRows.Count == 0)
                            {
                                SpeakST("앰코 라벨을 먼저 스캔 하세요");
                            }
                            else
                            {
                                int index = selectRows.FirstOrDefault().Index;

                                dgv_split.Rows[index].DefaultCellStyle.BackColor = Color.Aquamarine;
                                dgv_split.Rows[index].Cells["SplitLot"].Value = temp[2];
                                dgv_split.Rows[index].Cells["SplitQTY"].Value = temp[3];
                                dgv_split.Rows[index].Cells["SplitEA"].Value = "1";

                                SpeakST(selectRows[0].Cells["SplitNo"].Value.ToString().Remove(selectRows[0].Cells["SplitNo"].Value.ToString().LastIndexOf("-"), selectRows[0].Cells["SplitNo"].Value.ToString().Length - selectRows[0].Cells["SplitNo"].Value.ToString().LastIndexOf("-")));

                                SplitCheckSplitComp(tb_splitScan.Text);
                            }
                        }
                        else if (tb_splitScan.Text.Contains('+') == true)
                        {
                            SpeakST("고객 확인 바람");
                        }

                    }
                    else if (cb_splitMode.SelectedIndex == 1) //SKYWORKS
                    {
                        if (tb_splitScan.Text.Contains('+') == true)
                        {

                        }
                    }
                }

                tb_splitScan.Text = "";
            }
        }

        Color motherLotCompColor = Color.LawnGreen;

        private void SplitCheckSplitComp(string scandata)
        {

            // 0 DEV          1 Lot      2 WFR#      3 QTY
            //2UA3-8233-TR1C;ARUA3A3507;3507-P056A.04;9326;2346;Chipbond
            string[] temp = scandata.Split(';');

            List<DataGridViewRow> listSumRow = dgv_split.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["splitDevice"].Value.ToString() == temp[0] && row.Cells["splitLot"].Value.ToString().Contains(temp[2].Remove(temp[2].LastIndexOf('.'), temp[2].Length - temp[2].LastIndexOf('.'))) == true).ToList();
            listSumRow.Sort((a, b) => a.Index.CompareTo(b.Index));

            int totQTY = int.Parse(listSumRow[0].Cells["SplitQTY"].Value.ToString());
            int sumQTY = 0;
            int scanCNT = 0;

            for (int i = 1; i < listSumRow.Count; i++)
            {
                sumQTY += int.Parse(dgv_split.Rows[listSumRow[i].Index].Cells["SplitQTY"].Value.ToString() == "" ? "0" : dgv_split.Rows[listSumRow[i].Index].Cells["SplitQTY"].Value.ToString());

                scanCNT += dgv_split.Rows[listSumRow[i].Index].DefaultCellStyle.BackColor == Color.Aquamarine ? 1 : 0;
            }

            if (totQTY == sumQTY)
            {
                dgv_split.Rows[listSumRow[0].Index].DefaultCellStyle.BackColor = motherLotCompColor;
                SpeakST($"{listSumRow[0].Cells["SplitNo"].Value.ToString()} 완료");


            }
            else if(totQTY < sumQTY)
            {
                SpeakST($"{listSumRow[0].Cells["SplitNo"].Value.ToString()} 수량 초과");
            }
            else
            {
                if (cb_ReturnReel.Checked == true)
                {
                    if (listSumRow.Count == 2)
                    {
                        listSumRow[1].Cells["splitQTY"].Value = totQTY;
                        SpeakST($"{listSumRow[0].Cells["SplitNo"].Value.ToString()} 완료");
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (scanCNT == int.Parse(listSumRow[0].Cells["SplitEA"].Value.ToString()))
                    {
                        SpeakST($"{listSumRow[0].Cells["SplitNo"].Value.ToString()} 수량 틀림");

                        Form_Board form_Board = new Form_Board($"#{listSumRow[0].Cells["SplitNo"].Value.ToString()} 수량 틀림", Color.Black, Color.Red);
                        form_Board.ShowDialog();
                    }
                }
            }
        }

        private void SplitCheckSplitComp(int RowIndex)
        {

            // 0 DEV          1 Lot      2 WFR#      3 QTY
            //2UA3-8233-TR1C;ARUA3A3507;3507-P056A.04;9326;2346;Chipbond

            String Mlot = (dgv_split.Rows[RowIndex].Cells["splitLot"].Value.ToString().Remove(dgv_split.Rows[RowIndex].Cells["splitLot"].Value.ToString().IndexOf('.'), dgv_split.Rows[RowIndex].Cells["splitLot"].Value.ToString().Length - dgv_split.Rows[RowIndex].Cells["splitLot"].Value.ToString().IndexOf('.')));

            List<DataGridViewRow> listSumRow = dgv_split.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["splitDevice"].Value.ToString() == dgv_split.Rows[RowIndex].Cells["splitDevice"].Value.ToString() && (row.Cells["splitLot"].Value.ToString().Contains('.') == true ? row.Cells["splitLot"].Value.ToString().Remove(row.Cells["splitLot"].Value.ToString().IndexOf('.'), row.Cells["splitLot"].Value.ToString().Length- row.Cells["splitLot"].Value.ToString().IndexOf('.')) : row.Cells["splitLot"].Value.ToString()) == Mlot).ToList();
            listSumRow.Sort((a, b) => a.Index.CompareTo(b.Index));

            int totQTY = 0;

            if (int.TryParse(listSumRow[0].Cells["splitQTY"].Value.ToString(), out totQTY) == false)
                return;

            
            int sumQTY = 0;
            int scanCNT = 0;

            for (int i = 1; i < listSumRow.Count; i++)
            {
                sumQTY += int.Parse(dgv_split.Rows[listSumRow[i].Index].Cells["SplitQTY"].Value.ToString() == "" ? "0" : dgv_split.Rows[listSumRow[i].Index].Cells["SplitQTY"].Value.ToString());

                scanCNT += dgv_split.Rows[listSumRow[i].Index].DefaultCellStyle.BackColor == Color.Aquamarine ? 1 : 0;
            }

            if (totQTY == sumQTY)
            {
                dgv_split.Rows[listSumRow[0].Index].DefaultCellStyle.BackColor = motherLotCompColor;
                SpeakST($"{listSumRow[0].Cells["SplitNo"].Value.ToString()} 완료");
            }
            else
            {
                if (cb_ReturnReel.Checked == true)
                {
                    if (listSumRow.Count == 2)
                    {
                        listSumRow[0].DefaultCellStyle.BackColor = motherLotCompColor;
                        listSumRow[1].Cells["splitQTY"].Value = totQTY;
                        SpeakST($"{listSumRow[0].Cells["SplitNo"].Value.ToString()} 완료");
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (scanCNT == int.Parse(listSumRow[0].Cells["SplitEA"].Value.ToString()))
                    {
                        SpeakST($"{listSumRow[0].Cells["SplitNo"].Value.ToString()} 수량 틀림");

                        Form_Board form_Board = new Form_Board($"#{listSumRow[0].Cells["SplitNo"].Value.ToString()} 수량 틀림", Color.Black, Color.Red);
                        form_Board.ShowDialog();
                    }
                }
            }
        }


        private void tb_splitScan_Leave(object sender, EventArgs e)
        {
        }

        private void exportSplitData()
        {
            if (DialogResult.Yes != MessageBox.Show("Excel 출력 하시겠습니까?", "Excel 출력", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                SpeakST("취소");
                return;
            }


            //SetWaferReturnProgressba("Excel 생성 중...", 1);

            try
            {
                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                Workbook workbook = application.Workbooks.Add();// Filename: string.Format("{0}\\{1}", System.Environment.CurrentDirectory, @"\WaferReturn\WaferReturnOutTemp.xlsx"));

                Worksheet worksheet1 = workbook.Worksheets.get_Item(1);
                object misValue = System.Reflection.Missing.Value;

                application.Visible = false;
                worksheet1.Name = "ReelSortList";

                if (dgv_split.Rows.Count != 0)
                {
                    string[,] item = new string[dgv_split.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value.ToString().Contains("-") == true).ToList().Count, dgv_split.Columns.Count + 2];
                    string[] columns = new string[dgv_split.Columns.Count + 2];
                    string cust = "";
                    string returnnum = "";
                    string totlot = "";

                    Range rd = worksheet1.Range[worksheet1.Cells[1, 1], worksheet1.Cells[1, 14]];

                    if (dgv_split.Rows.Count > 0)
                    {
                        for (int c = 0; c < dgv_split.Columns.Count + 2; c++)
                        {
                            //컬럼 위치값을 가져오기
                            columns[c] = ExcelColumnIndexToName(c);
                        }

                        int nrow = 0;
                        string MotherLot = "";
                        string MotherLotDCC = "";

                        for (int rowNo = 0; rowNo < dgv_split.Rows.Count; rowNo++)
                        {
                            if (dgv_split.Rows[rowNo].Cells[0].Value.ToString().Contains("-") == true)
                            {
                                item[nrow, 0] = MotherLot;    // mother Lot
                                item[nrow, 1] = MotherLotDCC;
                                item[nrow, 2] = dgv_split.Rows[rowNo].Cells["splitLot"].Value.ToString();
                                item[nrow, 3] = dgv_split.Rows[rowNo].Cells["splitDCC"].Value.ToString();
                                item[nrow, 4] = dgv_split.Rows[rowNo].Cells["splitQTY"].Value.ToString();
                                item[nrow, 5] = "1";
                                item[nrow, 6] = "";
                                item[nrow, 7] = dgv_split.Rows[rowNo].Cells["splitDevice"].Value.ToString();

                                ++nrow;
                            }
                            else
                            {
                                if (dgv_split.Rows[rowNo].DefaultCellStyle.BackColor != motherLotCompColor)
                                {
                                    rowNo += int.Parse(dgv_split.Rows[rowNo].Cells["splitEA"].Value.ToString());
                                }
                                else if (dgv_split.Rows[rowNo].DefaultCellStyle.BackColor == motherLotCompColor)
                                {
                                    MotherLot = dgv_split.Rows[rowNo].Cells["splitLot"].Value.ToString();
                                    MotherLotDCC = dgv_split.Rows[rowNo].Cells["splitDCC"].Value.ToString();
                                }
                            }
                        }
                    }

                    //해당위치에 컬럼명을 담기
                    //worksheet1.get_Range("A1", columns[MtlList.Columns.Count - 1] + "1").Value2 = headers;
                    //해당위치부터 데이터정보를 담기

                    worksheet1.get_Range("A1").Value = "MotherLot#";
                    worksheet1.get_Range("B1").Value = "Dcc";
                    worksheet1.get_Range("C1").Value = "SplitLot#";
                    worksheet1.get_Range("D1").Value2 = "Dcc";
                    worksheet1.get_Range("E1").Value2 = "SplitDieQty";
                    worksheet1.get_Range("F1").Value2 = "SplitWaferQty";
                    worksheet1.get_Range("G1").Value2 = "SplitCust";
                    worksheet1.get_Range("H1").Value2 = "SplitSourceDevice";
                    worksheet1.get_Range("I1").Value2 = "Wafer ID / Qty";
                    worksheet1.get_Range("J1").Value2 = "WaferLot#";
                    worksheet1.get_Range("K1").Value2 = "Custinfo(SWR)";
                    worksheet1.get_Range("L1").Value2 = "PC Memo1";
                    worksheet1.get_Range("M1").Value2 = "PC Memo2";

                    rd = worksheet1.Range["A1", $"M{dgv_split.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value.ToString().Contains("-") == true).ToList().Count + 1}"];
                    //rd.BorderAround2(XlLineStyle.xlDash);
                    //rd.Borders[XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //rd.Borders[XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlContinuous;

                    rd.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    rd.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThick;
                    rd.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThick;

                    //SetWaferReturnProgressba("엑셀 양식 작성 완료...", 4);

                    //SetWaferReturnProgressba("Data 입력 중...", 5);
                    worksheet1.get_Range("A2", columns[item.GetLength(1) - 1] + (item.GetLength(0) + 1).ToString()).Value = item;
                    worksheet1.get_Range("A2", columns[item.GetLength(1) - 1] + (item.GetLength(0) + 1).ToString()).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    worksheet1.Cells.NumberFormat = @"@";
                    worksheet1.Columns.AutoFit();


                    string filePath = "";

                    //SetWaferReturnProgressba("파일 저장 중...", 7);

                    if (Properties.Settings.Default.SplitExcelSavePath != "")
                    {
                        filePath = string.Format("{0}\\Split_{1}.xls", Properties.Settings.Default.SplitExcelSavePath, DateTime.Now.ToString("yyyyMMddhhmmss"));
                        workbook.SaveAs(filePath, Excel.XlFileFormat.xlExcel8, System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                    }
                    else
                    {
                        filePath = string.Format("{0}\\Split_{1}.xls", System.Environment.CurrentDirectory + "\\Split", DateTime.Now.ToString("yyyyMMddhhmmss"));
                        workbook.SaveAs(filePath, Excel.XlFileFormat.xlExcel8, System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                    }

                    speech.SpeakAsync("엑셀 저장 완료");
                    //SetWaferReturnProgressba("파일 저장 완료", 8);

                    workbook.Close();
                    application.Quit();

                    releaseObject(application);
                    releaseObject(worksheet1);
                    releaseObject(workbook);

                    SetWaferReturnProgressba("Excel 종료", 9);

                    if (DialogResult.Yes == MessageBox.Show("파일을 여시겠습니까?", "file open?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        ProcessStartInfo info = new ProcessStartInfo("excel.exe", filePath);
                        Process.Start(info);
                    }


                    //SetWaferReturnProgressba("Excel 실행 완료", 10);

                }
                else
                {
                    MessageBox.Show("데이터가 없습니다.");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_splitExportExcel_Click(object sender, EventArgs e)
        {

        }

        private void btn_splitExportExcel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();

                fd.ShowNewFolderButton = true;

                if (Properties.Settings.Default.SplitExcelSavePath == "")
                    fd.SelectedPath = Environment.SpecialFolder.Desktop.ToString();
                else
                    fd.SelectedPath = Properties.Settings.Default.SplitExcelSavePath;

                if (DialogResult.OK == fd.ShowDialog())
                {
                    Properties.Settings.Default.SplitExcelSavePath = fd.SelectedPath;
                    Properties.Settings.Default.Save();

                    toolTip1.SetToolTip(btn_splitExportExcel, string.Format("{0}\n경로 변경 : 마우스 오른쪽 클릭", Properties.Settings.Default.SplitExcelSavePath));
                }
            }
            else
            {
                exportSplitData();
            }
        }

        private void btn_splitExportExcel_Click_1(object sender, EventArgs e)
        {

        }

        private void SaveShelfData()
        {
            Properties.Settings.Default.ShelfPreFix = tb_PreFix.Text;
            Properties.Settings.Default.ShelfStartShelf = tb_StartShelf.Text;
            Properties.Settings.Default.ShelfEndShelf = tb_EndShelf.Text;
            Properties.Settings.Default.ShelfStartBox = tb_StartBox.Text;
            Properties.Settings.Default.ShelfEndBox = tb_EndBox.Text;
            Properties.Settings.Default.Save();
        }



        private void btn_ShelfSearch_Click(object sender, EventArgs e)
        {
            string url = "";
            int Reelcnt = 0;
            int StartShelf = int.Parse(tb_StartShelf.Text);
            int EndShelf = int.Parse(tb_EndShelf.Text);
            int StartBox = int.Parse(tb_StartBox.Text);
            int EndBox = int.Parse(tb_EndBox.Text);

            SaveShelfData();

            cb_ShelfCust.Items.Clear();

            dgv_Shelf.Rows.Clear();


            //if (EndShelf == 0 && EndBox == 0)
            //{

            //}
            //else
            {
                
                int CustIndex = -1;

                if(rb_Range.Checked == true)
                {
                    GetShelfRange();
                }
                else if(rb_OneByOne.Checked == true)
                {
                    GetShelf();
                }

            }
        }

        private void GetShelf()
        {
            string url = "";
            int Reelcnt = 0;
            int CustIndex = -1;

            try
            {
                url = $"http://10.101.14.130:8180/eMES_Webservice/diebank_automation_service/inq_auto_gr_ent_list/{Properties.Settings.Default.LOCATION},%20,{tb_OnebyOne.Text},%20";
                string[] temp = GetWebServiceData(url).Split('\r');

                for (int i = 1; i < temp.Length; i++)
                {
                    string[] row = temp[i].Replace("\n", "").Split('\t');

                    dgv_Shelf.Rows.Add(new object[] { ++Reelcnt, row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10], row[11], row[12], row[13], row[14], row[15], row[16], row[17] });

                    if (dgv_Shelf.Rows[dgv_Shelf.RowCount - 1].Cells["Shelf_ReelIDDCC"].Value.ToString() != "")
                        dgv_Shelf.Rows[dgv_Shelf.RowCount - 1].DefaultCellStyle.BackColor = ShelfCompleteColor;

                    CustIndex = cb_ShelfCust.Items.IndexOf(row[0]);

                    if (CustIndex == -1)
                    {
                        cb_ShelfCust.Items.Add(row[0]);
                    }

                }

                SetShelfProgressVal(0);


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void GetShelfRange()
        {
            string url = "";
            int Reelcnt = 0;
            int StartShelf = int.Parse(tb_StartShelf.Text);
            int EndShelf = int.Parse(tb_EndShelf.Text);
            int StartBox = int.Parse(tb_StartBox.Text);
            int EndBox = int.Parse(tb_EndBox.Text);
            int CustIndex = -1;

            SetShelfProgressMax((EndShelf - StartShelf + 1) * (EndBox - StartBox + 1));

            for (int nShelf = StartShelf; nShelf <= EndShelf; nShelf++)
            {
                for (int nBox = StartBox; nBox <= EndBox; nBox++)
                {
                    try
                    {
                        url = $"http://10.101.14.130:8180/eMES_Webservice/diebank_automation_service/inq_auto_gr_ent_list/{Properties.Settings.Default.LOCATION},%20,{tb_PreFix.Text}{nShelf.ToString().PadLeft(3, '0')}{nBox.ToString().PadLeft(2, '0')},%20";
                        string[] temp = GetWebServiceData(url).Split('\r');

                        for (int i = 1; i < temp.Length; i++)
                        {
                            string[] row = temp[i].Replace("\n", "").Split('\t');

                            dgv_Shelf.Rows.Add(new object[] { ++Reelcnt, row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10], row[11], row[12], row[13], row[14], row[15], row[16], row[17] });

                            if (dgv_Shelf.Rows[dgv_Shelf.RowCount - 1].Cells["Shelf_ReelIDDCC"].Value.ToString() != "")
                                dgv_Shelf.Rows[dgv_Shelf.RowCount - 1].DefaultCellStyle.BackColor = ShelfCompleteColor;

                            CustIndex = cb_ShelfCust.Items.IndexOf(row[0]);

                            if (CustIndex == -1)
                            {
                                cb_ShelfCust.Items.Add(row[0]);
                            }

                        }

                        SetShelfProgressVal((nShelf - StartShelf) * (EndBox - StartBox + 1) + (nBox - StartBox));


                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
            }
        }

        private void SetShelfProgressMax(int max)
        {
            pb_Shelf.Maximum = max;
        }

        private void SetShelfProgressVal(int val)
        {
            pb_Shelf.Value = val;
            pb_Shelf.Update();
            pb_Shelf.Invalidate();

            l_ShelfProgress.Text = $"{pb_Shelf.Value}/{pb_Shelf.Maximum}";
            l_ShelfProgress.Update();
            l_ShelfProgress.Invalidate();
        }

        private void tb_ShelfScan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                ShelfLogFileSave(tb_ShelfScan.Text);

                int CustIndex = cb_ShelfCust.SelectedIndex;

                if (tb_ShelfScan.Text.Contains(":") == false)
                {
                    Bcrinfo bcr = K4_Parsing(tb_ShelfScan.Text.Replace("\r", ""));

                    if (bcr.result.ToUpper() == "OK" || bcr.result.ToUpper() == "DUPLICATE")
                    {
                        ShelfFindReel(bcr);
                    }
                }
                else if(tb_ShelfScan.Text.Contains(":") == true)
                {
                    Bcrinfo bcr = AmkorLabel2BCRINFO(tb_ShelfScan.Text);
                    ShelfValidation(bcr);
                }
                tb_ShelfScan.Text = "";
            }
        }

        private Bcrinfo AmkorLabel2BCRINFO(string AmkorLabel)
        {
            Bcrinfo Result = new Bcrinfo();
            string[] LabelInfo = AmkorLabel.Split(':');
            //lot                   dcc   device                qty       wqty   amkorID  cust
            //P3UK61.00-12        :     :4KL1-2802-TR1C      :0000015148:00002:0012395166:00488

            Result.Lot = LabelInfo[0].Trim();
            Result.DCC = LabelInfo[1].Trim();
            Result.Device = LabelInfo[2].Trim();
            Result.DieQty = LabelInfo[3].Trim();
            Result.WfrQty = LabelInfo[4].Trim();
            Result.WfrTTL = LabelInfo[4].Trim();
            
            return Result;
        }

        private void ShelfValidation(Bcrinfo bcr)
        {
            List<DataGridViewRow> row = new List<DataGridViewRow>();

            if (bcr.Device != "")
            {
                row = dgv_Shelf.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Shelf_Device"].Value.ToString() == bcr.Device).ToList();
            }

            if (bcr.Lot != "")
            {
                if (row.Count == 0)
                {
                    row = dgv_Shelf.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Shelf_Lot"].Value.ToString() == bcr.Lot).ToList();
                }
                else
                {
                    row = row.Cast<DataGridViewRow>().Where(r => r.Cells["Shelf_Lot"].Value.ToString() == bcr.Lot).ToList();
                }
            }                                 

            //if (cb_ShelfIgnoQTY.Checked == false)
            { 
                if (bcr.DieQty != "")// && cb_ShelfIgnoQTY.Checked == false)
                {
                    if (row.Count != 0)
                    {
                        row = row.Cast<DataGridViewRow>().Where(r => r.Cells["Shelf_QTY"].Value.ToString() == bcr.DieQty).ToList();
                    }
                }
            }

            if(row.Count == 1)
            {
                dgv_Shelf.Rows[row[0].Index].DefaultCellStyle.BackColor = ShelfValidationCompColor;
                dgv_Shelf.FirstDisplayedScrollingRowIndex = row[0].Index;
                SpeakST($"{row[0].Index} 확인 완료");
            }
            else if(row.Count == 0)
            {
                SpeakST("없는 데이터");
            }
            else
            {

            }
        }

        private void ShelfFindReel(Bcrinfo bcr)
        {
            List<DataGridViewRow> row = new List<DataGridViewRow>();

            if (bcr.Device != "")
            {
                row = dgv_Shelf.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Shelf_Device"].Value.ToString() == bcr.Device).ToList();
            }

            if (bcr.Lot != "")
            {
                if (row.Count == 0)
                {
                    row = dgv_Shelf.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Shelf_Lot"].Value.ToString() == bcr.Lot).ToList();
                }
                else
                {
                    row = row.Cast<DataGridViewRow>().Where(r => r.Cells["Shelf_Lot"].Value.ToString() == bcr.Lot).ToList();
                }
            }


            if (bcr.DieQty != "" && cb_ShelfIgnoQTY.Checked == false)
            {
                if (row.Count != 0)
                {
                    row = row.Cast<DataGridViewRow>().Where(r => r.Cells["Shelf_QTY"].Value.ToString() == bcr.DieQty).ToList();
                }
            }


            if (row.Count == 1)
            {
                if (dgv_Shelf.Rows[row[0].Index].DefaultCellStyle.BackColor == ShelfCompleteColor)
                {
                    SpeakST("스캔 완료");
                    return;
                }

                dgv_Shelf.FirstDisplayedScrollingRowIndex = row[0].Index;

                dgv_Shelf.Rows[row[0].Index].DefaultCellStyle.BackColor = ShelfCompleteColor;

                SpeakST($"{row[0].Index}");

                SetShelfProgressVal(dgv_Shelf.Rows.Cast<DataGridViewRow>().Where(r => r.DefaultCellStyle.BackColor == ShelfCompleteColor).ToList().Count);

                StorageData storageData = ShelfGetData(row[0].Index);

                string[] ReelIDDCC = ReelIDUpdate(storageData);

                dgv_Shelf.Rows[row[0].Index].Cells["Shelf_ReelIDDCC"].Value = $"{ReelIDDCC[0]}/{ReelIDDCC[1]}";
                //Fnc_UpdateDeviceInfo(bcr.Device, bcr.Lot, "", int.Parse(bcr.DieTTL), int.Parse(bcr.DieQty), int.Parse(bcr.WfrTTL), true, false);
                //AmkorBcrInfo Amkor = Fnc_GetAmkorBcrInfo(Form_Sort.strValReadfile, bcr.Lot, "", bcr.Device);

                storageData.ReelID = ReelIDDCC[0];
                storageData.ReelDCC = ReelIDDCC[1];

                string url = $"http://{(Properties.Settings.Default.TestMode == true ? TEST_MES : PRD_MES)}/eMES_Webservice/diebank_automation_service/rec_reel_inf/{storageData.Amkorid},{storageData.ReelID},{(storageData.ReelDCC == "" ? "%20" : storageData.ReelDCC)},{storageData.Die_Qty},{(BankHost_main.strID == "" ? "%20" : BankHost_main.strID)}";
                string res = InsertReelID(url).Result;

                if (res.ToUpper() == "OK")
                {
                    //InsertWebdata(url);
                    AmkorBcrInfo AmkorLabel = MakeAmkorLabelInfo(storageData);

                    string BarcodeInfo = MakeBCRString(row[0].Index);
                    string ZPLCode = Frm_Print.Fnc_Get_PrintFormat(1, BarcodeInfo, AmkorLabel, 1, 1);
                    
                    for(int i = 0; i< nud_Copy.Value; i++)
                    {
                        Frm_Print.Socket_MessageSend(ZPLCode);
                    }
                    

                    SpeakST("라벨출력");
                }
            }
            else if(row.Count == 0)
            {
                SpeakST("없는 Reel");
            }
            else
            {
                
            }
        }

        private AmkorBcrInfo MakeAmkorLabelInfo(StorageData data)
        {
            AmkorBcrInfo label = new AmkorBcrInfo();

            label.strAmkorid = data.Amkorid;
            label.strCust = data.Cust;
            label.strLotDcc = data.Lot_Dcc;
            label.strDevice = data.Device;
            label.strDiettl = data.Die_Qty;
            label.strLotNo = data.Lot;
            label.strWaferLotNo = data.Wafer_lot;
            label.strWfrttl = data.Rcv_WQty;
            label.strWfrQty = data.Rcv_WQty;
            label.strBillNo = data.Bill;
            label.strCoo = data.strCoo;
            label.strLotType = data.Lot_type;
            label.strRcvdate = data.Rcvddate;
            label.strRID = data.ReelID;
            label.strReelDCC = data.ReelDCC;
            
            return label;
        }

        private String MakeBCRString(int index)
        {
            return $"{dgv_Shelf.Rows[index].Cells["Shelf_Lot"].Value.ToString()}:{dgv_Shelf.Rows[index].Cells["Shelf_DCC"].Value.ToString()}:{dgv_Shelf.Rows[index].Cells["Shelf_Device"].Value.ToString()}:" +
                $"{dgv_Shelf.Rows[index].Cells["Shelf_QTY"].Value.ToString()}:{dgv_Shelf.Rows[index].Cells["Shelf_WaferQTY"].Value.ToString()}:{dgv_Shelf.Rows[index].Cells["Shelf_AmkorID"].Value.ToString()}:" +
                $"{dgv_Shelf.Rows[index].Cells["Shelf_Cust"].Value.ToString()}:{dgv_Shelf.Rows[index].Cells["Shelf_WLot"].Value.ToString()}:{dgv_Shelf.Rows[index].Cells["Shelf_ReelIDDCC"].Value.ToString().Split('/')[0]}:{dgv_Shelf.Rows[index].Cells["Shelf_ReelIDDCC"].Value.ToString().Split('/')[1]}";
        }

        private StorageData ShelfGetData(int ShelfIndex)
        {
            StorageData st = new StorageData();

            st.Cust         = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_Cust"].Value.ToString();
            st.Device       = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_Device"].Value.ToString();
            st.Lot          = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_Lot"].Value.ToString();
            st.Lot_Dcc      = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_DCC"].Value.ToString();
            //st.Rcv_Qty      = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_RcvQTY"].Value.ToString();
            st.Die_Qty      = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_QTY"].Value.ToString();
            st.Rcv_WQty     = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_WaferQTY"].Value.ToString();
            st.Rcvddate     = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_RcvDate"].Value.ToString();
            st.Lot_type     = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_BuildType"].Value.ToString();
            st.Bill         = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_Bill"].Value.ToString();
            st.Amkorid      = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_AmkorID"].Value.ToString();
            st.Wafer_lot    = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_WLot"].Value.ToString();
            st.strCoo       = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_COO"].Value.ToString();

            if (dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_ReelIDDCC"].Value.ToString().Contains('/') == true)
            {
                st.ReelID = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_ReelIDDCC"].Value.ToString().Split('/')[0];
                st.ReelDCC = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_ReelIDDCC"].Value.ToString().Split('/')[1];
            }
            //st.state        = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_"].Value.ToString();
            //st.strop        = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_"].Value.ToString();
            //st.strGRstatus  = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_"].Value.ToString();
            //st.Default_WQty = dgv_Shelf.Rows[ShelfIndex].Cells["Shelf_"].Value.ToString();
            //st.WSN          = strWSN;

            return st;
        }

        private void cb_ShelfCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShelfCust = cb_ShelfCust.SelectedIndex;
            Properties.Settings.Default.Save();

            int CustIndex = cb_ShelfCust.SelectedIndex;

            Fnc_Get_Information_Model(cb_ShelfCust.Text, cb_ShelfCustName);

            cb_ShelfCustName.SelectedIndex = 0;
            
        }

        private void cb_ShelfCustName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShelfCustName = cb_ShelfCustName.SelectedIndex;
            Properties.Settings.Default.Save();

            BankHost_main.strCustName = comboBox_Name.Text;
        }

        private void btn_ShelfStart_Click(object sender, EventArgs e)
        {
            if (cb_ShelfCustName.Text.Contains("모델") == true)
            {
                SpeakST("모델명을 선택해 주세요");
                return;
            }

            if (btn_ShelfStart.Text.ToUpper() == "START")
            {
                cb_ShelfCust.Enabled = false;
                cb_ShelfCustName.Enabled = false;
                tb_ShelfScan.Enabled = true;
                btn_ShelfSearch.Enabled = false;

                btn_ShelfStart.Text = "STOP";
            }
            else
            {
                cb_ShelfCust.Enabled = true;
                cb_ShelfCustName.Enabled = true;
                tb_ShelfScan.Enabled = false;
                btn_ShelfSearch.Enabled = true;

                btn_ShelfStart.Text = "START";
            }

            Fnc_Get_WorkBcrInfo(cb_ShelfCust.Text, cb_ShelfCustName.Text);
        }

        private void tb_ShelfScan_ImeModeChanged(object sender, EventArgs e)
        {
            tb_ShelfScan.ImeMode = ImeMode.Alpha;
            Debug.WriteLine($"{tb_ShelfScan.ImeMode} ? {ImeMode.Alpha}");
        }

        private void tb_ATVScan_ImeModeChanged(object sender, EventArgs e)
        {
            tb_ATVScan.ImeMode = ImeMode.Alpha;
        }

        private void textBox1_ImeModeChanged(object sender, EventArgs e)
        {
            textBox1.ImeMode = ImeMode.Alpha;
        }

        private void tb_scrapinput_ImeModeChanged(object sender, EventArgs e)
        {
            tb_scrapinput.ImeMode = ImeMode.Alpha;
        }

        private void tb_WaferReturnScan_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tb_WaferReturnScan.ImeMode = ImeMode.Alpha;
        }

        private void tb_ShelfScan_MouseClick(object sender, MouseEventArgs e)
        {
            tb_ShelfScan.ImeMode = ImeMode.Alpha;

            SetShelfProgressMax(dgv_Shelf.RowCount);
            SetShelfProgressVal(dgv_Shelf.Rows.Cast<DataGridViewRow>().Where(r => r.DefaultCellStyle.BackColor == ShelfCompleteColor).ToList().Count);
        }

        private void cb_ReturnReel_CheckedChanged(object sender, EventArgs e)
        {
            if(cb_ReturnReel.Checked == true)
            {
                dgv_split.Columns["SplitQTY"].ReadOnly = false;
                dgv_split.Columns["SplitLot"].ReadOnly = false;
            }
            else
            {
                dgv_split.Columns["SplitQTY"].ReadOnly = true;
                dgv_split.Columns["SplitLot"].ReadOnly = true;
            }
        }

        private void dgv_split_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(dgv_split.Rows[e.RowIndex].Cells["SplitNo"].Value.ToString().Contains('-') == true)
            {
                SplitCheckSplitComp(e.RowIndex);
                tb_splitScan.Focus();
            }
            
        }

        int ShelfClickedIndex = -1;

        private void dgv_Shelf_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void ContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Text == "Reset")
            {
                dgv_Shelf.Rows[ShelfClickedIndex].DefaultCellStyle.BackColor = Control.DefaultBackColor;
            }
            else if(e.ClickedItem.Text == "Find")
            {
                ShelfFindIndex = -1;
                frm_Find FindFrom = new frm_Find();
                FindFrom.FindEvent += FindFrom_FindEvent;
                FindFrom.ShowDialog();
            }
            else if(e.ClickedItem.Text == "Reset All")
            {
                for(int i = 0; i< dgv_Shelf.RowCount; i++)
                {
                    dgv_Shelf.Rows[i].DefaultCellStyle.BackColor = Control.DefaultBackColor;
                }                
            }

        }


        int ShelfFindIndex = -1;
        private void FindFrom_FindEvent(string Lot)
        {
            List<DataGridViewRow> rows = dgv_Shelf.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["Shelf_Lot"].Value.ToString().Contains(Lot) == true || r.Cells["Shelf_Device"].Value.ToString().Contains(Lot) == true).ToList();

            if(ShelfFindIndex != -1)
                dgv_Shelf.Rows[ShelfFindIndex].Selected = false;

            foreach(DataGridViewRow row in rows)
            {
                if(row.Index > ShelfFindIndex)
                {
                    ShelfFindIndex = row.Index;
                    dgv_Shelf.FirstDisplayedScrollingRowIndex = row.Index;
                    dgv_Shelf.Rows[row.Index].Selected = true;
                    break;
                }
                else if(ShelfFindIndex == rows[rows.Count -1].Index)
                {
                    ShelfFindIndex = rows[0].Index;
                    dgv_Shelf.FirstDisplayedScrollingRowIndex = rows[0].Index;
                    dgv_Shelf.Rows[rows[0].Index].Selected = true;
                }
            }
        }

        private void dgv_Shelf_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void dgv_Shelf_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Items.Clear();
                contextMenuStrip1.Items.Add("Find");
                contextMenuStrip1.Items.Add("Reset");
                contextMenuStrip1.Items.Add("Reset All");
                ShelfClickedIndex = e.RowIndex;

                contextMenuStrip1.ItemClicked += ContextMenuStrip1_ItemClicked;
            }
        }

        private void dataGridView_Lot_MouseClick(object sender, MouseEventArgs e)
        {
            ClickTime();
        }

        private void tb_ATVScan_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == System.Windows.Forms.Keys.Enter)
            {   //CR6-113852          :     :D6QM76300M12R7      :0000003308:00001:0011381987:00948:2227749:
                //Location	Lot# / Dcc	Cust	Source Device	DIE QTY	WFR QTY	제조자 (maker)  	제조사 주소

                string[] d = tb_ATVScan.Text.Split(':');

                if (d.Length < 6)
                    return;

                System.Collections.Generic.IEnumerable<DataGridViewRow> matchingRows = dgv_ATVLabel.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[1].Value.ToString() == d[0].Trim() && r.Cells[3].Value.ToString() == d[2].Trim() && int.Parse(r.Cells[4].Value.ToString()) == int.Parse(d[3].ToString()));

                int index = dgv_ATVLabel.Rows.IndexOf(matchingRows.FirstOrDefault());

                if(index == -1)
                {
                    SpeakST("자재 없음");
                }
                else
                {
                    dgv_ATVLabel.Rows[index].DefaultCellStyle.BackColor = Color.Blue;
                    dgv_ATVLabel.Rows[index].Selected = true;
                    dgv_ATVLabel.FirstDisplayedScrollingRowIndex = index;

                    string PrintCode = "^XA";
                    PrintCode += $"^FO50,10^FDItem name: Raw Die({dgv_ATVLabel.Rows[index].Cells[1].Value.ToString()})^CF0,40^FS";
                    PrintCode += $"^FO50,60^FDMaker: {dgv_ATVLabel.Rows[index].Cells[6].Value.ToString()}^CF0,40^FS";
                    PrintCode += $"^FO50,110^FDMaker Address : {dgv_ATVLabel.Rows[index].Cells[7].Value.ToString()}^CF0,40^FS";
                    PrintCode += $"^FO50,160^FDOrigin : {dgv_ATVLabel.Rows[index].Cells[8].Value.ToString()}^CF0,40^FS";

                    PrintCode += "^XZ";
                    byte[] zpl = Encoding.UTF8.GetBytes(PrintCode);

                    using (System.Net.Sockets.TcpClient socket = new System.Net.Sockets.TcpClient())
                    {

                        //IPAddress ip = IPAddress.Parse(Properties.Settings.Default.SecondPrinterIP);

                        socket.Connect("10.131.34.21", 9100);
                        StreamWriter writer = new StreamWriter(socket.GetStream());

                        for (int i = 0; i < (int)nud_ATV.Value; i++)
                        {
                            writer.Write(PrintCode);
                            writer.Flush();
                        }
                        writer.Close();
                    }

                }

                tb_ATVScan.Text = "";
            }
        }

        private void Split_data_sorting()
        {
            try
            {
                List<string[]> Split_list = new List<string[]>();
                string strFileName = string.Format("{0}\\Work\\Split_log\\{1}.txt", strExcutionPath, DateTime.Now.ToShortDateString());

                string[] temp = System.IO.File.ReadAllLines(strFileName);

                for (int i = 0; i < temp.Length; i++)
                {
                    Split_list.Add(temp[i].Split('\t'));
                }

                for (int i = 1; i < Split_list.Count - 1; i++)
                {
                    split_log_lowdata.Add(string.Join(";", Split_list[i]));

                    if (split_log_cust.Contains(Split_list[i][1]) == false)
                    {
                        split_log_cust.Add(Split_list[i][1]);
                        split_log_Linecode.Add(Split_list[i][1] +";"+ Split_list[i][0]);
                    }
                    else
                    {
                        if (split_log_Linecode.Contains(Split_list[i][0]) == false)
                        {
                            split_log_Linecode.Add(Split_list[i][1] + ";" + Split_list[i][0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void ShelfLogFileSave(string split_data)
        {
            string folderpath = strExcutionPath + "\\Work\\Shelf_log";
            string strFileName = string.Format("{0}\\Work\\Shelf_log\\{1}.txt", strExcutionPath, DateTime.Now.ToShortDateString());
            bool bdata = false;
            List<string> added_string = new List<string>();
            List<string> Split_list = new List<string>();
            FileStream stream = null;

            string[] temp = split_data.Split('\n');
            string[] files = new string[10];
            string[] sp;

            try
            {
                for (int i = 1; i < temp.Length; i++)
                {
                    if (temp[i] != "")
                    {
                        Split_list.Add(temp[i].Remove(temp[i].Length - 1));
                    }
                }

                if (System.IO.Directory.Exists(folderpath) == false)
                {
                    System.IO.Directory.CreateDirectory(folderpath);
                }

                if (System.IO.File.Exists(strFileName) == false)
                {
                    stream = System.IO.File.Create(strFileName);
                    stream.Dispose();
                }
                else
                {

                }

                files = System.IO.File.ReadAllLines(strFileName);
                string t = "";
                for (int i = 0; i < Split_list.Count; i++)
                {
                    bdata = false;
                    for (int j = 0; j < files.Length; j++)
                    {
                        t = string.Join("\t", files[j].Split('\t'), 0, files[j].Split('\t').Length == 12 ? files[j].Split('\t').Length - 2 : 10);

                        if (Split_list[i] == t)
                        {
                            bdata = true;
                            break;
                        }
                    }

                    if (bdata == false)
                        added_string.Add(Split_list[i]);
                }

                System.IO.StreamWriter fs = new StreamWriter(strFileName);

                string[] arr = new string[files.Length + added_string.Count];
                Array.Copy(files, arr, files.Length);


                if (!(added_string.Count == 1 && added_string[0] == ""))
                    Array.Copy(added_string.ToArray(), 0, arr, files.Length, added_string.Count);

                fs.Write(String.Join(Environment.NewLine, arr.Take(arr.Length).ToArray()));

                fs.Dispose();
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        private void SplitLogFileSave(string split_data)
        {
            string folderpath = strExcutionPath + "\\Work\\Split_log";
            string strFileName = string.Format("{0}\\Work\\Split_log\\{1}.txt", strExcutionPath, DateTime.Now.ToShortDateString());
            bool bdata = false;
            List<string> added_string = new List<string>();
            List<string> Split_list = new List<string>();
            FileStream stream = null;

            string[] temp = split_data.Split('\n');
            string[] files = new string[10];
            string[] sp;

            try
            {
                for (int i = 1; i < temp.Length; i++)
                {
                    if (temp[i] != "")
                    {                        
                        Split_list.Add(temp[i].Remove(temp[i].Length-1));
                    }
                }

                if (System.IO.Directory.Exists(folderpath) == false)
                {
                    System.IO.Directory.CreateDirectory(folderpath);
                }

                if (System.IO.File.Exists(strFileName) == false)
                {
                    stream = System.IO.File.Create(strFileName);
                    stream.Dispose();
                }
                else
                {

                }

                files = System.IO.File.ReadAllLines(strFileName);
                string t = "";
                for (int i = 0; i < Split_list.Count; i++)
                {
                    bdata = false;
                    for (int j = 0; j < files.Length; j++)
                    {
                         t = string.Join("\t", files[j].Split('\t'), 0, files[j].Split('\t').Length == 12 ? files[j].Split('\t').Length-2 : 10);

                        if (Split_list[i] == t)
                        {
                            bdata = true;
                            break;
                        }
                    }

                    if (bdata == false)
                        added_string.Add(Split_list[i]);
                }

                System.IO.StreamWriter fs = new StreamWriter(strFileName);

                string[] arr = new string[files.Length + added_string.Count];
                Array.Copy(files, arr, files.Length);


                if (!(added_string.Count == 1 && added_string[0] == ""))
                    Array.Copy(added_string.ToArray(), 0, arr, files.Length, added_string.Count);
                
                fs.Write(String.Join(Environment.NewLine, arr.Take(arr.Length).ToArray()));

                fs.Dispose();
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        private void rb_Range_CheckedChanged_1(object sender, EventArgs e)
        {
            rb_OneByOne.Checked = !rb_Range.Checked;
            tb_OnebyOne.Enabled = !rb_Range.Checked;
        }

        private void rb_OneByOne_CheckedChanged(object sender, EventArgs e)
        {
            rb_Range.Checked = !rb_OneByOne.Checked;
            tb_PreFix.Enabled = !rb_OneByOne.Checked;
            tb_StartShelf.Enabled = !rb_OneByOne.Checked;
            tb_StartBox.Enabled = !rb_OneByOne.Checked;
            tb_EndShelf.Enabled = !rb_OneByOne.Checked;
            tb_EndBox.Enabled = !rb_OneByOne.Checked;
        }

        private void Split_log_new_file_save(string split_data)
        {
            string folderpath = strExcutionPath + "\\Work\\Split_log";
            string strFileName = string.Format("{0}\\Work\\Split_log\\{1}.txt", strExcutionPath, DateTime.Now.ToShortDateString());
            
            string[] temp = split_data.Split('\n');
            string[] files = new string[10];
            
            try
            {
                if (File.Exists(strFileName) == true)
                    File.Delete(strFileName);

                System.IO.StreamWriter fs = new StreamWriter(strFileName);
            
                fs.Write(split_data);

                fs.Dispose();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void ChangeIME(System.Windows.Forms.Control ctl)
        {
            IntPtr context = ImmGetContext(ctl.Handle);
            Int32 dwConversion = 0;
            dwConversion = IME_CMODE_ALPHANUMERIC;
            ImmSetConversionStatus(context, dwConversion, 0);
        }


        bool GetIME()
        {
            Process p = Process.GetProcessesByName(System.Windows.Forms.Application.ProductName).FirstOrDefault();

            if (p == null)
                return false;

            IntPtr hwnd = p.MainWindowHandle;
            IntPtr hime = ImmGetDefaultIMEWnd(hwnd);
            IntPtr status = SendMessage(hime, WM_IME_CONTROL, new IntPtr(0x5), new IntPtr(0));


            
            if (status.ToInt32() != 0)
                return true;  
            
            return false;
        }

        bool bselected_mode_index = false;
        bool bmode6 = false, bmode7 = false, bmode8 = false, bmode9 = false;


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int nIndex_Lot = dataGridView_Lot.CurrentCell.RowIndex;
            int nIndex_Device = dataGridView_Device.CurrentCell.RowIndex;
            string strOrgLot = dataGridView_Lot.Rows[nIndex_Lot].Cells[1].Value.ToString();
            string strDevice = dataGridView_Device.Rows[nIndex_Device].Cells[1].Value.ToString();
            string strState = dataGridView_Lot.Rows[nIndex_Lot].Cells[6].Value.ToString();

            if (strState == "Complete")
            {
                MessageBox.Show("완료 된 Lot 입니다.\n\n완료된 Lot는 변경 할 수 없습니다.");

                return;
            }

            Form_Lotchange Frm_Lotchange = new Form_Lotchange();

            Frm_Lotchange.Fnc_Set_OrgName(strOrgLot);
            Frm_Lotchange.ShowDialog();

            if (strNewLotname == "")
                return;

            int nJudge = Fnc_ChangeLotName(strDevice, strOrgLot, strNewLotname);

            if (nJudge == 0)
            {
                string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
                string strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strDevice + "\\" + strDevice;

                string strlog = string.Format("P{0}+L{1}+Q{2}+000000+{3}Lot이름변경 Org:{4}", strDevice, strNewLotname, 0, BankHost_main.strOperator, strOrgLot);
                //Fnc_SaveLog_Work(strFileName, strlog);
                //Fnc_SaveLog_Work(strFileName_Device, strlog);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nProcess == 1000 && bRun == true && nResult == 1000)
            {
                nResult = 0;

                label_msg.Text = "-";

                int nJudge = 0;

                try
                {
                    nJudge = Fnc_UpdateDeviceInfo(strValDevice, strValLot, "", nValDiettl, nValDieQty, nValWfrttl, bupdate, bunprinted_device);
                }
                catch
                {
                    nJudge = -1;
                }

                nResult = nJudge;

                if (nResult == -1)
                    BankHost_main.Host.Host_Delete_BcrReadinfo(BankHost_main.strEqid, strValLot, 0);

                if (tabControl_Sort.SelectedIndex == 1)
                {
                    Fnc_WorkDownload(strWorkFileName);
                    Fnc_Find(strValLot);
                }
                else
                {
                    if (nResult == 1)
                    {
                        nLabelcount++;

                        label_msg.Text = "LABEL PRINT - IDX:" + Form_Sort.nLabelcount.ToString();
                    }
                    else if (nResult == 2)
                    {
                        label_msg.Text = "LABEL PRINT / 구성완료";
                        nLabelcount++;
                    }
                    else if (nResult == 0)
                        label_msg.Text = string.Format("{0} (WFR-{1})", strValLot, label_info_wfrqty.Text);
                }
                bRun = false;
                nProcess = 0;
            }
        }

        public void Fnc_Hist_Init()
        {
            comboBox_hist_device.SelectedIndex = 0;

            DateTime dToday = DateTime.Now;
            dateTimePicker_st.Value = dToday.Date;
            dateTimePicker_ed.Value = dToday.Date;

            comboBox_Hour_st.SelectedIndex = 8;
            comboBox_Min_st.SelectedIndex = 6;

            comboBox_Hour_ed.SelectedIndex = 17;
            comboBox_Min_ed.SelectedIndex = 6;

            label_histsel.Text = "-";
            textBox_input.Enabled = false;
            dateTimePicker_st.Enabled = false;
            dateTimePicker_ed.Enabled = false;
            comboBox_Hour_st.Enabled = false;
            comboBox_Hour_ed.Enabled = false;
            comboBox_Min_st.Enabled = false;
            comboBox_Min_ed.Enabled = false;
        }

        public void Fnc_Hist_DeviceLoad()
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + ".txt";

            string[] info = Fnc_ReadFile(strFileName);

            if (info == null)
                return;

            comboBox_hist_device.Items.Clear();

            for (int n = 0; n < info.Length; n++)
            {
                comboBox_hist_device.Items.Add(info[n]);
            }
        }

        public void Fnc_Hist_Load(string strDevice)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName;
            string strReadfile = strFileName + "\\" + strDevice + "\\" + strDevice + "_Worklog" + ".txt";

            string[] info = Fnc_ReadFile(strReadfile);

            dataGridView_hist.Columns.Clear();
            dataGridView_hist.Rows.Clear();
            dataGridView_hist.Refresh();

            if (info == null)
                return;

            dataGridView_hist.Columns.Add("작업일자", "작업일자");
            dataGridView_hist.Columns.Add("시간", "시간");
            dataGridView_hist.Columns.Add("Job", "Job");
            dataGridView_hist.Columns.Add("Device", "Device");
            dataGridView_hist.Columns.Add("Lot", "Lot");
            dataGridView_hist.Columns.Add("Die Qty", "Die Qty");
            dataGridView_hist.Columns.Add("Die ttl", "Die ttl");
            dataGridView_hist.Columns.Add("Wfr Qty", "Wfr Qty");
            dataGridView_hist.Columns.Add("State", "State");
            dataGridView_hist.Columns.Add("작업자", "작업자");

            for (int n = 0; n < info.Length; n++)
            {
                string[] strSplit_data = info[n].Split(',');
                string strdate = strSplit_data[0];
                string strtime = strSplit_data[1];

                string[] strSplit_bcr = strSplit_data[2].Split('+');

                if (strSplit_bcr.Length == 8)
                {
                    string strJob = strSplit_bcr[0];
                    string strDe = strSplit_bcr[1];
                    string strLot = strSplit_bcr[2];
                    string strdieqty = strSplit_bcr[3];
                    string strdiettl = strSplit_bcr[4];
                    string strwfrqty = strSplit_bcr[5];
                    string strstate = strSplit_bcr[6];
                    string strop = strSplit_bcr[7];

                    dataGridView_hist.Rows.Add(new object[10] { strdate, strtime, strJob, strDe, strLot, strdieqty, strdiettl, strwfrqty, strstate, strop });
                }
            }
        }

        public void Fnc_Information_Init()
        {
            Form_Input Frm_Input = new Form_Input();

            Frm_Input.Fnc_Init(0);

            int nTotal = dataGridView_worklist.Rows.Count;

            string strSetbillinfo = "", strSetCustinfo = "";
            int nLotcount = 0;

            Frm_Input.Fnc_cust_init();
            Frm_Input.Fnc_datagrid_init();

            var dtWorkinfo = BankHost_main.Host.Host_Get_Workinfo_All();

            for (int i = 0; i < nTotal; i++)
            {
                string strCust = dataGridView_worklist.Rows[i].Cells[1].Value.ToString();
                string strBill = dataGridView_worklist.Rows[i].Cells[9].Value.ToString();

                if (strSetbillinfo != strBill)
                {
                    if (strSetbillinfo != "")
                    {
                        bool bWorkcheck = false;
                        string strEqid = "", strHAWB = "", strState = "", strinfo = "";
                        /////////
                        if (dtWorkinfo.Rows.Count < 1)
                            bWorkcheck = false;
                        else
                        {
                            string strToday = string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                            for (int n=0; n< dtWorkinfo.Rows.Count; n++)
                            {
                                string strDate = dtWorkinfo.Rows[n]["DATETIME"].ToString(); strDate = strDate.Trim();
                                strDate = strDate.Substring(0, 8);                                

                                strEqid = dtWorkinfo.Rows[n]["EQID"].ToString(); strEqid = strEqid.Trim();
                                strHAWB = dtWorkinfo.Rows[n]["HAWB"].ToString(); strHAWB = strHAWB.Trim();
                                strState = dtWorkinfo.Rows[n]["STATE"].ToString(); strState = strState.Trim();

                                if (strHAWB == strSetbillinfo && strDate == strToday)
                                {
                                    strinfo = string.Format("작업중 {0};{1};{2}", strEqid, strHAWB, strState);
                                    //n = dtWorkinfo.Columns.Count;
                                    bWorkcheck = true;

                                    Frm_Input.Fnc_datagrid_add(strSetCustinfo, strSetbillinfo, nLotcount.ToString(), strinfo);

                                    if (Frm_Input.Fnc_cust_check(strSetCustinfo))
                                        Frm_Input.Fnc_cust_add(strSetCustinfo);
                                }                      
                            }
                        }
                        
                        if(!bWorkcheck)
                            Frm_Input.Fnc_datagrid_add(strSetCustinfo, strSetbillinfo, nLotcount.ToString(), "선택 가능");                        

                        if (Frm_Input.Fnc_cust_check(strSetCustinfo))
                            Frm_Input.Fnc_cust_add(strSetCustinfo);

                        strSetbillinfo = strBill;
                        strSetCustinfo = strCust;
                        nLotcount = 1;
                    }
                    else
                    {
                        strSetbillinfo = strBill;
                        strSetCustinfo = strCust;
                        nLotcount = 1;
                    }
                }
                else
                {
                    nLotcount++;
                }
                
                if (i == nTotal - 1)
                {
                    bool bWorkcheck = false;
                    string strEqid = "", strHAWB = "", strState = "", strinfo = "";
                    /////////
                    if (dtWorkinfo.Rows.Count < 1)
                        bWorkcheck = false;
                    else
                    {
                        string strToday = string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                        for (int n = 0; n < dtWorkinfo.Rows.Count; n++)
                        {
                            string strDate = dtWorkinfo.Rows[n]["DATETIME"].ToString(); strDate = strDate.Trim();
                            strDate = strDate.Substring(0, 8);                            

                            strEqid = dtWorkinfo.Rows[n]["EQID"].ToString(); strEqid = strEqid.Trim();
                            strHAWB = dtWorkinfo.Rows[n]["HAWB"].ToString(); strHAWB = strHAWB.Trim();
                            strState = dtWorkinfo.Rows[n]["STATE"].ToString(); strState = strState.Trim();

                            if (strHAWB == strSetbillinfo && strDate == strToday)
                            {
                                strinfo = string.Format("작업중 {0};{1};{2}", strEqid, strBill, strState);
                                //n = dtWorkinfo.Columns.Count;

                                Frm_Input.Fnc_datagrid_add(strSetCustinfo, strSetbillinfo, nLotcount.ToString(), strinfo);

                                bWorkcheck = true;
                            }
                        }
                    }

                    if (!bWorkcheck)
                        Frm_Input.Fnc_datagrid_add(strCust, strSetbillinfo, nLotcount.ToString(), "선택 가능");

                    if (Frm_Input.Fnc_cust_check(strSetCustinfo))
                        Frm_Input.Fnc_cust_add(strSetCustinfo);

                    int nCnt = comboBox_Name.Items.Count;

                    if (nCnt > 0)
                        comboBox_Name.SelectedIndex = 0;
                }
            }

            Frm_Input.Fnc_datagrid_saveinfo();
            Frm_Input.ShowDialog();

            if(BankHost_main.strOperator != "")
            {
                label_opinfo.Text = BankHost_main.strOperator;
                label_cust.Text = strSelCust;

                Fnc_Get_Information_Model(strSelCust, comboBox_Name);
                if (strSelCust == "940")
                {
                    Fnc_Set_Workfile_NoDevice(strSelBillno); //HY210315
                }
                else
                    Fnc_Set_Workfile(strSelBillno);

                int n = comboBox_Name.Items.Count;

                if (n > 0)
                    comboBox_Name.SelectedIndex = 0;
            }
            else
            {
                dataGridView_worklist.Columns.Clear();
                dataGridView_worklist.Rows.Clear();
                dataGridView_worklist.Refresh();
            }
            
        }

        public void Fnc_Information_Init2(int mode)
        {
            try
            {
                Form_Input Frm_Input = new Form_Input();

                Frm_Input.Fnc_Init(mode);

                int nTotal = dataGridView_worklist.Rows.Count;

                Frm_Input.Fnc_cust_init();
                Frm_Input.Fnc_datagrid_init();

                var dtWorkinfo = BankHost_main.Host.Host_Get_Workinfo(BankHost_main.strEqid);

                string strToday = string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                for (int n = 0; n < dtWorkinfo.Rows.Count; n++)
                {
                    string strDate = dtWorkinfo.Rows[n]["DATETIME"].ToString(); strDate = strDate.Trim();
                    strDate = strDate.Substring(0, 8);

                    string strEqid = dtWorkinfo.Rows[n]["EQID"].ToString(); strEqid = strEqid.Trim();
                    string strHAWB = dtWorkinfo.Rows[n]["HAWB"].ToString(); strHAWB = strHAWB.Trim();
                    string strJobName = dtWorkinfo.Rows[n]["JOB_NAME"].ToString(); strJobName = strJobName.Trim();
                    string strCust = strJobName == "" ? "NONE" : strJobName.Substring(4, 3);
                    int nCount = n + 1;

                    if (strDate == strToday)
                    {
                        Frm_Input.Fnc_datagrid_add(strCust, strHAWB, "-", strJobName);
                    }
                }

                Frm_Input.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private delegate void Update();
        public void Fnc_BcrInfo(string strInfo)
        {
            //textBox_Readdata.Invoke(new Update(() => textBox_Readdata.Text= strInfo));
            textBox_Readdata.Invoke(new Update(() => textBox_Readdata.Text = strInfo));            
        }


        public void ClearModeComboBox()
        {
            comboBox_mode.Items.Clear();
        }

        public void AddModeComboBox(string msg)
        {
            comboBox_mode.Items.Add(msg);
        }

        public void RemoveTabPage(int index)
        {
            tabControl_Sort.TabPages.RemoveAt(index);            
        }

        public void SelecteTab(int index)
        {
            tabControl_Sort.SelectedIndex = index;
        }

        public void init_mode_combobox()
        {
            string loc = Properties.Settings.Default.LOCATION;

            comboBox_mode.Items.Clear();

            if (loc == "K4")
            {                
                comboBox_mode.Items.Add("모드1: Auto GR");
                comboBox_mode.Items.Add("모드2: Auto GR(이전 작업 불러오기)");
                comboBox_mode.Items.Add("모드3: Validation(Webservice)");
                comboBox_mode.Items.Add("모드4: Validation(이전 작업 불러오기)");
                comboBox_mode.Items.Add("모드5: Amkor Barcode Scan Printer");
                comboBox_mode.Items.Add("모드6: Location History");
                comboBox_mode.Items.Add("모드7: Split Log");
                comboBox_mode.Items.Add("모드8: Scrap");
                comboBox_mode.Items.Add("모드9: Wafer Return");
                comboBox_mode.Items.Add("모드10: Update Shelf ReelID");
                comboBox_mode.Items.Add("모드11: Update Shelf ReelID(RETURN)");
            }
            else if(loc == "K5")
            {
                
                comboBox_mode.Items.Add("모드1: Auto GR");
                comboBox_mode.Items.Add("모드2: Auto GR(이전 작업 불러오기)");
                comboBox_mode.Items.Add("모드3: Validation(Webservice)");
                comboBox_mode.Items.Add("모드4: Validation(이전 작업 불러오기)");
                comboBox_mode.Items.Add("모드5: Amkor Barcode Scan Printer)");
                comboBox_mode.Items.Add("모드6: Update Shelf ReelID");                
            }
            else if(loc == "K3")
            {                
                comboBox_mode.Items.Add("모드1: Auto GR");
                comboBox_mode.Items.Add("모드2: Auto GR(이전 작업 불러오기)");
                comboBox_mode.Items.Add("모드3: Validation(Webservice)");
                comboBox_mode.Items.Add("모드4: Validation(이전 작업 불러오기)");
                comboBox_mode.Items.Add("모드5: Amkor Barcode Scan Printer)");
            }
        }
        
       


        public void SendPrintData(string zpl)
        {
            using (System.Net.Sockets.TcpClient socket = new System.Net.Sockets.TcpClient())
            {
                string ip = Properties.Settings.Default.SecondPrinterIP;

                socket.Connect(ip, 9100);
                StreamWriter writer = new StreamWriter(socket.GetStream());

                writer.Write(zpl);
                writer.Flush();

                writer.Close();
            }                
        }
        
        
        public void PrintSummary(AmkorBcrInfo amkor)
        {
            string LotTemp = amkor.strLotNo.Split('.')[0];
            bool isComplete = true;

            List<st2ndSumLabelInfo> LabelInfo = new List<st2ndSumLabelInfo>();
            List<st2ndSumLabelInfo> LabelBuff = new List<st2ndSumLabelInfo>();

            for (int i = 0; i < dataGridView_Lot.Rows.Count; i++)
            {
                if(dataGridView_Lot.Rows[i].Cells[1].Value.ToString().Split('.')[0] == LotTemp)
                {
                    if (dataGridView_Lot.Rows[i].Cells[7].Value.ToString().ToUpper() != "COMPLETE")
                    {
                        isComplete = false;
                        break;
                    }
                    else if(dataGridView_Lot.Rows[i].Cells[7].Value.ToString().ToUpper() == "COMPLETE")
                    {
                        st2ndSumLabelInfo LabelTemp = new st2ndSumLabelInfo();

                        LabelTemp.Lot = dataGridView_Lot.Rows[i].Cells[1].Value.ToString();
                        LabelTemp.DCC = dataGridView_Lot.Rows[i].Cells[2].Value.ToString();
                        LabelTemp.DEV = strValDevice;
                        LabelTemp.QTY = dataGridView_Lot.Rows[i].Cells[3].Value.ToString();
                        LabelTemp.WFTQTY = dataGridView_Lot.Rows[i].Cells[5].Value.ToString();
                        LabelTemp.AmkorID = dataGridView_Lot.Rows[i].Cells["AmkorID"].Value.ToString();

                        LabelBuff.Add(LabelTemp);
                    }
                }
            }

            if(isComplete == true)
            {
                LabelInfo = LabelBuff.OrderBy(x => x.Lot.Length).ThenBy(x => x.Lot).ToList();

                string LabelMSG = MakeTOTLabelTemplete110X170_2();

                //Bill
                LabelMSG +=                     
                    string.Format("^FO{0},{1}^AO,30,15^FD{2}^FS", SecondLabel.QTYStartWidth + Properties.Settings.Default.SecondPrinterOffsetX, 1281 + Properties.Settings.Default.SecondPrinterOffsetY, amkor.strBillNo);

                //CUST
                LabelMSG += string.Format("^FO{0},{1}^AO,30,15^FD{2}^FS", SecondLabel.QTYStartWidth + 70 + Properties.Settings.Default.SecondPrinterOffsetX, 1080 + Properties.Settings.Default.SecondPrinterOffsetY, amkor.strCust);
                
                //WAFER
                LabelMSG += string.Format("^FO{0},{1}^AO,30,15^FD{2}^FS", SecondLabel.QTYStartWidth2 + 30 + Properties.Settings.Default.SecondPrinterOffsetX, 1080 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo.Count);


                
                

                LabelMSG += ZPL_END;

                SendPrintData(LabelMSG);
            }
        }

        public void SetLotSPR(bool val)
        {
            LotSPR = val;
        }

        public string Make1LabelZPL(List<Form_Sort.st2ndSumLabelInfo> LabelInfo, string Cust)
        {
            string LabelMSG = "";

            for (int i = 0; i < LabelInfo.Count; i++)
            {
                //Lot
                if (i < 13)
                {
                    if (LabelInfo[i].Lot.Length < 13)
                    {
                        //lot                                                                                                                  
                        LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD{2}^FS", (int)Form_Sort.SecondLabel.LotStartWidth + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + i * (int)Form_Sort.SecondLabel.LineHeight - 10 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].Lot);
                        //DCC                                                                                                                  
                        LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD/{2}^FS", (int)Form_Sort.SecondLabel.LotStartWidth + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + i * (int)Form_Sort.SecondLabel.LineHeight + 10 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].DCC);
                    }
                    else
                    {
                        //lot1
                        LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD{2}^FS", (int)Form_Sort.SecondLabel.LotStartWidth + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + i * (int)Form_Sort.SecondLabel.LineHeight - 10 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].Lot.Substring(0, 8));
                        //lot2
                        LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD{2}^FS", (int)Form_Sort.SecondLabel.LotStartWidth + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + i * (int)Form_Sort.SecondLabel.LineHeight + 10 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].Lot.Substring(8, LabelInfo[i].Lot.Length - 8));
                        //DCC
                        LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD/{2}^FS", (int)Form_Sort.SecondLabel.LotStartWidth + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + i * (int)Form_Sort.SecondLabel.LineHeight + 20 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].DCC);

                    }
                    //QTY                                                                                                                  
                    LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD{2}^FS", (int)Form_Sort.SecondLabel.QTYStartWidth + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + i * (int)Form_Sort.SecondLabel.LineHeight + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].QTY);
                    //QR 출력                                                                                                              
                    LabelMSG += string.Format("^FO{0},{1}^BQN,2,2^FD  {2}^FS", (int)Form_Sort.SecondLabel.QRStartWidth + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.QRStartHeight + i * ((int)Form_Sort.SecondLabel.LineHeight) + Properties.Settings.Default.SecondPrinterOffsetY, string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", LabelInfo[i].Lot, LabelInfo[i].DCC, Form_Sort.strValDevice, LabelInfo[i].QTY, LabelInfo[i].WFTQTY, LabelInfo[i].AmkorID, Cust));
                }
                else
                {

                    if (LabelInfo[i].Lot.Length < 13)
                    {
                        //lot
                        LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD{2}^FS", (int)Form_Sort.SecondLabel.LotStartWidth2 + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + (i - 13) * (int)Form_Sort.SecondLabel.LineHeight - 10 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].Lot);
                        //DCC
                        LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD/{2}^FS", (int)Form_Sort.SecondLabel.LotStartWidth2 + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + (i - 13) * (int)Form_Sort.SecondLabel.LineHeight + 10 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].DCC);
                    }
                    else
                    {
                        //lot
                        LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD{2}^FS", (int)Form_Sort.SecondLabel.LotStartWidth2 + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + (i - 13) * (int)Form_Sort.SecondLabel.LineHeight - 10 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].Lot.Substring(0, 8));
                        LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD{2}^FS", (int)Form_Sort.SecondLabel.LotStartWidth2 + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + (i - 13) * (int)Form_Sort.SecondLabel.LineHeight + 10 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].Lot.Substring(8, LabelInfo[i].Lot.Length - 8));
                        //DCC
                        LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD/{2}^FS", (int)Form_Sort.SecondLabel.LotStartWidth2 + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + (i - 13) * (int)Form_Sort.SecondLabel.LineHeight + 20 + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].DCC);
                    }

                    //QTY
                    LabelMSG += string.Format("^FO{0},{1}^AO,20,15^FD{2}^FS", (int)Form_Sort.SecondLabel.QTYStartWidth2 + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.StartHeight + (i - 13) * (int)Form_Sort.SecondLabel.LineHeight + Properties.Settings.Default.SecondPrinterOffsetY, LabelInfo[i].QTY);
                    //QR 출력
                    LabelMSG += string.Format("^FO{0},{1}^BQN,2,2^FD  {2}^FS", (int)Form_Sort.SecondLabel.QRStartWidth2 + Properties.Settings.Default.SecondPrinterOffsetX, (int)Form_Sort.SecondLabel.QRStartHeight + (i - 13) * ((int)Form_Sort.SecondLabel.LineHeight) + Properties.Settings.Default.SecondPrinterOffsetY, string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}", LabelInfo[i].Lot, LabelInfo[i].DCC, Form_Sort.strValDevice, LabelInfo[i].QTY, LabelInfo[i].WFTQTY, LabelInfo[i].AmkorID, Cust));
                }
            }

            return LabelMSG;
        }

        public DialogResult InputBox(string title, string content, ref string value)
        {
            Form form = new Form();
            PictureBox picture = new PictureBox();
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Button buttonOk = new System.Windows.Forms.Button();
            System.Windows.Forms.Button buttonCancel = new System.Windows.Forms.Button();

            form.ClientSize = new Size(300, 100);
            form.Controls.AddRange(new Control[] { label, picture, textBox, buttonOk, buttonCancel });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            form.TopMost = true;

            form.Text = title;
            //picture.Image = Properties.Resources.Clogo;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            label.Text = content;
            textBox.Text = value;
            buttonOk.Text = "확인";
            buttonCancel.Text = "취소";

            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            picture.SetBounds(10, 10, 50, 50);
            label.SetBounds(65, 17, 100, 20);
            textBox.SetBounds(65, 40, 220, 20);
            buttonOk.SetBounds(135, 70, 70, 20);
            buttonCancel.SetBounds(215, 70, 70, 20);

            DialogResult dialogResult = form.ShowDialog();

            value = textBox.Text;
            return dialogResult;
        }

        public int GetNumericValue()
        {
            return (int)numericUpDown1.Value;
        }

        public int GetAmkorLabelcnt()
        {
            return AmkorLabelCnt;
        }

        public void SetAmkorlabelcnt(int  cnt)
        {
            AmkorLabelCnt = cnt;
        }

        public void SetnumeriValue(int  cnt)
        {
            numericUpDown1.Value = cnt;
        }


        

    }


    
    
    public class FailURLData
    {
        public string URL = "";

        public int Retry = 0;
        public string filaMSG = "";
    }

    public class StorageData
    {
        public string Plant = "";
        public string Cust = "";
        public string Device = "";
        public string Lot = "";
        public string Lot_Dcc = "";
        public string Rcv_Qty = "";
        public string Die_Qty = "";
        public string Rcv_WQty = "";
        public string Rcvddate = "";
        public string Lot_type = "";
        public string Bill = "";     
        public string Amkorid = "";
        public string Wafer_lot = "";
        public string strCoo = "";
        public string state = "";
        public string strop = "";
        public string strGRstatus = "";
        public string Default_WQty = "";
        public string shipment = "";
        public string Invoice = "";
        public string Loc = "";
        public string Hawb = "";
        public string WSN = "";
        public string ReadFile = "";
        public string ReelID = "";
        public string ReelDCC = "";
        public string LPN = "";

        public int Retry = 0;
        public string FailMSG = "";
    }

    public class Bcrinfo
    {
        public string Device = "";
        public string Lot = "";
        public string DCC = "";
        public string DieTTL = "";
        public string DieQty = "";
        public string WfrTTL = "";
        public string WfrQty = "";
        public string result = "";
        public string WSN = ""; // 230628
        public string LPN = "";
        public bool unprinted_device = false;
    }

    public class AmkorBcrInfo
    {
        public string strCust = "";
        public string strLotNo = "";
        public string strLotDcc = "";
        public string strDevice = "";
        public string strRcvdate = "";
        public string strDieQty = "";
        public string strDiettl = "";
        public string strWfrQty = "";
        public string strWfrttl = "";
        public string strBillNo = "";
        public string strAmkorid = "";
        public string strLotType = "";
        public string strWaferLotNo = "";
        public string strCoo = "";
        public string strOperator = "";
        public string strWSN = "";
        public string strRID = "";
        public string strReelDCC = "";
    }
}
