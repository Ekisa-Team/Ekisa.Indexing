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
        public async Task<ConfigModel?> ReadConfigFile(string configFilePath)
        {
            ConfigModel? config;

            try
            {
                config = JsonConvert.DeserializeObject<ConfigModel>(await File.ReadAllTextAsync(configFilePath));
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
        private void CheckConfigConstraints(ConfigModel config)
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

            // Validates webhook URL
            if (config.WebhookUrl == null)
            {
                throw new Exception("Webhook URL argument must be provided.");
            }

            // Validates if provided HTTP method is supported
            if (config.WebhookHttpMethod == null)
            {
                throw new Exception("HTTP method argument must be provided.");
            }

            List<string> availableHttpMethods = new() { "POST", "PUT", "PATCH", "DELETE" };
            if (!availableHttpMethods.Contains(config.WebhookHttpMethod ?? ""))
            {
                throw new Exception("The provided HTTP method is invalid.");
            }

            // Validates if provided trigger event is supported
            if (config.TriggerEvents == null)
            {
                throw new Exception("Trigger events argument must be provided.");
            }

            List<string> availableTriggerEvents = new() { "add", "update", "delete" };
            if (config.TriggerEvents.ToList().Any(e => !availableTriggerEvents.Contains(e)))
            {
                throw new Exception($"The provided trigger event is not yet supported.");
            }
        }
        #endregion
    }
}
