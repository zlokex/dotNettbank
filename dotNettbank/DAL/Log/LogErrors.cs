using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace DAL.Log
{
    public class LogErrors
    {
        private string pathName;
        private string dateTime;
        private string format;

        public LogErrors()
        {
            format = DateTime.Now.ToShortDateString().ToString() +
                " " + DateTime.Now.ToLongTimeString().ToString() + " ---> ";
            DateTime now = DateTime.Now;
            dateTime =
                now.Year.ToString() + "_" +
                now.Month.ToString() + "_" +
                now.Day.ToString() + "_" +
                now.Hour.ToString();
            pathName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\ErrorLogs");
        }

        public void errorLog(string errorMsg)
        {
            try
            {
                if (!Directory.Exists(pathName))
                    Directory.CreateDirectory(pathName);
                using (StreamWriter sw = new StreamWriter(Path.Combine(pathName, dateTime + ".log.txt"), true))
                {
                    sw.WriteLine(format + errorMsg);
                }
            }
            catch (Exception e)
            {
                Debug.Write("Failed to write log. Error: " + e.StackTrace.ToString());
            }
        }
    }
}
