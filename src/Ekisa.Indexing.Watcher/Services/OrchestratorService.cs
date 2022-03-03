using Ekisa.Indexing.Watcher.Models;
using Ekisa.Indexing.Watcher.Services;
using Ekisa.Indexing.Watcher.Utils;
using Syroot.Windows.IO;

namespace Ekisa.Indexing.Watcher.Core
{
    public class OrchestratorService
    {
        private readonly ConfigModel _config;
        private readonly ConfigService _configService;
        
        public OrchestratorService(ConfigModel config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _configService = new ConfigService();
        }

        public void Start()
        {
            _configService.Locations.TryGetValue(_config.Folder!, out string? folderFullPath);
            string listeningPath = folderFullPath!;

            if (_config.PathSuffix != null)
            {
                listeningPath = Path.Combine(listeningPath, string.Join(@"\", _config.PathSuffix.Select(p => p)));
            }

            FileWatcherService fw = new(listeningPath);
            fw.DirectoryChanged += HandleDirectoryChanged;
            fw.Watch();
            Console.WriteLine($"Listening for file changes on {listeningPath}");
        }

        private void HandleDirectoryChanged(string fullPath)
        {
            Console.WriteLine("From process " + fullPath);
        }
    }
}
