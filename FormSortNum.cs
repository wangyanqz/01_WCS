namespace WCS.Data
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class FormSortNum : Form
    {
        private DataGridViewTextBoxColumn batch_no;
        private Button buttonClose;
        private Button buttonExport;
        private Button buttonRefresh;
        private IContainer components;
        private DataGridView dataGridView1;
        private DateTimePicker dtprEnd;
        private DateTimePicker dtprStart;
        private DataGridViewTextBoxColumn handcode_per;
        private Label label1;
        private Label label2;
        private DataGridViewTextBoxColumn nodata_per;
        private DataGridViewTextBoxColumn noport_per;
        private DataGridViewTextBoxColumn num1;
        private DataGridViewTextBoxColumn num10;
        private DataGridViewTextBoxColumn num11;
        private DataGridViewTextBoxColumn num12;
        private DataGridViewTextBoxColumn num13;
        private DataGridViewTextBoxColumn num14;
        private DataGridViewTextBoxColumn num2;
        private DataGridViewTextBoxColumn num3;
        private DataGridViewTextBoxColumn num4;
        private DataGridViewTextBoxColumn num5;
        private DataGridViewTextBoxColumn num6;
        private DataGridViewTextBoxColumn num7;
        private DataGridViewTextBoxColumn num8;
        private DataGridViewTextBoxColumn num9;
        private DataGridViewTextBoxColumn sum_error;
        private DataGridViewTextBoxColumn sum_handcode;
        private DataGridViewTextBoxColumn sum_nochannel;
        private DataGridViewTextBoxColumn sum_nodata;
        private DataGridViewTextBoxColumn sum_num;
        private DataGridViewTextBoxColumn update_time;

        public FormSortNum()
        {
            this.InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            this.SaveAs();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FormSortNum_Load(object sender, EventArgs e)
        {
            this.dataGridView1.Width = base.Width - 20;
            this.dataGridView1.Height = base.Height - 70;
            this.refresh();
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            this.dataGridView1 = new DataGridView();
            this.buttonExport = new Button();
            this.buttonClose = new Button();
            this.buttonRefresh = new Button();
            this.dtprEnd = new DateTimePicker();
            this.dtprStart = new DateTimePicker();
            this.label2 = new Label();
            this.label1 = new Label();
            this.batch_no = new DataGridViewTextBoxColumn();
            this.sum_num = new DataGridViewTextBoxColumn();
            this.sum_nodata = new DataGridViewTextBoxColumn();
            this.nodata_per = new DataGridViewTextBoxColumn();
            this.sum_nochannel = new DataGridViewTextBoxColumn();
            this.noport_per = new DataGridViewTextBoxColumn();
            this.sum_error = new DataGridViewTextBoxColumn();
            this.sum_handcode = new DataGridViewTextBoxColumn();
            this.handcode_per = new DataGridViewTextBoxColumn();
            this.update_time = new DataGridViewTextBoxColumn();
            this.num1 = new DataGridViewTextBoxColumn();
            this.num2 = new DataGridViewTextBoxColumn();
            this.num3 = new DataGridViewTextBoxColumn();
            this.num4 = new DataGridViewTextBoxColumn();
            this.num5 = new DataGridViewTextBoxColumn();
            this.num6 = new DataGridViewTextBoxColumn();
            this.num7 = new DataGridViewTextBoxColumn();
            this.num8 = new DataGridViewTextBoxColumn();
            this.num9 = new DataGridViewTextBoxColumn();
            this.num10 = new DataGridViewTextBoxColumn();
            this.num11 = new DataGridViewTextBoxColumn();
            this.num12 = new DataGridViewTextBoxColumn();
            this.num13 = new DataGridViewTextBoxColumn();
            this.num14 = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)this.dataGridView1).BeginInit();
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
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] {
                this.batch_no, this.sum_num, this.sum_nodata, this.nodata_per, this.sum_nochannel, this.noport_per, this.sum_error, this.sum_handcode, this.handcode_per, this.update_time, this.num1, this.num2, this.num3, this.num4, this.num5, this.num6,
                this.num7, this.num8, this.num9, this.num10, this.num11, this.num12, this.num13, this.num14
            });
            this.dataGridView1.Location = new Point(1, 0x29);
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
            this.dataGridView1.Size = new Size(0x558, 610);
            this.dataGridView1.TabIndex = 1;
            this.buttonExport.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonExport.Location = new Point(700, 5);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new Size(90, 30);
            this.buttonExport.TabIndex = 15;
            this.buttonExport.Text = "导出";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new EventHandler(this.buttonExport_Click);
            this.buttonClose.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonClose.Location = new Point(0x3bd, 5);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new Size(90, 30);
            this.buttonClose.TabIndex = 14;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new EventHandler(this.buttonClose_Click);
            this.buttonRefresh.Font = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.buttonRefresh.Location = new Point(0x1bb, 5);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new Size(90, 30);
            this.buttonRefresh.TabIndex = 13;
            this.buttonRefresh.Text = "查询";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new EventHandler(this.buttonRefresh_Click);
            this.dtprEnd.CustomFormat = "yyyy-MM-dd";
            this.dtprEnd.Font = new Font("宋体", 11f);
            this.dtprEnd.Format = DateTimePickerFormat.Custom;
            this.dtprEnd.Location = new Point(0x123, 8);
            this.dtprEnd.Name = "dtprEnd";
            this.dtprEnd.Size = new Size(0x6f, 0x18);
            this.dtprEnd.TabIndex = 0x19;
            this.dtprStart.CustomFormat = "yyyy-MM-dd";
            this.dtprStart.Font = new Font("宋体", 11f);
            this.dtprStart.Format = DateTimePickerFormat.Custom;
            this.dtprStart.Location = new Point(0x61, 8);
            this.dtprStart.Name = "dtprStart";
            this.dtprStart.Size = new Size(0x6f, 0x18);
            this.dtprStart.TabIndex = 0x18;
            this.label2.AutoSize = true;
            this.label2.FlatStyle = FlatStyle.System;
            this.label2.Font = new Font("宋体", 12f);
            this.label2.Location = new Point(0xdb, 12);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x48, 0x10);
            this.label2.TabIndex = 0x17;
            this.label2.Text = "结束日期";
            this.label1.AutoSize = true;
            this.label1.FlatStyle = FlatStyle.System;
            this.label1.Font = new Font("宋体", 12f);
            this.label1.Location = new Point(0x19, 12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 0x10);
            this.label1.TabIndex = 0x16;
            this.label1.Text = "起始日期";
            this.batch_no.DataPropertyName = "batch_no";
            this.batch_no.HeaderText = "批次号";
            this.batch_no.Name = "batch_no";
            this.batch_no.ReadOnly = true;
            this.batch_no.Width = 150;
            this.sum_num.DataPropertyName = "sum_num";
            this.sum_num.HeaderText = "总量";
            this.sum_num.Name = "sum_num";
            this.sum_num.ReadOnly = true;
            this.sum_nodata.DataPropertyName = "sum_nodata";
            this.sum_nodata.HeaderText = "无数据";
            this.sum_nodata.Name = "sum_nodata";
            this.sum_nodata.ReadOnly = true;
            this.nodata_per.DataPropertyName = "nodata_per";
            this.nodata_per.HeaderText = "无数据比例";
            this.nodata_per.Name = "nodata_per";
            this.nodata_per.ReadOnly = true;
            this.nodata_per.Width = 110;
            this.sum_nochannel.DataPropertyName = "sum_nochannel";
            this.sum_nochannel.HeaderText = "无格口";
            this.sum_nochannel.Name = "sum_nochannel";
            this.sum_nochannel.ReadOnly = true;
            this.noport_per.DataPropertyName = "noport_per";
            this.noport_per.HeaderText = "无格口比例";
            this.noport_per.Name = "noport_per";
            this.noport_per.ReadOnly = true;
            this.noport_per.Width = 110;
            this.sum_error.DataPropertyName = "sum_error";
            this.sum_error.HeaderText = "错发件";
            this.sum_error.Name = "sum_error";
            this.sum_error.ReadOnly = true;
            this.sum_handcode.DataPropertyName = "sum_handcode";
            this.sum_handcode.HeaderText = "补码量";
            this.sum_handcode.Name = "sum_handcode";
            this.sum_handcode.ReadOnly = true;
            this.handcode_per.DataPropertyName = "handcode_per";
            this.handcode_per.HeaderText = "补码量比例";
            this.handcode_per.Name = "handcode_per";
            this.handcode_per.ReadOnly = true;
            this.handcode_per.Width = 110;
            this.update_time.DataPropertyName = "update_time";
            this.update_time.HeaderText = "更新时间";
            this.update_time.Name = "update_time";
            this.update_time.ReadOnly = true;
            this.update_time.Width = 200;
            this.num1.DataPropertyName = "num1";
            this.num1.HeaderText = "数量1";
            this.num1.Name = "num1";
            this.num1.ReadOnly = true;
            this.num1.Width = 70;
            this.num2.DataPropertyName = "num2";
            this.num2.HeaderText = "数量2";
            this.num2.Name = "num2";
            this.num2.ReadOnly = true;
            this.num2.Width = 70;
            this.num3.DataPropertyName = "num3";
            this.num3.HeaderText = "数量3";
            this.num3.Name = "num3";
            this.num3.ReadOnly = true;
            this.num3.Width = 70;
            this.num4.DataPropertyName = "num4";
            this.num4.HeaderText = "数量4";
            this.num4.Name = "num4";
            this.num4.ReadOnly = true;
            this.num4.Width = 70;
            this.num5.DataPropertyName = "num5";
            this.num5.HeaderText = "数量5";
            this.num5.Name = "num5";
            this.num5.ReadOnly = true;
            this.num5.Width = 70;
            this.num6.DataPropertyName = "num6";
            this.num6.HeaderText = "数量6";
            this.num6.Name = "num6";
            this.num6.ReadOnly = true;
            this.num6.Width = 70;
            this.num7.DataPropertyName = "num7";
            this.num7.HeaderText = "数量7";
            this.num7.Name = "num7";
            this.num7.ReadOnly = true;
            this.num7.Width = 70;
            this.num8.DataPropertyName = "num8";
            this.num8.HeaderText = "数量8";
            this.num8.Name = "num8";
            this.num8.ReadOnly = true;
            this.num8.Width = 70;
            this.num9.DataPropertyName = "num9";
            this.num9.HeaderText = "数量9";
            this.num9.Name = "num9";
            this.num9.ReadOnly = true;
            this.num9.Width = 70;
            this.num10.DataPropertyName = "num10";
            this.num10.HeaderText = "数量10";
            this.num10.Name = "num10";
            this.num10.ReadOnly = true;
            this.num10.Width = 80;
            this.num11.DataPropertyName = "num11";
            this.num11.HeaderText = "数量11";
            this.num11.Name = "num11";
            this.num11.ReadOnly = true;
            this.num11.Width = 80;
            this.num12.DataPropertyName = "num12";
            this.num12.HeaderText = "数量12";
            this.num12.Name = "num12";
            this.num12.ReadOnly = true;
            this.num12.Width = 80;
            this.num13.DataPropertyName = "num13";
            this.num13.HeaderText = "数量13";
            this.num13.Name = "num13";
            this.num13.ReadOnly = true;
            this.num13.Width = 80;
            this.num14.DataPropertyName = "num14";
            this.num14.HeaderText = "数量14";
            this.num14.Name = "num14";
            this.num14.ReadOnly = true;
            this.num14.Width = 80;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x55a, 0x28b);
            base.Controls.Add(this.dtprEnd);
            base.Controls.Add(this.dtprStart);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonExport);
            base.Controls.Add(this.buttonClose);
            base.Controls.Add(this.buttonRefresh);
            base.Controls.Add(this.dataGridView1);
            base.Name = "FormSortNum";
            this.Text = "分拣数量查询";
            base.WindowState = FormWindowState.Maximized;
            base.Load += new EventHandler(this.FormSortNum_Load);
            ((ISupportInitialize)this.dataGridView1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void refresh()
        {
            string errorMsg = string.Empty;
            if (this.dtprStart.Value.ToString("yyyyMMdd").CompareTo(this.dtprEnd.Value.ToString("yyyyMMdd")) > 0)
            {
                MessageBox.Show("起始日期不能大于结束日期!");
            }
            else
            {
                DateTime startTime = this.dtprStart.Value;
                DateTime endTime = this.dtprEnd.Value;
                DataSet set = DataBase.GetSortnumInf(startTime, endTime, ref errorMsg);
                if (errorMsg.Length > 0)
                {
                    MessageBox.Show("查询分拣数量信息异常！" + errorMsg);
                }
                else
                {
                    this.dataGridView1.DataSource = set.Tables[0];
                }
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

