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

        private static System.Timers.Timer aTimer;

        protected override void OnStart(string[] args)
        {
            FileWatcher f = new FileWatcher();
            TestTimer();
        }

        private void TestTimer()
        {
         
            aTimer = new System.Timers.Timer();

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Set the Interval to 2 seconds (2000 milliseconds).
            aTimer.Interval = 20000;
            aTimer.Enabled = true;

            var pathLocation = PathLocation();

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(pathLocation);

            foreach (string fileName in fileEntries)
            {
                var user = System.IO.File.GetAccessControl(fileName).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString(); 
                Console.WriteLine(user); // DOMAIN\username
            }


        }

        private string PathLocation()
        {
            string value = String.Empty;

            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["location"];
            }

            catch (Exception)
            {
                //Implement logging on future version.
            }


            return value;

        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            string message = String.Format("yeeeeeeeeee {0}", DateTime.Now.ToString());
            Logger.Log(message);
        }

        protected override void OnStop()
        {
            Logger.Log("Stop");
        }
    }
}
