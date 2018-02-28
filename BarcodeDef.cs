namespace WCS.DevSystem
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using WCS;

    public class BarcodeDef
    {
        public BillCodeDef billCodeDef;
        private Form1 mainFrm;
        private Regex r1;

        public BarcodeDef(Form1 mainFrm)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.mainFrm = mainFrm;
            this.InitBillCodeDef();
        }

        public string GetRightBillCode(string barcodes)
        {
            string str = "rightcode";
            int num = 0;
            if ((barcodes.Length == 0) || (barcodes == "NOREAD"))
            {
                return "NOREAD";
            }
            if (this.r1 == null)
            {
                return barcodes;
            }
            try
            {
                foreach (string str2 in barcodes.Split(new char[] { ',' }))
                {
                    if (((str2.Length >= 13) && this.r1.IsMatch(str2.Substring(0, 13))) && (str2.Substring(0, 13) != str))
                    {
                        str = str2.Substring(0, 13);
                        num++;
                    }
                    if (num >= 2)
                    {
                        return "NOREAD";
                    }
                }
                if (num == 1)
                {
                    return str;
                }
                return "NOREAD";
            }
            catch (Exception exception)
            {
                this.mainFrm.AddErrToListView("验证运单条码" + barcodes + "的规则失败!" + exception.Message);
                LogHelper.LogSimlpleString(DateTime.Now.ToString() + "验证运单条码" + barcodes + "的规则失败!" + exception.Message);
                return "NOREAD";
            }
        }

        private void InitBillCodeDef()
        {
            this.r1 = new Regex("^[0-9]{13}$");
        }
    }
}

