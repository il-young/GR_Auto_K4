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
    public partial class Form_Lotchange : Form
    {
        public string strOrgName = "", strNewName = "";

        public Form_Lotchange()
        {
            InitializeComponent();
        }

        public void Fnc_Set_OrgName(string strOrg)
        {
            strOrgName = strOrg;
            strNewName = "";
            textBox_org.Text = strOrgName;
            textBox_new.Text = strNewName;

            textBox_new.Focus();
        }

        public void Fnc_Exit()
        {
            this.Dispose();
        }

        private void Form_Lotchange_FormClosing(object sender, FormClosingEventArgs e)
        {
            Fnc_Exit();
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            if(textBox_new.Text != "")
            {
                DialogResult dialogResult1 = MessageBox.Show("Lot 이름이 변경 됩니다..\n\n정말 변경 하시겠습니까?", "Alart", MessageBoxButtons.YesNo);
                if (dialogResult1 == DialogResult.Yes)
                {
                    strNewName = textBox_new.Text;
                    Form_Sort.strNewLotname = strNewName;

                    Fnc_Exit();
                }
                else
                {
                    textBox_new.Text = "";
                    textBox_new.Focus();
                    return;
                }
            }
        }
    }
}
