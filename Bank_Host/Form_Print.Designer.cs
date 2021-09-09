namespace Bank_Host
{
    partial class Form_Print
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox_bcr = new System.Windows.Forms.PictureBox();
            this.textBox_dcc = new System.Windows.Forms.TextBox();
            this.textBox_coo = new System.Windows.Forms.TextBox();
            this.textBox_wfrLot = new System.Windows.Forms.TextBox();
            this.textBox_LotType = new System.Windows.Forms.TextBox();
            this.textBox_rvcdate = new System.Windows.Forms.TextBox();
            this.textBox_device = new System.Windows.Forms.TextBox();
            this.textBox_lotno = new System.Windows.Forms.TextBox();
            this.textBox_amkorid = new System.Windows.Forms.TextBox();
            this.textBox_billno = new System.Windows.Forms.TextBox();
            this.textBox_wfrqty = new System.Windows.Forms.TextBox();
            this.textBox_dieqty = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_cust = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label_dcc = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Close = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label_printname = new System.Windows.Forms.Label();
            this.label_Printuse = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox_receivedata = new System.Windows.Forms.TextBox();
            this.label_state = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_bcr)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGreen;
            this.panel1.Controls.Add(this.pictureBox_bcr);
            this.panel1.Controls.Add(this.textBox_dcc);
            this.panel1.Controls.Add(this.textBox_coo);
            this.panel1.Controls.Add(this.textBox_wfrLot);
            this.panel1.Controls.Add(this.textBox_LotType);
            this.panel1.Controls.Add(this.textBox_rvcdate);
            this.panel1.Controls.Add(this.textBox_device);
            this.panel1.Controls.Add(this.textBox_lotno);
            this.panel1.Controls.Add(this.textBox_amkorid);
            this.panel1.Controls.Add(this.textBox_billno);
            this.panel1.Controls.Add(this.textBox_wfrqty);
            this.panel1.Controls.Add(this.textBox_dieqty);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.textBox_cust);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label_dcc);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 433);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox_bcr
            // 
            this.pictureBox_bcr.Location = new System.Drawing.Point(953, 273);
            this.pictureBox_bcr.Name = "pictureBox_bcr";
            this.pictureBox_bcr.Size = new System.Drawing.Size(180, 132);
            this.pictureBox_bcr.TabIndex = 2;
            this.pictureBox_bcr.TabStop = false;
            // 
            // textBox_dcc
            // 
            this.textBox_dcc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_dcc.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_dcc.Location = new System.Drawing.Point(184, 123);
            this.textBox_dcc.Name = "textBox_dcc";
            this.textBox_dcc.Size = new System.Drawing.Size(141, 43);
            this.textBox_dcc.TabIndex = 3;
            this.textBox_dcc.Text = "01";
            this.textBox_dcc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_dcc_KeyPress);
            // 
            // textBox_coo
            // 
            this.textBox_coo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_coo.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_coo.Location = new System.Drawing.Point(184, 371);
            this.textBox_coo.Name = "textBox_coo";
            this.textBox_coo.Size = new System.Drawing.Size(303, 43);
            this.textBox_coo.TabIndex = 5;
            this.textBox_coo.Text = "XX";
            this.textBox_coo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_rvcdate_KeyPress);
            // 
            // textBox_wfrLot
            // 
            this.textBox_wfrLot.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_wfrLot.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_wfrLot.Location = new System.Drawing.Point(184, 322);
            this.textBox_wfrLot.Name = "textBox_wfrLot";
            this.textBox_wfrLot.Size = new System.Drawing.Size(303, 43);
            this.textBox_wfrLot.TabIndex = 5;
            this.textBox_wfrLot.Text = "XXXXX";
            this.textBox_wfrLot.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_rvcdate_KeyPress);
            // 
            // textBox_LotType
            // 
            this.textBox_LotType.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_LotType.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_LotType.Location = new System.Drawing.Point(184, 273);
            this.textBox_LotType.Name = "textBox_LotType";
            this.textBox_LotType.Size = new System.Drawing.Size(303, 43);
            this.textBox_LotType.TabIndex = 5;
            this.textBox_LotType.Text = "PR";
            this.textBox_LotType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_rvcdate_KeyPress);
            // 
            // textBox_rvcdate
            // 
            this.textBox_rvcdate.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_rvcdate.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_rvcdate.Location = new System.Drawing.Point(184, 224);
            this.textBox_rvcdate.Name = "textBox_rvcdate";
            this.textBox_rvcdate.Size = new System.Drawing.Size(303, 43);
            this.textBox_rvcdate.TabIndex = 5;
            this.textBox_rvcdate.Text = "2020/09/23";
            this.textBox_rvcdate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_rvcdate_KeyPress);
            // 
            // textBox_device
            // 
            this.textBox_device.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_device.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_device.Location = new System.Drawing.Point(184, 173);
            this.textBox_device.Name = "textBox_device";
            this.textBox_device.Size = new System.Drawing.Size(418, 43);
            this.textBox_device.TabIndex = 4;
            this.textBox_device.Text = "4HD8-BASE-TR1C";
            this.textBox_device.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_device_KeyPress);
            // 
            // textBox_lotno
            // 
            this.textBox_lotno.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_lotno.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_lotno.Location = new System.Drawing.Point(184, 73);
            this.textBox_lotno.Name = "textBox_lotno";
            this.textBox_lotno.Size = new System.Drawing.Size(303, 43);
            this.textBox_lotno.TabIndex = 2;
            this.textBox_lotno.Text = "DH220P204-05";
            this.textBox_lotno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_lotno_KeyPress);
            // 
            // textBox_amkorid
            // 
            this.textBox_amkorid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_amkorid.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_amkorid.Location = new System.Drawing.Point(809, 174);
            this.textBox_amkorid.Name = "textBox_amkorid";
            this.textBox_amkorid.Size = new System.Drawing.Size(373, 43);
            this.textBox_amkorid.TabIndex = 8;
            this.textBox_amkorid.Text = "123456";
            this.textBox_amkorid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_amkorid_KeyPress);
            // 
            // textBox_billno
            // 
            this.textBox_billno.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_billno.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_billno.Location = new System.Drawing.Point(809, 123);
            this.textBox_billno.Name = "textBox_billno";
            this.textBox_billno.Size = new System.Drawing.Size(373, 43);
            this.textBox_billno.TabIndex = 8;
            this.textBox_billno.Text = "6996266804";
            this.textBox_billno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_billno_KeyPress);
            // 
            // textBox_wfrqty
            // 
            this.textBox_wfrqty.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_wfrqty.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_wfrqty.Location = new System.Drawing.Point(809, 73);
            this.textBox_wfrqty.Name = "textBox_wfrqty";
            this.textBox_wfrqty.Size = new System.Drawing.Size(161, 43);
            this.textBox_wfrqty.TabIndex = 7;
            this.textBox_wfrqty.Text = "2";
            this.textBox_wfrqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_wfrqty_KeyPress);
            // 
            // textBox_dieqty
            // 
            this.textBox_dieqty.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_dieqty.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_dieqty.Location = new System.Drawing.Point(809, 23);
            this.textBox_dieqty.Name = "textBox_dieqty";
            this.textBox_dieqty.Size = new System.Drawing.Size(161, 43);
            this.textBox_dieqty.TabIndex = 6;
            this.textBox_dieqty.Text = "16268";
            this.textBox_dieqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_dieqty_KeyPress);
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(18, 364);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(160, 50);
            this.label11.TabIndex = 0;
            this.label11.Text = "COO :";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_cust
            // 
            this.textBox_cust.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox_cust.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_cust.Location = new System.Drawing.Point(184, 23);
            this.textBox_cust.Name = "textBox_cust";
            this.textBox_cust.Size = new System.Drawing.Size(141, 43);
            this.textBox_cust.TabIndex = 1;
            this.textBox_cust.Text = "488";
            this.textBox_cust.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_cust_KeyPress);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(18, 315);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(160, 50);
            this.label10.TabIndex = 0;
            this.label10.Text = "WFR LOT# :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_dcc
            // 
            this.label_dcc.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_dcc.Location = new System.Drawing.Point(18, 116);
            this.label_dcc.Name = "label_dcc";
            this.label_dcc.Size = new System.Drawing.Size(160, 50);
            this.label_dcc.TabIndex = 0;
            this.label_dcc.Text = "DCC :";
            this.label_dcc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(18, 266);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(160, 50);
            this.label9.TabIndex = 0;
            this.label9.Text = "LOT TYPE :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(643, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 50);
            this.label5.TabIndex = 0;
            this.label5.Text = "Amkor ID :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(18, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 50);
            this.label4.TabIndex = 0;
            this.label4.Text = "RCV-DATE :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(643, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 50);
            this.label8.TabIndex = 0;
            this.label8.Text = "BILL # :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(643, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 50);
            this.label7.TabIndex = 0;
            this.label7.Text = "WFR QTY :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(18, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 50);
            this.label3.TabIndex = 0;
            this.label3.Text = "DEVICE :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(643, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(160, 50);
            this.label6.TabIndex = 0;
            this.label6.Text = "DIE QTY :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(18, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 50);
            this.label2.TabIndex = 0;
            this.label2.Text = "LOT# :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(18, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "CUST :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_Close
            // 
            this.button_Close.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Close.Location = new System.Drawing.Point(878, 451);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(334, 152);
            this.button_Close.TabIndex = 1;
            this.button_Close.Text = "취소 (닫기)";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(538, 451);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(334, 152);
            this.button1.TabIndex = 1;
            this.button1.Text = "출력";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_Print_Click);
            // 
            // label_printname
            // 
            this.label_printname.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_printname.ForeColor = System.Drawing.Color.DarkBlue;
            this.label_printname.Location = new System.Drawing.Point(12, 451);
            this.label_printname.Name = "label_printname";
            this.label_printname.Size = new System.Drawing.Size(504, 50);
            this.label_printname.TabIndex = 0;
            this.label_printname.Text = "프린트 이름";
            this.label_printname.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Printuse
            // 
            this.label_Printuse.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Printuse.ForeColor = System.Drawing.Color.DarkBlue;
            this.label_Printuse.Location = new System.Drawing.Point(12, 501);
            this.label_Printuse.Name = "label_Printuse";
            this.label_Printuse.Size = new System.Drawing.Size(504, 50);
            this.label_Printuse.TabIndex = 0;
            this.label_Printuse.Text = "프린트 이름";
            this.label_Printuse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox_receivedata
            // 
            this.textBox_receivedata.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_receivedata.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_receivedata.Location = new System.Drawing.Point(0, 609);
            this.textBox_receivedata.Name = "textBox_receivedata";
            this.textBox_receivedata.ReadOnly = true;
            this.textBox_receivedata.Size = new System.Drawing.Size(1225, 35);
            this.textBox_receivedata.TabIndex = 2;
            // 
            // label_state
            // 
            this.label_state.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_state.ForeColor = System.Drawing.Color.DarkBlue;
            this.label_state.Location = new System.Drawing.Point(12, 556);
            this.label_state.Name = "label_state";
            this.label_state.Size = new System.Drawing.Size(504, 50);
            this.label_state.TabIndex = 0;
            this.label_state.Text = "-";
            this.label_state.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1225, 644);
            this.ControlBox = false;
            this.Controls.Add(this.textBox_receivedata);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label_state);
            this.Controls.Add(this.label_Printuse);
            this.Controls.Add(this.label_printname);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Print";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_Print";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Print_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_bcr)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox_dcc;
        private System.Windows.Forms.TextBox textBox_rvcdate;
        private System.Windows.Forms.TextBox textBox_device;
        private System.Windows.Forms.TextBox textBox_lotno;
        private System.Windows.Forms.TextBox textBox_billno;
        private System.Windows.Forms.TextBox textBox_wfrqty;
        private System.Windows.Forms.TextBox textBox_dieqty;
        private System.Windows.Forms.TextBox textBox_cust;
        private System.Windows.Forms.Label label_dcc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.PictureBox pictureBox_bcr;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label_printname;
        private System.Windows.Forms.Label label_Printuse;
        private System.Windows.Forms.TextBox textBox_amkorid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_coo;
        private System.Windows.Forms.TextBox textBox_wfrLot;
        private System.Windows.Forms.TextBox textBox_LotType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox_receivedata;
        private System.Windows.Forms.Label label_state;
    }
}