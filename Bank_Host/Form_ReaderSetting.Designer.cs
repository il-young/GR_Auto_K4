namespace Bank_Host
{
    partial class Form_ReaderSetting
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
            this.btn_close = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_cognex = new System.Windows.Forms.RadioButton();
            this.rb_Keyence = new System.Windows.Forms.RadioButton();
            this.tb_ip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_port = new System.Windows.Forms.MaskedTextBox();
            this.cb_web = new System.Windows.Forms.CheckBox();
            this.btn_Cognex = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_close
            // 
            this.btn_close.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_close.Location = new System.Drawing.Point(658, 102);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 29);
            this.btn_close.TabIndex = 0;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_cognex);
            this.groupBox1.Controls.Add(this.rb_Keyence);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(252, 69);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camera Type";
            // 
            // rb_cognex
            // 
            this.rb_cognex.AutoSize = true;
            this.rb_cognex.Location = new System.Drawing.Point(140, 25);
            this.rb_cognex.Name = "rb_cognex";
            this.rb_cognex.Size = new System.Drawing.Size(89, 20);
            this.rb_cognex.TabIndex = 1;
            this.rb_cognex.TabStop = true;
            this.rb_cognex.Text = "Cognex";
            this.rb_cognex.UseVisualStyleBackColor = true;
            this.rb_cognex.CheckedChanged += new System.EventHandler(this.rb_cognex_CheckedChanged);
            // 
            // rb_Keyence
            // 
            this.rb_Keyence.AutoSize = true;
            this.rb_Keyence.Location = new System.Drawing.Point(6, 25);
            this.rb_Keyence.Name = "rb_Keyence";
            this.rb_Keyence.Size = new System.Drawing.Size(96, 20);
            this.rb_Keyence.TabIndex = 0;
            this.rb_Keyence.TabStop = true;
            this.rb_Keyence.Text = "Keyence";
            this.rb_Keyence.UseVisualStyleBackColor = true;
            this.rb_Keyence.CheckedChanged += new System.EventHandler(this.rb_Keyence_CheckedChanged);
            // 
            // tb_ip
            // 
            this.tb_ip.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_ip.Location = new System.Drawing.Point(292, 40);
            this.tb_ip.Name = "tb_ip";
            this.tb_ip.Size = new System.Drawing.Size(137, 26);
            this.tb_ip.TabIndex = 2;
            this.tb_ip.Text = "192.168.100.2";
            this.tb_ip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_ip.TextChanged += new System.EventHandler(this.tb_ip_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(289, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(454, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "PORT :";
            // 
            // tb_port
            // 
            this.tb_port.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_port.Location = new System.Drawing.Point(457, 40);
            this.tb_port.Mask = "0000";
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(137, 26);
            this.tb_port.TabIndex = 6;
            this.tb_port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_port.TextChanged += new System.EventHandler(this.tb_port_TextChanged);
            // 
            // cb_web
            // 
            this.cb_web.AutoSize = true;
            this.cb_web.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_web.Location = new System.Drawing.Point(623, 40);
            this.cb_web.Name = "cb_web";
            this.cb_web.Size = new System.Drawing.Size(110, 23);
            this.cb_web.TabIndex = 7;
            this.cb_web.Text = "Webpage";
            this.cb_web.UseVisualStyleBackColor = true;
            this.cb_web.CheckedChanged += new System.EventHandler(this.cb_web_CheckedChanged);
            // 
            // btn_Cognex
            // 
            this.btn_Cognex.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Cognex.Location = new System.Drawing.Point(292, 108);
            this.btn_Cognex.Name = "btn_Cognex";
            this.btn_Cognex.Size = new System.Drawing.Size(110, 41);
            this.btn_Cognex.TabIndex = 8;
            this.btn_Cognex.Text = "Cognex";
            this.btn_Cognex.UseVisualStyleBackColor = true;
            this.btn_Cognex.Click += new System.EventHandler(this.btn_Cognex_Click);
            // 
            // Form_ReaderSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 161);
            this.Controls.Add(this.btn_Cognex);
            this.Controls.Add(this.cb_web);
            this.Controls.Add(this.tb_port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_ip);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_ReaderSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form_ReaderSetting";
            this.Load += new System.EventHandler(this.Form_ReaderSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_cognex;
        private System.Windows.Forms.RadioButton rb_Keyence;
        private System.Windows.Forms.TextBox tb_ip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox tb_port;
        private System.Windows.Forms.CheckBox cb_web;
        private System.Windows.Forms.Button btn_Cognex;
    }
}