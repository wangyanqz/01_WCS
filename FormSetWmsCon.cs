namespace WCS.Data
{
    using MySql.Data.MySqlClient;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;
    using WCS;

    public class FormSetWmsCon : Form
    {
        private Button buttonClose;
        private Button buttonSave;
        private ComboBox comboBox_if_handcode;
        private IContainer components;
        public Label label1;
        private Label label2;
        private Label label3;
        public Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        public Form1 mainFrm;
        private TextBox tberror_nodata1;
        private TextBox tberror_nodata2;
        private TextBox tberror_noport1;
        private TextBox tberror_noport2;
        private TextBox tberror_port;
        private TextBox tbmaxTurnNumer;

        public FormSetWmsCon(Form1 mainfrom)
        {
            this.InitializeComponent();
            this.mainFrm = mainfrom;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            string text = this.comboBox_if_handcode.Text;
            double num6 = 0.0;
            try
            {
                num = int.Parse(this.tberror_nodata1.Text);
                if ((num >= 1) && (num <= this.mainFrm.port_num))
                {
                    try
                    {
                        num2 = int.Parse(this.tberror_noport1.Text);
                        if ((num2 >= 1) && (num2 <= this.mainFrm.port_num))
                        {
                            try
                            {
                                num3 = int.Parse(this.tberror_nodata2.Text);
                                if ((num3 >= 1) && (num3 <= this.mainFrm.port_num))
                                {
                                    try
                                    {
                                        num4 = int.Parse(this.tberror_noport2.Text);
                                        if ((num4 >= 1) && (num4 <= this.mainFrm.port_num))
                                        {
                                            try
                                            {
                                                num5 = int.Parse(this.tberror_port.Text);
                                                if ((num5 >= 1) && (num5 <= this.mainFrm.port_num))
                                                {
                                                    switch (text)
                                                    {
                                                        case "是":
                                                        case "否":
                                                            if (text == "是")
                                                            {
                                                                text = "1";
                                                            }
                                                            else
                                                            {
                                                                text = "0";
                                                            }
                                                            try
                                                            {
                                                                num6 = double.Parse(this.tbmaxTurnNumer.Text);
                                                                if ((num6 >= 0.0) && (num6 <= 10.0))
                                                                {
                                                                    try
                                                                    {
                                                                        new MySqlCommand("update tb_sort_mode set if_handcode='" + text + "'", this.mainFrm.dbConn).ExecuteNonQuery();
                                                                    }
                                                                    catch (Exception exception)
                                                                    {
                                                                        MessageBox.Show("更新是否补码到数据库时异常！" + exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                                                        return;
                                                                    }
                                                                    this.mainFrm.error_nodata1 = num;
                                                                    this.mainFrm.error_noport1 = num2;
                                                                    this.mainFrm.error_nodata2 = num3;
                                                                    this.mainFrm.error_noport2 = num4;
                                                                    this.mainFrm.error_port = num5;
                                                                    this.mainFrm.if_handcode = text;
                                                                    this.mainFrm.maxTurnNumer = num6;
                                                                    XmlDocument document = new XmlDocument();
                                                                    document.Load(this.mainFrm.filename);
                                                                    document.DocumentElement.SelectSingleNode("/configuration/parameter/error_nodata1").InnerText = this.tberror_nodata1.Text;
                                                                    document.DocumentElement.SelectSingleNode("/configuration/parameter/error_noport1").InnerText = this.tberror_noport1.Text;
                                                                    document.DocumentElement.SelectSingleNode("/configuration/parameter/error_nodata2").InnerText = this.tberror_nodata2.Text;
                                                                    document.DocumentElement.SelectSingleNode("/configuration/parameter/error_noport2").InnerText = this.tberror_noport2.Text;
                                                                    document.DocumentElement.SelectSingleNode("/configuration/parameter/error_port").InnerText = this.tberror_port.Text;
                                                                    document.DocumentElement.SelectSingleNode("/configuration/parameter/if_handcode").InnerText = text;
                                                                    document.DocumentElement.SelectSingleNode("/configuration/parameter/maxTurnNumer").InnerText = this.tbmaxTurnNumer.Text;
                                                                    document.Save(this.mainFrm.filename);
                                                                    this.mainFrm.zddr_dev.Init_error_port();
                                                                    this.mainFrm.car_dev.max_handcode_sec = ((this.mainFrm.maxTurnNumer * this.mainFrm.car_num) * 0.5) / 1.2;
                                                                    MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                                                    base.Close();
                                                                    return;
                                                                }
                                                                MessageBox.Show("未补码件最多轮转圈数必须为0至10之间的数字！");
                                                            }
                                                            catch (Exception)
                                                            {
                                                                MessageBox.Show("未补码件最多轮转圈数必须为0至10之间的数字！");
                                                            }
                                                            return;
                                                    }
                                                    MessageBox.Show("请选择是否补码！");
                                                }
                                                else
                                                {
                                                    MessageBox.Show("错发件异常口必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                MessageBox.Show("错发件异常口必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("2#无格口异常口必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        MessageBox.Show("2#无格口异常口必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("2#无数据异常口必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("2#无数据异常口必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
                            }
                        }
                        else
                        {
                            MessageBox.Show("1#无格口异常口必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("1#无格口异常口必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
                    }
                }
                else
                {
                    MessageBox.Show("1#无数据异常口必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("1#无数据异常口必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
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

        private void FormSetWmsCon_Load(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.Load(this.mainFrm.filename);
            XmlNode node = document.DocumentElement.SelectSingleNode("/configuration/parameter/error_nodata1");
            this.tberror_nodata1.Text = node.InnerText;
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/error_noport1");
            this.tberror_noport1.Text = node.InnerText;
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/error_nodata2");
            this.tberror_nodata2.Text = node.InnerText;
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/error_noport2");
            this.tberror_noport2.Text = node.InnerText;
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/error_port");
            this.tberror_port.Text = node.InnerText;
            if (document.DocumentElement.SelectSingleNode("/configuration/parameter/if_handcode").InnerText == "0")
            {
                this.comboBox_if_handcode.Text = "否";
            }
            else
            {
                this.comboBox_if_handcode.Text = "是";
            }
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/maxTurnNumer");
            this.tbmaxTurnNumer.Text = node.InnerText;
        }

        private void InitializeComponent()
        {
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tberror_nodata1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tberror_noport1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tberror_noport2 = new System.Windows.Forms.TextBox();
            this.tberror_nodata2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tberror_port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_if_handcode = new System.Windows.Forms.ComboBox();
            this.tbmaxTurnNumer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSave.Location = new System.Drawing.Point(53, 373);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(120, 40);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonClose.Location = new System.Drawing.Point(239, 373);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(120, 40);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // tberror_nodata1
            // 
            this.tberror_nodata1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tberror_nodata1.Location = new System.Drawing.Point(196, 26);
            this.tberror_nodata1.Name = "tberror_nodata1";
            this.tberror_nodata1.Size = new System.Drawing.Size(158, 26);
            this.tberror_nodata1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(57, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "1#无数据异常口：";
            // 
            // tberror_noport1
            // 
            this.tberror_noport1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tberror_noport1.Location = new System.Drawing.Point(196, 71);
            this.tberror_noport1.Name = "tberror_noport1";
            this.tberror_noport1.Size = new System.Drawing.Size(158, 26);
            this.tberror_noport1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(57, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "1#无格口异常口：";
            // 
            // tberror_noport2
            // 
            this.tberror_noport2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tberror_noport2.Location = new System.Drawing.Point(196, 163);
            this.tberror_noport2.Name = "tberror_noport2";
            this.tberror_noport2.Size = new System.Drawing.Size(158, 26);
            this.tberror_noport2.TabIndex = 11;
            // 
            // tberror_nodata2
            // 
            this.tberror_nodata2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tberror_nodata2.Location = new System.Drawing.Point(196, 117);
            this.tberror_nodata2.Name = "tberror_nodata2";
            this.tberror_nodata2.Size = new System.Drawing.Size(158, 26);
            this.tberror_nodata2.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(57, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 16);
            this.label5.TabIndex = 10;
            this.label5.Text = "2#无格口异常口：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(57, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "2#无数据异常口：";
            // 
            // tberror_port
            // 
            this.tberror_port.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tberror_port.Location = new System.Drawing.Point(196, 208);
            this.tberror_port.Name = "tberror_port";
            this.tberror_port.Size = new System.Drawing.Size(158, 26);
            this.tberror_port.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(73, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "错发件异常口：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(105, 259);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "是否补码：";
            // 
            // comboBox_if_handcode
            // 
            this.comboBox_if_handcode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_if_handcode.FormattingEnabled = true;
            this.comboBox_if_handcode.Items.AddRange(new object[] {
            "是",
            "否"});
            this.comboBox_if_handcode.Location = new System.Drawing.Point(196, 255);
            this.comboBox_if_handcode.Name = "comboBox_if_handcode";
            this.comboBox_if_handcode.Size = new System.Drawing.Size(158, 24);
            this.comboBox_if_handcode.TabIndex = 16;
            // 
            // tbmaxTurnNumer
            // 
            this.tbmaxTurnNumer.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbmaxTurnNumer.Location = new System.Drawing.Point(196, 300);
            this.tbmaxTurnNumer.Name = "tbmaxTurnNumer";
            this.tbmaxTurnNumer.Size = new System.Drawing.Size(158, 26);
            this.tbmaxTurnNumer.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(9, 304);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(184, 16);
            this.label7.TabIndex = 17;
            this.label7.Text = "未补码件最多轮转圈数：";
            // 
            // FormSetWmsCon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 415);
            this.Controls.Add(this.tbmaxTurnNumer);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox_if_handcode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tberror_port);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tberror_noport2);
            this.Controls.Add(this.tberror_nodata2);
            this.Controls.Add(this.tberror_noport1);
            this.Controls.Add(this.tberror_nodata1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetWmsCon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置参数";
            this.Load += new System.EventHandler(this.FormSetWmsCon_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void FormSetWmsCon_Load_1(object sender, EventArgs e)
        {

        }
    }
}


