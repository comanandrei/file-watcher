using FilleWatchingService;
using System;
using System.IO;


namespace FileWatchingService
{
    public class FileWatcher
    {
        private FileSystemWatcher fileWatcher;
        private string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;


        public FileWatcher()
        {
            fileWatcher = new FileSystemWatcher(Util.GetSharedPath());

            fileWatcher.Created += new FileSystemEventHandler(fileWatcher_Created);
            fileWatcher.Deleted += new FileSystemEventHandler(fileWatcher_Deleted);
            fileWatcher.Changed += new FileSystemEventHandler(fileWatcher_Changed);

            fileWatcher.EnableRaisingEvents = true;
        }


        private void fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Util.Log(String.Format("File Changed: Path:{0} , Name:{1}; time: {2}", e.FullPath, e.Name, DateTime.Now.ToString()));
        }

        private void fileWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Util.Log(String.Format("File Deleted: Path:{0} , Name:{1}; time: {2}", e.FullPath, e.Name, DateTime.Now.ToString()));
        }

        private void fileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Util.Log(String.Format("File Created: Path:{0} , Name:{1}; time: {2}", e.FullPath, e.Name, DateTime.Now.ToString()));
        }
    }
}
