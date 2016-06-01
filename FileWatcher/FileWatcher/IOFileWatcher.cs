using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    public class IOFileWatcher
    {
        List<string> _directories;
        List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();

        public delegate void OnChange(object sender, FileSystemEventArgs args);

        private OnChange OnChangeEnt;

        public IOFileWatcher(List<string> dirsToWatch, string filter, OnChange onChange)
        {
            OnChangeEnt = onChange;

            foreach (var dir in dirsToWatch)
            {
                var watcher = new FileSystemWatcher(dir);
                watcher.Filter = filter;
                watcher.Changed += new FileSystemEventHandler(onChange);
                watcher.Created += new FileSystemEventHandler(onChange);
                watcher.Renamed += new RenamedEventHandler(OnChangeHandler);
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.FileName;
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
                _watchers.Add(watcher);
            }
        }

        private void OnChangeHandler(object sender, FileSystemEventArgs args)
        {
            this.OnChangeEnt(sender, args);
        }
    }
}
