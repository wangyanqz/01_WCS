


namespace WCS.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml;
    public partial class ParamSettings : Form
    {
        public Form1 mainFrm;

        public ParamSettings(Form1 mainfrom)
        {
            this.InitializeComponent();
            this.mainFrm = mainfrom;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.Load(this.mainFrm.filename);
            XmlNode node = document.DocumentElement.SelectSingleNode("/configuration/parameter/dbip");
            this.tb_addr.Text = node.InnerText;
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/dbport");
            this.tb_port.Text = node.InnerText;
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/db1ip");
            this.tb_db.Text = node.InnerText;
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/plcip");
            this.tb_plc.Text = node.InnerText;

        }

        private void button_save_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(this.mainFrm.filename);
                document.DocumentElement.SelectSingleNode("/configuration/parameter/dbip").InnerText = this.tb_addr.Text;
                document.DocumentElement.SelectSingleNode("/configuration/parameter/dbport").InnerText = this.tb_port.Text;
                document.Save(this.mainFrm.filename);
                MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                base.Close();
                return;


            }
            catch (Exception)
            {


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            XmlDocument document = new XmlDocument();
            document.Load(this.mainFrm.filename);
            document.DocumentElement.SelectSingleNode("/configuration/parameter/db1ip").InnerText = this.tb_db.Text;
            document.Save(this.mainFrm.filename);
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            base.Close();
            return;
        }

        private void button_plc_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.Load(this.mainFrm.filename);
            document.DocumentElement.SelectSingleNode("/configuration/parameter/plcip").InnerText = this.tb_plc.Text;
            document.Save(this.mainFrm.filename);
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            base.Close();
            return;
        }
    }
}
