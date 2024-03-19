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
    public partial class Form_CustNameUse : Form
    {
        public Form_CustNameUse()
        {
            InitializeComponent();
        }

        public void run_sql_command(string sql)
        {
            try
            {
                //lock (this)
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
                    //ssconn.Close();
                    //ssconn.Dispose();
                    //scom.Dispose();
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

        private void Form_CustNameUse_Load(object sender, EventArgs e)
        {
            DataSet ds = SearchData("select [CUST], [BCR_TYPE], [NAME], [USE] from TB_CUST_INFO with(nolock) order by [CUST] ");


            for(int i = 0; i < ds.Tables[0].Columns.Count -1; i++)
            {
                dgv_custname.Columns.Add(ds.Tables[0].Columns[i].ColumnName, ds.Tables[0].Columns[i].ColumnName);
            }

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "USE";
            checkBoxColumn.Width = 30;
            checkBoxColumn.Name = "checkBoxColumn";
            
            dgv_custname.Columns.Insert(3, checkBoxColumn);

            for(int i = 0; i< ds.Tables[0].Rows.Count; i++)
            {
                dgv_custname.Rows.Add(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][1].ToString(), ds.Tables[0].Rows[i][2].ToString(), ds.Tables[0].Rows[i][3].ToString());
            }

            ds = SearchData("select DISTINCT [CUST] from TB_CUST_INFO with(nolock) order by [CUST]");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                cb_Names.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }

        private void dgv_custname_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void dgv_custname_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.ColumnIndex != -1 && e.RowIndex != -1)
                run_sql_command($"update TB_CUST_INFO set [USE]={(dgv_custname.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "True" ? 0: 1)} where [CUST]='{dgv_custname.Rows[e.RowIndex].Cells["CUST"].Value.ToString()}' and [NAME]='{dgv_custname.Rows[e.RowIndex].Cells["NAME"].Value.ToString()}'");
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
