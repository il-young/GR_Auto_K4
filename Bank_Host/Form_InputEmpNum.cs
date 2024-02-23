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
    public partial class Form_InputEmpNum : Form
    {
        public delegate void evt_ReturnEmpNum(string empnum);
        public event evt_ReturnEmpNum ReturnEmpnumEvent;

        string empNum = "";

        public Form_InputEmpNum()
        {
            InitializeComponent();
        }

        public void setEmpNum(string num)
        {
            empNum = num;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (empNum == tb_empNum.Text)
                Close();
            else
            {
                MessageBox.Show("기존 작업자 사번과 다름니다.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReturnEmpnumEvent("RETURN");
            Close();
        }
    }
}
