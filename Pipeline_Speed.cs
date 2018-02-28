namespace WCS.DevSystem
{
    using OpcRcw.Da;
    using System;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WCS;
    using WCS.PlcSystem;

    public class Pipeline_Speed : DeviceBase, IOPCDataCallback
    {
        public Form1 mainFrm;
        public OPCServerAsyn opcServerAsyn;
        public Button[] pic;

        public Pipeline_Speed(Form1 mainfrom, DataTable bt)
        {
            this.mainFrm = mainfrom;
            base.deviceId = new string[bt.Rows.Count];
            base.devicePos = new string[bt.Rows.Count];
            base.showInf = new string[bt.Rows.Count];
            base.commandDB = new string[bt.Rows.Count];
            base.statusDB = new string[bt.Rows.Count];
            base.commandHandle = new int[bt.Rows.Count];
            base.statusHandle = new int[bt.Rows.Count];
            base.statusType = new string[bt.Rows.Count];
            base.deviceStatus = new int[bt.Rows.Count];
            base.xCoord = new int[bt.Rows.Count];
            base.yCoord = new int[bt.Rows.Count];
            base.xInc = new int[bt.Rows.Count];
            base.yInc = new int[bt.Rows.Count];
            this.pic = new Button[bt.Rows.Count];
            int index = 0;
            foreach (DataRow row in bt.Rows)
            {
                base.deviceId[index] = row["device_id"].ToString();
                base.devicePos[index] = row["device_pos"].ToString();
                base.showInf[index] = row["show_inf"].ToString();
                base.commandDB[index] = row["status_db"].ToString();
                base.statusDB[index] = row["status_db"].ToString();
                base.xCoord[index] = Convert.ToInt32("0" + row["x_coord"].ToString());
                base.yCoord[index] = Convert.ToInt32("0" + row["y_coord"].ToString());
                base.xInc[index] = Convert.ToInt32("0" + row["x_inc"].ToString());
                base.yInc[index] = Convert.ToInt32("0" + row["y_inc"].ToString());
                base.statusType[index] = row["status_type"].ToString();
                this.pic[index] = new Button();
                this.pic[index].Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
                this.pic[index].Size = new Size(base.xInc[index], base.yInc[index]);
                this.pic[index].Location = new Point(base.xCoord[index], base.yCoord[index]);
                this.pic[index].BackColor = Color.Transparent;
                this.pic[index].Tag = index.ToString();
                this.pic[index].Name = row["device_id"].ToString();
                this.pic[index].AutoSize = false;
                this.pic[index].Text = base.showInf[index];
                this.pic[index].TextAlign = ContentAlignment.MiddleCenter;
                this.pic[index].Click += new EventHandler(this.pic_DoubleClick);
                index++;
            }
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public override bool BindToPLC()
        {
            base.opcServer = new OPCServer();
            if (!base.opcServer.Connect())
            {
                return false;
            }
            if (!base.opcServer.AddGroup())
            {
                return false;
            }
            int num = 1;
            if (base.commandDB != null)
            {
                OPCITEMDEF[] items = new OPCITEMDEF[base.commandDB.Length];
                for (int i = 0; i < base.commandDB.Length; i++)
                {
                    items[i].szAccessPath = "";
                    items[i].bActive = 1;
                    items[i].hClient = num;
                    items[i].dwBlobSize = 1;
                    items[i].pBlob = IntPtr.Zero;
                    items[i].vtRequestedDataType = 8;
                    items[i].szItemID = $"S7:[S7_Connection_1]{base.commandDB[i]}";
                    num++;
                }
                if (!base.opcServer.AddItems(items, base.commandHandle))
                {
                    return false;
                }
            }
            if (!this.BindToPLCAsyn())
            {
                return false;
            }
            base.isBindToPLC = true;
            return base.isBindToPLC;
        }

        public bool BindToPLCAsyn()
        {
            this.opcServerAsyn = new OPCServerAsyn();
            if (!this.opcServerAsyn.Connect())
            {
                return false;
            }
            if (!this.opcServerAsyn.AddGroup(this))
            {
                return false;
            }
            int num = 1;
            if (base.statusDB != null)
            {
                OPCITEMDEF[] items = new OPCITEMDEF[base.statusDB.Length];
                for (int i = 0; i < base.statusDB.Length; i++)
                {
                    items[i].szAccessPath = "";
                    items[i].bActive = 1;
                    items[i].hClient = num;
                    items[i].dwBlobSize = 1;
                    items[i].pBlob = IntPtr.Zero;
                    items[i].vtRequestedDataType = 8;
                    items[i].szItemID = $"S7:[S7_Connection_1]{base.statusDB[i]}";
                    num++;
                }
                if (!this.opcServerAsyn.AddItems(items, base.statusHandle))
                {
                    return false;
                }
            }
            this.opcServerAsyn.ReadPlc(base.statusHandle);
            if (!this.opcServerAsyn.DataChange())
            {
                return false;
            }
            return true;
        }

        public void ManualCommandWrite(int tag, string cmdId)
        {
            string str;
            object[] values = new object[1];
            int[] itemHandle = new int[1];
            values[0] = cmdId;
            itemHandle[0] = base.commandHandle[tag];
            base.opcServer.SyncWrite(values, itemHandle);
            if (cmdId == "0")
            {
                str = "1米/秒";
            }
            else
            {
                str = "1.2米/秒";
            }
            this.mainFrm.AddSuccessToListView("发送修改主线速度命令：" + str);
            LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "发送修改主线速度命令:" + str);
        }

        public void OnCancelComplete(int dwTransid, int hGroup)
        {
            throw new NotImplementedException();
        }

        public void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                for (int i = 0; i < dwCount; i++)
                {
                    if (pErrors[i] == 0)
                    {
                        base.deviceStatus[phClientItems[i] - 1] = int.Parse(pvValues[i].ToString());
                        this.RefreshDisplay(phClientItems[i] - 1);
                    }
                }
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"更新流水线运行速度失败:-{exception.Message}");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + $"更新流水线运行速度失败:-{exception.Message}");
            }
        }

        public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                for (int i = 0; i < dwCount; i++)
                {
                    if (pErrors[i] == 0)
                    {
                        base.deviceStatus[phClientItems[i] - 1] = int.Parse(pvValues[i].ToString());
                        this.RefreshDisplay(phClientItems[i] - 1);
                    }
                }
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"更新流水线运行速度失败:-{exception.Message}");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + $"更新流水线运行速度失败:-{exception.Message}");
            }
        }

        public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
        {
            throw new NotImplementedException();
        }

        public void pic_DoubleClick(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32((sender as Button).Tag.ToString());
            new FormPipeline_Speed(this, tag).ShowDialog();
        }

        public void RefreshDisplay(int i)
        {
            if (base.deviceStatus[i] == 0)
            {
                this.pic[i].Text = "主线速度:1米/秒";
            }
            else
            {
                this.pic[i].Text = "主线速度:1.2米/秒";
            }
        }
    }
}

