namespace WCS
{
    using System;

    public class PortConfReturn
    {
        public string message;
        public PortConf[] result;
        public bool status;
        public string statusCode;
    }

    public class TaskInfo
    {
        public string resultCode;
        public string message;
        public TaskConf[] result;
    }

}


