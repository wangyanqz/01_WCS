namespace WCS.DevSystem
{
    using OpcRcw.Da;
    using System;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WCS;
    using WCS.PlcSystem;

    public class KZAN_Dev
    {
        public string[] commandDB;// 启用禁用PLC
        public int[] commandHandle;
        public string[] deviceId;
        public string[] deviceName;
        public string[] devicePos;
        public bool isBindToPLC;
        public Form1 mainFrm;
        public OPCServer opcServer = new OPCServer();
        public Button[] pic;
        public string[] returnDB;
        public string[] showInf;
        public int[] xCoord;
        public int[] xInc;
        public int[] yCoord;
        public int[] yInc;

        public KZAN_Dev(Form1 mainfrom, DataTable bt)
        {
            this.mainFrm = mainfrom;

            this.deviceId = new string[bt.Rows.Count];
            this.deviceName = new string[bt.Rows.Count];
            this.devicePos = new string[bt.Rows.Count];
            this.showInf = new string[bt.Rows.Count];
            this.commandDB = new string[bt.Rows.Count];
            this.returnDB = new string[bt.Rows.Count];
            this.commandHandle = new int[bt.Rows.Count];
            this.xCoord = new int[bt.Rows.Count];
            this.yCoord = new int[bt.Rows.Count];
            this.xInc = new int[bt.Rows.Count];
            this.yInc = new int[bt.Rows.Count];
            this.pic = new Button[bt.Rows.Count];
            int index = 0;
            foreach (DataRow row in bt.Rows)
            {
                this.deviceId[index] = row["device_id"].ToString();
                this.deviceName[index] = row["device_name"].ToString();
                this.devicePos[index] = row["device_pos"].ToString();
                this.showInf[index] = row["show_inf"].ToString();
                this.commandDB[index] = row["command_db"].ToString();
                this.returnDB[index] = row["return_db"].ToString();
                this.xCoord[index] = Convert.ToInt32("0" + row["x_coord"].ToString());
                this.yCoord[index] = Convert.ToInt32("0" + row["y_coord"].ToString());
                this.xInc[index] = Convert.ToInt32("0" + row["x_inc"].ToString());
                this.yInc[index] = Convert.ToInt32("0" + row["y_inc"].ToString());
                this.pic[index] = new Button();
                this.pic[index].Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
                this.pic[index].Size = new Size(this.xInc[index], this.yInc[index]);
                this.pic[index].Location = new Point(this.xCoord[index], this.yCoord[index]);
                this.pic[index].BackColor = Color.Transparent;
                this.pic[index].Tag = index.ToString();
                this.pic[index].Name = row["device_id"].ToString();
                this.pic[index].AutoSize = false;
                this.pic[index].Text = this.showInf[index];
                this.pic[index].TextAlign = ContentAlignment.MiddleCenter;
                this.pic[index].Click += new EventHandler(this.pic_Click);
                index++;
            }
        }

        public bool BindToPLC()
        {
            this.opcServer = new OPCServer();
            if (!this.opcServer.Connect())
            {
                return false;
            }
            if (!this.opcServer.AddGroup())
            {
                return false;
            }
            int num = 1;
            if (this.commandDB != null)
            {
                OPCITEMDEF[] items = new OPCITEMDEF[this.commandDB.Length];
                for (int i = 0; i < this.commandDB.Length; i++)
                {
                    items[i].szAccessPath = "";
                    items[i].bActive = 1;
                    items[i].hClient = num;
                    items[i].dwBlobSize = 1;
                    items[i].pBlob = IntPtr.Zero;
                    items[i].vtRequestedDataType = 8;
                    items[i].szItemID = $"S7:[S7_Connection_1]{this.commandDB[i]}";
                    num++;
                }
                if (!this.opcServer.AddItems(items, this.commandHandle))
                {
                    return false;
                }
            }
            this.isBindToPLC = true;
            return this.isBindToPLC;
        }

        public void ManualCommandWrite(int tag, string cmdId)
        {
            object[] values = new object[1];
            int[] itemHandle = new int[1];
            values[0] = cmdId;
            itemHandle[0] = this.commandHandle[tag];
            this.opcServer.SyncWrite(values, itemHandle);
        }

        public void pic_Click(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32((sender as Button).Tag.ToString());
            this.ManualCommandWrite(tag, this.returnDB[tag]);
        }
    }
}


