namespace Bank_Host
{
    partial class Form_InfoBoard
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
            this.tb_MSG = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tb_MSG
            // 
            this.tb_MSG.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tb_MSG.Font = new System.Drawing.Font("굴림", 120F, System.Drawing.FontStyle.Bold);
            this.tb_MSG.ForeColor = System.Drawing.Color.LightSlateGray;
            this.tb_MSG.Location = new System.Drawing.Point(12, 39);
            this.tb_MSG.Multiline = true;
            this.tb_MSG.Name = "tb_MSG";
            this.tb_MSG.ReadOnly = true;
            this.tb_MSG.Size = new System.Drawing.Size(413, 174);
            this.tb_MSG.TabIndex = 0;
            this.tb_MSG.Text = "2222";
            this.tb_MSG.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_MSG.WordWrap = false;
            this.tb_MSG.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_MSG_MouseClick);
            // 
            // Form_InfoBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 250);
            this.ControlBox = false;
            this.Controls.Add(this.tb_MSG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_InfoBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_InfoBoard";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_InfoBoard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_MSG;
    }
}