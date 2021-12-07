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
    public partial class Form_Input : Form
    {
        bool bok = false;
        DataTable dt = new DataTable();
        int nMode = 0;

        public Form_Input()
        {
            InitializeComponent();
        }

        private void textBox_sid_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox_sid.Text = textBox_sid.Text.ToUpper();

            if (e.KeyChar == (char)13)
            {
                if (nMode == 0 || nMode == 2 || nMode == 99)
                    return;

                string strsid = textBox_sid.Text;

                var dt = BankHost_main.SQL_GetUserDB(strsid);

                if(dt.Rows.Count == 0)
                {
                    MessageBox.Show("등록 되지 않은 사용자 입니다.\n\n관리자에게 등록 요청을 하십시오.");
                    BankHost_main.strOperator = "";

                    bok = false;

                    Fnc_Init(nMode);
                }
                else
                {
                    string strname = dt.Rows[0]["NAME"].ToString(); strname = strname.Trim();
                    string strgrade = dt.Rows[0]["GRADE"].ToString(); strname = strname.Trim();

                    if (nMode == 1)
                    {
                        if (strgrade != "A")
                        {
                            MessageBox.Show("해당 ID는 사용 권한이 없습니다.\n\n관리자에게 문의하세요.");
                            BankHost_main.strOperator = "";

                            bok = false;

                            return;
                        }
                    }

                    BankHost_main.strOperator = strname;

                    bok = true;

                    Fnc_Exit();
                }                
            }
        }

        public void Fnc_Init(int nIndex)
        {
            nMode = nIndex;

            if (nIndex == 0) //사번 입력
            {
                //label1.Text = "사번을 입력 하여 주십시오.";
                //label1.BackColor = Color.SlateGray;
                label3.Enabled = true;
                label4.Enabled = true;
                label5.Enabled = true;

                comboBox_cust.Enabled = true;
                dataGridView_bill.Enabled = true;
                textBox_bill.Enabled = false;
            }
            else if (nIndex == 99) //사번 입력
            {
                //label1.Text = "사번을 입력 하여 주십시오.";
                //label1.BackColor = Color.SlateGray;
                label3.Enabled = true;
                label4.Enabled = true;
                label5.Enabled = true;

                comboBox_cust.Enabled = false;
                dataGridView_bill.Enabled = true;
                textBox_bill.Enabled = false;
            }
            else 
            {
                label3.Enabled = false;
                label4.Enabled = false;                

                comboBox_cust.Enabled = false;
                dataGridView_bill.Enabled = false;

                if (nIndex == 2)
                {
                    textBox_bill.Enabled = true;
                    label5.Enabled = true;
                }
                else if(nIndex == 5)
                {
                    textBox_bill.Enabled = true;
                    label5.Enabled = true;
                }
                else
                {
                    textBox_bill.Enabled = false;
                    label5.Enabled = false;
                }
                
            }

            textBox_sid.Text = "";
            textBox_sid.Focus();

            Fnc_datagrid_init();
        }

        public void Fnc_Exit()
        {
            this.Dispose();
        }

        private void Form_Input_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(bok == false)
            {
                BankHost_main.strOperator = "";
            }

        }

        public void Fnc_datagrid_init()
        {
            dataGridView_bill.Columns.Clear();
            dataGridView_bill.Rows.Clear();
            dataGridView_bill.Refresh();

            Thread.Sleep(300);

            dataGridView_bill.Columns.Add("고객", "고객");
            dataGridView_bill.Columns.Add("Bill#", "Bill#");
            dataGridView_bill.Columns.Add("Lot수량", "Lot수량");
            dataGridView_bill.Columns.Add("상태", "상태");

            dataGridView_bill.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_bill.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_bill.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_bill.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "선택";
            checkBoxColumn.Width = 30;
            checkBoxColumn.Name = "checkBoxColumn";
            dataGridView_bill.Columns.Insert(4, checkBoxColumn);

            dataGridView_bill.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            dataGridView_bill.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_bill.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView_bill.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
        }

        public void Fnc_datagrid_add(string strCust, string strBill, string strLotcount, string strStatus)
        {
            dataGridView_bill.Rows.Add(new object[4] { strCust, strBill, strLotcount, strStatus});
        }

        public void Fnc_datagrid_saveinfo()
        {
            dt.Clear();

            foreach (DataGridViewColumn col in dataGridView_bill.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dataGridView_bill.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }
        }

        public void Fnc_cust_init()
        {
            comboBox_cust.Items.Clear();
        }

        public void Fnc_cust_add(string strCust)
        {
            comboBox_cust.Items.Add(strCust);
        }

        public bool Fnc_cust_check(string strCust)
        {
            int n = comboBox_cust.Items.Count;

            for(int i = 0; i<n; i++)
            {
                string str = comboBox_cust.Items[i].ToString();
                if (str == strCust)
                    return false;
            }

            return true;
        }

        private void comboBox_cust_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSetCust = comboBox_cust.Text;

            Fnc_datagrid_init();

            int nCount = dt.Rows.Count;

            for(int n = 0; n<nCount; n++)
            {
                string strGetcust = dt.Rows[n]["고객"].ToString();
                string strGetbill = dt.Rows[n]["Bill#"].ToString();
                string strGetlotcount = dt.Rows[n]["Lot수량"].ToString();
                string strGetstatus = dt.Rows[n]["상태"].ToString();

                if (strGetcust == strSetCust)
                {
                    Fnc_datagrid_add(strGetcust, strGetbill, strGetlotcount, strGetstatus);
                }
            }

            if(strSetCust == "453" || strSetCust == "734")
                textBox_bill.Enabled = false;
            else
                textBox_bill.Enabled = false;
        }

        private void dataGridView_bill_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int n = dataGridView_bill.Rows.Count;

            if (n < 1)
                return;

            int nIndex = dataGridView_bill.CurrentCell.RowIndex;
            bool bFalse = false;

            string strState = dataGridView_bill.Rows[nIndex].Cells[3].Value.ToString();
            if (strState.Contains("WORK"))
                bFalse = true;

            if (dataGridView_bill.Rows[nIndex].Cells[4].Value == null)
            {
                if (!bFalse)
                {
                    dataGridView_bill.Rows[nIndex].Cells[4].Value = true;

                    if (nMode == 99)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            if (i != nIndex)
                            {
                                dataGridView_bill.Rows[i].Cells[4].Value = false;
                            }
                        }
                    }
                }

                else
                    dataGridView_bill.Rows[nIndex].Cells[4].Value = false;
            }
            else
            {
                string Value = dataGridView_bill.Rows[nIndex].Cells[4].Value.ToString();

                if (Value == "True")
                {
                    dataGridView_bill.Rows[nIndex].Cells[4].Value = false;
                }
                else
                {
                    if (!bFalse)
                    {
                        dataGridView_bill.Rows[nIndex].Cells[4].Value = true;

                        if (nMode == 99)
                        {
                            for (int i = 0; i < n; i++)
                            {
                                if (i != nIndex)
                                {
                                    dataGridView_bill.Rows[i].Cells[4].Value = false;
                                }
                            }
                        }
                    }
                    else
                        dataGridView_bill.Rows[nIndex].Cells[4].Value = false;
                }
            }
         }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int n = dataGridView_bill.Rows.Count;

            if (n < 1)
                return;

            int nIndex = dataGridView_bill.CurrentCell.RowIndex;

            string strState = dataGridView_bill.Rows[nIndex].Cells[3].Value.ToString();

            if (strState.Contains("WORK"))
            {
                DialogResult dialogResult1 = MessageBox.Show("해당 Bill은 사용 중입니다. 그래도 선택 하시겠습니까?", "경고", MessageBoxButtons.YesNo);
                if (dialogResult1 == DialogResult.Yes)
                {
                    dataGridView_bill.Rows[nIndex].Cells[4].Value = true;
                }
            }
        }

        private void button_complete_Click(object sender, EventArgs e)
        {
            if(textBox_sid.Text == "" && nMode == 0)
            {
                MessageBox.Show("사번을 입력 해 주십시오");
                textBox_sid.Focus();
                return;
            }

            for (int i = 0; i < 20; i++)
                Form_Sort.strSelBillno[i] = "";

            if (nMode == 0)
            {
                int nCnt = dataGridView_bill.Rows.Count;
                int nCheckcount = 0;
                for (int n = 0; n < nCnt; n++)
                {
                    if (dataGridView_bill.Rows[n].Cells[4].Value != null)
                    {
                        string Value = dataGridView_bill.Rows[n].Cells[4].Value.ToString();

                        if (Value == "True")
                        {
                            Form_Sort.strSelBillno[nCheckcount] = dataGridView_bill.Rows[n].Cells[1].Value.ToString();
                            nCheckcount++;
                        }
                    }
                }

                if (nCheckcount == 0)
                {
                    MessageBox.Show("선택된 Bill이 없습니다. 작업 하실 Bill을 선택 하십시오.");
                    return;
                }

                if (comboBox_cust.Text == "")
                {
                    MessageBox.Show("선택된 고객이 없습니다. 고객을 선택 하십시오.");
                    return;
                }

                Form_Sort.strSelCust = comboBox_cust.Text;
            }
            else if (nMode == 99)
            {
                int nCnt = dataGridView_bill.Rows.Count;
                int nCheckcount = 0;
                for (int n = 0; n < nCnt; n++)
                {
                    if (dataGridView_bill.Rows[n].Cells[4].Value != null)
                    {
                        string Value = dataGridView_bill.Rows[n].Cells[4].Value.ToString();

                        if (Value == "True")
                        {
                            Form_Sort.strSelBillno[nCheckcount] = dataGridView_bill.Rows[n].Cells[1].Value.ToString();
                            Form_Sort.strSelCust = dataGridView_bill.Rows[n].Cells[0].Value.ToString();
                            Form_Sort.strSelJobName = dataGridView_bill.Rows[n].Cells[3].Value.ToString();
                            nCheckcount++;
                        }
                    }
                }

                if (nCheckcount == 0)
                {
                    MessageBox.Show("선택된 Bill이 없습니다. 작업 하실 Bill을 선택 하십시오.");
                    return;
                }
            }
            else if(nMode == 2)
            {
                if(textBox_bill.Text == "")
                {
                    MessageBox.Show("Bill# 입력 하여 주십시오.");
                    textBox_bill.Focus();
                    return;
                }

                Form_Sort.strInputBill = textBox_bill.Text.ToUpper();
            }
            else if(nMode == 5)
            {
                if (textBox_bill.Text == "")
                {
                    MessageBox.Show("Bill# 입력 하여 주십시오.");
                    textBox_bill.Focus();
                    return;
                }

                Form_Sort.strInputBill = textBox_bill.Text.ToUpper();
            }

            string strsid = textBox_sid.Text;

            var dt = BankHost_main.SQL_GetUserDB(strsid);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("등록 되지 않은 사용자 입니다.\n\n관리자에게 등록 요청을 하십시오.");
                BankHost_main.strOperator = "";

                bok = false;

                return;
            }
            else
            {
                string strname = dt.Rows[0]["NAME"].ToString(); strname = strname.Trim();
                string strgrade = dt.Rows[0]["GRADE"].ToString(); strname = strname.Trim();

                if(nMode == 0 || nMode == 1)
                {
                    if (strgrade != "A")
                    {
                        MessageBox.Show("해당 ID는 사용 권한이 없습니다.\n\n관리자에게 문의하세요.");
                        BankHost_main.strOperator = "";

                        bok = false;

                        return;
                    }
                }

                BankHost_main.strOperator = strname;

                bok = true;

                Fnc_Exit();
            }
        }

        private void textBox_bill_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
