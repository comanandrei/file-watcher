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
            // Create a timer with a ten second interval.
            aTimer = new System.Timers.Timer(10000);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Set the Interval to 2 seconds (2000 milliseconds).
            aTimer.Interval = 2000;
            aTimer.Enabled = true;




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
