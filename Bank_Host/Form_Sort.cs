﻿using System;
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


namespace Bank_Host
{
    public partial class Form_Sort : Form
    {
        Form_Progress Frm_Process = new Form_Progress();

        public string strExcutionPath = "", strWorkFileName = "", strWorkCust = "";
        string strSelDevice = "";
        public static string strNewLotname = "", strPrintName = "";
        public static bool bPrintUse = false;
        public static int nProcess = 0, nResult = 0;
        public static string strValDevice = "", strValLot = "", strValDcc = "", strValWfrcount = "", strValReadfile = "";
        public static string strGR_Device = "", strGR_Lot = "", strGR_AmkorID = "";
        public static int nValDiettl = 0, nValDieQty = 0, nValWfrttl = 0, nValWfrQty = 0, nLabelcount = 0, nLabelttl = 0;
        public static bool bupdate = false, bRun = false, bGridViewUpdate = false, bunprinted_device = false, bGRrun = false;
        public static string[] strSelBillno = new string[20] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        public static string strSelCust = "" , strSelBill = "", strInputBill = "", strSelJobName = "";

        SpeechSynthesizer speech = new SpeechSynthesizer();

        Form_Print Frm_Print = new Form_Print();

        public Form_Sort()
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath);
            
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
            var dt_list = BankHost_main.Host.Host_Get_BCRFormat();

            if (dt_list.Rows.Count == 0)
                return;

            string strCust = "", strName = "";

            comboBox_cust.Items.Clear();
            comboBox_Name.Items.Clear();
            comboBox_inch.Items.Clear();

            comboBox_Name.Items.Add("모델명을 입력 하세요!");

            for (int n = 0; n < dt_list.Rows.Count; n++)
            {
                WorkInfo AWork = new WorkInfo();

                AWork.strCust = dt_list.Rows[n]["CUST"].ToString(); AWork.strCust = AWork.strCust.Trim();
                AWork.strBank = dt_list.Rows[n]["BANK_NO"].ToString(); AWork.strBank = AWork.strBank.Trim();
                AWork.strDevicePos = dt_list.Rows[n]["DEVICE"].ToString(); AWork.strDevicePos = AWork.strDevicePos.Trim();
                AWork.strLotidPos = dt_list.Rows[n]["LOTID"].ToString(); AWork.strLotidPos = AWork.strLotidPos.Trim();
                AWork.strLotDigit = dt_list.Rows[n]["LOT_DIGIT"].ToString(); AWork.strLotDigit = AWork.strLotDigit.Trim();
                AWork.strQtyPos = dt_list.Rows[n]["WFR_QTY"].ToString(); AWork.strQtyPos = AWork.strQtyPos.Trim();
                AWork.strSPR = dt_list.Rows[n]["SPR"].ToString(); AWork.strSPR = AWork.strSPR.Trim();
                AWork.strMultiLot = dt_list.Rows[n]["MULTI_LOT"].ToString(); AWork.strMultiLot = AWork.strMultiLot.Trim();
                AWork.strModelName = dt_list.Rows[n]["NAME"].ToString(); AWork.strModelName = AWork.strModelName.Trim();
                AWork.strMtlType = dt_list.Rows[n]["MTL_TYPE"].ToString(); AWork.strMtlType = AWork.strMtlType.Trim();

                if (strCust != AWork.strCust)
                {
                    strCust = AWork.strCust;

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
                        comboBox_cust.Items.Add(strCust);
                    }
                }

                if (strName != AWork.strModelName)
                {
                    strName = AWork.strModelName;
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
                            if(AWork.strMtlType == "FOSB")
                                comboBox_Name.Items.Add(strName);
                        }
                        else
                        {
                            if (AWork.strMtlType != "FOSB")
                                comboBox_Name.Items.Add(strName);
                        }
                    }
                }
            }
        }

        public void Fnc_Get_Information_Model(string strCust)
        {
            var dt_list = BankHost_main.Host.Host_Get_BCRFormat();

            if (dt_list.Rows.Count == 0)
                return;

            string strName = "";

            comboBox_Name.Items.Clear();
            comboBox_Name.Items.Add("모델명을 입력 하세요!");

            for (int n = 0; n < dt_list.Rows.Count; n++)
            {
                WorkInfo AWork = new WorkInfo();

                AWork.strCust = dt_list.Rows[n]["CUST"].ToString(); AWork.strCust = AWork.strCust.Trim();
                AWork.strBank = dt_list.Rows[n]["BANK_NO"].ToString(); AWork.strBank = AWork.strBank.Trim();
                AWork.strDevicePos = dt_list.Rows[n]["DEVICE"].ToString(); AWork.strDevicePos = AWork.strDevicePos.Trim();
                AWork.strLotidPos = dt_list.Rows[n]["LOTID"].ToString(); AWork.strLotidPos = AWork.strLotidPos.Trim();
                AWork.strLotDigit = dt_list.Rows[n]["LOT_DIGIT"].ToString(); AWork.strLotDigit = AWork.strLotDigit.Trim();
                AWork.strQtyPos = dt_list.Rows[n]["WFR_QTY"].ToString(); AWork.strQtyPos = AWork.strQtyPos.Trim();
                AWork.strSPR = dt_list.Rows[n]["SPR"].ToString(); AWork.strSPR = AWork.strSPR.Trim();
                AWork.strMultiLot = dt_list.Rows[n]["MULTI_LOT"].ToString(); AWork.strMultiLot = AWork.strMultiLot.Trim();
                AWork.strModelName = dt_list.Rows[n]["NAME"].ToString(); AWork.strModelName = AWork.strModelName.Trim();
                AWork.strMtlType = dt_list.Rows[n]["MTL_TYPE"].ToString(); AWork.strMtlType = AWork.strMtlType.Trim();

                if (strCust == AWork.strCust)
                {
                    if (strName != AWork.strModelName)
                    {
                        strName = AWork.strModelName;
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
                                if (AWork.strMtlType == "FOSB")
                                    comboBox_Name.Items.Add(strName);
                            }
                            else
                            {
                                if (AWork.strMtlType != "FOSB")
                                    comboBox_Name.Items.Add(strName);
                            }

                        }
                    }
                }
            }
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

            bool bJudge = Frm_Print.Fnc_Print(amkorBcrInfo, nBcrType, nIndex, nttl);

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
            
            if(nSel == -1)
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
            Fnc_Get_Information_Model(strSelCust);
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
                    item.state + "\t" + item.strop + "\t" + item.strGRstatus + "\t" + item.Default_WQty;

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

                Application.DoEvents();
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

                Application.DoEvents();
            }

            nCount = 1;

            foreach (var item in list)
            {                
                dataGridView_worklist.Rows.Add(new object[13] { nCount, item.Cust, item.Device, item.Lot, item.Lot_Dcc, item.Rcv_Qty, item.Default_WQty, item.Rcvddate,
                    item.Lot_type, item.Bill, item.Amkorid, item.Wafer_lot, item.shipment });

                nCount++;
            }

            Frm_Process.Form_Display("\n작업을 마침니다.");
            Frm_Process.Hide();

            return list.Count;
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

                Application.DoEvents();
            }

            nCount = 1;

            foreach (var item in list)
            {
                strSelCust = item.Cust;

                dataGridView_worklist.Rows.Add(new object[13] { nCount, item.Cust, item.Device, item.Lot, item.Lot_Dcc, item.Rcv_Qty, item.Default_WQty, item.Rcvddate,
                    item.Lot_type, item.Bill, item.Amkorid, item.Wafer_lot, "" });

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

            DataTable dt = new DataTable();

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
            dataGridView_worklist.Columns.Add("SHIPMENT", "SHIPMENT");

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
                    dataGridView_worklist.Rows.Add(new object[13] { nIdex, data.Cust, data.Device, data.Lot, data.Lot_Dcc, data.Rcv_Qty, data.Default_WQty, data.Rcvddate,
                    data.Lot_type, data.Bill, data.Amkorid, data.Wafer_lot, data.shipment });

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
                    item.state + "\t" + item.strop + "\t" + item.strGRstatus + "\t" + item.Default_WQty + "\t" + item.shipment;

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

                Application.DoEvents();
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

            DataTable dt = new DataTable();

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
            for(int n = 0; n<list.Count; n++ )
            {
                string str = list[n].Device;

                if(strDeviceName != str)
                {
                    strTotalDevice = strTotalDevice + str + "_";
                    strDeviceName = str;
                }
            }
            strTotalDevice = strTotalDevice.Substring(0, strTotalDevice.Length - 1);

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
                    item.state + "\t" + item.strop + "\t" + item.strGRstatus + "\t" + item.Default_WQty;

                //if (strDevicecheck != item.Device)
                //{
                //    Fnc_WriteFile(strSavepath, strTotalDevice);
                //    strDevicecheck = item.Device;
                //}

                /////////////////////////////////////Device 폴더 생성
                //sDirDeviceNamePath = sDirFileNamePath + "\\" + item.Device;
                sDirDeviceNamePath = sDirFileNamePath + "\\" + strTotalDevice;
                DirectoryInfo diinfo = new DirectoryInfo(sDirDeviceNamePath);
                if (diinfo.Exists == false)
                {
                    diinfo.Create();
                }
                diinfo = null;
                /////////////////////////////////////File 저장
                string strLotsavepath = sDirDeviceNamePath + "\\" + strTotalDevice + ".txt";
                Fnc_WriteFile(strLotsavepath, strTxtline);
                ////////////////////////////////////

                nCount++;
                strMsg = string.Format("\n\n 작업 준비 중 입니다. {0} / {1}", nCount, list.Count);
                Frm_Process.Form_Display(strMsg);

                Application.DoEvents();
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

                Application.DoEvents();

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

                dataGridView_worklist.Rows.Add(new object[13] { nCount, item.Cust, item.Device, item.Lot, item.Lot_Dcc, item.Rcv_Qty, item.Default_WQty, item.Rcvddate,
                    item.Lot_type, item.Bill, item.Amkorid, item.Wafer_lot, item.shipment });

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

        public void Fnc_WorkDownload(string strWorkName)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkName + ".txt";

            string[] data = Fnc_ReadFile(strFileName);

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

            dataGridView_sort.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[12].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[13].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[14].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[15].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_sort.Columns[16].SortMode = DataGridViewColumnSortMode.NotSortable;

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

                Application.DoEvents();
                Thread.Sleep(1500);

                tabControl_Sort.SelectedIndex = 0;

                return;
            }
            List<StorageData> list_Job = new List<StorageData>();

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
                    if (strSplit_data.Length > 17)
                        st.shipment = strSplit_data[17];
                    else
                        st.shipment = "";

                    list_Job.Add(st);
                }
            }

            list_Job.Sort(CompareStorageData);

            int nCount = 1, nWait = 0, nWork = 0, nComplete = 0, nError = 0, nGR = 0;
            foreach (var item in list_Job)
            {
                dataGridView_sort.Rows.Add(new object[18] { nCount, item.Cust, item.Device, item.Lot, item.Rcv_Qty, item.Die_Qty, item.Default_WQty, item.Rcv_WQty, item.Rcvddate,
                    item.Lot_type, item.Bill, item.Amkorid, item.Wafer_lot, item.strCoo, item.state, item.strop, item.strGRstatus, item.shipment });

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

                if(item.strGRstatus == "COMPLETE" || item.strGRstatus == "Complete")
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

                if (strEqid == BankHost_main.strEqid && strToday == strDate && strHAWB != "")
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
            //dataGridView_workinfo.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;

            string strFileName = "";

            strFileName = BankHost_main.Host.Host_Get_JobfileName(BankHost_main.strEqid, strBill);
            if (strFileName != "")
                Fnc_WorkDownload(strFileName);
            else
            {
                MessageBox.Show("작업 이력을 불러 올 수 없습니다.!");
                return;
            }

            Application.DoEvents();

            int nLotcount = dataGridView_sort.Rows.Count;

            int nCount = 0;
            int nWait = 0, nWork = 0, nComplete = 0, nError = 0, nGr = 0;            

            for (int n = 0; n<nLotcount; n++)
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

                if (strGetBill == strBill)
                {
                    nCount++;
                    dataGridView_workinfo.Rows.Add(new object[11] { strGetBill, strGetCust, strGetDevice, strGetLot, strGetDiettl,
                        strGetWfrqty, strGetWfrttl,strGetAmkorid, strGetVali,strGetGr, strGetShipment});

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
                    else if(strGetGr == "ERROR")
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

            string strShipment = "";
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

            if(dataGridView_shipment.Rows.Count > 0)
            {
                dataGridView_shipment.Columns.Insert(0, checkBoxColumn);
                dataGridView_shipment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                dataGridView_shipment.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView_shipment.Sort(this.dataGridView_shipment.Columns["SHIPMENT"], ListSortDirection.Ascending);

                for(int k=0; k< dataGridView_shipment.Rows.Count; k++)
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
                var taskResut = Task.Run(async () =>
                {
                    return await BankHost_main.Host.Fnc_AutoGR(strgr);
                });

                string strResult = taskResut.Result;

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

                    string strResult2 = taskResut.Result;

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
        public string Gr_Process_Update(string strDevice, string strLot)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
            string strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + ".txt";
            strValReadfile = strFileName + "\\" + strDevice + "\\" + strDevice + ".txt";

            string strSaveFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
            string strSaveFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strDevice + "\\" + strDevice;
            string strlog = "";

            int dataIndex = Fnc_Getline_GR(strValReadfile, strLot,"", "",false);
            int deviceindex = Fnc_Getline_GR(strFileName_Device, strDevice,"", "", false);

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
                    st.Rcv_WQty + "\t" + st.Rcvddate + "\t" + st.Lot_type + "\t" + st.Bill + "\t" + st.Amkorid + "\t" + st.Wafer_lot + "\t" + st.strCoo + "\t" + st.state + "\t" + st.strop + "\t" + st.strGRstatus + "\t" + st.Default_WQty + "\t" + st.shipment;

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
                    item.state + "\t" + item.strop + "\t" + item.strGRstatus + "\t" + item.Default_WQty;

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

                Application.DoEvents();
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
                Application.DoEvents();
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
                    item.state + "\t" + item.strop + "\t" + item.strGRstatus + "\t" + item.Default_WQty;

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

                Application.DoEvents();
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
            if (bGRrun)
                return;

            int n = tabControl_Sort.SelectedIndex;
            BankHost_main.nSortTabNo = n;

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

                    Application.DoEvents();
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
            else if(n == 4)
            {
                if (strWorkFileName == "" || BankHost_main.strOperator == "")
                {
                    tabControl_Sort.SelectedIndex = 0;
                    return;
                }

                Fnc_Get_Unprinted_Deviceinfo();
                textBox_unprinted_device.Text = "";
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
                Application.DoEvents();
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
                    BankHost_main.Host.Host_Delete_BcrReadinfo(BankHost_main.strEqid, strLot,0);

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

                    //if(strGrState == "Working")
                    //{ 
                        BankHost_main.strWork_Lotinfo = "";
                    //}

                    textBox_Readdata.Focus();
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

            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
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
                strSaveFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strDevice + "\\" + strDevice;
            }

            string strlog = "";

            bool bReset = false;
            if (bupdate)
                bReset = false;
            else
                bReset = true;

            int dataIndex = 0;
            if (nDiettl == nDieQty)
                dataIndex = Fnc_Getline_Revision(strValReadfile, strLot, nDiettl.ToString());
            else
                dataIndex = Fnc_Getline(strValReadfile, strLot, strDcc, nDieQty.ToString(), bReset);

            string strSpeak = "";

            if (dataIndex == 9999)
            {
                label_msg.Text = "Error";

                strSpeak = string.Format("에러");
                speech.SpeakAsync(strSpeak);

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

            if (nDiettl == nDieQty)
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
            if (strSplit_data.Length > 17)
                st.shipment = strSplit_data[17];
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

            if(BankHost_main.nMaterial_type == 1)
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

            if (nQty == 0)
            {
                label_info.Text = string.Format("{0} - {1}", deviceindex + 1, Realindex + 1);
                label_info.BackColor = Color.DarkGray;
                label_info.ForeColor = Color.White;
                st.state = "Waiting";
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

                    Fnc_SaveLog_Work(strSaveFileName_Device, strlog, strSaveInfo,1);
                }
            }
            else if (nQty == nttl)
            {
                label_info.Text = string.Format("{0} - {1} 완료", deviceindex + 1, Realindex + 1);
                label_info.BackColor = Color.Blue;
                label_info.ForeColor = Color.White;
                st.state = "Complete";

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

            string strTxtline = st.Cust + "\t" + st.Device + "\t" + st.Lot + "\t" + st.Lot_Dcc + "\t" + st.Rcv_Qty + "\t" + st.Die_Qty + "\t" +
                    st.Rcv_WQty + "\t" + st.Rcvddate + "\t" + st.Lot_type + "\t" + st.Bill + "\t" + st.Amkorid + "\t" + st.Wafer_lot + "\t" + st.strCoo + "\t" + st.state + "\t" + st.strop + "\t" + st.strGRstatus + "\t" + st.Default_WQty + "\t" + st.shipment;

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
                Application.DoEvents();
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

        public string Fnc_Update_GR(string strDevice, string strLot, string state)
        {
            string strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\";
            string strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + ".txt";
            strValReadfile = strFileName + "\\" + strDevice + "\\" + strDevice + ".txt";

            string strSaveFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
            string strSaveFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strDevice + "\\" + strDevice;
            string strlog = "";

            int dataIndex = Fnc_Getline_GR(strValReadfile, strLot,"", "", false);
            int deviceindex = Fnc_Getline_GR(strFileName_Device, strDevice,"", "", false);

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
                    st.Rcv_WQty + "\t" + st.Rcvddate + "\t" + st.Lot_type + "\t" + st.Bill + "\t" + st.Amkorid + "\t" + st.Wafer_lot + "\t" + st.strCoo + "\t" + st.state + "\t" + st.strop + "\t" + st.strGRstatus + "\t" + st.Default_WQty + "\t" + st.shipment;

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
        public int Fnc_Getline_Revision(string strfilepath, string strData, string strCprQty)
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

                    if (strData == st.Lot && strCprQty == st.Rcv_Qty)
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

        public int Fnc_GetLotindex(string strData, string strDcc,  string strDieqty, bool bupdate)
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
                        if (strGetDiettl == strDieqty)
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
                            if(strGetState != "complete" && strGetState != "error")
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
            strValReadfile = strFileName + "\\" + strDevice + "\\" + strDevice + ".txt";

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
                strLotno = dataGridView_Lot.Rows[n].Cells[1].Value.ToString();
                strDieQty = dataGridView_Lot.Rows[n].Cells[3].Value.ToString();
                if (strData == strLotno && strDieQty == strQty)
                {
                    return n;
                }
            }

            return -1;
        }


        private void textBox_Readdata_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                if (textBox_Readdata.ImeMode != ImeMode.Alpha)
                {
                    textBox_Readdata.ImeMode = ImeMode.Alpha;
                }

                if (BankHost_main.nScanMode == 1)
                {
                    BankHost_main.strScanData = textBox_Readdata.Text;

                    BankHost_main.bGunRingMode_Run = true;

                    while (BankHost_main.bGunRingMode_Run)
                    {
                        Thread.Sleep(1);
                        Application.DoEvents();
                    }

                    textBox_Readdata.Text = "";
                    textBox_Readdata.Focus();
                }
            }
        }
        public Bcrinfo Fnc_Bcr_Parsing(string strBcr)
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

            char seperator = char.Parse(BankHost_main.strWork_SPR);
            bool bmultibcr = false;

            //1D Scan 인지 확인
            string strBcrType = BankHost_main.Host.Host_Get_BcrType(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
            string str1Dbcrcount = "0";
            bool b1Dbcr = false;

            if (strBcrType == "CODE39" || strBcrType == "CODE128")
            {
                b1Dbcr = true;
                str1Dbcrcount = BankHost_main.Host.Host_Get_Bcrcount(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
            }

            if (strBcr.Contains(',') && !b1Dbcr && strBcrType != "PDF417" && BankHost_main.strWork_Shot1Lot == "YES")
            {
                if(BankHost_main.strWork_Cust != "453" || BankHost_main.strWork_Cust != "734")
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

            if(strUdigit[0] == "D")
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

                        if(strUdigit[1] == strBarcode.Substring(0, strUdigit.Length))
                        {
                            strID = strBarcode;
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
                bcr.Lot = bcr.Lot.Substring(0, st-index);
            }

            nValWfrQty = BankHost_main.Host.Host_Get_BcrRead_Wfrcount(BankHost_main.strEqid, bcr.Lot);

            if (bcr.DieQty == "" || bcr.Lot == "")
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

            int nQty = Int32.Parse(bcr.DieQty);

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
                return null;

            bcr.Device = strSplit_Bcr[nDevicePos];
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
                    int n = Int32.Parse(strSplit_DevicePos[1].Substring(1, 1));
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

            if(bcr.WfrQty != "")
                BankHost_main.nWorkBcrcount = Int32.Parse(bcr.WfrQty);

            strFileName = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + strWorkFileName;
            strFileName_Device = strExcutionPath + "\\Work\\" + strWorkFileName + "\\" + bcr.Device + "\\" + bcr.Device;

            bcr.DieTTL = nDieTTL.ToString();
            bcr.WfrTTL = nWfrTTL.ToString();

            if (bcr.DieQty == "")
                bcr.DieQty = bcr.DieTTL;

            if (bcr.WfrQty == "")
                bcr.WfrQty = bcr.WfrTTL;

            string strSetID = bcr.Lot + "_" + bcr.DieQty;
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

                Application.DoEvents();
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

                Application.DoEvents();
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

            string strGrMethod = BankHost_main.Host.Host_Get_GrMethod(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
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
            if(textBox_unprinted_device.Text == "")
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
            DataTable dt = BankHost_main.Host.Host_Get_Unprinted_Device();

            dataGridView_unprintedinfo.Columns.Clear();
            dataGridView_unprintedinfo.Rows.Clear();
            dataGridView_unprintedinfo.Refresh();

            Thread.Sleep(300);

            dataGridView_unprintedinfo.DefaultCellStyle.Font = new Font("Calibri", 15);
            dataGridView_unprintedinfo.Columns.Add("ID", "ID");
            dataGridView_unprintedinfo.Columns.Add("Device", "Device");
            dataGridView_unprintedinfo.Columns.Add("Cust", "Cust");

            int nCount = dt.Rows.Count;
            for (int n = 0; n < nCount; n++)
            {
                string strDev= dt.Rows[n]["DEVICE"].ToString();
                string strCust = dt.Rows[n]["CUST_CODE"].ToString();

                dataGridView_unprintedinfo.Rows.Add(new object[3] { n+1, strDev, strCust });
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
            if(!BankHost_main.IsAutoFocus)
                BankHost_main.IsAutoFocus = true;
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            int nSel = comboBox_hist_device.SelectedIndex;

            if(nSel > 0)
            {
                dataGridView_hist.Columns.Clear();
                dataGridView_hist.Rows.Clear();
                dataGridView_hist.Refresh();

                Thread.Sleep(300);
            }

            if(nSel == 1) //시간별 조회
            {
                Fnc_Get_History();
            }
            else if(nSel == 2) //Bill# 기준
            {
                if(textBox_input.Text == "")
                {
                    MessageBox.Show("Bill# 를 입력 하세요!");
                    textBox_input.Focus();
                    return;
                }

                Fnc_Get_History_Bill(textBox_input.Text);
            }
            else if(nSel == 3) //Device 기준
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

            dataGridView_hist.DefaultCellStyle.Font = new Font("Calibri", 13);
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
            string strDevice = dataGridView_workinfo.Rows[0].Cells[2].Value.ToString() ;

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
            for (int n = 0; n<nLotCount; n++)
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
            string strMsg = strBase + strHawb + strCustNo +  strLots + strTotalQty + strWaferTotalQty + strBase2;

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
            if(dataGridView_shipment.Rows[nIndex].Cells[0].Value == null)
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

            for (int n = 0; n < dataGridView_Lot.RowCount ; n++)
            {
                if(dataGridView_Lot.Rows[n].Cells[1].Value.ToString().Contains(input) == true)
                {
                    dataGridView_Lot.Rows[n].Selected = true;
                    dataGridView_Lot.CurrentCell = dataGridView_Lot.Rows[n].Cells[0];
                    searched_row = n;
                    break;
                }


                if (dataGridView_Lot.Rows[n].Cells[3].Value.ToString().Contains(input) == true)
                {
                    dataGridView_Lot.Rows[n].Selected = true;
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

            for(int i= device_row_num; i< dataGridView_Device.RowCount; i++)
            {
                lot_row = get_wait_position(dataGridView_Device.Rows[i].Cells[1].Value.ToString(), lot_row_num);

                if(lot_row > -1)
                {
                    device_row_num = i;
                    lot_row_num = lot_row;

                    dataGridView_Device_CellClick(i, 0);

                    dataGridView_Lot.Rows[lot_row_num].Selected = true;
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

            for (int m = start_lot+1; m < info.Length; m++)
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
                Application.DoEvents();
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




        private void button_printbill_Click(object sender, EventArgs e)
        {
            Frm_Print.Fnc_Print_Billinfo(strSelBill);
        }

        public void Fnc_Get_History_Bill(string strGetBill)
        {
            var dt = BankHost_main.Host.Host_Get_Histinfo_Job_Bill(strGetBill);

            dataGridView_hist.DefaultCellStyle.Font = new Font("Calibri", 13);
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

            dataGridView_hist.DefaultCellStyle.Font = new Font("Calibri", 13);
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

            string strGrMethod = BankHost_main.Host.Host_Get_GrMethod(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);

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

            for (int n = 0; n<nLotCount; n++)
            {
                bool bcheck = false;

                string strDevice = dataGridView_workinfo.Rows[n].Cells[2].Value.ToString();
                string strLot = dataGridView_workinfo.Rows[n].Cells[3].Value.ToString();
                string strDieqty = dataGridView_workinfo.Rows[n].Cells[4].Value.ToString();
                string strWfrqty = dataGridView_workinfo.Rows[n].Cells[5].Value.ToString();
                string strWfrttl = dataGridView_workinfo.Rows[n].Cells[6].Value.ToString();
                string strAmkorid = dataGridView_workinfo.Rows[n].Cells[7].Value.ToString();
                string strVal = dataGridView_workinfo.Rows[n].Cells[8].Value.ToString();
                string strGr = dataGridView_workinfo.Rows[n].Cells[9].Value.ToString();

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

                        bJudge = Gr_Process_Direct(strDevice, strLot,strAmkorid, strDieqty, strWfrqty);

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
                Thread.Sleep(30);
            }
            
            strMsg = string.Format("\n\nGR 진행 Lot 수량: OK - {0}, NG - {1}", nGrcount-nGRNG, nGRNG);

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

                dataGridView_Lot.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_Lot.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
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

                StorageData st = new StorageData();

                int nWaitcount = 0, nWorkcount = 0, nCompletecount = 0, nErrorcount = 0;

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
                        st.shipment = strSplit_data[17];
                    else
                        st.shipment = "";

                    dataGridView_Lot.Rows.Add(new object[12] { m + 1, st.Lot, st.Lot_Dcc, st.Rcv_Qty, st.Die_Qty, st.Default_WQty, st.Rcv_WQty, st.state, st.strop, st.Bill, st.strGRstatus, st.shipment });

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
                dataGridView_Lot.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_Lot.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_Lot.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_Lot.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_Lot.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_Lot.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_Lot.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_Lot.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                dataGridView_Lot.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                dataGridView_Lot.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            }
            catch
            {
                bGridViewUpdate = false;
            }

            bGridViewUpdate = false;
            return 0;
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
                if(BankHost_main.bHost_connect)
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
            else if(nSel == 1)
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
            else if(nSel == 2)
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
            Fnc_Get_Information_Model(str);
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

        private void button1_Click(object sender, EventArgs e)
        {   
            if(BankHost_main.nScanMode == 0)
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
                       
            if(BankHost_main.strOperator == "")
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

            if (comboBox_Name.Text == "" || comboBox_Name.SelectedIndex == 0)
            {
                MessageBox.Show("모델 선택 하여 주십시오.");
                return;
            }

            BankHost_main.strWork_Cust = label_cust.Text;
            BankHost_main.strWork_Model = comboBox_Name.Text;

            try
            {
                BankHost_main.strWork_Shot1Lot = BankHost_main.Host.Host_Get_Shot1Lot(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
            }
            catch
            {
                BankHost_main.strWork_Shot1Lot = "NO";
            }

            int nMode = comboBox_mode.SelectedIndex;

            string str = "";

            if (nMode == 0 || nMode == 1)
            {
                str = string.Format("\n\n작업을 시작 합니다. AUTO GR 모드 ");
                //상태 변경//
                BankHost_main.Host.Host_Set_Ready(BankHost_main.strEqid, "OK", "1");
                BankHost_main.nWorkMode = 1;

                for(int n=0; n<strSelBillno.Length; n++)
                {
                    if (strSelBillno[n] != "" && strSelBillno[n].Length > 5)
                    {
                        string strJudge = BankHost_main.Host.Host_Set_Workinfo(BankHost_main.strEqid, strWorkFileName, strSelBillno[n], " ", "WORK");
                        if(strJudge != "OK")
                        {
                            MessageBox.Show("Bill 정보 DB 저장 실패!");
                        }
                    }
                }

                button_autogr.Enabled = true;
            }
            else
            {
                str = string.Format("\n\n작업을 시작 합니다. Validation 모드");
                //상태 변경//
                BankHost_main.Host.Host_Set_Ready(BankHost_main.strEqid,"OK", "2");
                BankHost_main.nWorkMode = 2;

                BankHost_main.Host.Host_Set_Workinfo(BankHost_main.strEqid, strWorkFileName, strSelBillno[0], "", "WORK");

                //button_autogr.BackColor = Color.LightGray;
                button_autogr.Enabled = false;
            }

            Frm_Process.Form_Show(str);

            //필요한 정보만 가져오기
            Frm_Process.Hide();

            Fnc_WorkDownload(strWorkFileName);

            BankHost_main.nProcess = 1000; //스캔 대기

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
        public void Fnc_Get_WorkBcrInfo(string strGetCust, string strModelName)
        {
            var dt_list = BankHost_main.Host.Host_Get_BCRFormat();

            if (dt_list.Rows.Count == 0)
                return;

            for (int n = 0; n < dt_list.Rows.Count; n++)
            {
                WorkInfo AWork = new WorkInfo();

                AWork.strCust = dt_list.Rows[n]["CUST"].ToString(); AWork.strCust = AWork.strCust.Trim();
                AWork.strBank = dt_list.Rows[n]["BANK_NO"].ToString(); AWork.strBank = AWork.strBank.Trim();
                AWork.strDevicePos = dt_list.Rows[n]["DEVICE"].ToString(); AWork.strDevicePos = AWork.strDevicePos.Trim();
                AWork.strLotidPos = dt_list.Rows[n]["LOTID"].ToString(); AWork.strLotidPos = AWork.strLotidPos.Trim();
                AWork.strLotDigit = dt_list.Rows[n]["LOT_DIGIT"].ToString(); AWork.strLotDigit = AWork.strLotDigit.Trim();
                AWork.strQtyPos = dt_list.Rows[n]["WFR_QTY"].ToString(); AWork.strQtyPos = AWork.strQtyPos.Trim();
                AWork.strSPR = dt_list.Rows[n]["SPR"].ToString(); AWork.strSPR = AWork.strSPR.Trim();
                AWork.strModelName = dt_list.Rows[n]["NAME"].ToString(); AWork.strModelName = AWork.strModelName.Trim();
                AWork.strUdigit = dt_list.Rows[n]["UDIGIT"].ToString(); AWork.strUdigit = AWork.strUdigit.Trim();
                AWork.strWfrPos = dt_list.Rows[n]["TTL_WFR_QTY"].ToString(); AWork.strWfrPos = AWork.strWfrPos.Trim();
                AWork.strMtlType = dt_list.Rows[n]["MTL_TYPE"].ToString(); AWork.strMtlType = AWork.strMtlType.Trim();

                if (strGetCust == AWork.strCust && strModelName == AWork.strModelName)
                {
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
                    Application.DoEvents();
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

            if (nSel == 0) //GR Mode
            {
                button_sel.Enabled = false;
                button_sel.Text = "GR 리스트 다운로드";
                
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
                        strMsg = string.Format("작업 정보를 가져오는데 실패 하였습니다.");
                        Frm_Process.Form_Display_Warning(strMsg);
                        Thread.Sleep(3000);
                        Frm_Process.Form_Hide();

                        return;
                    }

                    int nCount = Fnc_Get_Worklist_2(taskResut.Result);

                    if(nCount > 0)
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

                if(strGetJobName == "")
                {
                    MessageBox.Show("진행 중인 파일이 없습니다!");
                    return;
                }

                ///작업자 사번 입력 
                Form_Input Frm_Input = new Form_Input();

                Frm_Input.Fnc_Init(nSel);
                Frm_Input.ShowDialog();

                if (BankHost_main.strOperator == "")
                    return;

                label_opinfo.Text = BankHost_main.strOperator;

                strGetJobName = strGetJobName + ".txt";
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
                    Fnc_Get_Information_Model(strSelCust);

                    comboBox_Name.SelectedIndex = 0;
                }
            }
            else if(nSel == 2)
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

                var taskResut = BankHost_main.Host.Fnc_GetBillInformation(strInputBill);

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
                        Fnc_Get_Information_Model(strSelCust);

                        strSelBillno[0] = strInputBill;

                        if(strSelCust == "940")
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
            else if(nSel == 3)
            {
                 button_sel.Enabled = true;
                 button_sel.Text = "Validation 파일 선택";

                Fnc_Information_Init2();

                if (BankHost_main.strOperator == "")
                    return;

                label_opinfo.Text = BankHost_main.strOperator;
                label_cust.Text = strSelCust;

                string strGetJobName = strSelJobName;

                strGetJobName = strGetJobName + ".txt";
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
                    Fnc_Get_Information_Model(strSelCust);

                    comboBox_Name.SelectedIndex = 0;
                }
            }

            string strJudge = BankHost_main.Host.Host_Set_Ready(BankHost_main.strEqid, "WAIT", "");

            if (strJudge != "OK")
            {
                BankHost_main.bHost_connect = false;
                MessageBox.Show("DB 업데이트 실패!");
            }            
        }

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

                Fnc_Get_Information_Model(strSelCust);
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
        public void Fnc_Information_Init2()
        {
            Form_Input Frm_Input = new Form_Input();

            Frm_Input.Fnc_Init(99);

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
                string strCust = strJobName.Substring(4, 3);
                int nCount = n + 1;

                if (strDate == strToday)
                {            
                    Frm_Input.Fnc_datagrid_add(strCust, strHAWB, "-", strJobName);
                }
            }

            Frm_Input.ShowDialog();
        }

        private delegate void Update();
        public void Fnc_BcrInfo(string strInfo)
        {
            //textBox_Readdata.Invoke(new Update(() => textBox_Readdata.Text= strInfo));
            textBox_Readdata.Invoke(new Update(() => textBox_Readdata.Text = strInfo));            
        }

    }

    public class StorageData
    {
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
    }

    public class Bcrinfo
    {
        public string Device = "";
        public string Lot = "";
        public string DieTTL = "";
        public string DieQty = "";
        public string WfrTTL = "";
        public string WfrQty = "";
        public string result = "";
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
    }
}