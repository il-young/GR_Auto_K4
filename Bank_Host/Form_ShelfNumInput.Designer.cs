namespace Bank_Host
{
    partial class Form_ShelfNumInput
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
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_PreFix = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_StartShelf = new System.Windows.Forms.MaskedTextBox();
            this.tb_EndShelf = new System.Windows.Forms.MaskedTextBox();
            this.tb_StartBoxNo = new System.Windows.Forms.MaskedTextBox();
            this.tb_EndBox = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_StartBoxNo);
            this.groupBox1.Controls.Add(this.tb_StartShelf);
            this.groupBox1.Controls.Add(this.tb_PreFix);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 175);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Start";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(6, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "Box No.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(6, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "Shelf No.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 16F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(154, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 22);
            this.label3.TabIndex = 4;
            this.label3.Text = "~";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_EndBox);
            this.groupBox2.Controls.Add(this.tb_EndShelf);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(188, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(136, 175);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "End";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(6, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 14);
            this.label5.TabIndex = 6;
            this.label5.Text = "Box No.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(6, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 14);
            this.label6.TabIndex = 4;
            this.label6.Text = "Shelf No.";
            // 
            // tb_PreFix
            // 
            this.tb_PreFix.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.tb_PreFix.Location = new System.Drawing.Point(9, 41);
            this.tb_PreFix.Name = "tb_PreFix";
            this.tb_PreFix.Size = new System.Drawing.Size(100, 23);
            this.tb_PreFix.TabIndex = 7;
            this.tb_PreFix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(6, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 14);
            this.label7.TabIndex = 8;
            this.label7.Text = "Pre Fix.";
            // 
            // tb_StartShelf
            // 
            this.tb_StartShelf.Location = new System.Drawing.Point(9, 92);
            this.tb_StartShelf.Mask = "99999";
            this.tb_StartShelf.Name = "tb_StartShelf";
            this.tb_StartShelf.Size = new System.Drawing.Size(100, 26);
            this.tb_StartShelf.TabIndex = 7;
            this.tb_StartShelf.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_StartShelf.ValidatingType = typeof(int);
            // 
            // tb_EndShelf
            // 
            this.tb_EndShelf.Location = new System.Drawing.Point(9, 92);
            this.tb_EndShelf.Mask = "99999";
            this.tb_EndShelf.Name = "tb_EndShelf";
            this.tb_EndShelf.Size = new System.Drawing.Size(100, 26);
            this.tb_EndShelf.TabIndex = 8;
            this.tb_EndShelf.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_EndShelf.ValidatingType = typeof(int);
            // 
            // tb_StartBoxNo
            // 
            this.tb_StartBoxNo.Location = new System.Drawing.Point(9, 140);
            this.tb_StartBoxNo.Mask = "99999";
            this.tb_StartBoxNo.Name = "tb_StartBoxNo";
            this.tb_StartBoxNo.Size = new System.Drawing.Size(100, 26);
            this.tb_StartBoxNo.TabIndex = 9;
            this.tb_StartBoxNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_StartBoxNo.ValidatingType = typeof(int);
            // 
            // tb_EndBox
            // 
            this.tb_EndBox.Location = new System.Drawing.Point(9, 140);
            this.tb_EndBox.Mask = "99999";
            this.tb_EndBox.Name = "tb_EndBox";
            this.tb_EndBox.Size = new System.Drawing.Size(100, 26);
            this.tb_EndBox.TabIndex = 10;
            this.tb_EndBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_EndBox.ValidatingType = typeof(int);
            // 
            // Form_ShelfNumInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 200);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_ShelfNumInput";
            this.Text = "Shelf Input";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_PreFix;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox tb_StartBoxNo;
        private System.Windows.Forms.MaskedTextBox tb_StartShelf;
        private System.Windows.Forms.MaskedTextBox tb_EndBox;
        private System.Windows.Forms.MaskedTextBox tb_EndShelf;
    }
}