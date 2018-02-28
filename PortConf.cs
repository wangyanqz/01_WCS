namespace WCS
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PortConf
    {
        public string id;
        public string pipeline;
        public string sortPortCode;
        public string siteCode;
        public string siteName;
        public string sortMode;
        public string sortCode;
    }

    public struct TaskConf
    {
        public string barCode;
        public string weight;
        public string deviceIdQh;
        public string carId;
        public string oneCode;
        public string threeCode;
    }
    public struct DownTaskConf
    {
        public string barCode;
        public string carId;
        public string aimport;
        public string status;
    }

}
