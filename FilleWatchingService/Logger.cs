using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FilleWatchingService
{
    public static class Logger
    {
        public static void Log(string message)
        {
            try
            {
                string _message = String.Format("{0} {1}", message, Environment.NewLine);
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logFile.txt", _message);
            }
            catch (Exception)
            {
                //Implement logging on next version.

            }
        }
    }
}
