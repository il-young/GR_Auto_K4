using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.Diagnostics;
using System.Speech.Synthesis;
using Host;
//using MICube.SmartDriver.Base.TCP;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;



namespace Bank_Host
{
    public partial class BankHost_main : Form
    {

        public enum RetBcrResult { OK = 0, READ_FAIL = 1, LOT_MISSMATCH = 2, NO_CONNECT = 3 };
        public enum RetBcrState { NOT_WORKING = 0, START = 1, NG = 2,  COMPLETE = 3 };

        public static Host.Host Host = new Host.Host();
        public static bool bHost_connect = false;

        public static int LabelAddVal = 15;// 라벨 추가로 출력할 웨이퍼 갯수
        ///Mode 추가 
        public static int nScanMode = 0; //2021.04.07 추가  0: Vision, 1: Gun, 2: 개별 입력, 3: Multi scan
        public static string strScanData = ""; //mode1,3 GunRing scan data
        public static bool bGunRingMode_Run = false;
        public static int nAmkorBcrType = 0; //2021. 06.07 추가

        public static string ReaderData = "";

        //자재 타입 추가
        public static int nMaterial_type = 0; //0: Reel, 1: Fosb

        ///

        
        //Work
        public static string strWork_Lotinfo = "";
        public static int nNGcount = 0;
        public static int nWorkBcrcount = 0, nWork_BcrType = 0;
        public static int nWorkMode = 0;
        public static int nMaxpack = 0;
        public static int nInputMode = 0;
        public static string strEqid = "";

        int nColorindex = 0;

        Bcrinfo Read_Bcr = new Bcrinfo();
        string[] Bcr_result = { "", "", "", "", "", "" };

        //Work Barcode info
        public static string strWork_Cust = "", strWork_Bank = "", strWork_Bcrcount = "", strWork_DevicePos = "", 
            strWork_LotidPos = "", strWork_LotDigit = "", strWork_QtyPos = "", strWork_SPR = "", strWork_Model = "", strWork_WSN ="",
            strWork_Shot1Lot = "", strWork_Udigit = "", strWork_WfrQtyPos = "", strWork_MtlType = "", strLot2Wfr = "", strMultiLot = "", strTTLWFR = "", strWork_WSNPos = "";

        public string ForcePrintName = "QUALCOMM STD Multi-2D";

        Thread Thread_Progress = null;

        public string Version = "";
        public int nSelectedWin = 0;
        public bool IsExit = false , IsRun = false;
        public static bool IsGRrun = false, IsAutoFocus = false;

        public static string strLogfilePath = "", strFilPath = "";
        public static string strAdminID = "", strAdminPW = "", strOperator = "", strID = "", strGrade = "", strMESID = "", strMESPW = "";
        public static bool bAdminLogin = false, bVisionConnect = false;
        public static int nStartup = 0, nProcess = 0, nGRprocess = 0, nSortTabNo = 0;

        /// <summary>
        /// Amkor 바코드 출력
        /// </summary>
        public static string strLotNo = "", strDeviceNo = "", strDieQty = "", strWfrQrt = "", strAmkorID = "", strCust = "", strCustName = "";

        public Form_Sort Frm_Sort = new Form_Sort();
        Form_Set Frm_Set = new Form_Set();
        Form_Keynece Frm_Scanner = new Form_Keynece();
        Form_Progress Frm_Process = new Form_Progress();
        Form_MutiBcrIn Frm_MultiBcrIn = new Form_MutiBcrIn();
        Form_MultiBcrIn2 Frm_MultiBcrIn2 = new Form_MultiBcrIn2();

        bool LotSPR = false;

        public static object mf = null;


        SpeechSynthesizer speech = new SpeechSynthesizer();


        private void button1_Click_1(object sender, EventArgs e)
        {
            frm_LoactionLabel lPrint = new frm_LoactionLabel();

            lPrint.ShowDialog();
        }

        private void button2_Click_1Async(object sender, EventArgs e)
        {
            string str = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://10.101.14.130:8180/eMES_Webservice/lot_info_list/get_request_list/change,waiting_for_change,K4,2193,334027");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/HY"));

                HttpResponseMessage response = client.GetAsync("").Result;
                if (response.IsSuccessStatusCode)
                {
                    var contents = response.Content.ReadAsStringAsync();
                    str = contents.Result;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            speech.SpeakAsync("56");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form_MesPWChange mespw = new Form_MesPWChange();
            mespw.ShowDialog();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void rb_keyence_Click(object sender, EventArgs e)
        {
            if (rb_keyence.Checked == true)
            {
                Properties.Settings.Default.CameraType = "KEYENCE";
                Properties.Settings.Default.Save();

                if (DialogResult.Yes == MessageBox.Show("카메라를 Keyence로 변경 하시겠습니까?/n(재시작)", "카메라 변경", MessageBoxButtons.YesNo))
                {
                    this.Close();
                }
            }
        }

        private void rb_cognex_Click(object sender, EventArgs e)
        {
           

            if (rb_cognex.Checked == true)
            {
                Properties.Settings.Default.CameraType = "COGNEX";
                Properties.Settings.Default.Save();

                if (DialogResult.Yes == MessageBox.Show("카메라를 Cognex로 변경 하시겠습니까?/n(재시작)", "카메라 변경", MessageBoxButtons.YesNo))
                {
                    this.Close();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void rb_keyence_CheckedChanged(object sender, EventArgs e)
        {
           
        }

       

        public BankHost_main()
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath);
            strFilPath = di.ToString();

            Process thisProc = Process.GetCurrentProcess();

            if (IsProcessOpen("Bank_Host") == false)
            {

            }
            else
            {
                if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
                {
                    MessageBox.Show("프로그램이 이미 실행 중 입니다. 종료 후 다시 실행 하십시오.");
                    System.Environment.Exit(1);
                    return;
                }
            }

            InitializeComponent();

            Fnc_Init();
            Host.Host_Delete_BcrReadinfoAll(strEqid);
        }

        public void ThreadStart()
        {
            try
            {
                if (Thread_Progress != null)
                {
                    Thread_Progress.Abort();
                    Thread_Progress = null;
                }

                Thread_Progress = new Thread(ThreadProc);
                Thread_Progress.Start();
            }
            catch (Exception ex)
            {
                Fnc_SaveLog(ex.ToString(), 0);
            }
        }
        public void ThreadProc()
        {
            while (IsExit == false)
            {
                try
                {
                    if (this != null)
                    {
                        if (nMaterial_type == 0)
                        {
                            if (nScanMode == 0)
                            {
                                Process_Vision();
                            }
                            else if (nScanMode == 1 || nScanMode == 3)
                            {
                                Process_GunRing();
                            }
                        }
                        else
                        {
                            if (nScanMode == 1)
                            {
                                Process_GunRing_Fosb();
                            }
                        }
                    }

                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {

                }
                
            }
        }

        public void Process_Vision()
        {
            try
            {
                if (IsAutoFocus)
                {
                    Frm_Scanner.Socket_MessageSend("FTUNE");

                    while (true)
                    {
                        if (Frm_Scanner.strReceivedata.Contains("SUCCEEDED"))
                            break;

                        Thread.Sleep(100);
                    }

                    IsAutoFocus = false;
                }

                if (nProcess == 1000 && !IsRun && !IsGRrun)
                {
                    //Barcode reading
                    string strTrigger = Host.Host_Get_Bcr_Read_Result(strEqid);

                    if ((strTrigger == "1" || nInputMode == 1) && nWorkMode != 0)
                    {
                        string strGetState = "";
                        while (true)
                        {
                            strGetState = Host.Host_Get_Print_State(strEqid);
                            if (strGetState == "1" || strGetState == "4")
                                break;

                            Thread.Sleep(200);
                        }

                        IsRun = true;

                        Read_Bcr = null;
                        if (Properties.Settings.Default.CameraType == "KEYENCE")
                        {
                            string strbank = string.Format("LON,{0}", strWork_Bank.Length == 1 ? $"0{strWork_Bank}" : strWork_Bank);
                            //string strbank = string.Format("LON");
                            Frm_Scanner.Socket_MessageSend(strbank);
                            Thread.Sleep(350);
                            /*
                            sw_TriggerTime.Start();

                            while (Frm_Scanner.strReceivedata != "")
                            {
                                Thread.Sleep(1);
                                Application.DoEvents();
                                if (sw_TriggerTime.ElapsedMilliseconds > 2000)

                                    break;
                            }

                            sw_TriggerTime.Stop();
                            sw_TriggerTime.Reset();
                            */
                            Frm_Scanner.Socket_MessageSend("LOFF");
                        }
                        else if(Properties.Settings.Default.CameraType == "COGNEX")
                        {
                            ReaderData = "";
                            ((MainForm)mf).Trigger();
                        }
                        Thread.Sleep(80);

                        try
                        {
                            DateTime recvTime = DateTime.Now;

                            while(ReaderData == "")
                            {
                                Application.DoEvents();

                                if ((DateTime.Now - recvTime).TotalSeconds > 1)
                                    break;
                            }

                            Read_Bcr = Frm_Sort.Fnc_Bcr_Parsing(ReaderData);
                            
                        }
                        catch
                        {
                            IsRun = false;
                            Read_Bcr = null;
                        }

                        string strMsg = "", strResult = "";

                        if (Read_Bcr != null)
                            strResult = Read_Bcr.result;

                        if (!IsRun)
                        {
                             Frm_Sort.Fnc_BcrInfo("오류가 발견 되었습니다. 설정을 다시 확인 하세요!");
                        }
                        else
                        {
                            strMsg = string.Format("[{0}],{1}", strResult, Frm_Scanner.strReceivedata);
                            Frm_Sort.Fnc_BcrInfo(strMsg);
                        }

                        if (Read_Bcr != null)
                        {
                            if (Read_Bcr.result == "OK")
                            {
                                Form_Sort.strValDevice = Read_Bcr.Device;
                                Form_Sort.strValLot = Read_Bcr.Lot;
                                Form_Sort.strWSN = Read_Bcr.WSN;
                                Form_Sort.nValDiettl = Int32.Parse(Read_Bcr.DieTTL);
                                Form_Sort.nValDieQty = Int32.Parse(Read_Bcr.DieQty);
                                Form_Sort.nValWfrttl = Int32.Parse(Read_Bcr.WfrTTL);
                                Form_Sort.bupdate = true;
                                Form_Sort.bunprinted_device = Read_Bcr.unprinted_device;

                                Form_Sort.nProcess = 1000; //Update Start
                                Form_Sort.bRun = true;
                                Form_Sort.nResult = 1000;

                                while (Form_Sort.bRun)
                                {
                                    Thread.Sleep(1);
                                }

                                nProcess = 2000;
                            }
                            else
                            {
                                if (Read_Bcr.result == "DUPLICATE")
                                {
                                    nProcess = 1000;
                                    IsRun = false;
                                    nNGcount = 0;
                                }
                                else
                                {
                                    Host.Host_Delete_BcrReadinfo(strEqid, Read_Bcr.Lot, 0);
                                    nProcess = 2001;
                                }
                            }
                        }
                        else //Read fail
                        {
                            nNGcount++;

                            if (nNGcount == 3)
                            {
                                //State 0: Not working, 1: Start, 2: NG, 3: Complete
                                //Result 0: OK, 1: NG Reading fail, 2: NG Lot Missmatch, 3: Fail                                    
                                Bcr_result[0] = RetBcrState.NG.ToString();
                                Bcr_result[1] = RetBcrResult.NO_CONNECT.ToString();
                                Bcr_result[2] = "";
                                Bcr_result[3] = "";
                                Bcr_result[4] = "";
                                Bcr_result[5] = "";

                                string str = Host.Host_Set_Bcr_Data(strEqid, Bcr_result);
                                nNGcount = 0;
                            }

                            Thread.Sleep(500);
                            IsRun = false;
                        }
                    }
                    else if (strTrigger == "4")
                    {
                        //Manual Scan
                    }
                }
                else if (nProcess == 2000) //OK
                {
                    if (Form_Sort.nResult == -1) //Lot NG
                    {
                        //Result 0: OK, 1: NG Reading fail, 2: NG Lot Missmatch, 3: Fail                                    
                        Bcr_result[0] = RetBcrState.NG.ToString(); //State 2: NG  3: OK
                        Bcr_result[1] = RetBcrResult.LOT_MISSMATCH.ToString(); //Result 1: NG1, 2: NG2, 3: FAIL
                        Bcr_result[2] = ""; //
                        Bcr_result[3] = Form_Sort.strValLot;
                        Bcr_result[4] = Form_Sort.strValDevice;
                        Bcr_result[5] = Form_Sort.strWSN;

                        IsRun = false;
                        nProcess = 1000;
                    }
                    else if (Form_Sort.nResult == 1) //Lot complete
                    {
                        Bcr_result[0] = RetBcrState.COMPLETE.ToString();
                        Bcr_result[1] = "COMPLETE";
                        Bcr_result[2] = Form_Sort.nValWfrQty.ToString();
                        Bcr_result[3] = Form_Sort.strValLot;
                        Bcr_result[4] = Form_Sort.strValDevice;

                        IsRun = false;
                        nProcess = 3000;
                    }
                    else if (Form_Sort.nResult == 2) //GR Start
                    {
                        Bcr_result[0] = RetBcrState.COMPLETE.ToString();

                        if (Read_Bcr.unprinted_device)
                            Bcr_result[1] = "PASS";
                        else
                            Bcr_result[1] = "GR";

                        Bcr_result[2] = Form_Sort.nValWfrQty.ToString();
                        Bcr_result[3] = Form_Sort.strValLot;
                        Bcr_result[4] = Form_Sort.strValDevice;
                        Bcr_result[5] = Form_Sort.strWSN;

                        IsRun = false;
                        nProcess = 3001;
                    }
                    else if (Form_Sort.nResult == -2) //
                    {
                        Bcr_result[0] = RetBcrState.NG.ToString();
                        Bcr_result[1] = RetBcrResult.READ_FAIL.ToString();
                        Bcr_result[2] = ""; //
                        Bcr_result[3] = Form_Sort.strValLot;
                        Bcr_result[4] = Form_Sort.strValDevice;

                        IsRun = false;
                        nProcess = 1000;
                    }
                    else
                    {
                        Bcr_result[0] = RetBcrState.COMPLETE.ToString();
                        Bcr_result[1] = "OK";
                        Bcr_result[2] = Form_Sort.nValWfrQty.ToString();
                        Bcr_result[3] = Form_Sort.strValLot;
                        Bcr_result[4] = Form_Sort.strValDevice;

                        IsRun = false;
                        nProcess = 1000;
                    }

                    string str = Host.Host_Set_Bcr_Data(strEqid, Bcr_result);

                    nNGcount = 0;

                }
                else if (nProcess == 2001)
                {
                    Bcr_result[0] = RetBcrState.NG.ToString();
                    Bcr_result[1] = RetBcrResult.LOT_MISSMATCH.ToString();
                    Bcr_result[2] = "";
                    Bcr_result[3] = Form_Sort.strValLot;
                    Bcr_result[4] = Form_Sort.strValDevice;

                    string str = Host.Host_Set_Bcr_Data(strEqid, Bcr_result);

                    speech.SpeakAsync("랏트 미스매치");

                    string strMsg = string.Format("LOT MISSMATCH! LOT 정보를 확인 하십시오");
                    Frm_Process.Form_Show(strMsg);
                    Frm_Process.Form_Display_Warning(strMsg);
                    Thread.Sleep(2000);
                    Frm_Process.Form_Hide();

                    IsRun = false;
                    nNGcount = 0;
                    nProcess = 1000;
                }
                else if (nProcess == 3000) //Complete Print
                {
                    //Barcode Print
                    string[] printinfo = { "", "" };
                    printinfo[0] = "2"; printinfo[1] = Form_Sort.strValLot;
                    Host.Host_Set_Print_Data(strEqid, printinfo);

                    AmkorBcrInfo Amkor = Frm_Sort.Fnc_GetAmkorBcrInfo(Form_Sort.strValReadfile, Form_Sort.strValLot, Form_Sort.strValDcc, Form_Sort.strValDevice);

                    if (strWork_Shot1Lot == "YES")
                        Form_Sort.nLabelcount = 0;

                    if (ForcePrintName.Contains(BankHost_main.strCustName))
                        Read_Bcr.unprinted_device = false;

                    if (!Read_Bcr.unprinted_device)
                    {

                        
                        Frm_Sort.Fnc_Print_Start(Amkor, nWork_BcrType, true, Form_Sort.nLabelcount, Form_Sort.nLabelttl);
                        

                        if (nInputMode == 1)
                        {
                            speech.SpeakAsync("라벨 출력");
                        }
                    }
                    else
                    {
                        if (nInputMode == 1)
                        {
                            speech.SpeakAsync("미출력 디바이스");
                        }
                    }

                    printinfo[0] = "3"; printinfo[1] = Form_Sort.strValLot;
                    Host.Host_Set_Print_Data(strEqid, printinfo);

                    string strState = "";

                    while (true)
                    {
                        strState = Host.Host_Get_Print_State(strEqid);
                        if (strState == "1")
                            break;

                        if (nInputMode == 1)
                        {
                            Host.Host_Set_Print_Detach(strEqid, Form_Sort.strValLot);
                        }

                        if (strState == "4")//Retry
                        {
                            if (!Read_Bcr.unprinted_device)
                                Frm_Sort.Fnc_Print_Start(Amkor, nWork_BcrType, true, Form_Sort.nLabelcount, Form_Sort.nLabelttl);

                            printinfo[0] = "3"; printinfo[1] = Form_Sort.strValLot;
                            Host.Host_Set_Print_Data(strEqid, printinfo);
                        }

                        Thread.Sleep(200);
                    }

                    printinfo[0] = "1"; printinfo[1] = ""; //Ready
                    Host.Host_Set_Print_Data(strEqid, printinfo);

                    nProcess = 1000;

                    IsRun = false;
                }
                else if (nProcess == 3001) //GR Print
                {
                    //Barcode Print
                    //HY TEST
                    string[] printinfo = { "", "" };
                    printinfo[0] = "2"; printinfo[1] = Form_Sort.strValLot;
                    Host.Host_Set_Print_Data(strEqid, printinfo);

                    AmkorBcrInfo Amkor = Frm_Sort.Fnc_GetAmkorBcrInfo(Form_Sort.strValReadfile, Form_Sort.strValLot, Form_Sort.strValDcc, Form_Sort.strValDevice, Form_Sort.strWSN);

                    if (strWork_Shot1Lot == "YES")
                        Form_Sort.nLabelcount = 0;

                    if (ForcePrintName.Contains(BankHost_main.strCustName))
                        Read_Bcr.unprinted_device = false;

                    if (!Read_Bcr.unprinted_device)
                    {
                        Frm_Sort.Fnc_Print_Start(Amkor, nWork_BcrType, true, Form_Sort.nLabelcount, Form_Sort.nLabelttl);

                        if (nInputMode == 1)
                        {
                            speech.SpeakAsync("라벨 출력");
                        }
                    }
                    else
                    {
                        if (nInputMode == 1)
                        {
                            speech.SpeakAsync("미출력 디바이스");
                        }
                    }

                    printinfo[0] = "3"; printinfo[1] = Form_Sort.strValLot;
                    Host.Host_Set_Print_Data(strEqid, printinfo);

                    string strState = "";

                    Form_Sort.strGR_Device = Form_Sort.strValDevice;
                    Form_Sort.strGR_Lot = Form_Sort.strValLot;

                    while (true)
                    {
                        strState = Host.Host_Get_Print_State(strEqid);
                        if (strState == "1")
                            break;

                        if (nInputMode == 1)
                        {
                            Host.Host_Set_Print_Detach(strEqid, Form_Sort.strValLot);
                        }

                        if (strState == "4")//Retry
                        {
                            if (!Read_Bcr.unprinted_device)
                                Frm_Sort.Fnc_Print_Start(Amkor, nWork_BcrType, true, Form_Sort.nLabelcount, Form_Sort.nLabelttl);

                            printinfo[0] = "3"; printinfo[1] = Form_Sort.strValLot;
                            Host.Host_Set_Print_Data(strEqid, printinfo);
                        }

                        Thread.Sleep(200);
                    }


                    Form_Sort.nLabelcount = 0;
                    Form_Sort.nLabelttl = 0;

                    printinfo[0] = "1"; printinfo[1] = ""; //Ready
                    Host.Host_Set_Print_Data(strEqid, printinfo);

                    nProcess = 3002;

                    IsRun = false;
                }
                else if (nProcess == 3002) //Auto GR
                {
                    nProcess = 1000;
                    IsRun = false;
                }
                else if (nProcess == 4001)
                {
                    Read_Bcr = null;

                    string strbank = string.Format("LON,16");
                    //string strbank = string.Format("LON");
                    Frm_Scanner.Socket_MessageSend(strbank);
                    Thread.Sleep(350);

                    Frm_Scanner.Socket_MessageSend("LOFF");
                    Thread.Sleep(80);

                    try
                    {
                        Read_Bcr = Frm_Sort.Fnc_Bcr_Parsing(Frm_Scanner.strReceivedata);
                    }
                    catch
                    {
                        IsRun = false;
                        Read_Bcr = null;
                    }

                    if(Read_Bcr == null)
                    {
                        //if(read_err_cnt > 10)
                        //{
                        //    if (BankHost_main.IsAutoFocus == false)
                        //        BankHost_main.IsAutoFocus = true;

                        //    read_err_cnt = 0;
                        //}
                        //else
                        //{
                        //    read_err_cnt++;
                        //}
                    }
                }
            }
            catch
            {
                nProcess = 1000;
            }
        }

        int read_err_cnt = 0;

        private void label3_Click(object sender, EventArgs e)
        {            
            if(label3.Text == "K4")
            {
                DialogResult res = MessageBox.Show("위치를 K5로 변경 하시겠습니까?","위치 변경", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    label3.Text = "K5";
                    label3.ForeColor = Color.Green;

                    Properties.Settings.Default.LOCATION = "K5";
                    Properties.Settings.Default.Save();

                    BankHost_main.nScanMode = 1;                    
                }
            }
            else if(label3.Text =="K5")
            {
                DialogResult res = MessageBox.Show("위치를 K3로 변경 하시겠습니까?", "위치 변경", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    label3.Text = "K3";
                    label3.ForeColor = Color.Blue;

                    Properties.Settings.Default.LOCATION = "K3";
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                DialogResult res = MessageBox.Show("위치를 K4로 변경 하시겠습니까?", "위치 변경", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    label3.Text = "K4";
                    label3.ForeColor = Color.Red;

                    Properties.Settings.Default.LOCATION = "K4";
                    Properties.Settings.Default.Save();
                }
            }

            Frm_Sort.init_mode_combobox();

        }

        private void BankHost_main_Load(object sender, EventArgs e)
        {
            label3.Text = Properties.Settings.Default.LOCATION;
            Frm_MultiBcrIn2.SetLotSPREvent += Frm_MultiBcrIn2_SetLotSPREvent;


            if (label3.Text == "K4")
            {
                label3.ForeColor = Color.Red;             
            }
            else if (label3.Text == "K5")
            {
                label3.ForeColor = Color.Green;
            }
            else
            {
                label3.ForeColor = Color.Blue;
            }




            Frm_Sort.init_mode_combobox();
        }

        private void Frm_MultiBcrIn2_SetLotSPREvent(bool val)
        {
            LotSPR = val;
            Frm_Sort.SetLotSPR(val);
        }

        public void ProcessGun_Error(string strMsg)
        {
            Frm_Sort.Fnc_BcrInfo("");
            bGunRingMode_Run = false;

            Frm_Process.Form_Show(strMsg);
            Frm_Process.Form_Display_Warning(strMsg);
            Thread.Sleep(2000);
            Frm_Process.Form_Hide();
        }

        public void ProcessGun_LabelPrint()
        {
            //Barcode Print          
            AmkorBcrInfo Amkor = Frm_Sort.Fnc_GetAmkorBcrInfo(Form_Sort.strValReadfile, Form_Sort.strValLot, Form_Sort.strValDcc, Form_Sort.strValDevice);
            
            if (strWork_Shot1Lot == "YES")
                Form_Sort.nLabelcount = 0;

            if (ForcePrintName.Contains(BankHost_main.strCustName))
                Read_Bcr.unprinted_device = false;

            if (Read_Bcr == null || !Read_Bcr.unprinted_device)
            {
                //Form_Sort.nLabelcount = 1;
                //Form_Sort.nLabelttl = Form_Sort.nValWfrttl;

               
                Frm_Sort.Fnc_Print_Start(Amkor, nWork_BcrType, true, Form_Sort.nLabelcount, Form_Sort.nLabelttl);

                if(Frm_Sort.SecondPrinterMode == true)
                {

                }

                if (nInputMode == 1)
                {
                    speech.SpeakAsync("라벨 출력");
                }
            }
            else
            {
                if (nInputMode == 1)
                {
                    speech.SpeakAsync("미출력 디바이스");
                }
            }

            Frm_Sort.Fnc_BcrInfo("");
            bGunRingMode_Run = false;
        }


        public void Process_GunRing()
        {
            try
            {    
                if (bGunRingMode_Run)
                {
                    string strMsg = "", strResult = "";

                    try
                    {
                        if (BankHost_main.strCustName == "QUALCOM_SPLIT")
                        {
                            if (strScanData == "")
                                return;

                            if (strScanData.Split(',')[1].Substring(0, 2) == "1T")
                            {
                                strScanData = strScanData.Replace(strScanData.Split(',')[1], strScanData.Split(',')[1].Substring(2, strScanData.Split(',')[1].Length - 2));

                                List<StorageData> Splitdata = Frm_Sort.GetSplitData(strScanData);

                                if (Splitdata.Count != 0)
                                {
                                    //strResult = Read_Bcr.result;
                                    if(Splitdata.Count > Properties.Settings.Default.SplitWLabel)
                                    {
                                        for (int i = 0; i < Frm_Sort.GetQualcomSplitCopys(); i++)
                                        {
                                            speech.SpeakAsync("스플릿 라벨 출력");
                                            PrintSplit(Splitdata);
                                        }
                                        
                                    }
                                    else
                                    {//Form_Sort.strValReadfile, Form_Sort.strValLot, Form_Sort.strValDcc, Form_Sort.strValDevice

                                        foreach (StorageData sd in Splitdata)
                                        {
                                            Form_Sort.strValReadfile = sd.ReadFile;
                                            Form_Sort.strValLot = sd.Lot;
                                            Form_Sort.strValDcc = sd.Lot_Dcc;
                                            Form_Sort.strValDevice = sd.Device;

                                            for (int i = 0; i < Frm_Sort.GetQualcomSplitCopys(); i++)
                                                ProcessGun_LabelPrint();
                                        }
                                    }
                                    strScanData = "";
                                    bGunRingMode_Run = false;

                                }
                                else
                                {
                                    strMsg = string.Format("오류가 발견 되었습니다. 설정 또는 바코드 형식을 확인 하세요!");
                                    ProcessGun_Error(strMsg);
                                    return;
                                }
                            }
                            else if(LotSPR == true)
                            {
                                List<StorageData> Splitdata = Frm_Sort.GetSplitData(strScanData);

                                if (Splitdata.Count != 0)
                                {
                                    //strResult = Read_Bcr.result;

                                    PrintSplit(Splitdata);
                                    strScanData = "";
                                    bGunRingMode_Run = false;
                                }
                                else
                                {
                                    strMsg = string.Format("오류가 발견 되었습니다. 설정 또는 바코드 형식을 확인 하세요!");
                                    ProcessGun_Error(strMsg);
                                    return;
                                }
                            }
                            else
                            {
                                strMsg = string.Format("오류가 발견 되었습니다. 설정 또는 바코드 형식을 확인 하세요!");
                                ProcessGun_Error(strMsg);
                                return;
                            }
                        }
                        else
                        {

                            Read_Bcr = Frm_Sort.Fnc_Bcr_Parsing(strScanData);

                            if (Read_Bcr != null)
                                strResult = Read_Bcr.result;
                            else
                            {
                                strMsg = string.Format("오류가 발견 되었습니다. 설정 또는 바코드 형식을 확인 하세요!");
                                ProcessGun_Error(strMsg);
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Read_Bcr = null;
                        
                        strMsg = string.Format("오류가 발견 되었습니다. 설정을 다시 확인 하세요!");
                        ProcessGun_Error(strMsg);
                        return;
                    }

                    if (BankHost_main.strCustName == "QUALCOM_SPLIT")
                    {
                        //if(Frm_Sort.GetQualCommSplitGreenLabel() == true)
                        {
                            strMsg = string.Format("[{0}],{1}", strResult, strScanData);
                            Frm_Sort.Fnc_BcrInfo(strMsg);

                            if (Read_Bcr != null)
                            {
                                if (Read_Bcr.result == "OK")
                                {
                                    Form_Sort.strValDevice = Read_Bcr.Device;
                                    Form_Sort.strValLot = Read_Bcr.Lot;
                                    Form_Sort.nValDiettl = Read_Bcr.DieTTL == "" ? 0 : Int32.Parse(Read_Bcr.DieTTL);
                                    Form_Sort.nValDieQty = Read_Bcr.DieQty == "" ? 0 : Int32.Parse(Read_Bcr.DieQty);
                                    Form_Sort.nValWfrttl = Read_Bcr.WfrTTL == "" ? 0 : Int32.Parse(Read_Bcr.WfrTTL);
                                    Form_Sort.bupdate = true;
                                    Form_Sort.bunprinted_device = Read_Bcr.unprinted_device;

                                    Form_Sort.nProcess = 1000; //Update Start
                                    Form_Sort.bRun = true;
                                    Form_Sort.nResult = 1000;

                                    ////데이터 처리 대기
                                    while (Form_Sort.bRun)
                                    {
                                        Thread.Sleep(1);
                                    }

                                    if (Form_Sort.nResult == -1) //Lot NG
                                    {
                                        bGunRingMode_Run = false;
                                        return;
                                    }
                                    else if (Form_Sort.nResult == 1 || Form_Sort.nResult == 2) //Lot complete
                                    {
                                        ProcessGun_LabelPrint();
                                        return;
                                    }
                                    else if (Form_Sort.nResult == -2) //
                                    {
                                        strMsg = string.Format("READ FAIL");
                                        ProcessGun_Error(strMsg);
                                        return;
                                    }
                                    else
                                    {
                                        Frm_Sort.Fnc_BcrInfo("");
                                        bGunRingMode_Run = false;
                                    }
                                }
                                else
                                {
                                    if (Read_Bcr.result == "DUPLICATE")
                                    {
                                        Read_Bcr = null;

                                        strMsg = string.Format("중복 자재 입니다. 다른 자재를 스캔 하십시오.");
                                        ProcessGun_Error(strMsg);

                                        return;
                                    }
                                    else
                                    {
                                        Host.Host_Delete_BcrReadinfo(strEqid, Read_Bcr.Lot, 0);
                                        strMsg = string.Format("LOT MISSMATCH! LOT 정보를 확인 하십시오");
                                        ProcessGun_Error(strMsg);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        strMsg = string.Format("[{0}],{1}", strResult, strScanData);
                        Frm_Sort.Fnc_BcrInfo(strMsg);

                        if (Read_Bcr != null)
                        {
                            if (Read_Bcr.result == "OK")
                            {
                                Form_Sort.strValDevice = Read_Bcr.Device;
                                Form_Sort.strValLot = Read_Bcr.Lot;
                                Form_Sort.nValDiettl = Read_Bcr.DieTTL == "" ? 0 : Int32.Parse(Read_Bcr.DieTTL);
                                Form_Sort.nValDieQty = Read_Bcr.DieQty == "" ? 0 : Int32.Parse(Read_Bcr.DieQty);
                                Form_Sort.nValWfrttl = Read_Bcr.WfrTTL == "" ? 0 : Int32.Parse(Read_Bcr.WfrTTL);
                                Form_Sort.bupdate = true;
                                Form_Sort.bunprinted_device = Read_Bcr.unprinted_device;

                                Form_Sort.nProcess = 1000; //Update Start
                                Form_Sort.bRun = true;
                                Form_Sort.nResult = 1000;

                                ////데이터 처리 대기
                                while (Form_Sort.bRun)
                                {
                                    Thread.Sleep(1);
                                }

                                if (Form_Sort.nResult == -1) //Lot NG
                                {
                                    bGunRingMode_Run = false;
                                    return;
                                }
                                else if (Form_Sort.nResult == 1 || Form_Sort.nResult == 2) //Lot complete
                                {
                                    ProcessGun_LabelPrint();
                                    return;
                                }
                                else if (Form_Sort.nResult == -2) //
                                {
                                    strMsg = string.Format("READ FAIL");
                                    ProcessGun_Error(strMsg);
                                    return;
                                }
                                else
                                {
                                    Frm_Sort.Fnc_BcrInfo("");
                                    bGunRingMode_Run = false;
                                }
                            }
                            else
                            {
                                if (Read_Bcr.result == "DUPLICATE")
                                {
                                    Read_Bcr = null;

                                    strMsg = string.Format("중복 자재 입니다. 다른 자재를 스캔 하십시오.");
                                    ProcessGun_Error(strMsg);

                                    return;
                                }
                                else
                                {
                                    Host.Host_Delete_BcrReadinfo(strEqid, Read_Bcr.Lot, 0);
                                    strMsg = string.Format("LOT MISSMATCH! LOT 정보를 확인 하십시오");
                                    ProcessGun_Error(strMsg);
                                    return;
                                }
                            }
                        }
                    }                  
                }
                
            }
            catch
            {
                string strMsg = string.Format("오류가 발견 되었습니다.");
                ProcessGun_Error(strMsg);
                bGunRingMode_Run = false;
            }
        }

        public void PrintSplit(List<StorageData> SplitData)
        {
            string PrintCode = "^XA" + "^FO10,10 ^GB850,1500,3 ^FS" + "^FO10,60 ^GB850,3,3 ^FS" + "^FO250,10 ^GB3,1500,3 ^FS" + "^FO600,10 ^GB3,1500,3 ^FS" + "^FO750,10 ^GB3,1500,3 ^FS" +
                            "^FO10,170 ^GB1000,3,3 ^FS" + "^FO10,280 ^GB1000,3,3 ^FS" + "^FO10,390 ^GB1000,3,3 ^FS" + "^FO10,500 ^GB1000,3,3 ^FS" + "^FO10,610 ^GB1000,3,3 ^FS" +
                            "^FO10,720 ^GB1000,3,3 ^FS" + "^FO10,830 ^GB1000,3,3 ^FS" + "^FO10,940 ^GB1000,3,3 ^FS" + "^FO10,1050 ^GB1000,3,3 ^FS" + "^FO10,1160 ^GB1000,3,3 ^FS" +
                            "^FO10,1270 ^GB1000,3,3 ^FS" + "^FO10,1380 ^GB1000,3,3 ^FS" + "^FO10,1507 ^GB1000,3,3 ^FS" +

                            "^FO80,25" + "^FDDevice" + "^CF0,30" + "^FS" +
                            "^FO350,25" + "^FDLot / DCC" + "^CF0,30" + "^FS" +

                            "^FO650,25" + "^FDDie Q'ty" + "^CF0,20" + "^FS" +

                            "^FO760,25" + "^FDWafer Q'ty" + "^CF0,20" + "^FS" + 
                            $"^FO880,25 ^FDTTL:{SplitData.Count} ^CF0,30 ^FS";

            SplitData = SplitData.OrderBy(p => p.Lot).ToList();

            string bcrdata = "";

            for(int i = 0; i < SplitData.Count; i++)
            {
                PrintCode += $"^FO20,{(i== 0 ?  105 : 105 + i*110)} ^FD{SplitData[i].Device} ^CF0,25 ^FS";
                PrintCode += $"^FO260,{(i== 0 ? 85 : 85 + i * 110)} ^FD{SplitData[i].Lot.PadRight(20)} ^CF0,25 ^FS";
                PrintCode += $"^FO260,{(i == 0 ? 135 : 135 + i * 110)} ^FD{SplitData[i].Lot_Dcc.PadRight(5)} ^CF0,25 ^FS";

                PrintCode += $"^FO645,{(i == 0 ? 135 : 135 + i * 110)} ^FD{SplitData[i].Rcv_Qty} ^CF0,25 ^FS";
                PrintCode += $"^FO790,{(i == 0 ? 135 : 135 + i * 110)} ^FD{SplitData[i].Default_WQty} ^CF0,25 ^FS";

                bcrdata = $"{SplitData[i].Lot}:{ SplitData[i].Lot_Dcc}:{ SplitData[i].Device}:{ Convert.ToString(SplitData[i].Rcv_Qty).PadLeft(10, '0')}:{ Convert.ToString(SplitData[i].Default_WQty).PadLeft(5, '0')}:{ SplitData[i].Amkorid}:{Convert.ToString(BankHost_main.strWork_Cust).PadLeft(5, '0')}";

                PrintCode += $"^FO880,{(i == 0 ? 70 : 70 + i * 110)} ^BX,3,200 ^FD{bcrdata} ^FS";
            }

            PrintCode += $"^FO30,1530 ^FDLOT TYPE:{SplitData[0].Lot_type}\tRCV-DATE:{SplitData[0].Rcvddate}\tBill#:{SplitData[0].Bill} #({strWork_Cust}) ^CF0,40,30 ^FS";

            PrintCode += "^XZ";

            byte[] zpl = Encoding.UTF8.GetBytes(PrintCode);

            using (System.Net.Sockets.TcpClient socket = new System.Net.Sockets.TcpClient())
            {

                //IPAddress ip = IPAddress.Parse(Properties.Settings.Default.SecondPrinterIP);

                socket.Connect("10.131.34.21", 9100);
                StreamWriter writer = new StreamWriter(socket.GetStream());

                writer.Write(PrintCode);
                writer.Flush();

                writer.Close();
            }
        }

        public void Process_GunRing_Fosb()
        {
            try
            {
                if (bGunRingMode_Run)
                {
                    string strMsg = "", strResult = "";

                    try
                    {
                        Read_Bcr = Frm_Sort.Fnc_Bcr_Parsing_Fosb(strScanData);
                        if (Read_Bcr != null)
                            strResult = Read_Bcr.result;
                        else
                        {
                            strMsg = string.Format("오류가 발견 되었습니다. 설정 또는 바코드 형식을 확인 하세요!");
                            ProcessGun_Error(strMsg);
                            return;
                        }
                    }
                    catch
                    {
                        Read_Bcr = null;

                        strMsg = string.Format("오류가 발견 되었습니다. 설정을 다시 확인 하세요!");
                        ProcessGun_Error(strMsg);
                        return;
                    }

                    strMsg = string.Format("[{0}],{1}", strResult, strScanData);
                    Frm_Sort.Fnc_BcrInfo(strMsg);

                    if (Read_Bcr != null)
                    {
                        if (Read_Bcr.result == "OK")
                        {
                            Form_Sort.strValDevice = Read_Bcr.Device;
                            Form_Sort.strValLot = Read_Bcr.Lot;
                            Form_Sort.nValDiettl = Int32.Parse(Read_Bcr.DieTTL);
                            Form_Sort.nValDieQty = Int32.Parse(Read_Bcr.DieQty);
                            Form_Sort.nValWfrttl = Int32.Parse(Read_Bcr.WfrTTL);
                            Form_Sort.bupdate = true;
                            Form_Sort.bunprinted_device = Read_Bcr.unprinted_device;

                            Form_Sort.nProcess = 1000; //Update Start
                            Form_Sort.bRun = true;
                            Form_Sort.nResult = 1000;

                            ////데이터 처리 대기
                            while (Form_Sort.bRun)
                            {
                                Thread.Sleep(1);
                            }

                            if (Form_Sort.nResult == -1) //Lot NG
                            {
                                bGunRingMode_Run = false;
                                return;
                            }
                            else if (Form_Sort.nResult == 1 || Form_Sort.nResult == 2) //Lot complete
                            {
                                ProcessGun_LabelPrint();
                                return;
                            }
                            else if (Form_Sort.nResult == -2) //
                            {
                                strMsg = string.Format("READ FAIL");
                                ProcessGun_Error(strMsg);
                                return;
                            }
                            else
                            {
                                Frm_Sort.Fnc_BcrInfo("");
                                bGunRingMode_Run = false;
                            }
                        }
                        else
                        {
                            if (Read_Bcr.result == "DUPLICATE")
                            {
                                Read_Bcr = null;

                                strMsg = string.Format("중복 자재 입니다. 다른 자재를 스캔 하십시오.");
                                ProcessGun_Error(strMsg);

                                return;
                            }
                            else
                            {
                                Host.Host_Delete_BcrReadinfo(strEqid, Read_Bcr.Lot, 0);
                                strMsg = string.Format("LOT MISSMATCH! LOT 정보를 확인 하십시오");
                                ProcessGun_Error(strMsg);
                                return;
                            }
                        }
                    }
                }

            }
            catch
            {
                string strMsg = string.Format("오류가 발견 되었습니다.");
                ProcessGun_Error(strMsg);
                bGunRingMode_Run = false;
            }
        }

        public bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }

            return false;
        }

        public void Fnc_Init()
        {
            if (System.IO.File.Exists(Application.StartupPath + "\\FileUpdateinfo.ini") == true)
                Version =  System.IO.File.ReadAllText(Application.StartupPath + "\\FileUpdateinfo.ini");
            

            //Version = strtemp;// Application.ProductVersion;
            Text = "S/W Version:" + Version;

            Frm_Sort.MdiParent = this;
            Frm_Sort.Location = new Point(0, 0);
            Frm_Sort.Size = new Size(1042, 670);

            Frm_Set.MdiParent = this;
            Frm_Set.Location = new Point(0, 0);
            Frm_Set.Size = new Size(1042, 670);

            Frm_Scanner.MdiParent = this;
            Frm_Scanner.Location = new Point(0, 0);
            Frm_Scanner.Size = new Size(666, 480);

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath + @"\Log");
            if (!di.Exists) { di.Create(); }

            System.IO.DirectoryInfo di2 = new System.IO.DirectoryInfo(@"C:\log");
            if (!di2.Exists) { di2.Create(); }

            strLogfilePath = di.ToString();

            string strConnect = Host.Connect();

            if(strConnect == "OK")
            {
                bHost_connect = true;
                //label_server.Text = "서버 연결 OK";
                //label_server.BackColor = Color.Green;
            }
            else
            {
                //label_server.Text = "서버 연결 실패";
                //label_server.BackColor = Color.Red;
            }

            strAdminID = "admin";
            strAdminPW = "admin";

            string strScanMode = ConfigurationManager.AppSettings["Scan_mode"];
            if(strScanMode != "")
                nScanMode = Int32.Parse(strScanMode);

            string strMtltype = ConfigurationManager.AppSettings["Material_type"];
            if(strMtltype != "")
                nMaterial_type = Int32.Parse(strMtltype);

            string strAmkorBcrType = ConfigurationManager.AppSettings["AmkorBcr_type"];
            if (strAmkorBcrType != "")
                nAmkorBcrType = Int32.Parse(strAmkorBcrType);

            string strStart = ConfigurationManager.AppSettings["Startup"];
            nStartup = Int32.Parse(strStart);

            string strMaxPack = ConfigurationManager.AppSettings["MAX_PACK"];
            nMaxpack = Int32.Parse(strMaxPack);

            string strMode = ConfigurationManager.AppSettings["AUTO"];
            nInputMode = Int32.Parse(strMode);

            strEqid = ConfigurationManager.AppSettings["EQID"];

            if (nInputMode == 0)
                label_title.Text = "Host";
            else
                label_title.Text = "Host - Desktop";

            if (Properties.Settings.Default.CameraType == "KEYENCE")
            {
                Frm_Scanner.Fnc_Init(); ///Kyence 연결
                //mf.Dispose();
            }
            else if(Properties.Settings.Default.CameraType == "COGNEX")
            {
                mf = new MainForm();

                ((MainForm)mf).Show();
                ((MainForm)mf).Hide();

                //Frm_Scanner.Dispose();
            }

            if (nStartup == 0)
            {
                Fnc_Show_SortViewer();
            }

            Read_Bcr = null;

            ThreadStart(); //thread 시작

            timer1.Start();

            Fnc_SaveLog("프로그램 시작.", 0);
        }
        ///SQL 
        public static DataTable SQL_GetUserDB(string strID)
        {
            DataTable dt = Host.Host_GetUserDB(strID);

            return dt;
        }
        public static DataTable SQL_GetAllUser()
        {
            DataTable dt = Host.Host_GetAllUser();

            return dt;
        }

        public static void SQL_SetUserDB(string strID, string strName, string strGrade)
        {
            strID = strID.Trim();
            strName = strName.Trim();
            strGrade = strGrade.Trim();

            //Host.Host_DelUserDB(strID);
            Host.Host_SetUserDB(strID, strName, strGrade);
        }

        public static int SQL_DelUserDB(string strID)
        {
            int n = Host.Host_DelUserDB(strID);

            return n;
        }

        public void Fnc_Autofocus()
        {
            Frm_Scanner.Socket_MessageSend("FTUNF");
        }
        private void BankHost_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (nWorkMode != 0)
            {
                e.Cancel = true;
                return;
            }

            DialogResult dialogResult1 = MessageBox.Show("프로그램을 종료 하시겠습니까?", "Exit", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.Yes)
            {
                IsExit = true;

                Frm_Scanner.Socket_Close(); ///Kyence disconnect

                timer1.Stop();
                Frm_Sort.Fnc_PrintExit();

                Thread.Sleep(500);

                Frm_Process.Dispose();
                Frm_Sort.Dispose();
                Frm_Set.Dispose();
                Frm_Scanner.Dispose();

                GC.Collect(); 
                Fnc_SaveLog("프로그램 종료.", 0);

                System.Environment.Exit(1);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void button_Sort_Click(object sender, EventArgs e)
        {
            if (nWorkMode != 0)
                return;

            Fnc_Show_SortViewer();
        }

        private void button_Gr_Click(object sender, EventArgs e)
        {
            if (nWorkMode != 0)
                return;

            Fnc_Show_ScannerViewer();
        }

        public void Fnc_Show_SortViewer()
        {
            nSelectedWin = 0;

            Frm_Sort.Fnc_PrintHide();

            Frm_Sort.Show();
            Frm_Sort.Fnc_Init();

            button_Sort.ForeColor = System.Drawing.Color.OrangeRed;
            button_Print.ForeColor = System.Drawing.Color.LightGray;
            button_Bcr.ForeColor = System.Drawing.Color.LightGray;

            //Application.DoEvents();

            Fnc_SaveLog("Sort function 창 이동.", 0);
        }

        public void Fnc_Show_PrintViewer()
        {
            button_Print.ForeColor = System.Drawing.Color.OrangeRed;
            button_Sort.ForeColor = System.Drawing.Color.LightGray;
            button_Bcr.ForeColor = System.Drawing.Color.LightGray;
            Fnc_SaveLog("Print 창 이동.", 0);

            Frm_Sort.Fnc_PrintShow();
        }
        public void Fnc_Show_ScannerViewer()
        {
            button_Print.ForeColor = System.Drawing.Color.LightGray;
            button_Sort.ForeColor = System.Drawing.Color.LightGray;
            button_Bcr.ForeColor = System.Drawing.Color.OrangeRed;
            Fnc_SaveLog("Scanner 창 이동.", 0);

            Frm_Sort.Fnc_PrintHide();

            Frm_Set.Hide();
            Frm_Sort.Hide();
            Frm_Scanner.Show();
        }


        public void Fnc_Show_AutoGrViewer()
        {
            nSelectedWin = 0;

            Frm_Sort.Fnc_PrintHide();
            Frm_Sort.Hide();

            button_Print.ForeColor = System.Drawing.Color.LightGray;
            button_Sort.ForeColor = System.Drawing.Color.LightGray;
            button_Bcr.ForeColor = System.Drawing.Color.OrangeRed;

            //Application.DoEvents();

            Fnc_SaveLog("Auto GR function 창 이동.", 0);
        }

        public void Fnc_Show_SettingViewer()
        {
            nSelectedWin = -1;

            Frm_Set.Fnc_Init();
            Frm_Set.Fnc_UserAllView();
            
            Frm_Set.Show();
            Frm_Sort.Fnc_PrintHide();
            Frm_Sort.Hide();

            button_Print.ForeColor = System.Drawing.Color.LightGray;
            button_Sort.ForeColor = System.Drawing.Color.LightGray;
            button_Bcr.ForeColor = System.Drawing.Color.LightGray;

            //Application.DoEvents();

            Fnc_SaveLog("설정 창 이동.", 0);

            bAdminLogin = false;
        }

        public void Fnc_Show_MultiBcrin()
        {
            if (nScanMode == 3)
            {
                Frm_MultiBcrIn.strWorkFile = Frm_Sort.strWorkFileName;
                Frm_MultiBcrIn.strExcutionPath = Frm_Sort.strExcutionPath;
                Frm_MultiBcrIn.Fnc_Show();
            }
            else if(nScanMode == 1)
            {
                if(Frm_MultiBcrIn2.Visible == false)
                   Frm_MultiBcrIn2.Fnc_Show();
            }
        }

        public void Fnc_Hide_MultiBcrin()
        {
            if (nScanMode == 3)
            {
                Frm_MultiBcrIn.Fnc_Hide();
            }
            else if (nScanMode == 1)
            {
                Frm_MultiBcrIn2.Fnc_Hide();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string strToday = string.Format("{0}/{1:00}/{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string strHead = string.Format("{0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            label_day.Text = strToday;
            label_time.Text = strHead;

            if(nScanMode == 0)
            {
                gb_Camera.Enabled = true;

                if(Properties.Settings.Default.CameraType == "KEYENCE")
                {
                    rb_keyence.Checked = true;
                }
                else if(Properties.Settings.Default.CameraType == "COGNEX")
                {
                    rb_cognex.Checked = true;
                }


                if (bVisionConnect)
                {
                    if (!label_camera.Text.Contains("OK"))
                    {
                        label_camera.Text = "카메라 연결 OK";
                        label_camera.BackColor = Color.Green;
                    }
                }
                else
                {
                    if (!label_camera.Text.Contains("실패"))
                    {
                        label_camera.Text = "카메라 연결 실패";
                        label_camera.BackColor = Color.Red;
                    }
               }
            }
            else if(nScanMode == 1)
            {
                label_camera.Text = "GUN && RING Scanner";
                label_camera.BackColor = Color.Blue;

                gb_Camera.Enabled = false;
            }
            else if(nScanMode == 2)
            {
                label_camera.Text = "1Lot 1Wafer 개별 입력";
                label_camera.BackColor = Color.Blue;
            }
            else if (nScanMode == 3)
            {
                label_camera.Text = "1Lot BCR 동시 입력";
                label_camera.BackColor = Color.Blue;
            }

            if(nMaterial_type == 0)
            {
                label_type.Text = "REEL";
                label_type.BackColor = Color.Green;
            }
            else
            {
                label_type.Text = "FOSB";
                label_type.BackColor = Color.Blue;
            }

            if (bHost_connect)
            {
                if (!label_server.Text.Contains("OK"))
                {
                    label_server.Text = "서버 연결 OK";
                    label_server.BackColor = Color.Green;
                }

                if (nColorindex == 0)
                {
                    label_state.BackColor = System.Drawing.Color.Green;
                    nColorindex = 1;
                }
                else
                {
                    label_state.BackColor = System.Drawing.Color.Blue;
                    nColorindex = 0;
                }
            }
            else
            {
                if (!label_server.Text.Contains("실패"))
                {
                    label_server.Text = "서버 연결 실패";
                    label_server.BackColor = Color.Red;
                }

                label_state.BackColor = System.Drawing.Color.Red;
            }

            if (nSortTabNo == 2 && !bGunRingMode_Run)
            {
                Fnc_Show_MultiBcrin();
            }
            else
            {
                Fnc_Hide_MultiBcrin();
            }
        }

        private void button_Print_Click(object sender, EventArgs e)
        {
            if (nWorkMode != 0)
                return;

            Fnc_Show_PrintViewer();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strLog">log string</param>
        /// <param name="nType">0: system, 1:work, 2:setting, 3:error</param>
        static public void Fnc_SaveLog(string strLog, int nType) ///설비별 개별 로그 저장
        {
            string strPath = "";
            if (nType == 0)
                strPath = strLogfilePath + "\\system_";
            else if (nType == 1)
                strPath = strLogfilePath + "\\work_";
            else if (nType == 2)
                strPath = strLogfilePath + "\\setting_";
            else if (nType == 3)
                strPath = strLogfilePath + "\\error_";

            string strToday = string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string strHead = string.Format(",{0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            strPath = strPath + strToday + ".txt";
            strHead = strToday + strHead;

            string strSave;
            strSave = strHead + ',' + strLog;
            Fnc_WriteFile(strPath, strSave);
        }

        static public void Fnc_WriteFile(string strFileName, string strLine)
        {
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(strFileName, true))
            {
                file.WriteLine(strLine);
            }
        }

        private void button_setting_Click(object sender, EventArgs e)
        {
            if (nWorkMode != 0)
                return;

            if (bAdminLogin == false)
            {
                Form_Login Frm_Login = new Form_Login();

                Frm_Login.LogIn_Init();
                Frm_Login.ShowDialog();
            }

            if (bAdminLogin)
            {
                Fnc_Show_SettingViewer();
            }
        }

        //Process
        public static void Process_GetWorkInformation(WorkInfo Info)
        {
            //nWorkinch = Info.ninch;5
            strWork_Bank = Info.strBank;
            strWork_DevicePos = Info.strDevicePos;
            strWork_LotidPos = Info.strLotidPos;
            strWork_LotDigit = Info.strLotDigit;
            strWork_QtyPos = Info.strQtyPos;
            strWork_SPR = Info.strSPR;
            nWork_BcrType = Info.nBcrPrintType;
            strWork_Udigit = Info.strUdigit;
            strWork_WfrQtyPos = Info.strWfrPos;
            strWork_MtlType = Info.strMtlType;
            strLot2Wfr = Info.strLot2Wfr;
            strMultiLot = Info.strMultiLot;
            strTTLWFR = Info.strTTLWFR;
            strWork_WSNPos = Info.strWSN;
        }        
    }
}

public class WorkInfo
{
    //public int ninch = 0;
    public int nBcrcount = 0;
    public string strCust = "";
    public string strBank = "";
    public string strDevicePos = "";
    public string strLotidPos = "";
    public string strLotDigit = "";
    public string strQtyPos = "";
    public string strSPR = "";
    //public string strShot1Lot = "";
    public string strMultiLot = "";
    public string strModelName = "";
    public int nBcrPrintType = 0;
    public string strUdigit = "";
    public string strWfrPos = "";
    public string strMtlType = "";
    public string strLot2Wfr = "";
    public string strTTLWFR = "";
    public string strWSN = "";
    public string strExcelOut = "";
}
