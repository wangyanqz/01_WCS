using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCS
{
    public partial class ManualPlan : Form
    {
        public Form1 mainFrm;
        public ManualPlan(Form1 mainfrom)
        {
            InitializeComponent();
            this.mainFrm = mainfrom;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                        //DataBase.delete_port(int.Parse(dataBoundItem.Row["id"].ToString()), ref strErrorMsg);
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
    }
}
