using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Bank_Host
{
    public partial class Form1_Split_email : Form
    {
        SqlConnection conn = null;
        SqlCommand scom = new SqlCommand();
        SqlDataReader sdata = null;
        private string sqlConnectionString;

        public Form1_Split_email()
        {
            string ConnectionString = string.Format("server=10.135.200.35;database=GR_Automation;user id=amm;password=amm@123");
            sqlConnectionString = ConnectionString;
            conn = new SqlConnection(ConnectionString);

            scom.Connection = conn;

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

        private void Form1_Split_email_Load(object sender, EventArgs e)
        {
            List<string> temp = search_data("select LINE_CODE from TB_SPLIT_EMAIL");

            for(int i = 0; i < temp.Count;i++)
            {
                if(cb_linecode.Items.Contains(temp[i]) == false)
                    cb_linecode.Items.Add(temp[i]);
            }
        }

        private void cb_linecode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] temp = search_data(string.Format("select MAIL_ID from TB_SPLIT_EMAIL where LINE_CODE='{0}'",cb_linecode.Text))[0].Split(';');

            dgv_mail.Rows.Clear();

            for (int i = 0; i < temp.Length; i++)
            {
                if(temp[i] != "")
                    dgv_mail.Rows.Add(temp[i]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string temp = "";

            for(int i = 0; i< dgv_mail.RowCount -1; i++)
            {
                temp += dgv_mail.Rows[i].Cells[0].Value.ToString() + ";";
            }

            int cnt = run_count(string.Format("select count(*) from TB_SPLIT_EMAIL with(nolock) where LINE_CODE={0}", cb_linecode.Text));

            if(cnt == 0)
            {
                if(MessageBox.Show("추가하시겠습니까?", "추가", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    run_sql_command(string.Format("insert into TB_SPLIT_EMAIL(LINE_CODE,MAIL_ID) values('{0}', '{1}')", cb_linecode.Text, temp));
                }
            }
            else
            {
                run_sql_command(string.Format("update TB_SPLIT_EMAIL set MAIL_ID = '{0}' where LINE_CODE='{1}'", temp, cb_linecode.Text));
            }
           

        }

        public void run_sql_command(string sql)
        {
            try
            {
                lock (this)
                {                    
                    open_conn();

                    scom.CommandType = System.Data.CommandType.Text;
                    scom.CommandText = sql;
                    scom.ExecuteReader();

                    conn.Close();                   
                }
                
            }
            catch (Exception ex)
            {   
                throw;
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
                lock (this)
                {
                    open_conn();

                    scom.CommandType = System.Data.CommandType.Text;
                    scom.CommandText = sql_str;
                    res = (int)scom.ExecuteScalar();

                    conn.Close();
                    
                    return res;
                }
            }
            catch (Exception ex)
            {                
            }

            return res;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            cb_linecode.Text = "";
            dgv_mail.Rows.Clear();
        }
    }
}
