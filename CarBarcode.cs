namespace WCS
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct CarBarcode
    {
        public int carid;
        public string barcode;
        public DateTime scan_time;
    }
}
