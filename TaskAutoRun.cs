namespace WCS
{
    using System;
    using System.Threading;
    using WCS.DevSystem;

    internal class TaskAutoRun
    {
        private Form1 mainform;
       // public int plcRunControl;

        public TaskAutoRun(Form1 mainform)
        {
            this.mainform = mainform;
        }

        public void AutoRun()
        {
            while (!this.mainform.closing)
            {
                this.mainform.nonCmdDeviceThreadStatus = true;
                try
                {
                    if ((this.mainform.plcIsOK) && this.mainform.deviceInited && this.mainform.isStart)
                    {
                    
                        this.mainform.plcSystemMS.RefreshStatus();
                        this.mainform.car_pos.RefreshDisplay();
                     
                       
                        // this.mainform.ssj_dev.RefreshDisplay();
                        //this.mainform.car_dev.HandCodeTimeOut();
                        //if (this.mainform.plcSystemMS.is_runing && (this.mainform.plcSystemMS.carSpeedValue <= 600.0))
                        // {
                        //LogHelper.LogSimlpleString(DateTime.Now.ToString() + " 分拣机主线速度异常：" + ((this.mainform.plcSystemMS.carSpeedValue / 1000.0)).ToString() + "米/秒");
                        //  }
                    }
                }
                catch (Exception exception)
                {
                    LogHelper.LogSimlpleString(DateTime.Now.ToString() + $"更新分拣机运行状态失败:-{exception.Message}");
                }
                Thread.Sleep(2000);
                this.mainform.nonCmdDeviceThreadStatus = false;
                //this.plcRunControl = 1 - this.plcRunControl;
            }
            this.mainform.nonCmdDeviceThreadStatus = false;
        }
    }
}


