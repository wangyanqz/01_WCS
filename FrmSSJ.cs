namespace WCS.DevSystem
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmSSJ : Form
    {
        private Button btnClose;
        private Button btnSendCommand;
        private IContainer components;
        private GroupBox groupBox1;
        private Label lbDeviceFailure;
        private Label lbDeviceStatus;
        private Label lblCarSpeed1;
        private Label lblCarSpeed2;
        public RadioButton[] rdButton;
        private SSJ_Dev ssj_dev;
        private string strCommand;
        private int tag;
        public TextBox textBoxCarSpeed;
        private Timer timer1;

        public FrmSSJ(SSJ_Dev ssj_dev, int tag)
        {
            this.InitializeComponent();
            this.ssj_dev = ssj_dev;
            this.tag = tag;
            this.Text = ssj_dev.deviceId[tag] + "的状态及手动控制窗口";
            //if (ssj_dev.controlType[tag].Trim().Length > 0)
            //{
            //    this.rdButton = new RadioButton[ssj_dev.mainFrm.deviceControlDic.Count];
            //    int index = 0;
            //    foreach (string str in ssj_dev.mainFrm.deviceControlDic.Keys)
            //    {
            //        if (ssj_dev.mainFrm.deviceControlDic[str].controlType == ssj_dev.controlType[tag])
            //        {
            //            this.rdButton[index] = new RadioButton();
            //            this.rdButton[index].Name = ssj_dev.mainFrm.deviceControlDic[str].controlId;
            //            this.rdButton[index].Text = ssj_dev.mainFrm.deviceControlDic[str].controlDesc;
            //            this.rdButton[index].Location = new Point(20 + ((index % 2) * 160), 20 + (30 * (index / 2)));
            //            this.rdButton[index].Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            //            this.rdButton[index].Size = new Size(150, 20);
            //            this.groupBox1.Controls.Add(this.rdButton[index]);
            //            this.rdButton[index].CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
            //            index++;
            //        }
            //    }
            //    this.rdButton[0].Checked = true;
            //    this.strCommand = this.rdButton[0].Name;
            //}
            //else
            //{
            //    this.groupBox1.Visible = false;
            //    this.btnSendCommand.Visible = false;
            //}
            this.RefrushStatus(tag);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            this.ssj_dev.ManulCommand(this.tag, this.strCommand);
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
            this.components = new Container();
            this.btnClose = new Button();
            this.btnSendCommand = new Button();
            this.groupBox1 = new GroupBox();
            this.lbDeviceFailure = new Label();
            this.lbDeviceStatus = new Label();
            this.lblCarSpeed1 = new Label();
            this.lblCarSpeed2 = new Label();
            this.textBoxCarSpeed = new TextBox();
            this.timer1 = new Timer(this.components);
            base.SuspendLayout();
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.btnClose.Location = new Point(0xfe, 0x15c);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x62, 0x24);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.btnSendCommand.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.btnSendCommand.Location = new Point(0x47, 0x15c);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new Size(0x60, 0x24);
            this.btnSendCommand.TabIndex = 13;
            this.btnSendCommand.Text = "发送命令";
            this.btnSendCommand.UseVisualStyleBackColor = true;
            this.btnSendCommand.Click += new EventHandler(this.btnSendCommand_Click);
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.FlatStyle = FlatStyle.Popup;
            this.groupBox1.Location = new Point(3, 0x6b);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1a6, 0xde);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "点动控制";
            this.lbDeviceFailure.AutoSize = true;
            this.lbDeviceFailure.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.lbDeviceFailure.Location = new Point(0, 0x2a);
            this.lbDeviceFailure.Name = "lbDeviceFailure";
            this.lbDeviceFailure.Size = new Size(0x38, 0x10);
            this.lbDeviceFailure.TabIndex = 11;
            this.lbDeviceFailure.Text = "故障：";
            this.lbDeviceStatus.AutoSize = true;
            this.lbDeviceStatus.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.lbDeviceStatus.Location = new Point(0, 10);
            this.lbDeviceStatus.Name = "lbDeviceStatus";
            this.lbDeviceStatus.Size = new Size(0x38, 0x10);
            this.lbDeviceStatus.TabIndex = 10;
            this.lbDeviceStatus.Text = "状态：";
            this.lblCarSpeed1.AutoSize = true;
            this.lblCarSpeed1.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.lblCarSpeed1.Location = new Point(0, 0x4a);
            this.lblCarSpeed1.Name = "lblCarSpeed1";
            this.lblCarSpeed1.Size = new Size(0x58, 0x10);
            this.lblCarSpeed1.TabIndex = 15;
            this.lblCarSpeed1.Tag = "";
            this.lblCarSpeed1.Text = "主线速度：";
            this.lblCarSpeed2.AutoSize = true;
            this.lblCarSpeed2.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.lblCarSpeed2.Location = new Point(150, 0x4a);
            this.lblCarSpeed2.Name = "lblCarSpeed2";
            this.lblCarSpeed2.Size = new Size(0x30, 0x10);
            this.lblCarSpeed2.TabIndex = 0x10;
            this.lblCarSpeed2.Tag = "";
            this.lblCarSpeed2.Text = "米/秒";
            this.textBoxCarSpeed.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBoxCarSpeed.Location = new Point(0x53, 0x45);
            this.textBoxCarSpeed.Name = "textBoxCarSpeed";
            this.textBoxCarSpeed.ReadOnly = true;
            this.textBoxCarSpeed.Size = new Size(0x40, 0x1a);
            this.textBoxCarSpeed.TabIndex = 0x11;
            this.textBoxCarSpeed.Text = "1.000";
            this.timer1.Interval = 0x3e8;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(0xe4, 250, 0xf9);
            base.ClientSize = new Size(0x1ad, 0x187);
            base.Controls.Add(this.textBoxCarSpeed);
            base.Controls.Add(this.lblCarSpeed2);
            base.Controls.Add(this.lblCarSpeed1);
            base.Controls.Add(this.btnClose);
            base.Controls.Add(this.btnSendCommand);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.lbDeviceFailure);
            base.Controls.Add(this.lbDeviceStatus);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FrmSSJ";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "FrmSSJ";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                this.strCommand = ((RadioButton)sender).Name;
            }
        }

        private void RefrushStatus(int tag)
        {
            if (this.ssj_dev.statusType[tag] == "KZG")
            {
                this.lbDeviceStatus.Text = "设备状态：";
                if (!this.ssj_dev.mainFrm.plcSystemMS.is_auto)
                {
                    this.lbDeviceStatus.Text = this.lbDeviceStatus.Text + "手动模式、";
                }
                else
                {
                    this.lbDeviceStatus.Text = this.lbDeviceStatus.Text + "自动模式、";
                }
                if (!this.ssj_dev.mainFrm.plcSystemMS.is_runing)
                {
                    this.lbDeviceStatus.Text = this.lbDeviceStatus.Text + "停止状态、";
                }
                else
                {
                    this.lbDeviceStatus.Text = this.lbDeviceStatus.Text + "运行状态、";
                }
                if (this.ssj_dev.mainFrm.plcSystemMS.is_stop)
                {
                    this.lbDeviceStatus.Text = this.lbDeviceStatus.Text + "急停;";
                }
                else
                {
                    this.lbDeviceStatus.Text = this.lbDeviceStatus.Text + "无急停;";
                }
            }
            else
            {
                this.lbDeviceStatus.Visible = false;
            }
            if (this.ssj_dev.statusType[tag] == "KZG")
            {
                if (this.ssj_dev.deviceStatus[tag] != 0)
                {
                    string str = string.Empty;
                    if ((this.ssj_dev.deviceStatus[tag] & 1) != 0)
                    {
                        str = str + "启动位1光电异常;";
                    }
                    if ((this.ssj_dev.deviceStatus[tag] & 2) != 0)
                    {
                        str = str + "校验光电1异常;";
                    }
                    if ((this.ssj_dev.deviceStatus[tag] & 4) != 0)
                    {
                        str = str + "启动位2光电异常;";
                    }
                    if ((this.ssj_dev.deviceStatus[tag] & 8) != 0)
                    {
                        str = str + "校验光电2异常;";
                    }
                    if ((this.ssj_dev.deviceStatus[tag] & 0x10) != 0)
                    {
                        str = str + "变频器1过载;";
                    }
                    if ((this.ssj_dev.deviceStatus[tag] & 0x20) != 0)
                    {
                        str = str + "变频器2过载;";
                    }
                    if ((this.ssj_dev.deviceStatus[tag] & 0x40) != 0)
                    {
                        str = str + "主线速度异常;";
                    }
                    this.lbDeviceFailure.Text = "设备故障：" + str;
                }
                else
                {
                    this.lbDeviceFailure.Text = "设备故障：无";
                }
            }
            else if (this.ssj_dev.deviceStatus[tag] != 0)
            {
                string str2 = string.Empty;
                if ((this.ssj_dev.deviceStatus[tag] & 1) != 0)
                {
                    str2 = str2 + "光电异常;";
                }
                if ((this.ssj_dev.deviceStatus[tag] & 2) != 0)
                {
                    str2 = str2 + "跳闸小车编号:" + this.ssj_dev.deviceFailure[tag].ToString() + ";";
                }
                if ((this.ssj_dev.deviceStatus[tag] & 4) != 0)
                {
                    str2 = str2 + "急停;";
                }
                this.lbDeviceFailure.Text = "设备故障：" + str2;
            }
            else
            {
                this.lbDeviceFailure.Text = "设备故障：无";
            }
            if (this.ssj_dev.statusType[tag] == "KZG")
            {
                this.lblCarSpeed1.Visible = true;
                this.textBoxCarSpeed.Visible = true;
                this.lblCarSpeed2.Visible = true;
                this.timer1.Enabled = true;
            }
            else
            {
                this.lblCarSpeed1.Visible = false;
                this.textBoxCarSpeed.Visible = false;
                this.lblCarSpeed2.Visible = false;
                this.timer1.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.textBoxCarSpeed.Text = (this.ssj_dev.mainFrm.plcSystemMS.carSpeedValue / 1000.0).ToString();
        }
    }
}


