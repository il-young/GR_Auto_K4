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
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(78)))), ((int)(((byte)(88)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // label_title
            // 
            resources.ApplyResources(this.label_title, "label_title");
            this.label_title.BackColor = System.Drawing.Color.White;
            this.label_title.ForeColor = System.Drawing.Color.Blue;
            this.label_title.Name = "label_title";
            // 
            // button_Sort
            // 
            this.button_Sort.BackColor = System.Drawing.Color.White;
            this.button_Sort.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_Sort.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button_Sort.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.button_Sort, "button_Sort");
            this.button_Sort.Name = "button_Sort";
            this.button_Sort.UseVisualStyleBackColor = false;
            this.button_Sort.Click += new System.EventHandler(this.button_Sort_Click);
            // 
            // button_Bcr
            // 
            this.button_Bcr.BackColor = System.Drawing.Color.White;
            this.button_Bcr.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_Bcr.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button_Bcr.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.button_Bcr, "button_Bcr");
            this.button_Bcr.Name = "button_Bcr";
            this.button_Bcr.UseVisualStyleBackColor = false;
            this.button_Bcr.Click += new System.EventHandler(this.button_Gr_Click);
            // 
            // button_setting
            // 
            this.button_setting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.button_setting.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_setting.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button_setting.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.button_setting, "button_setting");
            this.button_setting.Name = "button_setting";
            this.button_setting.UseVisualStyleBackColor = false;
            this.button_setting.Click += new System.EventHandler(this.button_setting_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(78)))), ((int)(((byte)(88)))));
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label_day
            // 
            resources.ApplyResources(this.label_day, "label_day");
            this.label_day.BackColor = System.Drawing.Color.White;
            this.label_day.ForeColor = System.Drawing.Color.Black;
            this.label_day.Name = "label_day";
            // 
            // label_time
            // 
            resources.ApplyResources(this.label_time, "label_time");
            this.label_time.BackColor = System.Drawing.Color.White;
            this.label_time.ForeColor = System.Drawing.Color.Black;
            this.label_time.Name = "label_time";
            // 
            // button_Print
            // 
            this.button_Print.BackColor = System.Drawing.Color.White;
            this.button_Print.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_Print.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button_Print.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.button_Print, "button_Print");
            this.button_Print.Name = "button_Print";
            this.button_Print.UseVisualStyleBackColor = false;
            this.button_Print.Click += new System.EventHandler(this.button_Print_Click);
            // 
            // label_server
            // 
            this.label_server.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.label_server, "label_server");
            this.label_server.ForeColor = System.Drawing.Color.White;
            this.label_server.Name = "label_server";
            // 
            // label_camera
            // 
            this.label_camera.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.label_camera, "label_camera");
            this.label_camera.ForeColor = System.Drawing.Color.White;
            this.label_camera.Name = "label_camera";
            // 
            // label_state
            // 
            this.label_state.BackColor = System.Drawing.Color.Red;
            this.label_state.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.label_state, "label_state");
            this.label_state.ForeColor = System.Drawing.SystemColors.Info;
            this.label_state.Name = "label_state";
            // 
            // label_type
            // 
            this.label_type.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.label_type, "label_type");
            this.label_type.ForeColor = System.Drawing.Color.White;
            this.label_type.Name = "label_type";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Name = "label3";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // BankHost_main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label3);
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
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.Name = "BankHost_main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BankHost_main_FormClosing);
            this.Load += new System.EventHandler(this.BankHost_main_Load);
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
        private System.Windows.Forms.Label label3;
    }
}

