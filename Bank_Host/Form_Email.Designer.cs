namespace Bank_Host
{
    partial class Form_Email
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_To = new System.Windows.Forms.TextBox();
            this.textBox_Cc = new System.Windows.Forms.TextBox();
            this.textBox_Msg = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.textBox_Subject = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "받는 사람";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(53, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "참조";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(53, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "제목";
            // 
            // textBox_To
            // 
            this.textBox_To.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_To.Location = new System.Drawing.Point(100, 9);
            this.textBox_To.Name = "textBox_To";
            this.textBox_To.Size = new System.Drawing.Size(718, 29);
            this.textBox_To.TabIndex = 1;
            // 
            // textBox_Cc
            // 
            this.textBox_Cc.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Cc.Location = new System.Drawing.Point(100, 43);
            this.textBox_Cc.Name = "textBox_Cc";
            this.textBox_Cc.Size = new System.Drawing.Size(718, 29);
            this.textBox_Cc.TabIndex = 1;
            // 
            // textBox_Msg
            // 
            this.textBox_Msg.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Msg.Location = new System.Drawing.Point(8, 118);
            this.textBox_Msg.Multiline = true;
            this.textBox_Msg.Name = "textBox_Msg";
            this.textBox_Msg.Size = new System.Drawing.Size(822, 367);
            this.textBox_Msg.TabIndex = 1;
            // 
            // button_send
            // 
            this.button_send.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_send.Location = new System.Drawing.Point(671, 491);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(159, 54);
            this.button_send.TabIndex = 2;
            this.button_send.Text = "보내기";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // textBox_Subject
            // 
            this.textBox_Subject.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Subject.Location = new System.Drawing.Point(100, 78);
            this.textBox_Subject.Name = "textBox_Subject";
            this.textBox_Subject.Size = new System.Drawing.Size(718, 29);
            this.textBox_Subject.TabIndex = 1;
            // 
            // Form_Email
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(837, 553);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.textBox_Msg);
            this.Controls.Add(this.textBox_Subject);
            this.Controls.Add(this.textBox_Cc);
            this.Controls.Add(this.textBox_To);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Email";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "이메일 전송";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Email_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_To;
        private System.Windows.Forms.TextBox textBox_Cc;
        private System.Windows.Forms.TextBox textBox_Msg;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.TextBox textBox_Subject;
    }
}