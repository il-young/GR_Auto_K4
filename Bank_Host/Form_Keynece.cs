using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MICube.SmartDriver.Base.TCP;
using TCPConfig = MICube.SmartDriver.Base.TCP.Config;
using System.Configuration;

namespace Bank_Host
{
    public partial class Form_Keynece : Form
    {
        public TCP SocketManager = null;
        public TCP.EnumConnectStatus SocketState = TCP.EnumConnectStatus.None;
        public string strLogfilePath = "";
        public string strReceivedata = "", strSocketStatus = "";

        string STX = string.Format("{0}", (char)0x02);
        string ETX = string.Format("{0}", (char)0x03);
        bool initComp = false;

        public Form_Keynece()
        {
            InitializeComponent();
        }

        public void Fnc_Init()
        {
            timer1.Start();

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath + @"\Log");
            if (!di.Exists) { di.Create(); }
            strLogfilePath = di.ToString();

            Socket_Init();            
        }

        public void Fnc_Exit()
        {
            Socket_Close();
        }

        public void Socket_Init()
        {
            try
            {
                if (SocketManager != null)
                    return;

                //if (ConfigurationManager.AppSettings["CommunicationType"] != "Socket")
                    //return;

                SocketManager = new TCP();

                SocketManager.Config.ConnectMode = (TCPConfig.EnumConnectMode)Enum.Parse(typeof(TCPConfig.EnumConnectMode), ConfigurationManager.AppSettings["ConnectType"], true);
                SocketManager.Config.IpAddress = ConfigurationManager.AppSettings["IPAddress"];
                SocketManager.Config.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                SocketManager.Config.EquipmentId = ConfigurationManager.AppSettings["EquipmentId"];
                SocketManager.Config.ReconnectTimer = int.Parse(ConfigurationManager.AppSettings["RetryTime"]);

                SocketManager.OnConnectStatus += new TCP.OnConnectStatusEvent(socketManager_OnConnectStatus);
                SocketManager.OnReceivedStringMessage += new TCP.OnReceivedStringMessageEvent(socketManager_OnReceivedStringMessage);

                SocketManager.Open();

                string strMsg = string.Format("Keyence: {0}: {1}", SocketManager.Config.IpAddress, SocketManager.Config.Port);
                Fnc_SaveLog(strMsg);
                Fnc_SaveLog("SocketManager OK!");

                //if(BankHost_main.nInputMode == 1)

                Fnc_LoadFile(8); //Load file 7:진승리 위원님 파일, 8: GR

                initComp = true;
            }
            catch (Exception ex)
            {
                Fnc_SaveLog(ex.ToString());
            }
        }

        public void Socket_Close()
        {
            if (SocketManager != null)
            {
                if (BankHost_main.nInputMode == 1)
                    Socket_MessageSend("BLOAD,7");  //Load file 7:진승리 위원님 파일, 8: GR

                Thread.Sleep(300);

                string strMsg = string.Format("SocketManager Close: {0}", SocketManager.Config.IpAddress);
                Fnc_SaveLog(strMsg);
                SocketManager.Close();
            }

            timer1.Stop();
        }

       public void Socket_MessageSend(string strData)
        {
            SocketManager.SendMessage(STX + strData + ETX);
        }

        public void socketManager_OnConnectStatus(TCP.EnumConnectStatus connectStatus)
        {
            try
            {
                SocketState = connectStatus;

                if (connectStatus == TCP.EnumConnectStatus.Connected)
                {
                    
                }

                string strMsg = string.Format("socketManager_OnConnectStatus: {0}", connectStatus);
                Fnc_SaveLog(strMsg);
            }
            catch (Exception ex)
            {
                Fnc_SaveLog(ex.ToString());
            }
        }

        public void socketManager_OnReceivedStringMessage(string message)
        {
            try
            {
                BankHost_main.ReaderData = strReceivedata = message;
                Fnc_SaveLog(message);
            }
            catch (Exception ex)
            {
                Fnc_SaveLog(ex.ToString());
            }
        }

        public void Fnc_SaveLog(string strLog) ///설비별 개별 로그 저장
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath + @"\Log");
            if (!di.Exists) { di.Create(); }
            strLogfilePath = di.ToString();

            string strPath = "";

            strPath = strLogfilePath + "\\keyence_";

            string strToday = string.Format("{0}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string strHead = string.Format(",{0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            strPath = strPath + strToday + ".txt";
            strHead = strToday + strHead;

            string strSave;
            strSave = strHead + ',' + strLog;
            Fnc_WriteFile(strPath, strSave);
        }

        public void Fnc_WriteFile(string strFileName, string strLine)
        {
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(strFileName, true))
            {
                file.WriteLine(strLine);
            }
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            if (textBox_command.Text != "")
            {
                strReceivedata = "";
                Socket_MessageSend(textBox_command.Text);
            }
        }

        private void button_LON_Click(object sender, EventArgs e)
        {
            strReceivedata = "";
            Socket_MessageSend("LON");
        }

        private void button_LOFF_Click(object sender, EventArgs e)
        {
            strReceivedata = "";
            Socket_MessageSend("LOFF");
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("현재 카메라 셋팅을 저장 합니다. 계속 하시겠습니까?", "SAVE", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.Yes)
            {
                int nNo = Int32.Parse(textBox_No.Text);
                string strCom = string.Format("BSAVE,{0}", nNo);

                strReceivedata = "";
                Socket_MessageSend(strCom);
            }
        }

        private void Fnc_LoadFile(int nNo)
        {
            string strCom = string.Format("BLOAD,{0}", nNo);

            Socket_MessageSend(strCom);
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            int nNo = Int32.Parse(textBox_No.Text);
            Fnc_LoadFile(nNo);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (BankHost_main.nScanMode == 0 && Properties.Settings.Default.CameraType == "KEYENCE")
            {
                textBox_receivedata.Text = strReceivedata.Replace("\r","");
                label_state.Text = SocketState.ToString();

                if (label_state.Text.ToLower() == "connected")
                {
                    label_state.BackColor = Color.Green;
                    BankHost_main.bVisionConnect = true;
                }
                else
                {
                    label_state.BackColor = Color.Red;
                    BankHost_main.bVisionConnect = false;

                    if (initComp == true)
                    {
                        SocketManager.Open();
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
        }

        
    }
}
