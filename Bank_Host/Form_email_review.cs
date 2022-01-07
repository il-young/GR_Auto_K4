using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Data.SqlClient;

namespace Bank_Host
{
    public partial class Form_email_review : Form
    {
        SqlConnection conn = null;
        SqlCommand scom = new SqlCommand();
        SqlDataReader sdata = null;
        private string sqlConnectionString;
        List<string> dgv_val = new List<string>();


        public Form_email_review(string dgv_data)
        {
            string ConnectionString = string.Format("server=10.135.200.35;database=GR_Automation;user id=amm;password=amm@123");
            sqlConnectionString = ConnectionString;
            conn = new SqlConnection(ConnectionString);
            scom.Connection = conn;

            string[] temp = dgv_data.Split(';');

            for (int i = 0; i < temp.Length; i++)
                dgv_val.Add(temp[i]);

            InitializeComponent();
        }

        public List<string> search_data(string sql)
        {
            List<string> res = new List<string>();
            SqlDataReader dd;

            string ConnectionString = string.Format("server=10.135.200.35;database=GR_Automation;user id=amm;password=amm@123");

            SqlConnection ssconn = new SqlConnection(ConnectionString);
            SqlCommand scmd = new SqlCommand(sql, ssconn);
            string temp = "";

            try
            {
                lock (this)
                {
                    ssconn.Open();
                    scmd.CommandType = System.Data.CommandType.Text;
                    //scom.CommandText = sql;

                    if (ssconn.State == ConnectionState.Closed)
                        ssconn.Open();

                    dd = scmd.ExecuteReader();

                    while (dd.Read())
                    {
                        temp = "";


                        if (sql.Contains("select AGV_NAME from TBL_AGV_STATUS_LIST with(NOLOCK)") == true)
                        {
                            temp += dd[0].ToString();
                        }
                        else if (sql.Contains("TBL_AGV_STATUS_LIST") == true && sql.Contains("CURRENT_KEY") == false)
                        {
                            for (int i = 0; i < dd.FieldCount; i++)
                            {
                                if (i == 0)
                                {
                                    temp += dd.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss.fff") + ";";
                                }
                                else
                                {
                                    temp += dd[i].ToString() + ((i == dd.FieldCount - 1) ? "" : ";");
                                }
                            }
                        }
                        else if (sql.Contains("CURRENT_KEY") == true)
                        {
                            temp = dd[0].ToString();
                        }
                        else
                        {
                            for (int i = 0; i < dd.FieldCount; i++)
                            {
                                temp += dd[i].ToString() + ((i == dd.FieldCount - 1) ? "" : ";");
                            }
                        }
                        res.Add(temp);
                    }

                    dd.Close();
                    ssconn.Dispose();
                    scmd.Dispose();

                    return res;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                ssconn.Dispose();
                scmd.Dispose();
            }
        }
        private void open_conn()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }

        public void CloseConn()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();

            if (sdata != null)
                sdata.Close();
        }


        private void AddColmum()
        {
            dgv_splitlog_err_data.Rows.Clear();
            dgv_splitlog_err_data.Columns.Clear();

            DataGridViewColumn col = new DataGridViewColumn();
            DataGridViewCheckBoxCell dataGridViewCell = new  DataGridViewCheckBoxCell(false);
            col.CellTemplate = dataGridViewCell;
            col.HeaderText = "Check";

            dgv_splitlog_err_data.Columns.Add(col);
            dgv_splitlog_err_data.Columns.Add("No", "No.");
            dgv_splitlog_err_data.Columns.Add("Line", "Line");
            dgv_splitlog_err_data.Columns.Add("Cust", "Cust");
            dgv_splitlog_err_data.Columns.Add("Biunding", "Biunding#");
            dgv_splitlog_err_data.Columns.Add("Device", "Device#");
            dgv_splitlog_err_data.Columns.Add("Cust_Lot", "Cust Lot#");
            dgv_splitlog_err_data.Columns.Add("Dcc", "Dcc");
            dgv_splitlog_err_data.Columns.Add("Return_Qty", "Return Qty");
            dgv_splitlog_err_data.Columns.Add("Return_Wafer", "Return Wafer");
            dgv_splitlog_err_data.Columns.Add("Return_Date", "Return Date");
            dgv_splitlog_err_data.Columns.Add("Loc", "Loc");
            dgv_splitlog_err_data.Columns.Add("Status", "Status");            

            dgv_splitlog_err_data.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_splitlog_err_data.Columns[11].SortMode = DataGridViewColumnSortMode.NotSortable;
        }


        private void Form_email_review_Load(object sender, EventArgs e)
        {
            List<string> line_code = search_data("select LINE_CODE from TB_SPLIT_EMAIL");

            if (line_code.Count == 0)
            {
                cb_linecode.Text = "데이터가 없습니다.";
                tb_maillist.Text = "데이터가 없습니다.";
                return;
            }

            for (int i = 0; i < line_code.Count; i++)
                cb_linecode.Items.Add(line_code[i]);
                       
            AddColmum();

            string[] temp = new string[12];
            for (int i = 0; i < dgv_val.Count; i++)
            {
                Array.Copy(dgv_val[i].Split(','),0,temp,1, 11);
                temp[0] = "false";
                dgv_splitlog_err_data.Rows.Add(temp);
            }

            dgv_splitlog_err_data.AutoResizeColumns();

            SetMSG();
        }

        private void SetMSG()
        {
            if (Properties.Settings.Default.SplitLogMailHead != "")
                tb_head.Text = Properties.Settings.Default.SplitLogMailHead;

            if (Properties.Settings.Default.SplitLogMailBody != "")
                rtb_body.Text = Properties.Settings.Default.SplitLogMailBody;

            if (Properties.Settings.Default.SplitLogMailTail != "")
                rtb_tail.Text = Properties.Settings.Default.SplitLogMailTail;

            rtb_tail.Text += Environment.NewLine + BankHost_main.strOperator + " 드림";
        }

        private void cb_linecode_SelectedIndexChanged(object sender, EventArgs e)
        {
            tb_maillist.Text = search_data(string.Format("select MAIL_ID from TB_SPLIT_EMAIL where LINE_CODE='{0}'",cb_linecode.Text))[0];
            CheckAtLinecode(cb_linecode.Text);
        }

        private void CheckAtLinecode(string linecode)
        {
            for(int i = 0; i < dgv_splitlog_err_data.RowCount; i++)
            {
                if (dgv_splitlog_err_data.Rows[i].Cells[2].Value.ToString() == linecode)
                {
                    dgv_splitlog_err_data.Rows[i].Cells[0].Value = true;
                    dgv_splitlog_err_data.Rows[i].Selected = true;
                }
                else
                {
                    dgv_splitlog_err_data.Rows[i].Cells[0].Value = false;
                    dgv_splitlog_err_data.Rows[i].Selected = false;
                }
            }
        }

        private void btn_sendmail_Click(object sender, EventArgs e)
        {
            if(DialogResult.Yes == MessageBox.Show("메일을 보내시겠습니까?", "Send Mail", MessageBoxButtons.YesNo))
            {
                if (CheckMSG() == true)
                {
                    Fnc_SendEmail();
                    SaveMailMSG();
                }
            }            
        }

        private bool CheckMSG()
        {
            bool res = true;

            if (cb_linecode.Text == "")
            {
                res = false;
                MessageBox.Show("라인 코드가 선택 되지 않았습니다.", "Linecode Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }

            if(CountDGVChecked() == 0)
            {
                res = false;
                MessageBox.Show("선택된 Lot가 없습니다.", "Check Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if(cb_msg.Text =="")
            {
                res = false;
                MessageBox.Show("선택된 메시지가 없습니다.", "Message Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return res;
        }

        private int CountDGVChecked()
        {
            int cnt = 0;

            for(int i = 0; i < dgv_splitlog_err_data.RowCount; i++)
            {
                if (dgv_splitlog_err_data.Rows[i].Cells[0].Value.ToString() == "True")
                    cnt++;
            }
            
            return cnt;
        }

        private void SaveMailMSG()
        {
            Properties.Settings.Default.SplitLogMailHead = tb_head.Text;
            Properties.Settings.Default.SplitLogMailBody = rtb_body.Text;

            string[] tail = rtb_tail.Text.Split('\n');
            string res_tail = "";

            for(int i = 0; i < tail.Length; i++)
            {
                if (tail[i].Contains(BankHost_main.strOperator) == false)
                    res_tail += tail[i] + Environment.NewLine;
            }

            res_tail = res_tail.Remove(res_tail.Length - 1, 1);
            Properties.Settings.Default.SplitLogMailTail = res_tail;

            Properties.Settings.Default.Save();
        }

        private string MakeErrLotString()
        {
            string res = "";
            int count = 1;
            string temp = "";

            res = "No" + "\t" + dgv_splitlog_err_data.Columns[2].HeaderText + "\t" + dgv_splitlog_err_data.Columns[3].HeaderText + "\t" + dgv_splitlog_err_data.Columns[4].HeaderText + "\t"
                 + dgv_splitlog_err_data.Columns[5].HeaderText + "\t" + dgv_splitlog_err_data.Columns[6].HeaderText + "\t" + dgv_splitlog_err_data.Columns[7].HeaderText + "\t"
                  + dgv_splitlog_err_data.Columns[8].HeaderText + "\t" + dgv_splitlog_err_data.Columns[9].HeaderText + "\t" + dgv_splitlog_err_data.Columns[10].HeaderText + "\t"
                   + dgv_splitlog_err_data.Columns[11].HeaderText + Environment.NewLine;

            for (int i = 0; i < dgv_splitlog_err_data.RowCount; i++)
            {
                temp = dgv_splitlog_err_data.Rows[i].Cells[0].Value.ToString();
                if (temp == "True")
                {
                    res += count++.ToString() + "\t";                    
                    res += dgv_splitlog_err_data.Rows[i].Cells[2].Value != null ? dgv_splitlog_err_data.Rows[i].Cells[2].Value.ToString() + "\t" : string.Empty + "\t";
                    res += dgv_splitlog_err_data.Rows[i].Cells[3].Value != null ? dgv_splitlog_err_data.Rows[i].Cells[3].Value.ToString() + "\t" : string.Empty + "\t";
                    res += dgv_splitlog_err_data.Rows[i].Cells[4].Value != null ? dgv_splitlog_err_data.Rows[i].Cells[4].Value.ToString() + "\t" : string.Empty + "\t";
                    res += dgv_splitlog_err_data.Rows[i].Cells[5].Value != null ? dgv_splitlog_err_data.Rows[i].Cells[5].Value.ToString() + "\t" : string.Empty + "\t";
                    res += dgv_splitlog_err_data.Rows[i].Cells[6].Value != null ? dgv_splitlog_err_data.Rows[i].Cells[6].Value.ToString() + "\t" : string.Empty + "\t";
                    res += dgv_splitlog_err_data.Rows[i].Cells[7].Value != null ? dgv_splitlog_err_data.Rows[i].Cells[7].Value.ToString() + "\t" : string.Empty + "\t";
                    res += dgv_splitlog_err_data.Rows[i].Cells[8].Value != null ? dgv_splitlog_err_data.Rows[i].Cells[8].Value.ToString() + "\t" : string.Empty + "\t";
                    res += dgv_splitlog_err_data.Rows[i].Cells[9].Value != null ? dgv_splitlog_err_data.Rows[i].Cells[9].Value.ToString() + "\t" : string.Empty + "\t";
                    res += dgv_splitlog_err_data.Rows[i].Cells[10].Value != null ? dgv_splitlog_err_data.Rows[i].Cells[10].Value.ToString() + "\t" : string.Empty + "\t";
                    res += dgv_splitlog_err_data.Rows[i].Cells[11].Value != null ? dgv_splitlog_err_data.Rows[i].Cells[11].Value.ToString() + Environment.NewLine : string.Empty + Environment.NewLine;
                }                    
            }

            return res;
        }

        public void Fnc_SendEmail()
        {
            try
            {
                string lots = MakeErrLotString();
                string[] strSplit_address = tb_maillist.Text.Split(';');
                int ncount = strSplit_address.Length;

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

                for (int n = 0; n < ncount; n++)
                {
                    message.To.Add(strSplit_address[n]);
                }

                message.Subject = tb_head.Text;
                message.From = new System.Net.Mail.MailAddress("K4BANK@amkor.co.kr");

                //message.IsBodyHtml = true;
                //message.Attachments.Add(new System.Net.Mail.Attachment(err_local_img_path));
                //System.Net.Mail.LinkedResource linkedResource = new System.Net.Mail.LinkedResource(err_local_img_path);
                //linkedResource.ContentId = "MyPic";
                //System.Net.Mail.AlternateView view = System.Net.Mail.AlternateView.CreateAlternateViewFromString(string.Format("<pre>{0} 아래의 그림은 오류 발생한 나르미의 위치를 표시 합니다. <img src=cid:MyPic>", strMessage), null, "text/html");
                //view.LinkedResources.Add(linkedResource);
                //message.AlternateViews.Add(view);

                message.Body = rtb_body.Text + Environment.NewLine + cb_msg.Text + Environment.NewLine + rtb_tail.Text + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + lots;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("10.101.10.6");
                smtp.Credentials = new System.Net.NetworkCredential("", "");
                smtp.Port = 25;
                smtp.Send(message);

                message.Dispose();
                //linkedResource.Dispose();
                //view.Dispose();

                ///////////////////////////////
                ///
            }
            catch (Exception ex)
            {

            }
        }
    }
}
