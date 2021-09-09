namespace Bank_Host
{
    partial class Form_Keynece
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
            this.textBox_command = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_state = new System.Windows.Forms.Label();
            this.button_send = new System.Windows.Forms.Button();
            this.button_LON = new System.Windows.Forms.Button();
            this.button_LOFF = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_receivedata = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_Save = new System.Windows.Forms.Button();
            this.button_load = new System.Windows.Forms.Button();
            this.textBox_No = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_command
            // 
            this.textBox_command.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_command.Location = new System.Drawing.Point(149, 54);
            this.textBox_command.Name = "textBox_command";
            this.textBox_command.Size = new System.Drawing.Size(365, 35);
            this.textBox_command.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Gray;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(14, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "접속 상태";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Gray;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(14, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 37);
            this.label2.TabIndex = 0;
            this.label2.Text = "명령";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_state
            // 
            this.label_state.BackColor = System.Drawing.Color.Gray;
            this.label_state.ForeColor = System.Drawing.Color.White;
            this.label_state.Location = new System.Drawing.Point(149, 13);
            this.label_state.Name = "label_state";
            this.label_state.Size = new System.Drawing.Size(191, 37);
            this.label_state.TabIndex = 0;
            this.label_state.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(515, 53);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(86, 36);
            this.button_send.TabIndex = 2;
            this.button_send.Text = "전송";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // button_LON
            // 
            this.button_LON.Location = new System.Drawing.Point(149, 93);
            this.button_LON.Name = "button_LON";
            this.button_LON.Size = new System.Drawing.Size(135, 39);
            this.button_LON.TabIndex = 2;
            this.button_LON.Text = "LON";
            this.button_LON.UseVisualStyleBackColor = true;
            this.button_LON.Click += new System.EventHandler(this.button_LON_Click);
            // 
            // button_LOFF
            // 
            this.button_LOFF.Location = new System.Drawing.Point(287, 93);
            this.button_LOFF.Name = "button_LOFF";
            this.button_LOFF.Size = new System.Drawing.Size(135, 39);
            this.button_LOFF.TabIndex = 2;
            this.button_LOFF.Text = "LOFF";
            this.button_LOFF.UseVisualStyleBackColor = true;
            this.button_LOFF.Click += new System.EventHandler(this.button_LOFF_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Gray;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(14, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 37);
            this.label4.TabIndex = 0;
            this.label4.Text = "Receive Data";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_receivedata
            // 
            this.textBox_receivedata.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_receivedata.Location = new System.Drawing.Point(149, 138);
            this.textBox_receivedata.Multiline = true;
            this.textBox_receivedata.Name = "textBox_receivedata";
            this.textBox_receivedata.ReadOnly = true;
            this.textBox_receivedata.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_receivedata.Size = new System.Drawing.Size(491, 230);
            this.textBox_receivedata.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(149, 379);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(135, 39);
            this.button_Save.TabIndex = 2;
            this.button_Save.Text = "SAVE";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_load
            // 
            this.button_load.Location = new System.Drawing.Point(149, 424);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(135, 39);
            this.button_load.TabIndex = 2;
            this.button_load.Text = "LOAD";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // textBox_No
            // 
            this.textBox_No.Location = new System.Drawing.Point(31, 381);
            this.textBox_No.Name = "textBox_No";
            this.textBox_No.Size = new System.Drawing.Size(97, 29);
            this.textBox_No.TabIndex = 4;
            // 
            // Form_Keynece
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(666, 474);
            this.Controls.Add(this.textBox_No);
            this.Controls.Add(this.textBox_receivedata);
            this.Controls.Add(this.button_load);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.button_LOFF);
            this.Controls.Add(this.button_LON);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.textBox_command);
            this.Controls.Add(this.label_state);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Keynece";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form_Keynece";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_command;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_state;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Button button_LON;
        private System.Windows.Forms.Button button_LOFF;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_receivedata;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.TextBox textBox_No;
    }
}