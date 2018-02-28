namespace WCS.DevSystem
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FormPipeline_Speed : Form
    {
        private Button buttonClose;
        private Button buttonModify;
        private ComboBox comboBoxRunSpeed;
        private IContainer components;
        private Label label2;
        private Pipeline_Speed pipeline_speed;
        private int tag;

        public FormPipeline_Speed(Pipeline_Speed runMode, int tag)
        {
            this.InitializeComponent();
            this.pipeline_speed = runMode;
            this.tag = tag;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            string cmdId = this.comboBoxRunSpeed.Text.Trim();
            if ((cmdId != "1米/秒") && (cmdId != "1.2米/秒"))
            {
                MessageBox.Show("请选择正确的主线速度！", "系统提示");
            }
            else
            {
                if (cmdId == "1米/秒")
                {
                    cmdId = "0";
                }
                else
                {
                    cmdId = "1";
                }
                this.pipeline_speed.ManualCommandWrite(this.tag, cmdId);
                MessageBox.Show("保存主线速度成功！");
                base.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FormPipeline_Speed_Load(object sender, EventArgs e)
        {
            this.refresh();
        }

        private void InitializeComponent()
        {
            this.comboBoxRunSpeed = new ComboBox();
            this.label2 = new Label();
            this.buttonClose = new Button();
            this.buttonModify = new Button();
            base.SuspendLayout();
            this.comboBoxRunSpeed.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxRunSpeed.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.comboBoxRunSpeed.FormattingEnabled = true;
            this.comboBoxRunSpeed.Items.AddRange(new object[] { "1米/秒", "1.2米/秒" });
            this.comboBoxRunSpeed.Location = new Point(0x92, 0x2c);
            this.comboBoxRunSpeed.MaxDropDownItems = 5;
            this.comboBoxRunSpeed.Name = "comboBoxRunSpeed";
            this.comboBoxRunSpeed.Size = new Size(0xca, 0x18);
            this.comboBoxRunSpeed.TabIndex = 12;
            this.label2.AutoSize = true;
            this.label2.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label2.Location = new Point(7, 0x2f);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x88, 0x10);
            this.label2.TabIndex = 11;
            this.label2.Text = "当前主线速度为：";
            this.buttonClose.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonClose.Location = new Point(0xe1, 0xc2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new Size(120, 0x24);
            this.buttonClose.TabIndex = 10;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new EventHandler(this.buttonClose_Click);
            this.buttonModify.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonModify.Location = new Point(0x12, 0xc2);
            this.buttonModify.Name = "buttonModify";
            this.buttonModify.Size = new Size(120, 0x24);
            this.buttonModify.TabIndex = 9;
            this.buttonModify.Text = "保存";
            this.buttonModify.UseVisualStyleBackColor = true;
            this.buttonModify.Click += new EventHandler(this.buttonModify_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x16e, 0xff);
            base.Controls.Add(this.comboBoxRunSpeed);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.buttonClose);
            base.Controls.Add(this.buttonModify);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FormPipeline_Speed";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "设置主线速度";
            base.Load += new EventHandler(this.FormPipeline_Speed_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void refresh()
        {
            if (this.pipeline_speed.deviceStatus[this.tag] == 0)
            {
                this.comboBoxRunSpeed.Text = "1米/秒";
            }
            else
            {
                this.comboBoxRunSpeed.Text = "1.2米/秒";
            }
        }
    }
}


