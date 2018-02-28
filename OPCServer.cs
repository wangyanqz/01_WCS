namespace WCS.PlcSystem
{
    using OpcRcw.Da;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class OPCServer
    {
        private object GroupObj;
        private IOPCSyncIO IOPCSyncObj;
        private bool isAddGroup;
        private bool isAddItems;
        private bool isConnected;
        private const int LOCALE_ID = 0x804;
        private string objName;
        private int pSvrGroupHandle;   // 服务器组的句柄
        private IOPCServer ServerObj;
        public string plcip;
        

        public OPCServer()
        {
        }

        public OPCServer(string name)
        {
            this.objName = name;
        }

        public bool AddGroup()
        {
            int dwRequestedUpdateRate = 1; //单位：1000ms
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
                this.IOPCSyncObj = (IOPCSyncIO)this.GroupObj;
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

        public bool AddGroup(int dwRequestedUpdateRate)
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
                this.IOPCSyncObj = (IOPCSyncIO)this.GroupObj;
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
            if (!this.isAddGroup && !this.AddGroup())
            {
                return false;
            }
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
                        goto Label_0141;
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
        Label_0141:
            return this.isAddItems;
        }

        public bool Connect()
        {
            try
            {
              //System.Type typeFromProgID = System.Type.GetTypeFromProgID("ICONICS.SimulatorOPCDA", "127.0.0.1");
                System.Type typeFromProgID = System.Type.GetTypeFromProgID("OPC.SimaticNET", "127.0.0.1");
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

        public void DisConnect()
        {
            try
            {
                this.isConnected = false;
                this.isAddGroup = false;
                this.isAddItems = false;
                if (this.IOPCSyncObj != null)
                {
                    Marshal.ReleaseComObject(this.IOPCSyncObj);
                    this.IOPCSyncObj = null;
                }
                this.ServerObj.RemoveGroup(this.pSvrGroupHandle, 0);
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
                MessageBox.Show(exception.Message, "断开OPCServer连接出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private string GetQuality(long wQuality)
        {
            switch (wQuality)
            {
                case 8L:
                    return "BadNotConnected";

                case 12L:
                    return "BadDeviceFailure";

                case 0x10L:
                    return "BadSensorFailure";

                case 0L:
                    return "Bad";

                case 4L:
                    return "BadConfigurationError";

                case 0x18L:
                    return "BadCommFailure";

                case 0x1cL:
                    return "BadOutOfService";

                case 0x20L:
                    return "BadWaitingForInitialData";

                case 0x54L:
                    return "UncertainEGUExceeded";

                case 0x58L:
                    return "UncertainSubNormal";

                case 0xc0L:
                    return "Good";
            }
            return "Not handled";
        }

        public bool SyncRead(object[] values, int[] itemHandle)
        {
            IntPtr zero = IntPtr.Zero;
            IntPtr ppErrors = IntPtr.Zero;
            bool flag = false;
            try
            {
                if (values.Length != itemHandle.Length)
                {
                    MessageBox.Show(string.Format("需要读出数据的个数与添加Item的数据说明长度不一致", new object[0]), "读数据出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                this.IOPCSyncObj.Read(OPCDATASOURCE.OPC_DS_DEVICE, itemHandle.Length, itemHandle, out zero, out ppErrors);
                int[] destination = new int[itemHandle.Length];
                Marshal.Copy(ppErrors, destination, 0, itemHandle.Length);
                OPCITEMSTATE opcitemstate = new OPCITEMSTATE();
                for (int i = 0; i < itemHandle.Length; i++)
                {
                    if (destination[i] == 0)
                    {
                        opcitemstate = (OPCITEMSTATE)Marshal.PtrToStructure(zero, typeof(OPCITEMSTATE));
                        values[i] = opcitemstate.vDataValue.ToString();
                        zero = new IntPtr(zero.ToInt32() + Marshal.SizeOf(typeof(OPCITEMSTATE)));
                        flag = true;
                    }
                    else
                    {
                        string str;
                        this.ServerObj.GetErrorString(destination[i], 0x804, out str);
                        MessageBox.Show($"读出第{i}个数据时出错:{str}", "读数据出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return false;
                    }
                }
                return flag;
            }
            catch (Exception exception)
            {
                flag = false;
                MessageBox.Show(exception.Message, "Result-Read Items", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            //finally
            //{
            //    if (zero != IntPtr.Zero)
            //    {
            //        Marshal.FreeCoTaskMem(zero);
            //        zero = IntPtr.Zero;
            //    }
            //    if (ppErrors != IntPtr.Zero)
            //    {
            //        Marshal.FreeCoTaskMem(ppErrors);
            //        ppErrors = IntPtr.Zero;
            //    }
            //}
            return flag;
        }

        public bool SyncWrite(object[] values, int[] itemHandle)
        {
            IntPtr zero = IntPtr.Zero;
            bool flag = false;
            try
            {
                if (values.Length != itemHandle.Length)
                {
                    MessageBox.Show(string.Format("写入数据的个数与添加Item的数据说明长度不一致", new object[0]), "写数据出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                this.IOPCSyncObj.Write(values.Length, itemHandle, values, out zero);
                int[] destination = new int[values.Length];
                Marshal.Copy(zero, destination, 0, values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    if (destination[i] != 0)
                    {
                        string str;
                        this.ServerObj.GetErrorString(destination[i], 0x804, out str);
                        MessageBox.Show($"写入第{i}个数据时出错:{str}", "写数据出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return false;
                    }
                    flag = true;
                }
                return flag;
            }
            catch (Exception exception)
            {
                flag = false;
                MessageBox.Show(exception.Message, "写数据出错", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(zero);
                    zero = IntPtr.Zero;
                }
            }
            return flag;
        }

        private DateTime ToDateTime(OpcRcw.Da.FILETIME ft)
        {
            long dwHighDateTime = ft.dwHighDateTime;
            long fileTime = ((dwHighDateTime << 0x20) + ft.dwLowDateTime) + 0x430e234000L;
            return DateTime.FromFileTimeUtc(fileTime);
        }
    }
}


