using Ekisa.Indexing.Watcher.Utils;
using Syroot.Windows.IO;

namespace Ekisa.Indexing.Watcher.Core
{
    public class Orchestrator
    {
        private readonly string? _additionalPath;


        public Orchestrator()
        {
        }
        
        public Orchestrator(string additionalPath)
        {
            _additionalPath = additionalPath;
        }

        public void Start()
        {
            string listenPath = Path.Combine(KnownFolders.Downloads.Path, string.IsNullOrWhiteSpace(_additionalPath) ? "" : $"{_additionalPath}");
            FileWatcher fw = new(listenPath);
            fw.DirectoryChanged += HandleDirectoryChanged;
            fw.Watch();
        }

        private void HandleDirectoryChanged(string fullPath)
        {
            Console.WriteLine("From process " + fullPath);
        }
    }
}
