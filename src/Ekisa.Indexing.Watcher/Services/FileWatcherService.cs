using Ekisa.Indexing.Watcher.Enums;
using System.Reactive.Linq;

namespace Ekisa.Indexing.Watcher.Utils
{
    public delegate void Notify(string fullPath, TriggerEventKind triggerEventKind);

    public class FileWatcherService
    {
        public event Notify? DirectoryChanged;

        public FileSystemWatcher? watcher;

        private readonly string _path;

        public FileWatcherService(string path)
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
                .Subscribe(e => DirectoryChanged?.Invoke(e.EventArgs.FullPath, TriggerEventKind.Create));
        }      
    }
}
