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

namespace Bank_Host
{
    public partial class Form_MutiBcrIn : Form
    {
        int nBcrcount = 3;
        public bool bState = false;
        public string strWorkFile = "", strExcutionPath = "";
        public Form_MutiBcrIn()
        {
            InitializeComponent();
        }

        public void Fnc_Init()
        {
            textBox_bcrcount.Text = nBcrcount.ToString();
            textBox_bcr1.Text = "";
            textBox_bcr2.Text = "";
            textBox_bcr3.Text = "";
            textBox_bcr4.Text = "";
            textBox_bcr5.Text = "";
            textBox_bcr6.Text = "";
            textBox_bcr7.Text = "";

            textBox_bcr1.ImeMode = ImeMode.Alpha;
            textBox_bcr2.ImeMode = ImeMode.Alpha;
            textBox_bcr3.ImeMode = ImeMode.Alpha;
            textBox_bcr4.ImeMode = ImeMode.Alpha;
            textBox_bcr5.ImeMode = ImeMode.Alpha;
            textBox_bcr6.ImeMode = ImeMode.Alpha;
            textBox_bcr7.ImeMode = ImeMode.Alpha;

            textBox_bcr1.Focus();
        }

        private void textBox_bcrcount_TextChanged(object sender, EventArgs e)
        {
            if (textBox_bcrcount.Text == "")
                return;

            int nCount = Int32.Parse(textBox_bcrcount.Text);
            if (nCount < 1)
                textBox_bcrcount.Text = "1";
            else if(nCount > 7)
                textBox_bcrcount.Text = "7";

            nCount = Int32.Parse(textBox_bcrcount.Text);
            nBcrcount = nCount;

            if (nCount == 1)
            {
                textBox_bcr1.Visible = true;
                label_bcr1.Visible = true;

                textBox_bcr2.Visible = false;
                label_bcr2.Visible = false;

                textBox_bcr3.Visible = false;
                label_bcr3.Visible = false;

                textBox_bcr4.Visible = false;
                label_bcr4.Visible = false;

                textBox_bcr5.Visible = false;
                label_bcr5.Visible = false;

                textBox_bcr6.Visible = false;
                label_bcr6.Visible = false;

                textBox_bcr7.Visible = false;
                label_bcr7.Visible = false;
            }
            else if (nCount == 2)
            {
                textBox_bcr1.Visible = true;
                label_bcr1.Visible = true;

                textBox_bcr2.Visible = true;
                label_bcr2.Visible = true;

                textBox_bcr3.Visible = false;
                label_bcr3.Visible = false;

                textBox_bcr4.Visible = false;
                label_bcr4.Visible = false;

                textBox_bcr5.Visible = false;
                label_bcr5.Visible = false;

                textBox_bcr6.Visible = false;
                label_bcr6.Visible = false;

                textBox_bcr7.Visible = false;
                label_bcr7.Visible = false;
            }
            else if (nCount == 3)
            {
                textBox_bcr1.Visible = true;
                label_bcr1.Visible = true;

                textBox_bcr2.Visible = true;
                label_bcr2.Visible = true;

                textBox_bcr3.Visible = true;
                label_bcr3.Visible = true;

                textBox_bcr4.Visible = false;
                label_bcr4.Visible = false;

                textBox_bcr5.Visible = false;
                label_bcr5.Visible = false;

                textBox_bcr6.Visible = false;
                label_bcr6.Visible = false;

                textBox_bcr7.Visible = false;
                label_bcr7.Visible = false;
            }
            else if (nCount == 4)
            {
                textBox_bcr1.Visible = true;
                label_bcr1.Visible = true;

                textBox_bcr2.Visible = true;
                label_bcr2.Visible = true;

                textBox_bcr3.Visible = true;
                label_bcr3.Visible = true;

                textBox_bcr4.Visible = true;
                label_bcr4.Visible = true;

                textBox_bcr5.Visible = false;
                label_bcr5.Visible = false;

                textBox_bcr6.Visible = false;
                label_bcr6.Visible = false;

                textBox_bcr7.Visible = false;
                label_bcr7.Visible = false;
            }
            else if (nCount == 5)
            {
                textBox_bcr1.Visible = true;
                label_bcr1.Visible = true;

                textBox_bcr2.Visible = true;
                label_bcr2.Visible = true;

                textBox_bcr3.Visible = true;
                label_bcr3.Visible = true;

                textBox_bcr4.Visible = true;
                label_bcr4.Visible = true;

                textBox_bcr5.Visible = true;
                label_bcr5.Visible = true;

                textBox_bcr6.Visible = false;
                label_bcr6.Visible = false;

                textBox_bcr7.Visible = false;
                label_bcr7.Visible = false;
            }
            else if (nCount == 6)
            {
                textBox_bcr1.Visible = true;
                label_bcr1.Visible = true;

                textBox_bcr2.Visible = true;
                label_bcr2.Visible = true;

                textBox_bcr3.Visible = true;
                label_bcr3.Visible = true;

                textBox_bcr4.Visible = true;
                label_bcr4.Visible = true;

                textBox_bcr5.Visible = true;
                label_bcr5.Visible = true;

                textBox_bcr6.Visible = true;
                label_bcr6.Visible = true;

                textBox_bcr7.Visible = false;
                label_bcr7.Visible = false;
            }
            else if (nCount == 7)
            {
                textBox_bcr1.Visible = true;
                label_bcr1.Visible = true;

                textBox_bcr2.Visible = true;
                label_bcr2.Visible = true;

                textBox_bcr3.Visible = true;
                label_bcr3.Visible = true;

                textBox_bcr4.Visible = true;
                label_bcr4.Visible = true;

                textBox_bcr5.Visible = true;
                label_bcr5.Visible = true;

                textBox_bcr6.Visible = true;
                label_bcr6.Visible = true;

                textBox_bcr7.Visible = true;
                label_bcr7.Visible = true;
            }

            textBox_bcr1.Focus();
        }

        private void textBox_bcrcount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        public void Fnc_Exit()
        {
            this.Dispose();
        }

        public void Fnc_Show()
        {
            try
            {          
                if(!bState)
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
            if(bState)
            {
                bState = false;
                Hide();
            }
        }

        private void textBox_bcr1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_bcr1.Text = textBox_bcr1.Text.ToUpper();

                if (textBox_bcr1.ImeMode != ImeMode.Alpha)
                {
                    textBox_bcr1.ImeMode = ImeMode.Alpha;
                }

                int nWfrcount = Fnc_Bcr_GetWafercount(textBox_bcr1.Text);

                if(nWfrcount == -1)
                {
                    MessageBox.Show("리스트에 없는 자재 입니다.");
                    textBox_bcr1.Text = "";
                    textBox_bcr1.Focus();
                    return;
                }
                else if(nWfrcount == 0)
                {
                    MessageBox.Show("전산에 Wfr 수량이 누락 되었습니다.");
                    textBox_bcr1.Text = "";
                    textBox_bcr1.Focus();
                    return;
                }
                else
                {
                    nBcrcount = nWfrcount;
                    textBox_bcrcount.Text = nBcrcount.ToString();
                    Thread.Sleep(100);
                }

                if (nBcrcount == 1)
                {
                    string strBcrSum = "";

                    strBcrSum = string.Format("{0}", textBox_bcr1.Text);

                    if (BankHost_main.nScanMode == 3)
                    {
                        BankHost_main.strScanData = strBcrSum;

                        BankHost_main.bGunRingMode_Run = true;

                        Fnc_Hide();
                    }
                }
                else
                    textBox_bcr2.Focus();
            }
        }

        private void textBox_bcr2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_bcr2.Text = textBox_bcr2.Text.ToUpper();

                if (textBox_bcr2.ImeMode != ImeMode.Alpha)
                {
                    textBox_bcr2.ImeMode = ImeMode.Alpha;
                }

                if (nBcrcount == 2)
                {
                    string strBcrSum = "";

                    strBcrSum = string.Format("{0},{1}", textBox_bcr1.Text, textBox_bcr2.Text);

                    if (BankHost_main.nScanMode == 3)
                    {
                        BankHost_main.strScanData = strBcrSum;

                        BankHost_main.bGunRingMode_Run = true;

                        Fnc_Hide();
                    }
                }
                else
                    textBox_bcr3.Focus();
            }
        }

        private void textBox_bcr3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_bcr3.Text = textBox_bcr3.Text.ToUpper();

                if (textBox_bcr3.ImeMode != ImeMode.Alpha)
                {
                    textBox_bcr3.ImeMode = ImeMode.Alpha;
                }

                if (nBcrcount == 3)
                {
                    string strBcrSum = "";

                    strBcrSum = string.Format("{0},{1},{2}", textBox_bcr1.Text, textBox_bcr2.Text, textBox_bcr3.Text);

                    if (BankHost_main.nScanMode == 3)
                    {
                        BankHost_main.strScanData = strBcrSum;

                        BankHost_main.bGunRingMode_Run = true;

                        Fnc_Hide();
                    }
                }
                else
                    textBox_bcr4.Focus();
            }
        }

        private void textBox_bcr4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_bcr4.Text = textBox_bcr4.Text.ToUpper();

                if (nBcrcount == 4)
                {
                    if (textBox_bcr4.ImeMode != ImeMode.Alpha)
                    {
                        textBox_bcr4.ImeMode = ImeMode.Alpha;
                    }

                    string strBcrSum = "";

                    strBcrSum = string.Format("{0},{1},{2},{3}", textBox_bcr1.Text, textBox_bcr2.Text, textBox_bcr3.Text, textBox_bcr4.Text);

                    if (BankHost_main.nScanMode == 3)
                    {
                        BankHost_main.strScanData = strBcrSum;

                        BankHost_main.bGunRingMode_Run = true;

                        Fnc_Hide();
                    }
                }
                else
                    textBox_bcr5.Focus();
            }
        }

        private void textBox_bcr5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_bcr5.Text = textBox_bcr5.Text.ToUpper();

                if (nBcrcount == 5)
                {
                    if (textBox_bcr5.ImeMode != ImeMode.Alpha)
                    {
                        textBox_bcr5.ImeMode = ImeMode.Alpha;
                    }

                    string strBcrSum = "";

                    strBcrSum = string.Format("{0},{1},{2},{3},{4}", textBox_bcr1.Text, textBox_bcr2.Text, textBox_bcr3.Text, textBox_bcr4.Text, textBox_bcr5.Text);

                    if (BankHost_main.nScanMode == 3)
                    {
                        BankHost_main.strScanData = strBcrSum;

                        BankHost_main.bGunRingMode_Run = true;

                        Fnc_Hide();
                    }
                }
                else
                    textBox_bcr6.Focus();
            }
        }

        private void textBox_bcr6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_bcr6.Text = textBox_bcr6.Text.ToUpper();

                if (textBox_bcr6.ImeMode != ImeMode.Alpha)
                {
                    textBox_bcr6.ImeMode = ImeMode.Alpha;
                }

                if (nBcrcount == 6)
                {
                    string strBcrSum = "";

                    strBcrSum = string.Format("{0},{1},{2},{3},{4},{5}", textBox_bcr1.Text, textBox_bcr2.Text, textBox_bcr3.Text, textBox_bcr4.Text, textBox_bcr5.Text, textBox_bcr6.Text);

                    if (BankHost_main.nScanMode == 3)
                    {
                        BankHost_main.strScanData = strBcrSum;

                        BankHost_main.bGunRingMode_Run = true;

                        Fnc_Hide();
                    }
                }
                else
                    textBox_bcr7.Focus();
            }
        }

        private void textBox_bcr7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                textBox_bcr7.Text = textBox_bcr7.Text.ToUpper();

                if (textBox_bcr7.ImeMode != ImeMode.Alpha)
                {
                    textBox_bcr7.ImeMode = ImeMode.Alpha;
                }

                if (nBcrcount == 7)
                {
                    string strBcrSum = "";

                    strBcrSum = string.Format("{0},{1},{2},{3},{4},{5},{6}", textBox_bcr1.Text, textBox_bcr2.Text, textBox_bcr3.Text, textBox_bcr4.Text, textBox_bcr5.Text, textBox_bcr6.Text, textBox_bcr7.Text);

                    if (BankHost_main.nScanMode == 3)
                    {
                        BankHost_main.strScanData = strBcrSum;

                        BankHost_main.bGunRingMode_Run = true;

                        Fnc_Hide();
                    }
                }
            }
        }

        private void Form_MutiBcrIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            Fnc_Hide();
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

        public int Fnc_Bcr_GetWafercount(string strBcr)
        {
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
            
            //1D Scan 인지 확인
            string strBcrType = BankHost_main.Host.Host_Get_BcrType(BankHost_main.strWork_Cust, BankHost_main.strWork_Model);          
            if (strBcrType == "CODE39" || strBcrType == "CODE128")
            {
                return -1;
            }

            string[] strSplit_Bcr = strBcr.Split(seperator);
            if (strSplit_Bcr.Length < 3)
                return -1;

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

            string FileName = strExcutionPath + "\\Work\\" + strWorkFile + "\\";
            string FilePath = FileName + "\\" + bcr.Device + "\\" + bcr.Device + ".txt";

            int nResult = Fnc_Getinfo(FilePath, bcr.Lot);

            return nResult;
        }
        public int Fnc_Getinfo(string strfilepath, string strlot)
        {
            string[] info = Fnc_ReadFile(strfilepath);

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
                st.Rcv_WQty = strSplit_data[16];

                if (strlot == st.Lot)
                {
                    int nReturn = Int32.Parse(st.Rcv_WQty);

                    return nReturn;
                }

            }

            return -1;
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
    }
}
