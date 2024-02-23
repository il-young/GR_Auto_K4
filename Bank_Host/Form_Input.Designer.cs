namespace Bank_Host
{
    partial class Form_Input
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_sid = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_cust = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView_bill = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_bill = new System.Windows.Forms.TextBox();
            this.button_complete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_bill)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SlateGray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(631, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "정보를 입력 하십시오";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_sid
            // 
            this.textBox_sid.Font = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_sid.Location = new System.Drawing.Point(148, 59);
            this.textBox_sid.Name = "textBox_sid";
            this.textBox_sid.PasswordChar = '*';
            this.textBox_sid.Size = new System.Drawing.Size(457, 50);
            this.textBox_sid.TabIndex = 1;
            this.textBox_sid.Text = "176524";
            this.textBox_sid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_sid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_sid_KeyPress);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(13, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 45);
            this.label2.TabIndex = 2;
            this.label2.Text = "사번 입력";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(13, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 45);
            this.label3.TabIndex = 2;
            this.label3.Text = "고객 선택";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_cust
            // 
            this.comboBox_cust.Font = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.comboBox_cust.FormattingEnabled = true;
            this.comboBox_cust.Location = new System.Drawing.Point(148, 117);
            this.comboBox_cust.Name = "comboBox_cust";
            this.comboBox_cust.Size = new System.Drawing.Size(457, 53);
            this.comboBox_cust.TabIndex = 2;
            this.comboBox_cust.SelectedIndexChanged += new System.EventHandler(this.comboBox_cust_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(13, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 45);
            this.label4.TabIndex = 2;
            this.label4.Text = "Bill # 선택";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataGridView_bill
            // 
            this.dataGridView_bill.AllowUserToAddRows = false;
            this.dataGridView_bill.AllowUserToDeleteRows = false;
            this.dataGridView_bill.AllowUserToResizeColumns = false;
            this.dataGridView_bill.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LavenderBlush;
            this.dataGridView_bill.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_bill.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_bill.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_bill.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_bill.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_bill.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_bill.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_bill.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_bill.Location = new System.Drawing.Point(148, 179);
            this.dataGridView_bill.MultiSelect = false;
            this.dataGridView_bill.Name = "dataGridView_bill";
            this.dataGridView_bill.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_bill.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_bill.RowHeadersVisible = false;
            this.dataGridView_bill.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView_bill.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView_bill.RowTemplate.Height = 30;
            this.dataGridView_bill.Size = new System.Drawing.Size(457, 206);
            this.dataGridView_bill.TabIndex = 3;
            this.dataGridView_bill.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_bill_CellClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(127, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItem1.Text = "강제 선택";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(13, 397);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 45);
            this.label5.TabIndex = 2;
            this.label5.Text = "Bill # 입력";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_bill
            // 
            this.textBox_bill.Font = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_bill.Location = new System.Drawing.Point(148, 394);
            this.textBox_bill.Name = "textBox_bill";
            this.textBox_bill.Size = new System.Drawing.Size(457, 50);
            this.textBox_bill.TabIndex = 4;
            this.textBox_bill.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_bill.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_bill_KeyPress);
            // 
            // button_complete
            // 
            this.button_complete.Location = new System.Drawing.Point(355, 453);
            this.button_complete.Name = "button_complete";
            this.button_complete.Size = new System.Drawing.Size(250, 85);
            this.button_complete.TabIndex = 5;
            this.button_complete.Text = "입력 완료";
            this.button_complete.UseVisualStyleBackColor = true;
            this.button_complete.Click += new System.EventHandler(this.button_complete_Click);
            // 
            // Form_Input
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(631, 546);
            this.Controls.Add(this.button_complete);
            this.Controls.Add(this.dataGridView_bill);
            this.Controls.Add(this.comboBox_cust);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_bill);
            this.Controls.Add(this.textBox_sid);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Input";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Input_FormClosing);
            this.Load += new System.EventHandler(this.Form_Input_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_bill)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_sid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_cust;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView_bill;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_bill;
        private System.Windows.Forms.Button button_complete;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}