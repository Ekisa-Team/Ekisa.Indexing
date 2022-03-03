using Ekisa.Indexing.Watcher.Constants;
using Ekisa.Indexing.Watcher.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

                List<string> supportedTriggerEvents = new() { "CREATE" };
                if (!supportedTriggerEvents.Contains(@event.Kind))
                {
                    throw new Exception($"The provided trigger event is not yet supported. Supported trigger events: 'CREATE'");
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

                List<string> availableHttpMethods = new() { "POST", "PUT", "PATCH", "DELETE" };
                if (!availableHttpMethods.Contains(@event.WebhookHttpMethod ?? ""))
                {
                    throw new Exception("The provided HTTP method is invalid.");
                }
            }
        }
        #endregion
    }
}
