namespace WCS
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct sortcode_outport
    {
        public string destSortingCode;
        public string sortPortCode;
    }
}
