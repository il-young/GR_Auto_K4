using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Host
{
    public partial class frm_LoactionLabel : Form
    {
        //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        //[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //private static extern IntPtr GetModuleHandle(string lpModuleName);
        //[DllImport("kernel32.dll")]
        //static extern IntPtr LoadLibrary(string lpFileName);

        //private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        //private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static IntPtr hhook = IntPtr.Zero;

        Form_Print Frm_Print;

        //private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        //{
        //    if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        //    {
        //        int vkCode = Marshal.ReadInt32(lParam);

                                                                                
        //    }
        //    //return CallNextHookEx(_hookID, nCode, wParam, lParam);
        //}

        public void SetHook()
        {
            //IntPtr hInstance = LoadLibrary("User32");
            //hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }

        public static void UnHook()
        {
            //UnhookWindowsHookEx(hhook);
        }

        public void SetText(Keys i)
        {

        }


        public frm_LoactionLabel()
        {
            InitializeComponent();
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            string ZPLCODE = makePDFZPL(string.Format("{0}{1}{2}", tb_1.Text, label2.Text, tb_2.Text));

            if (rb_Single.Checked == true)
            {
                Frm_Print.SendStringToPrinter(ZPLCODE);
            }
            else if(rb_Copy.Checked == true)
            {
                string copy = "";
                int n = -1;

                InputBox("Copy", "출력 장수 입력", ref copy);

                if(int.TryParse(copy, out n) == true)
                {
                    if(n != 0 && n != -1)
                    {
                        for(int i = 0; i < n; i++)
                        {
                            Frm_Print.SendStringToPrinter(ZPLCODE);
                            System.Threading.Thread.Sleep(500);
                        }
                    }
                }
            }
            else if(rb_con.Checked == true)
            {
                string copy = "";
                int n = -1;
                int num = int.Parse(tb_2.Text);

                InputBox("Copy", "출력 갯수 입력", ref copy);

                if (int.TryParse(copy, out n) == true)
                {
                    if (n != 0 && n != -1)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            ZPLCODE = makePDFZPL(string.Format("{0}{1}{2}", tb_1.Text, label2.Text, num++));

                            Frm_Print.SendStringToPrinter(ZPLCODE);
                            System.Threading.Thread.Sleep(500);
                        }
                    }
                }
            }

            //Form_Print.SendStringToPrinter(ZPLCODE);
        }

        private string makePDFZPL(string code)
        {
            string ZPL = "^XA";
            ZPL += string.Format("^CF0,190,{0}", ((400 / code.Length) * 2) /10 *10);
            ZPL += string.Format("^FO30,30^FD{0}^FS", code);
            ZPL += "^BY5,3,150";
            ZPL += string.Format("^FO430,30^B7^FD{0}^FS",code);
            ZPL += "^XZ";

            return ZPL;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //UnHook();
            Close();
        }

        private void frm_LoactionLabel_Load(object sender, EventArgs e)
        {
            Frm_Print = new Form_Print();
            //SetHook();
        }

        public DialogResult InputBox(string title, string content, ref string value)
        {
            Form form = new Form();
            PictureBox picture = new PictureBox();
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Button buttonOk = new System.Windows.Forms.Button();
            System.Windows.Forms.Button buttonCancel = new System.Windows.Forms.Button();

            form.ClientSize = new Size(300, 100);
            form.Controls.AddRange(new Control[] { label, picture, textBox, buttonOk, buttonCancel });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            form.TopMost = true;

            form.Text = title;
            //picture.Image = Properties.Resources.Clogo;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            label.Text = content;
            textBox.Text = value;
            buttonOk.Text = "확인";
            buttonCancel.Text = "취소";

            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            picture.SetBounds(10, 10, 50, 50);
            label.SetBounds(65, 17, 100, 20);
            textBox.SetBounds(65, 40, 220, 20);
            buttonOk.SetBounds(135, 70, 70, 20);
            buttonCancel.SetBounds(215, 70, 70, 20);

            DialogResult dialogResult = form.ShowDialog();

            value = textBox.Text;
            return dialogResult;
        }

        private void rb_typing_CheckedChanged(object sender, EventArgs e)
        {
            if(rb_typing.Checked == true)
            {
                tb_scan.Enabled = false;
                tb_1.Enabled = true;
                tb_2.Enabled = true;
            }
        }

        private void rb_scan_CheckedChanged(object sender, EventArgs e)
        {
            if(rb_scan.Checked == true)
            {
                tb_scan.Enabled = true;
                tb_1.Enabled = false;
                tb_2.Enabled = false;
            }
        }

        private void tb_1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                tb_2.Focus();
            }
        }

        private void tb_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btn_Print_Click(sender, e);
            }
        }

        private void tb_scan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                string ZPLCODE = makePDFZPL(string.Format("{0}", tb_scan.Text));

                Frm_Print.SendStringToPrinter(ZPLCODE);

                tb_scan.Invoke((MethodInvoker)delegate
                {
                    tb_scan.Text = "";
                });
                
            }
        }

        private void rb_2_CheckedChanged(object sender, EventArgs e)
        {
            if(rb_2.Checked == true)
            {
                tb_Text2.Enabled = true;
            }
        }

        private void rb_1_CheckedChanged(object sender, EventArgs e)
        {
            if(rb_1.Checked == true)
            {
                tb_Text2.Enabled = false;
            }
        }

        private void tb_Text1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Return)
            {
                if(rb_1.Checked == true)
                {
                    btn_PrintText_Click(sender, e);
                }
                else
                {
                    tb_2.Focus();
                }
            }
        }

        private void btn_PrintText_Click(object sender, EventArgs e)
        {
            string zpl = "";
            if(rb_2.Checked == false)
            {
                zpl = MakeText1();
            }
            else
            {
                zpl = MakeText2();
            }

            Frm_Print.SendStringToPrinter(zpl);
        }

        public string MakeText1()
        {
            string zpl = "^XA";
            zpl += string.Format("^CF0,190,{0}", (800 / tb_Text1.Text.Length)*2  );
            zpl += string.Format("^FO30,30^FD{0}^FS", tb_Text1.Text);
            zpl += "^XZ";

            return zpl;
        }

        public string MakeText2()
        {
            string zpl = "^XA";
            zpl += string.Format("^CF0,80,{0}", tb_Text1.Text.Length > tb_Text2.Text.Length ? (800 / tb_Text1.Text.Length) * 2 : (800 / tb_Text2.Text.Length) * 2);
            zpl += string.Format("^FO30,20^FD{0}^FS", tb_Text1.Text);
            zpl += string.Format("^FO30,120^FD{0}^FS", tb_Text2.Text);
            zpl += "^XZ";

            return zpl;
        }

        private void tb_Text2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                btn_PrintText_Click(sender, e);                
            }
        }
    }
}
