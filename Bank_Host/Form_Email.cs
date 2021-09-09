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
    public partial class Form_Email : Form
    {
        public string strSendTo = "", strFrom = "",strCc = "", strSubject = "", strMessage = "";
        public string strCustNo = "";

        public Form_Email()
        {
            InitializeComponent();
        }

        public void Fnc_Init(string subject, string msg)
        {
            strSendTo = ConfigurationManager.AppSettings["Email_To"];
            strFrom = ConfigurationManager.AppSettings["Email_From"];
            strCc = ConfigurationManager.AppSettings["Email_Cc"];
            strMessage = msg;
            strSubject = subject;

            textBox_To.Text = strSendTo;
            textBox_Cc.Text = strCc;
            textBox_Subject.Text = strSubject;
            textBox_Msg.Text = strMessage;
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration
                   (ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("Email_To");
            config.AppSettings.Settings.Add("Email_To", strSendTo);
            config.Save(ConfigurationSaveMode.Modified);

            config.AppSettings.Settings.Remove("Email_From");
            config.AppSettings.Settings.Add("Email_From", strFrom);
            config.Save(ConfigurationSaveMode.Modified);

            config.AppSettings.Settings.Remove("Email_Cc");
            config.AppSettings.Settings.Add("Email_Cc", strCc);
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");

            strSendTo = textBox_To.Text;
            strCc = textBox_Cc.Text;
            strSubject = textBox_Subject.Text;
            strMessage = textBox_Msg.Text + "*GR Automation에서 보낸 메일 입니다.";

            Fnc_SendEmail(strMessage);

            MessageBox.Show("완료 되었습니다.");

            Fnc_Exit();
        }

        private void Form_Email_FormClosing(object sender, FormClosingEventArgs e)
        {
            Fnc_Exit();
        }

        public void Fnc_SendEmail(string strMessage)
        {
            string[] strSplit_address = strSendTo.Split(';');
            int ncount = strSplit_address.Length;

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            for (int n = 0; n < ncount; n++)
            {
                message.To.Add(strSplit_address[n]);
            }
            message.CC.Add(strCc);
            message.Subject = strSubject;
            message.From = new System.Net.Mail.MailAddress(strFrom);
            message.Body = strMessage;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("10.101.10.6");
            smtp.Credentials = new System.Net.NetworkCredential("", "");
            smtp.Port = 25;
            smtp.Send(message);
        }
        public void Fnc_Show()
        {
            Show();
        }

        public void Fnc_Hide()
        {
            Hide();
        }

        public void Fnc_Exit()
        {
            Dispose();
        }
    }
}
