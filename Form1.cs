namespace WCS
{
    using Newtonsoft.Json;
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using Data;
    using DevSystem;
    using PlcSystem;
    using TCP;
    using Properties;

    public partial class Form1 : Form
    {
        public CommTcpClient st_client;
        public BarcodeDef barcodeDef;
        public string batch_no = DateTime.Now.ToString("yyyyMMddHHmmss");
        private Button buttonHandModify;
        private Button buttonQuery;
        public Car_Dev car_dev;
        public int car_num = 63;
        public int port_num = 36;
        public Car_Pos car_pos;
        public bool closing;
        private IContainer components;
        public MySqlConnection dbConn;
        public MySqlConnection dbConn_auto_supply;
        public MySqlConnection dbConn_hand;
        public string dbConnectionString = "Server='192.168.141.30';UserId=wy;Password=123456;Database=wcs";
        public string dbconstr_remote = "Server='139.224.55.189';UserId=root;Password=bkls56mysql;Database=jcd_wcs";
        public string plc_ip = "127.0.0.1";
        public bool dbIsConned  =  false;
        public bool socketIsConned = false;
        public string device_id_remote = "004";
        public Dictionary<string, DeviceBase.DeviceControl> deviceControlDic = new Dictionary<string, DeviceBase.DeviceControl>();
        public string deviceID = "002";
        public bool deviceInited;
        public bool isStart = true;
        public int error_nodata1;
        public int error_nodata2;
        public int error_noport1;
        public int error_noport2;
        public int error_port;
        public string filename = (Application.StartupPath + @"\config.xml");
        //public string filename_data = (Application.StartupPath + @"\派件检索补码数据.xlsx");
        private GroupBox groupBoxQueryBill;
        private Thread handcodeCmdDeviceThread;
        public string if_handcode = "0";
        private bool if_must_close;
        private ImageList imageListView;
        public bool IsRegistered = true;
        public static Thread itemOffThread;
        public KZAN_Dev kzan_dev;
        private Label labelBillInf;
        private Label labelBillQuery;
        public Label labelrunMode_name;
        private ToolStripStatusLabel lblDBConStatus;
        public ToolStripStatusLabel lblFinishedTasks;
        private ToolStripStatusLabel lblPLCStatus;
        public ToolStripStatusLabel lblSannerStatus;
        public ToolStripStatusLabel lblTasks_error;
        public ToolStripStatusLabel lblTasks_handcode;
        public ToolStripStatusLabel lblTasks_nodata;
        public ToolStripStatusLabel lblTasks_noport;
        private ToolStripStatusLabel lblServerStatus;
        public int li_center_control_temp;
        public int li_handcode_tasks;
        public int li_num_time;
        private int li_Register_time;
        public ListView listViewCommand;
        private ListView listViewTask;
        private string log_Path = (Application.StartupPath + @"\runninglog");
        public long max_time;
        public double maxTurnNumer = 1.5;
        private Thread noCmdDeviceThread;
        public bool nonCmdDeviceThreadStatus;
        public bool plcIsOK;
        public PlcSystemMS plcSystemMS;
        //public List<TaskConf> tasklist = new List<TaskConf>();//   下载任务缓存队列
        public List<sortcode_outport> sortcode_outport_list = new List<sortcode_outport>();
        public SSJ_Dev ssj_dev;
        
        private TaskAutoRun taskautorun;
        public string runMode_name = "交叉带分拣系统";
        public string site_code = "321300";
        public string site_code1 = "378";
        public string site_code2 = "E006";

        private StatusStrip statusStrip1;
        public static Thread SupplyGoodsThread;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox textBoxBillCode;
        private TextBox textBoxResult;
        public bool thrDownloadTaskHandIsRuning;
        public bool thrItemOffServerIsRuning;
        public bool thrSupplyGoodsServerIsRuning;
        private System.Windows.Forms.Timer timer1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel toolStripStatusLabel4;
        private ToolStripStatusLabel toolStripStatusLabel6;
        public long used_time;
        public List<PortConf> wcsPortConfList;
        public int workModel = 0;
        public ZDDR_Dev zddr_dev;
        public int zddr_num = 14;
        public static Thread ZddrThread1;
        public static Thread ZddrThread10;
        public static Thread ZddrThread11;
        public static Thread ZddrThread12;
        public static Thread ZddrThread13;
        public static Thread ZddrThread14;
        public static Thread ZddrThread2;
        public static Thread ZddrThread3;
        public static Thread ZddrThread4;
        public static Thread ZddrThread5;
        public static Thread ZddrThread6;
        public static Thread ZddrThread7;
        public static Thread ZddrThread8;
        public static Thread ZddrThread9;
        public string zt_url = string.Empty;
        public int numAdd = 0;

        public Form1()
        {
            this.InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public void AddError(string ls_barcode, string ls_error)
        {
            try
            {
                if (this.listViewInfo.Items.Count >= 0x3e8)
                {
                    this.listViewInfo.Items.Clear();
                }
                ListViewItem item = new ListViewItem(new string[] { DateTime.Now.ToString("G"), ls_barcode, ls_error });
                this.listViewInfo.Items.Add(item);
            }
            catch (Exception exception)
            {
                this.AddErrToListView("增加提示记录异常！" + exception.Message);
                LogHelper.LogSimlpleString(exception.Message);
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblDBConStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPLCStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblServerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTasks_handcode = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTasks_error = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTasks_noport = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTasks_nodata = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFinishedTasks = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblWorkStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSannerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageListView = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.labelBillQuery = new System.Windows.Forms.Label();
            this.textBoxBillCode = new System.Windows.Forms.TextBox();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.groupBoxQueryBill = new System.Windows.Forms.GroupBox();
            this.buttonHandModify = new System.Windows.Forms.Button();
            this.labelBillInf = new System.Windows.Forms.Label();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listViewTask = new System.Windows.Forms.ListView();
            this.listViewCommand = new System.Windows.Forms.ListView();
            this.labelrunMode_name = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel_car = new System.Windows.Forms.Panel();
            this.label_time = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.lb_speed = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bt_plcworkstatus = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rb_stop = new System.Windows.Forms.RadioButton();
            this.rb_init = new System.Windows.Forms.RadioButton();
            this.rb_start = new System.Windows.Forms.RadioButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.button2 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.listViewInfo = new System.Windows.Forms.ListView();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxQueryBill.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel_car.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Silver;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblDBConStatus,
            this.lblPLCStatus,
            this.lblServerStatus,
            this.lblTasks_handcode,
            this.toolStripStatusLabel2,
            this.lblTasks_error,
            this.toolStripStatusLabel6,
            this.lblTasks_noport,
            this.toolStripStatusLabel4,
            this.lblTasks_nodata,
            this.toolStripStatusLabel3,
            this.lblFinishedTasks,
            this.toolStripStatusLabel1,
            this.lblWorkStatus});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 949);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1910, 51);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblDBConStatus
            // 
            this.lblDBConStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblDBConStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblDBConStatus.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDBConStatus.ForeColor = System.Drawing.Color.Red;
            this.lblDBConStatus.Image = global::WCS.Properties.Resources.yes;
            this.lblDBConStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDBConStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lblDBConStatus.Name = "lblDBConStatus";
            this.lblDBConStatus.Size = new System.Drawing.Size(172, 46);
            this.lblDBConStatus.Text = "数据库未连接";
            // 
            // lblPLCStatus
            // 
            this.lblPLCStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblPLCStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblPLCStatus.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.lblPLCStatus.ForeColor = System.Drawing.Color.Red;
            this.lblPLCStatus.Image = ((System.Drawing.Image)(resources.GetObject("lblPLCStatus.Image")));
            this.lblPLCStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lblPLCStatus.Name = "lblPLCStatus";
            this.lblPLCStatus.Size = new System.Drawing.Size(150, 46);
            this.lblPLCStatus.Text = "PLC未连接";
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblServerStatus.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.lblServerStatus.ForeColor = System.Drawing.Color.Red;
            this.lblServerStatus.Image = ((System.Drawing.Image)(resources.GetObject("lblServerStatus.Image")));
            this.lblServerStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(187, 46);
            this.lblServerStatus.Text = "后台服务未连接";
            // 
            // lblTasks_handcode
            // 
            this.lblTasks_handcode.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblTasks_handcode.AutoSize = false;
            this.lblTasks_handcode.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.lblTasks_handcode.ForeColor = System.Drawing.Color.Black;
            this.lblTasks_handcode.Name = "lblTasks_handcode";
            this.lblTasks_handcode.Size = new System.Drawing.Size(60, 45);
            this.lblTasks_handcode.Text = "0";
            this.lblTasks_handcode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(74, 46);
            this.toolStripStatusLabel2.Text = "补码件:";
            // 
            // lblTasks_error
            // 
            this.lblTasks_error.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblTasks_error.AutoSize = false;
            this.lblTasks_error.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblTasks_error.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.lblTasks_error.ForeColor = System.Drawing.Color.Black;
            this.lblTasks_error.Name = "lblTasks_error";
            this.lblTasks_error.Size = new System.Drawing.Size(60, 45);
            this.lblTasks_error.Text = "0";
            this.lblTasks_error.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabel6.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.toolStripStatusLabel6.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(74, 46);
            this.toolStripStatusLabel6.Text = "错发件:";
            // 
            // lblTasks_noport
            // 
            this.lblTasks_noport.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblTasks_noport.AutoSize = false;
            this.lblTasks_noport.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblTasks_noport.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.lblTasks_noport.ForeColor = System.Drawing.Color.Black;
            this.lblTasks_noport.Name = "lblTasks_noport";
            this.lblTasks_noport.Size = new System.Drawing.Size(60, 45);
            this.lblTasks_noport.Text = "0";
            this.lblTasks_noport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabel4.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.toolStripStatusLabel4.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(93, 46);
            this.toolStripStatusLabel4.Text = "无格口件:";
            // 
            // lblTasks_nodata
            // 
            this.lblTasks_nodata.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblTasks_nodata.AutoSize = false;
            this.lblTasks_nodata.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblTasks_nodata.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.lblTasks_nodata.ForeColor = System.Drawing.Color.Black;
            this.lblTasks_nodata.Name = "lblTasks_nodata";
            this.lblTasks_nodata.Size = new System.Drawing.Size(60, 45);
            this.lblTasks_nodata.Text = "0";
            this.lblTasks_nodata.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabel3.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(93, 46);
            this.toolStripStatusLabel3.Text = "无数据件:";
            // 
            // lblFinishedTasks
            // 
            this.lblFinishedTasks.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblFinishedTasks.AutoSize = false;
            this.lblFinishedTasks.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblFinishedTasks.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.lblFinishedTasks.ForeColor = System.Drawing.Color.Black;
            this.lblFinishedTasks.Name = "lblFinishedTasks";
            this.lblFinishedTasks.Size = new System.Drawing.Size(70, 45);
            this.lblFinishedTasks.Text = "0";
            this.lblFinishedTasks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(55, 46);
            this.toolStripStatusLabel1.Text = "总量:";
            // 
            // lblWorkStatus
            // 
            this.lblWorkStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblWorkStatus.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.lblWorkStatus.ForeColor = System.Drawing.Color.Red;
            this.lblWorkStatus.Image = ((System.Drawing.Image)(resources.GetObject("lblWorkStatus.Image")));
            this.lblWorkStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lblWorkStatus.Name = "lblWorkStatus";
            this.lblWorkStatus.Size = new System.Drawing.Size(92, 46);
            this.lblWorkStatus.Text = "脱机";
            // 
            // lblSannerStatus
            // 
            this.lblSannerStatus.Name = "lblSannerStatus";
            this.lblSannerStatus.Size = new System.Drawing.Size(23, 23);
            // 
            // imageListView
            // 
            this.imageListView.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListView.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListView.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl1.Location = new System.Drawing.Point(0, 57);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1375, 656);
            this.tabControl1.TabIndex = 33;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.LightCyan;
            this.tabPage1.Controls.Add(this.labelBillQuery);
            this.tabPage1.Controls.Add(this.textBoxBillCode);
            this.tabPage1.Controls.Add(this.textBoxResult);
            this.tabPage1.Controls.Add(this.groupBoxQueryBill);
            this.tabPage1.Font = new System.Drawing.Font("宋体", 14F);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1367, 626);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "运行监控";
            // 
            // labelBillQuery
            // 
            this.labelBillQuery.AutoSize = true;
            this.labelBillQuery.Location = new System.Drawing.Point(10, 549);
            this.labelBillQuery.Name = "labelBillQuery";
            this.labelBillQuery.Size = new System.Drawing.Size(104, 19);
            this.labelBillQuery.TabIndex = 3;
            this.labelBillQuery.Text = "运单条码：";
            // 
            // textBoxBillCode
            // 
            this.textBoxBillCode.Location = new System.Drawing.Point(114, 546);
            this.textBoxBillCode.Name = "textBoxBillCode";
            this.textBoxBillCode.Size = new System.Drawing.Size(260, 29);
            this.textBoxBillCode.TabIndex = 2;
            this.textBoxBillCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxBillCode_KeyPress);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(114, 587);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(894, 28);
            this.textBoxResult.TabIndex = 1;
            // 
            // groupBoxQueryBill
            // 
            this.groupBoxQueryBill.Controls.Add(this.buttonHandModify);
            this.groupBoxQueryBill.Controls.Add(this.labelBillInf);
            this.groupBoxQueryBill.Controls.Add(this.buttonQuery);
            this.groupBoxQueryBill.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBoxQueryBill.Font = new System.Drawing.Font("宋体", 9F);
            this.groupBoxQueryBill.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBoxQueryBill.Location = new System.Drawing.Point(0, 521);
            this.groupBoxQueryBill.Name = "groupBoxQueryBill";
            this.groupBoxQueryBill.Size = new System.Drawing.Size(1131, 100);
            this.groupBoxQueryBill.TabIndex = 4;
            this.groupBoxQueryBill.TabStop = false;
            this.groupBoxQueryBill.Text = "运单信息查询";
            // 
            // buttonHandModify
            // 
            this.buttonHandModify.Font = new System.Drawing.Font("宋体", 14F);
            this.buttonHandModify.Location = new System.Drawing.Point(631, 16);
            this.buttonHandModify.Name = "buttonHandModify";
            this.buttonHandModify.Size = new System.Drawing.Size(120, 35);
            this.buttonHandModify.TabIndex = 5;
            this.buttonHandModify.Text = "手工修改";
            this.buttonHandModify.UseVisualStyleBackColor = true;
            this.buttonHandModify.Click += new System.EventHandler(this.buttonHandModify_Click);
            // 
            // labelBillInf
            // 
            this.labelBillInf.AutoSize = true;
            this.labelBillInf.Font = new System.Drawing.Font("宋体", 14F);
            this.labelBillInf.Location = new System.Drawing.Point(10, 60);
            this.labelBillInf.Name = "labelBillInf";
            this.labelBillInf.Size = new System.Drawing.Size(104, 19);
            this.labelBillInf.TabIndex = 4;
            this.labelBillInf.Text = "运单信息：";
            // 
            // buttonQuery
            // 
            this.buttonQuery.Font = new System.Drawing.Font("宋体", 14F);
            this.buttonQuery.Location = new System.Drawing.Point(419, 16);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(120, 35);
            this.buttonQuery.TabIndex = 0;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.LightCyan;
            this.tabPage2.Controls.Add(this.listViewTask);
            this.tabPage2.Controls.Add(this.listViewCommand);
            this.tabPage2.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1367, 626);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "任务日志";
            // 
            // listViewTask
            // 
            this.listViewTask.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewTask.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewTask.FullRowSelect = true;
            this.listViewTask.GridLines = true;
            this.listViewTask.Location = new System.Drawing.Point(0, 0);
            this.listViewTask.Name = "listViewTask";
            this.listViewTask.Size = new System.Drawing.Size(570, 635);
            this.listViewTask.SmallImageList = this.imageListView;
            this.listViewTask.TabIndex = 0;
            this.listViewTask.UseCompatibleStateImageBehavior = false;
            this.listViewTask.View = System.Windows.Forms.View.Details;
            this.listViewTask.DoubleClick += new System.EventHandler(this.listViewTask_DoubleClick);
            // 
            // listViewCommand
            // 
            this.listViewCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCommand.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewCommand.FullRowSelect = true;
            this.listViewCommand.Location = new System.Drawing.Point(835, 0);
            this.listViewCommand.Name = "listViewCommand";
            this.listViewCommand.Size = new System.Drawing.Size(231, 628);
            this.listViewCommand.TabIndex = 4;
            this.listViewCommand.UseCompatibleStateImageBehavior = false;
            this.listViewCommand.View = System.Windows.Forms.View.Details;
            // 
            // labelrunMode_name
            // 
            this.labelrunMode_name.AutoSize = true;
            this.labelrunMode_name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(194)))));
            this.labelrunMode_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelrunMode_name.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelrunMode_name.Location = new System.Drawing.Point(863, 9);
            this.labelrunMode_name.Name = "labelrunMode_name";
            this.labelrunMode_name.Size = new System.Drawing.Size(367, 55);
            this.labelrunMode_name.TabIndex = 34;
            this.labelrunMode_name.Text = "交叉带分拣系统";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Lime;
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Location = new System.Drawing.Point(7, 62);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 32);
            this.button3.TabIndex = 39;
            this.button3.Text = "正常分配";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Yellow;
            this.button5.Enabled = false;
            this.button5.Font = new System.Drawing.Font("宋体", 12F);
            this.button5.Location = new System.Drawing.Point(114, 93);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(103, 32);
            this.button5.TabIndex = 41;
            this.button5.Text = "需要补码";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Red;
            this.button6.Enabled = false;
            this.button6.Font = new System.Drawing.Font("宋体", 12F);
            this.button6.Location = new System.Drawing.Point(114, 62);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(103, 32);
            this.button6.TabIndex = 42;
            this.button6.Text = "故障";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Gray;
            this.button4.Enabled = false;
            this.button4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.Location = new System.Drawing.Point(7, 31);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(103, 32);
            this.button4.TabIndex = 40;
            this.button4.Text = "无货";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Orange;
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("宋体", 12F);
            this.button1.Location = new System.Drawing.Point(7, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 32);
            this.button1.TabIndex = 39;
            this.button1.Text = "条码未识别";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(25, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 19);
            this.label1.TabIndex = 48;
            this.label1.Text = "格口状态";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Lime;
            this.button7.Enabled = false;
            this.button7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button7.Location = new System.Drawing.Point(16, 31);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(103, 32);
            this.button7.TabIndex = 47;
            this.button7.Text = "正常";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Yellow;
            this.button8.Enabled = false;
            this.button8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button8.Location = new System.Drawing.Point(16, 93);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(103, 32);
            this.button8.TabIndex = 49;
            this.button8.Text = "正在集包";
            this.button8.UseVisualStyleBackColor = false;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.Red;
            this.button9.Enabled = false;
            this.button9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button9.Location = new System.Drawing.Point(16, 62);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(103, 31);
            this.button9.TabIndex = 49;
            this.button9.Text = "故障";
            this.button9.UseVisualStyleBackColor = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(194)))));
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton10,
            this.toolStripButton2,
            this.toolStripButton5});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1910, 57);
            this.toolStrip2.TabIndex = 38;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripLabel1.Image = global::WCS.Properties.Resources.SNBC;
            this.toolStripLabel1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(180, 54);
            this.toolStripLabel1.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(124, 54);
            this.toolStripButton3.Text = "设置参数";
            this.toolStripButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton3.Click += new System.EventHandler(this.btnSetPara_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(124, 54);
            this.toolStripButton4.Text = "格口管理";
            this.toolStripButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton4.Click += new System.EventHandler(this.btnSetPort_Click);
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton10.Image")));
            this.toolStripButton10.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(124, 54);
            this.toolStripButton10.Text = "统计查询";
            this.toolStripButton10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton10.Click += new System.EventHandler(this.toolStripButton10_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::WCS.Properties.Resources.plan_01;
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(124, 54);
            this.toolStripButton2.Text = "分拣计划";
            this.toolStripButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(54, 54);
            this.toolStripButton5.Text = "退出";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click_1);
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1805, 657);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "视图二";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel_car);
            this.tabPage3.Location = new System.Drawing.Point(4, 31);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1946, 648);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "实时监控";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel_car
            // 
            this.panel_car.BackColor = System.Drawing.Color.White;
            this.panel_car.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel_car.Controls.Add(this.label_time);
            this.panel_car.Controls.Add(this.label8);
            this.panel_car.Controls.Add(this.button11);
            this.panel_car.Controls.Add(this.button10);
            this.panel_car.Controls.Add(this.lb_speed);
            this.panel_car.Controls.Add(this.label6);
            this.panel_car.Controls.Add(this.label5);
            this.panel_car.Controls.Add(this.label3);
            this.panel_car.Controls.Add(this.bt_plcworkstatus);
            this.panel_car.Controls.Add(this.label2);
            this.panel_car.Location = new System.Drawing.Point(0, 0);
            this.panel_car.Margin = new System.Windows.Forms.Padding(0);
            this.panel_car.Name = "panel_car";
            this.panel_car.Size = new System.Drawing.Size(1900, 800);
            this.panel_car.TabIndex = 0;
            // 
            // label_time
            // 
            this.label_time.AutoSize = true;
            this.label_time.Location = new System.Drawing.Point(1747, 505);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(55, 22);
            this.label_time.TabIndex = 396;
            this.label_time.Text = "0.000";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(1615, 506);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(142, 19);
            this.label8.TabIndex = 395;
            this.label8.Text = "本次运行时间：";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(32, 704);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 394;
            this.button11.Text = "button11";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(32, 693);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 393;
            this.button10.Text = "button10";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // lb_speed
            // 
            this.lb_speed.AutoSize = true;
            this.lb_speed.Location = new System.Drawing.Point(1727, 549);
            this.lb_speed.Name = "lb_speed";
            this.lb_speed.Size = new System.Drawing.Size(87, 22);
            this.lb_speed.TabIndex = 392;
            this.lb_speed.Text = "0.000m/s";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(1617, 550);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 19);
            this.label6.TabIndex = 391;
            this.label6.Text = "环线速度：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(1618, 599);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 19);
            this.label5.TabIndex = 51;
            this.label5.Text = "PLC状态：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(475, 697);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 390;
            this.label3.Text = "小车状态：";
            // 
            // bt_plcworkstatus
            // 
            this.bt_plcworkstatus.BackColor = System.Drawing.Color.Lime;
            this.bt_plcworkstatus.Enabled = false;
            this.bt_plcworkstatus.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_plcworkstatus.Location = new System.Drawing.Point(1720, 591);
            this.bt_plcworkstatus.Name = "bt_plcworkstatus";
            this.bt_plcworkstatus.Size = new System.Drawing.Size(129, 36);
            this.bt_plcworkstatus.TabIndex = 50;
            this.bt_plcworkstatus.Text = "正常";
            this.bt_plcworkstatus.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(517, 693);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 22);
            this.label2.TabIndex = 389;
            this.label2.Text = "label2";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl2.Location = new System.Drawing.Point(33, 112);
            this.tabControl2.Multiline = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1954, 683);
            this.tabControl2.TabIndex = 44;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(204)))));
            this.panel1.Controls.Add(this.rb_stop);
            this.panel1.Controls.Add(this.rb_init);
            this.panel1.Controls.Add(this.rb_start);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1910, 53);
            this.panel1.TabIndex = 50;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // rb_stop
            // 
            this.rb_stop.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_stop.BackColor = System.Drawing.Color.Transparent;
            this.rb_stop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rb_stop.FlatAppearance.BorderSize = 0;
            this.rb_stop.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.rb_stop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.rb_stop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.rb_stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_stop.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.rb_stop.Image = global::WCS.Properties.Resources.stop_01;
            this.rb_stop.Location = new System.Drawing.Point(258, 0);
            this.rb_stop.Name = "rb_stop";
            this.rb_stop.Size = new System.Drawing.Size(101, 50);
            this.rb_stop.TabIndex = 117;
            this.rb_stop.UseVisualStyleBackColor = false;
            this.rb_stop.Click += new System.EventHandler(this.rb_stop_Click);
            this.rb_stop.MouseLeave += new System.EventHandler(this.rb_stop_MouseLeave);
            this.rb_stop.MouseHover += new System.EventHandler(this.rb_stop_MouseHover);
            // 
            // rb_init
            // 
            this.rb_init.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_init.BackColor = System.Drawing.Color.Transparent;
            this.rb_init.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rb_init.FlatAppearance.BorderSize = 0;
            this.rb_init.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.rb_init.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.rb_init.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.rb_init.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb_init.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_init.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.rb_init.Image = global::WCS.Properties.Resources.connect_01;
            this.rb_init.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rb_init.Location = new System.Drawing.Point(13, 0);
            this.rb_init.Name = "rb_init";
            this.rb_init.Size = new System.Drawing.Size(132, 50);
            this.rb_init.TabIndex = 117;
            this.rb_init.UseVisualStyleBackColor = false;
            this.rb_init.Click += new System.EventHandler(this.rb_init_Click);
            this.rb_init.MouseLeave += new System.EventHandler(this.rb_init_MouseLeave);
            this.rb_init.MouseHover += new System.EventHandler(this.rb_init_MouseHover);
            // 
            // rb_start
            // 
            this.rb_start.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_start.BackColor = System.Drawing.Color.Transparent;
            this.rb_start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rb_start.FlatAppearance.BorderSize = 0;
            this.rb_start.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.rb_start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.rb_start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.rb_start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_start.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.rb_start.Image = global::WCS.Properties.Resources.start_01;
            this.rb_start.Location = new System.Drawing.Point(151, 0);
            this.rb_start.Name = "rb_start";
            this.rb_start.Size = new System.Drawing.Size(101, 50);
            this.rb_start.TabIndex = 117;
            this.rb_start.UseVisualStyleBackColor = false;
            this.rb_start.Click += new System.EventHandler(this.rb_start_Click);
            this.rb_start.MouseLeave += new System.EventHandler(this.rb_start_MouseLeave);
            this.rb_start.MouseHover += new System.EventHandler(this.rb_start_MouseHover);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(72, 53);
            this.toolStripButton1.Text = "初始化设备";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.RoyalBlue;
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("宋体", 12F);
            this.button2.Location = new System.Drawing.Point(114, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 32);
            this.button2.TabIndex = 45;
            this.button2.Text = "格口停用";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Location = new System.Drawing.Point(26, 806);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(226, 134);
            this.panel2.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(71, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 19);
            this.label4.TabIndex = 49;
            this.label4.Text = "小车状态";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button7);
            this.panel3.Controls.Add(this.button9);
            this.panel3.Controls.Add(this.button8);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(258, 806);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(133, 134);
            this.panel3.TabIndex = 52;
            // 
            // listViewInfo
            // 
            this.listViewInfo.Location = new System.Drawing.Point(857, 806);
            this.listViewInfo.Name = "listViewInfo";
            this.listViewInfo.Size = new System.Drawing.Size(1041, 140);
            this.listViewInfo.TabIndex = 53;
            this.listViewInfo.UseCompatibleStateImageBehavior = false;
            this.listViewInfo.View = System.Windows.Forms.View.Details;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1910, 1000);
            this.Controls.Add(this.listViewInfo);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelrunMode_name);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "新北洋交叉带环形分拣机控制系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBoxQueryBill.ResumeLayout(false);
            this.groupBoxQueryBill.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.panel_car.ResumeLayout(false);
            this.panel_car.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            if (!this.dbIsConned)
            {
                this.dbIsConned = this.DBIsConnect();
            }
            if (this.dbIsConned)
            {
                this.InitialDev();
            }
            this.setMenu();
        }

        private void setMenu()
        {
            if(1 == this.workModel)
              this.rb_init.Enabled =false;
            //this.btnConn.Enabled = !this.dbIsConned && this.IsRegistered;
            //this.btnConnPLC.Enabled = (!this.plcIsOK && this.dbIsConned) && this.IsRegistered;
            //this.btnInitDev.Enabled = ((!this.deviceInited && this.plcIsOK) && this.dbIsConned) && this.IsRegistered;
            //this.btnStrat.Enabled = ((this.deviceInited && this.plcIsOK) && (this.dbIsConned && (this.workModel != 1))) && this.IsRegistered;
           
        }

        private bool DBIsConnect()
        {
            try
            {
                //获取配置文件
                XmlDocument document = new XmlDocument();
                document.Load(this.filename);
                XmlNode node = document.DocumentElement.SelectSingleNode("/configuration/parameter/db1ip");
                String s_ip = node.InnerText;
                node = document.DocumentElement.SelectSingleNode("/configuration/parameter/plcip");
                this.plc_ip = node.InnerText;
                this.dbConnectionString = string.Format("Server = {0}; UserId = {1}; Password ={2}; Database ={3}", s_ip, "admin","123456","expressdb");
                this.dbConn = new MySqlConnection(this.dbConnectionString);
                this.dbConn.Open();
                DataBase.dbConn = this.dbConn;
                this.lblDBConStatus.Text = "数据库已连接！";
                this.lblDBConStatus.ForeColor = Color.Green;
                this.lblDBConStatus.Image = Resources.yes;
                this.AddSuccessToListView("wcs数据库连接成功！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "wcs数据库连接成功");
            }
            catch (Exception exception)
            {
                this.lblDBConStatus.Text = "数据库未连接！";
                this.lblDBConStatus.Image = Resources.warning;
                this.lblDBConStatus.ForeColor = Color.Red;
                this.AddErrToListView("wcs数据库连接异常！" + exception.Message.ToString());
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "wcs数据库连接异常！" + exception.Message.ToString());
                return false;
            }
            return true;
        }


        private bool ServerIsConnect()
        {
            st_client = new CommTcpClient();
            //获取配置文件
            XmlDocument document = new XmlDocument();
            document.Load(this.filename);
            XmlNode node = document.DocumentElement.SelectSingleNode("/configuration/parameter/serverip");
            string s_ip = node.InnerText;
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/serverport");
            String s_port = node.InnerText;

            st_client.Init(s_ip, int.Parse(s_port));
            try
            {
                bool bret = st_client.Connect();
                if (bret)
                {
                    this.lblServerStatus.Text = "服务器已连接！";
                    this.lblServerStatus.Image = Resources.yes;
                    this.lblServerStatus.ForeColor = Color.Green;
                    return true;
                }
                else
                {
                    this.lblServerStatus.Text = "服务器未连接！";
                    this.lblServerStatus.Image = Resources.warning;
                    this.lblServerStatus.ForeColor = Color.Red;
                    return false;
                }
            }
            catch (Exception exception)
            {
                this.lblServerStatus.Text = "服务器未连接！";
                this.lblServerStatus.Image = Resources.warning;
                this.lblServerStatus.ForeColor = Color.Red;
                this.AddErrToListView("服务器连接异常！" + exception.Message.ToString());
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "服务器连接异常！" + exception.Message.ToString());

            }
            return false;
        }
     
        private bool PlcIsConnect()
        {
            bool bret = false;
            Form form = new Form
            {
                Text = "正在连接PLC，请耐心等候......",
                ClientSize = new Size(0x124, 10),
                ControlBox = false,
                MaximizeBox = false,
                MinimizeBox = false,
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                StartPosition = FormStartPosition.CenterScreen
            };
            form.Show();

            this.plcIsOK = this.PLCConnection();
            form.Close();

            if (!this.plcIsOK)
            {
                this.lblPLCStatus.Text = "连接PLC失败！";
                this.lblPLCStatus.ForeColor = Color.Red;
                this.AddErrToListView("连接PLC失败！");
                this.lblPLCStatus.Image = Resources.warning;
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "连接PLC失败！");
                return false;
            }
            else
            {

                this.AddSuccessToListView("连接PLC成功！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "连接PLC成功！");
            }

            //PLC 连接成功后，需要建立opc协议
            //  if (true/*this.plcIsOK*/)
            {
                
                bret = this.car_dev.BindToPLC(100);
                if (!bret)
                {
                    this.lblPLCStatus.Text = "PLC通信错误！";
                    this.lblPLCStatus.ForeColor = Color.Red;
                    this.lblPLCStatus.Image = Resources.warning;
                    LogHelper.LogSimlpleString("car_dev连接失败！");
                    
                   // return false;
                }
               
                bret = this.car_pos.BindToPLC(100);
                if (!bret)
                {
                    this.lblPLCStatus.Text = "PLC通信错误！";
                    this.lblPLCStatus.ForeColor = Color.Red;
                    this.lblPLCStatus.Image = Resources.warning;
                    LogHelper.LogSimlpleString("car_pos连接失败！");
                   // return false;
                }
                this.ssj_dev.BindToPLC();
               
                this.deviceInited = true;
                //this.check_init();
                //this.setMenu();
                LogHelper.LogSimlpleString("car_pos连接失败！");

            }
            this.lblPLCStatus.Text = "PLC已连接";
            this.lblPLCStatus.ForeColor = Color.Green;
            this.lblPLCStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            return true;
        }

        private bool PLCConnection()
        {
            this.plcSystemMS = new PlcSystemMS(this);
            if (!this.plcSystemMS.RefreshStatus())
            {
                return false;
            }
            return true;
        }

        private void btnESC_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnInitDev_Click(object sender, EventArgs e)
        {
           this.car_dev.BindToPLC(100);
           this.car_pos.BindToPLC(100);

           
            //this.kzan_dev.BindToPLC();
            this.deviceInited = true;
            if (!this.deviceInited)
            {
                this.AddErrToListView("初始化设备失败！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "初始化设备失败！");
            }
            else
            {
                this.AddSuccessToListView("初始化设备成功！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "初始化设备成功！");
            }
            this.setMenu();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void btnSetPara_Click(object sender, EventArgs e)
        {
            new ParamSettings(this).ShowDialog();
        }

        private void btnSetPort_Click(object sender, EventArgs e)//格口管理
        {
            new FormPortManage(this).ShowDialog();
        }

        private void btnSetWmsCon_Click(object sender, EventArgs e)
        {
            new FormPortManage(this).ShowDialog();
        }

        private void btnSortNum_Click(object sender, EventArgs e)
        {
            new FormSortNum().ShowDialog();
        }

        private void btnStopWork_Click(object sender, EventArgs e)
        {
        }

        private void check_init()
        {
            try
            {
                this.plcIsOK = true;// wy
                this.deviceInited = true;
                if ((this.dbIsConned && this.plcIsOK) && this.deviceInited)
                {
                    this.workModel = 1;
                    this.lblWorkStatus.Text = "联机工作！";
                    this.lblWorkStatus.ForeColor = Color.Green;
                    this.lblWorkStatus.Image = Resources.yes;
                    this.setMenu();
                }
            }
            catch (Exception exception)
            {
                this.AddErrToListView("联机工作异常！" + exception.Message.ToString());
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "联机工作异常！" + exception.Message.ToString());
                return;
            }
            this.AddSuccessToListView("开始联机工作！");
            LogHelper.LogSimlpleString(DateTime.Now.ToString() + "开始联机工作！");

        }

        private void buttonHandModify_Click(object sender, EventArgs e)
        {
            new FormUpdateSiteCode(this, this.textBoxBillCode.Text.Trim()).ShowDialog();
            this.buttonQuery_Click(null, null);
        }

        public void update_register_code()
        {
            /*string str2 = DESPwd.DesEncrypt(this.max_time.ToString().PadLeft(8, '0') + "-" + this.used_time.ToString().PadLeft(8, '0'));
            try
            {
                if (!DataBase.Update_register_code(str2))
                {
                    this.AddErrToListView("连接数据库失败！");
                    LogHelper.LogSimlpleString(DateTime.Now.ToString() + "连接数据库失败！");
                }
            }
            catch (Exception exception)
            {
                this.AddErrToListView("更新注册码异常！" + exception.Message.ToString());
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "更新注册码异常！" + exception.Message.ToString());
            }
            if (this.max_time <= this.used_time)
            {
                this.IsRegistered = false;
                this.AddErrToListView("系统注册码已过期！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "系统注册码已过期！");
            }*/
        }
        // 更新界面 plc状态和 速度信息
        private void timer1_Tick(object sender, EventArgs e)
        {   if (1 == this.plcSystemMS.plcRunStatus)
            {
                this.bt_plcworkstatus.Text = "正在初始化";
                this.bt_plcworkstatus.BackColor = Color.LightSkyBlue;
            }
            if(3 == this.plcSystemMS.plcRunStatus)
            {
                this.bt_plcworkstatus.Text = "正在运行";
                this.bt_plcworkstatus.BackColor = Color.Lime;
            }
            else if(4 == this.plcSystemMS.plcRunStatus)
            {
                this.bt_plcworkstatus.Text = "停止";
                this.bt_plcworkstatus.BackColor = Color.Red;
            }
            return;
            this.lb_speed.Text = (this.plcSystemMS.carSpeedValue / 1000.0).ToString() + "米/秒";
            this.label_time.Text = this.plcSystemMS.getTotalRunTime();
        }

        private void textBoxBillCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.buttonQuery_Click(null, null);
            }
        }

        private void listViewTask_DoubleClick(object sender, EventArgs e)
        {
            if ((this.listViewTask.SelectedIndices != null) && (this.listViewTask.SelectedIndices.Count > 0))
            {
                this.textBoxBillCode.Text = this.listViewTask.FocusedItem.SubItems[1].Text;
                this.buttonQuery_Click(null, null);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        public bool Query_register_code()
        {
            /*string str = DataBase.Query_register_code();
            if ((str == "F") || string.IsNullOrEmpty(str))
            {
                return false;
            }
            string str2 = DESPwd.DesDecrypt(str);
            if ((str2.Length != 0x11) || (str2.Substring(8, 1) != "-"))
            {
                return false;
            }
            try
            {
                this.max_time = long.Parse(str2.Substring(0, 8));
                this.used_time = long.Parse(str2.Substring(9, 8));
            }
            catch (Exception)
            {
                return false;
            }*/
            return true;
        }

        private void delete_history_data()
        {
            string strErrorMsg = string.Empty;
            DataBase.delete_history_data(ref strErrorMsg);
            if (strErrorMsg.Length > 0)
            {
                this.AddErrToListView("清除历史数据失败！" + strErrorMsg);
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "清除历史数据失败！" + strErrorMsg);
            }
            else
            {
                this.AddSuccessToListView("清除历史数据成功！");
                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "清除历史数据成功！");
                try
                {
                    if (!Directory.Exists(this.log_Path))
                    {
                        this.AddErrToListView("不存在的路径：" + this.log_Path);
                        LogHelper.LogSimlpleString(DateTime.Now.ToString() + "不存在的路径：" + this.log_Path);
                    }
                    else
                    {
                        foreach (string str2 in Directory.GetFiles(this.log_Path))
                        {
                            FileInfo info = new FileInfo(str2);
                            if (!info.Exists)
                            {
                                this.AddErrToListView(str2 + "不存在!");
                            }
                            else if (info.LastWriteTime < DateTime.Now.AddDays(-30.0))
                            {
                                try
                                {
                                    System.IO.File.Delete(str2);
                                }
                                catch (Exception exception)
                                {
                                    this.AddErrToListView("删除文件" + str2 + "出现异常：" + exception.Message);
                                }
                            }
                        }
                    }
                }
                catch (Exception exception2)
                {
                    this.AddErrToListView("删除历史文件异常" + exception2.Message);
                    LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "删除历史文件异常" + exception2.Message);
                }
            }
        }

        public void InitWcsPortConf()//获取分拣口信息，包括目的地、位置、编号等信息
        {
            DataSet set = new DataSet();
            try
            {
                MySqlConnection connection = new MySqlConnection(this.dbConnectionString);
                connection.Open();
                set = MySqlHelper.ExecuteDataset(connection, "SELECT port_id,siteid FROM td_port where pipeline='" + this.deviceID + "'");
                if ((connection != null) && (connection.State == ConnectionState.Open))
                {
                    connection.Close();
                }
                if (set.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("分拣口信息结果集为空，无法生成分拣编码与分拣格口的对应字典", "系统提示");
                    LogHelper.LogSimlpleString(DateTime.Now.ToString() + "分拣口信息结果集为空，无法生成分拣编码与分拣格口的对应字典");
                }
                else
                {
                    this.sortcode_outport_list.Clear();
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        sortcode_outport _outport;
                        _outport.destSortingCode = row["siteid"].ToString();// 路由
                        _outport.sortPortCode = row["port_id"].ToString();//分拣口
                        if (!this.sortcode_outport_list.Contains(_outport))
                        {
                            this.sortcode_outport_list.Add(_outport);
                        }
                    }
                    this.AddSuccessToListView("生成分拣编码与分拣格口的对应字典成功!结果集行数为" + this.sortcode_outport_list.Count.ToString());
                    LogHelper.LogSimlpleString(DateTime.Now.ToString() + "生成分拣编码与分拣格口的对应字典成功!结果集行数为" + this.sortcode_outport_list.Count.ToString());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("生成分拣编码与站点信息的对应字典异常" + exception.Message, "系统提示");
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "生成分拣编码与站点信息的对应字典异常" + exception.Message);
            }


            //string str = string.Empty;
            //try
            //{
            //    WebClient client = new WebClient();
            //    Uri address = new Uri(this.zt_url, UriKind.Absolute);//xwc 从哪儿获取端口信息，作用是什么？
            //    if (!client.IsBusy)
            //    {
            //        client.Encoding = Encoding.UTF8;
            //        str = client.DownloadString(address);
            //    }
            //    client.Dispose();
            //}
            //catch (Exception exception)
            //{
            //    this.AddErrToListView($"获取WCS端口配置失败:{exception.Message}");
            //    LogHelper.LogSimlpleString(DateTime.Now.ToString() + $" 获取WCS端口配置失败:{exception.Message}");
            //    return false;
            //}
            //try
            //{
            //    PortConfReturn return2 = (PortConfReturn)JsonConvert.DeserializeObject(str, typeof(PortConfReturn));
            //    if (return2.status && (return2.statusCode == "SUCCESS"))
            //    {
            //        if (return2.result == null)
            //        {
            //            this.AddErrToListView("获取分拣口信息失败:返回结果集为空");
            //            LogHelper.LogSimlpleString(DateTime.Now.ToString() + "获取分拣口信息失败:返回结果集为空");
            //            return false;
            //        }
            //        this.wcsPortConfList = return2.result.ToList();
            //        if (this.wcsPortConfList.Count == 0)
            //        {
            //            this.AddErrToListView("获取分拣口信息失败:返回结果集行数为0");
            //            LogHelper.LogSimlpleString(DateTime.Now.ToString() + "获取分拣口信息失败:返回结果集行数为0");
            //            return false;
            //        }
            //        this.AddSuccessToListView("获取分拣口信息成功!结果集行数为" + this.wcsPortConfList.Count.ToString());
            //        LogHelper.LogSimlpleString(DateTime.Now.ToString() + "获取分拣口信息成功!结果集行数为" + this.wcsPortConfList.Count.ToString());
            //    }
            //    else
            //    {
            //        this.AddErrToListView("获取分拣口信息失败:返回状态为失败" + return2.message);
            //        LogHelper.LogSimlpleString(DateTime.Now.ToString() + "获取分拣口信息失败:返回状态为失败" + return2.message);
            //        return false;
            //    }
            //}
            //catch (Exception exception2)
            //{
            //    this.AddErrToListView($"解析WCS端口配置失败:{exception2.Message}");
            //    LogHelper.LogSimlpleString($" 解析WCS端口配置失败:{exception2.Message}");
            //    return false;
            //}
            //return true;
        }

        private void InitListView()
        {
            this.tabControl1.Width = base.Width - 20;
            this.tabPage1.Width = this.tabControl1.Width;
            this.tabPage2.Width = this.tabControl1.Width;
            this.groupBoxQueryBill.Location = new Point(10, (this.panel_car.Location.Y + this.panel_car.Height) + 30);
            this.groupBoxQueryBill.Size = new Size(this.panel_car.Width - 15, (this.tabPage1.Height - this.groupBoxQueryBill.Location.Y) - 20);
            this.labelBillQuery.Location = new Point(5, 0x21);
            this.textBoxBillCode.Location = new Point(this.labelBillQuery.Location.X + this.labelBillQuery.Width, this.labelBillQuery.Location.Y - 5);
            this.buttonQuery.Location = new Point((this.textBoxBillCode.Location.X + this.textBoxBillCode.Width) + 50, this.labelBillQuery.Location.Y - 8);
            this.labelBillInf.Location = new Point(5, (this.labelBillQuery.Location.Y + this.labelBillQuery.Height) + 20);
            this.textBoxResult.Location = new Point(this.textBoxBillCode.Location.X, (this.buttonQuery.Location.Y + this.buttonQuery.Height) + 10);
            this.textBoxResult.Size = new Size((this.groupBoxQueryBill.Width - this.textBoxBillCode.Location.X) - 5, 60);
            this.buttonHandModify.Location = new Point(this.buttonQuery.Location.X + 220, this.buttonQuery.Location.Y);
            this.labelBillQuery.BringToFront();
            this.textBoxBillCode.BringToFront();
            this.buttonQuery.BringToFront();
            this.labelBillInf.BringToFront();
            this.textBoxResult.BringToFront();
            this.buttonHandModify.BringToFront();
            this.groupBoxQueryBill.Controls.Add(this.labelBillQuery);
            this.groupBoxQueryBill.Controls.Add(this.textBoxBillCode);
            this.groupBoxQueryBill.Controls.Add(this.buttonQuery);
            this.groupBoxQueryBill.Controls.Add(this.labelBillInf);
            this.groupBoxQueryBill.Controls.Add(this.textBoxResult);
            this.labelBillQuery.Font = new Font("宋体", 14f);
            this.textBoxBillCode.Font = new Font("宋体", 14f);
            this.buttonQuery.Font = new Font("宋体", 14f);
            this.labelBillInf.Font = new Font("宋体", 14f);
            this.textBoxResult.Font = new Font("宋体", 14f);
            this.listViewTask.Location = new Point(0, 0);
            this.listViewTask.Size = new Size(750, this.tabPage1.Height);
            this.listViewCommand.Location = new Point(this.listViewTask.Location.X + this.listViewTask.Width, 0);
            this.listViewCommand.Size = new Size(this.tabPage1.Width - this.listViewTask.Width, this.listViewTask.Height);
            this.listViewTask.Columns.Add("扫码时间", (int)(this.listViewTask.Width * 0.25), HorizontalAlignment.Center);
            this.listViewTask.Columns.Add("运单条码", (int)(this.listViewTask.Width * 0.18), HorizontalAlignment.Center);
            this.listViewTask.Columns.Add("异常信息", (int)(this.listViewTask.Width * 0.52), HorizontalAlignment.Center);
            this.listViewTask.GridLines = true;
            this.listViewCommand.Columns.Add("时间", (int)(this.listViewCommand.Width * 0.17), HorizontalAlignment.Left);
            this.listViewCommand.Columns.Add("命令报文", (int)(this.listViewCommand.Width * 3.0), HorizontalAlignment.Left);
            this.listViewInfo.Columns.Add("时间", (int)(this.listViewInfo.Width * 0.25), HorizontalAlignment.Center);
            this.listViewInfo.Columns.Add("信息", (int)(this.listViewInfo.Width * 0.75), HorizontalAlignment.Center);
            
            this.listViewInfo.GridLines = true;
        }

        private void Form1_Load(object sender, EventArgs e)// 程序启动初始化操作
        {
            //初始化
            this.labelrunMode_name.Text = this.runMode_name;
             this.InitListView();
            //TODO:加载配置文件，小车个数，格口数，异常口，通信参数

         // 连接数据库
            if (!this.dbIsConned)
            {
                this.dbIsConned = this.DBIsConnect();
            }
            if (!this.dbIsConned)
            {

                
                //MessageBox.Show("连接数据库失败，请检查本机至服务器的网络连接、数据库是否启动等因素！", "系统提示");
                this.if_must_close = true;
            }
            else
            {
                this.lblDBConStatus.Text = "数据库已连接！";
            }
            // 连接后台服务
            if (!this.socketIsConned)
            {
                this.socketIsConned = this.ServerIsConnect();
             }
            if (!this.socketIsConned)
            {
                //MessageBox.Show("连接后台服务器失败，请检查本机至服务器的网络连接、数据库是否启动等因素！", "系统提示");
                this.if_must_close = true;
            }
           
            // 初始化界面操作
            // 初始化小车元素
            this.car_pos = new Car_Pos(this);

            for (int i = 1; i < this.car_num; i++)
            {
                this.panel_car.Controls.Add(this.car_pos.pic[i]);
                this.car_pos.pic[i].BringToFront();
            }
            this.car_dev = new Car_Dev(this);


            // 初始化落包口元素
            this.ssj_dev = new SSJ_Dev(this);

            for (int k = 0; k < this.port_num; k++)
            {
                this.panel_car.Controls.Add(this.ssj_dev.pic[k]);
                this.ssj_dev.pic[k].BringToFront();
            }
            this.zddr_dev = new ZDDR_Dev(this);
            return;
            //连接PLC
            if (!this.plcIsOK)
            {
                this.plcIsOK = this.PlcIsConnect();
            }
            this.check_init();
            this.setMenu();
           
            //bool bret = this.car_pos.ManualCommandDown(2, "117");
            //bret = this.car_pos.ManualCommandDown(3, "118");
            //bret = this.car_pos.ManualCommandDown(4, "119");
            //bool bret = this.car_pos.CarCtrl(5, "1");
            //bret = this.car_pos.CarCtrl(6, "0");
            //bret = this.car_pos.CarCtrl(7, "0");
            //bret = this.car_pos.CarCtrl(8, "1");
            //this.ssj_dev.ManualCommandWrite(9, "1");
            //this.car_pos.CtrPlcStatus(1);
           
            //界面状态刷新
            this.timer1.Enabled = true;
            this.taskautorun = new TaskAutoRun(this);
            this.noCmdDeviceThread = new Thread(new ThreadStart(this.taskautorun.AutoRun));
            this.noCmdDeviceThread.IsBackground = true;

            if (!this.noCmdDeviceThread.IsAlive)
            {
                this.noCmdDeviceThread.Start();
            }

           
            //this.btnConn_Click(null, null);
            if (this.dbIsConned)
            {
                this.barcodeDef = new BarcodeDef(this);// 运单号规则检测配置
                this.InitWcsPortConf();// 获取分拣口信息，包括目的地、位置、编号等信息
                //init_site_stop_list();
              //  try
                {
                   // new MySqlCommand("insert into tb_sort_num(batch_no,num1,num2,num3,num4,num5,num6,num7,num8,num9,num10,num11,num12,num13,num14,sum_num,sum_nodata,sum_nochannel,sum_error,sum_handcode,update_time)\r\n                                  VALUES('" + this.batch_no + "',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NOW())", this.dbConn).ExecuteNonQuery();
                }
             //   catch (Exception exception)
              //  {
               //     this.AddErrToListView("新增分拣记录1异常！" + exception.Message);
               //     LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "新增分拣记录1异常！" + exception.Message);
                //}
               // this.btnConnPLC_Click(null, null);
                this.timer1.Enabled = true;
            //this.taskautorun = new TaskAutoRun(this);
            //this.noCmdDeviceThread = new Thread(new ThreadStart(this.taskautorun.AutoRun));
            //this.noCmdDeviceThread.IsBackground = true;
            //if (!this.noCmdDeviceThread.IsAlive)
            //{
            //    //this.noCmdDeviceThread.Start();
            //}
           
                // return; 

                SupplyGoodsThread = new Thread(new ThreadStart(this.downloadTask_auto_supply));
                SupplyGoodsThread.IsBackground = true;
                if (!SupplyGoodsThread.IsAlive)
                {
                  //  SupplyGoodsThread.Start();
                }

                itemOffThread = new Thread(new ThreadStart(this.car_dev.itemOffTask));
                itemOffThread.IsBackground = true;
                if (!itemOffThread.IsAlive)
                {
                    itemOffThread.Start();
                }
                this.handcodeCmdDeviceThread = new Thread(new ThreadStart(this.downloadTask_hand));
                this.handcodeCmdDeviceThread.IsBackground = true;
                if (!this.handcodeCmdDeviceThread.IsAlive)
                {
                    this.handcodeCmdDeviceThread.Start();
                }
               
            }
        }

        public void downloadTask_hand()
        {
            string strErrorMsg = string.Empty;
            string comandText = string.Empty;
            DataSet set = new DataSet();
            string commandText = "select * from tb_bill_hand";
            string str4 = string.Empty;
            string str5 = string.Empty;
            int carid = 0;
            try
            {
                this.dbConn_hand = new MySqlConnection(this.dbConnectionString);
                this.dbConn_hand.Open(); 
                goto Label_06A8;
            }
            catch (Exception exception)
            {
                this.AddErrToListView("人工补码专用wcs数据库连接异常！" + exception.Message);
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "人工补码专用wcs数据库连接异常！" + exception.Message);
                return;
            }
        Label_0098:
            this.thrDownloadTaskHandIsRuning = true;
            if ((this.dbIsConned && this.plcIsOK) && this.deviceInited)
            {
                try
                {
                    set = MySqlHelper.ExecuteDataset(this.dbConn_hand, commandText);
                    if (set.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < set.Tables[0].Rows.Count; i++)
                        {
                            this.li_handcode_tasks++;
                            str4 = set.Tables[0].Rows[i]["billCode"].ToString();
                            str5 = set.Tables[0].Rows[i]["sortCode"].ToString();
                            set.Tables[0].Rows[i]["user_id"].ToString();
                            carid = 0;
                            for (int j = 0; j < this.car_dev.billCode.Length; j++)
                            {
                                if ((this.car_dev.billCode[j] == str4) && (((this.car_dev.loadStatus[j] == 1) || (this.car_dev.loadStatus[j] == 2)) || (this.car_dev.loadStatus[j] == 3)))
                                {
                                    carid = j + 1;
                                    break;
                                }
                            }
                            if (carid > 0)
                            {
                                int num4 = 0;
                                int num5 = 0;
                                string str6 = string.Empty;
                                string str7 = string.Empty;
                                string str8 = string.Empty;
                                if ((str5 == "00") || !this.if_right_sort_code(str5))
                                {
                                    num5 = 3;
                                    num4 = this.zddr_dev.error_noport[this.car_dev.sourcePort[carid - 1]];
                                    LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "补码" + str4 + "无通道");
                                }
                                else
                                {
                                    DataSet set2 = new DataSet();
                                    if (this.car_dev.sourcePort[carid - 1] <= 6)
                                    {
                                        string str9 = "SELECT c.port_id,c.person_id,c.if_paijian,c.sub_siteid FROM td_port c,td_port_inf inf where c.port_id = inf.port_id\r\n                        and c.pipeline='" + this.deviceID + "' and c.siteid='" + str5 + "' order by inf.port_status,inf.port_part,inf.num,CAST(c.port_id AS SIGNED)";
                                        set2 = MySqlHelper.ExecuteDataset(this.dbConn_hand, str9);
                                    }
                                    else
                                    {
                                        string str10 = "SELECT c.port_id,c.person_id,c.if_paijian,c.sub_siteid FROM td_port c,td_port_inf inf where c.port_id = inf.port_id\r\n                        and c.pipeline='" + this.deviceID + "' and c.siteid='" + str5 + "' order by inf.port_status,inf.port_part desc,inf.num,CAST(c.port_id AS SIGNED)";
                                        set2 = MySqlHelper.ExecuteDataset(this.dbConn_hand, str10);
                                    }
                                    if (set2.Tables[0].Rows.Count == 0)
                                    {
                                        LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "条码" + str4 + ",补码" + str5 + "无通道");
                                        num5 = 3;
                                        num4 = this.zddr_dev.error_noport[this.car_dev.sourcePort[carid - 1]];
                                    }
                                    else
                                    {
                                        num5 = 1;
                                        num4 = int.Parse(set2.Tables[0].Rows[0]["port_id"].ToString());
                                        str6 = set2.Tables[0].Rows[0]["person_id"].ToString();
                                        str7 = set2.Tables[0].Rows[0]["if_paijian"].ToString();
                                        str8 = set2.Tables[0].Rows[0]["sub_siteid"].ToString();
                                        DataBase.UpdateSiteCode(str4, this.site_code1, this.site_code2, str5, ref strErrorMsg);
                                        if (strErrorMsg.Length > 0)
                                        {
                                            this.AddErrToListView("条码" + str4 + ",补码" + str5 + "保存人工补码结果异常" + strErrorMsg);
                                            LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "条码" + str4 + ",补码" + str5 + "保存人工补码结果异常" + strErrorMsg);
                                        }
                                    }
                                }
                                this.car_dev.TaskPressHand(str4, carid, ref strErrorMsg, ref comandText, num4, num5, str6, str7, str8);
                                if (strErrorMsg.Length > 0)
                                {
                                    this.AddErrToListView(strErrorMsg);
                                    LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + strErrorMsg);
                                }
                                else if (comandText.Length > 0)
                                {
                                    comandText = "补码" + str4 + ",指令" + comandText;
                                    this.AddSuccessToListView(comandText);
                                    LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + comandText);
                                }
                            }
                            DataBase.delete_bill_hand(this.dbConn_hand, str4, ref strErrorMsg);
                            if (strErrorMsg.Length > 0)
                            {
                                this.AddErrToListView("删除人工补码临时记录" + str4 + "失败！" + strErrorMsg);
                                LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "删除人工补码临时记录" + str4 + "失败！" + strErrorMsg);
                            }
                        }
                    }
                    Thread.Sleep(300);
                    goto Label_06A1;
                }
                catch (Exception exception2)
                {
                    this.AddErrToListView("人工补码小车任务下发异常！" + exception2.Message);
                    LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "人工补码小车任务下发异常！" + exception2.Message);
                    goto Label_06A1;
                }
            }
            Thread.Sleep(200);
        Label_06A1:
            this.thrDownloadTaskHandIsRuning = false;
        Label_06A8:
            if (!this.closing)
            {
                goto Label_0098;
            }
        }

        public void downloadTask_auto_supply()
        {
           List<TaskConf> tasklisttmp = new List<TaskConf>();//   下载任务缓存队列
            while (!this.closing)
            {
                try
                {
                    if (GloabData.tasklist.Count() > 0)
                    {
                        //GloabData.gMutex1.WaitOne();
                        tasklisttmp.AddRange(GloabData.tasklist);
                        GloabData.tasklist.Clear();
                       // GloabData.gMutex1.ReleaseMutex();
                        foreach (TaskConf  tf in  tasklisttmp)
                        {
                            //if (this.plcSystemMS.is_runing)
                            
                                this.zddr_dev.downloadTask(tf);
                            
                            
                        }
                        tasklisttmp.Clear();
                    }
                    Thread.Sleep(50);
                    continue;
                }
                catch (Exception exception)
                {
                    this.AddErrToListView("1#自动导入异常!" + exception.Message);
                    LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + " 1#自动导入异常!" + exception.Message);
                    continue;
                }
            }
            this.thrSupplyGoodsServerIsRuning = false;
        }


        private void buttonQuery_Click(object sender, EventArgs e)
        {

            this.car_pos.BindToPLC(100);
            string str = this.textBoxBillCode.Text.Trim();
            if (str.Length == 0)
            {
                this.textBoxResult.Text = "";
            }
            else
            {
                DataSet set = new DataSet();
                set = MySqlHelper.ExecuteDataset(this.dbConn, "SELECT site_code1,site_code2,site_code3,SOURCE FROM tb_bill where bill_code='" + str + "'");
                if (set.Tables[0].Rows.Count == 0)
                {
                    this.textBoxResult.Text = "运单号【" + str + "】在WCS数据库中不存在！";
                }
                else
                {
                    string str2 = set.Tables[0].Rows[0]["site_code1"].ToString();
                    string str3 = set.Tables[0].Rows[0]["site_code2"].ToString();
                    string str4 = set.Tables[0].Rows[0]["site_code3"].ToString();
                    string str5 = set.Tables[0].Rows[0]["SOURCE"].ToString();
                    string commandText = "SELECT c.port_id,c.SiteNAME FROM td_port c where c.pipeline='" + this.deviceID + "' and c.siteid='" + str4 + "' order by CAST(c.port_id AS SIGNED)";
                    if (str5 == "1")
                    {
                        str5 = "自动分拣";
                    }
                    else if (str5 == "2")
                    {
                        str5 = "人工补码";
                    }
                    else
                    {
                        str5 = "供包补码";
                    }
                    DataSet set2 = new DataSet();
                    set2 = MySqlHelper.ExecuteDataset(this.dbConn, commandText);
                    if (set2.Tables[0].Rows.Count == 0)
                    {
                        this.textBoxResult.Text = "运单号" + str + ",站点【" + str2 + "-" + str3 + "-" + str4 + "】,未配置格口！来源：" + str5 + "。";
                    }
                    else
                    {
                        string str7 = string.Empty;
                        string str8 = string.Empty;
                        int num = 0;
                        foreach (DataRow row in set2.Tables[0].Rows)
                        {
                            if (num == 0)
                            {
                                str7 = row["port_id"].ToString();
                                str8 = row["SiteNAME"].ToString();
                            }
                            else
                            {
                                str7 = str7 + "、" + row["port_id"].ToString();
                            }
                            num++;
                        }
                        this.textBoxResult.Text = "运单号" + str + ",站点:" + str8 + "【" + str2 + "-" + str3 + "-" + str4 + "】,格口" + str7 + ",来源：" + str5 + "。";
                    }
                }
            }
        }

        private void listViewTask_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listViewCommand_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel_car_Paint(object sender, PaintEventArgs e)
        {

        }

        public void AddErrToListView(string content)
        {
            this.AddToListView(content, MsgType.Error, this.listViewInfo);
        }

        public void AddSuccessToListView(string content)
        {
            this.AddToListView(content, MsgType.Infomation, this.listViewInfo);
        }

        public void AddToListView(string content, MsgType type, ListView lvInfo)
        {
            if (lvInfo.InvokeRequired)
            {
                AddContentToListViewCallback method = new AddContentToListViewCallback(this.AddToListView);
                base.Invoke(method, new object[] { content, type, lvInfo });
            }
            else
            {
                if (lvInfo.Items.Count >= 0x7d0)
                {
                    lvInfo.Items.Clear();
                }
                ListViewItem item = new ListViewItem
                {
                    Text = DateTime.Now.ToString()
                };
                if (type == MsgType.Error)
                {
                    item.ForeColor = Color.Red;
                }
                else
                {
                    item.ForeColor = Color.Black;
                }
                item.SubItems.Add(content);
                lvInfo.Items.Insert(0, item);
            }
        }

        private bool InitialDev()
        {
            // 加载配置文件参数
            DataSet set = new DataSet();
            XmlDocument document = new XmlDocument();
            document.Load(this.filename);
            XmlNode node = document.DocumentElement.SelectSingleNode("/configuration/parameter/error_noport1");
            this.error_noport1 = int.Parse(node.InnerText);
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/error_nodata1");
            this.error_nodata1 = int.Parse(node.InnerText);
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/error_noport2");
            this.error_noport2 = int.Parse(node.InnerText);
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/error_nodata2");
            this.error_nodata2 = int.Parse(node.InnerText);
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/error_port");
            this.error_port = int.Parse(node.InnerText);
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/if_handcode");
            this.if_handcode = node.InnerText;
            node = document.DocumentElement.SelectSingleNode("/configuration/parameter/maxTurnNumer");
            this.maxTurnNumer = double.Parse(node.InnerText);
            if ((this.maxTurnNumer < 0.0) || (this.maxTurnNumer > 10.0))
            {
                this.maxTurnNumer = 1.5;
            }
            return true;
            //node = document.DocumentElement.SelectSingleNode("/configuration/url/zt_url");
            //this.zt_url = node.InnerText;
            //try
            //{
            //    new MySqlCommand("update tb_sort_mode set device_id='" + this.deviceID + "',if_handcode='" + this.if_handcode + "'", this.dbConn).ExecuteNonQuery();
            //    this.AddSuccessToListView("更新当前分拣线编号成功");
            //    LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "更新当前分拣线编号成功");
            //}
            //catch (Exception exception)
            //{
            //    this.AddErrToListView("更新当前分拣线编号异常" + exception.Message);
            //    LogHelper.LogSimlpleString(DateTime.Now.ToString("G") + "更新当前分拣线编号异常" + exception.Message);
            //    return false;
            //}
            //set = MySqlHelper.ExecuteDataset(this.dbConn, "select * from td_device_dic t where t.command_type='XC' order by device_id");
            //this.car_pos = new Car_Pos(this, set.Tables[0]);
            //for (int i = 0; i < set.Tables[0].Rows.Count; i++)
            //{
            //    this.panel_car.Controls.Add(this.car_pos.pic[i]);
            //    this.car_pos.pic[i].BringToFront();
            //}
            //this.car_dev = new Car_Dev(this, set.Tables[0]);
            set = MySqlHelper.ExecuteDataset(this.dbConn, "select * from td_device_dic t where t.command_type='KZAN' order by device_id");
            this.kzan_dev = new KZAN_Dev(this, set.Tables[0]);
            for (int j = 0; j < set.Tables[0].Rows.Count; j++)
            {
                switch (this.kzan_dev.devicePos[j])
                {
                    case "0":
                        base.Controls.Add(this.kzan_dev.pic[j]);
                        break;

                    case "1":
                        this.tabPage1.Controls.Add(this.kzan_dev.pic[j]);
                        break;

                    default:
                        base.Controls.Add(this.kzan_dev.pic[j]);
                        break;
                }
                this.kzan_dev.pic[j].BringToFront();
            }
            set = MySqlHelper.ExecuteDataset(this.dbConn, "select * from td_device_dic t where t.command_type='' order by device_id");
            //this.ssj_dev = new SSJ_Dev(this, set.Tables[0]);
            for (int k = 0; k < set.Tables[0].Rows.Count; k++)
            {
                this.panel_car.Controls.Add(this.ssj_dev.pic[k]);
                this.ssj_dev.pic[k].BringToFront();
            }
            set = MySqlHelper.ExecuteDataset(this.dbConn, "select * from td_device_dic t where t.command_type='ZDDR' order by device_id");
            this.zddr_dev = new ZDDR_Dev(this, set.Tables[0]);
            set = MySqlHelper.ExecuteDataset(this.dbConn, "select concat(Control_type,Control_id) 'key',Control_type,Control_id,Control_desc,t.condition from td_device_control_dic t order by Control_type,control_id");
            foreach (DataRow row in set.Tables[0].Rows)
            {
                DeviceBase.DeviceControl control;
                control.controlType = row["Control_type"].ToString();
                control.controlId = row["Control_id"].ToString();
                control.controlDesc = row["Control_desc"].ToString();
                control.condition = row["condition"].ToString();
                this.deviceControlDic.Add(row["key"].ToString(), control);
            }
            set.Dispose();
            return true;
        }

        public bool if_right_sort_code(string ls_sort_code)
        {
            if (ls_sort_code.Length != 0)
            {
                try
                {
                    foreach (sortcode_outport _outport in this.sortcode_outport_list)
                    {
                        if (ls_sort_code == _outport.destSortingCode)
                        {
                            return true;
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.AddErrToListView("判断分拣编码" + ls_sort_code + "是否存在异常" + exception.Message);
                    LogHelper.LogSimlpleString(DateTime.Now.ToString() + "判断分拣编码" + ls_sort_code + "是否存在异常" + exception.Message);
                    return false;
                }
            }
            return false;
        }

        private delegate void AddContentToListViewCallback(string content, Form1.MsgType type, ListView lvInfo);

        public enum MsgType
        {
            Error,
            Infomation
        }

        private void toolStripButton2_Click(object sender, EventArgs e)//分拣计划（测试使用）
        {
            new ManualPlan(this).ShowDialog();
        
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton8_Click(object sender, EventArgs e)//启动系统
        {
            //toolStripButton8.Image = Image.FromFile(@"C:\Users\Administrator\Desktop\RD88界面设计\RD88\ICON选中\ICON选中-07.png");
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            base.Close();
            Application.Exit();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)//统计查询
        {

        }

        private void toolStripButton10_Click(object sender, EventArgs e)//日志查询
        {

        }

        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
            this.Close();
            base.Close();
            Application.Exit();
        }

        private void rb_init_MouseHover(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.Image = Resources.connect_02;
        }

        private void rb_start_MouseHover(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.Image = Resources.start_02;
        }

        private void rb_start_MouseLeave(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.Image = Resources.start_01;
        }

        private void rb_stop_MouseHover(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.Image = Resources.stop_02;
        }

        private void rb_stop_MouseLeave(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.Image = Resources.stop_01;
        }
        private void rb_init_Click(object sender, EventArgs e)
        {
            // 连接数据库
            if (!this.dbIsConned)
            {
                this.dbIsConned = this.DBIsConnect();
            }
            if (!this.dbIsConned)
            {
                MessageBox.Show("连接数据库失败，请检查本机至数据库的网络连接、数据库是否启动等因素！", "系统提示");
                this.if_must_close = true;
            }
            else
            {
                this.lblDBConStatus.Text = "数据库已连接！";
            }
            // 连接后台服务
            if (!this.socketIsConned)
            {
                this.socketIsConned = this.ServerIsConnect();
            }
            if (!this.socketIsConned)
            {
                MessageBox.Show("连接后台服务器失败，请检查本机至服务器的网络连接、数据库是否启动等因素！", "系统提示");
                this.if_must_close = true;
            }
            //连接PLC
            if (!this.plcIsOK)
            {
                this.plcIsOK = this.PlcIsConnect();
            }
            else
            {
                MessageBox.Show("连接PLC失败，请检查本机至PLC的网络连接！", "系统提示");
            }
            //按钮表现状态
            this.setMenu();
        }

        private void rb_init_MouseLeave(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.Image = Resources.connect_01;
        }

        private void rb_start_Click(object sender, EventArgs e)
        {

            this.car_pos.CtrPlcStatus(1);
        }

        private void rb_stop_Click(object sender, EventArgs e)
        {
            this.car_pos.CtrPlcStatus(1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
           

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
