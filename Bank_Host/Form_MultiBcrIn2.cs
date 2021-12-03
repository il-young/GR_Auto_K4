using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Host
{
    public partial class Form_MultiBcrIn2 : Form
    {
        public bool bState = false;
        public static string strDeviceinfo = "";
        public string strBcrType = "", strDevicePos = "", strLotPos = "", strWfrQtyPos = "", strDieQtyPos = "";
        public int nWorkType = 0, nDevicePos = 0, nLotPos = 0, nDieQtyPos = 0, nWfrQtyPos = 0;

        public Form_MultiBcrIn2()
        {
            InitializeComponent();
        }

        public void Set_1d_input()
        {
            label_2dbcr.Visible = false;
            textBox_2dbcr.Visible = false;
            checkBox_devicefix.Visible = true;

            if(strDevicePos.Split(',')[0] == "-1" )
            {
                textBox_device.Enabled = false;
            }
            else
            {
                textBox_device.Enabled = true;
            }

            if(strLotPos.Split(',')[0] == "-1")
            {
                textBox_lot.Enabled = false;
            }
            else
            {
                textBox_lot.Enabled = true;
            }
            
            if(strDieQtyPos.Split(',')[0] == "-1")
            {
                textBox_qty.Enabled = false;
            }
            else
            {
                textBox_qty.Enabled = true;
            }

            if(strWfrQtyPos.Split(',')[0] == "-1")
            {
                textBox_wftqty.Enabled = false;
            }
            else
            {
                textBox_wftqty.Enabled = true;
            }

            nWorkType = 2;
        }

        private void Set_2d_input()
        {
            label_2dbcr.Visible = true;
            textBox_2dbcr.Visible = true;
            checkBox_devicefix.Visible = false;
            textBox_device.Enabled = false;
            textBox_lot.Enabled = false;
            textBox_qty.Enabled = false;
            textBox_wftqty.Enabled = false;

            nWorkType = 1;
        }

        public void Fnc_Init()
        {
            strBcrType = BankHost_main.Host.Host_Get_BcrType(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);
            strDevicePos = BankHost_main.strWork_DevicePos;
            strLotPos = BankHost_main.strWork_LotidPos;
            strWfrQtyPos = BankHost_main.strWork_WfrQtyPos;
            strDieQtyPos = BankHost_main.strWork_QtyPos;

            if (strBcrType == "DATAMATRIX" || strBcrType == "PDF417" || strBcrType == "QR")
            {
                Set_2d_input();
            }
            else
            {
                Set_1d_input();
            }

            textBox_device.Text = "";
            textBox_lot.Text = "";
            textBox_qty.Text = "";
            textBox_wftqty.Text = "";
            textBox_2dbcr.Text = "";

            textBox_2dbcr.ImeMode = ImeMode.Alpha;
            textBox_device.ImeMode = ImeMode.Alpha;
            textBox_lot.ImeMode = ImeMode.Alpha;
            textBox_qty.ImeMode = ImeMode.Alpha;
            textBox_wftqty.ImeMode = ImeMode.Alpha;

            string[] strSplit_DevicePos = strDevicePos.Split(',');
            nDevicePos = Int32.Parse(strSplit_DevicePos[0]);

            string[] strSplit_LotPos = strLotPos.Split(',');
            nLotPos = Int32.Parse(strSplit_LotPos[0]);

            string[] strSplit_DieQtyPos = strDieQtyPos.Split(',');
            nDieQtyPos = Int32.Parse(strSplit_DieQtyPos[0]);

            if (strWfrQtyPos != "")
            {
                string[] strSplit_WfrQtyPos = strWfrQtyPos.Split(',');
                nWfrQtyPos = Int32.Parse(strSplit_WfrQtyPos[0]);
            }
            else
                nWfrQtyPos = -1;

            if (strDevicePos == "" || strDevicePos == "-1")
            {
                checkBox_devicefix.Visible = true;
                textBox_device.Enabled = false;
            }

            if(strDieQtyPos == "" || strDieQtyPos == "-1")
            {
                textBox_qty.Enabled = false;
            }

            if (strWfrQtyPos == "" || strWfrQtyPos == "-1")
            {
                textBox_wftqty.Enabled = false;
            }

            if (nWorkType == 1)
            {
                if (checkBox_devicefix.Checked == false)
                {
                    textBox_2dbcr.Focus();
                }
                else
                {
                    textBox_device.Text = strDeviceinfo;
                    textBox_2dbcr.Focus();
                }
            }
            else
            {
                if (checkBox_devicefix.Checked == false)
                {
                    textBox_device.Focus();
                }
                else
                {
                    textBox_device.Text = strDeviceinfo;
                    textBox_lot.Focus();
                }
            }
        }

        private void textBox_lot_TextChanged(object sender, EventArgs e)
        {

        }

        public void Fnc_Show()
        {
            try
            {
                if (!bState)
                {
                    Show();

                    Fnc_Init();
                    bState = true;
                }
            }
            catch
            {

            }
        }

        public void Fnc_Hide()
        {
            if (bState)
            {
                bState = false;
                Hide();
            }
        }

        private void textBox_2dbcr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string strInfo = textBox_2dbcr.Text.ToUpper();

                if (BankHost_main.nScanMode == 1)
                {
                    BankHost_main.strScanData = strInfo;

                    BankHost_main.bGunRingMode_Run = true;

                    Fnc_Hide();
                }

                /*
                if (textBox_2dbcr.ImeMode != ImeMode.Alpha)
                {
                    textBox_2dbcr.ImeMode = ImeMode.Alpha;
                }

                string strSperator = BankHost_main.strWork_SPR;

                if (strSperator == "SPACE")
                    strSperator = " ";

                char[] charArr = strSperator.ToCharArray();

                string strGetData = textBox_2dbcr.Text;

                int nSpaceCount = 0;

                if (strGetData.Contains(strSperator))
                {
                    string[] strGetDataSplit = strGetData.Split(charArr);

                    int nCount = strGetDataSplit.Length + 1;

                    if (strSperator == " ")
                    {                        
                        string[] strDivid = new string[nCount];

                        nSpaceCount = 0;

                        for (int n = 0; n<nCount-1; n++)
                        {
                            if(strGetDataSplit[n] != "")
                            {
                                strDivid[nSpaceCount] = strGetDataSplit[n];
                                nSpaceCount++;
                            }
                        }

                        if(nDevicePos != -1)
                            textBox_device.Text = strDivid[nDevicePos];

                        if (nLotPos != -1)
                        {
                            if(strDivid[nLotPos].Length < 5)
                                textBox_lot.Text = strDivid[nLotPos + 1];
                            else
                                textBox_lot.Text = strDivid[nLotPos];
                        }

                        if (nDieQtyPos != -1)
                            textBox_qty.Text = strDivid[nDieQtyPos];

                        if (nWfrQtyPos != -1)
                            textBox_wftqty.Text = strDivid[nWfrQtyPos];

                    }
                    else
                    {
                        nSpaceCount = nCount;
                        if (nDevicePos != -1)
                            textBox_device.Text = strGetDataSplit[nDevicePos];

                        if (nLotPos != -1)
                            textBox_lot.Text = strGetDataSplit[nLotPos];

                        if (nDieQtyPos != -1)
                            textBox_qty.Text = strGetDataSplit[nDieQtyPos];

                        if (nWfrQtyPos != -1)
                            textBox_wftqty.Text = strGetDataSplit[nWfrQtyPos];
                    }
                }

                string strBcrSum = "";

                string[] strMakeBcr = new string[nSpaceCount];
                for (int i = 0; i < nSpaceCount; i++)
                {
                    strMakeBcr[i] = "";
                }

                //if (nDevicePos > -1)
                    strMakeBcr[0] = textBox_device.Text;

                if (nLotPos > -1)
                    strMakeBcr[1] = textBox_lot.Text;

                if (nDieQtyPos > -1)
                    strMakeBcr[2] = textBox_qty.Text;

                if (nWfrQtyPos > -1)
                    strMakeBcr[3] = textBox_wftqty.Text;


                for (int i = 0; i < nSpaceCount; i++)
                {
                    string str = strMakeBcr[i] + strSperator;
                    strBcrSum = strBcrSum + str;
                }

                strDeviceinfo = textBox_device.Text;

                if (BankHost_main.nScanMode == 1)
                {
                    BankHost_main.strScanData = strBcrSum;

                    BankHost_main.bGunRingMode_Run = true;

                    Fnc_Hide();
                }
                */
            }
        }

        private void textBox_wftqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (textBox_wftqty.ImeMode != ImeMode.Alpha)
                {
                    textBox_wftqty.ImeMode = ImeMode.Alpha;
                }

                if(strDieQtyPos == "" || strDieQtyPos == "-1")
                {
                    string strBcrSum = "";

                    string[] strMakeBcr = new string[10];
                    for (int i = 0; i < 10; i++)
                    {
                        strMakeBcr[i] = "";
                    }

                    strMakeBcr[0] = textBox_device.Text;
                    strMakeBcr[1] = textBox_lot.Text;
                    strMakeBcr[3] = textBox_wftqty.Text;

                    string strSperator = BankHost_main.strWork_SPR;

                    for (int i = 0; i < 10; i++)
                    {
                        string str = strMakeBcr[i] + strSperator;
                        strBcrSum = strBcrSum + str;
                    }

                    if (BankHost_main.nScanMode == 1)
                    {
                        BankHost_main.strScanData = strBcrSum;

                        BankHost_main.bGunRingMode_Run = true;

                        Fnc_Hide();
                    }
                }
                else
                    textBox_qty.Focus();
            }
        }

        private void textBox_device_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (textBox_device.ImeMode != ImeMode.Alpha)
                {
                    textBox_device.ImeMode = ImeMode.Alpha;
                }

                strDeviceinfo = textBox_device.Text;
                textBox_lot.Focus();
            }
        }

        private void textBox_lot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (textBox_lot.ImeMode != ImeMode.Alpha)
                {
                    textBox_lot.ImeMode = ImeMode.Alpha;
                }

                if(strWfrQtyPos == "" || strWfrQtyPos == "-1")
                {
                    if(strDieQtyPos == "" || strDieQtyPos == "-1")
                    {
                        string strBcrSum = "";

                        string[] strMakeBcr = new string[10];
                        for (int i = 0; i < 10; i++)
                        {
                            strMakeBcr[i] = "";
                        }

                        strMakeBcr[0] = textBox_device.Text;
                        strMakeBcr[1] = textBox_lot.Text;

                        string strSperator = BankHost_main.strWork_SPR;

                        for (int i = 0; i < 10; i++)
                        {
                            string str = strMakeBcr[i] + strSperator;
                            strBcrSum = strBcrSum + str;
                        }

                        if (BankHost_main.nScanMode == 1)
                        {
                            BankHost_main.strScanData = strBcrSum;

                            BankHost_main.bGunRingMode_Run = true;

                            Fnc_Hide();
                        }
                    }
                    else
                        textBox_qty.Focus();
                }
                else
                    textBox_wftqty.Focus();

                strDeviceinfo = textBox_device.Text;
            }
        }

        private void textBox_qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (textBox_qty.ImeMode != ImeMode.Alpha)
                {
                    textBox_qty.ImeMode = ImeMode.Alpha;
                }

                string strBcrSum = "";

                string[] strMakeBcr = new string[10];
                for(int i=0; i<10; i++)
                {
                    strMakeBcr[i] = "";
                }

                textBox_device.Text = textBox_device.Text.Trim();
                textBox_lot.Text = textBox_lot.Text.Trim();
                textBox_qty.Text = textBox_qty.Text.Trim();
                textBox_wftqty.Text = textBox_wftqty.Text.Trim();

                if(nDevicePos > -1)
                    strMakeBcr[nDevicePos] = textBox_device.Text;

                if(nLotPos > -1)
                    strMakeBcr[nLotPos] = textBox_lot.Text;

                if(nDieQtyPos > -1)
                    strMakeBcr[nDieQtyPos] = textBox_qty.Text;

                if(nWfrQtyPos > -1)
                    strMakeBcr[nWfrQtyPos] = textBox_wftqty.Text;                

                string strSperator = BankHost_main.strWork_SPR;

                for (int i = 0; i < 10; i++)
                {
                    string str = strMakeBcr[i] + strSperator;
                    strBcrSum = strBcrSum + str;
                }

                if (BankHost_main.nScanMode == 1)
                {
                    BankHost_main.strScanData = strBcrSum;

                    BankHost_main.bGunRingMode_Run = true;

                    Fnc_Hide();
                }
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("바코드 입력 창을 닫으시겠습니까?\n\n닫은 후에는 작업 종료 후 다시 사용 가능 합니다.", "Alart", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.Yes)
            {
                bState = true;
                Hide();
            }
        }
    }
}
