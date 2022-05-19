using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Host
{
    public partial class Form_ScrapComment : Form
    {
        public Form_ScrapComment()
        {
            InitializeComponent();
        }

        DataSet table = new DataSet();

        private void Form_ScrapComment_Load(object sender, EventArgs e)
        {
            ReadData();
        }


        private void ReadData()
        {
            table = SearchData("select * from TB_SCRAP_COMMENT with(NOLOCK)");

            dgv_Comment.Rows.Clear();

            for (int i = 0; i < table.Tables[0].Rows.Count; i++)
            {
                dgv_Comment.Rows.Add(false, table.Tables[0].Rows[i][2].ToString(), table.Tables[0].Rows[i][0].ToString(), table.Tables[0].Rows[i][3].ToString(), table.Tables[0].Rows[i][4].ToString(), table.Tables[0].Rows[i][5].ToString());
            }
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

        public void run_sql_command(string sql)
        {
            try
            {
                //lock (this)
                {
                    SqlConnection ssconn = new SqlConnection("server = 10.135.200.35; uid = amm; pwd = amm@123; database = GR_Automation");
                    ssconn.Open();
                    SqlCommand scom = new SqlCommand(sql, ssconn);
                    scom.CommandType = System.Data.CommandType.Text;
                    scom.CommandText = sql;
                    scom.ExecuteReader();

                    ssconn.Close();
                    ssconn.Dispose();
                    scom.Dispose();
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

        int SelectedNum = -1;

        private void dgv_Comment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedNum = int.Parse(dgv_Comment.Rows[e.RowIndex].Cells[2].Value.ToString());
            tb_Comment.Text = dgv_Comment.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = string.Format("insert into TB_SCRAP_COMMENT values('{0}','{1}','{2}',getdate(),'{3}')", tb_Plant.Text, tb_CustCode.Text, tb_Comment.Text, BankHost_main.strOperator);

            run_sql_command(sql);

            ReadData();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (SelectedNum != -1)
            {
                string sql = string.Format("update TB_SCRAP_COMMENT set COMMENT='{0}' where NO={1}", tb_Comment.Text, SelectedNum);

                run_sql_command(sql);

                ReadData();
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            dgv_Comment.EndEdit();
            if (SelectedNum != -1)
            {
                string sql = "";

                for(int i = 0; i< dgv_Comment.RowCount; i++)
                {
                    bool t = Convert.ToBoolean(dgv_Comment.Rows[i].Cells[0].Value);
                    if(t == true)
                    {
                        sql = string.Format("delete TB_SCRAP_COMMENT where NO={0}", dgv_Comment.Rows[i].Cells[2].Value.ToString());
                        run_sql_command(sql);
                    }
                }

                

                ReadData();
            }
        }
    }
}
