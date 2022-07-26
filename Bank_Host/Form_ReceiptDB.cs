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
    public partial class Form_ReceiptDB : Form
    {
        public Form_ReceiptDB()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "";

            if(tb_request.Text == "")
            {
                query = string.Format("select [CUSTOMER_NAME], [CUSTOMER_CODE], [LINE_CODE], [DATE], [TTL_CT], [GROSS_WT], [REQUEST_NUM], " +
                                "[LOT_QTY], [WEIGHT], [RECEIPT], [CONSIGNEE] from TB_SCRAP_RECEIPT with(NOLOCK) where [DATE]='{0}'", dateTimePicker1.Text);
            }
            else
            {
                query = string.Format("select [CUSTOMER_NAME], [CUSTOMER_CODE], [LINE_CODE], [DATE], [TTL_CT], [GROSS_WT], [REQUEST_NUM], " +
                                "[LOT_QTY], [WEIGHT], [RECEIPT], [CONSIGNEE] from TB_SCRAP_RECEIPT with(NOLOCK) where [REQUEST]='{0}' and [DATE]='{1}'", tb_request.Text, dateTimePicker1.Text);
            }

            dataGridView1.DataSource = SearchData(query).Tables[0];
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

        private void Form_ReceiptDB_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");

            button1_Click(sender, e);
        }

        int SelectedRow = 0;

        private void btn_Create_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[SelectedRow];
            List<string> name = new List<string> { row.Cells[0].Value.ToString() };
            List<string> code = new List<string> { row.Cells[1].Value.ToString() };
            //      0                   1               2       3       4           5           6           7           8           9       10
            //[CUSTOMER_NAME], [CUSTOMER_CODE], [LINE_CODE], [DATE], [TTL_CT], [GROSS_WT], [REQUEST_NUM],[LOT_QTY], [WEIGHT], [RECEIPT], [CONSIGNEE]
            Form_InBill inBill = new Form_InBill(code, name, row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString(), row.Cells[4].Value.ToString(),
                row.Cells[5].Value.ToString(), row.Cells[6].Value.ToString(), row.Cells[7].Value.ToString(), row.Cells[8].Value.ToString(),
                row.Cells[9].Value.ToString(), row.Cells[10].Value.ToString());

            inBill.ShowDialog();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRow = e.RowIndex;
        }
    }
}
