using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Bank_Host
{
    public partial class Form_Option : Form
    {
        public Form_Option()
        {
            InitializeComponent();
        }

        public void Fnc_Exit()
        {
            this.Dispose();
        }

        public void Fnc_Init_image()
        {
            try
            {
                label_basketTime1.Image = null;
                label_basketTime2.Image = null;
                label_basketTime3.Image = null;
                label_basketTime4.Image = null;

                label_basketTime1.Image = Image.FromFile("img\\OFF.png");
                label_basketTime2.Image = Image.FromFile("img\\OFF.png");
                label_basketTime3.Image = Image.FromFile("img\\OFF.png");
                label_basketTime4.Image = Image.FromFile("img\\OFF.png");

                if (BankHost_main.nScanMode == 0)
                {
                    label_basketTime1.Image = null;
                    label_basketTime1.Image = Image.FromFile("img\\ON.png");
                }
                else if (BankHost_main.nScanMode == 1)
                {
                    label_basketTime2.Image = null;
                    label_basketTime2.Image = Image.FromFile("img\\ON.png");
                }
                else if (BankHost_main.nScanMode == 2)
                {
                    label_basketTime3.Image = null;
                    label_basketTime3.Image = Image.FromFile("img\\ON.png");
                }
                else if (BankHost_main.nScanMode == 3)
                {
                    label_basketTime4.Image = null;
                    label_basketTime4.Image = Image.FromFile("img\\ON.png");
                }

                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration
                   (ConfigurationUserLevel.None);

                config.AppSettings.Settings.Remove("Scan_mode");
                config.AppSettings.Settings.Add("Scan_mode", BankHost_main.nScanMode.ToString());
                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");

                string str = ConfigurationManager.AppSettings["Scan_mode"];
                BankHost_main.nScanMode = Int32.Parse(str);
            }
            catch
            {

            }
        }

        public void Fnc_Init_image2()
        {
            try
            {
                label_typeimg1.Image = null;
                label_typeimg2.Image = null;
                label_amkorbcr1.Image = null;
                label_amkorbcr2.Image = null;

                label_typeimg1.Image = Image.FromFile("img\\OFF.png");
                label_typeimg2.Image = Image.FromFile("img\\OFF.png");
                label_amkorbcr1.Image = Image.FromFile("img\\OFF.png");
                label_amkorbcr2.Image = Image.FromFile("img\\OFF.png");

                if (BankHost_main.nMaterial_type == 0)
                {
                    label_typeimg1.Image = null;
                    label_typeimg1.Image = Image.FromFile("img\\ON.png");
                }
                else if (BankHost_main.nMaterial_type == 1)
                {
                    label_typeimg2.Image = null;
                    label_typeimg2.Image = Image.FromFile("img\\ON.png");
                }

                if(BankHost_main.nAmkorBcrType == 0)
                {
                    label_amkorbcr1.Image = null;
                    label_amkorbcr1.Image = Image.FromFile("img\\ON.png");
                }
                else
                {
                    label_amkorbcr2.Image = null;
                    label_amkorbcr2.Image = Image.FromFile("img\\ON.png");
                }

                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration
                   (ConfigurationUserLevel.None);

                config.AppSettings.Settings.Remove("Material_type");
                config.AppSettings.Settings.Add("Material_type", BankHost_main.nMaterial_type.ToString());
                config.Save(ConfigurationSaveMode.Modified);

                config.AppSettings.Settings.Remove("AmkorBcr_type");
                config.AppSettings.Settings.Add("AmkorBcr_type", BankHost_main.nAmkorBcrType.ToString());
                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");

                string str = ConfigurationManager.AppSettings["Material_type"];
                BankHost_main.nMaterial_type = Int32.Parse(str);

                str = ConfigurationManager.AppSettings["AmkorBcr_type"];
                BankHost_main.nAmkorBcrType = Int32.Parse(str);
            }
            catch
            {

            }
        }

        private void label_basketTime1_Click(object sender, EventArgs e)
        {            
            if (BankHost_main.nMaterial_type == 1)
            {
                BankHost_main.nScanMode = 1;
            }
            else
            {
                BankHost_main.nScanMode = 0;
            }
            
            Fnc_Init_image();
        }

        private void label_basketTime2_Click(object sender, EventArgs e)
        {
            BankHost_main.nScanMode = 1;
            Fnc_Init_image();
        }

        private void label_basketTime3_Click(object sender, EventArgs e)
        {
            if (BankHost_main.nMaterial_type == 1)
            {
                BankHost_main.nScanMode = 1;
            }
            else
            {
                BankHost_main.nScanMode = 2;
            }
                
            Fnc_Init_image();
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Fnc_Exit();
        }

        private void label_basketTime4_Click(object sender, EventArgs e)
        {
            if (BankHost_main.nMaterial_type == 1)
            {
                BankHost_main.nScanMode = 1;
            }
            else
            {
                BankHost_main.nScanMode = 3;
            }
                
            Fnc_Init_image();
        }

        private void label_typeimg1_Click(object sender, EventArgs e)
        {
            BankHost_main.nMaterial_type = 0;
            Fnc_Init_image2();
        }

        private void label_typeimg2_Click(object sender, EventArgs e)
        {
            BankHost_main.nMaterial_type = 1;
            Fnc_Init_image2();

            BankHost_main.nScanMode = 1;
            Fnc_Init_image();
        }

        private void label_amkorbcr1_Click(object sender, EventArgs e)
        {
            BankHost_main.nAmkorBcrType = 0;
            Fnc_Init_image2();
        }

        private void label_amkorbcr2_Click(object sender, EventArgs e)
        {
            BankHost_main.nAmkorBcrType = 1;
            Fnc_Init_image2();
        }

        private void Form_Option_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.LOCATION == "K5")
            {
                label_basketTime1.Enabled = false;
                label_basketTime2.Enabled = true;
            }
            else if(Properties.Settings.Default.LOCATION == "K4")
            {
                label_basketTime1.Enabled = true;
                label_basketTime2.Enabled = true;
            }
            else if(Properties.Settings.Default.LOCATION == "K3")
            {
                label_basketTime1.Enabled = true;
                label_basketTime2.Enabled = true;
            }
        }
    }
}
