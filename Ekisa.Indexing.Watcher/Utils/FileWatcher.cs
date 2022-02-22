using System.Reactive.Linq;

namespace Ekisa.Indexing.Watcher.Utils
{
    public delegate void Notify(string fullPath);

    public class FileWatcher
    {
        public event Notify? DirectoryChanged;

        public FileSystemWatcher? watcher;

        private readonly string _path;

        public FileWatcher(string path)
        {
            _path = path;            
        }

        public void Watch()
        {
            watcher = new()
            {
                Path = _path,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*",
                EnableRaisingEvents = true
            };

            Observable.FromEventPattern<FileSystemEventArgs>(watcher, "Changed")
                .Distinct(e => e.EventArgs.FullPath)
                .Subscribe(e => DirectoryChanged?.Invoke(e.EventArgs.FullPath));
        }      
    }
}
