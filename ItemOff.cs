namespace WCS.DevSystem
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ItemOff
    {
        public string Off_barcode;
        public int Off_outport;
        public string Off_person_id;
        public string Off_sub_siteid;
    }
}


