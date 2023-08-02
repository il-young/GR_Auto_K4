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
    public partial class Form_ReaderSetting : Form
    {
        

        DateTime updateTime = new DateTime();

        public Form_ReaderSetting()
        {
            InitializeComponent();
        }

        private void Form_ReaderSetting_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (Properties.Settings.Default.CameraType.ToUpper() == "KEYENCE")
            {
                rb_Keyence.Checked = true;
                rb_cognex.Checked = false;

                tb_ip.Text = Properties.Settings.Default.ReaderIP.Split(';')[0];
                tb_port.Text = Properties.Settings.Default.ReaderPort.Split(';')[0];
                cb_web.Checked = Properties.Settings.Default.ReaderWebpage.Split(';')[0].ToUpper() == "TRUE" ? true : false;
            }
            else if (Properties.Settings.Default.CameraType.ToUpper() == "COGNEX")
            {
                rb_Keyence.Checked = false;
                rb_cognex.Checked = true;

                tb_ip.Text = Properties.Settings.Default.ReaderIP.Split(';')[1];
                tb_port.Text = Properties.Settings.Default.ReaderPort.Split(';')[1];
                cb_web.Checked = Properties.Settings.Default.ReaderWebpage.Split(';')[1].ToUpper() == "TRUE" ? true : false;
            }
            updateTime = DateTime.Now;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rb_Keyence_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Keyence.Checked == true)
            {
                Properties.Settings.Default.CameraType = "KEYENCE";
                Properties.Settings.Default.Save();

                Refresh();
            }
        }

        private void rb_cognex_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_cognex.Checked == true)
            {
                Properties.Settings.Default.CameraType = "COGNEX";
                Properties.Settings.Default.Save();

                Refresh();
            }
        }

        private void tb_ip_TextChanged(object sender, EventArgs e)
        {
            if((DateTime.Now - updateTime).TotalSeconds >= 1)
            {
                string[] temp =  Properties.Settings.Default.ReaderIP.Split(';');

                if (rb_Keyence.Checked == true)
                    temp[0] = tb_ip.Text;
                else
                    temp[1] = tb_ip.Text;

                Properties.Settings.Default.ReaderIP = string.Join(";", temp);
                Properties.Settings.Default.Save();
            }
        }

        private void tb_port_TextChanged(object sender, EventArgs e)
        {
            if ((DateTime.Now - updateTime).TotalSeconds >= 1)
            {
                string[] temp = Properties.Settings.Default.ReaderPort.Split(';');

                if (rb_Keyence.Checked == true)
                    temp[0] = tb_port.Text;
                else
                    temp[1] = tb_port.Text;

                Properties.Settings.Default.ReaderPort = string.Join(";", temp);
                Properties.Settings.Default.Save();
            }
        }

        private void cb_web_CheckedChanged(object sender, EventArgs e)
        {
            if ((DateTime.Now - updateTime).TotalSeconds >= 1)
            {
                string[] temp = Properties.Settings.Default.ReaderWebpage.Split(';');

                if (rb_Keyence.Checked == true)
                    temp[0] = cb_web.Checked == true ? "TRUE" : "FALSE";
                else
                    temp[1] = cb_web.Checked == true ? "TRUE" : "FALSE";

                Properties.Settings.Default.ReaderWebpage = string.Join(";", temp);
                Properties.Settings.Default.Save();
            }
        }

        private void btn_Cognex_Click(object sender, EventArgs e)
        {
            BankHost_main.mf.ShowDialog();
        }
    }
}
