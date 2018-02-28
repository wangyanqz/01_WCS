namespace WCS.DevSystem
{
    using MySql.Data.MySqlClient;
    using OpcRcw.Da;
    using System;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WCS;
    using WCS.PlcSystem;

    public class SSJ_Dev : DeviceBase, IOPCDataCallback
    {
        public MySqlConnection dbConnFJK;
        public int li_ms_index;
        public Form1 mainFrm;
        public int[] offNumber;
        public OPCServerAsyn opcServerAsyn;
        public int FJK_Num;
        public SSJ_Dev(Form1 mainfrom)
        {
            this.mainFrm = mainfrom;
            this.FJK_Num = mainFrm.port_num;
            base.deviceId = new string[FJK_Num];
            base.showInf = new string[FJK_Num];
            base.controlDB = new string[FJK_Num];
            base.statusDB = new string[FJK_Num];
            base.failureDB = new string[FJK_Num];
            base.controlHandle = new int[FJK_Num];
            base.statusHandle = new int[FJK_Num];
            base.failureHandle = new int[FJK_Num];
            base.controlType = new string[FJK_Num];
            base.statusType = new string[FJK_Num];
            base.deviceStatus = new int[FJK_Num];
            base.deviceFailure = new int[FJK_Num];
            this.offNumber = new int[FJK_Num];
            base.xCoord = new int[FJK_Num];
            base.yCoord = new int[FJK_Num];
            base.xInc = new int[FJK_Num];
            base.yInc = new int[FJK_Num];
            base.pic = new Label[FJK_Num];

            int index = 0;
            int XPos = 350;
            int YPos = 58;

            for (int i=0;i<FJK_Num; i++)
            {
                base.deviceId[index] = (index+1).ToString();
                base.showInf[index] = (index + 1).ToString();
                base.controlDB[index] = "DB3,W" +(index * 2).ToString(); // 分拣口启停
                base.statusDB[index] = "DB6,W" + (index * 2).ToString();// 分拣口集包状态
                base.failureDB[index] = "DB10,W" + (index * 2).ToString();// 分拣口传感器状态
                base.xCoord[index] = XPos;
                base.yCoord[index] = YPos;
                base.xInc[index] = 50;
                base.yInc[index] = 40;
                base.controlType[index] ="0";
                base.statusType[index] = "0";
                this.offNumber[index] = 0;
                //if (base.statusType[index] == "KZG")
                //{
                //    this.li_ms_index = index;
                //}
                base.pic[index] = new Label();
                base.pic[index].Size = new Size(base.xInc[index], base.yInc[index]);
                base.pic[index].Location = new Point(base.xCoord[index], base.yCoord[index]);
                base.pic[index].BackColor = Color.Lime;
                base.pic[index].BorderStyle = BorderStyle.FixedSingle;
                base.pic[index].Tag = index.ToString();
                base.pic[index].Name = (index + 1).ToString();
                base.pic[index].AutoSize = false;
                base.pic[index].Text = base.showInf[index];
                base.pic[index].TextAlign = ContentAlignment.MiddleCenter;
                base.pic[index].Font = new Font("宋体", 12f);
                base.pic[index].DoubleClick += new EventHandler(this.pic_DoubleClick);

                if (i < 18)
                {
                    XPos += 60;
                    //YPos =200;
                }
                else
                {
                    XPos -= 60;
                    YPos = 510;
                }
                if (i == 17)
                {
                    XPos -= 60;
                    YPos = 510;
                    //YPos =200;
                }

                //XPos += 120;
                //YPos = 200;

                index++;
            }
            Control.CheckForIllegalCrossThreadCalls = false;
            
        }

        public override bool BindToPLC()
        {
            base.opcServer.plcip = this.mainFrm.plc_ip;
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
            if (base.controlDB != null)
            {
                OPCITEMDEF[] items = new OPCITEMDEF[base.controlDB.Length];
                for (int i = 0; i < base.controlDB.Length; i++)
                {
                    items[i].szAccessPath = "";
                    items[i].bActive = 1;
                    items[i].hClient = num;
                    items[i].dwBlobSize = 1;
                    items[i].pBlob = IntPtr.Zero;
                    items[i].vtRequestedDataType = 8;
                    items[i].szItemID = $"S7:[S7_Connection_1]{base.controlDB[i]}";
                    num++;
                }
                if (!base.opcServer.AddItems(items, base.controlHandle))
                {
                    return false;
                }
            }
            if (base.failureDB != null)
            {
                OPCITEMDEF[] opcitemdefArray2 = new OPCITEMDEF[base.failureDB.Length];
                for (int j = 0; j < base.failureDB.Length; j++)
                {
                    opcitemdefArray2[j].szAccessPath = "";
                    opcitemdefArray2[j].bActive = 1;
                    opcitemdefArray2[j].hClient = num;
                    opcitemdefArray2[j].dwBlobSize = 1;
                    opcitemdefArray2[j].pBlob = IntPtr.Zero;
                    opcitemdefArray2[j].vtRequestedDataType = 8;
                    opcitemdefArray2[j].szItemID = $"S7:[S7_Connection_1]{base.failureDB[j]}";
                    num++;
                }
                if (!base.opcServer.AddItems(opcitemdefArray2, base.failureHandle))
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
                     //items[0].szItemID = $"Logical.test3";
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

        public string getErrorInf(int tag, ref bool if_error)
        {
            string str = string.Empty;
            if_error = true;
            if (base.statusType[tag] == "KZG")
            {
                if (base.deviceStatus[tag] != 0)
                {
                    if ((base.deviceStatus[tag] & 1) != 0)
                    {
                        str = str + "启动位1光电异常;";
                    }
                    if ((base.deviceStatus[tag] & 2) != 0)
                    {
                        str = str + "校验光电1异常;";
                    }
                    if ((base.deviceStatus[tag] & 4) != 0)
                    {
                        str = str + "启动位2光电异常;";
                    }
                    if ((base.deviceStatus[tag] & 8) != 0)
                    {
                        str = str + "校验光电2异常;";
                    }
                    if ((base.deviceStatus[tag] & 0x10) != 0)
                    {
                        str = str + "变频器1过载;";
                    }
                    if ((base.deviceStatus[tag] & 0x20) != 0)
                    {
                        str = str + "变频器2过载;";
                    }
                    if ((base.deviceStatus[tag] & 0x40) != 0)
                    {
                        str = str + "主线速度异常;";
                    }
                    return (base.deviceId[tag] + "设备故障：" + str);
                }
                str = base.deviceId[tag] + "设备故障：无";
                if_error = false;
                return str;
            }
            if (base.deviceStatus[tag] != 0)
            {
                if ((base.deviceStatus[tag] & 4) != 0)
                {
                    str = str + base.deviceId[tag] + "：急停;";
                    if_error = false;
                }
                if ((base.deviceStatus[tag] & 1) != 0)
                {
                    str = str + base.deviceId[tag] + "故障：光电异常;";
                }
                if ((base.deviceStatus[tag] & 2) != 0)
                {
                    string str2 = str;
                    str = str2 + base.deviceId[tag] + "故障：跳闸小车编号" + base.deviceFailure[tag].ToString().PadLeft(3, '0') + ";";
                }
                return str;
            }
            str = base.deviceId[tag] + "设备故障：无";
            if_error = false;
            return str;
        }

        public void ManualCommandWrite(int tag, string cmdId)
        {
            object[] values = new object[1];
            int[] itemHandle = new int[1];
            values[0] = cmdId;
            itemHandle[0] = base.controlHandle[tag];
            base.opcServer.SyncWrite(values, itemHandle);
        }

        public bool ManulCommand(int tag, string cmdId)
        {
            switch (this.mainFrm.deviceControlDic[base.controlType[tag] + cmdId].condition)
            {
                case "0":
                    if (this.mainFrm.plcSystemMS.is_auto)
                    {
                        MessageBox.Show("不满足指令条件,必须工作在手动模式！");
                    }
                    else
                    {
                        this.ManualCommandWrite(tag, cmdId);
                    }
                    break;

                case "1":
                    this.ManualCommandWrite(tag, cmdId);
                    break;

                case "2":
                    if (base.deviceStatus[tag] != 0)
                    {
                        this.ManualCommandWrite(tag, cmdId);
                    }
                    else
                    {
                        MessageBox.Show("不满足指令条件，设备必须在故障状态！");
                    }
                    break;

                default:
                    MessageBox.Show("系统没有设置该指令的下发条件！");
                    break;
            }
            return true;
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
                        if (base.deviceStatus[phClientItems[i] - 1] != 0)
                        {
                            bool flag = true;
                            string content = this.getErrorInf(phClientItems[i] - 1, ref flag);
                            if (flag)
                            {
                                this.mainFrm.AddErrToListView(content);
                            }
                            else
                            {
                                this.mainFrm.AddSuccessToListView(content);
                            }
                            LogHelper.LogSimlpleString(DateTime.Now.ToString() + content);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"更新分拣口状态失败:-{exception.Message}");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + $"更新分拣口状态失败:-{exception.Message}");
            }
        }

        public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            //throw new NotImplementedException();
        }

        public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
        {
            throw new NotImplementedException();
        }

        public void pic_DoubleClick(object sender, EventArgs e)
        {
            int tag = Convert.ToInt32((sender as Label).Tag.ToString());
            new FrmSSJ(this, tag).ShowDialog();
        }

        public void RefreshDisplay()
        {
            this.RefreshDisplay(this.li_ms_index);
        }

        public void RefreshDisplay(int i)
        {
            if (base.statusType[i] == "KZG")
            {
                if (base.deviceStatus[i] != 0)
                {
                    base.pic[i].BackColor = Color.Orange;
                }
                else
                {
                    if (!this.mainFrm.plcSystemMS.is_auto)
                    {
                        base.pic[i].BackColor = Color.Yellow;
                    }
                    if (!this.mainFrm.plcSystemMS.is_runing)
                    {
                        base.pic[i].BackColor = Color.Gray;
                    }
                    if (this.mainFrm.plcSystemMS.is_stop)
                    {
                        base.pic[i].BackColor = Color.Red;
                    }
                    if ((this.mainFrm.plcSystemMS.is_auto && this.mainFrm.plcSystemMS.is_runing) && !this.mainFrm.plcSystemMS.is_stop)
                    {
                        base.pic[i].BackColor = Color.DarkGreen;
                    }
                }
            }
            else if (base.deviceStatus[i] != 0)
            {
                return;
                if ((base.deviceStatus[i] & 1) != 0)
                {
                    base.pic[i].BackColor = Color.Orange;
                }
                if ((base.deviceStatus[i] & 2) != 0)
                {
                    base.pic[i].BackColor = Color.Orange;
                    this.RefreshDisplayFailure(i);
                }
                if ((base.deviceStatus[i] & 4) != 0)
                {
                    base.pic[i].BackColor = Color.Red;
                    try
                    {
                        if (this.offNumber[i] >= 5)
                        {
                            int num = i + 1;
                            //new MySqlCommand("update td_port_inf set port_status='1',num=num+1 where port_id='" + num.ToString() + "'", this.dbConnFJK).ExecuteNonQuery();
                            //this.offNumber[i] = 0;
                           // this.mainFrm.AddSuccessToListView("更新分拣口" + ((i + 1)).ToString() + "停用状态和集包袋数量成功");
                           // LogHelper.LogSimlpleString(DateTime.Now.ToString() + "更新分拣口" + ((i + 1)).ToString() + "停用状态和集包袋数量成功");
                        }
                        else
                        {
                            int num4 = i + 1;
                           // new MySqlCommand("update td_port_inf set port_status='1' where port_id='" + num4.ToString() + "'", this.dbConnFJK).ExecuteNonQuery();
                          //  this.mainFrm.AddSuccessToListView("更新分拣口" + ((i + 1)).ToString() + "停用状态成功");
                         //   LogHelper.LogSimlpleString(DateTime.Now.ToString() + "更新分拣口" + ((i + 1)).ToString() + "停用状态成功");
                        }
                    }
                    catch (Exception exception)
                    {
                        int num7 = i + 1;
                        this.mainFrm.AddErrToListView(string.Format("更新分拣口" + num7.ToString() + "停用状态失败:-{0}", exception.Message));
                        int num8 = i + 1;
                        LogHelper.LogSimlpleString(DateTime.Now.ToString() + string.Format("更新分拣口" + num8.ToString() + "停用状态失败:-{0}", exception.Message));
                    }
                }
            }
            else
            {
                return;
                base.pic[i].BackColor = Color.DarkGreen;
                base.deviceFailure[i] = 0;
                try
                {
                    int num9 = i + 1;
                    new MySqlCommand("update td_port_inf set port_status='0' where port_status='1' and port_id='" + num9.ToString() + "'", this.dbConnFJK).ExecuteNonQuery();
                    LogHelper.LogSimlpleString(DateTime.Now.ToString() + "更新分拣口" + ((i + 1)).ToString() + "启用状态成功");
                }
                catch (Exception exception2)
                {
                    int num11 = i + 1;
                    this.mainFrm.AddErrToListView(string.Format("更新分拣口" + num11.ToString() + "启用状态失败:-{0}", exception2.Message));
                    int num12 = i + 1;
                    LogHelper.LogSimlpleString(DateTime.Now.ToString() + string.Format("更新分拣口" + num12.ToString() + "启用状态失败:-{0}", exception2.Message));
                }
            }
        }

        public bool RefreshDisplayFailure(int index)
        {
            try
            {
                if (!base.isBindToPLC)
                {
                    this.BindToPLC();
                }
                object[] values = new object[1];
                int[] itemHandle = new int[] { base.failureHandle[index] };
                base.opcServer.SyncRead(values, itemHandle);
                base.deviceFailure[index] = int.Parse(values[0].ToString());
                return true;
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"更新指定分拣口跳闸小车编号失败:-{exception.Message}");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + $"更新指定分拣口跳闸小车编号失败:-{exception.Message}");
                return false;
            }
        }
    }
}


