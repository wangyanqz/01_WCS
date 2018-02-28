namespace WCS.DevSystem
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Data;
    using System.Threading;
    using System.Windows.Forms;
    using WCS;

    public class ZDDR_Dev : DeviceBase
    {
        public MySqlConnection[] dbConn;
        public int[] error_nodata;
        public int[] error_noport;
        public int error_port;
        private double ldb_time = 60.0;
        public Form1 mainFrm;
        public int[] num;
        public int[] RecvTrace;
        public int[] returnCarId;
        public int[] returnTaskId;
        public int[] returnTaskStatus;
        public DateTime scan_time = DateTime.Now;
        public int[] taskid;
        public ZDDR_Dev(Form1 mainfrom)
        {
            this.mainFrm = mainfrom;
            base.deviceId = new string[20];
            base.commandDB = new string[20];
            base.returnDB = new string[20];
            base.commandHandle = new int[20];
            base.returnHandle = new int[20];
            this.returnTaskId = new int[20];
            this.returnCarId = new int[20];
            this.returnTaskStatus = new int[20];
            this.taskid = new int[20];
            this.error_nodata = new int[20];
            this.error_noport = new int[20];
            this.dbConn = new MySqlConnection[20];
            this.RecvTrace = new int[20];
            this.num = new int[20];
            int index = 0;
            //foreach (DataRow row in bt.Rows)
            //{
            //    base.deviceId[index] = row["device_id"].ToString();
            //    base.commandDB[index] = row["command_db"].ToString();
            //    base.returnDB[index] = row["return_db"].ToString();
            //    this.RecvTrace[index] = 1;
            //    this.num[index] = 0;
            //    try
            //    {
            //        this.dbConn[index] = new MySqlConnection(this.mainFrm.dbConnectionString);
            //        this.dbConn[index].Open();
            //    }
            //    catch (Exception exception)
            //    {
            //        //this.mainFrm.AddErrToListView("供包机专用数据库连接异常！" + exception.Message);
            //        //LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "供包机专用数据库连接异常！" + exception.Message);
            //        return;
            //    }
            //    index++;
            //}
            this.dbConn[0] = new MySqlConnection(this.mainFrm.dbConnectionString);
            try {
                this.dbConn[0].Open();
            }
            catch (Exception e)
            {
                return;
            }
            Control.CheckForIllegalCrossThreadCalls = false;
            this.Init_error_port();
            //this.InitTaskid();
        }
        public ZDDR_Dev(Form1 mainfrom, DataTable bt)
        {
            this.mainFrm = mainfrom;
            base.deviceId = new string[bt.Rows.Count];
            base.commandDB = new string[bt.Rows.Count];
            base.returnDB = new string[bt.Rows.Count];
            base.commandHandle = new int[bt.Rows.Count];
            base.returnHandle = new int[bt.Rows.Count];
            this.returnTaskId = new int[bt.Rows.Count];
            this.returnCarId = new int[bt.Rows.Count];
            this.returnTaskStatus = new int[bt.Rows.Count];
            this.taskid = new int[bt.Rows.Count];
            this.error_nodata = new int[bt.Rows.Count];
            this.error_noport = new int[bt.Rows.Count];
            this.dbConn = new MySqlConnection[bt.Rows.Count];
            this.RecvTrace = new int[bt.Rows.Count];
            this.num = new int[bt.Rows.Count];
            int index = 0;
            foreach (DataRow row in bt.Rows)
            {
                base.deviceId[index] = row["device_id"].ToString();
                base.commandDB[index] = row["command_db"].ToString();
                base.returnDB[index] = row["return_db"].ToString();
                this.RecvTrace[index] = 1;
                this.num[index] = 0;
                try
                {
                    this.dbConn[index] = new MySqlConnection(this.mainFrm.dbConnectionString);
                    this.dbConn[index].Open();
                }
                catch (Exception exception)
                {
                    //this.mainFrm.AddErrToListView("供包机专用数据库连接异常！" + exception.Message);
                    //LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "供包机专用数据库连接异常！" + exception.Message);
                    return;
                }
                index++;
            }
            Control.CheckForIllegalCrossThreadCalls = false;
            this.Init_error_port();
            this.InitTaskid();
        }

        public void downloadTask(TaskConf tf)
        {
            string content = string.Empty;
            int li_loadStatus = 0;
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str4 = string.Empty;
            string[] strArray = new string[] { "#上包台:",tf.deviceIdQh,",条码:", tf.barCode, ",重量:", tf.weight,"，小车号：",tf.carId};
            content = string.Concat(strArray);
            //this.mainFrm.AddSuccessToListView(content);
            LogHelper.LogSimlpleString(content);
            int aimport = this.find_port(tf.carId, tf.oneCode, tf.barCode, tf.weight, ref li_loadStatus, ref str2, ref str3, ref str4);
            if ((this.mainFrm.if_handcode == "1") && ((li_loadStatus == 2) || (li_loadStatus == 3)))
            {
                aimport = 0;
            }
            this.SmflPress(tf.carId, 0, tf.barCode, aimport, li_loadStatus, str2, str3, str4);
            
        }

        public int find_port(string carid, string aimid, string ls_billcode, string ld_Weight, ref int li_loadStatus, ref string ls_person_id, ref string ls_if_paijian, ref string ls_sub_siteid)
        {
            int smflindex = 0;
            string str = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;
            int num = 0;
            ls_person_id = string.Empty;
            ls_if_paijian = string.Empty;
            ls_sub_siteid = string.Empty;
            if (aimid == null)
            {
                //查询数据库三段码
                try
                {
                    DataSet set = MySqlHelper.ExecuteDataset(this.dbConn[smflindex], "SELECT site_code1,site_code2,site_code3 FROM tb_bill where bill_code='" + ls_billcode + "'");
                    if (set.Tables[0].Rows.Count == 0)
                    {
                        LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "运单号" + ls_billcode + "无三段码数据");
                        li_loadStatus = 2;
                        return this.error_nodata[smflindex];
                    }
                    str = set.Tables[0].Rows[0]["site_code1"].ToString();
                    str2 = set.Tables[0].Rows[0]["site_code2"].ToString();
                    str3 = set.Tables[0].Rows[0]["site_code3"].ToString();
                    // if ((str == this.mainFrm.site_code1) && (str2 == this.mainFrm.site_code2))
                    {
                        if (smflindex <= 6)
                        {
                            string commandText = "SELECT c.port_id,c.person_id,c.if_paijian,c.sub_siteid FROM td_port c,td_port_inf inf where c.port_id = inf.port_id\r\n                        and c.pipeline='" + this.mainFrm.deviceID + "' and c.siteid='" + aimid + "' order by inf.port_status,inf.port_part,inf.num,CAST(c.port_id AS SIGNED)";
                            set = MySqlHelper.ExecuteDataset(this.dbConn[smflindex], commandText);
                        }
                        else
                        {
                            string str5 = "SELECT c.port_id,c.person_id,c.if_paijian,c.sub_siteid FROM td_port c,td_port_inf inf where c.port_id = inf.port_id\r\n                        and c.pipeline='" + this.mainFrm.deviceID + "' and c.siteid='" + aimid + "' order by inf.port_status,inf.port_part desc,inf.num,CAST(c.port_id AS SIGNED)";
                            set = MySqlHelper.ExecuteDataset(this.dbConn[smflindex], str5);
                        }
                        if (set.Tables[0].Rows.Count == 0)
                        {
                            LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "运单号" + ls_billcode + "未配置滑槽落包通道");
                            li_loadStatus = 3;
                            return this.error_noport[smflindex];
                        }
                        li_loadStatus = 1;
                        num = int.Parse(set.Tables[0].Rows[0]["port_id"].ToString());
                        ls_person_id = set.Tables[0].Rows[0]["person_id"].ToString();
                        ls_if_paijian = set.Tables[0].Rows[0]["if_paijian"].ToString();
                        ls_sub_siteid = set.Tables[0].Rows[0]["sub_siteid"].ToString();
                        return num;
                    }
                    //this.mainFrm.AddError(ls_billcode, "错发件");
                    //LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "运单号" + ls_billcode + "错发件");
                    //li_loadStatus = 4;
                    // num = this.error_port;
                }
                catch (Exception exception)
                {
                    li_loadStatus = 2;
                    string[] strArray3 = new string[] { (smflindex + 1).ToString(), "#条码", ls_billcode, "查找分拣口异常", exception.Message };
                    //this.mainFrm.AddErrToListView(string.Concat(strArray3));
                    string[] strArray4 = new string[6];
                    strArray4[0] = DateTime.Now.ToString("G");
                    strArray4[1] = (smflindex + 1).ToString();
                    strArray4[2] = "#条码";
                    strArray4[3] = ls_billcode;
                    strArray4[4] = "查找分拣口异常";
                    strArray4[5] = exception.Message;
                    //LogHelper.LogSimlpleString(string.Concat(strArray4));
                    return this.error_nodata[smflindex];
                }
            }
            else//有三段码
            {
                try
                {
                    string commandText = "SELECT c.port_id,c.person_id,c.if_paijian,c.sub_siteid FROM td_port c,td_port_inf inf where c.port_id = inf.port_id\r\n                        and c.pipeline='" + this.mainFrm.deviceID + "' and c.siteid='" + aimid + "' order by inf.port_status,inf.port_part,inf.num,CAST(c.port_id AS SIGNED)";
                    DataSet set = MySqlHelper.ExecuteDataset(this.dbConn[0], commandText);
                    if (set.Tables[0].Rows.Count == 0)
                    {
                        LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "运单号" + ls_billcode + "未配置滑槽落包通道");
                        li_loadStatus = 3;
                        return this.error_noport[smflindex];
                    }
                    li_loadStatus = 1;
                    num = int.Parse(set.Tables[0].Rows[0]["port_id"].ToString());
                    ls_person_id = set.Tables[0].Rows[0]["person_id"].ToString();
                    ls_if_paijian = set.Tables[0].Rows[0]["if_paijian"].ToString();
                    ls_sub_siteid = set.Tables[0].Rows[0]["sub_siteid"].ToString();
                    return num;
                }
                catch (Exception exception)
                {
                    li_loadStatus = 2;
                    string[] strArray3 = new string[] { (smflindex + 1).ToString(), "#条码", ls_billcode, "查找分拣口异常", exception.Message };
                    //this.mainFrm.AddErrToListView(string.Concat(strArray3));
                    string[] strArray4 = new string[6];
                    strArray4[0] = DateTime.Now.ToString("G");
                    strArray4[1] = (smflindex + 1).ToString();
                    strArray4[2] = "#条码";
                    strArray4[3] = ls_billcode;
                    strArray4[4] = "查找分拣口异常";
                    strArray4[5] = exception.Message;
                    //LogHelper.LogSimlpleString(string.Concat(strArray4));
                    return this.error_nodata[smflindex];

                }

            }

        }

        public void Init_error_port()
        {
            try
            {
                for (int i = 0; i < base.commandDB.Length; i++)
                {
                    if (i <= 6)
                    {
                        this.error_nodata[i] = this.mainFrm.error_nodata1;
                        this.error_noport[i] = this.mainFrm.error_noport1;
                    }
                    else
                    {
                        this.error_nodata[i] = this.mainFrm.error_nodata2;
                        this.error_noport[i] = this.mainFrm.error_noport2;
                    }
                }
                this.error_port = this.mainFrm.error_port;
            }
            catch (Exception exception)
            {
                //this.mainFrm.AddErrToListView("初始化供包台异常口失败！" + exception.Message);
                //LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "初始化供包台异常口失败！" + exception.Message);
            }
        }

        public bool InitTaskid()
        {
            try
            {
                if (!base.isBindToPLC)
                {
                    base.BindToPLC(100);
                }
                object[] values = new object[1];
                int[] itemHandle = new int[1];
                for (int i = 0; i < base.commandHandle.Length; i++)
                {
                    itemHandle[0] = base.commandHandle[i];
                    base.opcServer.SyncRead(values, itemHandle);
                    string[] strArray = values[0].ToString().Substring(1, values[0].ToString().Length - 2).Split(new char[] { '|' });
                    this.taskid[i] = int.Parse(strArray[0]);
                    if (this.taskid[i] > 0x3e8)//1000
                    {
                        this.taskid[i] = 0;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"初始化供包台任务号失败:-{exception.Message}", "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
        }

        public bool RefreshStatus(int index)
        {
            try
            {
                if (!base.isBindToPLC)
                {
                    base.BindToPLC(100);
                }
                object[] values = new object[1];
                int[] itemHandle = new int[] { base.returnHandle[index] };
                base.opcServer.SyncRead(values, itemHandle);
                string[] strArray = values[0].ToString().Substring(1, values[0].ToString().Length - 2).Split(new char[] { '|' });
                this.returnTaskId[index] = int.Parse(strArray[0]);
                this.returnCarId[index] = int.Parse(strArray[3]);
                this.returnTaskStatus[index] = int.Parse(strArray[4]);
                return true;
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView($"更新供包台返回数据失败:-{exception.Message}");
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + $"更新供包台返回数据失败:-{exception.Message}");
                return false;
            }
        }

        public void SmflPress(string scarid,int smflindex, string ls_billcode, int li_out_port, int li_loadStatus, string ls_person_id, string ls_if_paijian, string ls_sub_siteid)
        {
            int carid = int.Parse(scarid);//小车号
            string errorMsg = string.Empty;
            try
            {
                this.SmflTaskPress(ls_billcode, li_out_port,carid, ref errorMsg);// 只需要下发小车号与目的格口号
                if (errorMsg.Length > 0)
                {
                    this.mainFrm.AddErrToListView(errorMsg);
                    LogHelper.LogSimlpleString(errorMsg);
                }
                else
                {
                    this.mainFrm.car_dev.billCode[carid - 1] = ls_billcode;
                    this.mainFrm.car_dev.loadStatus[carid - 1] = li_loadStatus;
                    this.mainFrm.car_dev.outport[carid - 1] = li_out_port;
                    //this.mainFrm.car_dev.sourcePort[carid - 1] = smflindex;
                    this.mainFrm.car_dev.person_id[carid - 1] = ls_person_id;
                    this.mainFrm.car_dev.if_paijian[carid - 1] = ls_if_paijian;
                    this.mainFrm.car_dev.sub_siteid[carid - 1] = ls_sub_siteid;
                    this.mainFrm.car_dev.last_time[carid - 1] = DateTime.Now;

                    //DownTaskConf cf;
                    //cf.aimport = li_out_port.ToString();
                    //cf.barCode = ls_billcode;
                    //cf.carId = scarid;
                    //cf.status = "1";
                    //GloabData.downtasklist.Enqueue(cf);

                }
            }
            catch (Exception exception)
            {
                string[] strArray2 = new string[] {  "#条码", ls_billcode, "，分配通道异常！", exception.Message };
                errorMsg = string.Concat(strArray2);
                this.mainFrm.AddErrToListView(errorMsg);
                LogHelper.LogSimlpleString( errorMsg);
            }
        }

        public void SmflTaskPress(string ls_billcode,int li_out_port,int carid, ref string errorMsg)//xwc 发送指令到PLC，格式是{15|209|1},小车12
        {
            string[] values = new string[1];
            int[] itemHandle = new int[1];
            carid = 0;
            errorMsg = string.Empty;
            try
            {
                string cmdId = Convert.ToString(li_out_port);
                bool bret = this.mainFrm.car_pos.ManualCommandDown(carid,cmdId);
                //itemHandle[0] = base.commandHandle[smflindex];
                //values[0] = $"{{|{li_out_port}|{"1"}}}";
                //base.opcServer.SyncWrite(values, itemHandle);
                //DateTime now = DateTime.Now;
                //if (carid > 0)
                //{
                //    now = DateTime.Now;
                //    while (this.mainFrm.car_dev.loadStatus[carid - 1] != 0)
                //    {
                //        Thread.Sleep(110);
                //        TimeSpan span2 = (TimeSpan)(DateTime.Now - now);
                //        if (span2.TotalSeconds > 2.0)
                //        {
                //            break;
                //        }
                //    }
                //}
                if (bret)
                {
                    string[] strArray2 = new string[] { "#条码", ls_billcode, ",指令", li_out_port.ToString(), ",小车", ((int)carid).ToString() };
                    this.mainFrm.AddSuccessToListView(string.Concat(strArray2));
                    LogHelper.LogSimlpleString(string.Concat(strArray2));
                    string[] strArray3 = new string[8];
                    return;
                }
            
            }
            catch (Exception exception)
            {
                string[] strArray4 = new string[] { "#条码", ls_billcode, ",写入任务时异常！", exception.Message };
                errorMsg = string.Concat(strArray4);
            }
        }
    }
}


