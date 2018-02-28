using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WCS
{
    class GloabData
    {
        public static Mutex gMutex1 = new Mutex(true, "gMutex1");
        public static List<TaskConf> tasklist = new List<TaskConf>();//   下载任务缓存队列
        //public static List<DownTaskConf> downtasklist = new List<DownTaskConf>();//   下载任务缓存队列
        public static  ConcurrentQueue<DownTaskConf> downtasklist = new ConcurrentQueue<DownTaskConf>();
    }
}
