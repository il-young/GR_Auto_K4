namespace Bank_Host
{
    partial class Form_Splitlog_Input
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
            this.label2 = new System.Windows.Forms.Label();
            this.tb_employee = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_cust = new System.Windows.Forms.ComboBox();
            this.cb_line_code = new System.Windows.Forms.ComboBox();
            this.tb_binding = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 45);
            this.label2.TabIndex = 3;
            this.label2.Text = "사번 입력  :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_employee
            // 
            this.tb_employee.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_employee.Location = new System.Drawing.Point(146, 27);
            this.tb_employee.Name = "tb_employee";
            this.tb_employee.Size = new System.Drawing.Size(177, 35);
            this.tb_employee.TabIndex = 4;
            this.tb_employee.Text = "396664";
            this.tb_employee.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_employee.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_employee_KeyDown);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 45);
            this.label1.TabIndex = 5;
            this.label1.Text = "Cust         :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(12, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 45);
            this.label3.TabIndex = 6;
            this.label3.Text = "Line Code :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(12, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 45);
            this.label4.TabIndex = 7;
            this.label4.Text = "Binding#   :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_cust
            // 
            this.cb_cust.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_cust.FormattingEnabled = true;
            this.cb_cust.Location = new System.Drawing.Point(146, 82);
            this.cb_cust.Name = "cb_cust";
            this.cb_cust.Size = new System.Drawing.Size(177, 38);
            this.cb_cust.TabIndex = 8;
            this.cb_cust.SelectedIndexChanged += new System.EventHandler(this.cb_cust_SelectedIndexChanged);
            this.cb_cust.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cb_cust_KeyDown);
            // 
            // cb_line_code
            // 
            this.cb_line_code.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_line_code.FormattingEnabled = true;
            this.cb_line_code.Location = new System.Drawing.Point(146, 130);
            this.cb_line_code.Name = "cb_line_code";
            this.cb_line_code.Size = new System.Drawing.Size(177, 38);
            this.cb_line_code.TabIndex = 9;
            this.cb_line_code.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cb_line_code_KeyDown);
            // 
            // tb_binding
            // 
            this.tb_binding.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_binding.Location = new System.Drawing.Point(146, 178);
            this.tb_binding.Name = "tb_binding";
            this.tb_binding.Size = new System.Drawing.Size(177, 35);
            this.tb_binding.TabIndex = 10;
            this.tb_binding.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_binding.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_binding_KeyDown);
            // 
            // btn_ok
            // 
            this.btn_ok.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ok.Location = new System.Drawing.Point(203, 219);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(120, 46);
            this.btn_ok.TabIndex = 11;
            this.btn_ok.Text = "확  인";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // Form_Splitlog_Input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 277);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.tb_binding);
            this.Controls.Add(this.cb_line_code);
            this.Controls.Add(this.cb_cust);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_employee);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Splitlog_Input";
            this.Text = "Split Log Input";
            this.Shown += new System.EventHandler(this.Form_Splitlog_Input_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_employee;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_cust;
        private System.Windows.Forms.ComboBox cb_line_code;
        private System.Windows.Forms.TextBox tb_binding;
        private System.Windows.Forms.Button btn_ok;
    }
}