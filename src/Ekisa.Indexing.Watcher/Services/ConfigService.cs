using Ekisa.Indexing.Watcher.Constants;
using Ekisa.Indexing.Watcher.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ekisa.Indexing.Watcher.Services
{
    public class ConfigService
    {
        #region Private Attributes
        private readonly string _configFilePath;
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

        #region Constructor
        public ConfigService(string configFilePath)
        {
            _configFilePath = configFilePath;
        }
        #endregion

        #region Public Methods
        public async Task<ConfigModel?> ReadConfigFile()
        {
            ConfigModel? config = null;

            try
            {
                config = JsonConvert.DeserializeObject<ConfigModel>(await File.ReadAllTextAsync(_configFilePath));
                CheckConfigConstraints(config!);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            return config;
        }
        #endregion

        #region Private Methods
        private void CheckConfigConstraints(ConfigModel config)
        {
            // Validates if provided folder is supported
            if (!_locations.ContainsKey(config.IndexingFolder ?? ""))
            {
                throw new Exception("The provided folder is not yet supported.");
            }

            // Validates if provided HTTP method is supported
            List<string> availableHttpMethods = new() { "POST", "PUT", "PATCH", "DELETE" };
            if (!availableHttpMethods.Contains(config.WebhookHttpMethod ?? ""))
            {
                throw new Exception("The provided HTTP method is invalid.");
            }

            // Validates if provided headers have a valid structure
            Type? headersType = config?.WebhookRequestHeaders?.GetType();
            if (headersType != null && headersType != typeof(JObject))
            {
                throw new Exception("The provided Header structure is invalid. It must be JSON format.");
            }  

            // Validates if provided body have a valid structure
            Type? bodyType = config?.WebhookRequestBody?.GetType();
            if (bodyType != null && bodyType != typeof(JObject))
            {
                throw new Exception("The provided Body structure is invalid. It must be JSON format.");
            }          
        }
        #endregion
    }
}
