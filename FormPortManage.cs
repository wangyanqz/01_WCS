namespace WCS.Data
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using WCS;

    public class FormPortManage : Form
    {
        private Button buttonAdd;
        private Button buttonClose;
        private Button buttonDelete;
        private Button buttonEdit;
        private Button buttonExport;
        private Button buttonRefresh;
        private IContainer components;
        private DataGridView dataGridView1;
        private EventLog eventLog1;
        private DataGridViewTextBoxColumn id;
        private DataGridViewTextBoxColumn if_paijian;
        public Form1 mainFrm;
        private DataGridViewTextBoxColumn person_id;
        private DataGridViewTextBoxColumn port_id;
        private DataGridViewTextBoxColumn siteid;
        private DataGridViewTextBoxColumn SiteNAME;
        private DataGridViewTextBoxColumn sub_siteid;

        public FormPortManage(Form1 mainfrom)
        {
            this.InitializeComponent();
            this.mainFrm = mainfrom;
            this.refresh();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            new FormPortAdd(this.mainFrm, "ADD").ShowDialog();
            this.refresh();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string strErrorMsg = string.Empty;
            try
            {
                if (this.dataGridView1.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("确定要删除选中行吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.No)
                    {
                        DataRowView dataBoundItem = this.dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
                        DataBase.delete_port(int.Parse(dataBoundItem.Row["id"].ToString()), ref strErrorMsg);
                        if (strErrorMsg.Length > 0)
                        {
                            MessageBox.Show("操作失败！" + strErrorMsg);
                        }
                        else
                        {
                            dataBoundItem.Delete();
                            MessageBox.Show("操作成功！");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选中要删除的行！");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("操作失败！" + exception.Message.ToString());
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.SelectedRows.Count > 0)
                {
                    DataRowView dataBoundItem = this.dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
                    int num = int.Parse(dataBoundItem.Row["id"].ToString());
                    string str = dataBoundItem.Row["port_id"].ToString();
                    string str2 = dataBoundItem.Row["siteid"].ToString();
                    string str3 = dataBoundItem.Row["SiteNAME"].ToString();
                    string str4 = dataBoundItem.Row["person_id"].ToString();
                    string str5 = dataBoundItem.Row["if_paijian"].ToString();
                    string str6 = dataBoundItem.Row["sub_siteid"].ToString();
                    new FormPortAdd(this.mainFrm, "EDIT", num, str, str2, str3, str4, str5, str6).ShowDialog();
                    this.refresh();
                }
                else
                {
                    MessageBox.Show("请选中要修改的行！");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("操作失败！" + exception.Message.ToString());
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            this.SaveAs();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.refresh();
        }

        private void buttonSiteInf_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.ColumnIndex < 0) && (e.RowIndex >= 0))
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle cellBounds = e.CellBounds;
                cellBounds.Inflate(-2, -2);
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), new Font("宋体", 9f), cellBounds, e.CellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
                e.Handled = true;
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

        private void FormPortManage_Load(object sender, EventArgs e)
        {
            //this.refresh();
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            this.dataGridView1 = new DataGridView();
            this.eventLog1 = new EventLog();
            this.buttonAdd = new Button();
            this.buttonDelete = new Button();
            this.buttonRefresh = new Button();
            this.buttonClose = new Button();
            this.buttonEdit = new Button();
            this.buttonExport = new Button();
            this.id = new DataGridViewTextBoxColumn();
            this.port_id = new DataGridViewTextBoxColumn();
            this.siteid = new DataGridViewTextBoxColumn();
            this.SiteNAME = new DataGridViewTextBoxColumn();
            this.person_id = new DataGridViewTextBoxColumn();
            this.sub_siteid = new DataGridViewTextBoxColumn();
            this.if_paijian = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)this.dataGridView1).BeginInit();
            this.eventLog1.BeginInit();
            base.SuspendLayout();
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = style;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { this.id, this.port_id, this.siteid, this.SiteNAME, this.person_id, this.sub_siteid, this.if_paijian });
            this.dataGridView1.Location = new Point(2, 1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            style2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style2.BackColor = SystemColors.Control;
            style2.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            style2.ForeColor = SystemColors.WindowText;
            style2.SelectionBackColor = SystemColors.Highlight;
            style2.SelectionForeColor = SystemColors.HighlightText;
            style2.WrapMode = DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = style2;
            style3.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.dataGridView1.RowsDefaultCellStyle = style3;
            this.dataGridView1.RowTemplate.Height = 0x17;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new Size(0x365, 0x20c);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellPainting += new DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            this.eventLog1.SynchronizingObject = this;
            this.buttonAdd.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonAdd.Location = new Point(0x18, 0x218);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new Size(80, 30);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "增加";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
            this.buttonDelete.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonDelete.Location = new Point(0xab, 0x218);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new Size(80, 30);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "删除";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new EventHandler(this.buttonDelete_Click);
            this.buttonRefresh.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonRefresh.Location = new Point(0x1d1, 0x218);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new Size(80, 30);
            this.buttonRefresh.TabIndex = 3;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new EventHandler(this.buttonRefresh_Click);
            this.buttonClose.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonClose.Location = new Point(0x264, 0x218);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new Size(80, 30);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new EventHandler(this.buttonClose_Click);
            this.buttonEdit.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonEdit.Location = new Point(0x13e, 0x218);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new Size(80, 30);
            this.buttonEdit.TabIndex = 5;
            this.buttonEdit.Text = "修改";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new EventHandler(this.buttonEdit_Click);
            this.buttonExport.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonExport.Location = new Point(0x2f7, 0x218);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new Size(80, 30);
            this.buttonExport.TabIndex = 13;
            this.buttonExport.Text = "导出";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new EventHandler(this.buttonExport_Click);
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.port_id.DataPropertyName = "port_id";
            this.port_id.HeaderText = "格口号";
            this.port_id.Name = "port_id";
            this.port_id.ReadOnly = true;
            this.siteid.DataPropertyName = "siteid";
            this.siteid.HeaderText = "站点编码";
            this.siteid.Name = "siteid";
            this.siteid.ReadOnly = true;
            this.SiteNAME.DataPropertyName = "SiteNAME";
            this.SiteNAME.HeaderText = "站点名称";
            this.SiteNAME.Name = "SiteNAME";
            this.SiteNAME.ReadOnly = true;
            this.SiteNAME.Width = 150;
            this.person_id.DataPropertyName = "person_id";
            this.person_id.HeaderText = "业务员编码";
            this.person_id.Name = "person_id";
            this.person_id.ReadOnly = true;
            this.person_id.Width = 150;
            this.sub_siteid.DataPropertyName = "sub_siteid";
            this.sub_siteid.HeaderText = "分部编码";
            this.sub_siteid.Name = "sub_siteid";
            this.sub_siteid.ReadOnly = true;
            this.if_paijian.DataPropertyName = "if_paijian";
            this.if_paijian.HeaderText = "是否派件扫描";
            this.if_paijian.Name = "if_paijian";
            this.if_paijian.ReadOnly = true;
            this.if_paijian.Width = 150;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x367, 0x23d);
            base.Controls.Add(this.buttonExport);
            base.Controls.Add(this.buttonEdit);
            base.Controls.Add(this.buttonClose);
            base.Controls.Add(this.buttonRefresh);
            base.Controls.Add(this.buttonDelete);
            base.Controls.Add(this.buttonAdd);
            base.Controls.Add(this.dataGridView1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FormPortManage";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "格口管理";
            base.Load += new EventHandler(this.FormPortManage_Load);
            ((ISupportInitialize)this.dataGridView1).EndInit();
            this.eventLog1.EndInit();
            base.ResumeLayout(false);
        }

        private void refresh()
        {
            string errorMsg = string.Empty;
            DataSet outportInf = DataBase.GetOutportInf(this.mainFrm.deviceID, ref errorMsg);
            if (errorMsg.Length > 0)
            {
                MessageBox.Show("查询格口信息异常！" + errorMsg);
            }
            else
            {
                this.dataGridView1.DataSource = outportInf.Tables[0];
            }
        }

        private void SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Execl files (*.xls)|*.xls",
                FilterIndex = 0,
                RestoreDirectory = true,
                CreatePrompt = true,
                Title = "Export Excel File To"
            };
            if (DialogResult.OK == dialog.ShowDialog())
            {
                Stream stream = dialog.OpenFile();
                StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(0));
                string str = "";
                try
                {
                    for (int i = 0; i < this.dataGridView1.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            str = str + "\t";
                        }
                        str = str + this.dataGridView1.Columns[i].HeaderText;
                    }
                    writer.WriteLine(str);
                    for (int j = 0; j < this.dataGridView1.Rows.Count; j++)
                    {
                        string str2 = "";
                        for (int k = 0; k < this.dataGridView1.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                str2 = str2 + "\t";
                            }
                            str2 = str2 + this.dataGridView1.Rows[j].Cells[k].Value.ToString();
                        }
                        writer.WriteLine(str2);
                    }
                    writer.Close();
                    stream.Close();
                }
                catch (Exception)
                {
                }
                finally
                {
                    writer.Close();
                    stream.Close();
                }
                MessageBox.Show(dialog.FileName + " 成功建立！");
            }
        }
    }
}


