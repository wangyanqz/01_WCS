namespace WCS.Data
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WCS;

    public class FormPortPackage : Form
    {
        private Button buttonClearAll;
        private Button buttonClearSelect;
        private Button buttonClose;
        private Button buttonRefresh;
        private IContainer components;
        private DataGridViewTextBoxColumn creattime;
        private DataGridView dataGridView1;
        public Form1 mainFrm;
        private DataGridViewTextBoxColumn packagecode;
        private BindingSource bindingSource1;
        private DataGridViewTextBoxColumn portcode;

        public FormPortPackage(Form1 mainfrom)
        {
            this.InitializeComponent();
            this.mainFrm = mainfrom;
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            string strErrorMsg = string.Empty;
            try
            {
                if (MessageBox.Show("确定要清空所有行的集包袋条码吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.No)
                {
                    DataBase.ClearAll(this.mainFrm.deviceID, ref strErrorMsg);
                    if (strErrorMsg.Length > 0)
                    {
                        MessageBox.Show("操作失败！" + strErrorMsg);
                    }
                    else
                    {
                        this.refresh();
                        MessageBox.Show("操作成功！");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("操作失败！" + exception.Message.ToString());
            }
        }

        private void buttonClearSelect_Click(object sender, EventArgs e)
        {
            string strErrorMsg = string.Empty;
            try
            {
                if (this.dataGridView1.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("确定要清空选中行的集包袋条码吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.No)
                    {
                        DataRowView dataBoundItem = this.dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
                        DataBase.ClearSelect(dataBoundItem.Row["portcode"].ToString(), ref strErrorMsg);
                        if (strErrorMsg.Length > 0)
                        {
                            MessageBox.Show("操作失败！" + strErrorMsg);
                        }
                        else
                        {
                            this.refresh();
                            MessageBox.Show("操作成功！");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选中要清空集包袋条码的行！");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("操作失败！" + exception.Message.ToString());
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.refresh();
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

        private void FormPortPackage_Load(object sender, EventArgs e)
        {
            this.refresh();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.portcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.packagecode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creattime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.buttonClearSelect = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 11F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.portcode,
            this.packagecode,
            this.creattime});
            this.dataGridView1.Location = new System.Drawing.Point(2, 1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 11F);
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(601, 523);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            // 
            // portcode
            // 
            this.portcode.DataPropertyName = "portcode";
            this.portcode.HeaderText = "分拣口编码";
            this.portcode.Name = "portcode";
            this.portcode.ReadOnly = true;
            this.portcode.Width = 130;
            // 
            // packagecode
            // 
            this.packagecode.DataPropertyName = "packagecode";
            this.packagecode.HeaderText = "集包袋条码";
            this.packagecode.Name = "packagecode";
            this.packagecode.ReadOnly = true;
            this.packagecode.Width = 130;
            // 
            // creattime
            // 
            this.creattime.DataPropertyName = "creattime";
            dataGridViewCellStyle2.Format = "G";
            dataGridViewCellStyle2.NullValue = null;
            this.creattime.DefaultCellStyle = dataGridViewCellStyle2;
            this.creattime.HeaderText = "换袋时间";
            this.creattime.Name = "creattime";
            this.creattime.ReadOnly = true;
            this.creattime.Width = 260;
            // 
            // buttonClose
            // 
            this.buttonClose.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonClose.Location = new System.Drawing.Point(511, 531);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(80, 30);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRefresh.Location = new System.Drawing.Point(402, 531);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(80, 30);
            this.buttonRefresh.TabIndex = 6;
            this.buttonRefresh.Text = "刷新";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonClearAll.Location = new System.Drawing.Point(12, 531);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(166, 30);
            this.buttonClearAll.TabIndex = 5;
            this.buttonClearAll.Text = "清空所有集包袋条码";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // buttonClearSelect
            // 
            this.buttonClearSelect.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonClearSelect.Location = new System.Drawing.Point(207, 531);
            this.buttonClearSelect.Name = "buttonClearSelect";
            this.buttonClearSelect.Size = new System.Drawing.Size(166, 30);
            this.buttonClearSelect.TabIndex = 8;
            this.buttonClearSelect.Text = "清空选中集包袋条码";
            this.buttonClearSelect.UseVisualStyleBackColor = true;
            this.buttonClearSelect.Click += new System.EventHandler(this.buttonClearSelect_Click);
            // 
            // FormPortPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 573);
            this.Controls.Add(this.buttonClearSelect);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.buttonClearAll);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPortPackage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "分拣口编码与集包袋条码的对应关系";
            this.Load += new System.EventHandler(this.FormPortPackage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        private void refresh()
        {
            string errorMsg = string.Empty;
            DataSet portPackageInf = DataBase.GetPortPackageInf(ref errorMsg);
            if (errorMsg.Length > 0)
            {
                MessageBox.Show("查询分拣口编码与集包袋条码的对应关系异常！" + errorMsg);
            }
            else
            {
                this.dataGridView1.DataSource = portPackageInf.Tables[0];
            }
        }
    }
}


