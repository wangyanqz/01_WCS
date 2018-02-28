namespace WCS.Data
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WCS;

    public class FormPortAdd : Form
    {
        private Button buttonClose;
        private Button buttonSave;
        private ComboBox comboBoxIf_paijian;
        private IContainer components;
        private int id;
        private string if_paijian;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        public Form1 mainFrm;
        private string name;
        private string ope_type;
        private string outport;
        private string person_id;
        private string Siteid;
        private string sub_siteid;
        private TextBox textBox_sub_siteid;
        private TextBox textBoxFull_name;
        private TextBox textBoxPort_id;
        private TextBox textBoxSiteid;
        private TextBox textBoxSiteNAME;

        public FormPortAdd(Form1 mainfrom, string ls_type)
        {
            this.ope_type = string.Empty;
            this.outport = string.Empty;
            this.Siteid = string.Empty;
            this.name = string.Empty;
            this.person_id = string.Empty;
            this.if_paijian = string.Empty;
            this.sub_siteid = string.Empty;
            this.InitializeComponent();
            this.mainFrm = mainfrom;
            this.ope_type = ls_type;
        }

        public FormPortAdd(Form1 mainfrom, string ls_type, int li_id, string ls_outport, string ls_Siteid, string ls_name, string ls_person_id, string ls_if_paijian, string ls_sub_siteid)
        {
            this.ope_type = string.Empty;
            this.outport = string.Empty;
            this.Siteid = string.Empty;
            this.name = string.Empty;
            this.person_id = string.Empty;
            this.if_paijian = string.Empty;
            this.sub_siteid = string.Empty;
            this.InitializeComponent();
            this.mainFrm = mainfrom;
            this.ope_type = ls_type;
            this.id = li_id;
            this.outport = ls_outport;
            this.Siteid = ls_Siteid;
            this.name = ls_name;
            this.person_id = ls_person_id;
            this.if_paijian = ls_if_paijian;
            this.sub_siteid = ls_sub_siteid;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string strErrorMsg = string.Empty;
            string s = this.textBoxPort_id.Text.Trim();
            string text = this.textBoxSiteid.Text;
            string str4 = this.textBoxSiteNAME.Text;
            string str5 = this.textBoxFull_name.Text;
            string str6 = this.textBox_sub_siteid.Text;
            string str7 = this.comboBoxIf_paijian.Text;
            int num = 0;
            try
            {
                num = int.Parse(s);
                if ((num >= 1) && (num <= this.mainFrm.port_num))
                {
                    if ((text.Length != 0) && (text.Length <= 10))
                    {
                        if ((str4.Length == 0) || (str4.Length > 40))
                        {
                            MessageBox.Show("站点名称不能为空且最大长度不能超过40！");
                        }
                        else if ((str7 != "是") && (str7 != "否"))
                        {
                            MessageBox.Show("请选择是否派件！");
                        }
                        else
                        {
                            if (str7 == "是")
                            {
                                str7 = "1";
                                if ((str5.Length == 0) || (str5.Length > 10))
                                {
                                    MessageBox.Show("业务员编码不能为空且最大长度不能超过10！");
                                    return;
                                }
                                if ((str6 != "00") && (str6.Length != 6))
                                {
                                    MessageBox.Show("分部编码为00或者长度为6位！");
                                    return;
                                }
                            }
                            else
                            {
                                str7 = "0";
                            }
                            if (this.ope_type == "ADD")
                            {
                                DataBase.add_port(this.mainFrm.deviceID, s, text, str4, str5, str7, str6, ref strErrorMsg);
                                if (strErrorMsg.Length > 0)
                                {
                                    MessageBox.Show("保存失败！" + strErrorMsg);
                                }
                                else
                                {
                                    this.textBoxPort_id.Text = "";
                                    this.textBoxSiteid.Text = "";
                                    this.textBoxSiteNAME.Text = "";
                                    this.textBoxFull_name.Text = "";
                                }
                            }
                            else
                            {
                                DataBase.edit_port(this.id, s, text, str4, str5, str7, str6, ref strErrorMsg);
                                if (strErrorMsg.Length > 0)
                                {
                                    MessageBox.Show("保存失败！" + strErrorMsg);
                                }
                                else
                                {
                                    MessageBox.Show("保存成功！");
                                    base.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("站点编码不能为空且最大长度不能超过10！");
                    }
                }
                else
                {
                    MessageBox.Show("格口号必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("格口号必须为1至" + this.mainFrm.port_num.ToString() + "之间的正整数！");
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

        private void FormPortAdd_Load(object sender, EventArgs e)
        {
            if (this.ope_type == "ADD")
            {
                this.Text = "增加格口";
            }
            else
            {
                this.Text = "修改格口";
                this.textBoxPort_id.Text = this.outport;
                this.textBoxSiteid.Text = this.Siteid;
                this.textBoxSiteNAME.Text = this.name;
                this.textBoxFull_name.Text = this.person_id;
                this.comboBoxIf_paijian.Text = this.if_paijian;
                this.textBox_sub_siteid.Text = this.sub_siteid;
            }
        }

        private void InitializeComponent()
        {
            this.buttonSave = new Button();
            this.label1 = new Label();
            this.textBoxPort_id = new TextBox();
            this.buttonClose = new Button();
            this.textBoxSiteid = new TextBox();
            this.label2 = new Label();
            this.textBoxSiteNAME = new TextBox();
            this.label5 = new Label();
            this.textBoxFull_name = new TextBox();
            this.label4 = new Label();
            this.comboBoxIf_paijian = new ComboBox();
            this.label3 = new Label();
            this.textBox_sub_siteid = new TextBox();
            this.label6 = new Label();
            base.SuspendLayout();
            this.buttonSave.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonSave.Location = new Point(90, 330);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new Size(100, 30);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new EventHandler(this.buttonSave_Click);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label1.Location = new Point(0x33, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x43, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "格口号：";
            this.textBoxPort_id.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBoxPort_id.Location = new Point(0x79, 0x10);
            this.textBoxPort_id.Name = "textBoxPort_id";
            this.textBoxPort_id.Size = new Size(0x13c, 0x18);
            this.textBoxPort_id.TabIndex = 2;
            this.buttonClose.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonClose.Location = new Point(0x11c, 330);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new Size(100, 30);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new EventHandler(this.buttonClose_Click);
            this.textBoxSiteid.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBoxSiteid.Location = new Point(0x79, 0x43);
            this.textBoxSiteid.Name = "textBoxSiteid";
            this.textBoxSiteid.Size = new Size(0x13c, 0x18);
            this.textBoxSiteid.TabIndex = 11;
            this.label2.AutoSize = true;
            this.label2.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label2.Location = new Point(0x24, 0x47);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x52, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "站点编码：";
            this.textBoxSiteNAME.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBoxSiteNAME.Location = new Point(0x79, 0x76);
            this.textBoxSiteNAME.Name = "textBoxSiteNAME";
            this.textBoxSiteNAME.Size = new Size(0x13c, 0x18);
            this.textBoxSiteNAME.TabIndex = 15;
            this.label5.AutoSize = true;
            this.label5.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label5.Location = new Point(0x24, 0x7a);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x52, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "站点简称：";
            this.textBoxFull_name.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBoxFull_name.Location = new Point(0x79, 170);
            this.textBoxFull_name.Name = "textBoxFull_name";
            this.textBoxFull_name.Size = new Size(0x13c, 0x18);
            this.textBoxFull_name.TabIndex = 0x15;
            this.label4.AutoSize = true;
            this.label4.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label4.Location = new Point(0x15, 0xae);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x61, 15);
            this.label4.TabIndex = 20;
            this.label4.Text = "业务员编码：";
            this.comboBoxIf_paijian.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.comboBoxIf_paijian.FormattingEnabled = true;
            this.comboBoxIf_paijian.Items.AddRange(new object[] { "是", "否" });
            this.comboBoxIf_paijian.Location = new Point(0x79, 0x111);
            this.comboBoxIf_paijian.Name = "comboBoxIf_paijian";
            this.comboBoxIf_paijian.Size = new Size(0x13c, 0x17);
            this.comboBoxIf_paijian.TabIndex = 0x17;
            this.label3.AutoSize = true;
            this.label3.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label3.Location = new Point(6, 0x115);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x70, 15);
            this.label3.TabIndex = 0x16;
            this.label3.Text = "是否派件扫描：";
            this.textBox_sub_siteid.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBox_sub_siteid.Location = new Point(0x77, 0xdd);
            this.textBox_sub_siteid.Name = "textBox_sub_siteid";
            this.textBox_sub_siteid.Size = new Size(0x13c, 0x18);
            this.textBox_sub_siteid.TabIndex = 0x19;
            this.label6.AutoSize = true;
            this.label6.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label6.Location = new Point(0x24, 0xe1);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x52, 15);
            this.label6.TabIndex = 0x18;
            this.label6.Text = "分部编码：";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1bf, 370);
            base.Controls.Add(this.textBox_sub_siteid);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.comboBoxIf_paijian);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.textBoxFull_name);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.textBoxSiteNAME);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.textBoxSiteid);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.textBoxPort_id);
            base.Controls.Add(this.buttonClose);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonSave);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FormPortAdd";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "FormPortAdd";
            base.Load += new EventHandler(this.FormPortAdd_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

