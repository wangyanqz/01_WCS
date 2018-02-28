namespace WCS.Data
{
    partial class ParamSettings
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_test2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_db = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_test1 = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.tb_addr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button_plc = new System.Windows.Forms.Button();
            this.tb_plc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(798, 573);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "通信管理";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_test2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.tb_db);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(10, 193);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(772, 144);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "服务器数据库接口";
            // 
            // button_test2
            // 
            this.button_test2.Location = new System.Drawing.Point(644, 32);
            this.button_test2.Name = "button_test2";
            this.button_test2.Size = new System.Drawing.Size(85, 29);
            this.button_test2.TabIndex = 2;
            this.button_test2.Text = "测试连接";
            this.button_test2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(644, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_db
            // 
            this.tb_db.Location = new System.Drawing.Point(103, 30);
            this.tb_db.Name = "tb_db";
            this.tb_db.Size = new System.Drawing.Size(222, 21);
            this.tb_db.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(29, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "数据库IP";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_test1);
            this.groupBox1.Controls.Add(this.button_save);
            this.groupBox1.Controls.Add(this.tb_port);
            this.groupBox1.Controls.Add(this.tb_addr);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(772, 144);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器数据交互接口";
            // 
            // button_test1
            // 
            this.button_test1.Location = new System.Drawing.Point(644, 32);
            this.button_test1.Name = "button_test1";
            this.button_test1.Size = new System.Drawing.Size(85, 29);
            this.button_test1.TabIndex = 2;
            this.button_test1.Text = "测试连接";
            this.button_test1.UseVisualStyleBackColor = true;
            this.button_test1.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(644, 77);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(85, 29);
            this.button_save.TabIndex = 2;
            this.button_save.Text = "保存";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // tb_port
            // 
            this.tb_port.Location = new System.Drawing.Point(102, 74);
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(222, 21);
            this.tb_port.TabIndex = 1;
            // 
            // tb_addr
            // 
            this.tb_addr.Location = new System.Drawing.Point(103, 30);
            this.tb_addr.Name = "tb_addr";
            this.tb_addr.Size = new System.Drawing.Size(222, 21);
            this.tb_addr.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(29, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "服务器端口";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(29, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器地址";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(1, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(806, 599);
            this.tabControl1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button_plc);
            this.groupBox3.Controls.Add(this.tb_plc);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(13, 367);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(772, 144);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PLC交互系统接口";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(644, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 29);
            this.button2.TabIndex = 2;
            this.button2.Text = "测试连接";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button_plc
            // 
            this.button_plc.Location = new System.Drawing.Point(644, 77);
            this.button_plc.Name = "button_plc";
            this.button_plc.Size = new System.Drawing.Size(85, 29);
            this.button_plc.TabIndex = 2;
            this.button_plc.Text = "保存";
            this.button_plc.UseVisualStyleBackColor = true;
            this.button_plc.Click += new System.EventHandler(this.button_plc_Click);
            // 
            // tb_plc
            // 
            this.tb_plc.Location = new System.Drawing.Point(103, 30);
            this.tb_plc.Name = "tb_plc";
            this.tb_plc.Size = new System.Drawing.Size(222, 21);
            this.tb_plc.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(29, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "IP";
            // 
            // ParamSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 624);
            this.Controls.Add(this.tabControl1);
            this.Name = "ParamSettings";
            this.Text = "基本参数";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.TextBox tb_port;
        private System.Windows.Forms.TextBox tb_addr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_test2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_db;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_test1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button_plc;
        private System.Windows.Forms.TextBox tb_plc;
        private System.Windows.Forms.Label label3;
    }
}