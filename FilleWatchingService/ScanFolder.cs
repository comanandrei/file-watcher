using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FilleWatchingService
{
    class ScanFolder
    {
        private static System.Timers.Timer aTimer;

        public ScanFolder()
        {
            
            aTimer = new System.Timers.Timer(10000);
                       
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            aTimer.Interval = 2000;
            aTimer.Enabled = true;

            Logger.Log("The Elapsed event was raised at 2222");
                    
        }

              private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        }
    }
}
