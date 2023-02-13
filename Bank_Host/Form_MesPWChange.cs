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
    public partial class Form_MesPWChange : Form
    {
        public Form_MesPWChange()
        {
            InitializeComponent();
        }

        private void tb_EmpNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                tb_ID.Focus();

                DataSet t = SearchData(string.Format("select [MES_ID] from TB_USER_INFO with(nolock) where [ID]={0}", tb_EmpNum.Text));

                if (t.Tables[0].Rows.Count == 0)
                {
                    tb_EmpNum.SelectAll();
                    MessageBox.Show("사번이 없습니다.\n사용자 등록을 먼저 진행해 주세요");
                }
                else if (t.Tables[0].Rows.Count == 1)
                {
                    tb_ID.Text = t.Tables[0].Rows[0][0].ToString();
                    tb_PW.Focus();
                }
                else
                {
                    tb_EmpNum.SelectAll();
                    MessageBox.Show("중복된 사번이 있습니다.\n관리자에게 문의해 주세요");
                }
            }
        }

        private void tb_ID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                tb_PW.Focus();
        }

        private void tb_PW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                btn_save_Click(sender, e);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            int res = RunSQL(string.Format("update TB_USER_INFO set [MES_ID]='{0}', [MES_PASSWORD]='{1}' where [ID]='{2}'\nselect @@ROWCOUNT", tb_ID.Text, tb_PW.Text, tb_EmpNum.Text));

            if(res == 1)
                MessageBox.Show("저장 되었습니다.");
            else
                MessageBox.Show("실패 하였습니다.");

            Close();
        }


        public int RunSQL(string sql)
        {
            int res = 0;
            try
            {
                using (SqlConnection c = new SqlConnection("server = 10.135.200.35; uid = amm; pwd = amm@123; database = GR_Automation"))
                {
                    c.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, c))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
                //frm_Main.save_log(string.Format("Call:{0} -> Function:{1}, Param:{2}", System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, name + "," + x+"," + y + "," +tag));
            }
            catch (Exception ex)
            {
                
            }
            finally
            {

            }

            return res;
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

    }
}
