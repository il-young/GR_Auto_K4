namespace Bank_Host
{
    partial class Form_MultiBcrIn2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_MultiBcrIn2));
            this.button_close = new System.Windows.Forms.Button();
            this.textBox_qty = new System.Windows.Forms.TextBox();
            this.textBox_lot = new System.Windows.Forms.TextBox();
            this.textBox_device = new System.Windows.Forms.TextBox();
            this.label_bcr3 = new System.Windows.Forms.Label();
            this.label_bcr2 = new System.Windows.Forms.Label();
            this.label_bcr1 = new System.Windows.Forms.Label();
            this.checkBox_devicefix = new System.Windows.Forms.CheckBox();
            this.label_bcr4 = new System.Windows.Forms.Label();
            this.label_2dbcr = new System.Windows.Forms.Label();
            this.textBox_2dbcr = new System.Windows.Forms.TextBox();
            this.textBox_wftqty = new System.Windows.Forms.TextBox();
            this.cb_lot = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button_close
            // 
            this.button_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.button_close.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_close.ForeColor = System.Drawing.Color.White;
            this.button_close.Image = ((System.Drawing.Image)(resources.GetObject("button_close.Image")));
            this.button_close.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_close.Location = new System.Drawing.Point(800, 9);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(95, 87);
            this.button_close.TabIndex = 17;
            this.button_close.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_close.UseVisualStyleBackColor = false;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // textBox_qty
            // 
            this.textBox_qty.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_qty.Location = new System.Drawing.Point(147, 307);
            this.textBox_qty.Name = "textBox_qty";
            this.textBox_qty.Size = new System.Drawing.Size(747, 46);
            this.textBox_qty.TabIndex = 16;
            this.textBox_qty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_qty_KeyPress);
            // 
            // textBox_lot
            // 
            this.textBox_lot.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_lot.Location = new System.Drawing.Point(147, 209);
            this.textBox_lot.Name = "textBox_lot";
            this.textBox_lot.Size = new System.Drawing.Size(629, 46);
            this.textBox_lot.TabIndex = 15;
            this.textBox_lot.TextChanged += new System.EventHandler(this.textBox_lot_TextChanged);
            this.textBox_lot.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_lot_KeyPress);
            // 
            // textBox_device
            // 
            this.textBox_device.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_device.Location = new System.Drawing.Point(147, 160);
            this.textBox_device.Name = "textBox_device";
            this.textBox_device.Size = new System.Drawing.Size(629, 46);
            this.textBox_device.TabIndex = 14;
            this.textBox_device.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_device_KeyPress);
            // 
            // label_bcr3
            // 
            this.label_bcr3.BackColor = System.Drawing.Color.LightGray;
            this.label_bcr3.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_bcr3.Location = new System.Drawing.Point(11, 307);
            this.label_bcr3.Name = "label_bcr3";
            this.label_bcr3.Size = new System.Drawing.Size(133, 46);
            this.label_bcr3.TabIndex = 11;
            this.label_bcr3.Text = "QTY";
            this.label_bcr3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_bcr2
            // 
            this.label_bcr2.BackColor = System.Drawing.Color.LightGray;
            this.label_bcr2.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_bcr2.Location = new System.Drawing.Point(11, 209);
            this.label_bcr2.Name = "label_bcr2";
            this.label_bcr2.Size = new System.Drawing.Size(133, 46);
            this.label_bcr2.TabIndex = 12;
            this.label_bcr2.Text = "LOT";
            this.label_bcr2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_bcr1
            // 
            this.label_bcr1.BackColor = System.Drawing.Color.LightGray;
            this.label_bcr1.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_bcr1.Location = new System.Drawing.Point(11, 159);
            this.label_bcr1.Name = "label_bcr1";
            this.label_bcr1.Size = new System.Drawing.Size(133, 46);
            this.label_bcr1.TabIndex = 13;
            this.label_bcr1.Text = "DEVICE";
            this.label_bcr1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox_devicefix
            // 
            this.checkBox_devicefix.AutoSize = true;
            this.checkBox_devicefix.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_devicefix.Location = new System.Drawing.Point(782, 180);
            this.checkBox_devicefix.Name = "checkBox_devicefix";
            this.checkBox_devicefix.Size = new System.Drawing.Size(115, 25);
            this.checkBox_devicefix.TabIndex = 18;
            this.checkBox_devicefix.Text = "device 고정";
            this.checkBox_devicefix.UseVisualStyleBackColor = true;
            // 
            // label_bcr4
            // 
            this.label_bcr4.BackColor = System.Drawing.Color.LightGray;
            this.label_bcr4.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_bcr4.Location = new System.Drawing.Point(12, 258);
            this.label_bcr4.Name = "label_bcr4";
            this.label_bcr4.Size = new System.Drawing.Size(133, 46);
            this.label_bcr4.TabIndex = 11;
            this.label_bcr4.Text = "WFR QTY";
            this.label_bcr4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_2dbcr
            // 
            this.label_2dbcr.BackColor = System.Drawing.Color.LightGray;
            this.label_2dbcr.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_2dbcr.Location = new System.Drawing.Point(11, 109);
            this.label_2dbcr.Name = "label_2dbcr";
            this.label_2dbcr.Size = new System.Drawing.Size(133, 46);
            this.label_2dbcr.TabIndex = 13;
            this.label_2dbcr.Text = "2D 바코드";
            this.label_2dbcr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_2dbcr
            // 
            this.textBox_2dbcr.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_2dbcr.Location = new System.Drawing.Point(147, 109);
            this.textBox_2dbcr.Name = "textBox_2dbcr";
            this.textBox_2dbcr.Size = new System.Drawing.Size(629, 46);
            this.textBox_2dbcr.TabIndex = 14;
            this.textBox_2dbcr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_2dbcr_KeyPress);
            // 
            // textBox_wftqty
            // 
            this.textBox_wftqty.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_wftqty.Location = new System.Drawing.Point(147, 258);
            this.textBox_wftqty.Name = "textBox_wftqty";
            this.textBox_wftqty.Size = new System.Drawing.Size(748, 46);
            this.textBox_wftqty.TabIndex = 14;
            this.textBox_wftqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_wftqty_KeyPress);
            // 
            // cb_lot
            // 
            this.cb_lot.AutoSize = true;
            this.cb_lot.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_lot.Location = new System.Drawing.Point(782, 224);
            this.cb_lot.Name = "cb_lot";
            this.cb_lot.Size = new System.Drawing.Size(115, 25);
            this.cb_lot.TabIndex = 19;
            this.cb_lot.Text = "구분자 삭제";
            this.cb_lot.UseVisualStyleBackColor = true;
            this.cb_lot.CheckedChanged += new System.EventHandler(this.cb_lot_CheckedChanged);
            // 
            // Form_MultiBcrIn2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(895, 357);
            this.ControlBox = false;
            this.Controls.Add(this.cb_lot);
            this.Controls.Add(this.checkBox_devicefix);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.textBox_qty);
            this.Controls.Add(this.textBox_lot);
            this.Controls.Add(this.textBox_wftqty);
            this.Controls.Add(this.textBox_2dbcr);
            this.Controls.Add(this.textBox_device);
            this.Controls.Add(this.label_bcr4);
            this.Controls.Add(this.label_bcr3);
            this.Controls.Add(this.label_bcr2);
            this.Controls.Add(this.label_2dbcr);
            this.Controls.Add(this.label_bcr1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_MultiBcrIn2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "바코드 입력";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.TextBox textBox_qty;
        private System.Windows.Forms.TextBox textBox_lot;
        private System.Windows.Forms.TextBox textBox_device;
        private System.Windows.Forms.Label label_bcr3;
        private System.Windows.Forms.Label label_bcr2;
        private System.Windows.Forms.Label label_bcr1;
        private System.Windows.Forms.CheckBox checkBox_devicefix;
        private System.Windows.Forms.Label label_bcr4;
        private System.Windows.Forms.Label label_2dbcr;
        private System.Windows.Forms.TextBox textBox_2dbcr;
        private System.Windows.Forms.TextBox textBox_wftqty;
        private System.Windows.Forms.CheckBox cb_lot;
    }
}