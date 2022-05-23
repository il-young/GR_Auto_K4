namespace Bank_Host
{
    partial class Form_CommentSelecter
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
            this.cb_comment = new System.Windows.Forms.ComboBox();
            this.tb_comment = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cb_comment
            // 
            this.cb_comment.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_comment.FormattingEnabled = true;
            this.cb_comment.Location = new System.Drawing.Point(127, 12);
            this.cb_comment.Name = "cb_comment";
            this.cb_comment.Size = new System.Drawing.Size(153, 27);
            this.cb_comment.TabIndex = 0;
            this.cb_comment.SelectedIndexChanged += new System.EventHandler(this.cb_comment_SelectedIndexChanged);
            this.cb_comment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cb_comment_KeyDown);
            // 
            // tb_comment
            // 
            this.tb_comment.Location = new System.Drawing.Point(12, 45);
            this.tb_comment.Multiline = true;
            this.tb_comment.Name = "tb_comment";
            this.tb_comment.ReadOnly = true;
            this.tb_comment.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_comment.Size = new System.Drawing.Size(390, 111);
            this.tb_comment.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(327, 162);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 30);
            this.button1.TabIndex = 2;
            this.button1.Text = "O  K";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button2.Location = new System.Drawing.Point(12, 162);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 30);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form_CommentSelecter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 203);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_comment);
            this.Controls.Add(this.cb_comment);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_CommentSelecter";
            this.Text = "Comment Selecter";
            this.Load += new System.EventHandler(this.Form_CommentSelecter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_comment;
        private System.Windows.Forms.TextBox tb_comment;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}