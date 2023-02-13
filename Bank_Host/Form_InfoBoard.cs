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
    public partial class Form_InfoBoard : Form
    {
        string msg = "";
        Color textColor;
        Color formColor;
        Font textFont;

        public Form_InfoBoard()
        {
            InitializeComponent();
        }

        public Form_InfoBoard(string s)
        {
            msg = s;
        }

        public Form_InfoBoard(string s, Color tC)
        {
            msg = s;
            textColor = tC;
        }

        public Form_InfoBoard(string s, Color tC, Color bC)
        {
            msg = s;
            textColor = tC;
            BackColor = bC;
        }

        private void Form_InfoBoard_Load(object sender, EventArgs e)
        {            
            Update();
        }

        public void SetFont(Font font)
        {
            textFont = font;
            Update();
        }

        public void Set(string s)
        {
            msg = s;
            Update();
        }

        public void Set(string s, Color tC)
        {
            msg = s;
            textColor = tC;
            Update();
        }

        public void Set(string s, Color tC, Color bC)
        {
            msg = s;
            textColor = tC;
            BackColor = bC;
            Update();
        }

        public void Update()
        {

            tb_MSG.Text = msg == null ? "" : msg;

            if (BackColor != null)
            {
                tb_MSG.BackColor = BackColor;
                this.BackColor = BackColor;
            }

            if (textColor != null)
                tb_MSG.ForeColor = textColor;


            if (textFont != null)
                tb_MSG.Font = textFont;

            tb_MSG.Select(0, 0);

            this.Invalidate();
        }

        private void tb_MSG_MouseClick(object sender, MouseEventArgs e)
        {
            Hide();
        }
    }
}
