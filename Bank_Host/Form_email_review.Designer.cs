namespace Bank_Host
{
    partial class Form_email_review
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_email_review));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_sendmail = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tb_maillist = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_linecode = new System.Windows.Forms.ComboBox();
            this.dgv_splitlog_err_data = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tb_head = new System.Windows.Forms.TextBox();
            this.rtb_body = new System.Windows.Forms.RichTextBox();
            this.cb_msg = new System.Windows.Forms.ComboBox();
            this.rtb_tail = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_splitlog_err_data)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgv_splitlog_err_data);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 293;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rtb_tail);
            this.panel2.Controls.Add(this.cb_msg);
            this.panel2.Controls.Add(this.rtb_body);
            this.panel2.Controls.Add(this.tb_head);
            this.panel2.Controls.Add(this.btn_sendmail);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 250);
            this.panel2.TabIndex = 1;
            // 
            // btn_sendmail
            // 
            this.btn_sendmail.AutoEllipsis = true;
            this.btn_sendmail.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_sendmail.BackgroundImage")));
            this.btn_sendmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_sendmail.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_sendmail.ImageKey = "(없음)";
            this.btn_sendmail.Location = new System.Drawing.Point(701, 203);
            this.btn_sendmail.Name = "btn_sendmail";
            this.btn_sendmail.Size = new System.Drawing.Size(96, 44);
            this.btn_sendmail.TabIndex = 4;
            this.btn_sendmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_sendmail.UseVisualStyleBackColor = true;
            this.btn_sendmail.Click += new System.EventHandler(this.btn_sendmail_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tb_maillist);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cb_linecode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 43);
            this.panel1.TabIndex = 0;
            // 
            // tb_maillist
            // 
            this.tb_maillist.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_maillist.Location = new System.Drawing.Point(389, 7);
            this.tb_maillist.Name = "tb_maillist";
            this.tb_maillist.ReadOnly = true;
            this.tb_maillist.Size = new System.Drawing.Size(399, 29);
            this.tb_maillist.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(287, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mail List :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Linecode :";
            // 
            // cb_linecode
            // 
            this.cb_linecode.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_linecode.FormattingEnabled = true;
            this.cb_linecode.Location = new System.Drawing.Point(122, 9);
            this.cb_linecode.Name = "cb_linecode";
            this.cb_linecode.Size = new System.Drawing.Size(121, 27);
            this.cb_linecode.TabIndex = 0;
            this.cb_linecode.SelectedIndexChanged += new System.EventHandler(this.cb_linecode_SelectedIndexChanged);
            // 
            // dgv_splitlog_err_data
            // 
            this.dgv_splitlog_err_data.AllowUserToAddRows = false;
            this.dgv_splitlog_err_data.AllowUserToDeleteRows = false;
            this.dgv_splitlog_err_data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_splitlog_err_data.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dgv_splitlog_err_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_splitlog_err_data.Location = new System.Drawing.Point(0, 0);
            this.dgv_splitlog_err_data.Name = "dgv_splitlog_err_data";
            this.dgv_splitlog_err_data.ReadOnly = true;
            this.dgv_splitlog_err_data.RowHeadersVisible = false;
            this.dgv_splitlog_err_data.RowTemplate.Height = 23;
            this.dgv_splitlog_err_data.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_splitlog_err_data.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_splitlog_err_data.Size = new System.Drawing.Size(800, 153);
            this.dgv_splitlog_err_data.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column11111";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // tb_head
            // 
            this.tb_head.Dock = System.Windows.Forms.DockStyle.Top;
            this.tb_head.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_head.Location = new System.Drawing.Point(0, 0);
            this.tb_head.Name = "tb_head";
            this.tb_head.Size = new System.Drawing.Size(800, 32);
            this.tb_head.TabIndex = 0;
            this.tb_head.Text = "[DIEBANK]반납 자재 확인 바랍니다.";
            // 
            // rtb_body
            // 
            this.rtb_body.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtb_body.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rtb_body.Location = new System.Drawing.Point(0, 32);
            this.rtb_body.Name = "rtb_body";
            this.rtb_body.Size = new System.Drawing.Size(800, 76);
            this.rtb_body.TabIndex = 5;
            this.rtb_body.Text = "안녕하십니까?\n반납담당자님.\n";
            // 
            // cb_msg
            // 
            this.cb_msg.Dock = System.Windows.Forms.DockStyle.Top;
            this.cb_msg.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_msg.FormattingEnabled = true;
            this.cb_msg.Items.AddRange(new object[] {
            "첨부된 Lot는 전산 & 현품이 불일치하여 메일을 발송 하오니 확인 바랍니다.",
            "첨부된 Lot는 전산 반납이 안된 사항 이오니 뱅크로 전산을 넘겨주시기 바랍니다.",
            "첨부된 Lot는 전산 반납은 되었으나, 현품이 미반납 상태 이오니 확인 바랍니다."});
            this.cb_msg.Location = new System.Drawing.Point(0, 108);
            this.cb_msg.Name = "cb_msg";
            this.cb_msg.Size = new System.Drawing.Size(800, 29);
            this.cb_msg.TabIndex = 6;
            // 
            // rtb_tail
            // 
            this.rtb_tail.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtb_tail.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold);
            this.rtb_tail.Location = new System.Drawing.Point(0, 137);
            this.rtb_tail.Name = "rtb_tail";
            this.rtb_tail.Size = new System.Drawing.Size(800, 60);
            this.rtb_tail.TabIndex = 7;
            this.rtb_tail.Text = "감사합니다.";
            // 
            // Form_email_review
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_email_review";
            this.Text = "Email Review";
            this.Load += new System.EventHandler(this.Form_email_review_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_splitlog_err_data)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tb_maillist;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_linecode;
        private System.Windows.Forms.DataGridView dgv_splitlog_err_data;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Button btn_sendmail;
        private System.Windows.Forms.RichTextBox rtb_tail;
        private System.Windows.Forms.ComboBox cb_msg;
        private System.Windows.Forms.RichTextBox rtb_body;
        private System.Windows.Forms.TextBox tb_head;
    }
}