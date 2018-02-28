using System;
using System.Windows.Forms;
using System.Threading;

namespace WCS
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool flag;
            Control.CheckForIllegalCrossThreadCalls = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Mutex mutex = new Mutex(true, "WCS", out flag);
            if (flag)
            {
                Application.Run(new Form1());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("已经在本机上启动了一个WCS系统！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
            }
        }
    }
}
