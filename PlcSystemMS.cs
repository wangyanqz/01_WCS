namespace WCS.PlcSystem
{
    using OpcRcw.Da;
    using System;
    using WCS;
    using WCS.DevSystem;

    public class PlcSystemMS
    {
        public int[] SensorHandle = new int[1];
        public string[] SensorDB = new string[50];
        private string carSpeedDB = "DB9,W2";
        private int[] carSpeedHandle = new int[1];
        public double carSpeedValue;
        public bool is_auto;
        public bool is_runing;
        public bool is_stop;
        private bool isBindToPLC;
        public Form1 mainFrm;
        private OPCServer opcServer = new OPCServer();
        public int plcRunStatus;
        public int plcRunStatusOld;
        private string plcStatusDB = "DB8,W0";
        private int[] plcStatusHandle = new int[1];
        private int plcStatusValue;
        private string[] readValues = new string[1];
        private string runHourDB = "DB9,W4";   //DB9,W4
        private string runSecDB = "DB9,W6";   //DB9,W4
        private int[] runHourHandle = new int[1];
        private int[] runSecHandle = new int[1];
        private int runHourValue;
        private int runsecValue;

        public PlcSystemMS(Form1 mainFrm)
        {
            this.mainFrm = mainFrm;
        }

        public bool BindToPLC()
        {
           
            this.opcServer.plcip = this.mainFrm.plc_ip;
            // 连接
            if (!this.opcServer.Connect())
            {
                return false;
            }
           
            // 添加组
            if (!this.opcServer.AddGroup())
            {
                return false;
            }
            // 添加项1
            OPCITEMDEF[] items = new OPCITEMDEF[1];
            items[0].szAccessPath = "";
            items[0].bActive = 1;
            items[0].hClient = 1;// 多个项，不一样
            items[0].dwBlobSize = 1;
            items[0].pBlob = IntPtr.Zero;
            items[0].vtRequestedDataType = 8;
            items[0].szItemID = $"S7:[S7_Connection_1]{this.plcStatusDB}";
            //items[0].szItemID = "Logical.test1";
            
            if (!this.opcServer.AddItems(items, this.plcStatusHandle))
            {
                return false;
            }
            //添加项2
            items[0].hClient = 2;
            items[0].szItemID = $"S7:[S7_Connection_1]{this.carSpeedDB}";
            //items[0].szItemID = "Logical.test2";
            if (!this.opcServer.AddItems(items, this.carSpeedHandle))
            {
                return false;
            }
            //添加项3
            items[0].hClient = 3;
            items[0].szItemID = $"S7:[S7_Connection_1]{this.runHourDB}";
            //items[0].szItemID = "Logical.test3";
            if (!this.opcServer.AddItems(items, this.runHourHandle))
            {
                return false;
            }

            //添加项3
            items[0].hClient = 4;
            items[0].szItemID = $"S7:[S7_Connection_1]{this.runSecDB}";
            //items[0].szItemID = "Logical.test3";
            if (!this.opcServer.AddItems(items, this.runSecHandle))
            {
                return false;
            }

            this.isBindToPLC = true;
            return this.isBindToPLC;
        }

        public string  getTotalRunTime()
        {
            string[] values = new string[1];
            if (!this.isBindToPLC && !this.BindToPLC())
            {
                return "-";
            }
            if (!this.opcServer.SyncRead(values, this.runHourHandle))
            {
                return "-";
            }
            this.runHourValue = int.Parse(values[0]);
            if (!this.opcServer.SyncRead(values, this.runSecHandle))
            {
                return "-";
            }
            
            this.runsecValue = int.Parse(values[0]);
            
            LogHelper.LogSimlpleString(DateTime.Now.ToString() + " 分拣机累计运行时间：" + this.runHourValue.ToString() + "小时！");
            return this.runHourValue.ToString() + "分" + this.runsecValue.ToString()+ "秒";
        }

        public bool RefreshStatus()
        {
            if (!this.isBindToPLC && !this.BindToPLC())
            {
                return false;
            }
            if (!this.opcServer.SyncRead(this.readValues, this.plcStatusHandle))
            {
                return false;
            }
            this.plcStatusValue = Convert.ToInt32(this.readValues[0]);// PLC状态，1-未初始化2-初始化完成 3-运行 4-停止 5 急停 6 故障

            if (this.plcStatusValue == 3)
            {
                this.is_runing = true;
                this.plcRunStatus = 3;
                this.is_auto = true;
            }
            else if (this.plcStatusValue == 4)
            {
                this.is_stop = true;
                this.plcRunStatus = 4;
                this.is_auto = false;
            }
            else
            {
                this.is_auto = false;
            }
            //if (this.plcRunStatusOld != this.plcRunStatus)
            //{
            //    this.plcRunStatusOld = this.plcRunStatus;
            //}
            //if ((this.plcStatusValue & 8) != 0)
            //{
            //    this.is_auto = true;
            //}
            //else
            //{
            //    this.is_auto = false;
            //}

            if (!this.opcServer.SyncRead(this.readValues, this.carSpeedHandle))
            {
                return false;
            }
            this.carSpeedValue = double.Parse(this.readValues[0]);
            return true;
        }
    }
}


