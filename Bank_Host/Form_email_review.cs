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

            richTextBox1.Text += Environment.NewLine +  BankHost_main.strOperator + " 드림";
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
    }
}
