using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Host
{
    public partial class Form_Splitlog_Input : Form
    {
        public delegate void return_select(string val);
        public event return_select return_select_event;


        string list_return_val = "";
        List<string> list_line_code = new List<string>();
        List<string> list_cust = new List<string>();
        public Form_Splitlog_Input()
        {
            InitializeComponent();
        }

        private void tb_employee_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                cb_cust.Focus();
            }
        }

        public Form_Splitlog_Input(List<string> cust, List<string> code)
        {
            CheckForIllegalCrossThreadCalls = false;

            list_cust = cust;
            list_line_code = code;
            InitializeComponent();
        }

        private void cb_cust_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cb_line_code.Focus();
            }
        }

        private void cb_line_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tb_binding.Focus();
            }
        }

        private void tb_binding_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok_Click(sender, e);
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if(tb_employee.Text =="")
            {
                MessageBox.Show("사원번호를 입력하세요");
                return;
            }
            
            if(cb_cust.Text == "")
            {
                MessageBox.Show("고객 코드를 입력 하세요");
                return;
            }

            if(cb_line_code.Text == "")
            {
                MessageBox.Show("라인코드를 입력하세요");
                return;
            }

            var dt = BankHost_main.SQL_GetUserDB(tb_employee.Text);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("등록 되지 않은 사용자 입니다.\n\n관리자에게 등록 요청을 하십시오.");
                BankHost_main.strOperator = "";

                return;
            }

            list_return_val = cb_cust.Text + ";" + cb_line_code.Text;
            return_select_event(list_return_val);
            Close();
        }

        private void cb_cust_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_line_code.Items.Clear();
            bool b = false;

            for(int i = 0; i < list_line_code.Count; i++)
            {
                if (cb_cust.Text == list_line_code[i].Split(';')[0])
                {
                    if (cb_line_code.Items.Count == 0)
                    {
                        cb_line_code.Items.Add(list_line_code[i].Split(';')[1]);                        
                    }
                    else
                    {
                        b = false;

                        for (int j = 0; j < cb_line_code.Items.Count; j++)
                        {
                          if(cb_line_code.Items[j].ToString() == list_line_code[i].Split(';')[1])
                            {
                                b = true;
                                break;
                            }   
                        }

                        if (b == false)
                        {
                            cb_line_code.Items.Add(list_line_code[i].Split(';')[1]);
                            b = false;
                        }
                    }
                    
                        
                }
            }
        }

        private void Form_Splitlog_Input_Load(object sender, EventArgs e)
        {
            
        }

        private void Form_Splitlog_Input_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < list_cust.Count; i++)
            {
                if (cb_cust.Items.Contains(list_cust[i]) == false)
                    cb_cust.Items.Add(list_cust[i]);
            }
        }
    }
}
