using FilleWatchingService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatchingService
{
    public class FileWatcher
    {
        private FileSystemWatcher _fileWatcher;
        private string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


        public FileWatcher()
        {
            _fileWatcher = new FileSystemWatcher(PathLocation());

            _fileWatcher.Created += new FileSystemEventHandler(_fileWatcher_Created);
            _fileWatcher.Deleted += new FileSystemEventHandler(_fileWatcher_Deleted);
            _fileWatcher.Changed += new FileSystemEventHandler(_fileWatcher_Changed);

            _fileWatcher.EnableRaisingEvents = true;
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

        private void _fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Logger.Log(String.Format("File Changed: Path:{0} , Name:{1}", e.FullPath, e.Name));
        }

        private void _fileWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Logger.Log(String.Format("File Deleted: Path:{0} , Name:{1}", e.FullPath, e.Name));
        }

        void _fileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Logger.Log(String.Format("File Created: Path:{0} , Name:{1}", e.FullPath, e.Name));
        }
    }
}
