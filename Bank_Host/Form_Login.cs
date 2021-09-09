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
    public partial class Form_Login : Form
    {
        public Form_Login()
        {
            InitializeComponent();
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            Fnc_LogIn();
        }

        public void Fnc_LogIn()
        {
            if (textBox_id.Text == BankHost_main.strAdminID && textBox_pw.Text == BankHost_main.strAdminPW)
            {
                BankHost_main.bAdminLogin = true;
                LogIn_Exit();
            }
            else
            {
                BankHost_main.bAdminLogin = false;
                MessageBox.Show("ID 또는 비밀번호가 틀립니다. 다시 시도 하여 주십시오.");
                textBox_id.Text = "";
                textBox_pw.Text = "";
                textBox_id.Focus();
            }
        }

        public void LogIn_Exit()
        {
            this.Dispose();
            GC.Collect();
        }

        public void LogIn_Init()
        {
            textBox_id.Text = "";
            textBox_pw.Text = "";
            textBox_id.Focus();
        }

        private void textBox_pw_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                Fnc_LogIn();
            }
        }

        private void Form_Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogIn_Exit();
        }
    }
}
