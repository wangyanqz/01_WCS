namespace WCS.DevSystem
{
    using OpcRcw.Da;
    using System;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WCS;
    using WCS.PlcSystem;

    public class Car_Pos : IOPCDataCallback
    {
        public string commandPlcDB = "DB4,W0";// 启用禁用PLC
        public int[] plcHandle = new int[1];
        public string[] W_CarCtrDB;// 小车启用禁用DB
        public string[] W_TaskDownDB;//落包口DB
        public int[] W_CarCtrlHandle;
        public int[] commandDownHandle;//落包口DB标识
        public string caridDB = "DB9,W0";// 当前小车编号
        public int[] caridHandle = new int[1];
        public int carNum  = 62 + 1; //小车个数, 数组元素0 没有用
        public string[] R_CarStatus;
        public int[] returnHandle;

        public int[] loadStatus;//小车当前状态，0-无货 1-有货
        public int[] deviceStatus;
        public bool isBindToPLC;
        public DateTime last_time = DateTime.Now.AddSeconds(-10.0);
        public double ldb_time = 100.0;
        private int li_carid;
        public Form1 mainFrm;
        public OPCServer opcServer = new OPCServer();
        public OPCServerAsyn opcServerAsyn;
        public Label[] pic;
        
        public int[] xCoord;
        public int[] xInc;
        public int[] yCoord;
        public int[] yInc;

        public Car_Pos(Form1 mainfrom)
        {
            this.mainFrm = mainfrom;
            this.carNum = this.mainFrm.car_num;
            this.ldb_time = ((this.carNum - 1) * 0.5) + 6.0;
            this.W_CarCtrDB = new string[carNum ];
            this.W_TaskDownDB = new string[carNum];
            this.R_CarStatus = new string[carNum];
            this.W_CarCtrlHandle = new int[carNum ];
            this.commandDownHandle = new int[carNum];
            this.returnHandle = new int[carNum];
            this.deviceStatus = new int[carNum];
            this.xCoord = new int[carNum];
            this.yCoord = new int[carNum];
            this.xInc = new int[carNum ];
            this.yInc = new int[carNum];
            // label控件
            this.pic = new Label[carNum];
           // int index = 0;
            int XPos = 350;
            int YPos = 100;

            for (int i=0; i< carNum; i++)
            {
                this.W_CarCtrDB[i] = "DB2,W" + (i * 2).ToString();// 禁用和启用小车DB
                this.W_TaskDownDB[i] = "DB1,DINT" + (i * 4).ToString();//DINT类型，4个字节，手动落包DB
                this.R_CarStatus[i] = "DB2,W" + (i * 2).ToString();//xwc 异步读取禁用和启用小车DB
                this.deviceStatus[i] = 0;
                
                // label位置坐标
                this.xCoord[i] = XPos;
                this.yCoord[i] = YPos;
                // label尺寸大小
                this.xInc[i] = 45;
                this.yInc[i] = 45;

                // 构造label
                this.pic[i] = new Label();
                this.pic[i].Size = new Size(this.xInc[i], this.yInc[i]);
                this.pic[i].Location = new Point(this.xCoord[i], this.yCoord[i]);
                this.pic[i].BackColor = Color.Gray;
                this.pic[i].BorderStyle = BorderStyle.FixedSingle;
                this.pic[i].Tag = i.ToString();
                this.pic[i].Name = i.ToString();
                this.pic[i].AutoSize = false;
                this.pic[i].Text = i.ToString();
                this.pic[i].TextAlign = ContentAlignment.MiddleCenter;
                this.pic[i].Font = new Font("宋体", 12f);
                this.pic[i].DoubleClick += new EventHandler(this.pic_DoubleClick);//绑定双击事件
                if (i == 0)
                    continue;
                if (i >= 24 && i <= 31)
                {
                    //XPos -=45;
                    YPos += 45;
                    
                    continue;
                }
                if (i >= 55 && i <= 62)
                {
                    //XPos = 100;
                    YPos -= 45;

                    continue;
                }
              

                if (i < 25)
                {
                    XPos += 45;
                    //YPos =200;
                }
                else
                {
                    XPos -= 45;
                    //YPos = 300;
                }
                
                
            }
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        /*public Car_Pos(Form1 mainfrom, DataTable bt)
        {
            this.mainFrm = mainfrom;
            this.ldb_time = (this.mainFrm.car_num * 0.5) + 6.0;
            this.commandDB = new string[carNum];
            this.returnDB = new string[carNum];
            this.commandHandle = new int[carNum];
            this.returnHandle = new int[1];
            this.deviceStatus = new int[carNum];
            this.xCoord = new int[carNum];
            this.yCoord = new int[carNum];
            this.xInc = new int[carNum];
            this.yInc = new int[carNum];
            this.pic = new Label[carNum];
            int index = 0;
            foreach (DataRow row in bt.Rows)
            {
                this.commandDB[index] = "DB17,B" + index.ToString();
                this.returnDB[index] = "DB17,B" + index.ToString();
                this.deviceStatus[index] = 0;
                this.xCoord[index] = Convert.ToInt32("0" + row["x_coord"].ToString());
                this.yCoord[index] = Convert.ToInt32("0" + row["y_coord"].ToString());
                this.xInc[index] = Convert.ToInt32("0" + row["x_inc"].ToString());
                this.yInc[index] = Convert.ToInt32("0" + row["y_inc"].ToString());
                this.pic[index] = new Label();
                this.pic[index].Size = new Size(this.xInc[index], this.yInc[index]);
                this.pic[index].Location = new Point(this.xCoord[index], this.yCoord[index]);
                this.pic[index].BackColor = Color.DeepSkyBlue;
                this.pic[index].BorderStyle = BorderStyle.FixedSingle;
                this.pic[index].Tag = index.ToString();
                this.pic[index].Name = row["device_id"].ToString();
                this.pic[index].AutoSize = false;
                this.pic[index].Text = row["show_inf"].ToString();
                this.pic[index].TextAlign = ContentAlignment.MiddleCenter;
                this.pic[index].Font = new Font("宋体", 7f);
                this.pic[index].DoubleClick += new EventHandler(this.pic_DoubleClick);
                index++;
            }
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        */
        public bool BindToPLC(int dwRequestedUpdateRate)
        {
            this.opcServer.plcip = this.mainFrm.plc_ip;
            this.opcServer = new OPCServer();
             if (!this.opcServer.Connect())
            {
                return false;
            }
            if (!this.opcServer.AddGroup(dwRequestedUpdateRate))
            {
                return false;
            }
            int num = 1;
            

            if (this.W_CarCtrDB != null)
            {
                OPCITEMDEF[] items = new OPCITEMDEF[this.W_CarCtrDB.Length];
                for (int i = 0; i < this.W_CarCtrDB.Length; i++)
                {
                    items[i].szAccessPath = "";
                    items[i].bActive = 1;
                    items[i].hClient = num;
                    items[i].dwBlobSize = 1;
                    items[i].pBlob = IntPtr.Zero;
                    items[i].vtRequestedDataType = 8;
                    items[i].szItemID = $"S7:[S7_Connection_1]{this.W_CarCtrDB[i]}";
                    num++;
                }
                if (!this.opcServer.AddItems(items, this.W_CarCtrlHandle))
                {
                    return false;
                }
            }

            if (this.W_TaskDownDB != null)
            {
                OPCITEMDEF[] items = new OPCITEMDEF[this.W_TaskDownDB.Length];
                for (int i = 0; i < this.W_TaskDownDB.Length; i++)
                {
                    items[i].szAccessPath = "";
                    items[i].bActive = 1;
                    items[i].hClient = num;
                    items[i].dwBlobSize = 1;
                    items[i].pBlob = IntPtr.Zero;
                    items[i].vtRequestedDataType = 8;
                    //items[i].szItemID = this.W_TaskDownDB[i];
                    items[i].szItemID = $"S7:[S7_Connection_1]{this.W_TaskDownDB[i]}";
                    num++;
                }
                if (!this.opcServer.AddItems(items, this.commandDownHandle))
                {
                    return false;
                }
            }

            OPCITEMDEF[] items1 = new OPCITEMDEF[1];
            items1[0].szAccessPath = "";
            items1[0].bActive = 1;
            items1[0].hClient = num;
            items1[0].dwBlobSize = 1;
            items1[0].pBlob = IntPtr.Zero;
            items1[0].vtRequestedDataType = 8;
            items1[0].szItemID = $"S7:[S7_Connection_1]{this.caridDB}";
            num++;
            if (!this.opcServer.AddItems(items1, this.caridHandle))
            {
                return false;
            }
            OPCITEMDEF[] items2 = new OPCITEMDEF[1];
            items2[0].szAccessPath = "";
            items2[0].bActive = 1;
            items2[0].hClient = num;
            items2[0].dwBlobSize = 1;
            items2[0].pBlob = IntPtr.Zero;
            items2[0].vtRequestedDataType = 8;
            items2[0].szItemID = $"S7:[S7_Connection_1]{this.commandPlcDB}";
            num++;
            if (!this.opcServer.AddItems(items2, this.plcHandle))
            {
                return false;
            }
            if (!this.BindToPLCAsyn())
            {
                MessageBox.Show("异步添加小车启用禁用状态失败！");
                return false;
            }
            this.isBindToPLC = true;
            return this.isBindToPLC;
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
            if (this.R_CarStatus != null)
            {
                OPCITEMDEF[] items = new OPCITEMDEF[this.R_CarStatus.Length];
                for (int i = 0; i < this.R_CarStatus.Length; i++)
                {
                    items[i].szAccessPath = "";
                    items[i].bActive = 1;
                    items[i].hClient = num;
                    items[i].dwBlobSize = 1;
                    items[i].pBlob = IntPtr.Zero;
                    items[i].vtRequestedDataType = 8;
                    items[i].szItemID = $"S7:[S7_Connection_1]{this.R_CarStatus[i]}";
                    num++;
                }
                if (!this.opcServerAsyn.AddItems(items, this.returnHandle))
                {
                    return false;
                }
            }
            this.opcServerAsyn.ReadPlc(this.returnHandle);
            if (!this.opcServerAsyn.DataChange())
            {
                return false;
            }
            return true;
        }
        public void CtrPlcStatus( int nStatus) // 1-- 启动，2--停止 db4
        {
            object[] values = new object[1];
            values[0] = nStatus;
            this.opcServer.SyncWrite(values, this.plcHandle);

        }
        public void ManualCommandWrite(int tag, string cmdId)
        {
            object[] values = new object[1];
            int[] itemHandle = new int[1];
            values[0] = cmdId;
            itemHandle[0] = this.W_CarCtrlHandle[tag];
            this.opcServer.SyncWrite(values, itemHandle);
        }

        public bool ManualCommandDown(int tag, string cmdId)
        {
            object[] values = new object[1];
            int[] itemHandle = new int[1];
            values[0] = cmdId;
            itemHandle[0] = this.commandDownHandle[tag];
            bool flag = false;
            flag  = this.opcServer.SyncWrite(values, itemHandle);

            if(flag == true)
            {
                this.mainFrm.car_dev.loadStatus[tag] = 1;
            }
            return flag;
        }
        public bool CarCtrl(int tag, string cmdId)
        {
            object[] values = new object[1];
            int[] itemHandle = new int[1];
            values[0] = cmdId;
            itemHandle[0] = this.W_CarCtrlHandle[tag];
            bool flag = false;
            flag = this.opcServer.SyncWrite(values, itemHandle);

            if (flag == true)
            {
                //增加小车状态修改 
               // this.mainFrm.car_dev.loadStatus[tag - 1] = 1;
            }
            return flag;
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
                        this.deviceStatus[phClientItems[i] - 1] = int.Parse(pvValues[i].ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"异步更新分拣小车启用状态失败:-{exception.Message}");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + $"异步更新分拣小车启用状态失败:-{exception.Message}");
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
                        this.deviceStatus[phClientItems[i] - 1] = int.Parse(pvValues[i].ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"异步读更新分拣小车启用状态失败:-{exception.Message}");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + $"异步读更新分拣小车启用状态失败:-{exception.Message}");
            }
        }

        public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
        {
            throw new NotImplementedException();
        }

        public void pic_DoubleClick(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32((sender as Label).Text.ToString());
            new FrmCarpos(this, tag).ShowDialog();
        }

        public int GetCarid()//获取当前小车编号，用于实时刷新小车位置
        {
            try
            {
                object[] values = new object[1];
                this.opcServer.SyncRead(values, this.caridHandle);
                return Convert.ToInt32(values[0]);
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"获取当前小车编号失败:-{exception.Message}");
                LogHelper.LogSimlpleString($"获取当前小车编号失败:-{exception.Message}");
                return 0;
            }
        }
        public void RefreshDisplay()
        {
            this.li_carid = this.GetCarid();
      
            if ((this.li_carid != 0) && (this.li_carid <= this.mainFrm.car_num - 1))
            {
                this.li_carid--;
                int index = this.pic.Length - 1;
                for (int i = this.pic.Length - 1; i > 0; i--)
                {
                    //index ++ ;
                    //if (index > this.mainFrm.car_num - 1)
                    //{
                    //    index = 0;
                    //}index
                    index = i;
                    if (this.li_carid == 0)
                    {
                        this.li_carid = this.mainFrm.car_num - 1;
                    }
                    this.pic[index].Text = this.li_carid.ToString();
                    if (this.deviceStatus[this.li_carid] == 1)//xwc 小车禁用
                    {
                        this.pic[index].BackColor = Color.Red;
                    }
                    else
                    {
                        
                        
                        {
                            switch (this.mainFrm.car_dev.loadStatus[this.li_carid])
                            {
                                case 0:
                                    this.pic[index].BackColor = Color.Gray;
                                    goto Label_025C;

                                case 1:
                                    this.pic[index].BackColor = Color.Lime;
                                    goto Label_025C;

                                case 2:
                                    this.pic[index].BackColor = Color.Yellow;
                                    goto Label_025C;

                                case 3:
                                    this.pic[index].BackColor = Color.Yellow;
                                    goto Label_025C;

                                case 4:
                                    this.pic[index].BackColor = Color.Blue;
                                    goto Label_025C;
                            }
                            this.pic[index].BackColor = Color.Gray;
                        }
                    }
                Label_025C:
                    this.li_carid--;
                }
            }
        }
    }
}


