namespace Bank_Host
{
    partial class frm_LoactionLabel
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_scan = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btn_Print = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rb_Single = new System.Windows.Forms.RadioButton();
            this.rb_con = new System.Windows.Forms.RadioButton();
            this.rb_Copy = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_2 = new System.Windows.Forms.TextBox();
            this.tb_1 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rb_typing = new System.Windows.Forms.RadioButton();
            this.rb_scan = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rb_2 = new System.Windows.Forms.RadioButton();
            this.rb_1 = new System.Windows.Forms.RadioButton();
            this.tb_Text2 = new System.Windows.Forms.TextBox();
            this.btn_PrintText = new System.Windows.Forms.Button();
            this.tb_Text1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_scan);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.btn_Print);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_2);
            this.groupBox1.Controls.Add(this.tb_1);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 411);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 319);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 21);
            this.label3.TabIndex = 10;
            this.label3.Text = "Font Size :";
            // 
            // tb_scan
            // 
            this.tb_scan.Enabled = false;
            this.tb_scan.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_scan.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.tb_scan.Location = new System.Drawing.Point(10, 176);
            this.tb_scan.Name = "tb_scan";
            this.tb_scan.Size = new System.Drawing.Size(347, 22);
            this.tb_scan.TabIndex = 6;
            this.tb_scan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_scan_KeyDown);
            this.tb_scan.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tb_scan_MouseDown);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(157, 317);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            190,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 32);
            this.numericUpDown1.TabIndex = 9;
            this.numericUpDown1.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // btn_Print
            // 
            this.btn_Print.BackColor = System.Drawing.Color.Chartreuse;
            this.btn_Print.Location = new System.Drawing.Point(269, 374);
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Size = new System.Drawing.Size(88, 31);
            this.btn_Print.TabIndex = 5;
            this.btn_Print.Text = "Print";
            this.btn_Print.UseVisualStyleBackColor = false;
            this.btn_Print.Click += new System.EventHandler(this.btn_Print_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rb_Single);
            this.groupBox4.Controls.Add(this.rb_con);
            this.groupBox4.Controls.Add(this.rb_Copy);
            this.groupBox4.Location = new System.Drawing.Point(10, 109);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(347, 61);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Print Mode";
            // 
            // rb_Single
            // 
            this.rb_Single.AutoSize = true;
            this.rb_Single.Checked = true;
            this.rb_Single.Location = new System.Drawing.Point(227, 31);
            this.rb_Single.Name = "rb_Single";
            this.rb_Single.Size = new System.Drawing.Size(84, 25);
            this.rb_Single.TabIndex = 2;
            this.rb_Single.TabStop = true;
            this.rb_Single.Text = "Single";
            this.rb_Single.UseVisualStyleBackColor = true;
            // 
            // rb_con
            // 
            this.rb_con.AutoSize = true;
            this.rb_con.Location = new System.Drawing.Point(89, 30);
            this.rb_con.Name = "rb_con";
            this.rb_con.Size = new System.Drawing.Size(132, 25);
            this.rb_con.TabIndex = 1;
            this.rb_con.Text = "Continuous";
            this.rb_con.UseVisualStyleBackColor = true;
            // 
            // rb_Copy
            // 
            this.rb_Copy.AutoSize = true;
            this.rb_Copy.Location = new System.Drawing.Point(6, 30);
            this.rb_Copy.Name = "rb_Copy";
            this.rb_Copy.Size = new System.Drawing.Size(77, 25);
            this.rb_Copy.TabIndex = 0;
            this.rb_Copy.Text = "Copy";
            this.rb_Copy.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 251);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = ":";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_2
            // 
            this.tb_2.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.tb_2.Location = new System.Drawing.Point(177, 248);
            this.tb_2.Name = "tb_2";
            this.tb_2.Size = new System.Drawing.Size(100, 32);
            this.tb_2.TabIndex = 3;
            this.tb_2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_2_KeyDown);
            this.tb_2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tb_2_MouseDown);
            // 
            // tb_1
            // 
            this.tb_1.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.tb_1.Location = new System.Drawing.Point(43, 248);
            this.tb_1.Name = "tb_1";
            this.tb_1.Size = new System.Drawing.Size(100, 32);
            this.tb_1.TabIndex = 2;
            this.tb_1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_1_KeyDown);
            this.tb_1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tb_1_MouseDown);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rb_typing);
            this.groupBox3.Controls.Add(this.rb_scan);
            this.groupBox3.Location = new System.Drawing.Point(10, 31);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(347, 61);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Input Mode";
            // 
            // rb_typing
            // 
            this.rb_typing.AutoSize = true;
            this.rb_typing.Checked = true;
            this.rb_typing.Location = new System.Drawing.Point(188, 30);
            this.rb_typing.Name = "rb_typing";
            this.rb_typing.Size = new System.Drawing.Size(90, 25);
            this.rb_typing.TabIndex = 1;
            this.rb_typing.TabStop = true;
            this.rb_typing.Text = "Typing";
            this.rb_typing.UseVisualStyleBackColor = true;
            this.rb_typing.CheckedChanged += new System.EventHandler(this.rb_typing_CheckedChanged);
            // 
            // rb_scan
            // 
            this.rb_scan.AutoSize = true;
            this.rb_scan.Location = new System.Drawing.Point(33, 30);
            this.rb_scan.Name = "rb_scan";
            this.rb_scan.Size = new System.Drawing.Size(76, 25);
            this.rb_scan.TabIndex = 0;
            this.rb_scan.Text = "Scan";
            this.rb_scan.UseVisualStyleBackColor = true;
            this.rb_scan.CheckedChanged += new System.EventHandler(this.rb_scan_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Code : ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.numericUpDown2);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.tb_Text2);
            this.groupBox2.Controls.Add(this.btn_PrintText);
            this.groupBox2.Controls.Add(this.tb_Text1);
            this.groupBox2.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(381, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 411);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Text";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rb_2);
            this.groupBox5.Controls.Add(this.rb_1);
            this.groupBox5.Location = new System.Drawing.Point(10, 31);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(347, 61);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Line";
            // 
            // rb_2
            // 
            this.rb_2.AutoSize = true;
            this.rb_2.Location = new System.Drawing.Point(188, 30);
            this.rb_2.Name = "rb_2";
            this.rb_2.Size = new System.Drawing.Size(40, 25);
            this.rb_2.TabIndex = 1;
            this.rb_2.Text = "2";
            this.rb_2.UseVisualStyleBackColor = true;
            this.rb_2.CheckedChanged += new System.EventHandler(this.rb_2_CheckedChanged);
            // 
            // rb_1
            // 
            this.rb_1.AutoSize = true;
            this.rb_1.Checked = true;
            this.rb_1.Location = new System.Drawing.Point(33, 30);
            this.rb_1.Name = "rb_1";
            this.rb_1.Size = new System.Drawing.Size(40, 25);
            this.rb_1.TabIndex = 0;
            this.rb_1.TabStop = true;
            this.rb_1.Text = "1";
            this.rb_1.UseVisualStyleBackColor = true;
            this.rb_1.CheckedChanged += new System.EventHandler(this.rb_1_CheckedChanged);
            // 
            // tb_Text2
            // 
            this.tb_Text2.Enabled = false;
            this.tb_Text2.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.tb_Text2.Location = new System.Drawing.Point(6, 169);
            this.tb_Text2.Name = "tb_Text2";
            this.tb_Text2.Size = new System.Drawing.Size(351, 32);
            this.tb_Text2.TabIndex = 7;
            this.tb_Text2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_Text2_KeyDown);
            this.tb_Text2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tb_Text2_MouseDown);
            // 
            // btn_PrintText
            // 
            this.btn_PrintText.BackColor = System.Drawing.Color.Chartreuse;
            this.btn_PrintText.Location = new System.Drawing.Point(269, 374);
            this.btn_PrintText.Name = "btn_PrintText";
            this.btn_PrintText.Size = new System.Drawing.Size(88, 31);
            this.btn_PrintText.TabIndex = 6;
            this.btn_PrintText.Text = "Print";
            this.btn_PrintText.UseVisualStyleBackColor = false;
            this.btn_PrintText.Click += new System.EventHandler(this.btn_PrintText_Click);
            // 
            // tb_Text1
            // 
            this.tb_Text1.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.tb_Text1.Location = new System.Drawing.Point(6, 109);
            this.tb_Text1.Name = "tb_Text1";
            this.tb_Text1.Size = new System.Drawing.Size(351, 32);
            this.tb_Text1.TabIndex = 0;
            this.tb_Text1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_Text1_MouseClick);
            this.tb_Text1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_Text1_KeyDown);
            this.tb_Text1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tb_Text1_MouseDown);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::Bank_Host.Properties.Resources.close_icon_13602;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Location = new System.Drawing.Point(738, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 25);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 317);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 21);
            this.label4.TabIndex = 12;
            this.label4.Text = "Font Size :";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown2.Location = new System.Drawing.Point(183, 315);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            190,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 32);
            this.numericUpDown2.TabIndex = 11;
            this.numericUpDown2.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // frm_LoactionLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 438);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_LoactionLabel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frm_LoactionLabel";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frm_LoactionLabel_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_2;
        private System.Windows.Forms.TextBox tb_1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rb_typing;
        private System.Windows.Forms.RadioButton rb_scan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rb_con;
        private System.Windows.Forms.RadioButton rb_Copy;
        private System.Windows.Forms.TextBox tb_scan;
        private System.Windows.Forms.Button btn_Print;
        private System.Windows.Forms.RadioButton rb_Single;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rb_2;
        private System.Windows.Forms.RadioButton rb_1;
        private System.Windows.Forms.TextBox tb_Text2;
        private System.Windows.Forms.Button btn_PrintText;
        private System.Windows.Forms.TextBox tb_Text1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
    }
}