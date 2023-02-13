namespace Bank_Host
{
    partial class Form_ScrapComment
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv_Comment = new System.Windows.Forms.DataGridView();
            this.gR_AutomationDataSet = new Bank_Host.GR_AutomationDataSet();
            this.tBSCRAPCOMMENTBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tB_SCRAP_COMMENTTableAdapter = new Bank_Host.GR_AutomationDataSetTableAdapters.TB_SCRAP_COMMENTTableAdapter();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Plant = new System.Windows.Forms.TextBox();
            this.tb_CustCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tb_Comment = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Edit = new System.Windows.Forms.Button();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Insert = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Comment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gR_AutomationDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBSCRAPCOMMENTBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btn_Delete);
            this.panel4.Controls.Add(this.btn_Edit);
            this.panel4.Controls.Add(this.btn_Insert);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 398);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(800, 52);
            this.panel4.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tb_CustCode);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.tb_Plant);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 89);
            this.panel2.TabIndex = 1;
            // 
            // dgv_Comment
            // 
            this.dgv_Comment.AllowUserToAddRows = false;
            this.dgv_Comment.AllowUserToDeleteRows = false;
            this.dgv_Comment.AllowUserToResizeColumns = false;
            this.dgv_Comment.AllowUserToResizeRows = false;
            this.dgv_Comment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Comment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Select,
            this.Column2,
            this.Column6,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dgv_Comment.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgv_Comment.Location = new System.Drawing.Point(0, 89);
            this.dgv_Comment.Name = "dgv_Comment";
            this.dgv_Comment.RowTemplate.Height = 23;
            this.dgv_Comment.Size = new System.Drawing.Size(800, 227);
            this.dgv_Comment.TabIndex = 2;
            this.dgv_Comment.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Comment_CellClick);
            // 
            // gR_AutomationDataSet
            // 
            this.gR_AutomationDataSet.DataSetName = "GR_AutomationDataSet";
            this.gR_AutomationDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tBSCRAPCOMMENTBindingSource
            // 
            this.tBSCRAPCOMMENTBindingSource.DataMember = "TB_SCRAP_COMMENT";
            this.tBSCRAPCOMMENTBindingSource.DataSource = this.gR_AutomationDataSet;
            // 
            // tB_SCRAP_COMMENTTableAdapter
            // 
            this.tB_SCRAP_COMMENTTableAdapter.ClearBeforeFill = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Plant :";
            // 
            // tb_Plant
            // 
            this.tb_Plant.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_Plant.Location = new System.Drawing.Point(133, 6);
            this.tb_Plant.Name = "tb_Plant";
            this.tb_Plant.Size = new System.Drawing.Size(67, 32);
            this.tb_Plant.TabIndex = 1;
            this.tb_Plant.Text = "K4";
            this.tb_Plant.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_CustCode
            // 
            this.tb_CustCode.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_CustCode.Location = new System.Drawing.Point(133, 44);
            this.tb_CustCode.Name = "tb_CustCode";
            this.tb_CustCode.Size = new System.Drawing.Size(67, 32);
            this.tb_CustCode.TabIndex = 3;
            this.tb_CustCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Customer :";
            // 
            // Select
            // 
            this.Select.FillWeight = 50F;
            this.Select.Frozen = true;
            this.Select.HeaderText = "Select";
            this.Select.Name = "Select";
            this.Select.Width = 50;
            // 
            // Column2
            // 
            this.Column2.FillWeight = 70F;
            this.Column2.Frozen = true;
            this.Column2.HeaderText = "Customer";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 80;
            // 
            // Column6
            // 
            this.Column6.FillWeight = 90F;
            this.Column6.Frozen = true;
            this.Column6.HeaderText = "Sequent No";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.Frozen = true;
            this.Column3.HeaderText = "Customer Commnet";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 300;
            // 
            // Column4
            // 
            this.Column4.FillWeight = 70F;
            this.Column4.Frozen = true;
            this.Column4.HeaderText = "Entry Date";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.Frozen = true;
            this.Column5.HeaderText = "Entry ID";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // tb_Comment
            // 
            this.tb_Comment.Dock = System.Windows.Forms.DockStyle.Right;
            this.tb_Comment.Location = new System.Drawing.Point(133, 316);
            this.tb_Comment.Multiline = true;
            this.tb_Comment.Name = "tb_Comment";
            this.tb_Comment.Size = new System.Drawing.Size(667, 82);
            this.tb_Comment.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(12, 352);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "Comment :";
            // 
            // btn_Edit
            // 
            this.btn_Edit.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Edit.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Edit.Location = new System.Drawing.Point(75, 0);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(75, 52);
            this.btn_Edit.TabIndex = 1;
            this.btn_Edit.Text = "Edit";
            this.btn_Edit.UseVisualStyleBackColor = true;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Delete.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Delete.Location = new System.Drawing.Point(150, 0);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(75, 52);
            this.btn_Delete.TabIndex = 2;
            this.btn_Delete.Text = "Delete";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Insert
            // 
            this.btn_Insert.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Insert.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Insert.Location = new System.Drawing.Point(0, 0);
            this.btn_Insert.Name = "btn_Insert";
            this.btn_Insert.Size = new System.Drawing.Size(75, 52);
            this.btn_Insert.TabIndex = 0;
            this.btn_Insert.Text = "Insert";
            this.btn_Insert.UseVisualStyleBackColor = true;
            this.btn_Insert.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form_ScrapComment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_Comment);
            this.Controls.Add(this.dgv_Comment);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Name = "Form_ScrapComment";
            this.Text = "Comment Table Edit Form";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_ScrapComment_Load);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Comment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gR_AutomationDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBSCRAPCOMMENTBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgv_Comment;
        private GR_AutomationDataSet gR_AutomationDataSet;
        private System.Windows.Forms.BindingSource tBSCRAPCOMMENTBindingSource;
        private GR_AutomationDataSetTableAdapters.TB_SCRAP_COMMENTTableAdapter tB_SCRAP_COMMENTTableAdapter;
        private System.Windows.Forms.TextBox tb_CustCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Plant;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Select;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.TextBox tb_Comment;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Edit;
        private System.Windows.Forms.Button btn_Insert;
    }
}