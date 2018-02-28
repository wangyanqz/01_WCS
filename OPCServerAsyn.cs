namespace WCS.PlcSystem
{
    using OpcRcw.Comn;
    using OpcRcw.Da;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class OPCServerAsyn
    {
        private bool boolValueChang;
        public int dwCookie;
        public object GroupObj;
        public IOPCAsyncIO2 IOPCAsyncIO2Obj;
        public IOPCGroupStateMgt IOPCGroupStateMgtObj;
        private bool isAddGroup;
        private bool isAddItems;
        private bool isConnected;
        private bool isDataChange;
        public const int LOCALE_ID = 0x804;
        private string objName;
        public IConnectionPoint pIConnectionPoint;
        public IConnectionPointContainer pIConnectionPointContainer;
        public int pSvrGroupHandle;
        public IOPCServer ServerObj;
        public string plcip { get; set; }
        
        public OPCServerAsyn()
        {
            this.boolValueChang = true;
        }

        public OPCServerAsyn(string name)
        {
            this.boolValueChang = true;
            this.objName = name;
        }

        public bool AddGroup(object Form_Main)
        {
            int dwRequestedUpdateRate = 0x3e8;
            int hClientGroup = 1;
            float num4 = 0f;
            int num5 = 0;
            GCHandle handle = GCHandle.Alloc(num5, GCHandleType.Pinned);
            GCHandle handle2 = GCHandle.Alloc(num4, GCHandleType.Pinned);
            Guid gUID = typeof(IOPCItemMgt).GUID;
            if (!this.isConnected && !this.Connect())
            {
                return false;
            }
            try
            {
                int num3;
                this.ServerObj.AddGroup("OPCGroup", 0, dwRequestedUpdateRate, hClientGroup, handle.AddrOfPinnedObject(), handle2.AddrOfPinnedObject(), 0x804, out this.pSvrGroupHandle, out num3, ref gUID, out this.GroupObj);
                this.IOPCAsyncIO2Obj = (IOPCAsyncIO2)this.GroupObj;
                this.IOPCGroupStateMgtObj = (IOPCGroupStateMgt)this.GroupObj;
                this.pIConnectionPointContainer = (IConnectionPointContainer)this.GroupObj;
                Guid riid = typeof(IOPCDataCallback).GUID;
                this.pIConnectionPointContainer.FindConnectionPoint(ref riid, out this.pIConnectionPoint);
                this.pIConnectionPoint.Advise(Form_Main, out this.dwCookie);
                this.isAddGroup = true;
            }
            catch (Exception exception)
            {
                this.isAddGroup = false;
                MessageBox.Show($"创建组对象时出错:-{exception.Message}", "建组出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                if (handle2.IsAllocated)
                {
                    handle2.Free();
                }
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
            return this.isAddGroup;
        }

        public bool AddGroup(object Form_Main, int dwRequestedUpdateRate)
        {
            int hClientGroup = 1;
            float num3 = 0f;
            int num4 = 0;
            GCHandle handle = GCHandle.Alloc(num4, GCHandleType.Pinned);
            GCHandle handle2 = GCHandle.Alloc(num3, GCHandleType.Pinned);
            Guid gUID = typeof(IOPCItemMgt).GUID;
            if (!this.isConnected && !this.Connect())
            {
                return false;
            }
            try
            {
                int num2;
                this.ServerObj.AddGroup("OPCGroup", 0, dwRequestedUpdateRate, hClientGroup, handle.AddrOfPinnedObject(), handle2.AddrOfPinnedObject(), 0x804, out this.pSvrGroupHandle, out num2, ref gUID, out this.GroupObj);
                this.IOPCAsyncIO2Obj = (IOPCAsyncIO2)this.GroupObj;
                this.IOPCGroupStateMgtObj = (IOPCGroupStateMgt)this.GroupObj;
                this.pIConnectionPointContainer = (IConnectionPointContainer)this.GroupObj;
                Guid riid = typeof(IOPCDataCallback).GUID;
                this.pIConnectionPointContainer.FindConnectionPoint(ref riid, out this.pIConnectionPoint);
                this.pIConnectionPoint.Advise(Form_Main, out this.dwCookie);
                this.isAddGroup = true;
            }
            catch (Exception exception)
            {
                this.isAddGroup = false;
                MessageBox.Show($"创建组对象时出错:-{exception.Message}", "建组出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                if (handle2.IsAllocated)
                {
                    handle2.Free();
                }
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
            return this.isAddGroup;
        }

        public bool AddItems(OPCITEMDEF[] items, int[] itemHandle)
        {
            IntPtr zero = IntPtr.Zero;
            IntPtr ppErrors = IntPtr.Zero;
            try
            {
                ((IOPCItemMgt)this.GroupObj).AddItems(items.Length, items, out zero, out ppErrors);
                int[] destination = new int[items.Length];
                Marshal.Copy(ppErrors, destination, 0, items.Length);
                IntPtr ptr = zero;
                for (int i = 0; i < items.Length; i++)
                {
                    if (destination[i] == 0)
                    {
                        OPCITEMRESULT opcitemresult = (OPCITEMRESULT)Marshal.PtrToStructure(ptr, typeof(OPCITEMRESULT));
                        itemHandle[i] = opcitemresult.hServer;
                        ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                        this.isAddItems = true;
                    }
                    else
                    {
                        this.isAddItems = false;
                        MessageBox.Show($"添加第{i + 1}个Item对象时出错", "添加Item对象出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        goto Label_012F;
                    }
                }
            }
            catch (Exception exception)
            {
                this.isAddItems = false;
                MessageBox.Show($"添加Item对象时出错:-{exception.Message}", "添加Item对象出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(zero);
                    zero = IntPtr.Zero;
                }
                if (ppErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(ppErrors);
                    ppErrors = IntPtr.Zero;
                }
            }
        Label_012F:
            return this.isAddItems;
        }

        public bool Connect()
        {
            try
            {
                // System.Type typeFromProgID = System.Type.GetTypeFromProgID("ICONICS.SimulatorOPCDA", "127.0.0.1");
                System.Type typeFromProgID = System.Type.GetTypeFromProgID("OPC.SimaticNET", plcip);
                this.ServerObj = (IOPCServer)Activator.CreateInstance(typeFromProgID);
                this.isConnected = true;
            }
            catch (Exception exception)
            {
                this.isConnected = false;
                MessageBox.Show($"对象{this.objName}建立OPCServer连接失败:-{exception.Message}", "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return this.isConnected;
        }

        public bool DataChange()
        {
            IntPtr zero = IntPtr.Zero;
            int pRevisedUpdateRate = 0;
            IntPtr phClientGroup = IntPtr.Zero;
            IntPtr pTimeBias = IntPtr.Zero;
            IntPtr pPercentDeadband = IntPtr.Zero;
            IntPtr pLCID = IntPtr.Zero;
            int num2 = 0;
            GCHandle handle = GCHandle.Alloc(num2, GCHandleType.Pinned);
            if (this.boolValueChang)
            {
                handle.Target = 1;
            }
            else
            {
                handle.Target = 0;
            }
            try
            {
                this.IOPCGroupStateMgtObj.SetState(zero, out pRevisedUpdateRate, handle.AddrOfPinnedObject(), pTimeBias, pPercentDeadband, pLCID, phClientGroup);
                this.isDataChange = true;
            }
            catch (Exception exception)
            {
                this.isDataChange = false;
                MessageBox.Show($"添加plc数据改变触发事件时出错:-{exception.Message}", "添加plc触发事件出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                handle.Free();
            }
            return this.isDataChange;
        }

        public string DisConnection()
        {
            string str = string.Empty;
            try
            {
                if (this.dwCookie != 0)
                {
                    this.pIConnectionPoint.Unadvise(this.dwCookie);
                    this.dwCookie = 0;
                }
                Marshal.ReleaseComObject(this.pIConnectionPoint);
                this.pIConnectionPoint = null;
                Marshal.ReleaseComObject(this.pIConnectionPointContainer);
                this.pIConnectionPointContainer = null;
                if (this.IOPCAsyncIO2Obj != null)
                {
                    Marshal.ReleaseComObject(this.IOPCAsyncIO2Obj);
                    this.IOPCAsyncIO2Obj = null;
                }
                this.ServerObj.RemoveGroup(this.pSvrGroupHandle, 0);
                if (this.IOPCGroupStateMgtObj != null)
                {
                    Marshal.ReleaseComObject(this.IOPCGroupStateMgtObj);
                    this.IOPCGroupStateMgtObj = null;
                }
                if (this.GroupObj != null)
                {
                    Marshal.ReleaseComObject(this.GroupObj);
                    this.GroupObj = null;
                }
                if (this.ServerObj != null)
                {
                    Marshal.ReleaseComObject(this.ServerObj);
                    this.ServerObj = null;
                }
            }
            catch (Exception exception)
            {
                str = exception.Message.ToString().Trim();
            }
            return str;
        }

        public string ReadPlc(int[] itemHandle)
        {
            string str = string.Empty;
            IntPtr zero = IntPtr.Zero;
            if (this.IOPCAsyncIO2Obj != null)
            {
                try
                {
                    int num;
                    this.IOPCAsyncIO2Obj.Read(itemHandle.Length, itemHandle, 1, out num, out zero);
                    int[] destination = new int[itemHandle.Length];
                    Marshal.Copy(zero, destination, 0, itemHandle.Length);
                    return str;
                }
                catch (Exception exception)
                {
                    str = exception.Message.ToString().Trim();
                }
            }
            return str;
        }

        public string WirtePlc(object[] values, int[] itemHandle)
        {
            string str = string.Empty;
            IntPtr zero = IntPtr.Zero;
            if (this.IOPCAsyncIO2Obj != null)
            {
                try
                {
                    int num;
                    this.IOPCAsyncIO2Obj.Write(values.Length, itemHandle, values, values.Length, out num, out zero);
                    int[] destination = new int[values.Length];
                    Marshal.Copy(zero, destination, 0, values.Length);
                    if (destination[0] != 0)
                    {
                        Marshal.FreeCoTaskMem(zero);
                        zero = IntPtr.Zero;
                        str = "异步写入数据发生异常！";
                    }
                }
                catch (Exception exception)
                {
                    str = exception.Message.ToString().Trim();
                }
            }
            return str;
        }
    }
}

