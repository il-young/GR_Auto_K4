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
    public partial class Form_ShelfNumInput : Form
    {
        public delegate void evtSelctShelf(string Start, string End);
        public event evtSelctShelf SelectShelfEvent;

        public Form_ShelfNumInput()
        {
            InitializeComponent();
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            SelectShelfEvent($"{tb_StartShelf.Text},{tb_StartBoxNo.Text}", $"{tb_EndShelf.Text},{tb_EndBox.Text}");
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tb_StartShelf_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                tb_EndShelf.Text = tb_StartShelf.Text;
                tb_StartBoxNo.Focus();
            }
        }

        private void tb_StartBoxNo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                tb_EndShelf.Focus();
            }
        }

        private void tb_EndShelf_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tb_EndBox.Focus();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
