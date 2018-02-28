namespace WCS.DevSystem
{
    using MySql.Data.MySqlClient;
    using OpcRcw.Da;
    using System;
    using System.Collections.Concurrent;
    using System.Data;
    using System.Threading;
    using System.Windows.Forms;
    using WCS;
    using WCS.PlcSystem;

    public class Car_Dev : IOPCDataCallback
    {
        public string[] billCode;
        public int[] caridHandle = new int[1];
        public string[] commandDB;
        public int[] commandHandle;
        public int[] deviceStatusOld;
        public string[] if_paijian;
        public bool isBindToPLC;
        public DateTime[] last_time;
        public int li_finished_tasks;
        public int li_finished_tasks_error;
        public int li_finished_tasks_nodata;
        public int li_finished_tasks_noport;
        public int[] loadStatus;
        public Form1 mainFrm;
        public double max_handcode_sec = 100.0;
        public OPCServer opcServer = new OPCServer();
        public OPCServerAsyn opcServerAsyn;
        public int[] outport;
        public string[] person_id;
        public ConcurrentQueue<ItemOff> queueItemOff = new ConcurrentQueue<ItemOff>();
        public DateTime refresh_time = DateTime.Now;
        public string[] R_CarDownTaskDB;
        public string[] R_CarDownTaskDB1;
        public int[] returnHandle;
        public int[] returnHandle1;
        public int[] sourcePort;
        public string[] sub_siteid;
        public int taskid;
        public int carNum;

        public Car_Dev(Form1 mainfrom)
        {
            this.mainFrm = mainfrom;
            carNum = this.mainFrm.car_num;

            //this.commandDB = new string[carNum];
            //this.R_CarDownTaskDB1[i] = "DB2,W" + (i * 2).ToString();// 禁用和启用小车DB
            this.R_CarDownTaskDB = new string[carNum];
            this.R_CarDownTaskDB1 = new string[carNum];
            //this.commandHandle = new int[carNum];
            this.returnHandle = new int[carNum];
            this.returnHandle1 = new int[carNum];
            this.deviceStatusOld = new int[carNum];
            this.outport = new int[carNum];
            this.billCode = new string[carNum];
            this.loadStatus = new int[carNum];
            this.last_time = new DateTime[carNum];
            this.sourcePort = new int[carNum];
            this.person_id = new string[carNum];
            this.sub_siteid = new string[carNum];
            this.if_paijian = new string[carNum];
            this.max_handcode_sec = ((this.mainFrm.maxTurnNumer * this.mainFrm.car_num) * 0.5) / 1.2;
            int index = 0;
            for(int i=0; i < carNum; i++)
            {
                //this.commandDB[index] = row["command_db"].ToString();
                this.R_CarDownTaskDB[index] = "DB5,W" + (index* 2).ToString();
                this.R_CarDownTaskDB1[index] = "DB2,W" + (index * 2).ToString();
              
                this.deviceStatusOld[index] = 0;
                this.outport[index] = 0;
                this.billCode[index] = string.Empty;
                this.loadStatus[index] = 0;
                this.last_time[index] = DateTime.Now;
                this.sourcePort[index] = -1;
                this.person_id[index] = string.Empty;
                this.sub_siteid[index] = string.Empty;
                this.if_paijian[index] = "0";
                index++;
            }
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        //public Car_Dev(Form1 mainfrom, DataTable bt)
        //{
        //    this.mainFrm = mainfrom;
        //    this.commandDB = new string[carNum];
        //    this.R_CarDownTaskDB = new string[carNum];
        //    this.commandHandle = new int[carNum];
        //    this.returnHandle = new int[carNum];
        //    this.deviceStatusOld = new int[carNum];
        //    this.outport = new int[carNum];
        //    this.billCode = new string[carNum];
        //    this.loadStatus = new int[carNum];
        //    this.last_time = new DateTime[carNum];
        //    this.sourcePort = new int[carNum];
        //    this.person_id = new string[carNum];
        //    this.sub_siteid = new string[carNum];
        //    this.if_paijian = new string[carNum];
        //    this.max_handcode_sec = ((this.mainFrm.maxTurnNumer * this.mainFrm.car_num) * 0.5) / 1.2;
        //    int index = 0;
        //    foreach (DataRow row in bt.Rows)
        //    {
        //        this.commandDB[index] = row["command_db"].ToString();
        //        this.R_CarDownTaskDB[index] = row["return_db"].ToString();
        //        this.deviceStatusOld[index] = 0;
        //        this.outport[index] = 0;
        //        this.billCode[index] = string.Empty;
        //        this.loadStatus[index] = 0;
        //        this.last_time[index] = DateTime.Now;
        //        this.sourcePort[index] = -1;
        //        this.person_id[index] = string.Empty;
        //        this.sub_siteid[index] = string.Empty;
        //        this.if_paijian[index] = "0";
        //        index++;
        //    }
        //    Control.CheckForIllegalCrossThreadCalls = false;
        //}

        public bool BindToPLC(int dwRequestedUpdateRate)
        {
            
            //异步绑定
            if (!this.BindToPLCAsyn(100))
            {
                return false;
            }
             this.isBindToPLC = true;
            return this.isBindToPLC;
        }

        public bool BindToPLCAsyn(int dwRequestedUpdateRate)
        {
            this.opcServerAsyn = new OPCServerAsyn();
            this.opcServerAsyn.plcip = this.mainFrm.plc_ip;
            if (!this.opcServerAsyn.Connect())
            {
                return false;
            }
            if (!this.opcServerAsyn.AddGroup(this, dwRequestedUpdateRate))
            {
                return false;
            }
            int num = 1;
            if (this.R_CarDownTaskDB != null)
            {
                OPCITEMDEF[] items = new OPCITEMDEF[this.R_CarDownTaskDB.Length];
                for (int i = 0; i < this.R_CarDownTaskDB.Length; i++)
                {
                    items[i].szAccessPath = "";
                    items[i].bActive = 1;
                    items[i].hClient = num;
                    items[i].dwBlobSize = 1;
                    items[i].pBlob = IntPtr.Zero;
                    items[i].vtRequestedDataType = 8;
                    items[i].szItemID = $"S7:[S7_Connection_1]{this.R_CarDownTaskDB[i]}";
                    num++;
                }

                //for (int i = 0; i < 4; i++)
                //{
                //    items[i].szAccessPath = "";
                //    items[i].bActive = 1;
                //    items[i].hClient = num;
                //    items[i].dwBlobSize = 1;
                //    items[i].pBlob = IntPtr.Zero;
                //    items[i].vtRequestedDataType = 8;
                //    items[i].szItemID = $"Logical.test{num}";
                //    if (i == 3)
                //    {
                //        items[i].hClient = num + 10;
                //        items[i].szItemID = "Numeric.NewDataItem1";
                //    }
                //    num++;
                //}

                if (!this.opcServerAsyn.AddItems(items, this.returnHandle))
                {
                    return false;
                }
            }
            //if (this.R_CarDownTaskDB1 != null)
            //{
            //    OPCITEMDEF[] items = new OPCITEMDEF[this.R_CarDownTaskDB1.Length];
            //    for (int i = 0; i < this.R_CarDownTaskDB1.Length; i++)
            //    {
            //        items[i].szAccessPath = "";
            //        items[i].bActive = 1;
            //        items[i].hClient = num;
            //        items[i].dwBlobSize = 1;
            //        items[i].pBlob = IntPtr.Zero;
            //        items[i].vtRequestedDataType = 8;
            //        items[i].szItemID = $"S7:[S7_Connection_1]{this.R_CarDownTaskDB1[i]}";
            //        num++;
            //    }

            //    //for (int i = 0; i < 4; i++)
            //    //{
            //    //    items[i].szAccessPath = "";
            //    //    items[i].bActive = 1;
            //    //    items[i].hClient = num;
            //    //    items[i].dwBlobSize = 1;
            //    //    items[i].pBlob = IntPtr.Zero;
            //    //    items[i].vtRequestedDataType = 8;
            //    //    items[i].szItemID = $"Logical.test{num}";
            //    //    if (i == 3)
            //    //    {
            //    //        items[i].hClient = num + 10;
            //    //        items[i].szItemID = "Numeric.NewDataItem1";
            //    //    }
            //    //    num++;
            //    //}

            //    if (!this.opcServerAsyn.AddItems(items, this.returnHandle1))
            //    {
            //        return false;
            //    }
            // }


            this.opcServerAsyn.ReadPlc(this.returnHandle);
            this.opcServerAsyn.ReadPlc(this.returnHandle1);
            if (!this.opcServerAsyn.DataChange())
            {
                return false;
            }
            return true;
        }

        public int GetCarid()
        {
            try
            {
                if (!this.isBindToPLC)
                {
                    this.BindToPLC(100);
                }
                object[] values = new object[1];
                this.opcServer.SyncRead(values, this.caridHandle);
                return Convert.ToInt32(values[0]);
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"扫码位置1获取当前小车编号失败:-{exception.Message}");
                LogHelper.LogSimlpleString($"扫码位置1获取当前小车编号失败:-{exception.Message}");
                return 0;
            }
        }

        public void HandCodeTimeOut()
        {
            string errorMsg = string.Empty;
            string comandText = string.Empty;
            try
            {
                for (int i = 0; i < this.billCode.Length; i++)
                {
                    if ((this.outport[i] == 0) && ((this.loadStatus[i] == 2) || (this.loadStatus[i] == 3)))
                    {
                        TimeSpan span = (TimeSpan)(DateTime.Now - this.last_time[i]);
                        if (span.TotalSeconds > this.max_handcode_sec)
                        {
                            this.TaskPressHand(this.billCode[i], i + 1, ref errorMsg, ref comandText, this.mainFrm.zddr_dev.error_noport[this.sourcePort[i]], this.loadStatus[i], "", "0", "");
                            if (errorMsg.Length > 0)
                            {
                                this.mainFrm.AddErrToListView(errorMsg);
                                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + errorMsg);
                            }
                            else if (comandText.Length > 0)
                            {
                                comandText = "补码超时" + this.billCode[i] + ",指令" + comandText;
                                this.mainFrm.AddSuccessToListView(comandText);
                                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + comandText);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView("检查补码超时任务异常" + exception.Message);
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "检查补码超时任务异常" + exception.Message);
            }
        }

        public void itemOffTask()
        {
            while (!this.mainFrm.closing)
            {
                this.mainFrm.thrItemOffServerIsRuning = true;
                try
                {
                    if (GloabData.downtasklist.Count > 0)
                    {
                        MySqlConnection connection = new MySqlConnection(this.mainFrm.dbConnectionString);
                        connection.Open();
                        while (GloabData.downtasklist.Count > 0)
                        {
                            DownTaskConf off;
                            if (GloabData.downtasklist.TryPeek(out off))
                            {
                                try
                                {
                                    if (off.status == "1")
                                    {
                                        //发送数据给后台程序
                                       // new MySqlCommand("insert into tl_paijian_wait(BILLCODE,scan_site,sub_siteid,person_id,scan_time)\r\n                                                    values('" + off.Off_barcode + "','" + this.mainFrm.site_code + "','" + off.Off_sub_siteid + "','','" + DateTime.Now.AddSeconds(-120.0).ToString("yyyy-MM-dd HH:mm:ss") + "')", connection).ExecuteNonQuery();
                                       // new MySqlCommand("insert into tl_paijian_wait(BILLCODE,scan_site,sub_siteid,person_id,scan_time)\r\n                                                    values('" + off.Off_barcode + "','" + off.Off_sub_siteid + "','','" + off.Off_person_id + "','" + DateTime.Now.AddSeconds(-100.0).ToString("yyyy-MM-dd HH:mm:ss") + "')", connection).ExecuteNonQuery();
                                    }
                                    else
                                    {
                                       // new MySqlCommand("insert into tl_paijian_wait(BILLCODE,scan_site,sub_siteid,person_id,scan_time)\r\n                                                    values('" + off.Off_barcode + "','" + this.mainFrm.site_code + "','','" + off.Off_person_id + "','" + DateTime.Now.AddSeconds(-120.0).ToString("yyyy-MM-dd HH:mm:ss") + "')", connection).ExecuteNonQuery();
                                    }
                                    GloabData.downtasklist.TryDequeue(out off);
                                }
                                catch (Exception exception)
                                {
                                    this.mainFrm.AddError(off.barCode, "上传集包数据记录异常!" + exception.Message);
                                    LogHelper.LogSimlpleString(" 条码" + off.barCode + "上传集包数据记录异常!" + exception.Message);
                                }
                            }
                            else
                            {
                                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "从待集包获取数据失败");
                                Thread.Sleep(50);
                            }
                        }
                        if ((connection != null) && (connection.State == ConnectionState.Open))
                        {
                            connection.Close();
                        }
                    }
                }
                catch (Exception exception2)
                {
                    this.mainFrm.AddError("","保存待集包扫描数据异常!" + exception2.Message);
                    LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + " 保存待集包扫描数据异常!" + exception2.Message);
                }
                this.mainFrm.thrItemOffServerIsRuning = false;
                Thread.Sleep(0x3e8);
            }
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
                        
                        if ((this.deviceStatusOld[phClientItems[i] - 1] == 1) && (int.Parse(pvValues[i].ToString()) == 0))
                        {
                            //if ((this.loadStatus[phClientItems[i] - 1] == 1) && (this.if_paijian[phClientItems[i] - 1] == "1"))
                            //{
                            //    ItemOff off;
                            //    off.Off_barcode = this.billCode[phClientItems[i] - 1];
                            //    off.Off_outport = this.outport[phClientItems[i] - 1];
                            //    off.Off_person_id = this.person_id[phClientItems[i] - 1];
                            //    off.Off_sub_siteid = this.sub_siteid[phClientItems[i] - 1];
                            //    this.queueItemOff.Enqueue(off);
                            //}
                            if (((this.loadStatus[phClientItems[i] - 1] == 1) || (this.loadStatus[phClientItems[i] - 1] == 2)) || ((this.loadStatus[phClientItems[i] - 1] == 3) || (this.loadStatus[phClientItems[i] - 1] == 4)))
                            {
                                this.li_finished_tasks++;
                                if (this.sourcePort[phClientItems[i] - 1] >= 0)
                                {
                                    this.mainFrm.zddr_dev.num[this.sourcePort[phClientItems[i] - 1]]++;
                                }
                            }
                            if (this.loadStatus[phClientItems[i] - 1] == 2)
                            {
                                this.li_finished_tasks_nodata++;
                            }
                            if (this.loadStatus[phClientItems[i] - 1] == 3)
                            {
                                this.li_finished_tasks_noport++;
                            }
                            if (this.loadStatus[phClientItems[i] - 1] == 4)
                            {
                                this.li_finished_tasks_error++;
                            }

                            LogHelper.LogSimlpleString( $"下架条码{this.billCode[phClientItems[i] - 1]},小车{(phClientItems[i] - 1).ToString()},分拣口{this.outport[phClientItems[i] - 1].ToString()}");
                            this.loadStatus[phClientItems[i] - 1] = 0;
                            DownTaskConf cf;
                            cf.aimport = this.outport[phClientItems[i] - 1].ToString();
                            cf.barCode = this.billCode[phClientItems[i] - 1];
                            cf.carId = phClientItems[i].ToString();
                            cf.status = "1";
                            GloabData.downtasklist.Enqueue(cf);
                        }
                        this.deviceStatusOld[phClientItems[i] - 1] = int.Parse(pvValues[i].ToString());
                    }
                }
                TimeSpan span = (TimeSpan)(DateTime.Now - this.refresh_time);
                if (span.TotalSeconds > 3.0)
                {
                    this.mainFrm.lblFinishedTasks.Text = this.li_finished_tasks.ToString();
                    this.mainFrm.lblTasks_nodata.Text = this.li_finished_tasks_nodata.ToString();
                    this.mainFrm.lblTasks_noport.Text = this.li_finished_tasks_noport.ToString();
                    this.mainFrm.lblTasks_error.Text = this.li_finished_tasks_error.ToString();
                    //this.mainFrm.lblTasks_handcode.Text = this.mainFrm.li_handcode_tasks.ToString();
                    this.refresh_time = DateTime.Now;
                }
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"异步更新分拣小车货物状态失败:-{exception.Message}");
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + $"异步更新分拣小车货物状态失败:-{exception.Message}");
            }
        }

        public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            int i = 0;
            try
            {
                for (i = 0; i < dwCount; i++)
                {
                    if (pErrors[i] == 0)
                    {
                        this.deviceStatusOld[phClientItems[i] - 1] = int.Parse(pvValues[i].ToString());
                        LogHelper.LogSimlpleString($":i= {i}");
                    } 
                }
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"异步读更新分拣小车货物状态失败:-{exception.Message}");
                LogHelper.LogSimlpleString($"异步读更新分拣小车货物状态失败:-{exception.Message}");
            }
        }

        public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
        {
            throw new NotImplementedException();
        }

        public void TaskPressHand(string shortbarcode, int carid, ref string errorMsg, ref string comandText, int li_out_port, int li_loadStatus, string ls_person_id, string ls_if_paijian, string ls_sub_siteid)
        {
            string[] values = new string[1];
            int[] itemHandle = new int[1];
            return;
            try
            {
                errorMsg = string.Empty;
                comandText = string.Empty;
                this.billCode[carid - 1] = shortbarcode;
                this.loadStatus[carid - 1] = li_loadStatus;
                this.outport[carid - 1] = li_out_port;
                this.person_id[carid - 1] = ls_person_id;
                this.sub_siteid[carid - 1] = ls_sub_siteid;
                this.if_paijian[carid - 1] = ls_if_paijian;
                this.last_time[carid - 1] = DateTime.Now;
                this.taskid++;
                itemHandle[0] = this.commandHandle[carid - 1];
                values[0] = $"{{{"3" + this.taskid.ToString()}|{"0"}|{this.outport[carid - 1].ToString()}|{"1"}}}";
                this.opcServer.SyncWrite(values, itemHandle);
                comandText = values[0].ToString();
            }
            catch (Exception exception)
            {
                errorMsg = shortbarcode + "写入补码任务异常" + exception.Message;
            }
        }
    }
}


