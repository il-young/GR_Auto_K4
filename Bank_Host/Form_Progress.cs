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
    public partial class Form_Progress : Form
    {
        public int nChangeIndex = 0;
        public bool bState = false;
        public string strbase = "데이터 처리 중 입니다. 기다려 주십시오.";

        public Form_Progress()
        {
            InitializeComponent();
        }

        public void Progress_Exit()
        {           
            bState = false;
            this.Dispose();
        }

        public void Form_Show(string strMsg)
        {
            try
            {
                label1.Text = strbase + strMsg;
                label1.BackColor = Color.RoyalBlue;
                bState = true;
                Show();
            }
            catch
            {

            }
        }

        public void Form_Display(string str)
        {
            label1.Text = strbase + str;
            label1.BackColor = Color.RoyalBlue;

            Application.DoEvents();
        }

        public void Form_Display_Warning(string str)
        {
            label1.Text = str;
            label1.BackColor = Color.Red;

            Application.DoEvents();
        }

        public void Form_Hide()
        {
            bState = false;
            Hide();
        }
    }
}
