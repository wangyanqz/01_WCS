namespace WCS.DevSystem
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmCarpos : Form
    {
        private Button btnClose;
        private Button btnOpen;
        private Button buttonStop;
        private Car_Pos car_Pos;
        private IContainer components;
        private Label lable1;
        private Label lbBarcode;
        private Label lbCarStatus;
        private Label lblDeviceInfo;
        private Label lbPort;
        private int tag;
        private TextBox textBoxBarcode;
        private Label label1;
        private TextBox textBox1;
        private Timer timer1;

        public FrmCarpos(Car_Pos ssj_dev, int tag)
        {
            this.InitializeComponent();
            this.car_Pos = ssj_dev;
            this.tag = tag;
            this.Text = tag.ToString() + "号小车运行状态窗口";
            this.RefrushStatus();
        }

        private void btnClose_Click(object sender, EventArgs e)//xwc 填入格口号，手动落包指令
        {
            string str = this.textBox1.Text;

            if(str == "")
            {
                MessageBox.Show("请输入落包口编号");
                return;
            }
                

            bool flag = this.car_Pos.ManualCommandDown(this.tag, str);

            if (!flag)
                MessageBox.Show("写入PLC失败，请检查是否连接OPC");
            base.Close();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            this.car_Pos.ManualCommandWrite(this.tag, "0");
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.car_Pos.ManualCommandWrite(this.tag, "1");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbPort = new System.Windows.Forms.Label();
            this.lbCarStatus = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lbBarcode = new System.Windows.Forms.Label();
            this.textBoxBarcode = new System.Windows.Forms.TextBox();
            this.buttonStop = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.lblDeviceInfo = new System.Windows.Forms.Label();
            this.lable1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPort.Location = new System.Drawing.Point(10, 98);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(88, 16);
            this.lbPort.TabIndex = 18;
            this.lbPort.Text = "通道编号：";
            // 
            // lbCarStatus
            // 
            this.lbCarStatus.AutoSize = true;
            this.lbCarStatus.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCarStatus.Location = new System.Drawing.Point(10, 17);
            this.lbCarStatus.Name = "lbCarStatus";
            this.lbCarStatus.Size = new System.Drawing.Size(88, 16);
            this.lbCarStatus.TabIndex = 16;
            this.lbCarStatus.Text = "小车状态：";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(141, 430);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(115, 39);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "下发落包指令";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lbBarcode
            // 
            this.lbBarcode.AutoSize = true;
            this.lbBarcode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBarcode.Location = new System.Drawing.Point(10, 57);
            this.lbBarcode.Name = "lbBarcode";
            this.lbBarcode.Size = new System.Drawing.Size(88, 16);
            this.lbBarcode.TabIndex = 20;
            this.lbBarcode.Text = "运单条码：";
            // 
            // textBoxBarcode
            // 
            this.textBoxBarcode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxBarcode.Location = new System.Drawing.Point(99, 53);
            this.textBoxBarcode.Name = "textBoxBarcode";
            this.textBoxBarcode.ReadOnly = true;
            this.textBoxBarcode.Size = new System.Drawing.Size(270, 26);
            this.textBoxBarcode.TabIndex = 22;
            // 
            // buttonStop
            // 
            this.buttonStop.Font = new System.Drawing.Font("宋体", 12F);
            this.buttonStop.Location = new System.Drawing.Point(236, 244);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(97, 39);
            this.buttonStop.TabIndex = 26;
            this.buttonStop.Text = "停用小车";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Visible = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Font = new System.Drawing.Font("宋体", 12F);
            this.btnOpen.Location = new System.Drawing.Point(90, 244);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(97, 39);
            this.btnOpen.TabIndex = 25;
            this.btnOpen.Text = "开启小车";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Visible = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // lblDeviceInfo
            // 
            this.lblDeviceInfo.AutoSize = true;
            this.lblDeviceInfo.Font = new System.Drawing.Font("宋体", 12F);
            this.lblDeviceInfo.Location = new System.Drawing.Point(99, 179);
            this.lblDeviceInfo.Name = "lblDeviceInfo";
            this.lblDeviceInfo.Size = new System.Drawing.Size(40, 16);
            this.lblDeviceInfo.TabIndex = 24;
            this.lblDeviceInfo.Text = "状态";
            // 
            // lable1
            // 
            this.lable1.AutoSize = true;
            this.lable1.Font = new System.Drawing.Font("宋体", 12F);
            this.lable1.Location = new System.Drawing.Point(10, 179);
            this.lable1.Name = "lable1";
            this.lable1.Size = new System.Drawing.Size(88, 16);
            this.lable1.TabIndex = 23;
            this.lable1.Text = "开启状态：";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(71, 372);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 20;
            this.label1.Text = "落包格口号：";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(181, 366);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(166, 26);
            this.textBox1.TabIndex = 22;
            // 
            // FrmCarpos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 502);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.lblDeviceInfo);
            this.Controls.Add(this.lable1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBoxBarcode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbBarcode);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbPort);
            this.Controls.Add(this.lbCarStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCarpos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmCarpos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void RefrushStatus()
        {
            if (this.car_Pos.deviceStatus[this.tag].ToString() == "0")
            {
                this.btnOpen.Enabled = false;
                this.buttonStop.Visible = true;
                this.lblDeviceInfo.Text = "已开启";
            }
            else
            {
                this.btnOpen.Visible = true;
                this.buttonStop.Enabled = false;
                this.lblDeviceInfo.Text = "已停用";
            }
            /*switch (this.car_Pos.mainFrm.car_dev.loadStatus[this.tag - 1])
            {
                case 0:
                    this.lbCarStatus.Text = "小车状态：无货";
                    this.lbBarcode.Visible = false;
                    this.lbPort.Visible = false;
                    this.textBoxBarcode.Visible = false;
                    return;

                case 1:
                    this.lbCarStatus.Text = "小车状态：正常";
                    this.textBoxBarcode.Text = this.car_Pos.mainFrm.car_dev.billCode[this.tag - 1].ToString();
                    this.lbPort.Text = "通道编号：" + this.car_Pos.mainFrm.car_dev.outport[this.tag - 1].ToString();
                    return;

                case 2:
                    this.lbCarStatus.Text = "小车状态：无数据";
                    this.textBoxBarcode.Text = this.car_Pos.mainFrm.car_dev.billCode[this.tag - 1].ToString();
                    this.lbPort.Text = "通道编号：" + this.car_Pos.mainFrm.car_dev.outport[this.tag - 1].ToString();
                    return;

                case 3:
                    this.lbCarStatus.Text = "小车状态：无格口";
                    this.textBoxBarcode.Text = this.car_Pos.mainFrm.car_dev.billCode[this.tag - 1].ToString();
                    this.lbPort.Text = "通道编号：" + this.car_Pos.mainFrm.car_dev.outport[this.tag - 1].ToString();
                    return;

                case 4:
                    this.lbCarStatus.Text = "小车状态：错发件";
                    this.textBoxBarcode.Text = this.car_Pos.mainFrm.car_dev.billCode[this.tag - 1].ToString();
                    this.lbPort.Text = "通道编号：" + this.car_Pos.mainFrm.car_dev.outport[this.tag - 1].ToString();
                    return;
            }
            this.lbCarStatus.Text = "小车状态：" + this.car_Pos.mainFrm.car_dev.loadStatus[this.tag - 1].ToString() + "【未识别】";
            this.textBoxBarcode.Text = this.car_Pos.mainFrm.car_dev.billCode[this.tag - 1].ToString();
            this.lbPort.Text = "通道编号：" + this.car_Pos.mainFrm.car_dev.outport[this.tag - 1].ToString();*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.RefrushStatus();
        }
    }
}


