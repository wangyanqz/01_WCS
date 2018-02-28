
namespace WCS.TCP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using WCS.DevSystem;
    using Newtonsoft.Json;

    //接收到数据的回调处理函数
    //public delegate void DataHandler(byte[] data, int length, object state);
    public class CommTcpClient
        {
            private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CommTcpClient));

            private TcpClient tcpClient;

            public String ServerIP { get; set; }

            public int Port { get; set; }
            //线程休息时间
            public int ThreadSleepInterval { get; set; }

            public Boolean IsConnected { get; set; }

            //public DataHandler RecvDataHandler { get; set; } 
            //public event ReceivedHandler OnDataReceived;
            //public event SocketErrorHandler OnSocketError;


            Thread recvThread;
        private Form1 frm;

        public CommTcpClient()
            {
                ThreadSleepInterval = 100;
            }

            public void Init(String serverIp, int port)
            {
                this.ServerIP = serverIp;
            this.Port = port;
            }

            public Boolean Connect()
            {
                try
                {
                    if (recvThread != null)
                        recvThread.Abort();
                }
                catch (Exception ex)
                {
                   logger.Error(ex.Message);
                }

                tcpClient = new TcpClient();
                try
                {

                    tcpClient.Connect(ServerIP, Port);
                    tcpClient.Client.Blocking = true;
                    //tcpClient.Client.ReceiveTimeout = 1000;
                    tcpClient.Client.LingerState = new LingerOption(true, 0);
                    recvThread = new Thread(new ThreadStart(RecvRequestFromClient));
                    recvThread.Start();
                    IsConnected = true;
                    return true;
                }
                catch (SocketException ex)
                {
                    logger.Error(ex.Message);

                }
                return false;

            }

        public bool Send(byte[] data)
        {
            if (tcpClient == null || tcpClient.Connected == false)
                Connect();
            bool bret = false;
            int ret = tcpClient.Client.Send(data);
            if (ret > 0)
                bret = true;
            return bret;
        }

        public void Reconnect()
            {

                this.Close();
                this.Connect();

            }
            public void Close()
            {
                IsConnected = false;
                try
                {
                    if (tcpClient != null)
                    {
                        tcpClient.Client.Close();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                //LogHelper.LogSimlpleString(DateTime.Now.ToString() + "服务器连接异常！" + exception.Message.ToString());
            }
            }
        public bool ParseData(byte[] byteArrayIn)
        {
            if (byteArrayIn == null)
                return false;
            return true;

            

        }
        public void RecvRequestFromClient()
        {
            int availableBytes = 0;
            SocketError errCode;
            int bytesRead = 0;
            int recvLen = 0;
            bool bHas = false;
            string str;
            TaskInfo info;
            List<TaskConf> listtmp = new List<TaskConf>();
            byte[] data = CommTcpClient.organizeData("2", null);
            while (IsConnected)
            {
                if (tcpClient == null || tcpClient.Client == null || tcpClient.Connected == false)
                {
                    return;
                }
                this.Send(data);
                while (!bHas)
                {
                    try
                    {
                        availableBytes = tcpClient.Available;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        return;
                    }

                    if (availableBytes > 0)
                    {
                        bHas = true;
                        byte[] byteArrayIn = new byte[availableBytes];
                        try
                        {

                            bytesRead = 0;
                            recvLen = availableBytes;
                            bytesRead = tcpClient.Client.Receive(byteArrayIn, 0, recvLen, SocketFlags.None, out errCode);
                            str = System.Text.Encoding.Default.GetString(byteArrayIn);
                            info = (TaskInfo)JsonConvert.DeserializeObject(str, typeof(TaskInfo));
                            if(info.result.Length > 0)
                            {
                                listtmp = info.result.ToList<TaskConf>();
                                GloabData.tasklist.AddRange(listtmp);
                            }
                            
                            listtmp.Clear();
                            // LogHelper.LogSimlpleString(str);
                        }
                        catch (SocketException se)
                        {
                            logger.Error(se.Message);
                            return;
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.Message);
                            logger.Error(ex.StackTrace);

                        }

                    }
                    else
                        Thread.Sleep(5);
                }
                Thread.Sleep(ThreadSleepInterval);
            }
        }
        // bs 最终传递的byte[]
        //byte[] bs = data.getBytes("UTF-8");
        // 数据长度
        //      int len = bs.length;
        //      byte[] lenBytes = intToBytes(len);
        //      bs[2]=lenBytes[0];
        //bs[3]=lenBytes[1];
        //bs[4]=lenBytes[2];
        //bs[5]=lenBytes[3];

        //bs[0]=getBuffSumCheck(getBuffSumCheck((byte)0, lenBytes, 0, 4),	bs, 6, len);

        public void SendDataFromClient()
        {
            bool bSend = false;

            List<TaskConf> listtmp = new List<TaskConf>();
            
            while (IsConnected)
            {
                if (tcpClient == null || tcpClient.Client == null || tcpClient.Connected == false)
                {
                    break;
                }
                try
                {
                    bSend = this.Send("123");
                }
                catch( Exception e)
                {
                        logger.Error(e.Message);
                }
                
             }
                Thread.Sleep(ThreadSleepInterval);
            }


        /**aisc码相加校验*/
        public static Byte getBuffSumCheck(Byte init, Byte[] buf, int offset, int len)
        {
            Byte ini = init;
            for (int i = offset; i < len; i++)
            {
                ini += buf[i];
            }
            return ini;
        }


        /* @param value  
               *            要转换的int值  从低位到高位
               * @return byte数组 
               */
        public static Byte[] intToBytes(Int32 value)
        {
            Byte[] src = new Byte[4];
            src[3] = (Byte)((value >> 24) & 0xFF);
            src[2] = (Byte)((value >> 16) & 0xFF);
            src[1] = (Byte)((value >> 8) & 0xFF);
            src[0] = (Byte)(value & 0xFF);
            return src;
        }
        public static Byte[] organizeData(string order,string strid,Byte[]data)
        {
            Int32 nDataLen = 0;
            if (data != null)
            {
                nDataLen = 12 + data.Length; 
            }
      
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(order);
            byte[] byteArray1 = System.Text.Encoding.Default.GetBytes(strid);
            Byte[] senddata = new Byte[nDataLen];
            for (int i = 0; i < senddata.Length; i++)
            {
                senddata[i] = (byte)0;
            }
            if (data != null)
            {
                for (int i = 7; i < data.Length; i++)
                {
                    senddata[i] = data[i - 7];
                }
            }
            Byte[] lenBytes = intToBytes(nDataLen);
            senddata[1] = 0;
            senddata[2] =lenBytes[0];
            senddata[3] =lenBytes[1];
            senddata[4] =lenBytes[2];
            senddata[5] =lenBytes[3];
            senddata[6] = byteArray[0];
            for (int i = 0; i < 4; i++)
            {
                senddata[0] += (byte)(lenBytes[i]);
            }
            String end = "#";
            byteArray = System.Text.Encoding.Default.GetBytes(end);
            int nlen = byteArray.Length;
            nlen ++;
            senddata[7] = byteArray[0];
            senddata[0] = getBuffSumCheck(senddata[0], senddata, 6, nDataLen);
            return senddata;
        }




    }







        //Thread recvThread;
        //private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SocketClient));
        //private TcpClient tcpClient;

        //public String ServerIP { get; set; }

        //public int Port { get; set; }

        ///// <summary>  
        ///// Socket套接字  
        ///// </summary>  
        //private  Socket _Socket = null;
        ///// <summary>  
        ///// 远程服务器Ip  
        ///// </summary>  
        //public  System.Net.IPAddress RemoteIpAddr = IPAddress.Parse("127.0.0.1");

        ///// <summary>  
        ///// 是否联接  
        ///// </summary>  
        //public bool Connected
        //{
        //    get
        //    {
        //        if (_Socket != null)
        //            return _Socket.Connected;
        //        else
        //            return false;
        //    }
        //}

        ///// <summary>  
        ///// 初始化Socket，建立连接  
        ///// </summary>  
        ///// <returns></returns>  
        //public  bool InitSocket(String ip, int port)
        //{
        //    try
        //    {
        //        _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        RemoteIpAddr = IPAddress.Parse(ip);
        //        IPEndPoint iep = new IPEndPoint(RemoteIpAddr, port);
        //        _Socket.Connect(iep);
        //    }
        //    catch
        //    {
        //        //如果出错，则断开连接  
        //        DisConnect();
        //        return false;
        //    }
        //    return _Socket.Connected; ;
        //}

        ///// <summary>  
        ///// 发送数据  
        ///// </summary>  
        ///// <param name="SendData"></param>  
        //public  void Send(byte[] SendData)
        //{
        //    try
        //    {
        //        if (_Socket.Connected)
        //        {
        //            _Socket.Send(SendData, SendData.Length, 0);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //    }
        //}

        ///// <summary>  
        ///// 断开连接  
        ///// </summary>  
        //public  bool DisConnect()
        //{
        //    try
        //    {
        //        if (_Socket != null)
        //        {
        //            //如果已建立与服务器的连接，则断开联接  
        //            if (_Socket.Connected)
        //            {
        //                _Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
        //            }
        //            _Socket.Close();
        //            _Socket = null;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        return false;
        //    }
        //}
    }

