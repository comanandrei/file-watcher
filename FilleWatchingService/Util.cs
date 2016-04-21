using System;
using System.IO;

namespace FilleWatchingService
{
    class Util
    {
        //Remove the dot(.) from the extension
        internal static string GetSimpleExtension(string fileExtension)
        {
            return fileExtension.Replace(".", "");
        }

        //Get trasition folder (location) from configuration
        internal static string GetTransitionPath()
        {
            string value = String.Empty;

            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["moveLocation"];
            }

            catch (Exception)
            {
                //Implement logging on future version.
            }


            return value;

        }

        //Get folder location (scaned) from configuration
        public static string GetSharedPath()
        {
            string scanFolderPath = String.Empty;

            try
            {
                scanFolderPath = System.Configuration.ConfigurationManager.AppSettings["location"];
            }

            catch (Exception)
            {
                throw;
            }

            return scanFolderPath;

        }

        //Write message in the log file
        public static void Log(string message)
        {
            try
            {
                string _message = String.Format("{0} {1}", message, Environment.NewLine);
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logFile.txt", _message);
            }
            catch (Exception)
            {
                throw;

            }
        }

    }
}
