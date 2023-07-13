using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Host
{
    public partial class Form_addWSNDev : Form
    {
        public Form_addWSNDev()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void run_sql_command(string sql)
        {
            try
            {
                //lock (this)
                {
                    using (SqlConnection ssconn = new SqlConnection("server = 10.135.200.35; uid = amm; pwd = amm@123; database = GR_Automation"))
                    {
                        ssconn.Open();
                        using (SqlCommand scom = new SqlCommand(sql, ssconn))
                        {
                            scom.CommandType = System.Data.CommandType.Text;
                            scom.CommandText = sql;
                            scom.ExecuteReader();
                        }
                    }
                    //ssconn.Close();
                    //ssconn.Dispose();
                    //scom.Dispose();
                }
                //frm_Main.save_log(string.Format("Call:{0} -> Function:{1}, Param:{2}", System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql));
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private System.Data.DataSet SearchData(string sql)
        {
            System.Data.DataSet dt = new System.Data.DataSet();

            try
            {
                using (SqlConnection c = new SqlConnection("server = 10.135.200.35; uid = amm; pwd = amm@123; database = GR_Automation"))
                {
                    c.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, c))
                    {
                        using (SqlDataAdapter adt = new SqlDataAdapter(cmd))
                        {
                            adt.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        private void Form_addWSNDev_Load(object sender, EventArgs e)
        {
            LoadDevice();
        }


        private void LoadDevice()
        {
            lb_Device.Items.Clear();

            DataSet ds = SearchData($"select Source_Device from TB_QORVO_WSN_DEVICE with(nolock)");

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lb_Device.Items.Add(row["Source_Device"].ToString());
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if(lb_Device.SelectedIndex != -1 && lb_Device.SelectedIndex < lb_Device.Items.Count)
            {
                run_sql_command($"delete from TB_QORVO_WSN_DEVICE where Source_Device='{lb_Device.Items[lb_Device.SelectedIndex]}'");
                LoadDevice();
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            string Dev = "";

            if (DialogResult.OK == InputBox("Device를 입력 하세요", "Device를 입력 하세요", ref Dev))
            {
                run_sql_command($"Insert INTO TB_QORVO_WSN_DEVICE (Source_Device) values ('{Dev}') ");

                LoadDevice();
            }
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
    }
}
