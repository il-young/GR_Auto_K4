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
    public partial class frm_Find : Form
    {
        public delegate void FindEvt(string Lot);
        public event FindEvt FindEvent;

        public frm_Find()
        {
            InitializeComponent();
        }

        private void tb_Find_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                FindEvent(tb_Find.Text);
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Find_Click(object sender, EventArgs e)
        {
            tb_Find_KeyDown(sender, new KeyEventArgs(Keys.Enter));
        }
    }
}
