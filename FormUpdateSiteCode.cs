namespace WCS.Data
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WCS;

    public class FormUpdateSiteCode : Form
    {
        private Button buttonClose;
        private Button buttonSave;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        public Form1 mainFrm;
        private TextBox textBoxBill_code;
        private TextBox textBoxSiteid;

        public FormUpdateSiteCode(Form1 mainfrom, string ls_bill_code)
        {
            this.InitializeComponent();
            this.mainFrm = mainfrom;
            this.textBoxBill_code.Text = ls_bill_code;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string strErrorMsg = string.Empty;
            string barcodes = this.textBoxBill_code.Text.Trim();
            string str3 = this.textBoxSiteid.Text.Trim();
            if (barcodes.Length == 0)
            {
                MessageBox.Show("运单编号不能为空！");
            }
            else if (this.mainFrm.barcodeDef.GetRightBillCode(barcodes) == "NOREAD")
            {
                MessageBox.Show("运单编号不能符合运单规则！");
            }
            else if (!this.mainFrm.if_right_sort_code(str3))
            {
                MessageBox.Show("站点编码不存在！");
            }
            else
            {
                DataBase.UpdateSiteCode(barcodes, this.mainFrm.site_code1, this.mainFrm.site_code2, str3, ref strErrorMsg);
                if (strErrorMsg.Length > 0)
                {
                    MessageBox.Show("修改运单的站点编码异常" + strErrorMsg);
                }
                else
                {
                    MessageBox.Show("操作成功！" + Environment.NewLine + "请把运单重新分拣！");
                    base.Close();
                }
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

        private void InitializeComponent()
        {
            this.textBoxSiteid = new TextBox();
            this.label2 = new Label();
            this.textBoxBill_code = new TextBox();
            this.buttonClose = new Button();
            this.label1 = new Label();
            this.buttonSave = new Button();
            this.label3 = new Label();
            base.SuspendLayout();
            this.textBoxSiteid.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBoxSiteid.Location = new Point(0x62, 0x4c);
            this.textBoxSiteid.Name = "textBoxSiteid";
            this.textBoxSiteid.Size = new Size(0x109, 0x18);
            this.textBoxSiteid.TabIndex = 0x11;
            this.label2.AutoSize = true;
            this.label2.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label2.Location = new Point(12, 80);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x52, 15);
            this.label2.TabIndex = 0x10;
            this.label2.Text = "站点编码：";
            this.textBoxBill_code.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBoxBill_code.Location = new Point(0x62, 0x17);
            this.textBoxBill_code.Name = "textBoxBill_code";
            this.textBoxBill_code.Size = new Size(0x109, 0x18);
            this.textBoxBill_code.TabIndex = 14;
            this.buttonClose.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonClose.Location = new Point(0xf5, 0xd9);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new Size(100, 30);
            this.buttonClose.TabIndex = 15;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new EventHandler(this.buttonClose_Click);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label1.Location = new Point(12, 0x1b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x52, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "运单编号：";
            this.buttonSave.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonSave.Location = new Point(0x33, 0xd9);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new Size(100, 30);
            this.buttonSave.TabIndex = 12;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new EventHandler(this.buttonSave_Click);
            this.label3.AutoSize = true;
            this.label3.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label3.ForeColor = Color.Red;
            this.label3.Location = new Point(0x34, 0x102);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0xac, 15);
            this.label3.TabIndex = 0x12;
            this.label3.Text = "保存成功后，请重新分拣";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x17b, 0x11a);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.textBoxSiteid);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.textBoxBill_code);
            base.Controls.Add(this.buttonClose);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonSave);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FormUpdateSiteCode";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "手动更新站点编码";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

