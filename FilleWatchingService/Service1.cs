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

            var scanFolderPath = PathLocation();
            
            // Get the list of files found in the directory.
            string[] scanFolderFiles= Directory.GetFiles(scanFolderPath);
            foreach (string filePath in scanFolderFiles)
            {
                var movingPath = prepareFileToMove(filePath);
              
                try
                {
                    //To move a file or folder to a new location:
                    System.IO.File.Move(filePath, movingPath);
                }
                catch (Exception)
                {

                    throw new FileNotFoundException("The file couldn't been moved!");
                }


            }


        }

        //Check if the file must be movedand  Create the new location of the file.
        private string prepareFileToMove(string filePath)
        {
            var entityFile = new FileInfo(filePath);

            //string name = entityFile.Name;

            string extensionFile = Util.GetSimpleExtension(entityFile.Extension);

            string transitionPath = PathMoveLocation();
            //Transition location found
            if (!String.IsNullOrEmpty(transitionPath))
            {
                transitionPath = transitionPath + Path.DirectorySeparatorChar + extensionFile;

                //The directory doesn't exists
                if (!Directory.Exists(transitionPath))
                {
                    // Try to create the directory.
                    try
                    {
                        Directory.CreateDirectory(transitionPath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The directory couldn't been created : {0}", e.ToString());
                    }
                }

                return transitionPath + Path.DirectorySeparatorChar + setFileName(entityFile, transitionPath) + entityFile.Extension;
            } else
            {
                //todo:  test if is works
                throw new ApplicationException("The transition path is not found.");
            }

        }

        //Verify if scaned file has a doppelganger in transition
        //use the md5 hashing to compare the files content 
        private Boolean filesCompare(string filepath1, string filepath2)
        {
            using (var reader1 = new System.IO.FileStream(filepath1, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var reader2 = new System.IO.FileStream(filepath2, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    byte[] hash1;
                    byte[] hash2;

                    using (var md51 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                    {
                        md51.ComputeHash(reader1);
                        hash1 = md51.Hash;
                    }

                    using (var md52 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                    {
                        md52.ComputeHash(reader2);
                        hash2 = md52.Hash;
                    }

                    int j = 0;
                    for (j = 0; j < hash1.Length; j++)
                    {
                        if (hash1[j] != hash2[j])
                        {
                            break;
                        }
                    }

                    //if both hashed contents are identical
                    if (j == hash1.Length)
                    {
                        return true;
                    }

                    return false;
                }
            }
        } 

        //Verify if is another file in the transition folder with the same name
        private string setFileName(FileInfo fileEntity, string newFullPath)
        {
            int count = 1;

            string tempFileName = fileEntity.Name;

            newFullPath = newFullPath + "\\" + tempFileName;

            while (File.Exists(newFullPath))
            {
                Boolean isClone = filesCompare(fileEntity.Directory.FullName, newFullPath);
                tempFileName = string.Format("{0}({1})", tempFileName, count++);
                newFullPath = Path.Combine(newFullPath, tempFileName + fileEntity.Extension);
            }

            return tempFileName;
        }

        //Get trasition folder (location) from configuration
        private string PathMoveLocation()
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
        private string PathLocation()
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
