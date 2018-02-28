namespace WCS
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ZddrInf
    {
        public string barcode;
        public double barcode_weight;
        public int item_type;        //  任务类型 0 正常任务  1 异常任务
        public string site_code;     //  目的地信息
        public string[] ChuteId;   //  备选隔口信息
    }
}

