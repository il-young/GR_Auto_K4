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
    public partial class Form_Request : Form
    {
        public delegate void PressOK(string RequestNum);
        public event PressOK PressOK_Event;

        public delegate void PressCancel();
        public event PressCancel PressCancel_Event;

        List<string> request = new List<string>();
        string TextMSG = "";

        public Form_Request()
        {
            InitializeComponent();
        }

        public Form_Request(List<string> RequestList)
        {
            request = RequestList;
            InitializeComponent();
        }

        

        public Form_Request(List<string> RequestList, string msg)
        {
            request = RequestList;
            TextMSG = msg;
            InitializeComponent();
        }

        private void Form_Request_Load(object sender, EventArgs e)
        {
            for(int i = 0; i< request.Count; i++)
            {
                cb_RequestList.Items.Add(request[i]);
            }

            if(TextMSG != "")
            {
                label1.Text = TextMSG;
            }
            
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            PressOK_Event(cb_RequestList.Text);
            Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            PressCancel_Event();
            Close();
        }

        private void cb_RequestList_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == 13)
            {
                if(cb_RequestList.Text != "")
                btn_OK_Click(sender, e);
            }
        }

        private void cb_RequestList_ImeModeChanged(object sender, EventArgs e)
        {

        }
    }
}
