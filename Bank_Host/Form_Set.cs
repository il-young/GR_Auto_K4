using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Bank_Host
{
    public partial class Form_Set : Form
    {
        public Form_Set()
        {
            InitializeComponent();
        }

        public void Fnc_Init()
        {
            comboBox_startup.SelectedIndex = BankHost_main.nStartup;
            if (Form_Sort.bPrintUse)
                comboBox_printuse.SelectedIndex = 0;
            else
                comboBox_printuse.SelectedIndex = 1;

            comboBox_auto.SelectedIndex = BankHost_main.nInputMode;

            textBox_printname.Text = Form_Sort.strPrintName;
            textBox_maxpack.Text = BankHost_main.nMaxpack.ToString();
            textBox_eqid.Text = BankHost_main.strEqid;

            Fnc_Update_PrintType();
            Fnc_Update_BcrInfo();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if(textBox_sid.Text != "" && textBox_name.Text != "" && textBox_grade.Text != "")
                BankHost_main.SQL_SetUserDB(textBox_sid.Text, textBox_name.Text, textBox_grade.Text);

            Fnc_UserAllView();
        }

        public void Fnc_UserAllView()
        {
            var dt_list = BankHost_main.SQL_GetAllUser();

            dataGridView_List.Columns.Clear();
            dataGridView_List.Rows.Clear();
            dataGridView_List.Refresh();

            dataGridView_List.Columns.Add("#", "#");
            dataGridView_List.Columns.Add("사번", "사번");
            dataGridView_List.Columns.Add("이름", "이름");

            if (dt_list.Rows.Count == 0)
                return;

            int nCnt = 1;
            for(int n = 0; n < dt_list.Rows.Count; n++)
            {
                string strsid = dt_list.Rows[n]["ID"].ToString();   strsid = strsid.Trim();
                string strname = dt_list.Rows[n]["NAME"].ToString(); strname = strname.Trim();

                dataGridView_List.Rows.Add(new object[3] { nCnt, strsid, strname });
                nCnt++;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int nCount = dataGridView_List.RowCount;

            if (nCount < 1)
                return;

            int nIndex = dataGridView_List.CurrentCell.RowIndex;


            string sid = dataGridView_List.Rows[nIndex].Cells[1].Value.ToString();

            BankHost_main.SQL_DelUserDB(sid);

            Fnc_UserAllView();
        }

        private void button_allsave_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("저장 하시겠습니끼?", "저장", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.No)
            {
                return;
            }

            Fnc_Update_Config();
        }

        public void Fnc_Update_Config()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration
                    (ConfigurationUserLevel.None);

            int nIndex = comboBox_startup.SelectedIndex;
            int nIndex2 = comboBox_printuse.SelectedIndex;
            int nIndex3 = comboBox_auto.SelectedIndex;

            if (nIndex != -1)
            {
                config.AppSettings.Settings.Remove("Startup");
                config.AppSettings.Settings.Add("Startup", nIndex.ToString());
            }

            if (nIndex2 != -1)
            {
                config.AppSettings.Settings.Remove("Print_Use");
                config.AppSettings.Settings.Add("Print_Use", nIndex2.ToString());
            }

            if(nIndex3 != -1)
            {
                config.AppSettings.Settings.Remove("AUTO");
                config.AppSettings.Settings.Add("AUTO", nIndex3.ToString());
            }

            config.AppSettings.Settings.Remove("Print_Name");
            config.AppSettings.Settings.Add("Print_Name", textBox_printname.Text);

            config.AppSettings.Settings.Remove("MAX_PACK");
            config.AppSettings.Settings.Add("MAX_PACK", textBox_maxpack.Text);

            config.AppSettings.Settings.Remove("EQID");
            config.AppSettings.Settings.Add("EQID", textBox_eqid.Text);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            Properties.Settings.Default.TimeOutMin = int.Parse(tb_TimeOutMin.Text);
            Properties.Settings.Default.Save();

            BankHost_main.nStartup = nIndex;
        }

        private void button_savetype_Click(object sender, EventArgs e)
        {
            int n = comboBox_custType.SelectedIndex;

            if (textBox_custno.Text == "" || n < 0)
            {
                MessageBox.Show("정보를 입력 하여 주십시오");
                return;
            }

            BankHost_main.Host.Host_Set_PrintType(textBox_custno.Text, (n + 1).ToString());
            Fnc_Update_PrintType();

        }

        public void Fnc_Update_PrintType()
        {
            var dt_list = BankHost_main.Host.Host_Get_PrintAllType();

            dataGridView_custType.Columns.Clear();
            dataGridView_custType.Rows.Clear();
            dataGridView_custType.Refresh();

            dataGridView_custType.Columns.Add("#", "#");
            dataGridView_custType.Columns.Add("CUST_NO", "CUST_NO");
            dataGridView_custType.Columns.Add("TYPE", "TYPE");

            if (dt_list.Rows.Count == 0)
                return;

            int nCnt = 1;
            for (int n = 0; n < dt_list.Rows.Count; n++)
            {
                string strCust = dt_list.Rows[n]["PRINT_CUST"].ToString(); strCust = strCust.Trim();
                string strType = dt_list.Rows[n]["PRINT_TYPE"].ToString(); strType = strType.Trim();

                dataGridView_custType.Rows.Add(new object[3] { nCnt, strCust, strType });
                nCnt++;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int nCount = dataGridView_custType.RowCount;

            if (nCount < 1)
                return;

            int nIndex = dataGridView_custType.CurrentCell.RowIndex;


            string strCust = dataGridView_custType.Rows[nIndex].Cells[1].Value.ToString();

            BankHost_main.Host.Host_Delete_PrintType(strCust);

            Fnc_Update_PrintType();
        }

        private void button_savebcr_Click(object sender, EventArgs e)
        {
            if (textBox_bcr1.Text == "" || textBox_bcr3.Text == "" || textBox_bcr4.Text == "" || textBox_bcr5.Text ==" " || textBox_bcr12.Text == "" ||
                textBox_bcr6.Text == "" || textBox_bcr7.Text == "" || textBox_bcr8.Text == "" || textBox_bcr10.Text == "" || textBox_bcr11.Text == "" )
            {
                MessageBox.Show("정보를 입력 하여 주십시오");
                return;
            }

            BankHost_main.Host.Host_Set_BCRFormat(textBox_bcr1.Text, textBox_bcr12.Text, textBox_bcr3.Text, textBox_bcr4.Text, textBox_bcr5.Text,
                textBox_bcr6.Text, textBox_bcr7.Text, textBox_bcr8.Text, textBox_bcr9.Text, textBox_bcr10.Text, textBox_bcr11.Text, textBox_bcr13.Text, textBox_bcr14.Text, textBox_bcr15.Text, textBox_bcr16.Text);

            Fnc_Update_BcrInfo();
        }

        public void Fnc_Update_BcrInfo()
        {
            var dt_list = BankHost_main.Host.Host_Get_BCRFormat();

            dataGridView_bcrconfig.Columns.Clear();
            dataGridView_bcrconfig.Rows.Clear();
            dataGridView_bcrconfig.Refresh();

            dataGridView_bcrconfig.Columns.Add("#", "#");
            dataGridView_bcrconfig.Columns.Add("CUST_NO", "CUST_NO");
            dataGridView_bcrconfig.Columns.Add("MULTILOT", "MULTILOT");
            dataGridView_bcrconfig.Columns.Add("BANK_NO", "BANK_NO");
            dataGridView_bcrconfig.Columns.Add("BCR_TYPE", "BCR_TYPE");
            dataGridView_bcrconfig.Columns.Add("BCR_CNT", "BCR_CNT");
            dataGridView_bcrconfig.Columns.Add("BCR_NAME", "BCR_NAME");
            dataGridView_bcrconfig.Columns.Add("DEVICE", "DEVICE");
            dataGridView_bcrconfig.Columns.Add("LOTID", "LOTID");
            dataGridView_bcrconfig.Columns.Add("LOT_DIGIT", "LOT_DIGIT");
            dataGridView_bcrconfig.Columns.Add("DIEQTY", "DIEQTY");
            dataGridView_bcrconfig.Columns.Add("SPR", "SPR");
            dataGridView_bcrconfig.Columns.Add("GR 방식", "GR 방식");
            dataGridView_bcrconfig.Columns.Add("UDIGIT", "UDIGIT");
            dataGridView_bcrconfig.Columns.Add("WFRQTY", "WFRQTY");
            dataGridView_bcrconfig.Columns.Add("MTL_TYPE", "MTL_TYPE");

            if (dt_list.Rows.Count == 0)
                return;

            int nCnt = 1;
            for (int n = 0; n < dt_list.Rows.Count; n++)
            {
                string strCust = dt_list.Rows[n]["CUST"].ToString(); strCust = strCust.Trim();
                string strMulti = dt_list.Rows[n]["MULTI_LOT"].ToString(); strMulti = strMulti.Trim();
                string strBank = dt_list.Rows[n]["BANK_NO"].ToString(); strBank = strBank.Trim();
                string strBcrType = dt_list.Rows[n]["BCR_TYPE"].ToString(); strBcrType = strBcrType.Trim();
                string strBcrCount = dt_list.Rows[n]["BCR_CNT"].ToString(); strBcrCount = strBcrCount.Trim();
                string strBcrName = dt_list.Rows[n]["NAME"].ToString(); strBcrName = strBcrName.Trim();
                string strDevice = dt_list.Rows[n]["DEVICE"].ToString(); strDevice = strDevice.Trim();
                string strLotid = dt_list.Rows[n]["LOTID"].ToString(); strLotid = strLotid.Trim();
                string strLotdigit = dt_list.Rows[n]["LOT_DIGIT"].ToString(); strLotdigit = strLotdigit.Trim();
                string strdieqty = dt_list.Rows[n]["WFR_QTY"].ToString(); strdieqty = strdieqty.Trim();
                string strSpr = dt_list.Rows[n]["SPR"].ToString(); strdieqty = strdieqty.Trim();
                string strGrmethod = dt_list.Rows[n]["GR_METHOD"].ToString(); strGrmethod = strGrmethod.Trim();
                string strUdigit = dt_list.Rows[n]["UDIGIT"].ToString(); strUdigit = strUdigit.Trim();
                string strWfrqty = dt_list.Rows[n]["TTL_WFR_QTY"].ToString(); strWfrqty = strWfrqty.Trim();
                string strMtlType = dt_list.Rows[n]["MTL_TYPE"].ToString(); strMtlType = strMtlType.Trim();

                dataGridView_bcrconfig.Rows.Add(new object[16] { nCnt, strCust, strMulti, strBank , strBcrType , strBcrCount ,
                                                                strBcrName, strDevice, strLotid, strLotdigit, strdieqty, strSpr,strGrmethod, strUdigit, strWfrqty, strMtlType});
                nCnt++;
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int nCount = dataGridView_bcrconfig.RowCount;

            if (nCount < 1)
                return;

            int nIndex = dataGridView_bcrconfig.CurrentCell.RowIndex;


            string strCust = dataGridView_bcrconfig.Rows[nIndex].Cells[1].Value.ToString();
            string strName = dataGridView_bcrconfig.Rows[nIndex].Cells[6].Value.ToString();

            BankHost_main.Host.Host_Delete_BCRFormat(strCust, strName);

            Fnc_Update_BcrInfo();
        }

        private void dataGridView_bcrconfig_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;

            if (colIndex != 0)
                colIndex = 0;

            if (rowIndex == -1)
                return;

            textBox_bcr1.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[1].Value.ToString();
            textBox_bcr12.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[2].Value.ToString();
            textBox_bcr3.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[3].Value.ToString();
            textBox_bcr4.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[4].Value.ToString();
            textBox_bcr5.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[5].Value.ToString();
            textBox_bcr6.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[6].Value.ToString();
            textBox_bcr7.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[7].Value.ToString();
            textBox_bcr8.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[8].Value.ToString();
            textBox_bcr9.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[9].Value.ToString();
            textBox_bcr10.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[10].Value.ToString();
            textBox_bcr11.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[11].Value.ToString();
            textBox_bcr13.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[12].Value.ToString();
            textBox_bcr14.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[13].Value.ToString();
            textBox_bcr15.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[14].Value.ToString();
            textBox_bcr16.Text = dataGridView_bcrconfig.Rows[rowIndex].Cells[15].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form_MesPWChange mespw = new Form_MesPWChange();
            mespw.ShowDialog();
        }

        private void Form_Set_Load(object sender, EventArgs e)
        {
            tb_TimeOutMin.Text = Properties.Settings.Default.TimeOutMin.ToString();          
        }

        private void tb_TimeOutMin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                Properties.Settings.Default.TimeOutMin = int.Parse(tb_TimeOutMin.Text);
                Properties.Settings.Default.Save();
            }
        }
    }
}
