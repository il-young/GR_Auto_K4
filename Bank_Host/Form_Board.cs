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
    public partial class Form_Board : Form
    {
        string m = "";
        string btext = "";
        Color MSGColor;
        Color BackColor;

        public Form_Board(string msg)
        {
            m = msg;

            InitializeComponent();
        }

        public Form_Board(string msg, Color c, Color bc)
        {
            m = msg;
            MSGColor = c;
            BackColor = bc;

            InitializeComponent();
        }


        public Form_Board(string msg, string ButtonText)
        {
            m = msg;
            btext = ButtonText;

            InitializeComponent();
        }

        private void Form_Board_Load(object sender, EventArgs e)
        {
            textBox1.Text = m;

            if(btext != "")
                button1.Text = btext;

            if (MSGColor != null)
                textBox1.ForeColor = MSGColor;

            if (BackColor != null)
                textBox1.BackColor = BackColor;

            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
