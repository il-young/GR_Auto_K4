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
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Data.SqlClient;
using System.Net;
using System.IO;

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
            //var dt_list = BankHost_main.Host.Host_Get_BCRFormat();

            List<Dictionary<string, string>> cust = WAS2CUST(GetWebServiceData($"http://10.131.10.84:8080/api/diebank/bcr-master/k4/json"));

            dataGridView_bcrconfig.Columns.Clear();
            dataGridView_bcrconfig.Rows.Clear();
            dataGridView_bcrconfig.Refresh();

            dataGridView_bcrconfig.Columns.Add("#", "#");
            dataGridView_bcrconfig.Columns.Add("CUST_CODE", "CUST_CODE");
            dataGridView_bcrconfig.Columns.Add("CUST_NAME", "CUST_NAME");
            dataGridView_bcrconfig.Columns.Add("BCR_TYPE", "BCR_TYPE");
            dataGridView_bcrconfig.Columns.Add("SPLITER", "SPLITER");//o
            dataGridView_bcrconfig.Columns.Add("USE", "USE");//x
            dataGridView_bcrconfig.Columns.Add("BCD01", "BCD01");//
            dataGridView_bcrconfig.Columns.Add("BCD02", "BCD02");//
            dataGridView_bcrconfig.Columns.Add("BCD03", "BCD03");//
            dataGridView_bcrconfig.Columns.Add("BCD04", "BCD04");//
            dataGridView_bcrconfig.Columns.Add("BCD05", "BCD05");//
            dataGridView_bcrconfig.Columns.Add("BCD06", "BCD06");//
            dataGridView_bcrconfig.Columns.Add("BCD07", "BCD07");//
            dataGridView_bcrconfig.Columns.Add("BCD08", "BCD08");//
            dataGridView_bcrconfig.Columns.Add("BCD09", "BCD09");//
            dataGridView_bcrconfig.Columns.Add("BCD10", "BCD10");//
            dataGridView_bcrconfig.Columns.Add("BCD11", "BCD11");//
            dataGridView_bcrconfig.Columns.Add("BCD12", "BCD12");//
            dataGridView_bcrconfig.Columns.Add("REGISTER", "REGISTER");
            dataGridView_bcrconfig.Columns.Add("REG_TIME", "REG_TIME");
            dataGridView_bcrconfig.Columns.Add("EDITOR", "EDITOR");
            dataGridView_bcrconfig.Columns.Add("EDIT_TIME", "EDIT_TIME");
            dataGridView_bcrconfig.Columns.Add("REMARK", "REMARK");
            dataGridView_bcrconfig.Columns.Add("ROW_NUM", "ROW_NUM");
            dataGridView_bcrconfig.Columns.Add("UDIGIT", "UDIGIT");
            dataGridView_bcrconfig.Columns.Add("RESULT", "RESULT");
            dataGridView_bcrconfig.Columns.Add("MESSAGE", "MESSAGE");

            if (cust.Count == 0)
                return;

            for (int n = 0; n < cust.Count; n++)
            {
                dataGridView_bcrconfig.Rows.Add(new object[] { n + 1,
                    cust[n]["CUST_CODE"].ToString().Trim(),
                    cust[n]["CUST_NAME"].ToString().Trim(),
                    cust[n]["BCR_TYPE"].ToString().Trim(),
                    cust[n]["SPLITER"].ToString().Trim(),
                    cust[n]["USE"].ToString().Trim(),
                    cust[n]["BCD01"].ToString().Trim(),
                    cust[n]["BCD02"].ToString().Trim(),
                    cust[n]["BCD03"].ToString().Trim(),
                    cust[n]["BCD04"].ToString().Trim(),
                    cust[n]["BCD05"].ToString().Trim(),
                    cust[n]["BCD06"].ToString().Trim(),
                    cust[n]["BCD07"].ToString().Trim(),
                    cust[n]["BCD08"].ToString().Trim(),
                    cust[n]["BCD09"].ToString().Trim(),
                    cust[n]["BCD10"].ToString().Trim(),
                    cust[n]["BCD11"].ToString().Trim(),
                    cust[n]["BCD12"].ToString().Trim(),
                    cust[n]["REGISTER"].ToString().Trim(),
                    cust[n]["REG_TIME"].ToString().Trim(),
                    cust[n]["EDITOR"].ToString().Trim(),
                    cust[n]["EDIT_TIME"].ToString().Trim(),
                    cust[n]["REMARK"].ToString().Trim(),
                    cust[n]["RESULT"].ToString().Trim(),
                    cust[n]["MESSAGE"].ToString().Trim()});
            }
        }

        public static string GetWebServiceData(string url)
        {
            string responseText = string.Empty;

            try
            {
                byte[] arr = new byte[10];

                //new frm_InboundMain().SaveLog("WLOG", "INFO","GET : " + url);

                

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 2000;
                request.Method = "GET";
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(Properties.Settings.Default.USER_NAME + ":" + Properties.Settings.Default.USER_PW)));

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        responseText = reader.ReadToEnd();
                        // do something with the response data
                    }
                }

                

                return responseText;
            }
            catch (WebException ex)
            {
                string errorMessage = string.Empty;

                

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
            if(e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                Properties.Settings.Default.TimeOutMin = int.Parse(tb_TimeOutMin.Text);
                Properties.Settings.Default.Save();
            }
        }

        private ChromeDriverService _driverService = null;
        private ChromeOptions _options = null;
        private ChromeDriver _driver = null;


        private void button2_Click(object sender, EventArgs e)
        {
            _driverService = ChromeDriverService.CreateDefaultService();
            _driverService.HideCommandPromptWindow = true;

            _options = new ChromeOptions();
            _options.AddArgument("disable-gpu");

            _options.AddArgument("headless");                
            _options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);

            _driver = new ChromeDriver(_driverService, _options);
            _driver.Navigate().GoToUrl("http://aak1ws01/eMES/index.jsp");  // 웹 사이트에 접속합니다. 
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[3]/td[2]/p/font/span/input")).SendKeys(BankHost_main.strMESID);    // ID 입력          
            _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[4]/td[2]/p/font/span/input")).SendKeys(BankHost_main.strMESPW);   // PW 입력            
            _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[5]/td[2]/font/span/input")).SendKeys(BankHost_main.strID);   // 사번 입력         
            _driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[3]/td/table/tbody/tr[6]/td/p/input")).Click();   // Main 로그인 버튼            

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

            _driver.Navigate().GoToUrl("http://aak1ws01/eMES/commons/custlist_popup.jsp");   // Scrap request 항목으로 이동

            System.Collections.ObjectModel.ReadOnlyCollection<OpenQA.Selenium.IWebElement> cust_combo = _driver.FindElements(By.Name("selCustList"));

            string[] cust = cust_combo[0].Text.Replace('\r', ' ').Split('\n');
            string q = "";

            run_sql_command("delete from TB_SCRAP_CUST");

            foreach(string name in cust)
            {
                q = string.Format("insert into TB_SCRAP_CUST values({0}, '{1}')", name.Split(':')[0].Trim(), name.Split(':')[1].Trim());
                run_sql_command(q);
            }

            MessageBox.Show("고객코드 가져오기가 완료 되었습니다.");
        }

        public void run_sql_command(string sql)
        {
            try
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
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private void btn_WSN_Click(object sender, EventArgs e)
        {
            using (Form_addWSNDev wSNDev = new Form_addWSNDev())
            {
                wSNDev.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form_ReaderSetting setting = new Form_ReaderSetting();

            setting.ShowDialog();
        }

        private void btn_CustVisible_Click(object sender, EventArgs e)
        {
            Form_CustNameUse custNameUse = new Form_CustNameUse();

            custNameUse.ShowDialog();
        }
    }
}
