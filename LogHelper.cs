namespace WCS.DevSystem
{
    using log4net;
    using log4net.Config;
    using System;
    using System.Collections;
    using System.Diagnostics;

    public class LogHelper
    {
        public string filePath = string.Empty;
        public static ILog log;
        public string loggerName = string.Empty;

        static LogHelper()
        {
            if (log == null)
            {
                if (log == null)
                {
                    //XmlConfigurator.Configure();
                    log = LogManager.GetLogger("loginfo");
                }
            }
        }

        public static void AddQueueLog(Queue que)
        {
            try
            {
                int count = 0;
                count = que.Count;
                if (count == 0)
                {
                    return;
                }
                log.Debug("<TrancationItem Time=\"" + DateTime.Now.ToString() + "\">" + Environment.NewLine);
                for (int i = 0; i < count; i++)
                {
                    log.Debug("<ActionItem SEQ=\"" + i.ToString() + "\"><![CDATA[");
                    log.Debug(que.Dequeue().ToString());
                    log.Debug("]]></ActionItem>" + Environment.NewLine);
                }
                log.Debug("</TrancationItem>" + Environment.NewLine);
            }
            catch (Exception exception)
            {
                //ApplicationLog.WriteError(exception);
            }
            que.Clear();
        }

        public static void LogException(Exception ex)
        {
            string message = "<ExceptionMessage Time=\"" + DateTime.Now.ToString() + "\"><![CDATA[" + ex.Message + "]]></ExceptionMessage>" + Environment.NewLine + "<ExceptionStatck Time=\"" + DateTime.Now.ToString() + "\"><![CDATA[" + ex.StackTrace + "]]></ExceptionStatck>" + Environment.NewLine;
            log.Debug(message);
        }

        public static void LogSimlpleString(string str)
        {
            try
            {
                log.Debug(str + Environment.NewLine);
            }
            catch (Exception exception)
            {
                EventLogEntryType error = EventLogEntryType.Error;
                new EventLog("Application") { Source = "GBIA-航显" }.WriteEntry(exception.Message, error);
            }
        }
    }
}


