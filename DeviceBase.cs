namespace WCS.DevSystem
{
    using OpcRcw.Da;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using WCS.PlcSystem;

    public class DeviceBase
    {
        public string[] commandDB;
        public int[] commandHandle;
        public string[] commandType;
        public string[] controlDB;
        public int[] controlHandle;
        public string[] controlType;
        public int[] deviceFailure;
        public string[] deviceId;
        public string[] devicePos;
        public int[] deviceStatus;
        public string[] failureDB;
        public int[] failureHandle;
        public string[] failureType;
        public bool isBindToPLC;
        public long lastRefreshTicks;
        public OPCServer opcServer = new OPCServer();
        public Label[] pic;
        public string[] returnDB;
        public int[] returnHandle;
        public string[] showInf;
        public string[] statusDB;
        public int[] statusHandle;
        public string[] statusType;
        public int[] xCoord;
        public int[] xInc;
        public int[] yCoord;
        public int[] yInc;

        public virtual bool BindToPLC()
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
            if (this.returnDB != null)
            {
                OPCITEMDEF[] opcitemdefArray2 = new OPCITEMDEF[this.returnDB.Length];
                for (int j = 0; j < this.returnDB.Length; j++)
                {
                    opcitemdefArray2[j].szAccessPath = "";
                    opcitemdefArray2[j].bActive = 1;
                    opcitemdefArray2[j].hClient = num;
                    opcitemdefArray2[j].dwBlobSize = 1;
                    opcitemdefArray2[j].pBlob = IntPtr.Zero;
                    opcitemdefArray2[j].vtRequestedDataType = 8;
                    opcitemdefArray2[j].szItemID = $"S7:[S7_Connection_1]{this.returnDB[j]}";
                    num++;
                }
                if (!this.opcServer.AddItems(opcitemdefArray2, this.returnHandle))
                {
                    return false;
                }
            }
            if (this.controlDB != null)
            {
                OPCITEMDEF[] opcitemdefArray3 = new OPCITEMDEF[this.controlDB.Length];
                for (int k = 0; k < this.controlDB.Length; k++)
                {
                    opcitemdefArray3[k].szAccessPath = "";
                    opcitemdefArray3[k].bActive = 1;
                    opcitemdefArray3[k].hClient = num;
                    opcitemdefArray3[k].dwBlobSize = 1;
                    opcitemdefArray3[k].pBlob = IntPtr.Zero;
                    opcitemdefArray3[k].vtRequestedDataType = 8;
                    opcitemdefArray3[k].szItemID = $"S7:[S7_Connection_1]{this.controlDB[k]}";
                    num++;
                }
                if (!this.opcServer.AddItems(opcitemdefArray3, this.controlHandle))
                {
                    return false;
                }
            }
            if (this.statusDB != null)
            {
                OPCITEMDEF[] opcitemdefArray4 = new OPCITEMDEF[this.statusDB.Length];
                for (int m = 0; m < this.statusDB.Length; m++)
                {
                    opcitemdefArray4[m].szAccessPath = "";
                    opcitemdefArray4[m].bActive = 1;
                    opcitemdefArray4[m].hClient = num;
                    opcitemdefArray4[m].dwBlobSize = 1;
                    opcitemdefArray4[m].pBlob = IntPtr.Zero;
                    opcitemdefArray4[m].vtRequestedDataType = 8;
                    opcitemdefArray4[m].szItemID = $"S7:[S7_Connection_1]{this.statusDB[m]}";
                    num++;
                }
                if (!this.opcServer.AddItems(opcitemdefArray4, this.statusHandle))
                {
                    return false;
                }
            }
            if (this.failureDB != null)
            {
                OPCITEMDEF[] opcitemdefArray5 = new OPCITEMDEF[this.failureDB.Length];
                for (int n = 0; n < this.failureDB.Length; n++)
                {
                    opcitemdefArray5[n].szAccessPath = "";
                    opcitemdefArray5[n].bActive = 1;
                    opcitemdefArray5[n].hClient = num;
                    opcitemdefArray5[n].dwBlobSize = 1;
                    opcitemdefArray5[n].pBlob = IntPtr.Zero;
                    opcitemdefArray5[n].vtRequestedDataType = 8;
                    opcitemdefArray5[n].szItemID = $"S7:[S7_Connection_1]{this.failureDB[n]}";
                    num++;
                }
                if (!this.opcServer.AddItems(opcitemdefArray5, this.failureHandle))
                {
                    return false;
                }
            }
            this.isBindToPLC = true;
            return this.isBindToPLC;
        }

        public bool BindToPLC(int dwRequestedUpdateRate)
        {
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
            if (this.returnDB != null)
            {
                OPCITEMDEF[] opcitemdefArray2 = new OPCITEMDEF[this.returnDB.Length];
                for (int j = 0; j < this.returnDB.Length; j++)
                {
                    opcitemdefArray2[j].szAccessPath = "";
                    opcitemdefArray2[j].bActive = 1;
                    opcitemdefArray2[j].hClient = num;
                    opcitemdefArray2[j].dwBlobSize = 1;
                    opcitemdefArray2[j].pBlob = IntPtr.Zero;
                    opcitemdefArray2[j].vtRequestedDataType = 8;
                    opcitemdefArray2[j].szItemID = $"S7:[S7_Connection_1]{this.returnDB[j]}";
                    num++;
                }
                if (!this.opcServer.AddItems(opcitemdefArray2, this.returnHandle))
                {
                    return false;
                }
            }
            if (this.controlDB != null)
            {
                OPCITEMDEF[] opcitemdefArray3 = new OPCITEMDEF[this.controlDB.Length];
                for (int k = 0; k < this.controlDB.Length; k++)
                {
                    opcitemdefArray3[k].szAccessPath = "";
                    opcitemdefArray3[k].bActive = 1;
                    opcitemdefArray3[k].hClient = num;
                    opcitemdefArray3[k].dwBlobSize = 1;
                    opcitemdefArray3[k].pBlob = IntPtr.Zero;
                    opcitemdefArray3[k].vtRequestedDataType = 8;
                    opcitemdefArray3[k].szItemID = $"S7:[S7_Connection_1]{this.controlDB[k]}";
                    num++;
                }
                if (!this.opcServer.AddItems(opcitemdefArray3, this.controlHandle))
                {
                    return false;
                }
            }
            if (this.statusDB != null)
            {
                OPCITEMDEF[] opcitemdefArray4 = new OPCITEMDEF[this.statusDB.Length];
                for (int m = 0; m < this.statusDB.Length; m++)
                {
                    opcitemdefArray4[m].szAccessPath = "";
                    opcitemdefArray4[m].bActive = 1;
                    opcitemdefArray4[m].hClient = num;
                    opcitemdefArray4[m].dwBlobSize = 1;
                    opcitemdefArray4[m].pBlob = IntPtr.Zero;
                    opcitemdefArray4[m].vtRequestedDataType = 8;
                    opcitemdefArray4[m].szItemID = $"S7:[S7_Connection_1]{this.statusDB[m]}";
                    num++;
                }
                if (!this.opcServer.AddItems(opcitemdefArray4, this.statusHandle))
                {
                    return false;
                }
            }
            if (this.failureDB != null)
            {
                OPCITEMDEF[] opcitemdefArray5 = new OPCITEMDEF[this.failureDB.Length];
                for (int n = 0; n < this.failureDB.Length; n++)
                {
                    opcitemdefArray5[n].szAccessPath = "";
                    opcitemdefArray5[n].bActive = 1;
                    opcitemdefArray5[n].hClient = num;
                    opcitemdefArray5[n].dwBlobSize = 1;
                    opcitemdefArray5[n].pBlob = IntPtr.Zero;
                    opcitemdefArray5[n].vtRequestedDataType = 8;
                    opcitemdefArray5[n].szItemID = $"S7:[S7_Connection_1]{this.failureDB[n]}";
                    num++;
                }
                if (!this.opcServer.AddItems(opcitemdefArray5, this.failureHandle))
                {
                    return false;
                }
            }
            this.isBindToPLC = true;
            return this.isBindToPLC;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DeviceControl
        {
            public string controlType;
            public string controlId;
            public string controlDesc;
            public string condition;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DeviceFailure
        {
            public string failureDesc;
            public string failureKind;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DeviceStatus
        {
            public string statusDesc;
            public string statusKind;
            public string statusColor;
        }
    }
}


