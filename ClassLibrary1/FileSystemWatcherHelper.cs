using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cnBlogs.Core
{
    public class FileSystemWatcherHelper
    {
        private static FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

        private static List<EditFileInfo> EditFileList = new List<EditFileInfo>();

        public FileSystemWatcherHelper()
        {
            fileSystemWatcher.Path = AppDomain.CurrentDomain.BaseDirectory;
            fileSystemWatcher.Filter = "*.*";
            fileSystemWatcher.NotifyFilter = NotifyFilters.FileName| NotifyFilters.DirectoryName
                |NotifyFilters.CreationTime|NotifyFilters.LastAccess|NotifyFilters.LastWrite|NotifyFilters.Size;
            fileSystemWatcher.IncludeSubdirectories = true;
            InitEvents();
        }

        public FileSystemWatcherHelper(string path,string filter, NotifyFilters notifyFilter,bool incluedSubDirectory = true)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            fileSystemWatcher.Path = path;
            fileSystemWatcher.Filter = filter;
            fileSystemWatcher.NotifyFilter = notifyFilter;
            fileSystemWatcher.IncludeSubdirectories = incluedSubDirectory;
            InitEvents();
        }
        private void InitEvents()
        {
            fileSystemWatcher.Changed += OnChanged;
            fileSystemWatcher.Created += OnCreated;
            fileSystemWatcher.EnableRaisingEvents = true;
        }
        public List<EditFileInfo> GetChangedFileInfo()
        {
            return EditFileList;
        }

        private void OnChanged(object sender,FileSystemEventArgs e)
        {
            try
            {
                var changeFile = new EditFileInfo
                {
                    FileName = e.Name,
                    FullPath = e.FullPath,
                    LastWriteTime = DateTime.Now,
                };
                EditFileList.Add(changeFile);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                var createFile = new EditFileInfo
                {
                    FileName = e.Name,
                    FullPath = e.FullPath,
                    CreateTime = DateTime.Now
                };
                EditFileList.Add(createFile);
            }
            catch(Exception ce)
            {
                Trace.WriteLine(ce.Message);
            }
        }
    }
    public class EditFileInfo
    {
        public string FileName { get; set; }

        public string FullPath { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastWriteTime { get; set; }
    }
}
