using Ekisa.Indexing.Watcher.Constants;
using Ekisa.Indexing.Watcher.Enums;
using Ekisa.Indexing.Watcher.Models;
using Ekisa.Indexing.Watcher.Utils;
using Newtonsoft.Json;

namespace Ekisa.Indexing.Watcher.Services
{
    public class ConfigService 
    {
        #region Private Attributes
        private readonly Dictionary<string, string> _locations = new()
        {
            { "Desktop", FoldersConstants.DESKTOP_PATH },
            { "Downloads", FoldersConstants.DOWNLOADS_PATH },
            { "Documents", FoldersConstants.DOCUMENTS_PATH },
            { "Music", FoldersConstants.MUSIC_PATH },
            { "Pictures", FoldersConstants.PICTURES_PATH },
            { "Videos", FoldersConstants.VIDEOS_PATH }
        };
        #endregion

        #region Getters & Setters 
        public Dictionary<string, string> Locations { 
            get { return _locations; } 
        }
        #endregion

        #region Constructor
        public ConfigService()
        {
        }
        #endregion

        #region Public Methods
        public async Task<Config?> ReadConfigFile(string configFilePath)
        {
            Config? config;

            try
            {
                config = JsonConvert.DeserializeObject<Config>(await File.ReadAllTextAsync(configFilePath));
                CheckConfigConstraints(config!);
            }
            catch 
            {
                throw;
            }

            return config;
        }
        #endregion

        #region Private Methods
        private void CheckConfigConstraints(Config config)
        {
            // Validates if provided folder is supported
            if (config.Folder == null)
            {
                throw new Exception("Folder argument must be provided.");
            }

            if (!_locations.ContainsKey(config.Folder ?? ""))
            {
                throw new Exception("The provided folder is not yet supported.");
            }

            // Validates trigger events
            if (config.TriggerEvents == null)
            {
                throw new Exception("Trigger events argument must be provided.");
            }

            foreach (ConfigTriggerEvent @event in config.TriggerEvents)
            {
                // Validates event kind
                if (@event.Kind == null)
                {
                    throw new Exception("Kind argument must be provided.");
                }

                var supportedTriggerEvents = EnumUtils.EnumNamedValues<TriggerEventKind>();
                if (!supportedTriggerEvents.Contains(@event.Kind))
                {
                    throw new Exception($"The provided trigger event is not yet supported. Supported trigger events: 'Create'");
                }

                // Validates webhook URL
                if (@event.WebhookUrl == null)
                {
                    throw new Exception("Webhook URL argument must be provided.");
                }

                // Validates if provided HTTP method is supported
                if (@event.WebhookHttpMethod == null)
                {
                    throw new Exception("HTTP method argument must be provided.");
                }

                List<string> supportedHttpMethods = EnumUtils.EnumNamedValues<WebhookHttpMethod>();
                if (!supportedHttpMethods.Contains(@event.WebhookHttpMethod ?? ""))
                {
                    throw new Exception("The provided HTTP method is not yet supported. Supported HTTP methods: 'Get', 'Post', 'Put', 'Patch' and 'Delete'");
                }
            }
        }
        #endregion
    }
}
