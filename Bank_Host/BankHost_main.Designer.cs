namespace Bank_Host
{
    partial class BankHost_main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BankHost_main));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label_title = new System.Windows.Forms.Label();
            this.button_Sort = new System.Windows.Forms.Button();
            this.button_Bcr = new System.Windows.Forms.Button();
            this.button_setting = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label_day = new System.Windows.Forms.Label();
            this.label_time = new System.Windows.Forms.Label();
            this.button_Print = new System.Windows.Forms.Button();
            this.label_server = new System.Windows.Forms.Label();
            this.label_camera = new System.Windows.Forms.Label();
            this.label_state = new System.Windows.Forms.Label();
            this.label_type = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1264, 54);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(78)))), ((int)(((byte)(88)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 733);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1264, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "Copyright 2020 - Amkor Technology Korea Automation Engineering ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.BackColor = System.Drawing.Color.White;
            this.label_title.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_title.ForeColor = System.Drawing.Color.Blue;
            this.label_title.Location = new System.Drawing.Point(146, 13);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(57, 29);
            this.label_title.TabIndex = 4;
            this.label_title.Text = "Host";
            // 
            // button_Sort
            // 
            this.button_Sort.BackColor = System.Drawing.Color.White;
            this.button_Sort.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_Sort.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button_Sort.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button_Sort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Sort.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Sort.Image = ((System.Drawing.Image)(resources.GetObject("button_Sort.Image")));
            this.button_Sort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Sort.Location = new System.Drawing.Point(5, 68);
            this.button_Sort.Name = "button_Sort";
            this.button_Sort.Size = new System.Drawing.Size(208, 75);
            this.button_Sort.TabIndex = 5;
            this.button_Sort.Text = "        AUTO GR";
            this.button_Sort.UseVisualStyleBackColor = false;
            this.button_Sort.Click += new System.EventHandler(this.button_Sort_Click);
            // 
            // button_Bcr
            // 
            this.button_Bcr.BackColor = System.Drawing.Color.White;
            this.button_Bcr.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_Bcr.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button_Bcr.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button_Bcr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Bcr.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Bcr.Image = ((System.Drawing.Image)(resources.GetObject("button_Bcr.Image")));
            this.button_Bcr.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Bcr.Location = new System.Drawing.Point(6, 226);
            this.button_Bcr.Name = "button_Bcr";
            this.button_Bcr.Size = new System.Drawing.Size(207, 75);
            this.button_Bcr.TabIndex = 5;
            this.button_Bcr.Text = "        SCANNER";
            this.button_Bcr.UseVisualStyleBackColor = false;
            this.button_Bcr.Click += new System.EventHandler(this.button_Gr_Click);
            // 
            // button_setting
            // 
            this.button_setting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.button_setting.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_setting.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button_setting.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button_setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_setting.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_setting.Image = ((System.Drawing.Image)(resources.GetObject("button_setting.Image")));
            this.button_setting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_setting.Location = new System.Drawing.Point(6, 645);
            this.button_setting.Name = "button_setting";
            this.button_setting.Size = new System.Drawing.Size(207, 75);
            this.button_setting.TabIndex = 5;
            this.button_setting.Text = "     설정";
            this.button_setting.UseVisualStyleBackColor = false;
            this.button_setting.Click += new System.EventHandler(this.button_setting_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(78)))), ((int)(((byte)(88)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 679);
            this.label1.TabIndex = 6;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label_day
            // 
            this.label_day.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_day.AutoSize = true;
            this.label_day.BackColor = System.Drawing.Color.White;
            this.label_day.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_day.ForeColor = System.Drawing.Color.Black;
            this.label_day.Location = new System.Drawing.Point(925, 18);
            this.label_day.Name = "label_day";
            this.label_day.Size = new System.Drawing.Size(104, 23);
            this.label_day.TabIndex = 12;
            this.label_day.Text = "0000/00/00";
            // 
            // label_time
            // 
            this.label_time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_time.AutoSize = true;
            this.label_time.BackColor = System.Drawing.Color.White;
            this.label_time.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_time.ForeColor = System.Drawing.Color.Black;
            this.label_time.Location = new System.Drawing.Point(1033, 18);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(80, 23);
            this.label_time.TabIndex = 13;
            this.label_time.Text = "00:00:00";
            // 
            // button_Print
            // 
            this.button_Print.BackColor = System.Drawing.Color.White;
            this.button_Print.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_Print.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button_Print.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button_Print.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Print.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Print.Image = ((System.Drawing.Image)(resources.GetObject("button_Print.Image")));
            this.button_Print.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Print.Location = new System.Drawing.Point(6, 147);
            this.button_Print.Name = "button_Print";
            this.button_Print.Size = new System.Drawing.Size(207, 75);
            this.button_Print.TabIndex = 5;
            this.button_Print.Text = "       PRINT";
            this.button_Print.UseVisualStyleBackColor = false;
            this.button_Print.Click += new System.EventHandler(this.button_Print_Click);
            // 
            // label_server
            // 
            this.label_server.BackColor = System.Drawing.Color.Red;
            this.label_server.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_server.ForeColor = System.Drawing.Color.White;
            this.label_server.Location = new System.Drawing.Point(12, 315);
            this.label_server.Name = "label_server";
            this.label_server.Size = new System.Drawing.Size(191, 36);
            this.label_server.TabIndex = 15;
            this.label_server.Text = "서버 접속 실패";
            this.label_server.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_camera
            // 
            this.label_camera.BackColor = System.Drawing.Color.Red;
            this.label_camera.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_camera.ForeColor = System.Drawing.Color.White;
            this.label_camera.Location = new System.Drawing.Point(12, 391);
            this.label_camera.Name = "label_camera";
            this.label_camera.Size = new System.Drawing.Size(191, 36);
            this.label_camera.TabIndex = 15;
            this.label_camera.Text = "카메라 연결 실패";
            this.label_camera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_state
            // 
            this.label_state.BackColor = System.Drawing.Color.Red;
            this.label_state.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_state.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_state.ForeColor = System.Drawing.SystemColors.Info;
            this.label_state.Location = new System.Drawing.Point(218, 54);
            this.label_state.Name = "label_state";
            this.label_state.Size = new System.Drawing.Size(1046, 5);
            this.label_state.TabIndex = 17;
            this.label_state.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_type
            // 
            this.label_type.BackColor = System.Drawing.Color.Red;
            this.label_type.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_type.ForeColor = System.Drawing.Color.White;
            this.label_type.Location = new System.Drawing.Point(12, 353);
            this.label_type.Name = "label_type";
            this.label_type.Size = new System.Drawing.Size(191, 36);
            this.label_type.TabIndex = 15;
            this.label_type.Text = "Reel Type";
            this.label_type.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BankHost_main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1264, 761);
            this.Controls.Add(this.label_state);
            this.Controls.Add(this.label_camera);
            this.Controls.Add(this.label_type);
            this.Controls.Add(this.label_server);
            this.Controls.Add(this.label_day);
            this.Controls.Add(this.label_time);
            this.Controls.Add(this.button_setting);
            this.Controls.Add(this.button_Print);
            this.Controls.Add(this.button_Bcr);
            this.Controls.Add(this.button_Sort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_title);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.Name = "BankHost_main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bank Host";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BankHost_main_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Button button_Sort;
        private System.Windows.Forms.Button button_Bcr;
        private System.Windows.Forms.Button button_setting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label_day;
        private System.Windows.Forms.Label label_time;
        private System.Windows.Forms.Button button_Print;
        private System.Windows.Forms.Label label_server;
        private System.Windows.Forms.Label label_camera;
        private System.Windows.Forms.Label label_state;
        private System.Windows.Forms.Label label_type;
    }
}

