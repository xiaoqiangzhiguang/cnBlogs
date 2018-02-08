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
        private FileSystemWatcher fileSystemWatcher;

        public FileSystemWatcherHelper(string path)
        {
            fileSystemWatcher = new FileSystemWatcher(path);
        }
        public FileSystemWatcherHelper()
        {
        }

        public Assembly ReturnAssemblyInfo()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
