using FilleWatchingService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Security.Principal;

namespace FileWatchingService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            new FileWatcher();
            CronProcess();
        }

        private void CronProcess()
        {

            System.Timers.Timer aTimer = new System.Timers.Timer();

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += OnTimedEvent;

            // Set the Interval to 20 seconds (20000 milliseconds).
            aTimer.Interval = 20000;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
          
        }

        protected override void OnStop()
        {
            Util.Log("Stop");
        }

        // Cron event
        //Scan the "share folder"
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            var scanFolderPath = Util.GetSharedPath();
            string message;
            // Get the list of files found in the directory.
            string[] scanFolderFiles = Directory.GetFiles(scanFolderPath);
            if((scanFolderFiles != null))
            {
                foreach (string filePath in scanFolderFiles)
                {
                    new ScanFile(filePath);
                }

                message = String.Format("The folder was scaned at {0}", DateTime.Now.ToString());
            } else
            {
                message = String.Format("The folder was scaned at {0}. The folder was empty.", DateTime.Now.ToString());
            }
            
            Util.Log(message);
        }

       
    }
}
